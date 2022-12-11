using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Data.Constants.Common;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;

namespace Parking.Management.Application.Helpers.Attribute;

public class RoleAttribute: AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _role;
    public RoleAttribute(string role)
    {
        _role = role;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var unitOfWork = context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork<SqlDbContext>)) as IUnitOfWork<SqlDbContext>;
            if (unitOfWork == null)
                throw new Exception();
            
            if (!context.HttpContext.User.HasClaim(_ => _.Type.Equals(ClaimConstants.UserId)))
                throw new UnauthorizedAccessException();

            var userId = Guid.Parse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimConstants.UserId))?.Value ?? String.Empty);
            var userRole = unitOfWork.DbContext.UserRoles
                .Where(_ => _.UserId == userId)
                .Include(_ => _.Role)
                .FirstOrDefault();

            if (userRole == null || userRole.Role.Code != _role)
                throw new UnauthorizedAccessException();
        }
        catch (Exception exception)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}