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
    public interface IMaterialTypeService
    {
        Task<IPaginate<MaterialTypeResponse>> GetListMaterialType(int page, int size);
        Task<MaterialTypeResponse> GetDetailMaterialType(Guid id);
        Task<bool> CreateMaterialType(MaterialTypeRequest request);
        Task<bool> UpdateMaterialType(Guid id, MaterialTypeRequest request);
        Task<IPaginate<MaterialTypeResponse>> SearchMaterialTypeByName(string name, int page, int size);
    }
}
