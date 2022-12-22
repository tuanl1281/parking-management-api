using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Vehicle;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Vehicle.Request;

namespace Parking.Management.Application.Controllers;

public class VehiclesController: BaseController
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] VehicleFilterRequestModel filter)
    {
        var result = await _vehicleService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _vehicleService.Get(id);
        return BuildResultResponse(result);
    }
    
    [HttpGet("{id}/Log")]
    public async Task<ActionResult<ResultResponseModel>> GetLog([FromQuery] VehicleLogFilterRequestModel filter, [FromRoute] Guid id)
    {
        var result = await _vehicleService.GetLog(filter, id);
        return BuildResultResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] VehicleAddRequestModel model)
    {
        var result = await _vehicleService.Add(model);
        return BuildResultResponse(result);
    }

    [HttpPost("{id}/AddCustomer/{customerId}")]
    public async Task<ActionResult<ResultResponseModel>> AddCustomer([FromRoute] Guid customerId, [FromRoute] Guid id)
    {
        await _vehicleService.AddCustomer(customerId, id);
        return BuildResultResponse(true);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] VehicleUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _vehicleService.Update(model, id);
        return BuildResultResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _vehicleService.Delete(id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}/RemoveCustomer")]
    public async Task<ActionResult<ResultResponseModel>> RemoveCustomer([FromRoute] Guid id)
    {
        await _vehicleService.RemoveCustomer(id);
        return BuildResultResponse(true);
    }
}