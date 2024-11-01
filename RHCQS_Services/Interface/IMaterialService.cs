using RHCQS_BusinessObject.Payload.Request.Material;
using RHCQS_BusinessObject.Payload.Response.Material;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IMaterialService
    {
        Task<IPaginate<MaterialResponse>> GetListMaterial(int page, int size);
        Task<MaterialResponse> GetDetailMaterial(Guid id);
        Task<bool> CreateMaterial(MaterialRequest request);
        Task<bool> UpdateMaterial(Guid id, MaterialUpdateRequest request);
        Task<IPaginate<MaterialResponse>> SearchMaterialByName(string name, int page, int size);
        Task<IPaginate<MaterialResponse>> FilterMaterialByType(Guid materialTypeId, int page, int size);
    }
}
