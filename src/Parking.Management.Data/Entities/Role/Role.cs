using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;
using Parking.Management.Data.Entities.User;

namespace Parking.Management.Data.Entities.Role;

[Table("Roles")]
public class Role: BaseEntity
{
    public Role(string code)
    {
        Code = code;
    }
    
    public Role(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; set; }
    
    public string Description { get; set; }
    
    public virtual ICollection<UserRole> UserRoles { get; set; }
}