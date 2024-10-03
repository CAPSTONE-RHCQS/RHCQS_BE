using RHCQS_BusinessObject.Payload.Request;
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
    public interface IPackageTypeService
    {
        Task<IPaginate<PackageTypeResponse>> GetAllPackageTypesAsync(int page, int size);
        Task<PackageType> CreatePackageTypeAsync(PackageTypeRequest packageType);
    }
}
