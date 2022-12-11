using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Customer;

public class CustomerConfigurations: BaseConfiguration<Entities.Customer.Customer>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Customer.Customer> builder)
    {
    }
}