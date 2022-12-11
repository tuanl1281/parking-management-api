using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.User;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.User.Request;

namespace Parking.Management.Application.Controllers;

public class UsersController: BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] UserFilterRequestModel filter)
    {
        var result = await _userService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _userService.Get(id);
        return BuildResultResponse(result);
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] UserAddRequestModel model)
    {
        var result = await _userService.Add(model);
        return BuildResultResponse(result);
    }
    

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] UserUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _userService.Update(model, id);
        return BuildResultResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _userService.Delete(id);
        return BuildResultResponse(result);
    }
    
    [HttpGet("ValidateCredential")]
    public ActionResult<PagingResponseModel> ValidateCredential()
    {
        return Ok();
    }
    
    [HttpGet("Information")]
    public async Task<ActionResult<ResultResponseModel>> GetDetailOfUser()
    {
        var result = await _userService.GetDetailOfUser(Principal.UserId);
        return BuildResultResponse(result);
    }

            
    [HttpGet("Permission")]
    public async Task<ActionResult<ResultResponseModel>> GetPermission()
    {
        var result = await _userService.GetPermissionOfUser(Principal.UserId, Principal.UserId);
        return BuildResultResponse(result);
    }
        
    [HttpGet("{id}/Permission")]
    public async Task<ActionResult<ResultResponseModel>> GetPermissionOfUser([FromRoute] Guid id)
    {
        var result = await _userService.GetPermissionOfUser(id, Principal.UserId);
        return BuildResultResponse(result);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<ResultResponseModel>> Login([FromBody] LoginRequestModel model)
    {
        var result = await _userService.Login(model);
        return BuildResultResponse(result);
    }
    
    [HttpPut("ChangePassword")]
    public async Task<ActionResult<ResultResponseModel>> ChangePassword([FromBody] ChangePasswordRequestModel model)
    {
        var result = await _userService.ChangePassword(model, Principal.UserId, Principal.UserId);
        return BuildResultResponse(result);
    }
    
    [HttpPut("{id}/ChangePassword")]
    public async Task<ActionResult<ResultResponseModel>> ChangePassword([FromBody] ChangePasswordRequestModel model, [FromRoute] Guid id)
    {
        var result = await _userService.ChangePassword(model, id, Principal.UserId);
        return BuildResultResponse(result);
    }
}