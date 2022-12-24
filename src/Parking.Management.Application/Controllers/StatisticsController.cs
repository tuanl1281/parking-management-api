using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Statistic;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Statistic.Request;

namespace Parking.Management.Application.Controllers;

public class StatisticsController: BaseController
{
    private readonly IStatisticService _statisticService;

    public StatisticsController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    
    [HttpGet("Vehicle/Log")]
    public async Task<ActionResult<PagingWithStatisticResponseModel>> GetPagedResult([FromQuery] VehicleStatisticRequestModel filter)
    {
        var result = await _statisticService.GetVehicleLogPagedDetailResult(filter, Principal.UserId);
        return BuildPagingWithStatisticResponse(result.Statistic, result.Data, result.TotalCounts);
    }
}