﻿using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DinkToPdf.Contracts;
using DinkToPdf;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClosedXML;

namespace RHCQS_Services.Implement
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PackageService> _logger;
        private readonly Cloudinary _cloudinary;
        private readonly IConverter _converter;
        public PackageService(IUnitOfWork unitOfWork, ILogger<PackageService> logger, Cloudinary cloudinary, IConverter converter)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
            _converter = converter;
        }

        private static PackageResponse MapPackageToResponse(Package package)
        {
            return new PackageResponse(
                package.Id,
                package.PackageName,
                package?.Type,
                package.Unit,
                package.Price,
                package.Status,
                package.InsDate,
                package.UpsDate,
                package.PackageLabors?.Select(pl => new PackageLaborResponse(
                    pl.Id,
                    pl.LaborId,
                    pl.Labor?.Name,
                    pl.Labor?.Type,
                    pl.Labor?.Price ?? 0.0,
                    pl.InsDate
                )).ToList() ?? new List<PackageLaborResponse>(),

                package.PackageMaterials?.Select(pm => new PackageMaterialResponse(
                    pm.Id,
                    pm.Material.MaterialSectionId,
                    pm.Material.MaterialSection?.Name,
                    pm.Material?.Name,
                    pm.Material?.Type,
                    pm.Material.Price ?? 0.0,
                    pm.Material.Unit
                    //pm.Material.Size,
                    //pm.Material.Shape,
                    //pm.Material.ImgUrl,
                    //pm.Material.Description,
                    //pm.InsDate
                )).ToList() ?? new List<PackageMaterialResponse>(),

                package.PackageHouses?.Select(ph => new PackageHousesResponse(
                    ph.Id,
                    ph.DesignTemplateId,
                    ph.ImgUrl,
                    ph.InsDate
                )).ToList() ?? new List<PackageHousesResponse>(),

                package.PackageMapPromotions?.Select(ph => new PackagePromotionResponse(
                    ph.Id,
                    ph.PromotionId,
                    ph.Promotion.Name,
                    ph.Promotion.Value,
                    ph.Promotion.StartTime,
                    ph.Promotion.InsDate
                )).ToList() ?? new List<PackagePromotionResponse>(),

                package.WorkTemplates?.Select(wt => new WorkTemplateResponse(
                    wt.Id,
                    wt.ContructionWorkId,
                    wt.ContructionWork?.WorkName,
                    wt.LaborCost ?? 0,
                    wt.MaterialCost ?? 0,
                    wt.MaterialFinishedCost ?? 0
                )).ToList() ?? new List<WorkTemplateResponse>()

            );
        }

        private static PackageListResponse MapPackageListToResponse(Package package)
        {
            return new PackageListResponse(
                package.Id,
                package.PackageName,
                package.Unit,
                package.Price,
                package.Status,
                package.Type ?? string.Empty
            );
        }
        public async Task<IPaginate<PackageListResponse>> GetListPackageAsync(int page, int size)
        {
            var listPackage = await _unitOfWork.GetRepository<Package>().GetList(
                selector: x => MapPackageListToResponse(x),
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
                        x.Type,
                        x.PackageName,
                        x.Unit,
                        x.Price,
                        x.Status,
                        x.InsDate,
                        x.UpsDate,
                        x.PackageLabors.Select(pl => new PackageLaborResponseForMoblie(
                            pl.Labor.Name,
                            pl.Labor.Type
                        )).ToList(),
                        x.PackageMaterials.Select(pm => new PackageMaterialResponse(
                            pm.Id,
                            pm.Material.MaterialSectionId,
                            pm.Material.MaterialSection.Name,
                            pm.Material.Name,
                            pm.Material.Type,
                            pm.Material.Price ?? 0.0,
                            pm.Material.Unit
                            //pm.Material.Size,
                            //pm.Material.Shape,
                            //pm.Material.ImgUrl,
                            //pm.Material.Description,
                            //pm.InsDate
                        )).ToList() ?? new List<PackageMaterialResponse>(),
                        x.PackageHouses.Select(ph => new PackageHousesResponse(
                            ph.Id,
                            ph.DesignTemplateId,
                            ph.ImgUrl,
                            ph.InsDate
                        )).ToList() ?? new List<PackageHousesResponse>()
                    ),
                    include: x => x.Include(pd => pd.PackageLabors)
                                   .ThenInclude(lb => lb.Labor)
                                .Include(pd => pd.PackageMaterials)
                                   .ThenInclude(ms => ms.Material)
                                   .ThenInclude(ms => ms.MaterialSection)
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
                               .Include(pd => pd.PackageLabors)
                               .ThenInclude(lb => lb.Labor)
                               .Include(pd => pd.PackageMaterials)
                               .ThenInclude(ms => ms.Material)
                               .ThenInclude(ms => ms.MaterialSection)
                               .Include(p => p.PackageMapPromotions)
                               .ThenInclude(p => p.Promotion)
                               .Include(w => w.WorkTemplates)
                               .ThenInclude(ct => ct.ContructionWork)
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
                               .Include(pd => pd.PackageLabors)
                               .ThenInclude(lb => lb.Labor)
                               .Include(pd => pd.PackageMaterials)
                               .ThenInclude(ms => ms.Material)
                               .ThenInclude(ms => ms.MaterialSection)
                               .Include(p => p.PackageMapPromotions)
                               .ThenInclude(p => p.Promotion)
                               .Include(w => w.WorkTemplates)
                               .ThenInclude(ct => ct.ContructionWork)
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
            var workTemplateRepo = _unitOfWork.GetRepository<WorkTemplate>();
            var constructionWorkResourceRepo = _unitOfWork.GetRepository<ConstructionWorkResource>();

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
                Type = packageRequest.PackageType,
                PackageName = packageRequest.PackageName,
                Unit = packageRequest.Unit,
                Price = packageRequest.Price,
                Status = packageRequest.Status,
                InsDate = LocalDateTime.VNDateTime(),
            };

            package.PackageLabors = packageRequest.PackageLabors?.Select(pl => new PackageLabor
            {
                Id = Guid.NewGuid(),
                LaborId = pl.LaborId,
                PackageId = package.Id,
                InsDate = LocalDateTime.VNDateTime(),
            }).ToList() ?? new List<PackageLabor>();

            package.PackageMaterials = packageRequest.PackageMaterials?.Select(pm => new PackageMaterial
            {
                Id = Guid.NewGuid(),
                MaterialId = pm.MaterialId,
                PackageId = package.Id,
                InsDate = LocalDateTime.VNDateTime()
            }).ToList() ?? new List<PackageMaterial>();

            if ( packageRequest.PackageHouses != null)
            {
                package.PackageHouses = packageRequest.PackageHouses?.Select(ph => new PackageHouse
                {
                    Id = Guid.NewGuid(),
                    DesignTemplateId = ph.DesignTemplateId,
                    ImgUrl = ph.ImgUrl,
                    Description = ph.Description,
                    PackageId = package.Id,
                    InsDate = LocalDateTime.VNDateTime()
                }).ToList() ?? new List<PackageHouse>();
            }

            await packageRepo.InsertAsync(package);

            if (packageRequest.WorkTemplate != null)
            {
                var workTemplates = packageRequest.WorkTemplate?.Select(wt => new WorkTemplate
                {
                    Id = Guid.NewGuid(),
                    PackageId = package.Id,
                    ContructionWorkId = wt.ConstructionWorKid,
                    LaborCost = wt.LaborCost,
                    MaterialCost = wt.MaterialCost,
                    MaterialFinishedCost = wt.MaterialFinishedCost,
                    InsDate = LocalDateTime.VNDateTime(),
                }).ToList();

                if (workTemplates != null && workTemplates.Any())
                {
                    await workTemplateRepo.InsertRangeAsync(workTemplates);
                }
            }

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

        public async Task<Package> UpdatePackage(PackageRequest packageRequest, Guid packageId)
        {
            if (packageRequest == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var packageRepo = _unitOfWork.GetRepository<Package>();
            var workTemplateRepo = _unitOfWork.GetRepository<WorkTemplate>();

            var existingPackage = await packageRepo.FirstOrDefaultAsync(
                predicate: p => p.Id == packageId,
                include: p => p.Include(pl => pl.PackageLabors)
                               .Include(pm => pm.PackageMaterials)
                               .Include(ph => ph.PackageHouses)
                               .Include(wt => wt.WorkTemplates)
            );

            if (existingPackage == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.PackageNotFound
                );
            }

            existingPackage.Type = packageRequest.PackageType;
            existingPackage.PackageName = packageRequest.PackageName;
            existingPackage.Unit = packageRequest.Unit;
            existingPackage.Price = packageRequest.Price;
            existingPackage.Status = packageRequest.Status;
            existingPackage.UpsDate = LocalDateTime.VNDateTime();

            existingPackage.PackageLabors = packageRequest.PackageLabors?.Select(pl => new PackageLabor
            {
                LaborId = pl.LaborId,
                PackageId = existingPackage.Id,
                InsDate = LocalDateTime.VNDateTime()
            }).ToList() ?? new List<PackageLabor>();

            existingPackage.PackageMaterials = packageRequest.PackageMaterials?.Select(pm => new PackageMaterial
            {
                MaterialId = pm.MaterialId,
                PackageId = existingPackage.Id,
                InsDate = LocalDateTime.VNDateTime()
            }).ToList() ?? new List<PackageMaterial>();
            if (packageRequest.PackageHouses != null)
            {
                existingPackage.PackageHouses = packageRequest.PackageHouses?.Select(ph => new PackageHouse
                {
                    DesignTemplateId = ph.DesignTemplateId,
                    ImgUrl = ph.ImgUrl,
                    Description = ph.Description,
                    PackageId = existingPackage.Id,
                    InsDate = LocalDateTime.VNDateTime()
                }).ToList() ?? new List<PackageHouse>();
            }

            if (packageRequest.WorkTemplate!= null)
            {
                var existingWorkTemplates = existingPackage.WorkTemplates.ToList();

                foreach (var workTemplateRequest in packageRequest.WorkTemplate)
                {
                    var existingWorkTemplate = existingWorkTemplates
                        .FirstOrDefault(wt => wt.ContructionWorkId == workTemplateRequest.ConstructionWorKid);

                    if (existingWorkTemplate != null)
                    {
                        existingWorkTemplate.LaborCost = workTemplateRequest.LaborCost;
                        existingWorkTemplate.MaterialCost = workTemplateRequest.MaterialCost;
                        existingWorkTemplate.MaterialFinishedCost = workTemplateRequest.MaterialFinishedCost;
                    }
                    workTemplateRepo.UpdateAsync(existingWorkTemplate);
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
                var normalizedSearch = name.RemoveDiacritics().ToLower();

                var packageItems = await _unitOfWork.GetRepository<Package>()
                    .GetListAsync(predicate: p => p.Status == AppConstant.General.Active);

                var filteredItems = packageItems
                    .Where(con =>
                        !string.IsNullOrEmpty(con.PackageName) &&
                        con.PackageName.RemoveDiacritics().ToLower().Contains(normalizedSearch) ||
                        !string.IsNullOrEmpty(con.Type) &&
                        con.Type.RemoveDiacritics().ToLower().Contains(normalizedSearch))
                    .ToList();

                return filteredItems.Select(packageItem => new AutoPackageResponse(
                    packageId: packageItem.Id,
                    packageName: packageItem.PackageName!,
                    type: packageItem.Type!,
                    price: packageItem.Price ?? 0
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> GeneratePackagePdf(Guid packageId)
        {
            var package = await GetPackageDetail(packageId);
            if (package == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.PackageNotFound);
            }

            try
            {

                var htmlContent = GeneratePackageHtmlContent(package);

                var doc = new HtmlToPdfDocument
                {
                    GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
                    Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = null }
                }
            }
                };

                string dllPath = Path.Combine(AppContext.BaseDirectory, "ExternalLibraries", "libwkhtmltox.dll");
                NativeLibrary.Load(dllPath);

                var pdf = _converter.Convert(doc);

                using (var pdfStream = new MemoryStream(pdf))
                {
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription($"{package.PackageName}_Details.pdf", pdfStream),
                        Folder = "PDFPackage",
                        PublicId = $"Package_{package.PackageName}_{package.Id}",
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                           AppConstant.ErrMessage.FailUploadPackagePdf);
                    }
                    return uploadResult.SecureUrl.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating PDF for package: " + ex.Message);
            }
        }

        private string GeneratePackageHtmlContent(PackageResponse package)
        {
            var sb = new StringBuilder();

            sb.Append($@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }}
        th, td {{
            border: 1px solid black;
            padding: 8px;
            text-align: left;
        }}
        thead {{
            display: table-header-group;
        }}
        tbody {{
            display: table-row-group;
        }}
        tr {{
            page-break-inside: avoid;
            page-break-after: auto;
        }}
        h1, h2 {{
            color: #333;
        }}
        .section-title {{
            margin-top: 30px;
            color: #2c3e50;
        }}
        
        @media print {{
            h2.section-title {{
                page-break-before: always;
                margin-top: 50px;
            }}
            table {{
                page-break-inside: auto;
            }}
            tr {{
                page-break-inside: avoid;
                page-break-after: auto;
            }}
            thead {{
                display: table-header-group;
            }}
            tbody {{
                display: table-row-group;
            }}
        }}
    </style>
</head>
<body>

<!-- Package Metadata -->
<h1>{package.PackageName}</h1>
<p><strong>Loại:</strong> {(
                    string.Equals(package.PackageType, "ROUGH", StringComparison.OrdinalIgnoreCase) ? "Thô" :
                    string.Equals(package.PackageType, "FINISHED", StringComparison.OrdinalIgnoreCase) ? "Hoàn thiện" :
                    package.PackageType)}</p>

<p><strong>Đơn vị:</strong> {package.Unit}</p>
<p><strong>Giá:</strong> {package.Price:N0} VND</p>");

            // Package Materials
            if (package.PackageMaterials != null && package.PackageMaterials.Any())
            {
                sb.Append($@"
    <h2 class='section-title'>Nguyên vật liệu</h2>
        <table>
            <thead>
                <tr>
                    <th>Mục Vật Liệu</th>
                    <th>Tên Vật Liệu</th>
                    <th>Đơn Vị</th>
                    <th>Giá</th>
                </tr>
            </thead>
            <tbody>");

                foreach (var material in package.PackageMaterials)
                {
                    sb.Append($@"
            <tr>
                <td>{material.MaterialSectionName}</td>
                <td>{material.MaterialName}</td>
                <td>{material.Unit}</td>
                <td>{material.Price:N0} VND</td>
            </tr>");
                }

                sb.Append($@"
        </tbody>
    </table>");
            }

            // Package Labors
            if (package.PackageLabors != null && package.PackageLabors.Any())
            {
                sb.Append($@"
    <h2 class='section-title'>Nhân công</h2>
        <table>
            <thead>
                <tr>
                    <th>Tên Công Việc</th>
                    <th>Loại</th>
                    <th>Thành Tiền</th>
                </tr>
            </thead>
            <tbody>");

                foreach (var labor in package.PackageLabors)
                {
                    var laborType = labor.Type == "Rough" ? "Thô" : labor.Type == "Finished" ? "Hoàn thiện" : labor.Type;

                    sb.Append($@"
            <tr>
                <td>{labor.NameOfLabor}</td>
                <td>{laborType}</td>
                <td>{labor.Price:N0} VND</td>
            </tr>");
                }

                sb.Append($@"
        </tbody>
    </table>");
            }

            sb.Append($@"
</body>
</html>");

            return sb.ToString();
        }
    }

}

