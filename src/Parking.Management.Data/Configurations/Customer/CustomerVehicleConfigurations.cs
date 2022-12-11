using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Customer;

public class CustomerVehicleConfigurations: BaseConfiguration<Entities.Customer.CustomerVehicle>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Customer.CustomerVehicle> builder)
    {
        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Vehicles)
            .HasForeignKey(_ => _.CustomerId);
    }
}