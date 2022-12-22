using Parking.Management.ViewModel.Common.Enum;

namespace Parking.Management.ViewModel.Vehicle.Response;

public class VehicleResponseModel
{
    public Guid Id { get; set; }
    
    public VehicleTypes Type { get; set; }
    
    public string Name { get; set; }
    
    public string Brand { get; set; }
    
    public string LicenseNumber { get; set; }
}