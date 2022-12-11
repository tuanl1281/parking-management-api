using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Role;

public class RolePermissionConfigurations: BaseConfiguration<Entities.Role.RolePermission>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Role.RolePermission> builder)
    {
    }
}