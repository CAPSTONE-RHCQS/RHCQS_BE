using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IHouseDesignVersionService
    {
        Task<HouseDesignVersionItemResponse> GetDetailVersionById(Guid versionId);
        Task<bool> CreateHouseDesignVersion(Guid accountId, HouseDesignVersionRequest request);
        Task<bool> UploadDesignDrawing(List<IFormFile> files, Guid versionId);
        Task<bool> ApproveHouseDrawing(Guid Id, AssignHouseDrawingRequest request);
        Task<string> ConfirmDesignDrawingFromCustomer(Guid versionId);
        Task<string> CommentDesignDrawingFromCustomer(Guid versionId, FeedbackHouseDesignDrawingRequest comment);
    }
}
