using AutoMapper;
using Parking.Management.ViewModel.Transaction.Response;

namespace Parking.Management.Service.MappingProfiles.Transaction;

public class TransactionMappingProfiles: Profile
{
    public TransactionMappingProfiles()
    {
        CreateMap<Data.Entities.Transaction.Transaction, TransactionResponseModel>();
    }
}