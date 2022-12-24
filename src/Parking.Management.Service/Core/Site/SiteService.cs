using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Common.Exception;
using Parking.Management.ViewModel.Common.Response;
using Parking.Management.ViewModel.Site.Request;
using Parking.Management.ViewModel.Site.Response;
using Parking.Management.ViewModel.Camera.Request;
using Parking.Management.ViewModel.Camera.Response;

namespace Parking.Management.Service.Core.Site;

public interface ISiteService: IBaseService<SqlDbContext, Data.Entities.Site.Site, SiteFilterRequestModel, SiteResponseModel, SiteAddRequestModel, SiteUpdateRequestModel>
{
    Task AddCamera(List<CameraAddRequestModel> models, Guid id);

    Task RemoveCamera(List<Guid> cameraIds, Guid id);

    Task<List<CameraResponseModel>> GetCamera(Guid id);
    
    #region --- Utilities ---
    Task<IQueryable<Data.Entities.Site.Site>> GenerateSiteQuery(SiteFilterRequestModel filter, Guid userId);
    #endregion
}

public class SiteService: BaseService<SqlDbContext, Data.Entities.Site.Site, SiteFilterRequestModel, SiteResponseModel, SiteAddRequestModel, SiteUpdateRequestModel>, ISiteService
{
    
    public SiteService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }
    
    public async Task AddCamera(List<CameraAddRequestModel> models, Guid id)
    {
        #region --- Validate ---
        var site = await _unitOfWork.Repository<Data.Entities.Site.Site>().GetAsync(_ => _.Id == id);
        if (site == null)
            throw new ServiceException(null, "Site isn't existed");
        #endregion
        
        /* Do it */
        foreach (var model in models)
        {
            var camera = _mapper.Map<CameraAddRequestModel, Data.Entities.Camera.Camera>(model);
            camera.SiteId = site.Id;
            /* Add */
            _unitOfWork.Repository<Data.Entities.Camera.Camera>().Add(camera);
        }
        /* Save */
        await _unitOfWork.DbContext.SaveChangesAsync();
    }

    public async Task RemoveCamera(List<Guid> cameraIds, Guid id)
    {
        #region --- Validate ---
        var site = await _unitOfWork.Repository<Data.Entities.Site.Site>().GetAsync(_ => _.Id == id);
        if (site == null)
            throw new ServiceException(null, "Site isn't existed");
        #endregion
        
        /* Do it */
        var cameras = await _unitOfWork.Repository<Data.Entities.Camera.Camera>().GetManyAsync(_ => _.SiteId == site.Id);
        foreach (var camera in cameras)
        {
            if (!cameraIds.Contains(camera.Id))
                continue;
            
            /* Remove */
            _unitOfWork.Repository<Data.Entities.Camera.Camera>().Delete(camera);
        }
        /* Save */
        await _unitOfWork.DbContext.SaveChangesAsync();
    }

    public async Task<List<CameraResponseModel>> GetCamera(Guid id)
    {
        #region --- Validate ---
        var site = await _unitOfWork.Repository<Data.Entities.Site.Site>().GetAsync(_ => _.Id == id);
        if (site == null)
            throw new ServiceException(null, "Site isn't existed");
        #endregion
    
        /* Query */
        var vehicles = await _unitOfWork.Repository<Data.Entities.Camera.Camera>().GetManyAsync(_ => _.SiteId == site.Id);
        /* Builder */
        var vehicleModels = _mapper.Map<List<Data.Entities.Camera.Camera>, List<CameraResponseModel>>(vehicles.ToList());
        /* Return */
        return vehicleModels;
    }
    
    public override async Task<PagingResponseModel<SiteResponseModel>> GetPagedResult(SiteFilterRequestModel filter, Guid userId)
    {
        /* Query */
        var query = await GenerateSiteQuery(filter, userId);
        var sites = await query
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        /* Builder */
        var result = new PagingResponseModel<SiteResponseModel>();
        result.Data = _mapper.Map<List<Data.Entities.Site.Site>, List<SiteResponseModel>>(sites);
        result.TotalCounts = await query.CountAsync();
        /* Return */
        return result;
    }

    #region --- Utilities ---
    public async Task<IQueryable<Data.Entities.Site.Site>> GenerateSiteQuery(SiteFilterRequestModel filter, Guid userId)
    {
        return _unitOfWork.DbContext.Sites
            .Where(_ => string.IsNullOrEmpty(filter.Keyword) || _.Name.ToLower() == filter.Keyword.ToLower())
            .Where(_ => string.IsNullOrEmpty(filter.Name) || _.Name.ToLower() == filter.Name.ToLower());
    }
    #endregion
}