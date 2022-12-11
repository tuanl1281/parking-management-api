using System.Collections;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Data.Repositories.Common;

namespace Parking.Management.Data.Infrastructures;

public interface IUnitOfWork<TContext>: IDisposable where TContext : DbContext
{
    TContext DbContext { get; }

    IBaseRepository<TEntity, TContext> Repository<TEntity>() where TEntity : class;
    
    int SaveChanges();

    Task<int> SaveChangesAsync();
}

public class UnitOfWork<TContext>: Disposable, IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly IDbFactory<TContext> _dbFactory;
    private readonly TContext _dbContext;
    
    private readonly Hashtable _repositories;
    
    public UnitOfWork(IDbFactory<TContext> dbFactory)
    {
        _dbFactory = dbFactory;
        _dbContext = _dbContext ?? _dbFactory.Init();
        _repositories = new Hashtable();
    }

    public TContext DbContext => _dbContext;

    public IBaseRepository<TEntity, TContext> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;
        /* Get instance */
        if (_repositories.ContainsKey(type))
            return (IBaseRepository<TEntity, TContext>) _repositories[type];
        /* Add instance */
        var repositoryType = typeof(BaseRepository<,>);
        var repositoryInstance =
            Activator.CreateInstance(repositoryType.MakeGenericType(new [] { typeof (TEntity), typeof(TContext) }), _dbFactory);

        _repositories.Add(type, repositoryInstance);
        /* Return */
        return (IBaseRepository<TEntity, TContext>) _repositories[type];
    }
    
    public int SaveChanges()
    {
        var result = DbContext.SaveChanges();
        return result;
    }

    public async Task<int> SaveChangesAsync()
    {
        var result = await DbContext.SaveChangesAsync();
        return result;
    }
}