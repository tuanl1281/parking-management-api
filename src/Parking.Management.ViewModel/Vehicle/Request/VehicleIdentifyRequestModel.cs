using Microsoft.AspNetCore.Http;
using Parking.Management.ViewModel.Common.Attribute;

namespace Parking.Management.ViewModel.Vehicle.Request;

public class VehicleIdentifyRequestModel
{
    [ObjectProperty("file")]
    public IFormFile File { get; set; }
}