using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Data.Entities.User;
using Parking.Management.Data.Entities.Role;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Role.Request;
using Parking.Management.ViewModel.Role.Response;

namespace Parking.Management.Service.Core.Role;

public interface IRoleService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.Role.Role, RoleFilterRequestModel, RoleResponseModel, RoleAddRequestModel, RoleUpdateRequestModel>
{
    Task AddUser(List<Guid> userIds, Guid id);
    
    Task RemoveUser(List<Guid> userIds, Guid id);
    
    Task AddPermission(List<Guid> permissionIds, Guid id);
    
    Task RemovePermission(List<Guid> permissionIds, Guid id);
}

public class RoleService: BaseService<SqlDbContext, Parking.Management.Data.Entities.Role.Role, RoleFilterRequestModel, RoleResponseModel, RoleAddRequestModel, RoleUpdateRequestModel>, IRoleService
{
    public RoleService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task AddUser(List<Guid> userIds, Guid id)
    {
        /* Validate */
        var role = await _unitOfWork.Repository<Parking.Management.Data.Entities.Role.Role>().GetAsync(_ => _.Id == id);
        if (role == null)
            throw new ServiceException(null, "Role isn't existed");
        /* Execute */
        var userRoles = await _unitOfWork.Repository<UserRole>().GetManyAsync(_ => _.RoleId == id);
        var differences = ArrayUtilities.Different(userRoles.Select(_ => _.UserId).ToList(), userIds);
        
        /* Add */
        foreach (var userId in differences.Added)
        {
            var userRole = new UserRole(userId, role.Id);
            _unitOfWork.Repository<UserRole>().Add(userRole);
        }

        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task RemoveUser(List<Guid> userIds, Guid id)
    {
        /* Validate */
        var role = await _unitOfWork.Repository<Parking.Management.Data.Entities.Role.Role>().GetAsync(_ => _.Id == id);
        if (role == null)
            throw new ServiceException(null, "Role isn't existed");
        /* Execute */
        var userRoles = await _unitOfWork.Repository<UserRole>().GetManyAsync(_ => _.RoleId == id);
        var differences = ArrayUtilities.Different(userRoles.Select(_ => _.UserId).ToList(), userIds);
        
        /* Remove */
        foreach (var userId in differences.Removed)
        {
            var userRole = userRoles.FirstOrDefault(_ => _.UserId == userId && _.RoleId == role.Id);
            if (userRole != null)
                _unitOfWork.Repository<UserRole>().Delete(userRole);
        }
        
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AddPermission(List<Guid> permissionIds, Guid id)
    {
        /* Validate */
        var role = await _unitOfWork.Repository<Parking.Management.Data.Entities.Role.Role>().GetAsync(_ => _.Id == id);
        if (role == null)
            throw new ServiceException(null, "Role isn't existed");
        /* Execute */
        var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetManyAsync(_ => _.RoleId == id);
        var differences = ArrayUtilities.Different(rolePermissions.Select(_ => _.PermissionId).ToList(), permissionIds);
        
        /* Add */
        foreach (var permissionId in differences.Added)
        {
            var permissionRole = new RolePermission(permissionId, role.Id);
            _unitOfWork.Repository<RolePermission>().Add(permissionRole);
        }

        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task RemovePermission(List<Guid> permissionIds, Guid id)
    {
        /* Validate */
        var role = await _unitOfWork.Repository<Parking.Management.Data.Entities.Role.Role>().GetAsync(_ => _.Id == id);
        if (role == null)
            throw new ServiceException(null, "Role isn't existed");
        /* Execute */
        var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetManyAsync(_ => _.RoleId == id);
        var differences = ArrayUtilities.Different(rolePermissions.Select(_ => _.PermissionId).ToList(), permissionIds);
        
        /* Remove */
        foreach (var permissionId in differences.Removed)
        {
            var rolePermission = rolePermissions.FirstOrDefault(_ => _.PermissionId == permissionId && _.RoleId == role.Id);
            if (rolePermission != null)
                _unitOfWork.Repository<RolePermission>().Delete(rolePermission);
        }
        
        /* Save */
        await _unitOfWork.SaveChangesAsync();
    }
}

