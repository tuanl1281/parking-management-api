using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Site;

[Table("Sites")]
public class Site: BaseEntity
{
    public Site(string name, string address)
    {
        Name = name;
        Address = address;
    }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public virtual ICollection<Data.Entities.Camera.Camera> Cameras { get; set; }
}