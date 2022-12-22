using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;
using Parking.Management.ViewModel.Common.Enum;

namespace Parking.Management.Data.Entities.Transaction;

[Table("Transactions")]
public class Transaction: BaseEntity
{
    public Transaction(TransactionTypes type, DateTime time, double amount, double newestBalance, string description, Guid walletId)
    {
        Time = time;
        Type = type;
        Amount = amount;
        NewestBalance = newestBalance;
        Description = description;
        WalletId = walletId;
    }
    
    public TransactionTypes Type { get; set; }
    
    public DateTime Time { get; set; }
    
    public double Amount { get; set; }
    
    public double NewestBalance { get; set; }
    
    public string? Description { get; set; }
    
    public Guid WalletId { get; set; }
    
    public virtual Entities.Wallet.Wallet Wallet { get; set; }
}