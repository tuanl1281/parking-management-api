using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Entities.Role;
using Parking.Management.Data.Entities.User;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Permission.Request;
using Parking.Management.ViewModel.Permission.Response;

namespace Parking.Management.Service.Core.Permission;

public interface IPermissionService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.Permission.Permission, PermissionFilterRequestModel, PermissionResponseModel, PermissionAddRequestModel, PermissionUpdateRequestModel>
{
    Task AddUser(List<Guid> userIds, Guid id);

    Task RemoveUser(List<Guid> userIds, Guid id);
    
    Task AddRole(List<Guid> roleIds, Guid id);

    Task RemoveRole(List<Guid> roleIds, Guid id);

}

public class PermissionService: BaseService<SqlDbContext, Parking.Management.Data.Entities.Permission.Permission, PermissionFilterRequestModel, PermissionResponseModel, PermissionAddRequestModel, PermissionUpdateRequestModel>, IPermissionService
{
    public PermissionService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }
    
    public async Task AddUser(List<Guid> userIds, Guid id)
    {
        /* Validate */
        var permission = await _unitOfWork.Repository<Parking.Management.Data.Entities.Permission.Permission>().GetAsync(_ => _.Id == id);
        if (permission == null)
            throw new ServiceException(null, "Permission isn't existed");
        /* Execute */
        var userPermissions = await _unitOfWork.Repository<UserPermission>().GetManyAsync(_ => _.PermissionId == id);
        var differences = ArrayUtilities.Different(userPermissions.Select(_ => _.UserId).ToList(), userIds);
        
        /* Add */
        foreach (var userId in differences.Added)
        {
            var userPermission = new UserPermission(userId, permission.Id);
            _unitOfWork.Repository<UserPermission>().Add(userPermission);
        }

        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task RemoveUser(List<Guid> userIds, Guid id)
    {
        /* Validate */
        var permission = await _unitOfWork.Repository<Parking.Management.Data.Entities.Permission.Permission>().GetAsync(_ => _.Id == id);
        if (permission == null)
            throw new ServiceException(null, "Permission isn't existed");
        /* Execute */
        var userPermissions = await _unitOfWork.Repository<UserPermission>().GetManyAsync(_ => _.PermissionId == id);
        var differences = ArrayUtilities.Different(userPermissions.Select(_ => _.UserId).ToList(), userIds);
        
        /* Remove */
        foreach (var userId in differences.Removed)
        {
            var userPermission = userPermissions.FirstOrDefault(_ => _.UserId == userId && _.PermissionId == permission.Id);
            if (userPermission != null)
                _unitOfWork.Repository<UserPermission>().Delete(userPermission);
        }
        
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AddRole(List<Guid> roleIds, Guid id)
    {
        /* Validate */
        var permission = await _unitOfWork.Repository<Parking.Management.Data.Entities.Permission.Permission>().GetAsync(_ => _.Id == id);
        if (permission == null)
            throw new ServiceException(null, "Permission isn't existed");
        /* Execute */
        var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetManyAsync(_ => _.PermissionId == id);
        var differences = ArrayUtilities.Different(rolePermissions.Select(_ => _.RoleId).ToList(), roleIds);
        
        /* Add */
        foreach (var roleId in differences.Added)
        {
            var rolePermission = new RolePermission(roleId, permission.Id);
            _unitOfWork.Repository<RolePermission>().Add(rolePermission);
        }

        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task RemoveRole(List<Guid> roleIds, Guid id)
    {
        /* Validate */
        var permission = await _unitOfWork.Repository<Parking.Management.Data.Entities.Permission.Permission>().GetAsync(_ => _.Id == id);
        if (permission == null)
            throw new ServiceException(null, "Permission isn't existed");
        /* Execute */
        var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetManyAsync(_ => _.PermissionId == id);
        var differences = ArrayUtilities.Different(rolePermissions.Select(_ => _.RoleId).ToList(), roleIds);
        
        /* Remove */
        foreach (var roleId in differences.Removed)
        {
            var rolePermission = rolePermissions.FirstOrDefault(_ => _.RoleId == roleId && _.PermissionId == permission.Id);
            if (rolePermission != null)
                _unitOfWork.Repository<RolePermission>().Delete(rolePermission);
        }
        
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
}