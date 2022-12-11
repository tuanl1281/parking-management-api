using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Role;

public class RoleConfigurations: BaseConfiguration<Entities.Role.Role>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Role.Role> builder)
    {
        builder.HasMany(_ => _.UserRoles)
            .WithOne(_ => _.Role)
            .HasForeignKey(_ => _.RoleId);
    }
}