using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Camera;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Camera.Request;

namespace Parking.Management.Application.Controllers;

public class CamerasController: BaseController
{
    private readonly ICameraService _cameraService;

    public CamerasController(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] CameraFilterRequestModel filter)
    {
        var result = await _cameraService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _cameraService.Get(id);
        return BuildResultResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] CameraAddRequestModel model)
    {
        var result = await _cameraService.Add(model);
        return BuildResultResponse(result);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] CameraUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _cameraService.Update(model, id);
        return BuildResultResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _cameraService.Delete(id);
        return BuildResultResponse(result);
    }
}