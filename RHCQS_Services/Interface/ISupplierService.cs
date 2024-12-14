using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request.Sup;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface ISupplierService
    {
        Task<IPaginate<SupplierResponse>> GetListSupplier(int page, int size);
        Task<SupplierResponse> GetDetailSupplier(Guid id);
        Task<bool> CreateSupplier(SupplierRequest request);
        Task<string?> UploadSupplierImage(IFormFile image);
        Task<bool> UpdateSupplier(Guid id, SupplierUpdateRequest request);
        Task<List<SupplierResponse>> SearchSupplierByName(string name);
        Task<IPaginate<SupplierResponse>> SearchSupplierByNameWithPag(string? name, int page, int size);
    }
}
