using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.User;

[Table("UserRoles")]
public class UserRole: BaseEntity
{
    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
    
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
    
    public Guid RoleId { get; set; }
    
    public virtual Role.Role Role { get; set; }
}