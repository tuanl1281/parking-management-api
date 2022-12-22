using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;
using Parking.Management.ViewModel.Common.Enum;

namespace Parking.Management.Data.Entities.Vehicle;

[Table("Vehicles")]
public class Vehicle: BaseEntity
{
    public Vehicle(VehicleTypes type, string name, string brand, string licenseNumber): base()
    {
        Type = type;
        Name = name;
        Brand = brand;
        LicenseNumber = licenseNumber;
    }
    
    public Vehicle(VehicleTypes type, string name, string brand, string licenseNumber, Guid customerId): base()
    {
        Type = type;
        Name = name;
        Brand = brand;
        LicenseNumber = licenseNumber;
        CustomerId = customerId;
    }
    
    public VehicleTypes Type { get; set; }
    
    public string Name { get; set; }
    
    public string Brand { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public Guid? CustomerId { get; set; }
    
    public virtual Data.Entities.Customer.Customer Customer { get; set; }
    
    public virtual ICollection<Data.Entities.Vehicle.VehicleLog> Logs { get; set; }
}