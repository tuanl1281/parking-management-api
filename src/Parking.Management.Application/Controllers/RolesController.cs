using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Role;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Role.Request;

namespace Parking.Management.Application.Controllers;

public class RolesController: BaseController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] RoleFilterRequestModel filter)
    {
        var result = await _roleService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _roleService.Get(id);
        return BuildResultResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] RoleAddRequestModel model)
    {
        var result = await _roleService.Add(model);
        return BuildResultResponse(result);
    }

    [HttpPost("{id}/AddUser")]
    public async Task<ActionResult<ResultResponseModel>> AddUser([FromBody] List<Guid> userIds, [FromRoute] Guid id)
    {
        await _roleService.AddUser(userIds, id);
        return BuildResultResponse(null);
    }

    [HttpPost("{id}/AddPermission")]
    public async Task<ActionResult<ResultResponseModel>> AddPermission([FromBody] List<Guid> permissionIds, [FromRoute] Guid id)
    {
        await _roleService.AddPermission(permissionIds, id);
        return BuildResultResponse(null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] RoleUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _roleService.Update(model, id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _roleService.Delete(id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}/RemoveUser")]
    public async Task<ActionResult<ResultResponseModel>> RemoveUser([FromBody] List<Guid> userIds, [FromRoute] Guid id)
    {
        await _roleService.RemoveUser(userIds, id);
        return BuildResultResponse(null);
    }

    [HttpDelete("{id}/RemovePermission")]
    public async Task<ActionResult<ResultResponseModel>> RemovePermission([FromBody] List<Guid> permissionIds, [FromRoute] Guid id)
    {
        await _roleService.RemovePermission(permissionIds, id);
        return BuildResultResponse(null);
    }
}