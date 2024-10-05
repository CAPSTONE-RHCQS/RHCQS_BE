using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PackageService> _logger;

        public PackageService(IUnitOfWork unitOfWork, ILogger<PackageService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Tách phần logic ánh xạ đối tượng Package sang PackageResponse thành phương thức riêng
        private static PackageResponse MapPackageToResponse(Package package)
        {
            return new PackageResponse(
                package.Id,
                package.PackageTypeId,
                package.PackageName,
                package.Unit,
                package.Price,
                package.Status,
                package.InsDate,
                package.UpsDate,
                package.PackageDetails?.Select(pd => new PackageDetailsResponse(
                    pd.Id,
                    pd.Action,
                    pd.Type,
                    pd.InsDate,
                    pd.PackageLabors?.Select(pl => new PackageLaborResponse(
                        pl.Id,
                        pl.LaborId,
                        pl.Labor?.Name,
                        pl.Price,
                        pl.Quantity,
                        pl.InsDate
                    )).ToList() ?? new List<PackageLaborResponse>(),
                    pd.PackageMaterials?.Select(pm => new PackageMaterialResponse(
                        pm.Id,
                        pm.MaterialSectionId,
                        pm.MaterialSection?.Name,
                        pm.MaterialSection?.Material?.Name,
                        pm.MaterialSection?.Material?.InventoryQuantity ?? 0,
                        (double?)((decimal?)pm.MaterialSection?.Material?.Price ?? 0.0m),
                        pm.MaterialSection?.Material?.Unit,
                        pm.MaterialSection?.Material?.Size,
                        pm.MaterialSection?.Material?.Shape,
                        pm.MaterialSection?.Material?.ImgUrl,
                        pm.MaterialSection?.Material?.Description,
                        pm.InsDate
                    )).ToList() ?? new List<PackageMaterialResponse>()
                )).ToList() ?? new List<PackageDetailsResponse>(),
                package.PackageHouses?.Select(ph => new PackageHousesResponse(
                    ph.Id,
                    ph.DesignTemplateId,
                    ph.ImgUrl,
                    ph.InsDate
                )).ToList() ?? new List<PackageHousesResponse>(),
                package.PackageType != null
                    ? new PackageTypeResponse(
                        package.PackageType.Id,
                        package.PackageType.Name,
                        package.PackageType.InsDate
                    )
                    : null
            );
        }

        public async Task<IPaginate<PackageResponse>> GetListPackageAsync(int page, int size)
        {
                var listPackage = await _unitOfWork.GetRepository<Package>().GetList(
                    selector: x => MapPackageToResponse(x),
                    include: x => x.Include(x => x.PackageHouses)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageLabors)
                               .ThenInclude(lb => lb.Labor)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageMaterials)
                               .ThenInclude(pm => pm.MaterialSection)
                               .ThenInclude(ms => ms.Material)
                               .Include(x => x.PackageType),
                    orderBy: x => x.OrderBy(x => x.InsDate),
                    predicate: x => x.Status == "Active",
                    page: page,
                    size: size
                );

                return listPackage;
        }
        public async Task<PackageResponse> GetPackageDetail(Guid id)
        {
            var package = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.Status == "Active",
                include: x => x.Include(x => x.PackageHouses)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageLabors)
                               .ThenInclude(lb => lb.Labor)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageMaterials)
                               .ThenInclude(pm => pm.MaterialSection)
                               .ThenInclude(ms => ms.Material)
                               .Include(x => x.PackageType)
            );

            return package != null ? MapPackageToResponse(package) : null;
        }

        public async Task<PackageResponse> GetPackageByName(string name)
        {
            var package = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(
                predicate: x => x.PackageName.Contains(name) && x.Status == "Active",
                include: x => x.Include(x => x.PackageHouses)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageLabors)
                               .ThenInclude(lb => lb.Labor)
                               .Include(x => x.PackageDetails)
                               .ThenInclude(pd => pd.PackageMaterials)
                               .ThenInclude(pm => pm.MaterialSection)
                               .ThenInclude(ms => ms.Material)
                               .Include(x => x.PackageType)
            );

            return package != null ? MapPackageToResponse(package) : null;
        }
    }

}

