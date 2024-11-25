using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface ILaborService
    {
        Task<IPaginate<LaborResponse>> GetListLabor(int page, int size);
        Task<LaborResponse> GetDetailLabor(Guid id);
        Task<bool> CreateLabor(LaborRequest request);
        Task<bool> UpdateLabor(Guid id, LaborRequest request);
        Task<bool> ImportLaborFromExcel(IFormFile excelFile);
        Task<List<LaborResponse>> SearchLaborByName(Guid packageId, string name);
    }
}
