using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Customer;

[Table("Customers")]
public class Customer: BaseEntity
{
    public Customer(string firstName, string lastName, string phoneNumber, string address): base()
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Address = address;
    }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public virtual Entities.Wallet.Wallet Wallet { get; set; }
    
    public virtual ICollection<Entities.Vehicle.Vehicle> Vehicles { get; set; }
}