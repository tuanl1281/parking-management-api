using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Camera;

[Table("Cameras")]
public class Camera: BaseEntity
{
    public Camera(string name)
    {
        Name = name;
    }

    public Camera(string name, Guid siteId)
    {
        SiteId = siteId;
    }
    
    public string Name { get; set; }
    
    public Guid? SiteId { get; set; }
    
    public virtual Data.Entities.Site.Site Site { get; set; }
}