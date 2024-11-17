using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public MaterialService(IUnitOfWork unitOfWork, IUploadImgService uploadImgService,ILogger<MaterialService> logger)
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
                    MaterialSectionName = x.MaterialSection.Name,
                    SupplierName = x.Supplier.Name
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
                MaterialSectionName = material.MaterialSection.Name,
                SupplierName = material.Supplier.Name
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
                if ( materialSection == null || supplier == null)
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
                    IsAvailable = request.IsAvailable
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

        public async Task<List<MaterialResponse>> SearchMaterialByName(string name)
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
                    MaterialSectionName = x.MaterialSection.Name,
                    SupplierName = x.Supplier.Name
                },
                predicate: m => m.Name.Contains(name),
                include: m => m.Include(m => m.MaterialSection)
                               .Include(m => m.Supplier),
                orderBy: x => x.OrderBy(x => x.InsDate)
            ); 
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
                    MaterialSectionName = x.MaterialSection.Name,
                    SupplierName = x.Supplier.Name
                },
                predicate: m => m.MaterialSectionId == materialSectionId,
                include: m => m.Include(m => m.MaterialSection).Include(m => m.Supplier),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }
    }
}
