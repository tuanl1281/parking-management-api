using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Vehicle;

public class VehicleLogConfigurations: BaseConfiguration<Entities.Vehicle.VehicleLog>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Vehicle.VehicleLog> builder)
    {
        builder.HasOne(_ => _.Vehicle)
            .WithMany(_ => _.Logs)
            .HasForeignKey(_ => _.VehicleId);
    }
}