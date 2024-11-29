using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Request.MateSec;
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
    public class MaterialSectionService : IMaterialSectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MaterialSectionService> _logger;

        public MaterialSectionService(IUnitOfWork unitOfWork, ILogger<MaterialSectionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<MaterialSectionResponse>> GetListMaterialSection(int page, int size)
        {
            return await _unitOfWork.GetRepository<MaterialSection>().GetList(
                selector: x => new MaterialSectionResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    InsDate = x.InsDate,
                    Code = x.Code/*,
                    Type = x.Type*/
                },
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<MaterialSectionResponse> GetDetailMaterialSection(Guid id)
        {
            var labor = await _unitOfWork.GetRepository<MaterialSection>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (labor == null)
                return new MaterialSectionResponse();

            return new MaterialSectionResponse
            {
                Id = labor.Id,
                Name = labor.Name,
                InsDate = labor.InsDate,
                Code = labor.Code/*,
                Type = labor.Type*/
            };
        }

        public async Task<bool> CreateMaterialSection(MaterialSectionRequest request)
        {
            try
            {
                var newMaterialSection = new MaterialSection
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    InsDate = LocalDateTime.VNDateTime(),
                    Code = request.Code/*,
                    Type = request.Type*/
                };
                await _unitOfWork.GetRepository<MaterialSection>().InsertAsync(newMaterialSection);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi tạo mới loại vật liệu."
                );
            }
        }

        public async Task<bool> UpdateMaterialSection(Guid id, MaterialSectionUpdateRequest request)
        {
            try
            {
                var materialSection = await _unitOfWork.GetRepository<MaterialSection>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (materialSection == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Loại vật liệu không tồn tại."
                    );
                }

                materialSection.Name = request.Name ?? materialSection.Name;

                _unitOfWork.GetRepository<MaterialSection>().UpdateAsync(materialSection);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi cập nhật loại vật liệu."
                );
            }
        }

        public async Task<List<MaterialSectionResponse>> SearchMaterialSectionByName(string name)
        {
            return (List<MaterialSectionResponse>)await _unitOfWork.GetRepository<MaterialSection>().GetListAsync(
                selector: x => new MaterialSectionResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    InsDate = x.InsDate,
                    Code = x.Code/*,
                    Type = x.Type*/
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }
    }
}
