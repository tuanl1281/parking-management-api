using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Transaction;

[Table("Transactions")]
public class Transaction: BaseEntity
{
    public Transaction(DateTime time, double amount, double newestBalance, Guid walletId)
    {
        Time = time;
        Amount = amount;
        NewestBalance = newestBalance;
        WalletId = walletId;
    }
    
    public DateTime Time { get; set; }
    
    public double Amount { get; set; }
    
    public double NewestBalance { get; set; }
    
    public Guid TransactionTypeId { get; set; }
    
    public virtual Entities.Transaction.TransactionType Type { get; set; }
    
    public Guid WalletId { get; set; }
    
    public virtual Entities.Wallet.Wallet Wallet { get; set; }
}