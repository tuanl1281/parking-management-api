using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Vehicle;

[Table("VehicleLogs")]
public class VehicleLog: BaseEntity
{
    public VehicleLog(DateTime time): base()
    {
        Time = time;
    }

    public VehicleLog(DateTime time, string imageRecognition): base()
    {
        Time = time;
        ImageRecognition = imageRecognition;
    }
    
    public DateTime Time { get; set; }
    
    public string ImageRecognition { get; set; }
    
    public Guid VehicleId { get; set; }
    
    public virtual Data.Entities.Vehicle.Vehicle Vehicle { get; set; }
}