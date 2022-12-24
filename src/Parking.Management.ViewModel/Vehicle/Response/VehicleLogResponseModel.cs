using Parking.Management.ViewModel.Customer.Response;

namespace Parking.Management.ViewModel.Vehicle.Response;

public class VehicleLogResponseModel
{
    public Guid Id { get; set; }
    
    public DateTime Time { get; set; }
    
    public string Coordinate { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public string ImageRecognition { get; set; }
    
    public VehicleResponseModel? Vehicle { get; set; }
    
    public CustomerResponseModel? Customer { get; set; }
}