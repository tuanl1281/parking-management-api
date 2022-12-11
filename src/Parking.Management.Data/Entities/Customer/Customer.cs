using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Customer;

[Table("Customers")]
public class Customer: BaseEntity
{
    public Customer(string firstName, string lastName, string address)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Address { get; set; }
    
    public virtual Entities.Wallet.Wallet Wallet { get; set; }
    
    public virtual ICollection<Entities.Customer.CustomerVehicle> Vehicles { get; set; }
}