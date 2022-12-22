using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Vehicle;

public class VehicleConfigurations: BaseConfiguration<Entities.Vehicle.Vehicle>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Vehicle.Vehicle> builder)
    {
        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Vehicles)
            .HasForeignKey(_ => _.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}