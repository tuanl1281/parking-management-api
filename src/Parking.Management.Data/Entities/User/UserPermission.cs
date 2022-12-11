using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.User;

[Table("UserPermissions")]
public class UserPermission: BaseEntity
{
    public UserPermission(Guid userId, Guid permissionId)
    {
        UserId = userId;
        PermissionId = permissionId;
    }
    
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
    
    public Guid PermissionId { get; set; }
    
    public virtual Permission.Permission Permission { get; set; }
}