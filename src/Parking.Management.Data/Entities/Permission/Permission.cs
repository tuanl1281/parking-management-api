using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;
using Parking.Management.Data.Entities.Role;
using Parking.Management.Data.Entities.User;

namespace Parking.Management.Data.Entities.Permission;

[Table("Permissions")]
public class Permission: BaseEntity
{
    public Permission(string code)
    {
        Code = code;
    }
    
    public Permission(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; set; }
    
    public string Description { get; set; }
    
    public virtual ICollection<UserPermission> UserPermissions { get; set; }
    
    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}