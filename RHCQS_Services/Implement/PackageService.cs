using CloudinaryDotNet.Actions;
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
                package.PackageTypeId,
                package.PackageName,
                package.Unit,
                package.Price,
                package.Status,
                package.InsDate,
                package.UpsDate,
                package.PackageDetails?.Select(pd => new PackageDetailsResponse(
                    pd.Id,
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
                InsDate = LocalDateTime.VNDateTime(),
                PackageDetails = packageRequest.PackageDetails.Select(pd => new PackageDetail
                {
                    Id = Guid.NewGuid(),
                    Type = pd.Type,
                    InsDate = LocalDateTime.VNDateTime(),
                    PackageLabors = pd.PackageLabors?.Select(pl => new PackageLabor
                    {
                        Id = Guid.NewGuid(),
                        LaborId = pl.LaborId,
                        Price = pl.TotalPrice,
                        InsDate = LocalDateTime.VNDateTime(),
                    }).ToList(),
                    PackageMaterials = pd.PackageMaterials?.Select(pm => new PackageMaterial
                    {
                        Id = Guid.NewGuid(),
                        MaterialSectionId = pm.MaterialSectionId,
                        InsDate = LocalDateTime.VNDateTime()
                    }).ToList()
                }).ToList(),
                PackageHouses = packageRequest.PackageHouses?.Select(ph => new PackageHouse
                {
                    Id = Guid.NewGuid(),
                    DesignTemplateId = ph.DesignTemplateId,
                    ImgUrl = ph.ImgUrl,
                    Description = ph.Description,
                    InsDate = LocalDateTime.VNDateTime()
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
            existingPackage.UpsDate = LocalDateTime.VNDateTime();

            foreach (var pd in packageRequest.PackageDetails)
            {
                var existingPackageDetail = existingPackage.PackageDetails.FirstOrDefault(detail => detail.PackageId == packageid);

                if (existingPackageDetail != null)
                {
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
                var normalizedSearch = name.RemoveDiacritics().ToLower();

                var packageItems = await _unitOfWork.GetRepository<Package>()
                    .GetListAsync(predicate: p => p.Status == AppConstant.General.Active,
                                  include: p => p.Include(p => p.PackageType));

                var filteredItems = packageItems
                    .Where(con =>
                        !string.IsNullOrEmpty(con.PackageName) &&
                        con.PackageName.RemoveDiacritics().ToLower().Contains(normalizedSearch) ||
                        !string.IsNullOrEmpty(con.PackageType.Name) &&
                        con.PackageType.Name.RemoveDiacritics().ToLower().Contains(normalizedSearch))
                    .ToList();

                return filteredItems.Select(packageItem => new AutoPackageResponse(
                    packageId: packageItem.Id,
                    packageName: packageItem.PackageName!,
                    type: packageItem.PackageType.Name!,
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
            // Retrieve package details by ID
            var package = await GetPackageDetail(packageId);
            if (package == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.PackageNotFound);
            }

            try
            {
                // Step 1: Generate HTML content based on package details
                var htmlContent = GeneratePackageHtmlContent(package); // Implement this method to format the package details as HTML

                // Step 2: Configure and convert HTML to PDF
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

/*                string dllPath = Path.Combine(AppContext.BaseDirectory, "ExternalLibraries", "libwkhtmltox.dll");
                NativeLibrary.Load(dllPath);*/

                var pdf = _converter.Convert(doc);

                // Step 3: Upload PDF to Cloudinary
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

                    // Optionally store the PDF URL in your database
                    // ...

                    // Step 4: Return the Cloudinary URL of the uploaded PDF
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
            string.Equals(package.PackageType?.Name, "ROUGH", StringComparison.OrdinalIgnoreCase) ? "Thô" :
            string.Equals(package.PackageType?.Name, "FINISHED", StringComparison.OrdinalIgnoreCase) ? "Hoàn thiện" :
            package.PackageType?.Name)}</p>

<p><strong>Đơn vị:</strong> {package.Unit}</p>
<p><strong>Giá:</strong> {package.Price:N0} VND</p>");

            // Package Details - Materials
            if (package.PackageDetails != null && package.PackageDetails.Any(detail => detail.PackageMaterials != null && detail.PackageMaterials.Any()))
            {
                sb.Append($@"
    <h2 class='section-title'>Nguyên vật liệu</h2>");

                foreach (var detail in package.PackageDetails)
                {
                    if (detail.PackageMaterials != null && detail.PackageMaterials.Any())
                    {
                        sb.Append($@"
            <table>
                <thead>
                    <tr>
                        <th>Mục Vật Liệu</th>
                        <th>Tên Vật Liệu</th>
                        <th>Đơn Vị</th>
                        <th>Giá</th>
                        <th>Kích Thước</th>
                        <th>Hình Dạng</th>
                    </tr>
                </thead>
                <tbody>");

                        foreach (var material in detail.PackageMaterials)
                        {
                            sb.Append($@"
                <tr>
                    <td>{material.MaterialSectionName}</td>
                    <td>{material.MaterialName}</td>
                    <td>{material.Unit}</td>
                    <td>{material.Price:N0} VND</td>
                    <td>{material.Size}</td>
                    <td>{material.Shape}</td>
                </tr>");
                        }

                        sb.Append($@"
            </tbody>
        </table>");
                    }
                }
            }

            // Package Details - Labor
            if (package.PackageDetails != null && package.PackageDetails.Any(detail => detail.PackageLabors != null && detail.PackageLabors.Any()))
            {
                sb.Append($@"
    <h2 class='section-title'>Nhân công</h2>");

                foreach (var detail in package.PackageDetails)
                {
                    if (detail.PackageLabors != null && detail.PackageLabors.Any())
                    {
                        sb.Append($@"
            <table>
                <thead>
                    <tr>
                        <th>Tên Công Việc</th>
                        <th>Loại</th>
                        <th>Thành Tiền</th>
                    </tr>
                </thead>
                <tbody>");

                        foreach (var labor in detail.PackageLabors)
                        {
                            var laborType = labor.Type == "Rough" ? "Thô" : labor.Type == "Finished" ? "Hoàn thiện" : labor.Type;

                            sb.Append($@"
                <tr>
                    <td>{labor.NameOfLabor}</td>
                    <td>{laborType}</td>
                    <td>{labor.TotalPrice:N0} VND</td>
                </tr>");
                        }

                        sb.Append($@"
            </tbody>
        </table>");
                    }
                }
            }

            sb.Append($@"
</body>
</html>");

            return sb.ToString();
        }

    }

}

