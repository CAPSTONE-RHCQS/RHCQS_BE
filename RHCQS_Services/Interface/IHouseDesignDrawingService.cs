using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IHouseDesignDrawingService
    {
        Task<IPaginate<ListHouseDesginResponse>> GetListHouseDesignDrawings(int page, int size);
        Task<IPaginate<ListHouseDesginResponse>> GetListHouseDesignDrawingsForDesignStaff(int page, int size, Guid accountId);
        Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawing(Guid id);
        Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawingByType(string type);
        Task<List<ListHouseDesginResponse>> GetListTaskByAccount(Guid accountId);
        Task<(bool IsSuccess, string Message)> CreateListTaskHouseDesignDrawing(HouseDesignDrawingRequest item);
        Task<List<ListHouseDesginResponse>> ViewDrawingPreviousStep(Guid accountId, Guid projectId);
        Task<List<ListHouseDesginResponse>> ViewDrawingByProjectId(Guid projectId);
        Task<string> CreateProjectHaveDrawing(Guid projectId, Guid accountId, ProjectHaveDrawingRequest files);

        Task<string> ConfirmDrawingAvaliable(Guid drawingId, AssignHouseDrawingRequest request);
        Task<string> DesignRequirements(Guid projectId);
        Task<string> GetStatusHouseDesign(Guid houseDesignId);
    }
}
