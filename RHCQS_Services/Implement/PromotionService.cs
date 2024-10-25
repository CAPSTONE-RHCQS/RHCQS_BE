using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Promotion;
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
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPromotionService> _logger;

        public PromotionService(IUnitOfWork unitOfWork, ILogger<IPromotionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<PromotionResponse>> GetListPromotion(int page, int size)
        {
            var promotionList = await _unitOfWork.GetRepository<Promotion>().GetList(
                                selector: p => new PromotionResponse(p.Id, p.Name, p.Code, p.Value, p.InsDate, p.StartTime, p.ExpTime, p.IsRunning),
                                page: page,
                                size: size
                                );
            return promotionList;
        }

        public async Task<List<PromotionResponse>> GetListPromotionVaild()
        {
            var promotionList = await _unitOfWork.GetRepository<Promotion>().GetListAsync(
                                predicate: p => p.IsRunning == true,
                                selector: p => new PromotionResponse(p.Id, p.Name, p.Code, p.Value, p.InsDate, p.StartTime, p.ExpTime, p.IsRunning)
                                );
            return promotionList.ToList();
        }

        public async Task<PromotionResponse> GetDetailPromotion(Guid promotionId)
        {
            var promotionItem = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(
                                predicate: p => p.Id == promotionId);
            return new PromotionResponse(promotionItem.Id, promotionItem.Name, promotionItem.Code, promotionItem.Value,
                                         promotionItem.InsDate, promotionItem.StartTime, promotionItem.ExpTime, promotionItem.IsRunning);
        }

        public async Task<List<PromotionResponse>> SearchPromotionByName(string promotionName)
        {
            var promotionList = await _unitOfWork.GetRepository<Promotion>().GetListAsync(
       predicate: p => p.Name!.ToLower().Contains(promotionName.ToLower()));

            if (promotionList == null || !promotionList.Any())
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_Promotion);
            }

            return promotionList.Select(promotionItem => new PromotionResponse(
                promotionItem.Id,
                promotionItem.Name,
                promotionItem.Code,
                promotionItem.Value,
                promotionItem.InsDate,
                promotionItem.StartTime,
                promotionItem.ExpTime,
                promotionItem.IsRunning
            )).ToList();
        }

        public async Task<bool> CreatePromotion(PromotionRequest request)
        {
            if (request.StartTime <= DateTime.Now)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Start_Time);
            }

            //Note: Exp time - Start time must > 2 days 
            if ((request.ExpTime - request.StartTime)?.TotalDays < 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Distance_Time);
            }
            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Code = GenerateRandom.GenerateRandomString(10),
                Value = request.Value,
                InsDate = DateTime.Now,
                StartTime = request.StartTime,
                Name = request.Name,
                ExpTime = request.ExpTime,
                IsRunning = true,
                Unit = AppConstant.Unit.UnitPriceD
            };

            await _unitOfWork.GetRepository<Promotion>().InsertAsync(promotion);

            var packageInfo = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(x => x.Id == request.PackageId);
            if (packageInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.PackageNotFound);
            }
            var mapPackage = new PackageMapPromotion
            {
                Id = Guid.NewGuid(),
                PackageId = request.PackageId,
                PromotionId = promotion.Id,
                InsDate = DateTime.Now
            };
            await _unitOfWork.GetRepository<PackageMapPromotion>().InsertAsync(mapPackage);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return true;
        }

        public async Task<bool> UpdatePromotion(Guid promotionId, UpdatePromotionRequest request)
        {
            var promotionInfo = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(p => p.Id == promotionId);
            if (promotionInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_Promotion);
            }

            if (promotionInfo.IsRunning == true)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Promotion_No_Update);
            }

            if (request.StartTime.HasValue && request.StartTime < DateTime.Now)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Start_Time);
            }

            if (request.ExpTime.HasValue && request.ExpTime < DateTime.Now)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Exp_Time);
            }

            if (request.StartTime.HasValue && request.ExpTime.HasValue &&
                (request.ExpTime - request.StartTime)?.TotalDays < 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Distance_Time);
            }

            if (request.StartTime.HasValue && request.ExpTime == null && (promotionInfo.ExpTime - request.StartTime)?.TotalDays < 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Distance_Time);
            }

            if (request.ExpTime.HasValue && request.StartTime == null && (request.ExpTime - promotionInfo.StartTime)?.TotalDays < 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invalid_Distance_Time);
            }

            // Cập nhật thông tin khuyến mãi
            promotionInfo.Name = request.Name ?? promotionInfo.Name;
            promotionInfo.Value = request.Value ?? promotionInfo.Value;
            promotionInfo.StartTime = request.StartTime ?? promotionInfo.StartTime;
            promotionInfo.ExpTime = request.ExpTime ?? promotionInfo.ExpTime;

            _unitOfWork.GetRepository<Promotion>().UpdateAsync(promotionInfo);

            bool isUpdate = await _unitOfWork.CommitAsync() > 0;
            return isUpdate;
        }

        public async Task<string> BanPromotion(Guid promotionId)
        {
            var promotionInfo = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(p => p.Id == promotionId);
            if (promotionInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_Promotion);
            }
            promotionInfo.IsRunning = false;
            _unitOfWork.GetRepository<Promotion>().UpdateAsync(promotionInfo);

            string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Internal_Server_Error;
            return result;
        }
    }
}
