using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.User;

[Table("Users")]
public class User: BaseEntity
{
    public User(string fullName, string userName, string password)
    {
        FullName = fullName;
        UserName = userName;
        Password = password;
    }

    public string FullName { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
}