using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Wallet;

public class WalletConfigurations: BaseConfiguration<Entities.Wallet.Wallet>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Wallet.Wallet> builder)
    {
    }
}