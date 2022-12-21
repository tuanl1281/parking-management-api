using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Role.Request;

public class RoleAddRequestModel
{
    public string Code { get; set; }
    
    public string Description { get; set; }
}

public class RoleUpdateRequestModel: RoleAddRequestModel
{
}

#region --- Utilities ---
public class RoleFilterRequestModel: PagingFilterRequest
{
}
#endregion
