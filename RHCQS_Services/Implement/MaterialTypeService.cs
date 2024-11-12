using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
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
    public class MaterialTypeService : IMaterialTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MaterialTypeService> _logger;

        public MaterialTypeService(IUnitOfWork unitOfWork, ILogger<MaterialTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<MaterialTypeResponse>> GetListMaterialType(int page, int size)
        {
            return await _unitOfWork.GetRepository<MaterialType>().GetList(
                selector: x => new MaterialTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag
                },
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<MaterialTypeResponse> GetDetailMaterialType(Guid id)
        {
            var materialType = await _unitOfWork.GetRepository<MaterialType>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (materialType == null)
                return new MaterialTypeResponse();

            return new MaterialTypeResponse
            {
                Id = materialType.Id,
                Name = materialType.Name,
                InsDate = materialType.InsDate,
                UpsDate = materialType.UpsDate,
                Deflag = materialType.Deflag
            };
        }

        public async Task<bool> CreateMaterialType(MaterialTypeRequest request)
        {
            try
            {
                var newMaterialType = new MaterialType
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    Deflag = request.Deflag
                };
                await _unitOfWork.GetRepository<MaterialType>().InsertAsync(newMaterialType);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "An error while creating a new material type."
                );
            }
        }

        public async Task<bool> UpdateMaterialType(Guid id, MaterialTypeRequest request)
        {
            try
            {
                var materialType = await _unitOfWork.GetRepository<MaterialType>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (materialType == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Material type does not exist."
                    );
                }

                materialType.Name = request.Name ?? materialType.Name;
                materialType.Deflag = request.Deflag ?? materialType.Deflag;

                materialType.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<MaterialType>().UpdateAsync(materialType);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "An error while updating a new material type."
                );
            }
        }

        public async Task<IPaginate<MaterialTypeResponse>> SearchMaterialTypeByName(string name, int page, int size)
        {
            return await _unitOfWork.GetRepository<MaterialType>().GetList(
                selector: x => new MaterialTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }
    }
}
