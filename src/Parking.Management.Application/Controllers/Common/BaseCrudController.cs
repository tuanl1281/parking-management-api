using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Common.Response;

namespace Parking.Management.Application.Controllers.Common;

/*
	Generic
	- E: Entity
	- F: Filter model
	- V: View model
	- A: Add model
	- U: Update model
*/

public class BaseController<TContext, TE, TF, TV, TA, TU> : BaseController where TContext : DbContext where TE : class where TF : class where TV : class where TA : class where TU : class
{
	private readonly IBaseService<TContext, TE, TF, TV, TA, TU> _service;

	public BaseController(IBaseService<TContext, TE, TF, TV, TA, TU> service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<ActionResult<PagingResponseModel>> GetPagedResult([FromQuery] TF filter)
	{
		var result = await _service.GetPagedResult(filter, Principal.UserId);
		return BuildPagingResponse(result.Data, result.TotalCounts);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ResultResponseModel>> Get([FromRoute] Guid id)
	{
		var result = await _service.Get(id);
		return BuildResultResponse(result);
	}

	[HttpPost]
	public async Task<ActionResult<ResultResponseModel>> Add([FromBody] TA model)
	{
		var result = await _service.Add(model);
		return BuildResultResponse(result);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<ResultResponseModel>> Update([FromBody] TU model, [FromRoute] Guid id)
	{
		var result = await _service.Update(model, id);
		return BuildResultResponse(result);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<ResultResponseModel>> Delete([FromRoute] Guid id)
	{
		var result = await _service.Delete(id);
		return BuildResultResponse(result);
	}
}
