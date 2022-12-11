using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Customer.Request;
using Parking.Management.ViewModel.Customer.Response;

namespace Parking.Management.Service.Core.Customer;

public interface ICustomerService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.Customer.Customer, CustomerFilterRequestModel, CustomerResponseModel, CustomerAddRequestModel, CustomerUpdateRequestModel>
{
}

public class CustomerService: BaseService<SqlDbContext, Parking.Management.Data.Entities.Customer.Customer, CustomerFilterRequestModel, CustomerResponseModel, CustomerAddRequestModel, CustomerUpdateRequestModel>, ICustomerService
{
    public CustomerService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }
}