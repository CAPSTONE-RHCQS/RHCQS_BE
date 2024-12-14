using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Sup;
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
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadImgService _uploadImgService;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(IUnitOfWork unitOfWork, IUploadImgService uploadImgService, ILogger<SupplierService> logger)
        {
            _unitOfWork = unitOfWork;
            _uploadImgService = uploadImgService;
            _logger = logger;
        }

        public async Task<bool> CreateSupplier(SupplierRequest request)
        {
            try
            {       
                var newSupplier = new Supplier
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    ConstractPhone = request.ConstractPhone,
                    ImgUrl = request.ImgUrl,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    Deflag = request.Deflag,
                    ShortDescription = request.ShortDescription,
                    Description = request.Description,
                    Code = request.Code
                };
                await _unitOfWork.GetRepository<Supplier>().InsertAsync(newSupplier);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi tạo mới nhà cung cấp."
                );
            }
        }

        public async Task<string> UploadSupplierImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    "Không tìm thấy ảnh được tải."
                );
            }

            var imageUrl = await _uploadImgService.UploadImage(image, "Supplier");
            return imageUrl;
        }

        public async Task<SupplierResponse> GetDetailSupplier(Guid id)
        {
            var supplier = await _unitOfWork.GetRepository<Supplier>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (supplier == null)
                return new SupplierResponse();

            return new SupplierResponse
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                ConstractPhone = supplier.ConstractPhone,
                ImgUrl = supplier.ImgUrl,
                InsDate = supplier.InsDate,
                UpsDate = supplier.UpsDate,
                Deflag = supplier.Deflag,
                ShortDescription = supplier.ShortDescription,
                Description = supplier.Description,
                Code = supplier.Code
            };
        }

        public async Task<IPaginate<SupplierResponse>> GetListSupplier(int page, int size)
        {
            return await _unitOfWork.GetRepository<Supplier>().GetList(
                selector: x => new SupplierResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ConstractPhone = x.ConstractPhone,
                    ImgUrl = x.ImgUrl,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    Code = x.Code
                },
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<List<SupplierResponse>> SearchSupplierByName(string name)
        {
            return (List<SupplierResponse>)await _unitOfWork.GetRepository<Supplier>().GetListAsync(
                selector: x => new SupplierResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ConstractPhone = x.ConstractPhone,
                    ImgUrl = x.ImgUrl,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    Code = x.Code
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }
        public async Task<IPaginate<SupplierResponse>> SearchSupplierByNameWithPag(string? name, int page, int size)
        {
            return await _unitOfWork.GetRepository<Supplier>().GetList(
                selector: x => new SupplierResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ConstractPhone = x.ConstractPhone,
                    ImgUrl = x.ImgUrl,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    Code = x.Code
                },
                predicate: x => x.Name.Contains(name) || string.IsNullOrWhiteSpace(name),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<bool> UpdateSupplier(Guid id, SupplierUpdateRequest request)
        {
            try
            {
                var supplier = await _unitOfWork.GetRepository<Supplier>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (supplier == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Nhà cung cấp không tồn tại."
                    );
                }

                if (request.Image != null && request.Image.Length > 0)
                {
                    var imageUrl = await _uploadImgService.UploadImage(request.Image, "Supplier");
                    supplier.ImgUrl = imageUrl;
                }

                supplier.Name = request.Name ?? supplier.Name;
                supplier.Email = request.Email ?? supplier.Email;
                supplier.ConstractPhone = request.ConstractPhone ?? supplier.ConstractPhone;
                supplier.Deflag = request.Deflag ?? supplier.Deflag;
                supplier.ShortDescription = request.ShortDescription ?? supplier.ShortDescription;
                supplier.Description = request.Description ?? supplier.Description;

                supplier.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<Supplier>().UpdateAsync(supplier);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi tạo mới nhà cung cấp."
                );
            }
        }
    }
}
