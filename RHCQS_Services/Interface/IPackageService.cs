using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IPackageService
    {
        public Task<IPaginate<PackageResponse>> GetListPackageAsync(int page, int size);
        Task<PackageResponse> GetPackageDetail(Guid id);
        Task<PackageResponse> GetPackageByName(string name);
    }
}
