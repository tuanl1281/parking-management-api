using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Customer;

[Table("CustomerVehicle")]
public class CustomerVehicle: BaseEntity
{
    public CustomerVehicle(string name, string brand, string licenceNumber)
    {
        Name = name;
        Brand = brand;
        LicenceNumber = licenceNumber;
    }
    
    public string Name { get; set; }
    
    public string Brand { get; set; }
    
    public string LicenceNumber { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public virtual Entities.Customer.Customer Customer { get; set; }
}