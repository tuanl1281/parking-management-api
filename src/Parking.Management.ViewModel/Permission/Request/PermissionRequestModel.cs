using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Permission.Request;

public class PermissionAddRequestModel
{
    public string Code { get; set; }
    
    public string Description { get; set; }
}

public class PermissionUpdateRequestModel: PermissionAddRequestModel
{
}

#region --- Utilities ---
public class PermissionFilterRequestModel: PagingFilterRequest
{
}
#endregion