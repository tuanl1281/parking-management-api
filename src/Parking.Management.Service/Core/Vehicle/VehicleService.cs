using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Vehicle.Request;
using Parking.Management.ViewModel.Vehicle.Response;

namespace Parking.Management.Service.Core.Vehicle;

public interface IVehicleService: IBaseService<SqlDbContext, Data.Entities.Vehicle.Vehicle, VehicleFilterRequestModel, VehicleResponseModel, VehicleAddRequestModel, VehicleUpdateRequestModel>
{
    Task AddCustomer(Guid customerId, Guid id);
    
    Task RemoveCustomer(Guid id);

    Task<List<VehicleLogResponseModel>> GetLog(VehicleLogFilterRequestModel filter, Guid id);
    
    #region --- Utilities ---
    Task<IQueryable<Data.Entities.Vehicle.Vehicle>> GenerateVehicleQuery(VehicleFilterRequestModel filter, Guid userId);
    #endregion
}

public class VehicleService: BaseService<SqlDbContext, Data.Entities.Vehicle.Vehicle, VehicleFilterRequestModel, VehicleResponseModel, VehicleAddRequestModel, VehicleUpdateRequestModel>, IVehicleService
{
    public VehicleService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }
    
    public async Task AddCustomer(Guid customerId, Guid id)
    {
        #region --- Validate ---
        var vehicle = await _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().GetAsync(_ => _.Id == id);
        if (vehicle == null)
            throw new ServiceException(null, "Vehicle isn't existed");
        
        var customer = await _unitOfWork.Repository<Data.Entities.Customer.Customer>().GetAsync(_ => _.Id == customerId);
        if (customer == null)
            throw new ServiceException(null, "Customer isn't existed");
        #endregion
        
        /* Do it */
        vehicle.CustomerId = customer.Id;
        /* Update */
        _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().Update(vehicle);
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task RemoveCustomer(Guid id)
    {
        #region --- Validate ---
        var vehicle = await _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().GetAsync(_ => _.Id == id);
        if (vehicle == null)
            throw new ServiceException(null, "Vehicle isn't existed");
        #endregion
        
        /* Do it */
        vehicle.CustomerId = null;
        /* Update */
        _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().Update(vehicle);
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<VehicleLogResponseModel>> GetLog(VehicleLogFilterRequestModel filter, Guid id)
    {
        /* Query */
        var fromDate = filter.FromDate.ToLocalDateTime();
        var toDate = filter.ToDate.ToLocalDateTime();
        
        var query = _unitOfWork.DbContext.VehicleLogs
            .Where(_ => _.Id == id)
            .Where(_ => _.Time >= fromDate)
            .Where(_ => _.Time <= toDate.EndOfDay());
        var logs = await query.ToListAsync();
        /* Builder */
        var logModels = _mapper.Map<List<Data.Entities.Vehicle.VehicleLog>, List<VehicleLogResponseModel>>(logs);
        /* Return */
        return logModels;
    }
    
    public override async Task<PagingResponseModel<VehicleResponseModel>> GetPagedResult(VehicleFilterRequestModel filter, Guid userId)
    {
        /* Query */
        var query = await GenerateVehicleQuery(filter, userId);
        var vehicles = await query
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        /* Builder */
        var result = new PagingResponseModel<VehicleResponseModel>();
        result.Data = _mapper.Map<List<Data.Entities.Vehicle.Vehicle>, List<VehicleResponseModel>>(vehicles);
        result.TotalCounts = await query.CountAsync();
        /* Return */
        return result;
    }

    #region --- Utilities ---
    public async Task<IQueryable<Data.Entities.Vehicle.Vehicle>> GenerateVehicleQuery(VehicleFilterRequestModel filter, Guid userId)
    {
        return _unitOfWork.DbContext.Vehicles
            .Where(_ => string.IsNullOrEmpty(filter.Keyword) || _.Name.ToLower() == filter.Keyword.ToLower() || _.Brand.ToLower() == filter.Keyword.ToLower() || _.LicenseNumber.ToLower() == filter.Keyword.ToLower())
            .Where(_ => string.IsNullOrEmpty(filter.Name) || _.Name.ToLower() == filter.Name.ToLower())
            .Where(_ => string.IsNullOrEmpty(filter.Brand) || _.Brand.ToLower() == filter.Brand.ToLower())
            .Where(_ => string.IsNullOrEmpty(filter.LicenseNumber) || _.LicenseNumber.ToLower() == filter.LicenseNumber.ToLower())
            .Where(_ => !filter.Type.HasValue || _.Type == filter.Type.Value);
    }
    #endregion
}