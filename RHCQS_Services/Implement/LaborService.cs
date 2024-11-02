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
    public class LaborService : ILaborService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LaborService> _logger;

        public LaborService(IUnitOfWork unitOfWork, ILogger<LaborService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<LaborResponse>> GetListLabor(int page, int size)
        {
            return await _unitOfWork.GetRepository<Labor>().GetList(
                selector: x => new LaborResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    Type = x.Type
                },
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<LaborResponse> GetDetailLabor(Guid id)
        {
            var labor = await _unitOfWork.GetRepository<Labor>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (labor == null)
                return new LaborResponse();

            return new LaborResponse
            {
                Id = labor.Id,
                Name = labor.Name,
                Price = labor.Price,
                InsDate = labor.InsDate,
                UpsDate = labor.UpsDate,
                Deflag = labor.Deflag,
                Type = labor.Type
            };
        }

        public async Task<bool> CreateLabor(LaborRequest request)
        {
            try
            {
                var newLabor = new Labor
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Price = request.Price,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    Deflag = request.Deflag,
                    Type = request.Type
                };
                await _unitOfWork.GetRepository<Labor>().InsertAsync(newLabor);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xảy ra lỗi khi đang khởi tạo nhân công mới."
                );
            }
        }

        public async Task<bool> UpdateLabor(Guid id, LaborRequest request)
        {
            try
            {
                var labor = await _unitOfWork.GetRepository<Labor>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (labor == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Nhân công không tồn tại."
                    );
                }

                labor.Name = request.Name ?? labor.Name;
                labor.Price = request.Price.HasValue ? (double)request.Price.Value : labor.Price;
                labor.Deflag = request.Deflag ?? labor.Deflag;
                labor.Type = request.Type ?? labor.Type;

                labor.UpsDate = DateTime.Now;

                _unitOfWork.GetRepository<Labor>().UpdateAsync(labor);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Có lỗi xảy ra khi cập nhật nhân công."
                );
            }
        }

        public async Task<IPaginate<LaborResponse>> SearchLaborByName(string name, int page, int size)
        {
            return await _unitOfWork.GetRepository<Labor>().GetList(
                selector: x => new LaborResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    Type = x.Type
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }
    }
}
