using AutoMapper;
using Parking.Management.ViewModel.Customer.Request;
using Parking.Management.ViewModel.Customer.Response;

namespace Parking.Management.Service.MappingProfiles.Customer;

public class CustomerMappingProfiles: Profile
{
    public CustomerMappingProfiles()
    {
        CreateMap<CustomerAddRequestModel, Parking.Management.Data.Entities.Customer.Customer>();
        CreateMap<CustomerUpdateRequestModel, Parking.Management.Data.Entities.Customer.Customer>();
        
        CreateMap<Parking.Management.Data.Entities.Customer.Customer, CustomerResponseModel>()
            .ForMember(dest => dest.Balance, src => src.MapFrom(_ => _.Wallet.Balance));
    }
}