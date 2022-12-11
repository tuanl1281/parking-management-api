using AutoMapper;
using Parking.Management.ViewModel.User.Request;
using Parking.Management.ViewModel.User.Response;

namespace Parking.Management.Service.MappingProfiles.User;

public class UserMappingProfiles: Profile
{
    public UserMappingProfiles()
    {
        CreateMap<UserAddRequestModel, Parking.Management.Data.Entities.User.User>();
        CreateMap<UserUpdateRequestModel, Parking.Management.Data.Entities.User.User>();
        
        CreateMap<Parking.Management.Data.Entities.User.User, UserResponseModel>();
        CreateMap<Parking.Management.Data.Entities.User.User, UserDetailResponseModel>();
    }
}