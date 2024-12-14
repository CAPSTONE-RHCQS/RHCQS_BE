using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Request.MateSec;
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
    public interface IMaterialSectionService
    {
        Task<IPaginate<MaterialSectionResponse>> GetListMaterialSection(int page, int size);
        Task<MaterialSectionResponse> GetDetailMaterialSection(Guid id);
        Task<bool> CreateMaterialSection(MaterialSectionRequest request);
        Task<bool> UpdateMaterialSection(Guid id, MaterialSectionUpdateRequest request);
        Task<List<MaterialSectionResponse>> SearchMaterialSectionByName(string name);
        Task<IPaginate<MaterialSectionResponse>> SearchMaterialSectionByNameWithPag(string? name, int page, int size);
    }
}
