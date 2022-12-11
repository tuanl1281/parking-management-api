using Microsoft.AspNetCore.Mvc;
using Parking.Management.Application.Controllers.Common;
using Parking.Management.Service.Core.Customer;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Customer.Request;

namespace Parking.Management.Application.Controllers;

public class CustomersController: BaseController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] CustomerFilterRequestModel filter)
    {
        var result = await _customerService.GetPagedResult(filter, Principal.UserId);
        return BuildPagingResponse(result.Data, result.TotalCounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
    {
        var result = await _customerService.Get(id);
        return BuildResultResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<ResultResponseModel>> Add([FromBody] CustomerAddRequestModel model)
    {
        var result = await _customerService.Add(model);
        return BuildResultResponse(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Update([FromBody] CustomerUpdateRequestModel model, [FromRoute] Guid id)
    {
        var result = await _customerService.Update(model, id);
        return BuildResultResponse(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
    {
        var result = await _customerService.Delete(id);
        return BuildResultResponse(result);
    }
}