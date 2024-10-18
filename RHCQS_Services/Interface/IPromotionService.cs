using RHCQS_BusinessObject.Payload.Request.Promotion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IPromotionService
    {
        Task<IPaginate<PromotionResponse>> GetListPromotion(int page, int size);
        Task<List<PromotionResponse>> GetListPromotionVaild();
        Task<PromotionResponse> GetDetailPromotion(Guid promotionId);
        Task<List<PromotionResponse>> SearchPromotionByName(string promotionName);
        Task<bool> CreatePromotion(PromotionRequest request);
        Task<bool> UpdatePromotion(Guid promotionId, UpdatePromotionRequest request);
        Task<string> BanPromotion(Guid promotionId);
    }
}
