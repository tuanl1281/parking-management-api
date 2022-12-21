using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Customer.Request;
using Parking.Management.ViewModel.Customer.Response;

namespace Parking.Management.Service.Core.Customer;

public interface ICustomerService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.Customer.Customer, CustomerFilterRequestModel, CustomerResponseModel, CustomerAddRequestModel, CustomerUpdateRequestModel>
{
    Task<List<CustomerVehicleResponseModel>> GetVehicle(Guid id);
    
    Task AddVehicle(List<CustomerVehicleAddRequestModel> vehicles, Guid id);

    Task RemoveVehicle(List<Guid> vehiclesIds, Guid id);
}

public class CustomerService: BaseService<SqlDbContext, Parking.Management.Data.Entities.Customer.Customer, CustomerFilterRequestModel, CustomerResponseModel, CustomerAddRequestModel, CustomerUpdateRequestModel>, ICustomerService
{
    public CustomerService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }


    public override async Task<object> Add(CustomerAddRequestModel model)
    {
        #region --- Validate ---
        if (string.IsNullOrEmpty(model.FirstName))
            throw new ServiceException(null, "First name is required");

        if (string.IsNullOrEmpty(model.LastName))
            throw new ServiceException(null, "Last name is required");

        if (string.IsNullOrEmpty(model.PhoneNumber))
            throw new ServiceException(null, "Phone number is required");

        if (string.IsNullOrEmpty(model.Address))
            throw new ServiceException(null, "Address is required");

        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.PhoneNumber == model.PhoneNumber);
        if (customer != null)
            throw new ServiceException(null, "Phone number is registered");
        #endregion

        /* Builder for customer */
        customer = _mapper.Map<CustomerAddRequestModel, Data.Entities.Customer.Customer>(model);
        /* Add customer */
        _unitOfWork.Repository<Data.Entities.Customer.Customer>().Add(customer);        
        /* Builder for wallet */
        var wallet = new Data.Entities.Wallet.Wallet(0, customer.Id);
        /* Add wallet */
        _unitOfWork.Repository<Data.Entities.Wallet.Wallet>().Add(wallet);
        /* Save */
        await _unitOfWork.SaveChangesAsync();
        /* Return */
        return customer.Id;
    }
    
    public async Task<List<CustomerVehicleResponseModel>> GetVehicle(Guid id)
    {
        #region --- Validate ---
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == id);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
    
        /* Query */
        var customerVehicles = await _unitOfWork.Repository<Data.Entities.Customer.CustomerVehicle>().GetManyAsync(_ => _.CustomerId == customer.Id);
        /* Builder */
        var customerVehicleModels = _mapper.Map<List<Data.Entities.Customer.CustomerVehicle>, List<CustomerVehicleResponseModel>>(customerVehicles.ToList());
        /* Return */
        return customerVehicleModels;
    }
    
    public async Task AddVehicle(List<CustomerVehicleAddRequestModel> vehicles, Guid id)
    {
        #region --- Validate ---
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == id);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
        
        /* Do it */
        foreach (var vehicle in vehicles)
        {
            var customerVehicle = _mapper.Map<CustomerVehicleAddRequestModel, Data.Entities.Customer.CustomerVehicle>(vehicle);
            customerVehicle.CustomerId = customer.Id;

            _unitOfWork.DbContext.CustomerVehicles.Add(customerVehicle);
        }
        /* Save */
        await _unitOfWork.DbContext.SaveChangesAsync();
    }

    public async Task RemoveVehicle(List<Guid> vehiclesIds, Guid id)
    {
        #region --- Validate ---
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == id);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
        
        /* Do it */
        var customerVehicles = await _unitOfWork.Repository<Data.Entities.Customer.CustomerVehicle>().GetManyAsync(_ => _.CustomerId == customer.Id);
        foreach (var customerVehicle in customerVehicles)
            _unitOfWork.DbContext.CustomerVehicles.Remove(customerVehicle);
        /* Save */
        await _unitOfWork.DbContext.SaveChangesAsync();
    }
}