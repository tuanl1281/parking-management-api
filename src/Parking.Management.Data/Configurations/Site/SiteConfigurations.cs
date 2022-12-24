using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Site;

public class SiteConfigurations: BaseConfiguration<Entities.Site.Site>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Site.Site> builder)
    {
    }
}