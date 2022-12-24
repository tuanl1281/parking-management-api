using Microsoft.AspNetCore.Mvc;
using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Site.Request;

public class SiteAddRequestModel
{
    public string Name { get; set; }
    
    public string Address { get; set; }
}

public class SiteUpdateRequestModel: SiteAddRequestModel
{
}

#region --- Utilities ---
public class SiteFilterRequestModel: PagingFilterRequest
{
    [FromQuery(Name = "keyword")]
    public string Keyword { get; set; }
    
    [FromQuery(Name = "name")]
    public string Name { get; set; }
}
#endregion