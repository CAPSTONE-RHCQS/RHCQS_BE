using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Mate;
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
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadImgService _uploadImgService;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(IUnitOfWork unitOfWork, IUploadImgService uploadImgService, ILogger<MaterialService> logger)
        {
            _unitOfWork = unitOfWork;
            _uploadImgService = uploadImgService;
            _logger = logger;
        }

        public async Task<IPaginate<MaterialResponse>> GetListMaterial(int page, int size)
        {
            return await _unitOfWork.GetRepository<Material>().GetList(
                selector: x => new MaterialResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Unit = x.Unit,
                    Size = x.Size,
                    Shape = x.Shape,
                    ImgUrl = x.ImgUrl,
                    Description = x.Description,
                    IsAvailable = x.IsAvailable,
                    UnitPrice = x.UnitPrice,
                    MaterialSectionId = x.MaterialSectionId,
                    SupplierId = x.SupplierId,
                    MaterialSectionType = x.MaterialSection.Type,
                    SupplierName = x.Supplier.Name,
                    Code = x.Code
                },
                include: m => m.Include(m => m.MaterialSection).Include(m => m.Supplier),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<MaterialResponse> GetDetailMaterial(Guid id)
        {
            var material = await _unitOfWork.GetRepository<Material>().FirstOrDefaultAsync(
                predicate: m => m.Id == id,
                include: m => m.Include(m => m.MaterialSection).Include(m => m.Supplier));
            if (material == null)
                return new MaterialResponse();

            return new MaterialResponse
            {
                Id = material.Id,
                Name = material.Name,
                Price = material.Price,
                Unit = material.Unit,
                Size = material.Size,
                Shape = material.Shape,
                ImgUrl = material.ImgUrl,
                Description = material.Description,
                IsAvailable = material.IsAvailable,
                UnitPrice = material.UnitPrice,
                MaterialSectionId = material.MaterialSectionId,
                SupplierId = material.SupplierId,
                MaterialSectionType = material.MaterialSection.Type,
                SupplierName = material.Supplier.Name,
                Code = material.Code
            };
        }

        public async Task<bool> CreateMaterial(MaterialRequest request)
        {
            try
            {

                var materialSection = await _unitOfWork.GetRepository<MaterialSection>()
                    .FirstOrDefaultAsync(ms => ms.Id == request.MaterialSectionId);

                var supplier = await _unitOfWork.GetRepository<Supplier>()
                    .FirstOrDefaultAsync(s => s.Id == request.SupplierId);
                if (materialSection == null || supplier == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        "MaterialSection or Supplier does not exist."
                    );
                }

                var newMaterial = new Material
                {
                    Id = Guid.NewGuid(),
                    SupplierId = (Guid)request.SupplierId,
                    MaterialSectionId = request.MaterialSectionId,
                    Name = request.Name,
                    Price = request.Price,
                    Unit = request.Unit,
                    Size = request.Size,
                    Shape = request.Shape,
                    ImgUrl = request.ImgUrl,
                    Description = request.Description,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    UnitPrice = request.UnitPrice,
                    IsAvailable = request.IsAvailable,
                    Code = request.Code
                };
                await _unitOfWork.GetRepository<Material>().InsertAsync(newMaterial);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "An error occurred while creating the material."
                );
            }
        }

        public async Task<string> UploadMaterialImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    "No image file uploaded."
                );
            }

            var imageUrl = await _uploadImgService.UploadImage(image, "Material");
            return imageUrl;
        }

        public async Task<bool> UpdateMaterial(Guid id, MaterialUpdateRequest request)
        {
            try
            {
                var material = await _unitOfWork.GetRepository<Material>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (material == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Material không tồn tại."
                    );
                }

                if (request.Image != null && request.Image.Length > 0)
                {
                    var imageUrl = await _uploadImgService.UploadImage(request.Image, "Material");
                    material.ImgUrl = imageUrl;
                }

                material.Name = request.Name ?? material.Name;
                material.Price = request.Price.HasValue ? (double)request.Price.Value : material.Price;
                material.Unit = request.Unit ?? material.Unit;
                material.Size = request.Size ?? material.Size;
                material.Shape = request.Shape ?? material.Shape;
                material.ImgUrl = request.ImgUrl ?? material.ImgUrl;
                material.Description = request.Description ?? material.Description;
                material.SupplierId = request.SupplierId ?? material.SupplierId;
                material.MaterialSectionId = request.MaterialSectionId ?? material.MaterialSectionId;
                material.UnitPrice = request.UnitPrice ?? material.UnitPrice;
                material.IsAvailable = request.IsAvailable ?? material.IsAvailable;

                material.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<Material>().UpdateAsync(material);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Có lỗi xảy ra khi cập nhật material."
                );
            }
        }

        public async Task<List<MaterialResponse>> SearchMaterialByName(Guid packageId, string name)
        {
            var result = (await _unitOfWork.GetRepository<PackageMaterial>()
                .GetListAsync(
                    selector: x => new MaterialResponse
                    {
                        Id = x.Material.Id,
                        Name = x.Material.Name,
                        Price = x.Material.Price,
                        Unit = x.Material.Unit,
                        Size = x.Material.Size,
                        Shape = x.Material.Shape,
                        ImgUrl = x.Material.ImgUrl,
                        Description = x.Material.Description,
                        IsAvailable = x.Material.IsAvailable,
                        UnitPrice = x.Material.UnitPrice,
                        MaterialSectionId = x.Material.MaterialSectionId,
                        SupplierId = x.Material.SupplierId,
                        MaterialSectionType = x.Material.MaterialSection.Type,
                        SupplierName = x.Material.Supplier.Name,
                        Code = x.Material.Code
                    },
                predicate: m => m.PackageDetail.PackageId == packageId && m.Material.Name.Contains(name),
                include: m => m.Include(m => m.Material.MaterialSection)
                               .Include(m => m.Material.Supplier),
                orderBy: x => x.OrderBy(x => x.Material.InsDate)
            )).ToList();
            return result;
        }

        public async Task<List<MaterialResponse>> FilterMaterialBySection(Guid materialSectionId)
        {
            return (List<MaterialResponse>)await _unitOfWork.GetRepository<Material>().GetListAsync(
                selector: x => new MaterialResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Unit = x.Unit,
                    Size = x.Size,
                    Shape = x.Shape,
                    ImgUrl = x.ImgUrl,
                    Description = x.Description,
                    IsAvailable = x.IsAvailable,
                    UnitPrice = x.UnitPrice,
                    MaterialSectionId = x.MaterialSectionId,
                    SupplierId = x.SupplierId,
                    MaterialSectionType = x.MaterialSection.Type,
                    SupplierName = x.Supplier.Name,
                    Code = x.Code
                },
                predicate: m => m.MaterialSectionId == materialSectionId,
                include: m => m.Include(m => m.MaterialSection).Include(m => m.Supplier),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }

        public async Task<bool> ImportMaterialFromExcel(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null || excelFile.Length == 0)
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, "No file uploaded");

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var newCodes = new HashSet<string>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var code = worksheet.Cells[row, 10].Value?.ToString();

                            if (await _unitOfWork.GetRepository<Material>().AnyAsync(l => l.Code == code) || newCodes.Contains(code))
                            {
                                throw new AppConstant.MessageError(
                                    (int)AppConstant.ErrCode.Bad_Request,
                                    $"Duplicate code found: {code}. Please upload another file."
                                );
                            }

                            newCodes.Add(code);

                            var supplierIdValue = worksheet.Cells[row, 1].Value?.ToString();
                            if (!Guid.TryParse(supplierIdValue, out Guid supplierId))
                            {
                                throw new AppConstant.MessageError(
                                    (int)AppConstant.ErrCode.Bad_Request,
                                    $"Invalid SupplierId format at row {row}. Please check the file."
                                );
                            }

                            var materialsectionIdValue = worksheet.Cells[row, 9].Value?.ToString();
                            if (!Guid.TryParse(materialsectionIdValue, out Guid materialsectionId))
                            {
                                throw new AppConstant.MessageError(
                                    (int)AppConstant.ErrCode.Bad_Request,
                                    $"Invalid SupplierId format at row {row}. Please check the file."
                                );
                            }

                            var material = new Material
                            {
                                Id = Guid.NewGuid(),
                                SupplierId = supplierId,
                                Name = worksheet.Cells[row, 2].Value?.ToString(),
                                Price = Convert.ToDouble(worksheet.Cells[row, 3].Value ?? 0),
                                Unit = worksheet.Cells[row, 4].Value?.ToString(),
                                Size = worksheet.Cells[row, 5].Value?.ToString(),
                                Shape = worksheet.Cells[row, 6].Value?.ToString(),
                                Description = worksheet.Cells[row, 7].Value?.ToString(),
                                UnitPrice = worksheet.Cells[row, 8].Value?.ToString(),
                                MaterialSectionId = materialsectionId,
                                Code = code,
                                IsAvailable = true,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime()
                            };

                            await _unitOfWork.GetRepository<Material>().InsertAsync(material);
                        }


                        return await _unitOfWork.CommitAsync() > 0;
                    }
                }
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, $"Duplicate code found. Please upload another file.");
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, $"Error while importing data: {ex.Message}");
            }
        }
    }
}
