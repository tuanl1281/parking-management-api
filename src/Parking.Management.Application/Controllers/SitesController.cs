using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Site;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Site.Request;
using Parking.Management.ViewModel.Camera.Request;

namespace Parking.Management.Application.Controllers;

public class SitesController: BaseController
{
    private readonly ISiteService _siteService;

    public SitesController(ISiteService siteService)
    {
        _siteService = siteService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] SiteFilterRequestModel filter)
    {
        var result = await _siteService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _siteService.Get(id);
        return BuildResultResponse(result);
    }
    
    [HttpGet("{id}/Camera")]
    public async Task<ActionResult<ResultResponseModel>> GetCamera([FromRoute] Guid id)
    {
        var result = await _siteService.GetCamera(id);
        return BuildResultResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] SiteAddRequestModel model)
    {
        var result = await _siteService.Add(model);
        return BuildResultResponse(result);
    }

    [HttpPost("{id}/AddCamera")]
    public async Task<ActionResult<ResultResponseModel>> AddCamera([FromBody] List<CameraAddRequestModel> models, [FromRoute] Guid id)
    {
        await _siteService.AddCamera(models, id);
        return BuildResultResponse(true);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] SiteUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _siteService.Update(model, id);
        return BuildResultResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _siteService.Delete(id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}/RemoveCamera")]
    public async Task<ActionResult<ResultResponseModel>> RemoveCamera([FromBody] List<Guid> models, [FromRoute] Guid id)
    {
        await _siteService.RemoveCamera(models, id);
        return BuildResultResponse(true);
    }
}