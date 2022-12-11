using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Transaction;

[Table("TransactionTypes")]
public class TransactionType: BaseEntity
{
    public TransactionType(string code, string name, string description, double amount)
    {
        Code = code;
        Name = name;
        Description = description;
        Amount = amount;
    }
    
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public double Amount { get; set; }
    
    public virtual ICollection<Transaction> Transactions { get; set; }
}