using Microsoft.AspNetCore.Mvc;
using Parking.Management.ViewModel.Common.Enum;
using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Vehicle.Request;

public class VehicleAddRequestModel
{
    public VehicleTypes Type { get; set; }
    
    public string Name { get; set; }
    
    public string Brand { get; set; }
    
    public string LicenseNumber { get; set; }
    
    public Guid? CustomerId { get; set; }
}

public class VehicleUpdateRequestModel: VehicleAddRequestModel
{
}

#region --- Utilities ---
public class VehicleFilterRequestModel: PagingFilterRequest
{
    [FromQuery(Name = "keyword")]
    public string Keyword { get; set; }
    
    [FromQuery(Name = "name")]
    public string Name { get; set; }
    
    [FromQuery(Name = "brand")]
    public string Brand { get; set; }

    [FromQuery(Name = "licenseNumber")]
    public string LicenseNumber { get; set; }
    
    [FromQuery(Name = "type")]
    public VehicleTypes? Type { get; set; }
}
#endregion