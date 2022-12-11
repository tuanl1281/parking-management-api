using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.User;

public class UserConfigurations: BaseConfiguration<Entities.User.User>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.User.User> builder)
    {
    }
}