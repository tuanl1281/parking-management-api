using Parking.Management.ViewModel.Permission.Response;
using Parking.Management.ViewModel.Role.Response;

namespace Parking.Management.ViewModel.User.Response;

public class UserResponseModel
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; }
    
    public string UserName { get; set; }
}

public class UserDetailResponseModel
{
    public Guid Id { get; set; }

    public string Fullname { get; set; }
    
    public string UserName { get; set; }

    public RoleResponseModel? Role { get; set; }

    public List<PermissionResponseModel> Permissions { get; set; }
}

public class LoginResponseModel
{
    public string Token { get; set; }

    public string TokenType { get; set; }

    public long ExpiresIn { get; set; }

    public UserDetailResponseModel UserInfo { get; set; }
}