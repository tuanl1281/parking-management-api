using AutoMapper;
using Parking.Management.ViewModel.Vehicle.Response;

namespace Parking.Management.Service.MappingProfiles.Vehicle;

public class VehicleLogMappingProfiles: Profile
{
    public VehicleLogMappingProfiles()
    {
        CreateMap<Data.Entities.Vehicle.VehicleLog, VehicleLogResponseModel>();
    }
}