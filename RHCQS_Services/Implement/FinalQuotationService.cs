using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class FinalQuotationService : IFinalQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FinalQuotationService> _logger;

        public FinalQuotationService(IUnitOfWork unitOfWork, ILogger<FinalQuotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByCustomerName(string name)
        {
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                x => x.Project.Customer.Username.Equals(name),
                include: x => x.Include(x => x.Project)
                                .ThenInclude(x => x.Customer)
                               .Include(x => x.Promotion)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesItem)
                               .Include(x => x.EquipmentItems)
                               .Include(x => x.FinalQuotationItems)
                               .Include(x => x.BatchPayments)
            );

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.Not_Found_FinalQuotaion);
            }

            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.Customer.Username,
                finalQuotation.ProjectId,
                finalQuotation.PromotionId,
                finalQuotation.TotalPrice,
                finalQuotation.Note,
                finalQuotation.Version,
                finalQuotation.InsDate,
                finalQuotation.UpsDate,
                finalQuotation.Status,
                finalQuotation.Deflag,
                finalQuotation.QuotationUtilitiesId,
                finalQuotation.AccountId,
                finalQuotation.ReasonReject,
                finalQuotation.BatchPayments.Select(bp => new BatchPaymentInfo(
                    bp.Id,
                    bp.Description,
                    bp.Percents,
                    bp.Price,
                    bp.Unit
                )).ToList(),
                finalQuotation.EquipmentItems.Select(ei => new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note
                )).ToList(),
                finalQuotation.FinalQuotationItems.Select(fqi => new FinalQuotationItemResponse(
                    fqi.Id,
                    fqi.Name,
                    fqi.Unit,
                    fqi.Weight,
                    fqi.UnitPriceLabor,
                    fqi.UnitPriceRough,
                    fqi.UnitPriceFinished,
                    fqi.TotalPriceLabor,
                    fqi.TotalPriceRough,
                    fqi.TotalPriceFinished,
                    fqi.InsDate
                )).ToList(),
                finalQuotation.Promotion != null ? new PromotionInfo(
                    finalQuotation.Promotion.Id,
                    finalQuotation.Promotion.Name,
                    finalQuotation.Promotion.Value
                ) : null,
                finalQuotation.QuotationUtilities != null ? new List<UtilityInfo> {
                    new UtilityInfo(
                    finalQuotation.QuotationUtilities.Id,
                    finalQuotation.QuotationUtilities.Description,
                    finalQuotation.QuotationUtilities.Coefiicient ?? 0,
                    finalQuotation.QuotationUtilities.Price ?? 0
                )} : new List<UtilityInfo>()
            );

            return response;
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationById(Guid id)
        {
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                x => x.Id == id,
                include: x => x.Include(x => x.Project)
                                .ThenInclude(x => x.Customer)
                               .Include(x => x.Promotion)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesItem)
                               .Include(x => x.EquipmentItems)
                               .Include(x => x.FinalQuotationItems)
                               .Include(x => x.BatchPayments)
            );

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.Not_Found_FinalQuotaion);
            }

            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.Customer.Username,
                finalQuotation.ProjectId,
                finalQuotation.PromotionId,
                finalQuotation.TotalPrice,
                finalQuotation.Note,
                finalQuotation.Version,
                finalQuotation.InsDate,
                finalQuotation.UpsDate,
                finalQuotation.Status,
                finalQuotation.Deflag,
                finalQuotation.QuotationUtilitiesId,
                finalQuotation.AccountId,
                finalQuotation.ReasonReject,
                finalQuotation.BatchPayments.Select(bp => new BatchPaymentInfo(
                    bp.Id,
                    bp.Description,
                    bp.Percents,
                    bp.Price,
                    bp.Unit
                )).ToList(),
                finalQuotation.EquipmentItems.Select(ei => new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note
                )).ToList(),
                finalQuotation.FinalQuotationItems.Select(fqi => new FinalQuotationItemResponse(
                    fqi.Id,
                    fqi.Name,
                    fqi.Unit,
                    fqi.Weight,
                    fqi.UnitPriceLabor,
                    fqi.UnitPriceRough,
                    fqi.UnitPriceFinished,
                    fqi.TotalPriceLabor,
                    fqi.TotalPriceRough,
                    fqi.TotalPriceFinished,
                    fqi.InsDate
                )).ToList(),
                finalQuotation.Promotion != null ? new PromotionInfo(
                    finalQuotation.Promotion.Id,
                    finalQuotation.Promotion.Name,
                    finalQuotation.Promotion.Value
                ) : null,
                finalQuotation.QuotationUtilities != null ? new List<UtilityInfo> {
                    new UtilityInfo(
                    finalQuotation.QuotationUtilities.Id,
                    finalQuotation.QuotationUtilities.Description,
                    finalQuotation.QuotationUtilities.Coefiicient ?? 0,
                    finalQuotation.QuotationUtilities.Price ?? 0
                )} : new List<UtilityInfo>()
            );

            return response;
        }


        public async Task<IPaginate<FinalQuotationListResponse>> GetListFinalQuotation(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<FinalQuotation>().GetList(
                selector: x => new FinalQuotationListResponse(x.Id, x.Project.Customer.Username, x.Version, x.Status),
                include: x => x.Include(x => x.Project)
                                .ThenInclude(x => x.Customer!),
                page: page,
                size: size
                );
            return list;
        }
    }
}
