using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Transaction;

public class TransactionConfigurations: BaseConfiguration<Entities.Transaction.Transaction>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Transaction.Transaction> builder)
    {
        builder.HasOne(_ => _.Wallet)
            .WithMany(_ => _.Transactions)
            .HasForeignKey(_ => _.WalletId);
    }
}