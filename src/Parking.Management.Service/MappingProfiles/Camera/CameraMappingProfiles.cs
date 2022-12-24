using AutoMapper;
using Parking.Management.ViewModel.Camera.Request;
using Parking.Management.ViewModel.Camera.Response;

namespace Parking.Management.Service.MappingProfiles.Camera;

public class CameraMappingProfiles: Profile
{
    public CameraMappingProfiles()
    {
        CreateMap<CameraAddRequestModel, Data.Entities.Camera.Camera>();
        CreateMap<CameraUpdateRequestModel, Data.Entities.Camera.Camera>();

        CreateMap<Data.Entities.Camera.Camera, CameraResponseModel>();
    }
}