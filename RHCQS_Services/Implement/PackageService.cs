using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Package;
using RHCQS_BusinessObject.Payload.Response.Utility;
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
                        pl.Labor?.Type,
                        pl.Price,
                        pl.InsDate
                    )).ToList() ?? new List<PackageLaborResponse>(),
                    pd.PackageMaterials?.Select(pm => new PackageMaterialResponse(
                        pm.Id,
                        pm.MaterialSectionId,
                        pm.MaterialSection?.Name,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Name,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Price ?? 0.0,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Unit,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Size,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Shape,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.ImgUrl,
                        pm.MaterialSection?.Materials.FirstOrDefault()?.Description,
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
                    : new PackageTypeResponse()
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
                           .ThenInclude(ms => ms.Materials)
                           .Include(x => x.PackageType),
                orderBy: x => x.OrderBy(x => x.InsDate),
                predicate: x => x.Status == "Active",
                page: page,
                size: size
            );

            return listPackage;
        }
        public async Task<List<PackageResponseForMoblie>> GetListPackage()
        {
            try
            {
                var listPackage = await _unitOfWork.GetRepository<Package>().GetListAsync(
                selector: x => new PackageResponseForMoblie(
                    x.Id,
                    x.PackageTypeId,
                    x.PackageType.Name,
                    x.PackageName,
                    x.Unit,
                    x.Price,
                    x.Status,
                    x.InsDate,
                    x.UpsDate,
                    x.PackageDetails.Select(pd => new PackageDetailsResponseForMoblie(
                        pd.Id,
                        pd.Action,
                        pd.Type,
                        pd.InsDate,
                        pd.PackageLabors.Select(pl => new PackageLaborResponseForMoblie(
                            pl.Labor.Name,
                            pl.Labor.Type
                        )).ToList(),
                        pd.PackageMaterials.Select(pm => new PackageMaterialResponse(
                            pm.Id,
                            pm.MaterialSectionId,
                            pm.MaterialSection.Name,
                            pm.MaterialSection.Materials.FirstOrDefault().Name,
                            pm.MaterialSection.Materials.FirstOrDefault().Price ?? 0.0,
                            pm.MaterialSection.Materials.FirstOrDefault().Unit,
                            pm.MaterialSection.Materials.FirstOrDefault().Size,
                            pm.MaterialSection.Materials.FirstOrDefault().Shape,
                            pm.MaterialSection.Materials.FirstOrDefault().ImgUrl,
                            pm.MaterialSection.Materials.FirstOrDefault().Description,
                            pm.InsDate
                        )).ToList() ?? new List<PackageMaterialResponse>()
                    )).ToList(),
                    x.PackageHouses.Select(ph => new PackageHousesResponse(
                        ph.Id,
                        ph.DesignTemplateId,
                        ph.ImgUrl,
                        ph.InsDate
                    )).ToList() ?? new List<PackageHousesResponse>()
                ),
                include: x => x.Include(x => x.PackageType)
                               .Include(x => x.PackageDetails)
                                   .ThenInclude(pd => pd.PackageLabors)
                                       .ThenInclude(lb => lb.Labor)
                               .Include(x => x.PackageDetails)
                                   .ThenInclude(pd => pd.PackageMaterials)
                                       .ThenInclude(pm => pm.MaterialSection)
                                           .ThenInclude(ms => ms.Materials)
                               .Include(x => x.PackageHouses),
                orderBy: x => x.OrderBy(x => x.InsDate),
                predicate: x => x.Status == "Active"
            );

                return listPackage.ToList();
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, ex.Message);
            }
            
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
                               .ThenInclude(ms => ms.Materials)
                               .Include(x => x.PackageType)
            );

            return package != null ? MapPackageToResponse(package) :
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Resource
                );
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
                               .ThenInclude(ms => ms.Materials)
                               .Include(x => x.PackageType)
            );

            return package != null ? MapPackageToResponse(package) :
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Resource
                );
        }

        public async Task<bool> CreatePackage(PackageRequest packageRequest)
        {
            if (packageRequest == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var packageRepo = _unitOfWork.GetRepository<Package>();
            if (await packageRepo.AnyAsync(p => p.PackageName.Contains(packageRequest.PackageName)))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PackageExists
                );
            }

            var package = new Package
            {
                Id = Guid.NewGuid(),
                PackageTypeId = packageRequest.PackageTypeId,
                PackageName = packageRequest.PackageName,
                Unit = packageRequest.Unit,
                Price = packageRequest.Price,
                Status = packageRequest.Status,
                InsDate = DateTime.Now,
                PackageDetails = packageRequest.PackageDetails.Select(pd => new PackageDetail
                {
                    Id = Guid.NewGuid(),
                    Action = pd.Action,
                    Type = pd.Type,
                    InsDate = DateTime.Now,
                    PackageLabors = pd.PackageLabors?.Select(pl => new PackageLabor
                    {
                        Id = Guid.NewGuid(),
                        LaborId = pl.LaborId,
                        Price = pl.TotalPrice,
                        InsDate = DateTime.Now,
                    }).ToList(),
                    PackageMaterials = pd.PackageMaterials?.Select(pm => new PackageMaterial
                    {
                        Id = Guid.NewGuid(),
                        MaterialSectionId = pm.MaterialSectionId,
                        InsDate = DateTime.Now
                    }).ToList()
                }).ToList(),
                PackageHouses = packageRequest.PackageHouses?.Select(ph => new PackageHouse
                {
                    Id = Guid.NewGuid(),
                    DesignTemplateId = ph.DesignTemplateId,
                    ImgUrl = ph.ImgUrl,
                    Description = ph.Description,
                    InsDate = DateTime.Now
                }).ToList()
            };

            await packageRepo.InsertAsync(package);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CreatePackage
                );
            }
            return isSuccessful;
        }

        public async Task<Package> UpdatePackage(PackageRequest packageRequest, Guid packageid)
        {
            if (packageRequest == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var packageRepo = _unitOfWork.GetRepository<Package>();

            var existingPackage = await packageRepo.FirstOrDefaultAsync(
                predicate: p => p.Id == packageid,
                include: p => p.Include(pd => pd.PackageDetails)
                               .ThenInclude(pl => pl.PackageLabors)
                               .Include(pd => pd.PackageDetails)
                               .ThenInclude(pm => pm.PackageMaterials)
                               .Include(p => p.PackageHouses)
            );

            if (existingPackage == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.PackageNotFound
                );
            }

            existingPackage.PackageTypeId = packageRequest.PackageTypeId;
            existingPackage.PackageName = packageRequest.PackageName;
            existingPackage.Unit = packageRequest.Unit;
            existingPackage.Price = packageRequest.Price;
            existingPackage.Status = packageRequest.Status;
            existingPackage.UpsDate = DateTime.Now;

            foreach (var pd in packageRequest.PackageDetails)
            {
                var existingPackageDetail = existingPackage.PackageDetails.FirstOrDefault(detail => detail.PackageId == packageid);

                if (existingPackageDetail != null)
                {
                    existingPackageDetail.Action = pd.Action;
                    existingPackageDetail.Type = pd.Type;

                    foreach (var labor in pd.PackageLabors)
                    {
                        var existingLabor = existingPackageDetail.PackageLabors.FirstOrDefault(l => l.PackageDetailId == existingPackageDetail.Id);
                        if (existingLabor != null)
                        {
                            existingLabor.LaborId = labor.LaborId;
                            existingLabor.Price = labor.TotalPrice;
                        }
                        else
                        {
                            continue;
                            throw new AppConstant.MessageError(
                                (int)AppConstant.ErrCode.Conflict,
                                AppConstant.ErrMessage.PackageLaborNotFound
                            );
                        }
                    }

                    foreach (var material in pd.PackageMaterials)
                    {
                        var existingMaterial = existingPackageDetail.PackageMaterials.FirstOrDefault(m => m.PackageDetailId == existingPackageDetail.Id);
                        if (existingMaterial != null)
                        {
                            existingMaterial.MaterialSectionId = material.MaterialSectionId;
                        }
                        else
                        {
                            continue;
                            throw new AppConstant.MessageError(
                                (int)AppConstant.ErrCode.Conflict,
                                AppConstant.ErrMessage.PackagematerialNotFound
                            );
                        }
                    }
                }
                else
                {

                }
            }

            foreach (var ph in packageRequest.PackageHouses)
            {
                var existingPackageHouse = existingPackage.PackageHouses.FirstOrDefault(h => h.PackageId == packageid);
                if (existingPackageHouse != null)
                {
                    existingPackageHouse.DesignTemplateId = ph.DesignTemplateId;
                    existingPackageHouse.ImgUrl = ph.ImgUrl;
                    existingPackageHouse.Description = ph.Description;
                }
                else
                {
                    continue;
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.PackageHouseNotFound
                    );
                }
            }

            packageRepo.UpdateAsync(existingPackage);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.UpdatePackage
                );
            }

            return existingPackage;
        }

        public async Task<List<AutoPackageResponse>> GetDetailPackageByContainName(string name)
        {
            try
            {
                var normalizedName = name.RemoveDiacritics();

                var packageItems = await _unitOfWork.GetRepository<Package>()
                    .GetListAsync(predicate: p => p.Status == AppConstant.General.Active);

                var filteredItems = packageItems
                    .Where(con =>
                        con.PackageName != null && con.PackageName.RemoveDiacritics().Contains(normalizedName))
                    .ToList();

                return filteredItems.Select(packageItem => new AutoPackageResponse(
                    packageId: packageItem.Id,
                    packageName: packageItem.PackageName!,
                    price: packageItem.Price ?? 0 
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}

