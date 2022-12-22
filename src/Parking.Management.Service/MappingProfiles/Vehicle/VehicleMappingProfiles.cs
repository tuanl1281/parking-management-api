using AutoMapper;
using Parking.Management.ViewModel.Vehicle.Request;
using Parking.Management.ViewModel.Vehicle.Response;

namespace Parking.Management.Service.MappingProfiles.Vehicle;

public class VehicleMappingProfiles: Profile
{
    public VehicleMappingProfiles()
    {
        CreateMap<VehicleAddRequestModel, Data.Entities.Vehicle.Vehicle>();
        CreateMap<VehicleUpdateRequestModel, Data.Entities.Vehicle.Vehicle>();

        CreateMap<Data.Entities.Vehicle.Vehicle, VehicleResponseModel>();
    }
}