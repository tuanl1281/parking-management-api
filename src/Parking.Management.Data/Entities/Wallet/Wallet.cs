using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Wallet;

[Table("Wallets")]
public class Wallet: BaseEntity
{
    public Wallet(double balance, Guid customerId)
    {
        Balance = balance;
        CustomerId = customerId;
    }
    
    public double Balance { get; set; }
    
    public Guid CustomerId { get; set; }

    public virtual Entities.Customer.Customer Customer { get; set; }
    
    public virtual ICollection<Entities.Transaction.Transaction> Transactions { get; set; }
}