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
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(IUnitOfWork unitOfWork, ILogger<SupplierService> logger)
        {
            _unitOfWork = unitOfWork;
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
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    Deflag = request.Deflag,
                    ShortDescription = request.ShortDescription,
                    Description = request.Description
                };
                await _unitOfWork.GetRepository<Supplier>().InsertAsync(newSupplier);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "An error while creating a new supplier."
                );
            }
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
                Description = supplier.Description
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
                    Description = x.Description
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
                    Description = x.Description
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }

        public async Task<bool> UpdateSupplier(Guid id, SupplierRequest request)
        {
            try
            {
                var supplier = await _unitOfWork.GetRepository<Supplier>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (supplier == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Supplier does not exist."
                    );
                }

                supplier.Name = request.Name ?? supplier.Name;
                supplier.Email = request.Email ?? supplier.Email;
                supplier.ConstractPhone = request.ConstractPhone ?? supplier.ConstractPhone;
                supplier.ImgUrl = request.ImgUrl ?? supplier.ImgUrl;
                supplier.Deflag = request.Deflag ?? supplier.Deflag;
                supplier.ShortDescription = request.ShortDescription ?? supplier.ShortDescription;
                supplier.Description = request.Description ?? supplier.Description;

                supplier.UpsDate = DateTime.Now;

                _unitOfWork.GetRepository<Supplier>().UpdateAsync(supplier);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "An error while updating a supplier."
                );
            }
        }
    }
}
