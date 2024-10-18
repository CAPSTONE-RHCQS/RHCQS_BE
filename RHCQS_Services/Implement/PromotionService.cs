using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
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
                                selector: p => new PromotionResponse(p.Id, p.Name, p.Code, p.Value, p.InsDate, p.StartTime, p.ExpTime),
                                page: page,
                                size: size
                                );
            return promotionList;
        }

        public async Task<PromotionResponse> GetDetailPromotion(Guid promotionId)
        {
            var promotionItem = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(
                                predicate: p => p.Id == promotionId);
            return new PromotionResponse(promotionItem.Id, promotionItem.Name, promotionItem.Code, promotionItem.Value,
                                         promotionItem.InsDate, promotionItem.StartTime, promotionItem.ExpTime);
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
                promotionItem.ExpTime
            )).ToList();
        }

        public async Task<bool> CreatePromotion(PromotionRequest request)
        {
            if (request.StartTime <= DateTime.Now)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invail_Start_Time);
            }

            //Note: Exp time - Start time must > 2 days 
            if ((request.ExpTime - request.StartTime)?.TotalDays < 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invail_Distance_Time);
            }
            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Code = GenerateRandom.GenerateRandomString(10),
                Value = request.Value,
                InsDate = DateTime.Now,
                StartTime = request.StartTime,
                Name = request.Name,
                ExpTime = request.ExpTime
            };

            await _unitOfWork.GetRepository<Promotion>().InsertAsync(promotion);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return true;
        }
    }
}
