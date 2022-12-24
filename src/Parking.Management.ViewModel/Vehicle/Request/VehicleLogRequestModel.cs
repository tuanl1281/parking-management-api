using Microsoft.AspNetCore.Mvc;
using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Vehicle.Request;

#region --- Utilities ---
public class VehicleLogFilterRequestModel: CommonFilterRequest
{
    [FromQuery(Name = "hasRegistered")] 
    public bool? HasRegistered { get; set; } = null;
}

public class VehicleLogForCustomerFilterRequestModel: DateTimeFilterRequest
{
}
#endregion