using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.User;

public class UserPermissionConfigurations: BaseConfiguration<Entities.User.UserPermission>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.User.UserPermission> builder)
    {
    }
}