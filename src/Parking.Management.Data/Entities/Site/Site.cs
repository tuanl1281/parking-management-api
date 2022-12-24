using System.ComponentModel.DataAnnotations.Schema;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Entities.Site;

[Table("Sites")]
public class Site: BaseEntity
{
    public Site(string name, string address, double fee)
    {
        Name = name;
        Address = address;
        Fee = fee;
    }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public double Fee { get; set; }
    
    public virtual ICollection<Data.Entities.Camera.Camera> Cameras { get; set; }
}