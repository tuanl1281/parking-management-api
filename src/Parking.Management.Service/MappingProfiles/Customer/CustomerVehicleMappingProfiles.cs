using AutoMapper;
using Parking.Management.ViewModel.Customer.Request;
using Parking.Management.ViewModel.Customer.Response;

namespace Parking.Management.Service.MappingProfiles.Customer;

public class CustomerVehicleMappingProfiles: Profile
{
    public CustomerVehicleMappingProfiles()
    {
        CreateMap<CustomerVehicleAddRequestModel, Parking.Management.Data.Entities.Customer.CustomerVehicle>();

        CreateMap<Parking.Management.Data.Entities.Customer.CustomerVehicle, CustomerVehicleResponseModel>();
    }
}