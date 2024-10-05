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

        public async Task<IPaginate<PackageResponse>> GetListPackageAsync(int page, int size)
        {
            var listPackage = await _unitOfWork.GetRepository<Package>().GetList(
                selector: x => new PackageResponse(
                    x.Id,
                    x.PackageTypeId,
                    x.PackageName,
                    x.Unit,
                    x.Price,
                    x.Status,
                    x.InsDate,
                    x.UpsDate,
                    x.PackageDetails.Select(pd => new PackageDetailsResponse(
                        pd.Id,
                        pd.Action,
                        pd.Type,
                        pd.InsDate,
                        pd.PackageLabors.Select(pl => new PackageLaborResponse(
                            pl.Id,
                            pl.LaborId,
                            pl.Labor.Name,
                            pl.Price,
                            pl.Quantity,
                            pl.InsDate
                        )).ToList(),
                        pd.PackageMaterials.Select(pm => new PackageMaterialResponse(
                            pm.Id,
                            pm.MaterialSectionId,
                            pm.MaterialSection.Name,
                            pm.MaterialSection.Material.Name,
                            pm.MaterialSection.Material.InventoryQuantity,
                            pm.MaterialSection.Material.Price,
                            pm.MaterialSection.Material.Unit,
                            pm.MaterialSection.Material.Size,
                            pm.MaterialSection.Material.Shape,
                            pm.MaterialSection.Material.ImgUrl,
                            pm.MaterialSection.Material.Description,
                            pm.InsDate
                        )).ToList()
                    )).ToList(),
                    x.PackageHouses.Select(ph => new PackageHousesResponse(
                        ph.Id,
                        ph.DesignTemplateId,
                        ph.ImgUrl,
                        ph.InsDate
                    )).ToList(),
                    new PackageTypeResponse(
                        x.PackageType.Id,
                        x.PackageType.Name,
                        x.PackageType.InsDate
                    )
                ),
                include: x => x.Include(x => x.PackageHouses)
                                .Include(x => x.PackageDetails)
                                .ThenInclude(pd => pd.PackageLabors)
                                .Include(x => x.PackageDetails)
                                .ThenInclude(pd => pd.PackageMaterials)
                                .ThenInclude(pd => pd.MaterialSection)
                                .Include(x => x.PackageType),
                orderBy: x => x.OrderBy(x => x.InsDate),
                predicate: x => x.Status == "Active",
                page: page,
                size: size
            );

            return listPackage;
        }

    }
}
