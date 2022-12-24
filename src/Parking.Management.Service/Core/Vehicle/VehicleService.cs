using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parking.Management.Data.Context;
using Parking.Management.Data.Entities.Vehicle;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Enum;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Customer.Response;
using Parking.Management.ViewModel.Vehicle.Request;
using Parking.Management.ViewModel.Vehicle.Response;

namespace Parking.Management.Service.Core.Vehicle;

public interface IVehicleService: IBaseService<SqlDbContext, Data.Entities.Vehicle.Vehicle, VehicleFilterRequestModel, VehicleResponseModel, VehicleAddRequestModel, VehicleUpdateRequestModel>
{
    Task<List<string>> Identify(IFormFile file);
    
    Task AddCustomer(Guid customerId, Guid id);
    
    Task RemoveCustomer(Guid id);

    Task<List<VehicleLogResponseModel>> GetLog(VehicleLogFilterRequestModel filter, Guid id);
    #region --- Utilities ---
    Task<IQueryable<Data.Entities.Vehicle.Vehicle>> GenerateVehicleQuery(VehicleFilterRequestModel filter, Guid userId);
    #endregion
}

public class VehicleService: BaseService<SqlDbContext, Data.Entities.Vehicle.Vehicle, VehicleFilterRequestModel, VehicleResponseModel, VehicleAddRequestModel, VehicleUpdateRequestModel>, IVehicleService
{
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private readonly IHttpClientService _httpClientService;
    
    public VehicleService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork, IConfiguration configuration, IFileService fileService, IHttpClientService httpClientService) : base(mapper, unitOfWork)
    {
        _configuration = configuration;
        _fileService = fileService;
        _httpClientService = httpClientService;
    }

    public async Task<List<string>> Identify(IFormFile file)
    {
        if (string.IsNullOrEmpty(_configuration["Identify:Endpoint"]) || string.IsNullOrEmpty(_configuration["Identify:Token"]))
            throw new ServiceException(null, "Configuration is empty");
        /* Identify */
        var payload = new VehicleIdentifyRequestModel();
        payload.File = file;

        var response = await _httpClientService.PostAsync<ResultResponseModel<VehicleIdentifyResponseModel>>(_configuration["Identify:Endpoint"], payload, ContentTypes.MultipartFormData);
        var licensePlates = response.Data?.LicensePlates ?? new List<VehicleIdentifyLicensePlateRequestModel>();
        /* Query */
        var vehicles = await _unitOfWork.Repository<Data.Entities.Vehicle.Vehicle>().GetAllAsync();
        foreach (var vehicle in vehicles)
            vehicle.LicenseNumber = (new Regex("[_-]")).Replace(vehicle.LicenseNumber.ToUpper(), String.Empty);
        /* Image */
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        await _fileService.WriteImageRecognition(file.OpenReadStream(), fileName);
        /* Builder */
        var result = new List<string>();
        foreach (var licensePlate in licensePlates)
        {
            var number = (new Regex("[_-]")).Replace(licensePlate.Number.ToUpper(), String.Empty);
            var vehicle = vehicles.FirstOrDefault(_ => _.LicenseNumber == number);
            var vehicleLog = new VehicleLog(response.Data.Time, JsonSerializer.Serialize(licensePlate.Coordinate), licensePlate.Number.ToUpper(), fileName, vehicle?.Id);
            /* Add */
            _unitOfWork.Repository<Data.Entities.Vehicle.VehicleLog>().Add(vehicleLog);
            result.Add(licensePlate.Number.ToUpper());
        }
        /* Save */
        await _unitOfWork.SaveChangesAsync();
        /* Return */
        return result;
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
            .Where(_ => _.VehicleId == id)
            .Where(_ => _.Time >= fromDate)
            .Where(_ => _.Time <= toDate.EndOfDay());
        var logs = await query.ToListAsync();
        /* Builder */
        var logModels = new List<VehicleLogResponseModel>();
        foreach (var log in logs)
        {
            var logModel = _mapper.Map<Data.Entities.Vehicle.VehicleLog, VehicleLogResponseModel>(log);
            if (!string.IsNullOrEmpty(log.ImageRecognition))
                logModel.ImageRecognition = $"{UrlUtilities.GetBaseUrl(new HttpContextAccessor().HttpContext, true)}/{_fileService.GetPathOfImageRecognition(log.ImageRecognition)}";
            if (log.Vehicle != null && log.Vehicle.Customer != null)
                logModel.Customer = _mapper.Map<Data.Entities.Customer.Customer, CustomerResponseModel>(log.Vehicle.Customer);
            /* Add */
            logModels.Add(logModel);
        }
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
                .Where(_ => !filter.Type.HasValue || _.Type == filter.Type.Value)
                .Where(_ => !filter.HasRegistered.HasValue || (filter.HasRegistered.HasValue && _.CustomerId.HasValue) || (!filter.HasRegistered.HasValue && !_.CustomerId.HasValue));
    }
    #endregion
}