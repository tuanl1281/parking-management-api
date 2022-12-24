using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parking.Management.Data.Extensions;
using Parking.Management.Data.Entities.Common;
using Parking.Management.Data.Entities.User;
using Parking.Management.Data.Entities.Role;
using Parking.Management.Data.Entities.Permission;
using Parking.Management.Data.Entities.Customer;
using Parking.Management.Data.Entities.Wallet;
using Parking.Management.Data.Entities.Transaction;
using Parking.Management.Data.Entities.Site;
using Parking.Management.Data.Entities.Vehicle;
using Parking.Management.Data.Entities.Camera;
using Parking.Management.Data.Configurations.User;
using Parking.Management.Data.Configurations.Role;
using Parking.Management.Data.Configurations.Permission;
using Parking.Management.Data.Configurations.Customer;
using Parking.Management.Data.Configurations.Site;
using Parking.Management.Data.Configurations.Wallet;
using Parking.Management.Data.Configurations.Transaction;
using Parking.Management.Data.Configurations.Vehicle;
using Parking.Management.Data.Configurations.Camera;

namespace Parking.Management.Data.Context;

public class SqlDbContext: DbContext
{
    private readonly IConfiguration _configuration;
    
    public SqlDbContext()
    {
    }
    
    public SqlDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlDbContext(DbContextOptions<SqlDbContext> options): base(options)
    {
    }
    
    #region --- User ---
    public DbSet<User> Users { get; set; }
    
    public DbSet<UserRole> UserRoles { get; set; }
    
    public DbSet<UserPermission> UserPermissions { get; set; }
    #endregion
    
    #region --- Role ---
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RolePermission> RolePermissions { get; set; }
    #endregion

    #region --- Permisison ---
    public DbSet<Permission> Permissions { get; set; }
    #endregion

    #region --- Customer ---
    public DbSet<Customer> Customers { get; set; }
    #endregion

    #region --- Wallet ---
    public DbSet<Wallet> Wallets { get; set; }
    #endregion
    
    #region --- Transaction ---
    public DbSet<Transaction> Transactions { get; set; }
    #endregion
    
    #region --- Vehicle ---
    public DbSet<Vehicle> Vehicles { get; set; }
    
    public DbSet<VehicleLog> VehicleLogs { get; set; }
    #endregion

    #region --- Site ---
    public DbSet<Site> Sites { get; set; }
    #endregion
    
    #region --- Camera --
    public DbSet<Camera> Cameras { get; set; }
    #endregion
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /* Config */
        optionsBuilder.UseLazyLoadingProxies();
        // optionsBuilder.UseMySql(_configuration["SqlDb:ConnectionString"], ServerVersion.AutoDetect(_configuration["SqlDb:ConnectionString"]));
        optionsBuilder.UseMySql("Server=141.147.155.1;Port=6603;Database=parking_management;Uid=mariadb;Pwd=deV0ps@#!", ServerVersion.AutoDetect("Server=141.147.155.1;Port=6603;Database=parking_management;Uid=mariadb;Pwd=deV0ps@#!"));
        /* Base */
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region --- User ---
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new UserRoleConfigurations());
        modelBuilder.ApplyConfiguration(new UserPermissionConfigurations());
        #endregion
        
        #region --- Role ---
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
        modelBuilder.ApplyConfiguration(new RolePermissionConfigurations());
        #endregion
        
        #region --- Permission ---
        modelBuilder.ApplyConfiguration(new PermissionConfigurations());
        #endregion
        
        #region --- Customer ---
        modelBuilder.ApplyConfiguration(new CustomerConfigurations());
        #endregion
        
        #region --- Wallet ---
        modelBuilder.ApplyConfiguration(new WalletConfigurations());
        #endregion

        #region --- Transaction ---
        modelBuilder.ApplyConfiguration(new TransactionConfigurations());
        #endregion
        
        #region --- Vehicle ---
        modelBuilder.ApplyConfiguration(new VehicleConfigurations());
        modelBuilder.ApplyConfiguration(new VehicleLogConfigurations());
        #endregion

        #region --- Site ---
        modelBuilder.ApplyConfiguration(new SiteConfigurations());
        #endregion
        
        #region --- Camera ---
        modelBuilder.ApplyConfiguration(new CameraConfigurations());
        #endregion
        
        /* Apply global filter */
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SoftDeletedBaseEntity).IsAssignableFrom(entityType.ClrType))
                modelBuilder.Entity(entityType.ClrType).AddFilter<SoftDeletedBaseEntity>(_ => !_.IsDeleted);
        }
        /* Base */
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        /* Insert */
        var insertedEntries = ChangeTracker.Entries()
            .Where(_ => _.State == EntityState.Added)
            .Select(_ => _.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            /* Date Created */
            if (insertedEntry is BaseEntity)
                ((BaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) insertedEntry).DateCreated = DateTime.Now;
        }
        
        /* Update */
        var modifiedEntries = ChangeTracker.Entries()
            .Where(_ => _.State == EntityState.Modified)
            .Select(_ => _.Entity);
        
        foreach (var modifiedEntry in modifiedEntries)
        {
            /* Date Updated */
            if (modifiedEntry is BaseEntity)
                ((BaseEntity) modifiedEntry).DateUpdated = DateTime.Now;
            
            if (modifiedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) modifiedEntry).DateUpdated = DateTime.Now;
        }

        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        /* Insert */
        var insertedEntries = ChangeTracker.Entries()
            .Where(_ => _.State == EntityState.Added)
            .Select(_ => _.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            /* Date Created */
            if (insertedEntry is BaseEntity)
                ((BaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) insertedEntry).DateCreated = DateTime.Now;
        }
        
        /* Update */
        var modifiedEntries = ChangeTracker.Entries()
            .Where(_ => _.State == EntityState.Modified)
            .Select(_ => _.Entity);
        
        foreach (var modifiedEntry in modifiedEntries)
        {
            /* Date Created */
            if (modifiedEntry is BaseEntity)
                ((BaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            
            if (modifiedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) modifiedEntry).DateCreated = DateTime.Now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
