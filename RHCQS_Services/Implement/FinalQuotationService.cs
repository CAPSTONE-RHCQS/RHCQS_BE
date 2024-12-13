using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System.Text;
using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Response.App;
using System.Runtime.InteropServices;

using RHCQS_BusinessObject.Helper;
using DocumentFormat.OpenXml.Office2010.PowerPoint;


namespace RHCQS_Services.Implement
{
    public class FinalQuotationService : IFinalQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FinalQuotationService> _logger;
        private readonly IConverter _converter;
        private readonly Cloudinary _cloudinary;
        public FinalQuotationService(IUnitOfWork unitOfWork, ILogger<FinalQuotationService> logger,
                        IConverter converter, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _converter = converter;
            _cloudinary = cloudinary;
        }

        public async Task<bool> CancelFinalQuotation(Guid Id, CancelQuotation reason)
        {
            if (reason == null || Id == Guid.Empty || string.IsNullOrEmpty(reason.ReasonCancel))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();

            var finalQuotation = await finalQuotationRepo.FirstOrDefaultAsync(x => x.Id == Id);

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Resource
                );
            }

            if (finalQuotation.Status.Equals(AppConstant.QuotationStatus.CANCELED)
                || finalQuotation.Status.Equals(AppConstant.QuotationStatus.FINALIZED))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.CancelFinalQuotaionAlready
                );
            }
            var project = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(x => x.Id == finalQuotation.ProjectId);
            project.Status = AppConstant.ProjectStatus.ENDED;
            _unitOfWork.GetRepository<Project>().UpdateAsync(project);

            finalQuotation.Status = AppConstant.QuotationStatus.CANCELED;
            finalQuotation.ReasonReject = reason.ReasonCancel;
            finalQuotation.UpsDate = LocalDateTime.VNDateTime();
            finalQuotation.Deflag = false;

            finalQuotationRepo.UpdateAsync(finalQuotation);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CancelFinalQuotaion
                );
            }

            return true;
        }
        public async Task<string> FeedbackFixFinalFromCustomer(Guid finalId, FeedbackQuotationRequest comment)
        {
            var finalquotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == finalId);

            if (finalquotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalidate_Quotation);
            }
            if (finalquotation.Status == AppConstant.QuotationStatus.ENDED ||
    finalquotation.Status == AppConstant.QuotationStatus.UPDATING)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Not_Comment_Quotation);
            }
            finalquotation.Note = comment.Note;
            finalquotation.Status = AppConstant.QuotationStatus.UPDATING;
            _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalquotation);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }
        public async Task<string> ConfirmArgeeFinalFromCustomer(Guid finalId)
        {
            var finalquotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == finalId,
                                                    include: x => x.Include(x => x.Project)
                                                    .ThenInclude(x => x.Customer!));

            if (finalquotation != null)
            {
                var projectQuotations = await _unitOfWork.GetRepository<FinalQuotation>()
                        .GetListAsync(predicate: x => x.ProjectId == finalquotation.ProjectId);

                if (projectQuotations.Any(x => x.Status == AppConstant.QuotationStatus.FINALIZED))
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.Already_Finalized_Quotation
                    );
                }

                //Update status present quotation
                finalquotation.Status = AppConstant.QuotationStatus.FINALIZED;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalquotation);

                //Update all version quotation - version present
                foreach (var quotation in projectQuotations.Where(x => x.Id != finalquotation.Id))
                {
                    quotation.Status = AppConstant.QuotationStatus.ENDED;
                    _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(quotation);
                }

                var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
                return isSuccessful;
            }
            else
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Quotation);
            }
        }
        public async Task<FinalQuotationResponse> CreateFinalQuotation(Guid projectId)
        {
            //try
            //{
                var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();
                if (await finalQuotationRepo.AnyAsync(p => p.ProjectId == projectId && p.Version == 0))
                {
                    return await GetDetailFinalQuotationByProjectId(projectId);
                }

                var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                            x => x.ProjectId == projectId &&
                            x.Status == AppConstant.QuotationStatus.FINALIZED,
                            include: x => x.Include(x => x.InitialQuotationItems)
                                           .Include(x => x.Project)
                                           .Include(x => x.PackageQuotations)
                                           .Include(x => x.Promotion)
                                           .Include(x => x.QuotationUtilities)
                                                .ThenInclude(x => x.UtilitiesItem)
                                           .Include(x => x.BatchPayments)
                                            .ThenInclude(x => x.Payment!)
                            );
                if (initialQuotation == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Not_Found,
                        AppConstant.ErrMessage.Not_Found_InitialQuotaion
                    );
                }

                var projectExists = await _unitOfWork.GetRepository<Project>()
                    .FirstOrDefaultAsync(p => p.Id == projectId);
                if (projectExists == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                        AppConstant.ErrMessage.ProjectFinalIdNotfound);
                }
                var finalQuotation = new FinalQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    PromotionId = initialQuotation.PromotionId,
                    Discount = (initialQuotation.Promotion?.Value ?? 0) * (projectExists.Area ?? 0),
                    TotalPrice = 0,
                    Note = null,
                    Version = 0,
                    InsDate = LocalDateTime.VNDateTime(),
                    Status = AppConstant.QuotationStatus.PENDING,
                    Deflag = true,
                    BatchPayments = new List<BatchPayment>(),
                    QuotationUtilities = new List<QuotationUtility>()
                };

                var BatchPaymentRepo = _unitOfWork.GetRepository<BatchPayment>();
                foreach (var batchPayment in initialQuotation.BatchPayments.Where(bp => bp.ContractId == null))
                {
                    batchPayment.FinalQuotationId = finalQuotation.Id;
                    batchPayment.InsDate = LocalDateTime.VNDateTime();
                    BatchPaymentRepo.UpdateAsync(batchPayment);
                }

                var QuotationUtilityRepo = _unitOfWork.GetRepository<QuotationUtility>();
                foreach (var initialUtility in initialQuotation.QuotationUtilities)
                {
                    initialUtility.FinalQuotationId = finalQuotation.Id;
                    initialUtility.UpsDate = LocalDateTime.VNDateTime();
                    QuotationUtilityRepo.UpdateAsync(initialUtility);
                }

                await finalQuotationRepo.InsertAsync(finalQuotation);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.CreatePackage
                    );
                }
                return await GetDetailFinalQuotationByProjectId(projectId);
            //}
            //catch (Exception ex) { throw; }
        }
        public async Task<Guid?> UpdateFinalQuotation(FinalRequest request)
        {
            //try
            //{
            double? totalUtilities = 0;
            double? totalEquipmentItems = 0;
            double? totalQuotationItems = 0;
            double? promotation = 0;
            double newVersion = 1;
            if (request == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            #region Check request duplicate
            //Utility
            var isValidUtility = ValidateDuplicateUtilities(request.Utilities, out var duplicateIds);
            if (!isValidUtility)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedUtility);
            }

            //Equiment
            var isValidEquiqment = ValidateDuplicateEquiment(request.EquipmentItems, out var duplicateNames);
            if (!isValidEquiqment)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedEquiment);
            }
            //worktemlateid
            //var isValidWorkTemplate = ValidateDuplicateWorkTemplateIds(request.FinalQuotationItems, out var duplicateConstructionIds);
            //if (!isValidWorkTemplate)
            //{
            //    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedWorktemplate);
            //}
            #endregion

            #region check and update something
            var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();

            var projectExists = await _unitOfWork.GetRepository<Project>()
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId);
            var checkFinalized = await finalQuotationRepo.FirstOrDefaultAsync(
                p => p.ProjectId == request.ProjectId && p.Status == AppConstant.QuotationStatus.FINALIZED);
            if (checkFinalized != null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.FinalizedFinalUpdateFailed);
            }
            if (projectExists == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                    AppConstant.ErrMessage.ProjectFinalIdNotfound);
            }
            if (request.PromotionId.HasValue)
            {
                var promotionExists = await _unitOfWork.GetRepository<Promotion>()
                    .FirstOrDefaultAsync(p => p.Id == request.PromotionId);
                if (promotionExists == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                        AppConstant.ErrMessage.PromotionIdNotfound);
                }
                promotation = promotionExists.Value * projectExists.Area;

            }

            var highestFinalQuotation = await finalQuotationRepo.FirstOrDefaultAsync(
                p => p.ProjectId == request.ProjectId
                    && p.BatchPayments.Any(),
                orderBy: p => p.OrderByDescending(p => p.Version),
                include: p => p.Include(x => x.Project)
            );
            var finalQuotationItems = highestFinalQuotation.FinalQuotationItems;

            if (highestFinalQuotation != null)
            {
                newVersion = highestFinalQuotation?.Version + 1 ?? 1;

                var duplicateVersion = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                    predicate: x => x.ProjectId == request.ProjectId && x.Version == newVersion
                );

                if (duplicateVersion != null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Conflict_Version);
                }
            }

            if (highestFinalQuotation?.Version >= AppConstant.General.MaxVersion)
            {
                highestFinalQuotation.Project.Status = AppConstant.ProjectStatus.ENDED;
                _unitOfWork.GetRepository<Project>().UpdateAsync(highestFinalQuotation.Project);
                await _unitOfWork.CommitAsync();
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.MaxVersionQuotation);
            }
            #endregion

            #region updatepresentversion
            var presentFinalQuotation = await finalQuotationRepo.FirstOrDefaultAsync(
                p => p.ProjectId == request.ProjectId && p.Version == request.VersionPresent
                    && p.BatchPayments.Any(),
                orderBy: p => p.OrderByDescending(p => p.Version),
                include: p => p.Include(x => x.Project)
                               .Include(f => f.FinalQuotationItems)
                               .Include(x => x.BatchPayments)
                                    .ThenInclude(x => x.Payment)
                               .Include(x => x.BatchPayments)
            );
            presentFinalQuotation.Status = AppConstant.QuotationStatus.ENDED;
            presentFinalQuotation.UpsDate = LocalDateTime.VNDateTime();
            _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(presentFinalQuotation);

            presentFinalQuotation.Project.Address = string.IsNullOrEmpty(request.Address) ?
                              presentFinalQuotation.Project.Address : request.Address;
            presentFinalQuotation.Project.CustomerName = string.IsNullOrEmpty(request.CustomerName) ?
                              presentFinalQuotation.Project.CustomerName : request.CustomerName;
            presentFinalQuotation.Project.UpsDate = LocalDateTime.VNDateTime();
            _unitOfWork.GetRepository<Project>().UpdateAsync(presentFinalQuotation.Project);
            #endregion

            #region createfinal
            var finalQuotation = new FinalQuotation
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                PromotionId = request.PromotionId,
                TotalPrice = 0,
                Note = request.Note,
                Version = newVersion,
                InsDate = LocalDateTime.VNDateTime(),
                UpsDate = LocalDateTime.VNDateTime(),
                Status = AppConstant.QuotationStatus.REVIEWING,
                Deflag = true,
                Discount = promotation,
                BatchPayments = new List<BatchPayment>(),
                IsDraft = true
            };
            #endregion

            #region create batchpayment
            if (request.BatchPaymentInfos != null)
            {
                foreach (var bp in request.BatchPaymentInfos)
                {
                    presentFinalQuotation.BatchPayments = presentFinalQuotation.BatchPayments.ToList();
                    var matchingBatchPayment = presentFinalQuotation.BatchPayments
                        .FirstOrDefault(existingBatchPayment => existingBatchPayment.NumberOfBatch == bp.NumberOfBatch);

                    if (matchingBatchPayment != null)
                    {
                        var payment = new Payment
                        {
                            Id = Guid.NewGuid(),
                            PaymentTypeId = matchingBatchPayment.Payment.PaymentTypeId,
                            InsDate = LocalDateTime.VNDateTime(),
                            TotalPrice = bp.Price,
                            Percents = matchingBatchPayment.Payment.Percents,
                            Description = matchingBatchPayment.Payment.Description,
                            Unit = AppConstant.Unit.UnitPrice,

                            PaymentDate = bp.PaymentDate,
                            PaymentPhase = bp.PaymentPhase,
                        };

                        var batchPayment = new BatchPayment
                        {
                            Id = Guid.NewGuid(),
                            InitialQuotationId = matchingBatchPayment.InitialQuotationId,
                            ContractId = matchingBatchPayment.ContractId,
                            InsDate = LocalDateTime.VNDateTime(),
                            FinalQuotationId = finalQuotation.Id,
                            Payment = payment,
                            Status = matchingBatchPayment.Status,
                            NumberOfBatch = matchingBatchPayment.NumberOfBatch
                        };

                        finalQuotation.BatchPayments.Add(batchPayment);
                    }
                }
            }
            #endregion

            #region create finalquotationitem
            if (request.FinalQuotationItems != null)
            {
                foreach (var fqi in request.FinalQuotationItems)
                {
                    Guid constructionId;
                    FinalQuotationItem finalQuotationItem;

                        var constructionItemExists = await _unitOfWork.GetRepository<ConstructionItem>()
                            .FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionId);

                        if (constructionItemExists == null && constructionItemExists.Type != AppConstant.Type.ROUGH)
                        {
                            throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                                AppConstant.ErrMessage.ConstructionIdNotfound);
                        }
                        constructionId = constructionItemExists.Id;
                        finalQuotationItem = new FinalQuotationItem
                        {
                            Id = Guid.NewGuid(),
                            ConstructionItemId = constructionId,
                            InsDate = LocalDateTime.VNDateTime(),
                            QuotationItems = new List<QuotationItem>()
                        };


                    if (fqi.QuotationItems != null && fqi.QuotationItems.Count > 0)
                    {
                        foreach (var qi in fqi.QuotationItems)
                        {
                            double? quotationPrice = 0;
                            var quotationItem = new QuotationItem
                            {
                                Id = Guid.NewGuid(),
                                Unit = qi.Unit,
                                Weight = qi.Weight,
                                UnitPriceLabor = qi.UnitPriceLabor,
                                UnitPriceRough = qi.UnitPriceRough,
                                UnitPriceFinished = qi.UnitPriceFinished,
                                TotalPriceLabor = (qi.UnitPriceLabor) * (qi.Weight),
                                TotalPriceRough = (qi.UnitPriceRough) * (qi.Weight),
                                TotalPriceFinished = (qi.UnitPriceFinished) * (qi.Weight),
                                WorkTemplateId = qi.WorkTemplateId,
                                Note = qi.Note
                            };
                            quotationPrice = quotationItem.TotalPriceLabor + quotationItem.TotalPriceRough + quotationItem.TotalPriceFinished;
                            finalQuotationItem.QuotationItems.Add(quotationItem);
                            totalQuotationItems += quotationPrice;
                        }
                    }

                    finalQuotation.FinalQuotationItems.Add(finalQuotationItem);
                }
            }
            #endregion

            #region create equiment
            if (request.EquipmentItems != null)
            {
                foreach (var equipment in request.EquipmentItems)
                {
                    var existingEquipment = finalQuotation.EquipmentItems
                        .FirstOrDefault(e => e.Name == equipment.Name);

                    if (existingEquipment != null)
                    {
                        existingEquipment.Unit = equipment.Unit;
                        existingEquipment.Quantity = equipment.Quantity;
                        existingEquipment.UnitOfMaterial = equipment.UnitOfMaterial;
                        existingEquipment.TotalOfMaterial = equipment.UnitOfMaterial * equipment.Quantity;
                        existingEquipment.Note = equipment.Note;
                        existingEquipment.Type = equipment.Type;
                        _unitOfWork.GetRepository<EquipmentItem>().UpdateAsync(existingEquipment);
                        totalEquipmentItems += existingEquipment.TotalOfMaterial;
                    }
                    else
                    {
                        var newEquipmentItem = new EquipmentItem
                        {
                            Id = Guid.NewGuid(),
                            Name = equipment.Name,
                            Unit = equipment.Unit,
                            Quantity = equipment.Quantity,
                            UnitOfMaterial = equipment.UnitOfMaterial,
                            TotalOfMaterial = equipment.UnitOfMaterial * equipment.Quantity,
                            Note = equipment.Note,
                            Type = equipment.Type
                        };

                        finalQuotation.EquipmentItems.Add(newEquipmentItem);
                        totalEquipmentItems += newEquipmentItem.TotalOfMaterial;
                    }
                }
            }
            #endregion

            #region create utilities
            if (request.Utilities != null && request.Utilities.Count > 0)
            {
                foreach (var utility in request.Utilities)
                {
                    var utilityItem = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == utility.UtilitiesItemId);
                    QuotationUtility utlItem;

                    if (utilityItem == null)
                    {
                        var utilitiesSection = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == utility.UtilitiesItemId);
                        var item = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.SectionId == utilitiesSection.Id);

                        utlItem = new QuotationUtility
                        {
                            Id = Guid.NewGuid(),
                            UtilitiesItemId = item?.Id ?? null,
                            FinalQuotationId = finalQuotation.Id,
                            Name = item?.Name ?? utilitiesSection.Name ?? string.Empty,
                            Coefficient = utility.Coefficient ?? 0,
                            Price = utility.Price,
                            Description = utility.Description,
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            UtilitiesSectionId = utilitiesSection.Id,
                            Quanity = utility.Quantity
                        };

                        // Update totalUtilities based on available coefficient from item or itemOption
                        //var coefficient = item?.Coefficient ?? 0;
                        totalUtilities += /*(coefficient != 0) ? utility.Price * coefficient :*/ utility.Price;
                    }
                    else
                    {
                        var section = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == utilityItem.SectionId);
                        utlItem = new QuotationUtility
                        {
                            Id = Guid.NewGuid(),
                            UtilitiesItemId = utilityItem.Id,
                            FinalQuotationId = finalQuotation.Id,
                            Name = utilityItem.Name ?? string.Empty,
                            Coefficient = utility.Coefficient,
                            Price = utility.Price,
                            Description = utility.Description,
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            UtilitiesSectionId = utilityItem.SectionId,
                            Quanity = utility.Quantity
                        };
                        totalUtilities += utility.Price;
                    }

                    await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);

                }
            }
            #endregion
            finalQuotation.TotalPrice = totalUtilities + totalEquipmentItems + totalQuotationItems - promotation;


            await finalQuotationRepo.InsertAsync(finalQuotation);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.FinalQuotationUpdateFailed
                );
            }
            return finalQuotation.Id;
            //}catch(Exception ex)
            //{
            //    throw;
            //}

        }
        public async Task DeleteFinalQuotation(Guid finalQuotationId)
        {
            var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();
            var finalQuotation = await finalQuotationRepo.FirstOrDefaultAsync(fq => fq.Id == finalQuotationId,
                include: fq => fq
                    .Include(f => f.FinalQuotationItems)
                        .ThenInclude(fqi => fqi.QuotationItems)
                    .Include(f => f.BatchPayments)
                    .Include(f => f.EquipmentItems)
                    .Include(f => f.QuotationUtilities)
                    .Include(f => f.Media)
            );

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.NotFound,
                    AppConstant.ErrMessage.FinalNotfound
                );
            }

            if (finalQuotation.Version == 0)
            {
                foreach (var batchPayment in finalQuotation.BatchPayments)
                {
                    batchPayment.FinalQuotationId = null;
                    var batchPaymentRepo = _unitOfWork.GetRepository<BatchPayment>();
                    batchPaymentRepo.UpdateAsync(batchPayment);
                }

                foreach (var quotationUtility in finalQuotation.QuotationUtilities)
                {
                    quotationUtility.FinalQuotationId = null;
                    var quotationUtilityRepo = _unitOfWork.GetRepository<QuotationUtility>();
                    quotationUtilityRepo.UpdateAsync(quotationUtility);
                }

                foreach (var finalQuotationItem in finalQuotation.FinalQuotationItems)
                {
                    foreach (var quotationItem in finalQuotationItem.QuotationItems)
                    {
                        var quotationItemRepo = _unitOfWork.GetRepository<QuotationItem>();
                        quotationItemRepo.DeleteAsync(quotationItem);
                    }

                    var finalQuotationItemRepo = _unitOfWork.GetRepository<FinalQuotationItem>();
                    finalQuotationItemRepo.DeleteAsync(finalQuotationItem);
                }

                foreach (var equipmentItem in finalQuotation.EquipmentItems)
                {
                    var equipmentItemRepo = _unitOfWork.GetRepository<EquipmentItem>();
                    equipmentItemRepo.DeleteAsync(equipmentItem);
                }

                foreach (var media in finalQuotation.Media)
                {
                    var mediaRepo = _unitOfWork.GetRepository<Medium>();
                    mediaRepo.DeleteAsync(media);
                }
            }
            else
            {
                foreach (var finalQuotationItem in finalQuotation.FinalQuotationItems)
                {
                    foreach (var quotationItem in finalQuotationItem.QuotationItems)
                    {
                        var quotationItemRepo = _unitOfWork.GetRepository<QuotationItem>();
                        quotationItemRepo.DeleteAsync(quotationItem);
                    }

                    var finalQuotationItemRepo = _unitOfWork.GetRepository<FinalQuotationItem>();
                    finalQuotationItemRepo.DeleteAsync(finalQuotationItem);
                }

                foreach (var batchPayment in finalQuotation.BatchPayments)
                {
                    var batchPaymentRepo = _unitOfWork.GetRepository<BatchPayment>();
                    batchPaymentRepo.DeleteAsync(batchPayment);
                }

                foreach (var equipmentItem in finalQuotation.EquipmentItems)
                {
                    var equipmentItemRepo = _unitOfWork.GetRepository<EquipmentItem>();
                    equipmentItemRepo.DeleteAsync(equipmentItem);
                }

                foreach (var quotationUtility in finalQuotation.QuotationUtilities)
                {
                    var quotationUtilityRepo = _unitOfWork.GetRepository<QuotationUtility>();
                    quotationUtilityRepo.DeleteAsync(quotationUtility);
                }

                foreach (var media in finalQuotation.Media)
                {
                    var mediaRepo = _unitOfWork.GetRepository<Medium>();
                    mediaRepo.DeleteAsync(media);
                }
            }

            // Delete the final quotation itself
            finalQuotationRepo.DeleteAsync(finalQuotation);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.FinalQuotationUpdateFailed
                );
            }
        }

        public async Task<string> ApproveFinalFromManager(Guid finalId, ApproveQuotationRequest request)
        {
            var finalItem = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == finalId);

            if (finalItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                               AppConstant.ErrMessage.Not_Found_FinalQuotaion);
            if (request.Type?.ToLower() == AppConstant.QuotationStatus.APPROVED.ToLower())
            {
                finalItem.Status = AppConstant.QuotationStatus.APPROVED;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalItem);

                var project = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(x => x.Id == finalItem.ProjectId);
                project.Status = AppConstant.ProjectStatus.UNDER_REVIEW;
                _unitOfWork.GetRepository<Project>().UpdateAsync(project);
                var data = await GetDetailFinalQuotationById(finalItem.Id);
                try
                {
                    var htmlContent = GenerateHtmlContent(data);

                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                                ColorMode = ColorMode.Color,
                                Orientation = Orientation.Portrait,
                                PaperSize = PaperKind.A4
                            },
                        Objects = {
                                new ObjectSettings() {
                                    PagesCount = true,
                                    HtmlContent = htmlContent,
                                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = null }
                                }
                            }
                    };

                    string dllPath = Path.Combine(AppContext.BaseDirectory, "ExternalLibraries", "libwkhtmltox.dll");
                    NativeLibrary.Load(dllPath);

                    var pdf = _converter.Convert(doc);
                    //Upload cloudinary
                    using (var pdfStream = new MemoryStream(pdf))
                    {
                        // Tạo tham số để upload lên Cloudinary
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription($"{data.ProjectId}_Quotation.pdf", pdfStream),
                            Folder = "FinalQuotation",
                            PublicId = $"Bao_gia_chi_tiet_{data.ProjectId}_{data.Version}",
                            UseFilename = true,
                            UniqueFilename = true,
                            Overwrite = true
                        };

                        // Upload file lên Cloudinary
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        // Kiểm tra nếu upload không thành công
                        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                        }

                        //Tạo Media lưu file
                        var mediaInfo = new Medium
                        {
                            Id = Guid.NewGuid(),
                            HouseDesignVersionId = null,
                            Name = AppConstant.General.Final,
                            Url = uploadResult.Url.ToString(),
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            SubTemplateId = null,
                            PaymentId = null,
                            FinalQuotationId = finalItem.Id,
                            InitialQuotationId = null,
                        };

                        await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);
                        bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                        if (!isSuccessful)
                        {
                            throw new AppConstant.MessageError(
                                (int)AppConstant.ErrCode.Conflict,
                                AppConstant.ErrMessage.FinalQuotationUpdateFailed
                            );
                        }

                        return uploadResult.SecureUrl.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (request.Type?.ToLower() == AppConstant.QuotationStatus.REJECTED.ToLower())
            {
                if (request.Reason == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                                               AppConstant.ErrMessage.Reason_Rejected_Required);
                }
                finalItem.Status = AppConstant.QuotationStatus.REJECTED;
                finalItem.ReasonReject = request.Reason;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalItem);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.FinalQuotationUpdateFailed
                    );
                }
                return AppConstant.Message.REJECTED;
            }

            return null;
        }      

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationById(Guid id)
        {
            //try
            //{
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
            x => x.Id.Equals(id)
            && (x.Deflag == true)
            && x.BatchPayments.Any(),
            include: x => x.Include(x => x.Project)
                           .ThenInclude(x => x.Customer!)
                           .Include(x => x.Promotion)
                           .Include(x => x.QuotationUtilities)
                               .ThenInclude(qu => qu.UtilitiesSection)
                               .ThenInclude(qu => qu.UtilitiesItems)
                           .Include(x => x.EquipmentItems)
                           .Include(x => x.FinalQuotationItems)
                               .ThenInclude(qu => qu.QuotationItems)
                           .Include(x => x.BatchPayments!)
                               .ThenInclude(p => p.Payment!)
                               .ThenInclude(p => p.PaymentType!)
            );

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.Not_Found_FinalQuotaion);
            }

            var BatchPayments = () => finalQuotation.BatchPayments
            .OrderBy(bp => bp.NumberOfBatch)
            .Select(bp => new BatchPaymentResponse(
                    bp?.Payment!.Id ?? Guid.Empty,
                    bp?.Payment?.PaymentTypeId ?? Guid.Empty,
                    bp?.Payment?.PaymentType?.Name ?? string.Empty,
                    bp?.ContractId ?? null,
                    bp?.InsDate ?? null,
                    bp?.Status,
                    bp?.Payment?.UpsDate,
                    bp?.Payment?.Description,
                    bp?.Payment?.Percents ?? 0,
                    bp?.Payment?.TotalPrice,
                    bp?.Payment?.Unit,
                    bp?.Payment?.PaymentDate,
                    bp?.Payment?.PaymentPhase,
                    bp?.NumberOfBatch
                )
            ).ToList();

            var EquipmentItems = () => finalQuotation.EquipmentItems.Select(ei =>
                new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note,
                    ei.Type
                )
            ).ToList();

            var QuotationItems = async (List<QuotationItem> quotationItems) =>
            {
                var result = new List<QuotationItemResponse>();
                foreach (var qi in quotationItems)
                {
                    string workName = null;
                    if (qi.WorkTemplateId != null)
                    {
                        workName = await GetWorkNameById((Guid)qi.WorkTemplateId);
                    }
                    result.Add(new QuotationItemResponse(
                        qi.Id,
                        qi.WorkTemplateId ?? null,
                        workName,
                        qi.Unit ?? null,
                        qi.Weight,
                        qi.UnitPriceLabor,
                        qi.UnitPriceRough,
                        qi.UnitPriceFinished,
                        qi.TotalPriceLabor,
                        qi.TotalPriceRough,
                        qi.TotalPriceFinished,
                        qi.InsDate,
                        qi.UpsDate,
                        qi.Note
                    ));
                }

                return result.Where(response => response != null).ToList();
            };
            var finalQuotationItemsList = new List<FinalQuotationItemResponse>();

            foreach (var fqi in finalQuotation.FinalQuotationItems)
            {
                var constructionItemRepo = _unitOfWork.GetRepository<ConstructionItem>();
                Guid constructionId;
                string constructionType;
                string name;
                    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionItemId);
                    if (constructionItem == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            AppConstant.ErrMessage.ConstructionIdNotfound);
                    }
                    constructionId = constructionItem.Id;
                    constructionType = constructionItem?.Type ?? null;
                    name = constructionItem?.Name ?? string.Empty;
                    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                        fqi.Id,
                        constructionId,
                        name,
                        constructionType,
                        fqi.InsDate,
                        quotationItemsList
                    ));
            }

            var batchPaymentsList = BatchPayments();
            var equipmentItemsList = EquipmentItems();

            var promotionInfo = finalQuotation.Promotion != null
                ? new PromotionInfo(
                    finalQuotation.Promotion.Id,
                    finalQuotation.Promotion.Name,
                    finalQuotation.Promotion.Value
                )
                : null;

            var utilityInfoList = finalQuotation.QuotationUtilities?.Select(qUtility => new UtilityInf(
                qUtility.Id,
                qUtility.UtilitiesItemId,
                qUtility.UtilitiesSectionId,
                qUtility.Name,
                qUtility.Description ?? string.Empty,
                qUtility.Coefficient ?? 0,
                qUtility.Price ?? 0,
                qUtility.UtilitiesItem != null
                    ? qUtility.UtilitiesItem.Section?.UnitPrice ?? 0
                    : qUtility.UtilitiesSection?.UnitPrice ?? 0,
                qUtility.UtilitiesItem != null
                    ? qUtility.UtilitiesItem.Section?.Unit ?? string.Empty
                    : qUtility.UtilitiesSection?.Unit ?? string.Empty,
                qUtility.Quanity ?? null
            )).ToList() ?? new List<UtilityInf>();

            var constructionRough = finalQuotationItemsList
                    .Where(item => item.Type == AppConstant.Type.WORK_ROUGH)
                    .SelectMany(item => item.QuotationItems)
                    .GroupBy(qi => AppConstant.Type.WORK_ROUGH)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(qi => qi.TotalPriceRough ?? 0),
                        group.Sum(qi => qi.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();

            var constructionFinished = finalQuotationItemsList
                .Where(item => item.Type == AppConstant.Type.WORK_FINISHED)
                .SelectMany(item => item.QuotationItems)
                .GroupBy(qi => AppConstant.Type.WORK_FINISHED)
                .Select(group => new ConstructionSummary(
                    group.Key,
                    group.Sum(qi => qi.TotalPriceRough ?? 0),
                    group.Sum(qi => qi.TotalPriceLabor ?? 0)
                )).FirstOrDefault();
            var equipmentCost = finalQuotation.EquipmentItems
                .Sum(ei => ei.TotalOfMaterial);

            var equipmentCostSummary = new ConstructionSummary(
                "EQUIPMENT",
                (double)(equipmentCost ?? 0.0),
                0
            );
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                            ci => ci.ProjectId == finalQuotation.ProjectId,
                            include: ci => ci.Include(x => x.PackageQuotations)
                                             .ThenInclude(x => x.Package)
                                             .Include(x => x.Project)
                                             .ThenInclude(x => x.HouseDesignDrawings)
                                             .ThenInclude(x => x.HouseDesignVersions)
                                             .ThenInclude(x => x.Media)
                                             .Include(x => x.InitialQuotationItems)
                                             .ThenInclude(x => x.ConstructionItem)
                                             .ThenInclude(x => x.SubConstructionItems),
                            orderBy: query => query.OrderByDescending(x => x.Version));

            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == AppConstant.Type.ROUGH);

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == AppConstant.Type.FINISHED);

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId == null ? null : roughPackage.PackageId,
                roughPackage?.Package.PackageName ?? string.Empty,
                roughPackage?.Package.Price ?? 0,

                finishedPackage?.PackageId == null ? null : finishedPackage.PackageId,
                finishedPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.Package.Price ?? 0,
                finishedPackage?.Package.Unit ?? string.Empty
            );
            var initInfoList = initialQuotation.InitialQuotationItems
                .Where(item => item.ConstructionItemId != null)
                .Select(item =>
                {
                    var coefficient = item.SubConstructionId != null
                        ? item.ConstructionItem.SubConstructionItems?
                            .FirstOrDefault(sub => sub.Id == item.SubConstructionId)?.Coefficient
                        : item.ConstructionItem.Coefficient;

                    var calculatedArea = coefficient > 0 ? item.Area * coefficient : item.Area;

                    return new InitQuotationInfo(
                        item.ConstructionItem.Name,
                        calculatedArea
                    );
                })
                .ToList();

            var houseDesignDrawingsList = initialQuotation.Project.HouseDesignDrawings
                .OrderBy(hd => hd.Step)
                .SelectMany(hd => hd.HouseDesignVersions)
                .Select(hd =>
                {
                    var media = hd.Media.FirstOrDefault(m => m.HouseDesignVersionId == hd.Id);
                    var url = media?.Url ?? string.Empty;

                    return new HouseDrawingVersionInf(
                        hd.HouseDesignDrawingId,
                        hd.Name ?? string.Empty,
                        url,
                        hd.Version
                    );
                })
                .ToList();

            var customerPhone = finalQuotation.Project.Customer.PhoneNumber;
            var email = finalQuotation.Project.Customer.Email;
            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.CustomerName ?? string.Empty,
                customerPhone,
                email,
                finalQuotation.ProjectId,
                finalQuotation.Project.Area ?? null,
                initialQuotation.Id,
                initialQuotation.Version,
                houseDesignDrawingsList,
                packageInfo,
                finalQuotation.Project.Type ?? string.Empty,
                finalQuotation.Project.Address ?? string.Empty,
                finalQuotation?.Discount ?? null,
                finalQuotation.TotalPrice,
                finalQuotation.Note,
                initialQuotation.OthersAgreement,
                finalQuotation.Version,
                finalQuotation.InsDate,
                finalQuotation.UpsDate,
                finalQuotation.Status,
                finalQuotation.Deflag,
                finalQuotation.ReasonReject,
                initInfoList,
                batchPaymentsList,
                equipmentItemsList,
                finalQuotationItemsList,
                promotionInfo,
                utilityInfoList,
                constructionRough ?? new ConstructionSummary(),
                constructionFinished ?? new ConstructionSummary(),
                equipmentCostSummary ?? new ConstructionSummary()
            );

            return response;
            //}
            //catch (Exception ex)
            //{
            //    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            //}
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByProjectId(Guid projectid)
        {

            //try
            //{
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                x => x.ProjectId.Equals(projectid)
                && x.Version == 0
                && (x.Deflag == true)
                && x.BatchPayments.Any(),
                include: x => x.Include(x => x.Project)
                               .ThenInclude(x => x.Customer!)
                               .Include(x => x.Promotion)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesItem)
                                   .ThenInclude(qu => qu.Section)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesSection)
                                   .ThenInclude(qu => qu.UtilitiesItems)
                               .Include(x => x.EquipmentItems)
                               .Include(x => x.FinalQuotationItems)
                               .ThenInclude(qu => qu.QuotationItems)
                               .Include(x => x.BatchPayments!)
                                   .ThenInclude(p => p.Payment!)
                                   .ThenInclude(p => p.PaymentType!)
                );

            if (finalQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.Not_Found_FinalQuotaion);
            }

            var BatchPayments = () => finalQuotation.BatchPayments
            .OrderBy(bp => bp.NumberOfBatch)
            .Select(bp => new BatchPaymentResponse(
                    bp?.Payment.Id ?? Guid.Empty,
                    bp?.Payment?.PaymentTypeId ?? Guid.Empty,
                    bp?.Payment?.PaymentType?.Name ?? string.Empty,
                    bp?.ContractId ?? null,
                    bp?.InsDate ?? null,
                    bp?.Status,
                    bp?.Payment?.UpsDate,
                    bp?.Payment?.Description,
                    bp?.Payment?.Percents ?? 0,
                    bp?.Payment?.TotalPrice,
                    bp?.Payment?.Unit,
                    bp?.Payment?.PaymentDate,
                    bp?.Payment?.PaymentPhase,
                    bp?.NumberOfBatch
                )
            ).ToList();

            var EquipmentItems = () => finalQuotation.EquipmentItems.Select(ei =>
                new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note,
                    ei.Type
                )
            ).ToList();
            var QuotationItems = async (List<QuotationItem> quotationItems) =>
            {
                var result = new List<QuotationItemResponse>();
                foreach (var qi in quotationItems)
                {
                    string workName = null;
                    if (qi.WorkTemplateId != null)
                    {
                        workName = await GetWorkNameById((Guid)qi.WorkTemplateId);
                    }
                    result.Add(new QuotationItemResponse(
                        qi.Id,
                        qi.WorkTemplateId ?? null,
                        workName,
                        qi.Unit ?? null,
                        qi.Weight,
                        qi.UnitPriceLabor,
                        qi.UnitPriceRough,
                        qi.UnitPriceFinished,
                        qi.TotalPriceLabor,
                        qi.TotalPriceRough,
                        qi.TotalPriceFinished,
                        qi.InsDate,
                        qi.UpsDate,
                        qi.Note
                    ));
                }
                return result.Where(response => response != null).ToList();
            };

            var finalQuotationItemsList = new List<FinalQuotationItemResponse>();

            foreach (var fqi in finalQuotation.FinalQuotationItems)
            {
                var constructionItemRepo = _unitOfWork.GetRepository<ConstructionItem>();
                Guid constructionId;
                string constructionType;
                string name;

                    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionItemId);
                    if (constructionItem == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            AppConstant.ErrMessage.ConstructionIdNotfound);
                    }

                    constructionId = constructionItem.Id;
                constructionType = constructionItem?.Type ?? null;
                    name = constructionItem?.Name ?? string.Empty;
                    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                        fqi.Id,
                        constructionId,
                        name,
                        constructionType,

                        fqi.InsDate,
                        quotationItemsList
                    ));
            }

            var batchPaymentsList = BatchPayments();
            var equipmentItemsList = EquipmentItems();

            var promotionInfo = finalQuotation.Promotion != null
                ? new PromotionInfo(
                    finalQuotation.Promotion.Id,
                    finalQuotation.Promotion.Name,
                    finalQuotation.Promotion.Value
                )
                : null;

            var utilityInfoList = finalQuotation.QuotationUtilities?.Select(qUtility => new UtilityInf(
                qUtility.Id,
                qUtility.UtilitiesItemId,
                qUtility.UtilitiesSectionId,
                qUtility.Name,
                qUtility.Description ?? string.Empty,
                qUtility.Coefficient ?? 0,
                qUtility.Price ?? 0,
                qUtility.UtilitiesItem != null
                    ? qUtility.UtilitiesItem.Section?.UnitPrice ?? 0
                    : qUtility.UtilitiesSection?.UnitPrice ?? 0,
                qUtility.UtilitiesItem != null
                    ? qUtility.UtilitiesItem.Section?.Unit ?? string.Empty
                    : qUtility.UtilitiesSection?.Unit ?? string.Empty,
                qUtility.Quanity ?? null
            )).ToList() ?? new List<UtilityInf>();

            var constructionRough = finalQuotationItemsList
                    .Where(item => item.Type == AppConstant.Type.WORK_ROUGH)
                    .SelectMany(item => item.QuotationItems)
                    .GroupBy(qi => AppConstant.Type.WORK_ROUGH)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(qi => qi.TotalPriceRough ?? 0),
                        group.Sum(qi => qi.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();

            var constructionFinished = finalQuotationItemsList
                .Where(item => item.Type == AppConstant.Type.WORK_FINISHED)
                .SelectMany(item => item.QuotationItems)
                .GroupBy(qi => AppConstant.Type.WORK_FINISHED)
                .Select(group => new ConstructionSummary(
                    group.Key,
                    group.Sum(qi => qi.TotalPriceRough ?? 0),
                    group.Sum(qi => qi.TotalPriceLabor ?? 0)
                )).FirstOrDefault();
            var equipmentCost = finalQuotation.EquipmentItems
                .Sum(ei => ei.TotalOfMaterial);

            var equipmentCostSummary = new ConstructionSummary(
                "EQUIPMENT",
                (double)(equipmentCost ?? 0.0),
                0
            );
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                            ci => ci.ProjectId == finalQuotation.ProjectId,
                            include: ci => ci.Include(x => x.PackageQuotations)
                                             .ThenInclude(x => x.Package)
                                             .Include(x => x.Media)
                                             .Include(x => x.Project)
                                             .ThenInclude(x => x.HouseDesignDrawings)
                                             .ThenInclude(x => x.HouseDesignVersions)
                                             .ThenInclude(x => x.Media)
                                             .Include(x => x.InitialQuotationItems)
                                             .ThenInclude(x => x.ConstructionItem)
                                             .ThenInclude(x => x.SubConstructionItems),
                            orderBy: query => query.OrderByDescending(x => x.Version));
            var mediaList = initialQuotation.Media;
            var roughPackage = initialQuotation.PackageQuotations
                    .FirstOrDefault(item => item.Type == AppConstant.Type.ROUGH);

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == AppConstant.Type.FINISHED);

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId == null ? null : roughPackage.PackageId,
                roughPackage?.Package.PackageName ?? string.Empty,
                roughPackage?.Package.Price ?? 0,
                finishedPackage?.PackageId == null ? null : finishedPackage.PackageId,
                finishedPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.Package.Price ?? 0,
                finishedPackage?.Package.Unit ?? string.Empty
            );

            var initInfoList = initialQuotation.InitialQuotationItems
                .Where(item => item.ConstructionItemId != null)
                .Select(item =>
                {
                    var coefficient = item.SubConstructionId != null
                        ? item.ConstructionItem.SubConstructionItems?
                            .FirstOrDefault(sub => sub.Id == item.SubConstructionId)?.Coefficient
                        : item.ConstructionItem.Coefficient;

                    var calculatedArea = coefficient > 0 ? item.Area * coefficient : item.Area;

                    return new InitQuotationInfo(
                        item.ConstructionItem.Name,
                        calculatedArea
                    );
                })
                .ToList();

            var houseDesignDrawingsList = initialQuotation.Project.HouseDesignDrawings
                .OrderBy(hd => hd.Step)
                .SelectMany(hd => hd.HouseDesignVersions)
                .Select(hd =>
                {
                    var url = mediaList.FirstOrDefault(m => m.HouseDesignVersionId == hd.Id)?.Url ?? string.Empty;

                    return new HouseDrawingVersionInf(
                        hd.HouseDesignDrawingId,
                        hd.Name ?? string.Empty,
                        url,
                        hd.Version
                    );
                })
                .ToList();
            var customerPhone = finalQuotation.Project.Customer.PhoneNumber;
            var email = finalQuotation.Project.Customer.Email;
            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.CustomerName ?? string.Empty,
                customerPhone,
                email,
                finalQuotation.ProjectId,
                finalQuotation.Project.Area ?? null,
                initialQuotation.Id,
                initialQuotation.Version,
                houseDesignDrawingsList,
                packageInfo,
                finalQuotation.Project.Type ?? string.Empty,
                finalQuotation.Project.Address ?? string.Empty,
                finalQuotation?.Discount ?? null,
                finalQuotation.TotalPrice,
                finalQuotation.Note,
                initialQuotation.OthersAgreement,
                finalQuotation.Version,
                finalQuotation.InsDate,
                finalQuotation.UpsDate,
                finalQuotation.Status,
                finalQuotation.Deflag,
                finalQuotation.ReasonReject,
                initInfoList,
                batchPaymentsList,
                equipmentItemsList,
                finalQuotationItemsList,
                promotionInfo,
                utilityInfoList,
                constructionRough ?? new ConstructionSummary(),
                constructionFinished ?? new ConstructionSummary(),
                equipmentCostSummary ?? new ConstructionSummary()
            );

            return response;
            //}
            //catch (Exception ex)
            //{
            //    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            //}

        }

        public async Task<IPaginate<FinalQuotationListResponse>> GetListFinalQuotation(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<FinalQuotation>().GetList(
                selector: x => new FinalQuotationListResponse(
                    x.Id,
                    x.Project.Customer != null ? x.Project.Customer.Username : string.Empty,
                    x.Version,
                    x.Status
                ),
                include: x => x.Include(x => x.Project)
                               .ThenInclude(x => x.Customer!),
                page: page,
                size: size
            );

            return list;
        }

        private string GenerateHtmlContent(FinalQuotationResponse request)
        {
            var sb = new StringBuilder();
            sb.Append(@"
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        @page {
            margin: 30px;
        }
        body {
            margin: 0;
            padding: 0;
            font-family: 'Arial', sans-serif;
            font-size: 14px;
            color: black;
        }
        h1 {
            font-size: 24px;
            font-weight: bold;
            text-align: center;
            margin-bottom: 20px;
        }
        h2 {
            font-size: 18px;
            font-weight: bold;
            text-align: left;
            margin-top: 30px;
        }
        h3 {
            font-size: 16px;
            font-weight: bold;
            margin-top: 20px;
        }
        p {
            font-size: 14px;
            line-height: 1.5;
            margin: 0;
            text-align: left;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
            page-break-inside: auto;
        }
        tr {
            page-break-inside: avoid;
            page-break-after: auto;
        }
        th, td {
            border: 1px solid black;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f0f0f0;
        }
        .center {
            text-align: center;
        }
        .signature {
            margin-top: 50px;
            width: 100%;
        }
        .signature-row {
            width: 100%;
            display: table;
            margin-top: 20px;
        }
        .signature-column {
            display: table-cell;
            text-align: center;
        }
        .signature-column strong {
            display: block;
            margin-top: 50px;
        }
        .total {
            background-color: #f2f2f2;
            font-weight: bold;
            color: red;
        }
        .work-name {
            max-width: 200px;
            word-wrap: break-word;
            white-space: pre-wrap;
            text-align: left;
        }
.two-columns {
    overflow: hidden; /* Để chứa các phần tử float */
}

.column {
    float: left; /* Đưa mỗi cột sang trái */
    width: 48%; /* Mỗi cột chiếm 48% chiều rộng */
    text-align: justify; /* Căn đều chữ trong mỗi cột */
    box-sizing: border-box; /* Đảm bảo padding không ảnh hưởng đến chiều rộng */
    margin-right: 4%; /* Khoảng cách giữa các cột */
}

.column:last-child {
    margin-right: 0; /* Loại bỏ margin cho cột cuối cùng */
}

    </style>
</head>

<body>
    <h1>BÁO GIÁ CHI TIẾT NHÀ Ở DÂN DỤNG</h1>
<div class='two-columns'>
   <div class='column'>
    <p><strong>LOẠI CÔNG TRÌNH:</strong> ");

            if (request.ProjectType.ToLower() == "all")
            {
                sb.Append("Phần Thô & Hoàn thiện");
            }
            else if (request.ProjectType.ToLower() == "rough")
            {
                sb.Append("Phần Thô");
            }
            else if (request.ProjectType.ToLower() == "finished")
            {
                sb.Append("Phần Hoàn Thiện");
            }
            else
            {
                sb.Append("Có Bản Vẽ");
            }
            sb.Append(@"
        <p><strong>CHỦ ĐẦU TƯ:</strong> " + request.AccountName + @"</p>
        <p><strong>SỐ ĐIỆN THOẠI:</strong> " + request.PhoneNumber + @"</p>
        <p><strong>ĐỊA CHỈ EMAIL:</strong> " + request.Email + @"</p>
    </div>
    <div class='column'>
        <p><strong>DIỆN TÍCH XÂY DỰNG:</strong> " + request.Area + @"</p>
        <p><strong>ĐỊA CHỈ XÂY DỰNG:</strong> " + request.ProjectAddress + @"</p>
        <p><strong>ĐƠN GIÁ THI CÔNG:</strong> " + request.PackageQuotationList.PackageRough + @"," + request.PackageQuotationList.PackageFinished + @"</p>
        <p><strong>TỔNG GIÁ TRỊ HỢP ĐỒNG:</strong> " + $"{request.TotalPrice:N0}" + @" VND</p>
    </div>
</div>
<h2>BẢNG TỔNG HỢP CHI PHÍ XÂY DỰNG</h2>
<div class='table-container'>
    <table>
        <thead>
            <tr>
                <th>STT</th>
                <th>KHOẢN MỤC CHI PHÍ</th>
                <th>CHI PHÍ VẬT TƯ</th>
                <th>CHI PHÍ NHÂN CÔNG</th>
                <th>THÀNH TIỀN (VND)</th>
            </tr>
        </thead>
        <tbody>");

            int noCount = 1;
            var rough = request.ConstructionRough;
            decimal roughTotalAmount = (decimal)(rough.TotalPriceRough + rough.TotalPriceLabor);
            string roughTypeDisplay = rough.Type == "ROUGH" ? "Phần thô" : "Phần thô";
            sb.Append($@"
    <tr>
        <td>{noCount++}</td>
        <td>{roughTypeDisplay}</td>
        <td>{rough.TotalPriceRough:N0}</td>
        <td>{rough.TotalPriceLabor:N0}</td>
        <td class='highlight'>{roughTotalAmount:N0}</td>
    </tr>");

            var finished = request.ConstructionFinished;
            decimal finishedTotalAmount = (decimal)(finished.TotalPriceRough + finished.TotalPriceLabor);
            string finishedTypeDisplay = finished.Type == "FINISHED" ? "Phần hoàn thiện" : "Phần hoàn thiện";
            sb.Append($@"
    <tr>
        <td>{noCount++}</td>
        <td>{finishedTypeDisplay}</td>
        <td>{finished.TotalPriceRough:N0}</td>
        <td>{finished.TotalPriceLabor:N0}</td>
        <td class='highlight'>{finishedTotalAmount:N0}</td>
    </tr>");

            // Add equipment cost summary
            var equipmentCostSummary = request.Equitment;
            decimal equipmentTotalAmount = (decimal)(equipmentCostSummary.TotalPriceRough + equipmentCostSummary.TotalPriceLabor);
            string equipmentTypeDisplay = equipmentCostSummary.Type == "EQUIPMENT" ? "Phần thiết bị" : "Phần thiết bị";

            sb.Append($@"
    <tr>
        <td>{noCount++}</td>
        <td>{equipmentTypeDisplay}</td>
        <td>{equipmentCostSummary.TotalPriceRough:N0}</td>
        <td>{equipmentCostSummary.TotalPriceLabor:N0}</td>
        <td class='highlight'>{equipmentTotalAmount:N0}</td>
    </tr>");

            decimal utilityTotal = 0;

            if (request.UtilityInfos != null && request.UtilityInfos.Count > 0)
            {

                foreach (var utility in request.UtilityInfos)
                {
                    decimal utilityAmount = (decimal)utility.Price;
                    utilityTotal += utilityAmount;
                }
                string utilityTypeDisplay = "Phần tiện ích";

                sb.Append($@"
            <tr>
                <td>{noCount++}</td>
                <td>{utilityTypeDisplay}</td>
                <td></td>
                <td></td>
                <td class='highlight'>{utilityTotal:N0}</td>
            </tr>");
            }

            decimal total = roughTotalAmount + finishedTotalAmount + equipmentTotalAmount + utilityTotal;
            decimal roundedTotal = Math.Round(total);
            sb.Append($@"
    <tr class='total'>
        <td colspan='4'>Tổng giá trị hợp đồng </td>
        <td class='highlight'>{total:N0}</td>
    </tr>
</tbody>
</table>
</div>");

            sb.Append($@"
<div class='signature'>
    <div class='signature-row'>
        <div class='signature-column'>
            NGƯỜI LẬP<br />
            <strong></strong>
        </div>
        <div class='signature-column'>
            NGƯỜI CHỦ TRÌ<br />
            <strong></strong>
        </div>
    </div>
</div>");
            // Begin the table
            sb.Append($@"
<h2>BẢNG BÁO GIÁ CHI TIẾT</h2>
<table border='1' cellpadding='5' cellspacing='0'>
    <thead>
        <tr>
            <th>STT</th>
            <th>NỘI DUNG<br /> CÔNG VIỆC</th>
            <th>DVT</th>
            <th>KHỐI LƯỢNG</th>
            <th>ĐƠN GIÁ<br /> NHÂN CÔNG</th>
            <th>ĐƠN GIÁ<br /> VẬT TƯ THÔ</th>
            <th>ĐƠN GIÁ<br /> VẬT TƯ H.T</th>
            <th>THÀNH TIỀN<br /> NHÂN CÔNG</th>
            <th>THÀNH TIỀN<br /> VẬT TƯ THÔ</th>
            <th>THÀNH TIỀN<br /> VẬT TƯ H.T</th>
            <th>GHI CHÚ</th>
        </tr>
    </thead>
    <tbody>");

            int noCons = 0;

            // Handling "ROUGH" items
            var roughItems = request.FinalQuotationItems.Where(x => x.Type == "WORK_ROUGH").ToList();
            if (roughItems.Any())
            {
                sb.Append($@"
        <tr>
            <td>{++noCons}</td>
            <td colspan='10'><b>PHẦN VẬT TƯ THÔ</b></td>
        </tr>");

                foreach (var construction in roughItems)
                {
                    // Display construction name as a section header
                    sb.Append($@"
            <tr>
                <td>{++noCons}</td>
                <td><b>{construction.ContructionName}</b></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
            </tr>");

                    foreach (var item in construction.QuotationItems)
                    {
                        sb.Append($@"
                <tr>
                    <td>{++noCons}</td>
                    <td class='work-name'>{item.WorkName}</th>
                    <td>{item.Unit}</td>
                    <td>{item.Weight:N0}</td>
                    <td>{item.UnitPriceLabor:N0}</td>
                    <td>{item.UnitPriceRough:N0}</td>
                    <td>{item.UnitPriceFinished:N0}</td>
                    <td>{item.TotalPriceLabor:N0}</td> 
                    <td>{item.TotalPriceRough:N0}</td>
                    <td>{item.TotalPriceFinished:N0}</td>
                    <td>{item.Note}</td>
                </tr>");
                    }
                }
            }

            // Handling "FINISHED" items
            var finishedItems = request.FinalQuotationItems.Where(x => x.Type == "WORK_FINISHED").ToList();
            if (finishedItems.Any())
            {
                sb.Append($@"
        <tr>
            <td>{++noCons}</td>
            <td colspan='10'><b>PHẦN HOÀN THIỆN</b></td>
        </tr>");

                foreach (var construction in finishedItems)
                {
                    // Display construction name as a section header
                    sb.Append($@"
            <tr>
                <td>{++noCons}</td>
                <td><b>{construction.ContructionName}</b></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
            </tr>");

                    foreach (var item in construction.QuotationItems)
                    {
                        sb.Append($@"
                <tr>
                    <td>{++noCons}</td>
                    <td class='work-name'>{item.WorkName}</th>
                    <td>{item.Unit}</td>
                    <td>{item.Weight:N0}</td>
                    <td>{item.UnitPriceLabor:N0}</td>
                    <td>{item.UnitPriceRough:N0}</td>
                    <td>{item.UnitPriceFinished:N0}</td>
                    <td>{item.TotalPriceLabor:N0}</td> 
                    <td>{item.TotalPriceRough:N0}</td>
                    <td>{item.TotalPriceFinished:N0}</td>
                    <td>{item.Note}</td>
                </tr>");
                    }
                }
            }

            // End the table
            sb.Append("</tbody></table>");


            sb.Append($@"
        </tbody>
    </table>

    <h4>CHI PHÍ THIẾT BỊ</h4>
    <table border='1' cellpadding='5' cellspacing='0'>
        <thead>
            <tr>
                <th>STT</th>
                <th>NỘI DUNG CÔNG VIỆC</th>
                <th>DVT</th>
                <th>KHỐI LƯỢNG</th>
                <th>ĐƠN GIÁ<br />VẬT TƯ HOÀN THIỆN</th>
                <th>THÀNH TIỀN<br />VẬT TƯ HOÀN THIỆN</th>
                <th>GHI CHÚ</th>
            </tr>
        </thead>
        <tbody>");

            int noEqui = 0;
            foreach (var item in request.EquipmentItems)
            {
                sb.Append($@"
            <tr>
                <td>{++noEqui}</td>
                <td>{item.Name}</td>
                <td>{item.Unit}</td>
                <td>{item.Quantity:N0}</td>
                <td>{item.UnitOfMaterial:N0}</td>
                <td>{item.TotalOfMaterial:N0}</td>
                <td>{item.Note}</td>
            </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>

    <h4>CHI PHÍ KHÁC</h4>
    <table border='1' cellpadding='5' cellspacing='0'>
        <thead>
            <tr>
                <th>STT</th>
                <th>TIỆN ÍCH</th>
                <th>HỆ SỐ</th>
                <th>ĐƠN GIÁ</th>
                <th>THÀNH TIỀN</th>
                <th>GHI CHÚ</th>
            </tr>
        </thead>
        <tbody>");

            int noUti = 0;
            foreach (var item in request.UtilityInfos)
            {
                sb.Append($@"
            <tr>
                <td>{++noUti}</td>
                <td>{item.Name}</td>
                <td>{item.Coefficient:N0}</td>
                <td>{item.UnitPrice:N0}</td>
                <td>{item.Price:N0}</td>
                <td>{item.Description:N0}</td>
            </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>

    <h2>KHUYẾN MÃI</h2>
    <table>
            <tr>
                <th>STT</th>
                <th>KHUYẾN MÃI</th>
                <th>GIÁ TRỊ (VND)</th>
                <th>THÀNH TIỀN</th>
            </tr>
            ");
            int noPro = 0;
            var promotion = request.PromotionInfo;
            if (promotion != null)
            {
                sb.Append($@"
                     <tr>
                        <td>{noPro++}</td>
                        <td>{promotion.Name}</td>
                        <td>{promotion.Value}</td>
                        <td>{roundedTotal - promotion.Value:N0}</td>
                    </tr>");
            }
            else
            {
                sb.Append($@"
                    <tr>
                        <td colspan='4'>Không có khuyên mãi</td>
                    </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>

    <h2>PHƯƠNG THỨC THANH TOÁN</h2>
    <p>Tổng giá trị hợp đồng sẽ được thanh toán theo các đợt sau:</p>
    <table>
        <tr>
            <th>ĐỢT</th>
            <th>NỘI DUNG THANH TOÁN</th>
            <th>GIÁ TRỊ(%)</th>
            <th>GIÁ TRỊ (VND)</th>
            <th>NGÀY THANH TOÁN</th>
            <th>HẠN CHÓT</th>
        </tr>");

            int stageCounter = 1;
            foreach (var payment in request.BatchPaymentInfos)
            {
                sb.Append($@"
        <tr>
            <td>{stageCounter}</td>
            <td>{payment.Description}</td>
            <td>{payment.Percents}</td>
            <td>{payment.Price:N0}</td>
            <td>{payment.PaymentDate}</td>
            <td>{payment.PaymentPhase}</td>
        </tr>");
                stageCounter++;
            }

            sb.Append(@"
    </table>

    <h2>CÁC ĐIỀU KHOẢN KHÁC</h2>
    <ul>
");
            if (!string.IsNullOrWhiteSpace(request.OthersAgreement))
            {
                sb.Append($@"
    <li><strong>Các điều khoản:</strong> {request.OthersAgreement}</li>
");
            }
            else
            {
                sb.Append($@"
    <li><strong>Các điều khoản:</strong> Không có điều khoản nào được cung cấp.</li>
");
            }

            sb.Append(@"
</body>
</html>");


            return sb.ToString();
        }

        public async Task<List<FinalAppResponse>> GetListFinalQuotationByProjectId(Guid projectId)
        {
            var paginatedList = await _unitOfWork.GetRepository<FinalQuotation>()
                .GetList(
                    predicate: x => x.ProjectId == projectId &&
                                (x.Status == AppConstant.QuotationStatus.APPROVED ||
                                x.Status == AppConstant.QuotationStatus.FINALIZED),
                    selector: x => new FinalAppResponse(
                        x.Id,
                        x.Version,
                         x.Media != null && x.Media.Any() ? x.Media.First().Url : string.Empty,
                         x.Status!
                    ),
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer!)
                                   .Include(x => x.Media),
                    orderBy: x => x.OrderByDescending(x => x.Version)
                );


            return paginatedList.Items.ToList();
        }

        public bool ValidateDuplicateUtilities(List<UtilitiesUpdateRequestForFinal> items, out List<Guid>? duplicateIds)
        {
            var duplicateGroups = items
                .GroupBy(item => item.UtilitiesItemId)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateGroups.Any())
            {
                duplicateIds = duplicateGroups.Select(g => g.Key).ToList();
                return false;
            }

            duplicateIds = null;
            return true;
        }
        public bool ValidateDuplicateEquiment(List<EquipmentItemsRequest> items, out string? duplicateNames)
        {
            var duplicateItems = items
                .GroupBy(item => item.Name?.Trim().ToLower())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateItems.Any())
            {
                duplicateNames = string.Join(", ", duplicateItems);
                return false;
            }

            duplicateNames = null;
            return true;
        }
        public bool CheckConstructionIds(
            List<FinalQuotationItemRequest> requests,
            ICollection<FinalQuotationItem> finalQuotationItems)
        {
            if (requests == null || !requests.Any() || finalQuotationItems == null || !finalQuotationItems.Any())
                return false;

            foreach (var request in requests)
            {
                var match = finalQuotationItems.Any(item =>
                    item.ConstructionItemId == request.ConstructionId);

                if (match)
                {
                    return true;
                }
            }

            return false;
        }
        public bool ValidateDuplicateWorkTemplateIds(
            List<FinalQuotationItemRequest> requests,
            out List<Guid>? duplicateConstructionIds)
        {
            duplicateConstructionIds = requests
                .Where(r => r.QuotationItems != null && r.QuotationItems.Any())
                .GroupBy(r => r.ConstructionId)
                .Where(group =>
                    group.SelectMany(g => g.QuotationItems)
                         .GroupBy(q => q.WorkTemplateId)
                         .Any(g => g.Count() > 1)) 
                .Select(g => g.Key)
                .ToList();

            return !duplicateConstructionIds.Any();
        }


        public async Task<string> GetStatusFinalQuotation(Guid finalId)
        {
            var finalInfo = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(predicate: i => i.Id == finalId);
            if (finalInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.FinalNotfound);
            }
            string result = finalInfo.Status;
            return result;
        }
        public async Task<string> GetWorkNameById(Guid workTemplateId)
        {

            var workTemplate = await _unitOfWork.GetRepository<WorkTemplate>().FirstOrDefaultAsync(
                x => x.Id.Equals(workTemplateId),
                include: x => x.Include(x => x.ContructionWork));
            return workTemplate?.ContructionWork?.WorkName ?? string.Empty;
        }

    }
}
