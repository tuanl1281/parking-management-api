using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Parking.Management.Application.Helpers.Database;

public static class DatabaseHelpers
{
    public static void SqlMigrate<T>(this IApplicationBuilder application) where T : DbContext
    {
        using var scope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<T>();
        if (context == null)
            return;

        context.Database.EnsureCreated();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }

    public static void SqlSeed<T>(this IApplicationBuilder application) where T : DbContext
    {
        using var scope = application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<T>();

        context?.AuthenticationSeeder();
        context?.SaveChanges();
    }
    
    #region --- Authentication ---
    private static T AuthenticationSeeder<T>(this T context) where T : DbContext => context;
    #endregion
}