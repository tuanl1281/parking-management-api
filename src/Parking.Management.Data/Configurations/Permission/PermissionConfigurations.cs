using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Permission;

public class PermissionConfigurations: BaseConfiguration<Entities.Permission.Permission>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Permission.Permission> builder)
    {
        builder.HasMany(_ => _.UserPermissions)
            .WithOne(_ => _.Permission)
            .HasForeignKey(_ => _.PermissionId);
        
        builder.HasMany(_ => _.RolePermissions)
            .WithOne(_ => _.Permission)
            .HasForeignKey(_ => _.PermissionId);
    }
}