using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.Management.ViewModel.Common.Attribute;

namespace Parking.Management.ViewModel.Vehicle.Request;

public class VehicleIdentifyRequestModel
{
    [FromForm(Name = "file")]
    [ObjectProperty("file")]
    public IFormFile File { get; set; }

    [FromForm(Name = "cameraId")]
    [ObjectProperty("cameraId")]
    public Guid? CameraId { get; set; }
    
    [FromForm(Name = "isLog")]
    [ObjectProperty("isLog")]
    public bool IsLog { get; set; } = true;
}