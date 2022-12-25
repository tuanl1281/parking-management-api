using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Camera.Request;

public class CameraAddRequestModel
{
    public string Name { get; set; }
    
    public Guid? SiteId { get; set; }
}

public class CameraUpdateRequestModel
{
    public string Name { get; set; }
    
    public Guid SiteId { get; set; }
}

#region --- Utilities ---
public class CameraFilterRequestModel : PagingFilterRequest
{
}
#endregion