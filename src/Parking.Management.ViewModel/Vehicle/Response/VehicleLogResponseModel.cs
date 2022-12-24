using Parking.Management.ViewModel.Camera.Response;
using Parking.Management.ViewModel.Customer.Response;
using Parking.Management.ViewModel.Site.Response;

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
    
    public CameraResponseModel? Camera { get; set; }
    
    public SiteResponseModel? Site { get; set; }
}