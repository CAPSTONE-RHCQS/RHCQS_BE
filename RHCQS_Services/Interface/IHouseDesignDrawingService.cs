using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Response;
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
        Task<IPaginate<HouseDesignDrawingResponse>> GetListHouseDesignDrawings(int page, int size);
        Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawing(Guid id);
        Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawingByType(string type);
        Task<List<HouseDesignDrawingResponse>> GetListTaskByAccount(Guid accountId);
        Task<(bool IsSuccess, string Message)> CreateListTaskHouseDesignDrawing(HouseDesignDrawingRequest item);


    }
}
