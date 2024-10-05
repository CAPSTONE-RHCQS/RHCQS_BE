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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class InitialQuotationService : IInitialQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InitialQuotationService> _logger;

        public InitialQuotationService(IUnitOfWork unitOfWork, ILogger<InitialQuotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<InitialQuotationListResponse>> GetListInitialQuotation(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<InitialQuotation>().GetList(
                selector: x => new InitialQuotationListResponse(x.Id, x.Project.Customer.Username, x.Version, x.Area, x.Status),
                include: x => x.Include(x => x.Project)
                                .ThenInclude(x => x.Customer!),
                page: page,
                size: size
                );
            return list;
        }

        public async Task<InitialQuotationResponse> GetDetailInitialQuotationById(Guid id)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.Id.Equals(id),
                        include: x => x.Include(x => x.InitialQuotationItems)
                                       .ThenInclude(x => x.ConstructionItem)
                                       .ThenInclude(x => x.SubConstructionItems!)
                                       .Include(x => x.Project)
                                       .ThenInclude(x => x.Customer!)
                                       .Include(x => x.PackageQuotations)
                                       .ThenInclude(x => x.Package)
                                       .Include(x => x.Promotion)
                                       .Include(x => x.BactchPayments)
                );

            var promo = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(x => x.Id == initialQuotation.PromotionId);
            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "ROUGH");

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "FINISHED");

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId ?? Guid.Empty,
                roughPackage?.Package.PackageName ?? string.Empty,
                roughPackage?.Package.Price ?? 0,
                finishedPackage?.PackageId ?? Guid.Empty,
                finishedPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.Package.Price ?? 0,
                finishedPackage?.Package.Unit ?? string.Empty
            );
            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponse(
                            item.Id,
                            item.ConstructionItem?.Name,
                            item.SubConstructionId.HasValue ?
                              item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Name : item.ConstructionItem.Name,
                            item.Area,
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem.Coefficient
                            )).ToList();

            var promotionResponse = new PromotionInfo(
                            initialQuotation.Promotion.Id,
                            initialQuotation.Promotion.Name,
                            initialQuotation.Promotion.Value);

            var batchPaymentResponse =
                            initialQuotation.BactchPayments.Select(item => new BatchPaymentInfo(
                                item.Id,
                                item.Description,
                                item.Percents,
                                item.Price,
                                item.Unit)).ToList();

            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project.Customer.Username,
                ProjectId = initialQuotation.Project.Id,
                PromotionId = initialQuotation.PromotionId,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag,
                Note = initialQuotation.Note,
                TotalRough = initialQuotation.TotalRough,
                TotalUtilities = initialQuotation.TotalUtilities,
                Unit = initialQuotation.Unit,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses,
                PromotionInfo = promotionResponse,
                BatchPaymentInfos = batchPaymentResponse
            };

            return result;
        }

        public async Task<string> AssignQuotation(Guid accountId, Guid initialQuotationId)
        {
            var infoStaff = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == accountId,
                            include: a => a.Include(a => a.InitialQuotations));

            if (infoStaff.InitialQuotations.Count > 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Too_Many_Requests, AppConstant.ErrMessage.OverloadStaff);
            }

            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(i => i.Id == initialQuotationId);
            if (initialItem.Deflag == true)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Too_Many_Requests, AppConstant.ErrMessage.QuotationHasStaff);
            }
            if(initialItem != null)
            {
                initialItem.AccountId = accountId;
                initialItem.Deflag = true;
            } 

            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? "Phân công Sales thành công!" : throw new Exception("Phân công thất bại!");
        }

    }
}
