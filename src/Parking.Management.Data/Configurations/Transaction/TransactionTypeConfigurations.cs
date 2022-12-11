using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Configurations.Common;

namespace Parking.Management.Data.Configurations.Transaction;

public class TransactionTypeConfigurations: BaseConfiguration<Entities.Transaction.TransactionType>
{
    protected override void ConfigureMoreProperties(EntityTypeBuilder<Entities.Transaction.TransactionType> builder)
    {
        builder.HasMany(_ => _.Transactions)
            .WithOne(_ => _.Type)
            .HasForeignKey(_ => _.TransactionTypeId);
    }
}