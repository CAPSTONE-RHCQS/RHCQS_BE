using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
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
                                       .Include(x => x.QuotationUtilities)
                                            .ThenInclude(x => x.UtilitiesItem)
                                       .Include(x => x.BatchPayments)
                );

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

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.Id,
                            item.Description ?? string.Empty,
                            item.Coefiicient ?? 0,
                            item.Price ?? 0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            var batchPaymentResponse =
                            initialQuotation.BatchPayments.Select(item => new BatchPaymentInfo(
                                item.Id,
                                item.Description,
                                item.Percents,
                                item.Price,
                                item.Unit)).ToList() ?? new List<BatchPaymentInfo>();

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
                UtilityInfos = utiResponse,
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
            if (initialItem != null)
            {
                initialItem.AccountId = accountId;
                initialItem.Deflag = true;
                var finalQuotation = new FinalQuotation()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = initialItem.ProjectId,
                    AccountId = accountId,
                    Deflag = false
                };
                await _unitOfWork.GetRepository<FinalQuotation>().InsertAsync(finalQuotation);
            }

            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);
            
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? "Phân công Sales thành công!" : throw new Exception("Phân công thất bại!");
        }

        public async Task<bool> ApproveInitialFromManager(Guid initialId, ApproveQuotationRequest request)
        {
            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Id == initialId);

            if (initialItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                               AppConstant.ErrMessage.Not_Found_InitialQuotaion);

            if (request.Type == AppConstant.InitialQuotationStatus.APPROVED)
            {
                initialItem.Status = AppConstant.InitialQuotationStatus.APPROVED;
            }
            else
            {
                initialItem.Status = AppConstant.InitialQuotationStatus.REJECTED;
                initialItem.ReasonReject = request.Reason;
            }

            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateInitialQuotation(UpdateInitialRequest request)
        {
            try
            {
                var initialItem = new InitialQuotation()
                {
                    Id = Guid.NewGuid(),
                    AccountId = request.AccountId,
                    ProjectId = request.ProjectId,
                    PromotionId = request.Promotions?.Id != null ? request.Promotions.Id : (Guid?)null,
                    Area = request.Area,
                    TimeProcessing = request.TimeProcessing,
                    TimeRough = request.TimeRough,
                    TimeOthers = request.TimeOthers,
                    OthersAgreement = request.OthersAgreement,
                    InsDate = DateTime.Now,
                    Status = AppConstant.InitialQuotationStatus.UNDER_REVIEW,
                    Version = request.VersionPresent + 1,
                    IsTemplate = false,
                    Deflag = true,
                    Note = null,
                    TotalRough = request.TotalRough,
                    TotalUtilities = request.TotalUtilities,
                    Unit = AppConstant.Unit.UnitPrice,
                    ReasonReject = null
                };
                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);

                foreach (var item in request.Items)
                {
                    var itemInitial = new InitialQuotationItem()
                    {
                        Id = Guid.NewGuid(),
                        Name = null,
                        ConstructionItemId = item.ConstructionItemId,
                        SubConstructionId = item.SubConstructionId,
                        Area = item.Area,
                        Price = item.Price,
                        UnitPrice = AppConstant.Unit.UnitPriceD,
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        InitialQuotationId = initialItem.Id,
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(itemInitial);
                }

                foreach (var package in request.Packages)
                {
                    var packageQuotation = new PackageQuotation
                    {
                        Id = Guid.NewGuid(),
                        PackageId = package.PackageId,
                        InitialQuotationId = initialItem.Id,
                        Type = package.Type,
                        InsDate = DateTime.Now
                    };

                    await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageQuotation);
                }

                foreach (var utl in request.Utilities)
                {
                    var utlItem = new QuotationUtility
                    {
                        Id = Guid.NewGuid(),
                        UtilitiesItemId = utl.UtilitiesItemId,
                        FinalQuotationId = null,
                        InitialQuotationId = initialItem.Id,
                        Name = utl.Description,
                        Coefiicient = utl.Coefiicient,
                        Price = utl.Price,
                        Description = utl.Description,
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                    };
                    await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                }

                //Create a batch payments
                foreach (var item in request.BatchPayments)
                {
                    var payItem = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        ContractId = null,
                        Price = item.Price,
                        PaymentDate = null,
                        PaymentPhase = null,
                        IntitialQuotationId = initialItem.Id,
                        Percents = item.Percents,
                        InsDate = DateTime.Now,
                        FinalQuotationId = null,
                        Description = item.Description,
                        Unit = AppConstant.Unit.UnitPrice
                    };
                    await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(payItem);
                }

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }
    }
}
