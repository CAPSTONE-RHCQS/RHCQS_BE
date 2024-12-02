using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
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
        Task<string?> UploadMaterialImage(IFormFile image);
        Task<bool> UpdateMaterial(Guid id, MaterialUpdateRequest request);
        Task<List<MaterialResponse>> SearchMaterialByName(Guid packageId, string name);
        Task<List<MaterialResponse>> SearchMaterialByNameWithoutPackage(string name);
        Task<List<MaterialResponse>> FilterMaterialBySection(Guid materialSectionId);
        Task<bool> ImportMaterialFromExcel(IFormFile excelFile);
    }
}
