using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Parking.Management.Data.Infrastructures;

using Microsoft.EntityFrameworkCore;

public interface IDbFactory<out TContext> where TContext : DbContext
{
    TContext Init();
}

public class DbFactory<TContext>: Disposable, IDbFactory<TContext> where TContext: DbContext, new()
{
    private TContext? _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DbFactory(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public TContext Init() => _dbContext ??= (TContext) Activator.CreateInstance(typeof(TContext), _configuration, _httpContextAccessor);
    
    protected override void DisposeCore()
    {
        _dbContext?.Dispose();
    }
}