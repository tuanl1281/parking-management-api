using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Parking.Management.ViewModel.Common.Attribute;

namespace Parking.Management.ViewModel.Vehicle.Response;

public class VehicleIdentifyResponseModel
{
    [JsonProperty("time")]
    [ObjectProperty("time")]
    public DateTime Time { get; set; }
    
    [JsonProperty("licensePlates")]
    [ObjectProperty("licensePlates")]
    public List<VehicleIdentifyLicensePlateRequestModel> LicensePlates { get; set; }
}

public class VehicleIdentifyLicensePlateRequestModel
{
    [JsonProperty("coordinate")]
    [ObjectProperty("coordinate")]
    public VehicleIdentifyLicensePlateCoordinateResponseModel Coordinate { get; set; }
    
    [JsonProperty("number")]
    [ObjectProperty("number")]
    public string Number { get; set; }
}

public class VehicleIdentifyLicensePlateCoordinateResponseModel
{
    [JsonProperty("minimumVertical")]
    [JsonPropertyName("minimumVertical")]
    [ObjectProperty("minimumVertical")]
    public double MinimumVertical { get; set; }
    
    [JsonProperty("maximumVertical")]
    [JsonPropertyName("maximumVertical")]
    [ObjectProperty("maximumVertical")]
    public double MaximumVertical { get; set; }
    
    [JsonProperty("minimumHorizontal")]
    [JsonPropertyName("minimumHorizontal")]
    [ObjectProperty("minimumHorizontal")]
    public double MinimumHorizontal { get; set; }
    
    [JsonProperty("maximumHorizontal")]
    [JsonPropertyName("maximumHorizontal")]
    [ObjectProperty("maximumHorizontal")]
    public double MaximumHorizontal { get; set; }
}