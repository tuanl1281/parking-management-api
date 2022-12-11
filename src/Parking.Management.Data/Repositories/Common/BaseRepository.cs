using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Repositories.Common;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrastructures;

public interface IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    IDbFactory<TContext> DbFactory { get; }

    TContext DbContext { get; }
    
    #region Sync Methods
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns></returns>
    IEnumerable<TEntity> GetAll(bool allowTracking = true);

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    TEntity? GetById(object id, bool allowTracking = true);

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    TEntity? Get(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity"></param>
    void Add(TEntity entity);

    /// <summary>
    /// Update an entity
    /// </summary>
    /// <param name="entity"></param>
    void Update(TEntity entity);

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity"></param>
    void Delete(TEntity entity);

    /// <summary>
    /// Delete an entity by id
    /// </summary>
    /// <param name="id"></param>
    void Delete(object id);
    
    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="where"></param>
    void Delete(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Delete the entities
    /// </summary>
    /// <param name="entities"></param>
    void DeleteRange(IEnumerable<TEntity> entities);
    #endregion

    #region Async Methods

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get entity by id async
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(object id, bool allowTracking = true);

    /// <summary>
    /// Get entities lambda expression async
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get all entities async
    /// </summary>
    /// <returns></returns>
    Task<List<TEntity>> GetAllAsync(bool allowTracking = true);

    /// <summary>
    /// Add new entity async
    /// </summary>
    /// <param name="entity"></param>
    void AddAsync(TEntity entity);

    /// <summary>
    /// Update an entity async
    /// </summary>
    /// <param name="entity"></param>
    void UpdateAsync(TEntity entity);

    /// <summary>
    /// Delete an entity by id async
    /// </summary>
    /// <param name="id"></param>
    void DeleteAsync(object id);

    /// <summary>
    /// Delete by expression async
    /// </summary>
    /// <param name="where"></param>
    void DeleteAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Delete the entities async
    /// </summary>
    /// <param name="entities"></param>
    void DeleteRangeAsync(IEnumerable<TEntity> entities);
    #endregion
}

public class BaseRepository<TEntity, TContext>: Disposable, IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    #region --- Properties ---
    private readonly IDbFactory<TContext> _dbFactory;
    private readonly TContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public IDbFactory<TContext> DbFactory => _dbFactory;
    public TContext DbContext => _dbContext;
    #endregion
    
    public BaseRepository(IDbFactory<TContext> dbFactory)
    {
        _dbFactory = dbFactory;
        _dbContext ??= _dbFactory.Init();
        _dbSet = _dbContext.Set<TEntity>();
    }

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual TEntity? GetById(object id, bool allowTracking = true)
    {
        return _dbSet.Find(id);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true)
    {
        return _dbSet.Where(predicate).AsEnumerable();
    }

    /// <summary>
    /// Get list of entities
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> GetAll(bool allowTracking = true)
    {
        return _dbSet.AsEnumerable();
    }

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    /// <summary>
    /// Update an entity
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Delete(TEntity entity)
    {
        var existing = _dbSet.Find(entity);
        if (existing == null)
            return;
        
        if (typeof(SoftDeletedBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            (existing as SoftDeletedBaseEntity)!.IsDeleted = true;
            _dbSet.Attach(existing);
            _dbContext.Entry(existing).State = EntityState.Modified;
        }
        else
            _dbSet.Remove(existing);
    }
    
    /// <summary>
    /// Delete an entity by id
    /// </summary>
    /// <param name="id"></param>
    public virtual void Delete(object id)
    {
        var existing = _dbSet.Find(id);
        if (existing == null)
            return;
        
        if (typeof(SoftDeletedBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            (existing as SoftDeletedBaseEntity)!.IsDeleted = true;
            _dbSet.Attach(existing);
            _dbContext.Entry(existing).State = EntityState.Modified;
        }
        else
            _dbSet.Remove(existing);
    }

    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="where"></param>
    public virtual void Delete(Expression<Func<TEntity, bool>> where)
    {
        IEnumerable<TEntity> entities = _dbSet.Where(where).AsEnumerable();
        foreach (TEntity entity in entities)
        {
            if (typeof(SoftDeletedBaseEntity).IsAssignableFrom(typeof(TEntity)))
            {
                (entity as SoftDeletedBaseEntity)!.IsDeleted = true;
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            else
                _dbSet.Remove(entity);
        }
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities"></param>
    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true)
    {
        if (allowTracking)
            return await _dbSet.FirstOrDefaultAsync(predicate);
        
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Get entity by id async
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<TEntity?> GetByIdAsync(object id, bool allowTracking = true)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool allowTracking = true)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Get all entities async
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> GetAllAsync(bool allowTracking = true)
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Add new entity async
    /// </summary>
    /// <param name="entity"></param>
    public virtual void AddAsync(TEntity entity)
    {
        _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Update an entity async
    /// </summary>
    /// <param name="entity"></param>
    public virtual void UpdateAsync(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbSet.Attach(entity);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="id"></param>
    public virtual void DeleteAsync(object id)
    {
        TEntity? existing = _dbSet.Find(id);
        if (existing != null)
            _dbSet.Remove(existing);
    }

    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="predicate"></param>
    public virtual void DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        IEnumerable<TEntity> entities = _dbSet.Where(predicate).AsEnumerable();
        foreach (TEntity entity in entities)
            _dbSet.Remove(entity);
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities"></param>
    public virtual void DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}