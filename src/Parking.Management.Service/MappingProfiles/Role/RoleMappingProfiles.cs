using AutoMapper;
using Parking.Management.ViewModel.Role.Request;
using Parking.Management.ViewModel.Role.Response;

namespace Parking.Management.Service.MappingProfiles.Role;

public class RoleMappingProfiles: Profile
{
    public RoleMappingProfiles()
    {
        CreateMap<RoleAddRequestModel, Parking.Management.Data.Entities.Role.Role>();
        CreateMap<RoleUpdateRequestModel, Parking.Management.Data.Entities.Role.Role>();
        
        CreateMap<Parking.Management.Data.Entities.Role.Role, RoleResponseModel>();
    }
}