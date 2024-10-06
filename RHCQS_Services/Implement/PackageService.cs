﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
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
                               .ThenInclude(ms => ms.Material)
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
            if (await packageRepo.AnyAsync(p => p.PackageName == packageRequest.PackageName))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Package với name này đã tồn tại."
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
                        Quantity = pl.Quantity,
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
                    InsDate = DateTime.Now
                }).ToList()
            };

            await packageRepo.InsertAsync(package);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Tạo package thất bại"
                );
            }
            return isSuccessful;
        }

        public async Task<Package> UpdatePackage(PackageRequest packageRequest)
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
                predicate: p => p.PackageName == packageRequest.PackageName,
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
                    "Không tìm thấy package với tên này."
                );
            }

            existingPackage.PackageTypeId = packageRequest.PackageTypeId;
            existingPackage.PackageName = packageRequest.PackageName;
            existingPackage.Unit = packageRequest.Unit;
            existingPackage.Price = packageRequest.Price;
            existingPackage.Status = packageRequest.Status;
            existingPackage.UpsDate = DateTime.Now;

            existingPackage.PackageDetails.Clear();
            foreach (var pd in packageRequest.PackageDetails)
            {
                var newPackageDetail = new PackageDetail
                {
                    Action = pd.Action,
                    Type = pd.Type,
                    InsDate = DateTime.Now,
                    PackageLabors = pd.PackageLabors?.Select(pl => new PackageLabor
                    {
                        LaborId = pl.LaborId,
                        Price = pl.TotalPrice,
                        Quantity = pl.Quantity,
                        InsDate = DateTime.Now
                    }).ToList() ?? new List<PackageLabor>(),
                    PackageMaterials = pd.PackageMaterials?.Select(pm => new PackageMaterial
                    {
                        MaterialSectionId = pm.MaterialSectionId,
                        InsDate = DateTime.Now,
                    }).ToList() ?? new List<PackageMaterial>()
                };
                existingPackage.PackageDetails.Add(newPackageDetail);
            }

            existingPackage.PackageHouses.Clear();
            foreach (var ph in packageRequest.PackageHouses)
            {
                var newPackageHouse = new PackageHouse
                {
                    DesignTemplateId = ph.DesignTemplateId,
                    ImgUrl = ph.ImgUrl,
                    InsDate = DateTime.Now
                };
                existingPackage.PackageHouses.Add(newPackageHouse);
            }

            packageRepo.UpdateAsync(existingPackage);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Cập nhật package thất bại."
                );
            }

            return existingPackage;
        }

    }

}

