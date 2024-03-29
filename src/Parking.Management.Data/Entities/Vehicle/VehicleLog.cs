using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Vehicle;

[Table("VehicleLogs")]
public class VehicleLog: BaseEntity
{
    public VehicleLog(DateTime time, string coordinate, string licenseNumber): base()
    {
        Time = time;
        Coordinate = coordinate;
        LicenseNumber = licenseNumber;
    }

    public VehicleLog(DateTime time, string coordinate, string licenseNumber, string imageRecognition): base()
    {
        Time = time;
        Coordinate = coordinate;
        LicenseNumber = licenseNumber;
        ImageRecognition = imageRecognition;
    }
    
    public VehicleLog(DateTime time, string coordinate, string licenseNumber, string imageRecognition, Guid? vehicleId): base()
    {
        Time = time;
        Coordinate = coordinate;
        LicenseNumber = licenseNumber;
        ImageRecognition = imageRecognition;
        VehicleId = vehicleId;
    }
    
    public VehicleLog(DateTime time, string coordinate, string licenseNumber, string imageRecognition, Guid? vehicleId, Guid? cameraId): base()
    {
        Time = time;
        Coordinate = coordinate;
        LicenseNumber = licenseNumber;
        ImageRecognition = imageRecognition;
        VehicleId = vehicleId;
        CameraId = cameraId;
    }
    
    public VehicleLog(DateTime time, string coordinate, string licenseNumber, string imageRecognition, Guid? vehicleId, Guid? cameraId, Guid? siteId): base()
    {
        Time = time;
        Coordinate = coordinate;
        LicenseNumber = licenseNumber;
        ImageRecognition = imageRecognition;
        VehicleId = vehicleId;
        CameraId = cameraId;
        SiteId = siteId;
    }
    
    public DateTime Time { get; set; }
    
    public string Coordinate { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public string ImageRecognition { get; set; }
    
    public Guid? VehicleId { get; set; }
    
    public virtual Data.Entities.Vehicle.Vehicle Vehicle { get; set; }    
    
    public Guid? CameraId { get; set; }
    
    public virtual Data.Entities.Camera.Camera Camera { get; set; }    
    
    public Guid? SiteId { get; set; }
    
    public virtual Data.Entities.Site.Site Site { get; set; }
}