using AutoMapper;
using Parking.Management.Data.Context;
using Parking.Management.Data.Infrastructures;
using Parking.Management.Service.Core.Common;
using Parking.Management.ViewModel.Camera.Request;
using Parking.Management.ViewModel.Camera.Response;

namespace Parking.Management.Service.Core.Camera;

public interface ICameraService: IBaseService<SqlDbContext, Data.Entities.Camera.Camera, CameraFilterRequestModel, CameraResponseModel, CameraAddRequestModel, CameraUpdateRequestModel>
{
}

public class CameraService: BaseService<SqlDbContext, Data.Entities.Camera.Camera, CameraFilterRequestModel, CameraResponseModel, CameraAddRequestModel, CameraUpdateRequestModel>, ICameraService
{
    
    public CameraService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork) : base(mapper, unitOfWork)
    {
    }
}