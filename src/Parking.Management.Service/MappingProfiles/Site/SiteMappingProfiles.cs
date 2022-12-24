using AutoMapper;
using Parking.Management.ViewModel.Site.Request;
using Parking.Management.ViewModel.Site.Response;

namespace Parking.Management.Service.MappingProfiles.Site;

public class SiteMappingProfiles: Profile
{
    public SiteMappingProfiles()
    {
        CreateMap<SiteAddRequestModel, Data.Entities.Site.Site>();
        CreateMap<SiteUpdateRequestModel, Data.Entities.Site.Site>();

        CreateMap<Data.Entities.Site.Site, SiteResponseModel>();
    }
}