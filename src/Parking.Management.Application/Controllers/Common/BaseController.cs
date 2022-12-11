using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Parking.Management.Application.Helpers.Authentication;
using Parking.Management.Data.Constants.Common;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Common.Response;

namespace Parking.Management.Application.Controllers.Common;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class BaseController: ControllerBase
{
    protected new IPrincipal Principal
    {
        get
        {
            try
            {
                var userId = GetClaim(ClaimConstants.UserId);
                if (string.IsNullOrEmpty(userId))
                    throw new Exception();

                return new Principal()
                {
                    UserId = Guid.Parse(userId),
                    FullName = GetClaim(ClaimConstants.FullName),
                    UserName = GetClaim(ClaimConstants.UserName),
                    Role = GetClaim(ClaimConstants.Role),
                };
            }
            catch (Exception)
            {
                throw new ServiceException("Invalid token");
            }
        }
    }
    
    public BaseController()
    {
    }

    private string GetClaim(string claim)
    {
        if (!HttpContext.User.HasClaim(_ => _.Type.Equals(claim)))
            return String.Empty;

        return HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(claim))?.Value ?? String.Empty;
    }
    
    protected ResultResponseModel BuildResultResponse(object data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        HttpContext.Response.StatusCode = (int)statusCode;
        return new ResultResponseModel(true, data);
    }

    protected ResultResponseModel BuildResultResponse(object data, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        HttpContext.Response.StatusCode = (int) statusCode;
        return new ResultResponseModel(true, data, message);
    }
    
    protected PagingResponseModel BuildPagingResponse(object data, double totalCounts)
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        return new PagingResponseModel(data, totalCounts);
    }
}