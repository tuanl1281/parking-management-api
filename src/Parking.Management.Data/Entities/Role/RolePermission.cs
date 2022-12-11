using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Role;

[Table("RolePermissions")]
public class RolePermission: BaseEntity
{
    public RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
    
    public Guid RoleId { get; set; }
    
    public virtual Role Role { get; set; }
    
    public Guid PermissionId { get; set; }
    
    public virtual Permission.Permission Permission { get; set; }
}