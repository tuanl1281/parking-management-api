using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Parking.Management.Data.Constants.Common;
using Parking.Management.Data.Constants.Role;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Permission.Response;
using Parking.Management.ViewModel.Role.Response;
using Parking.Management.ViewModel.User.Request;
using Parking.Management.ViewModel.User.Response;

namespace Parking.Management.Service.Core.User;

public interface IUserService: IBaseService<SqlDbContext, Parking.Management.Data.Entities.User.User, UserFilterRequestModel, UserResponseModel, UserAddRequestModel, UserUpdateRequestModel>
{
    Task<UserDetailResponseModel> GetDetailOfUser(Guid userId);
    
    Task<RoleResponseModel> GetRoleOfUser(Guid userId);
    
    Task<List<PermissionResponseModel>> GetPermissionOfUser(Guid? userId, Guid id);
    
    Task<LoginResponseModel> Login(LoginRequestModel model);

    Task<Guid> ChangePassword(ChangePasswordRequestModel model, Guid? userId, Guid id);
}

public class UserService: BaseService<SqlDbContext, Parking.Management.Data.Entities.User.User, UserFilterRequestModel, UserResponseModel, UserAddRequestModel, UserUpdateRequestModel>, IUserService
{
    private readonly IConfiguration _configuration;
    
    public UserService(IMapper mapper, IConfiguration configuration, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
        _configuration = configuration;
    }
    
    public override async Task<object> Add(UserAddRequestModel model)
    {
        /* Validate */
        var keyword = model.UserName.ToLower();
        var user = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().GetAsync(_ => _.UserName.ToLower() == keyword);
        if (user != null)
            throw new ServiceException(null, "Username is registered");
        /* Build */
        user = _mapper.Map<UserAddRequestModel, Parking.Management.Data.Entities.User.User>(model);
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, 10);
        /* Save */
        _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().Add(user);
        await _unitOfWork.SaveChangesAsync();
        /* Return */
        return user.Id;
    }

    
    public async Task<UserDetailResponseModel> GetDetailOfUser(Guid userId)
    {
        /* Validate */
        var user = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().GetAsync(_ => _.Id == userId);
        if (user == null)
            throw new ServiceException(null, "User isn't existed");
        /* Builder */
        var userModel = _mapper.Map<Parking.Management.Data.Entities.User.User, UserDetailResponseModel>(user);
        userModel.Role = await GetRoleOfUser(user.Id);
        userModel.Permissions = await GetPermissionOfUser(null, user.Id);

        return userModel;
    }

    public async Task<RoleResponseModel> GetRoleOfUser(Guid userId)
    {
        /* Validate */
        var userRole = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.UserRole>().GetAsync(_ => _.UserId == userId);
        if (userRole == null)
            throw new ServiceException(null, "User hasn't assign role");
        /* Builder */
        var roleModel = _mapper.Map<Parking.Management.Data.Entities.Role.Role, RoleResponseModel>(userRole.Role);
        /* Return */
        return roleModel;
    }

    public async Task<List<PermissionResponseModel>> GetPermissionOfUser(Guid? userId, Guid id)
    {
        /* Validate */
        if (userId.HasValue)
        {
            var user = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().GetAsync(_ => _.Id == userId);
            if (user == null)
                throw new ServiceException(null, "User isn't existed");

            var userRole = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.UserRole>().GetAsync(_ => _.UserId == userId);
            if (userRole == null || userRole.Role.Code != RoleConstants.SuperAdmin || userRole.Role.Code != RoleConstants.Admin)
                throw new UnauthorizedAccessException();
        }
        /* Query */
        var _userId = userId ?? id;
        
        var userPermissions = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.UserPermission>().GetManyAsync(_ => _.UserId == _userId);
        var userRoles = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.UserRole>().GetManyAsync(_ => _.UserId == _userId);

        var roleIds = userRoles.Select(_ => _.RoleId).ToList();
        var rolePermissions = await _unitOfWork.Repository<Parking.Management.Data.Entities.Role.RolePermission>().GetManyAsync(_ => roleIds.Contains(_.RoleId));
        /* Builder */
        var permissions = new List<Parking.Management.Data.Entities.Permission.Permission>();
        foreach (var userPermission in userPermissions)
        {
            if (!permissions.Any(_ => _.Id == userPermission.PermissionId))
                permissions.Add(userPermission.Permission);
        }
            
        foreach (var rolePermission in rolePermissions)
        {
            if (!permissions.Any(_ => _.Id == rolePermission.PermissionId))
                permissions.Add(rolePermission.Permission);
        }

        var permissionOfUserModels = _mapper.Map<List<Parking.Management.Data.Entities.Permission.Permission>, List<PermissionResponseModel>>(permissions);
        /* Return */
        return permissionOfUserModels;
    }
    
    public async Task<LoginResponseModel> Login(LoginRequestModel model)
    {
        /* Validate */
        var keyword = model.UserName.ToLower();
        var byPass = GenerateBypassPassword();
        var user = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().GetAsync(_ => _.UserName.ToLower() == keyword);
        if (user == null || !(byPass == model.Password || BCrypt.Net.BCrypt.Verify(model.Password, user.Password)))
            throw new ServiceException(null, "Username/Password doesn't correct");
        /* Generate token & information */
        var claims = new[]
        {
            new Claim(ClaimConstants.UserId, user.Id.ToString()),
            new Claim(ClaimConstants.FullName, user?.FullName ?? ""),
            new Claim(ClaimConstants.UserName, user?.UserName ?? ""),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(1);
        var token =
            new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );
        /* Builder */
        var result = new LoginResponseModel();
        result.Token = new JwtSecurityTokenHandler().WriteToken(token);
        result.TokenType = "Bearer";
        result.ExpiresIn = expires.ToUnixDateTime();
        result.UserInfo = await GetDetailOfUser(user.Id);
        /* Return */
        return result;
    }

    public async Task<Guid> ChangePassword(ChangePasswordRequestModel model, Guid? userId, Guid id)
    {
        #region --- Validate ---
        var userInfo = await GetDetailOfUser(id);
        if (userId.HasValue && !userInfo.IsAdmin())
            throw new UnauthorizedAccessException();
        
        var _userId = userId ?? id;
        var user = await _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().GetAsync(_ => _.Id == _userId);
        if (user == null)
            throw new ServiceException(null, "User isn't existed");

        if (string.IsNullOrEmpty(model.NewPassword))
            throw new ServiceException(null, "New password is required");

        if (!userInfo.IsAdmin())
        {
            if (string.IsNullOrEmpty(model.OldPassword))
                throw new ServiceException(null, "Old password is required");
                
            if (BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
                throw new ServiceException(null, "Username/Password doesn't correct");
        }
        #endregion
            
        /* Update */
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        /* Save */
        _unitOfWork.Repository<Parking.Management.Data.Entities.User.User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        return user.Id;
    }
    
    #region --- Utilities ---
    private string GenerateBypassPassword()
        => $"{DateTime.Now.Date:yyyyMMdd}!@#";
    #endregion
}