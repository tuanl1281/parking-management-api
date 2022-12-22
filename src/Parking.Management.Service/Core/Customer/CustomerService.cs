using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Customer.Request;
using Parking.Management.ViewModel.Customer.Response;
using Parking.Management.ViewModel.Vehicle.Request;
using Parking.Management.ViewModel.Vehicle.Response;

namespace Parking.Management.Service.Core.Customer;

public interface ICustomerService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.Customer.Customer, CustomerFilterRequestModel, CustomerResponseModel, CustomerAddRequestModel, CustomerUpdateRequestModel>
{
    Task AddVehicle(List<VehicleAddRequestModel> models, Guid id);

    Task RemoveVehicle(List<Guid> vehiclesIds, Guid id);
    
    Task<List<VehicleResponseModel>> GetVehicle(Guid id);
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

    public async Task AddVehicle(List<VehicleAddRequestModel> models, Guid id)
    {
        #region --- Validate ---
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == id);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
        
        /* Do it */
        foreach (var model in models)
        {
            var vehicle = _mapper.Map<VehicleAddRequestModel, Data.Entities.Vehicle.Vehicle>(model);
            vehicle.CustomerId = customer.Id;
            /* Add */
            _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().Add(vehicle);
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
        var vehicles = await _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().GetManyAsync(_ => _.CustomerId == customer.Id);
        foreach (var vehicle in vehicles)
        {
            vehicle.CustomerId = null;
            /* Update */
            _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().Update(vehicle);
        }
        /* Save */
        await _unitOfWork.DbContext.SaveChangesAsync();
    }
        
    public async Task<List<VehicleResponseModel>> GetVehicle(Guid id)
    {
        #region --- Validate ---
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == id);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
    
        /* Query */
        var vehicles = await _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().GetManyAsync(_ => _.CustomerId == customer.Id);
        /* Builder */
        var vehicleModels = _mapper.Map<List<Data.Entities.Vehicle.Vehicle>, List<VehicleResponseModel>>(vehicles.ToList());
        /* Return */
        return vehicleModels;
    }

}