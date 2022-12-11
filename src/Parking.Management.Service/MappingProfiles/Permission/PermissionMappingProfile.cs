using AutoMapper;
using Parking.Management.ViewModel.Permission.Request;
using Parking.Management.ViewModel.Permission.Response;

namespace Parking.Management.Service.MappingProfiles.Permission;

public class PermissionMappingProfiles: Profile
{
    public PermissionMappingProfiles()
    {
        CreateMap<PermissionAddRequestModel, Parking.Management.Data.Entities.Permission.Permission>();
        CreateMap<PermissionUpdateRequestModel, Parking.Management.Data.Entities.Permission.Permission>();
        
        CreateMap<Parking.Management.Data.Entities.Permission.Permission, PermissionResponseModel>();
    }
}