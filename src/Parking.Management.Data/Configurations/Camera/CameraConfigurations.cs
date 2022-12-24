using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Camera;

public class CameraConfigurations: BaseConfiguration<Entities.Camera.Camera>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Camera.Camera> builder)
    {
        builder.HasOne(_ => _.Site)
            .WithMany(_ => _.Cameras)
            .HasForeignKey(_ => _.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}