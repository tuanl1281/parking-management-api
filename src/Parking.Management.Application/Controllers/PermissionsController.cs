using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Permission;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Permission.Request;

namespace Parking.Management.Application.Controllers;

public class PermissionsController: BaseController
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] PermissionFilterRequestModel filter)
    {
        var result = await _permissionService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _permissionService.Get(id);
        return BuildResultResponse(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] PermissionAddRequestModel model)
    {
        var result = await _permissionService.Add(model);
        return BuildResultResponse(result);
    }
    
    [HttpPost("{id}/AddUser")]
    public async Task<ActionResult<ResultResponseModel>> AddUser([FromBody] List<Guid> userIds, [FromRoute] Guid id)
    {
        await _permissionService.AddUser(userIds, id);
        return BuildResultResponse(null);
    }

    [HttpPost("{id}/AddRole")]
    public async Task<ActionResult<ResultResponseModel>> AddRole([FromBody] List<Guid> roleIds, [FromRoute] Guid id)
    {
        await _permissionService.AddRole(roleIds, id);
        return BuildResultResponse(null);
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] PermissionUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _permissionService.Update(model, id);
        return BuildResultResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _permissionService.Delete(id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}/RemoveUser")]
    public async Task<ActionResult<ResultResponseModel>> RemoveUser([FromBody] List<Guid> userIds, [FromRoute] Guid id)
    {
        await _permissionService.RemoveUser(userIds, id);
        return BuildResultResponse(null);
    }

    [HttpDelete("{id}/RemoveRole")]
    public async Task<ActionResult<ResultResponseModel>> RemoveRole([FromBody] List<Guid> roleIds, [FromRoute] Guid id)
    {
        await _permissionService.RemoveRole(roleIds, id);
        return BuildResultResponse(null);
    }

}