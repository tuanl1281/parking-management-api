using Parking.Management.Data.Constants.Role;
using Parking.Management.ViewModel.User.Response;

namespace Parking.Management.Service.Utilities;

public static class AuthenticationUtilities
{
    private static bool IsRole(this UserDetailResponseModel? model, string roleCode)
        => model != null && model.Role != null && model.Role.Code == roleCode;
    
    public static bool IsAdmin(this UserDetailResponseModel? model)
        => IsRole(model, RoleConstants.Admin);
}