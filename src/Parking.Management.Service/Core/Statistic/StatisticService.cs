using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Customer.Response;
using Parking.Management.ViewModel.Vehicle.Request;
using Parking.Management.ViewModel.Vehicle.Response;
using Parking.Management.ViewModel.Statistic.Request;
using Parking.Management.ViewModel.Statistic.Response;

namespace Parking.Management.Service.Core.Statistic;

public interface IStatisticService
{
    Task<PagingWithStatisticResponseModel<VehicleLogOverviewStatisticResponseModel, VehicleLogResponseModel>> GetVehicleLogPagedDetailResult(VehicleStatisticRequestModel filter, Guid userId);
}

public class StatisticService: IStatisticService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<SqlDbContext> _unitOfWork;

    private readonly IFileService _fileService;
    
    public StatisticService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork, IFileService fileService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

        _fileService = fileService;
    }

    public async Task<PagingWithStatisticResponseModel<VehicleLogOverviewStatisticResponseModel, VehicleLogResponseModel>> GetVehicleLogPagedDetailResult(VehicleStatisticRequestModel filter, Guid userId)
    {
        /* Query */
        var fromDate = filter.FromDate.ToSystemDateTime();
        var toDate = filter.ToDate.ToSystemDateTime();

        var query = _unitOfWork.DbContext.VehicleLogs
            .Where(_ => _.Time >= fromDate)
            .Where(_ => _.Time <= toDate.EndOfDay());
        
        var logs = await query
            .OrderByDescending(_ => _.Time)
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        /* Builder */
        var result = new PagingWithStatisticResponseModel<VehicleLogOverviewStatisticResponseModel, VehicleLogResponseModel>();
        result.TotalCounts = await query.CountAsync();
        result.Data = new();
        result.Statistic = new();
        
        foreach (var log in logs)
        {
            var logModel = _mapper.Map<Data.Entities.Vehicle.VehicleLog, VehicleLogResponseModel>(log);
            if (!string.IsNullOrEmpty(log.ImageRecognition))
                logModel.ImageRecognition = $"{UrlUtilities.GetBaseUrl(new HttpContextAccessor().HttpContext, true)}/{_fileService.GeneratePathImageRecognitionForUrl(log.ImageRecognition)}";
            if (log.Vehicle != null && log.Vehicle.Customer != null)
                logModel.Customer = _mapper.Map<Data.Entities.Customer.Customer, CustomerResponseModel>(log.Vehicle.Customer);
            /* Add */
            result.Data.Add(logModel);
        }
        
        result.Statistic.Total = await query.CountAsync();
        result.Statistic.HasRegistered = await query.Where(_ => _.Vehicle.CustomerId.HasValue).CountAsync();
        result.Statistic.HasntRegistered = await query.Where(_ => !_.Vehicle.CustomerId.HasValue).CountAsync();
        /* Return */
        return result;
    }
}