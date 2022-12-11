using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.User;

public class UserRoleConfigurations: BaseConfiguration<Entities.User.UserRole>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.User.UserRole> builder)
    {
    }
}