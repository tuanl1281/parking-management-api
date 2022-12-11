using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.User.Request;

public class UserAddRequestModel
{
    public string FullName { get; set; }
    
    public string UserName { get; set; }

    public string Password { get; set; }
}

public class UserUpdateRequestModel: UserAddRequestModel
{
}

public class LoginRequestModel
{
    [JsonProperty("username")]
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    
    [JsonProperty("password")]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}

public class ChangePasswordRequestModel
{
    [JsonProperty("oldPassword")]
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }
    
    [JsonProperty("newPassword")]
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}

#region --- Utilities ---
public class UserFilterRequestModel: PagingFilterRequest
{
}
#endregion
