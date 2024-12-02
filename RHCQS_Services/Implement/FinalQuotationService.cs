﻿using CloudinaryDotNet.Actions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Request;
using static RHCQS_BusinessObjects.AppConstant;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RHCQS_BusinessObject.Payload.Response.App;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using RHCQS_BusinessObject.Helper;
using Newtonsoft.Json;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;

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
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invail_Quotation);
            }

            finalquotation.Note = comment.Note;
            finalquotation.Status = AppConstant.QuotationStatus.PROCESSING;
            _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalquotation);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }
        public async Task<string> ConfirmArgeeFinalFromCustomer(Guid finalId)
        {
            var finalquotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == finalId);

            if (finalquotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invail_Quotation);
            }
            var project = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(x => x.Id == finalquotation.ProjectId);
            project.Status = AppConstant.ProjectStatus.UNDER_REVIEW;
            _unitOfWork.GetRepository<Project>().UpdateAsync(project);

            finalquotation.Status = AppConstant.QuotationStatus.FINALIZED;
            _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalquotation);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }
        public async Task<FinalQuotationResponse> CreateFinalQuotation(Guid projectId)
        {
            try
            {

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
                    Discount = initialQuotation.Promotion.Value * projectExists.Area,
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

                //finalQuotation.FinalQuotationItems = initialQuotation.InitialQuotationItems.Select(iqi => new FinalQuotationItem
                //{
                //    Id = Guid.NewGuid(),
                //    ConstructionItemId = iqi.ConstructionItemId,
                //    SubContructionId = iqi.SubConstructionId,
                //    Area = iqi.Area,
                //    InsDate = LocalDateTime.VNDateTime(),
                //}).ToList();

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
            }
            catch (Exception ex) { throw; }
        }
        public async Task<Guid?> UpdateFinalQuotation(FinalRequest request)
        {
            //try
            //{
            double? totalUtilities = 0;
            double? totalEquipmentItems = 0;
            double? totalQuotationItems = 0;
            double? promotation = 0;

            if (request == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            #region Check request duplicate
            //Construction 
            //var isValidContruction = ValidateDuplicateFinalQuotationItems(request.FinalQuotationItems, out var duplicateDetails);
            //if (!isValidContruction)
            //{
            //    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedConstruction);
            //}

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
                    && p.QuotationUtilities.Any()
                    && p.BatchPayments.Any(),
                orderBy: p => p.OrderByDescending(p => p.Version),
                include: p => p.Include(x => x.Project)
                               .Include(f => f.FinalQuotationItems)
                               .Include(x => x.BatchPayments)
                                    .ThenInclude(x => x.Payment)
                               .Include(x => x.BatchPayments)
            );
            var finalQuotationItems = highestFinalQuotation.FinalQuotationItems;

            if (highestFinalQuotation != null)
            {
                highestFinalQuotation.Status = AppConstant.QuotationStatus.PROCESSING;
                highestFinalQuotation.UpsDate = LocalDateTime.VNDateTime();
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(highestFinalQuotation);

                highestFinalQuotation.Project.Address = string.IsNullOrEmpty(request.Address) ?
                                  highestFinalQuotation.Project.Address : request.Address;
                highestFinalQuotation.Project.CustomerName = string.IsNullOrEmpty(request.CustomerName) ?
                                  highestFinalQuotation.Project.CustomerName : request.CustomerName;
                highestFinalQuotation.Project.UpsDate = LocalDateTime.VNDateTime();
                _unitOfWork.GetRepository<Project>().UpdateAsync(highestFinalQuotation.Project);
            }

            if (highestFinalQuotation?.Version >= AppConstant.General.MaxVersion)
            {
                highestFinalQuotation.Project.Status = AppConstant.ProjectStatus.ENDED;
                _unitOfWork.GetRepository<Project>().UpdateAsync(highestFinalQuotation.Project);
                await _unitOfWork.CommitAsync();
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.MaxVersionQuotation);
            }
            #endregion
            #region createfinal
            double newVersion = highestFinalQuotation?.Version + 1 ?? 1;
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
                    highestFinalQuotation.BatchPayments = highestFinalQuotation.BatchPayments.ToList();
                    var matchingBatchPayment = highestFinalQuotation.BatchPayments
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
                    //var subConstructionRepo = _unitOfWork.GetRepository<SubConstructionItem>();
                    //var subConstructionItemExists = await subConstructionRepo.FirstOrDefaultAsync(sb => sb.Id == fqi.SubconstructionId);

                    Guid constructionId;
                    //Guid subConstructionId;
                    //Double? coefficient;
                    //string contructionType;
                    //var finalArea = highestFinalQuotation.FinalQuotationItems.FirstOrDefault();
                    FinalQuotationItem finalQuotationItem;
                    //if (subConstructionItemExists != null)
                    //{
                    //    var constructionItemExists = await _unitOfWork.GetRepository<ConstructionItem>()
                    //        .FirstOrDefaultAsync(ci => ci.Id == subConstructionItemExists.ConstructionItemsId);
                    //    constructionId = fqi.ConstructionId;
                    //    subConstructionId = subConstructionItemExists.Id;
                    //    //coefficient = subConstructionItemExists.Coefficient;
                    //    contructionType = constructionItemExists.Type ?? string.Empty;

                    //    finalQuotationItem = new FinalQuotationItem
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        ConstructionItemId = constructionId,
                    //        SubContructionId = subConstructionId,
                    //        InsDate = LocalDateTime.VNDateTime(),
                    //        Area = finalArea.Area,
                    //        QuotationItems = new List<QuotationItem>()
                    //    };
                    //}
                    //else
                    //{
                        var constructionItemExists = await _unitOfWork.GetRepository<ConstructionItem>()
                            .FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionId);

                        if (constructionItemExists == null && constructionItemExists.Type != AppConstant.Type.ROUGH)
                        {
                            throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                                AppConstant.ErrMessage.ConstructionIdNotfound);
                        }
                        constructionId = constructionItemExists.Id;
                        //coefficient = constructionItemExists.Coefficient;
                        //contructionType = constructionItemExists.Type ?? string.Empty;
                        finalQuotationItem = new FinalQuotationItem
                        {
                            Id = Guid.NewGuid(),
                            ConstructionItemId = constructionId,
                            InsDate = LocalDateTime.VNDateTime(),
                            QuotationItems = new List<QuotationItem>()
                        };
                    //}


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
                            //if (qi.LaborId != null)
                            //{
                            //    var laborExists = await _unitOfWork.GetRepository<Labor>()
                            //        .FirstOrDefaultAsync(l => l.Id == qi.LaborId);
                            //    if (laborExists == null)
                            //    {
                            //        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            //            AppConstant.ErrMessage.LaborIdNotfound);
                            //    }
                            //    double? quotationLaborPrice = 0;
                            //    var quotationItemLabor = new QuotationItem
                            //    {
                            //        Id = Guid.NewGuid(),
                            //        Unit = "ca làm",
                            //        Weight = qi.Weight,
                            //        UnitPriceLabor = laborExists.Price,
                            //        UnitPriceRough = null,
                            //        UnitPriceFinished = null,
                            //        TotalPriceLabor =/* (coefficient != 0) ? (laborExists.Price) * (qi.Weight) * coefficient
                            //        : */(laborExists.Price) * (qi.Weight),
                            //        TotalPriceRough = null,
                            //        TotalPriceFinished = null,
                            //        Note = qi.Note
                            //    };
                            //    quotationLaborPrice = quotationItemLabor.TotalPriceLabor;
                            //    quotationItemLabor.QuotationLabors = new List<QuotationLabor>
                            //    {
                            //        new QuotationLabor
                            //        {
                            //            Id = Guid.NewGuid(),
                            //            LaborId = laborExists.Id,
                            //            LaborPrice = laborExists.Price
                            //        }
                            //    };
                            //    finalQuotationItem.QuotationItems.Add(quotationItemLabor);
                            //    totalQuotationItems += quotationLaborPrice;
                            //}

                            //if (qi.MaterialId != null)
                            //{
                            //    var materialExists = await _unitOfWork.GetRepository<Material>()
                            //        .FirstOrDefaultAsync(m => m.Id == qi.MaterialId);
                            //    if (materialExists == null)
                            //    {
                            //        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            //            AppConstant.ErrMessage.MaterialIdNotfound);
                            //    }

                            //    QuotationItem quotationItemMaterial = new QuotationItem
                            //    {
                            //        Id = Guid.NewGuid(),
                            //        Unit = materialExists.Unit,
                            //        Weight = qi.Weight,
                            //        Note = qi.Note,
                            //        UnitPriceLabor = null,
                            //        TotalPriceLabor = null,
                            //        TotalPriceFinished = null
                            //    };
                            //    double? quotationMaterialPrice = 0;
                            //    switch (contructionType)
                            //    {
                            //        case "ROUGH":
                            //            quotationItemMaterial.UnitPriceRough = materialExists.Price;
                            //            quotationItemMaterial.TotalPriceRough =/* (coefficient != 0) ? (materialExists.Price) * (qi.Weight) * coefficient
                            //                : */(materialExists.Price) * (qi.Weight);
                            //            quotationMaterialPrice = quotationItemMaterial.TotalPriceRough ?? 0;
                            //            break;

                            //        case "FINISHED":
                            //            quotationItemMaterial.UnitPriceFinished = materialExists.Price;
                            //            quotationItemMaterial.TotalPriceFinished =/* (coefficient != 0) ? (materialExists.Price) * (qi.Weight) * coefficient
                            //                : */(materialExists.Price) * (qi.Weight);
                            //            quotationMaterialPrice = quotationItemMaterial.TotalPriceFinished ?? 0;
                            //            break;

                            //        default:
                            //            throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionTypeInvalidate);
                            //    }

                            //    quotationItemMaterial.QuotationMaterials = new List<QuotationMaterial>
                            //    {
                            //        new QuotationMaterial
                            //        {
                            //            Id = Guid.NewGuid(),
                            //            MaterialId = materialExists.Id,
                            //            Unit = materialExists.Unit,
                            //            MaterialPrice = materialExists.Price
                            //        }
                            //    };

                            //    finalQuotationItem.QuotationItems.Add(quotationItemMaterial);
                            //    totalQuotationItems += quotationMaterialPrice;
                            //}
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
            //try
            //{
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
            finalQuotationRepo.DeleteAsync(finalQuotation);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.FinalQuotationUpdateFailed
                );
            }
            //}
            //catch (Exception ex) { throw; }
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

        /*        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByCustomerName(string name)
                {
                    try
                    {
                        var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                            x => x.Project.Customer != null &&
                                 x.Version == 0 &&
                                 x.Project.Customer.Username.Equals(name) &&
                                 x.Deflag == true,
                            include: x => x.Include(x => x.Project)
                                           .ThenInclude(x => x.Customer!)
                                           .Include(x => x.Promotion)
                                           .Include(x => x.QuotationUtilities)
                                               .ThenInclude(qu => qu.UtilitiesItem)
                                           .Include(x => x.EquipmentItems)
                                           .Include(x => x.FinalQuotationItems)
                                           .Include(x => x.FinalQuotationItems)
                                               .ThenInclude(co => co.QuotationItems)
                                               .ThenInclude(co => co.QuotationLabors)
                                               .ThenInclude(co => co.Labor)
                                           .Include(x => x.FinalQuotationItems)
                                               .ThenInclude(co => co.QuotationItems)
                                               .ThenInclude(co => co.QuotationMaterials)
                                               .ThenInclude(co => co.Material)
                                           .Include(x => x.BatchPayments!)
                                               .ThenInclude(p => p.Payment!)
                                               .ThenInclude(p => p.PaymentType!)
                            );

                        if (finalQuotation == null)
                        {
                            throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                               AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                        }

                        var BatchPayments = () => finalQuotation.BatchPayments.Select(bp =>
                            new BatchPaymentResponse(
                                bp?.Payment?.Id ?? Guid.Empty,
                                bp?.Payment?.PaymentTypeId ?? Guid.Empty,
                                bp?.Payment?.PaymentType?.Name ?? string.Empty,
                                bp?.ContractId ?? Guid.Empty,
                                bp?.InsDate,
                                bp?.Status ?? string.Empty,
                                bp?.Payment?.UpsDate,
                                bp?.Payment?.Description,
                                bp?.Payment?.Percents,
                                bp?.Payment?.TotalPrice,
                                bp?.Payment?.Unit,
                                bp?.Payment?.PaymentDate,
                                bp?.Payment?.PaymentPhase
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

                        var QuotationItems = (List<QuotationItem> quotationItems) => quotationItems.Select(qi =>
                        {
                            if (qi.QuotationLabors.Any())
                            {
                                var labor = qi.QuotationLabors.FirstOrDefault()?.Labor;
                                var displayName = labor?.Name;
                                var laborId = labor?.Id ?? Guid.Empty;
                                var code = labor?.Code ?? string.Empty;
                                return new QuotationItemResponse(
                                    qi.Id,
                                    laborId,
                                    displayName,
                                    code,
                                    qi.Unit ?? "m2",
                                    qi.Weight,
                                    qi.UnitPriceLabor,
                                    qi.TotalPriceLabor,
                                    qi.InsDate,
                                    qi.UpsDate,
                                    qi.Note
                                );
                            }
                            else if (qi.QuotationMaterials.Any())
                            {
                                var material = qi.QuotationMaterials.FirstOrDefault()?.Material;
                                var displayName = material?.Name;
                                var materialId = material?.Id ?? Guid.Empty;
                                var code = material?.Code ?? string.Empty;
                                return new QuotationItemResponse(
                                    qi.Id,
                                    materialId,
                                    displayName,
                                    code,
                                    qi.Unit,
                                    qi.Weight,
                                    qi.UnitPriceRough,
                                    qi.UnitPriceFinished,
                                    qi.TotalPriceRough,
                                    qi.TotalPriceFinished,
                                    qi.InsDate,
                                    qi.UpsDate,
                                    qi.Note
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }).Where(response => response != null).ToList();

                        var finalQuotationItemsList = new List<FinalQuotationItemResponse>();

                        foreach (var fqi in finalQuotation.FinalQuotationItems)
                        {
                            var constructionItemRepo = _unitOfWork.GetRepository<ConstructionItem>();
                            var subConstructionRepo = _unitOfWork.GetRepository<SubConstructionItem>();

                            var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionItemId);

                            Guid constructionOrSubConstructionId;
                            double? coefficient;
                            string constructionType;
                            string contructname;

                            if (constructionItem != null)
                            {
                                constructionOrSubConstructionId = fqi.ConstructionItemId;
                                coefficient = constructionItem.Coefficient;
                                constructionType = constructionItem.Type;
                                contructname = constructionItem.Name;
                            }
                            else
                            {
                                var subConstructionItem = await subConstructionRepo.FirstOrDefaultAsync(sb => sb.Id == fqi.ConstructionItemId);

                                if (subConstructionItem == null)
                                {
                                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                                        $"Construction or SubConstruction item with ID {fqi.ConstructionItemId} không tồn tại.");
                                }

                                constructionOrSubConstructionId = subConstructionItem.ConstructionItemsId;
                                coefficient = subConstructionItem.Coefficient;
                                constructionType = subConstructionItem.ConstructionItems?.Type;
                                contructname = subConstructionItem.Name;
                            }

                            finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                                fqi.Id,
                                constructionOrSubConstructionId,
                                name,
                                constructionType,
                                coefficient,
                                fqi.InsDate,
                                QuotationItems(fqi.QuotationItems.ToList())
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
                            qUtility.UtilitiesItem?.Section?.UnitPrice ?? 0,
                            qUtility.UtilitiesItem?.Section?.Unit ?? string.Empty
                        )).ToList() ?? new List<UtilityInf>();

                        var constructionRough = finalQuotationItemsList
                            .Where(item => item.Type == "ROUGH")
                            .SelectMany(item => item.QuotationItems)
                            .GroupBy(qi => "ROUGH")
                            .Select(group => new ConstructionSummary(
                                group.Key,
                                group.Sum(qi => qi.TotalPriceRough ?? 0),
                                group.Sum(qi => qi.TotalPriceLabor ?? 0)
                            )).FirstOrDefault();

                        var constructionFinished = finalQuotationItemsList
                            .Where(item => item.Type == "FINISHED")
                            .SelectMany(item => item.QuotationItems)
                            .GroupBy(qi => "FINISHED")
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
                        var initialQuotationId = await _unitOfWork.GetRepository<InitialQuotation>()
                            .FirstOrDefaultAsync(ci => ci.ProjectId == finalQuotation.ProjectId && ci.Status == AppConstant.QuotationStatus.FINALIZED);
                        var response = new FinalQuotationResponse(
                            finalQuotation.Id,
                            finalQuotation.Project.Customer.Username ?? string.Empty,
                            finalQuotation.ProjectId,
                            initialQuotationId.Id,
                            finalQuotation.Project.Type ?? string.Empty,
                            finalQuotation.Project.Address ?? string.Empty,
                            finalQuotation.TotalPrice,
                            finalQuotation.Note,
                            finalQuotation.Version,
                            finalQuotation.InsDate,
                            finalQuotation.UpsDate,
                            finalQuotation.Status,
                            finalQuotation.Deflag,
                            finalQuotation.ReasonReject,
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
                    }
                    catch (Exception ex)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
                    }
                }*/

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationById(Guid id)
        {
            //try
            //{
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
            x => x.Id.Equals(id)
            && (x.Deflag == true)
            && x.QuotationUtilities.Any()
            && x.BatchPayments.Any(),
            include: x => x.Include(x => x.Project)
                           .ThenInclude(x => x.Customer!)
                           .Include(x => x.Project)
                               .ThenInclude(x => x.HouseDesignDrawings)
                               .ThenInclude(x => x.HouseDesignVersions)
                           .Include(x => x.Promotion)
                           .Include(x => x.QuotationUtilities)
                               .ThenInclude(qu => qu.UtilitiesSection)
                               .ThenInclude(qu => qu.UtilitiesItems)
                           .Include(x => x.EquipmentItems)
                           .Include(x => x.FinalQuotationItems)
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
                //var subConstructionRepo = _unitOfWork.GetRepository<SubConstructionItem>();


                //var subConstructionItem = await subConstructionRepo.FirstOrDefaultAsync(sb => sb.Id == fqi.SubContructionId);
                Guid constructionId;
                //Guid? subConstructionId;
                //double? coefficient;
                string constructionType;
                string name;

                //if (subConstructionItem != null)
                //{
                //    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == subConstructionItem.ConstructionItemsId);
                //    subConstructionId = fqi.SubContructionId;
                //    //coefficient = subConstructionItem.Coefficient;
                //    constructionType = constructionItem?.Type?.ToLower() switch
                //    {
                //        "rough" => "Phần thô",
                //        "finished" => "Phần hoàn thiện",
                //        _ => string.Empty
                //    };
                //    name = constructionItem.Name ?? string.Empty;
                //    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                //    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                //        fqi.Id,
                //        fqi.ConstructionItemId,
                //        subConstructionId,
                //        name,
                //        fqi.Area ?? null,
                //        constructionType,
                //        //coefficient,
                //        fqi.InsDate,
                //        quotationItemsList
                //    ));
                //}
                //else
                //{
                    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionItemId);
                    if (constructionItem == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            AppConstant.ErrMessage.ConstructionIdNotfound);
                    }
                    constructionId = constructionItem.Id;
                    //coefficient = constructionItem.Coefficient;
                    constructionType = constructionItem?.Type?.ToLower() switch
                    {
                        "work_rough" => "Phần thô",
                        "work_finished" => "Phần hoàn thiện",
                        _ => string.Empty
                    };
                    name = constructionItem?.Name ?? string.Empty;
                    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                        fqi.Id,
                        constructionId,
                        //fqi.SubContructionId,
                        name,
                        //fqi.Area ?? null,
                        constructionType,
                        //coefficient,
                        fqi.InsDate,
                        quotationItemsList
                    ));
                //}
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
                    .Where(item => item.Type == AppConstant.Type.ROUGH)
                    .SelectMany(item => item.QuotationItems)
                    .GroupBy(qi => AppConstant.Type.ROUGH)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(qi => qi.TotalPriceRough ?? 0),
                        group.Sum(qi => qi.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();

            var constructionFinished = finalQuotationItemsList
                .Where(item => item.Type == AppConstant.Type.FINISHED)
                .SelectMany(item => item.QuotationItems)
                .GroupBy(qi => AppConstant.Type.FINISHED)
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
                                             .Include(x => x.InitialQuotationItems)
                                             .ThenInclude(x => x.ConstructionItem),
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
                .Where(item => item.ConstructionItem != null)
                .Select(item => new InitQuotationInfo(
                    item.ConstructionItem.Name,
                    item.Area
                ))
                .ToList();
            var houseDesignDrawingsList = finalQuotation.Project.HouseDesignDrawings.OrderBy(hd => hd.Step)
                .SelectMany(hd => hd.HouseDesignVersions)
                .Select(hd => new HouseDrawingVersionInf(
                    hd.Id,
                    hd.Name ?? string.Empty,
                    hd.Version
                ))
                .ToList();
            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.CustomerName ?? string.Empty,
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
            //    catch (Exception ex)
            //    {
            //        throw new AppConstant.MessageError((int) AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            //    }
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByProjectId(Guid projectid)
        {

            //try
            //{
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                x => x.ProjectId.Equals(projectid)
                && x.Version == 0
                && (x.Deflag == true)
                && x.QuotationUtilities.Any()
                && x.BatchPayments.Any(),
                include: x => x.Include(x => x.Project)
                               .ThenInclude(x => x.Customer!)
                               .Include(x => x.Project)
                                   .ThenInclude(x => x.HouseDesignDrawings)
                                   .ThenInclude(x => x.HouseDesignVersions)
                               .Include(x => x.Promotion)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesItem)
                                   .ThenInclude(qu => qu.Section)
                               .Include(x => x.QuotationUtilities)
                                   .ThenInclude(qu => qu.UtilitiesSection)
                                   .ThenInclude(qu => qu.UtilitiesItems)
                               .Include(x => x.EquipmentItems)
                               .Include(x => x.FinalQuotationItems)
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
            //var QuotationItems = (List<QuotationItem> quotationItems) => quotationItems.Select(qi =>
            //{
            //    return new QuotationItemResponse(
            //        qi.Id,
            //        qi.Unit ?? null,
            //        qi.Weight,
            //        qi.UnitPriceLabor,
            //        qi.UnitPriceRough,
            //        qi.UnitPriceFinished,
            //        qi.TotalPriceLabor,
            //        qi.TotalPriceRough,
            //        qi.TotalPriceFinished,
            //        qi.InsDate,
            //        qi.UpsDate,
            //        qi.Note
            //    );
            //    //if (qi.QuotationLabors.Any())
            //    //{
            //    //    var labor = qi.QuotationLabors.FirstOrDefault()?.Labor;
            //    //    var displayName = labor?.Name;
            //    //    var laborId = labor?.Id ?? Guid.Empty;
            //    //    var code = labor?.Code ?? string.Empty;
            //    //    return new QuotationItemResponse(
            //    //        qi.Id,
            //    //        laborId,
            //    //        displayName,
            //    //        //code,
            //    //        qi.Unit ?? "ca làm",
            //    //        qi.Weight,
            //    //        qi.UnitPriceLabor,
            //    //        qi.TotalPriceLabor,
            //    //        qi.InsDate,
            //    //        qi.UpsDate,
            //    //        qi.Note
            //    //    );
            //    //}
            //    //else if (qi.QuotationMaterials.Any())
            //    //{
            //    //    var material = qi.QuotationMaterials.FirstOrDefault()?.Material;
            //    //    var displayName = material?.Name;
            //    //    var materialId = material?.Id ?? Guid.Empty;
            //    //    var code = material?.Code ?? string.Empty;
            //    //    return new QuotationItemResponse(
            //    //        qi.Id,
            //    //        materialId,
            //    //        displayName,
            //    //        //code,
            //    //        qi.Unit,
            //    //        qi.Weight,
            //    //        qi.UnitPriceRough,
            //    //        qi.UnitPriceFinished,
            //    //        qi.TotalPriceRough,
            //    //        qi.TotalPriceFinished,
            //    //        qi.InsDate,
            //    //        qi.UpsDate,
            //    //        qi.Note
            //    //    );
            //    //}
            //    //else
            //    //{
            //    //    return null;
            //    //}
            //}).Where(response => response != null).ToList();

            var finalQuotationItemsList = new List<FinalQuotationItemResponse>();

            foreach (var fqi in finalQuotation.FinalQuotationItems)
            {
                var constructionItemRepo = _unitOfWork.GetRepository<ConstructionItem>();
                //var subConstructionRepo = _unitOfWork.GetRepository<SubConstructionItem>();


                //var subConstructionItem = await subConstructionRepo.FirstOrDefaultAsync(sb => sb.Id == fqi.SubContructionId);
                Guid constructionId;
                //Guid? subConstructionId;
                //double? coefficient;
                string constructionType;
                string name;

                //if (subConstructionItem != null)
                //{
                //    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == subConstructionItem.ConstructionItemsId);
                //    subConstructionId = fqi.SubContructionId;
                //    //coefficient = subConstructionItem.Coefficient;
                //    constructionType = constructionItem?.Type?.ToLower() switch
                //    {
                //        "rough" => "Phần thô",
                //        "finished" => "Phần hoàn thiện",
                //        _ => string.Empty
                //    };
                //    name = constructionItem?.Name ?? string.Empty;
                //    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                //    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                //        fqi.Id,
                //        fqi.ConstructionItemId,
                //        subConstructionId,
                //        name,
                //        fqi.Area ?? null,
                //        constructionType,
                //        //coefficient,
                //        fqi.InsDate,
                //        quotationItemsList
                //    ));
                //}
                //else
                //{
                    var constructionItem = await constructionItemRepo.FirstOrDefaultAsync(ci => ci.Id == fqi.ConstructionItemId);
                    if (constructionItem == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                            AppConstant.ErrMessage.ConstructionIdNotfound);
                    }

                    constructionId = constructionItem.Id;
                    //coefficient = constructionItem.Coefficient;
                    constructionType = constructionItem?.Type?.ToLower() switch
                    {
                        "work_rough" => "Phần thô",
                        "work_finished" => "Phần hoàn thiện",
                        _ => string.Empty
                    };
                    name = constructionItem?.Name ?? string.Empty;
                    var quotationItemsList = await QuotationItems(fqi.QuotationItems.ToList());
                    finalQuotationItemsList.Add(new FinalQuotationItemResponse(
                        fqi.Id,
                        constructionId,
                        //fqi.SubContructionId,
                        name,
                        //fqi.Area ?? null,
                        constructionType,
                        //coefficient,
                        fqi.InsDate,
                        quotationItemsList
                    ));
                //}
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
                    .Where(item => item.Type == AppConstant.Type.ROUGH)
                    .SelectMany(item => item.QuotationItems)
                    .GroupBy(qi => AppConstant.Type.ROUGH)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(qi => qi.TotalPriceRough ?? 0),
                        group.Sum(qi => qi.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();

            var constructionFinished = finalQuotationItemsList
                .Where(item => item.Type == AppConstant.Type.FINISHED)
                .SelectMany(item => item.QuotationItems)
                .GroupBy(qi => AppConstant.Type.FINISHED)
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
                                             .Include(x => x.InitialQuotationItems)
                                             .ThenInclude(x => x.ConstructionItem),
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
                .Where(item => item.ConstructionItem != null)
                .Select(item => new InitQuotationInfo(
                    item.ConstructionItem.Name,
                    item.Area
                ))
                .ToList();
            var houseDesignDrawingsList = finalQuotation.Project.HouseDesignDrawings.OrderBy(hd => hd.Step)
                .SelectMany(hd => hd.HouseDesignVersions)
                .Select(hd => new HouseDrawingVersionInf(
                    hd.Id,
                    hd.Name ?? string.Empty,
                    hd.Version
                ))
                .ToList();

            var response = new FinalQuotationResponse(
                finalQuotation.Id,
                finalQuotation.Project.CustomerName ?? string.Empty,
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
    </style>
</head>
<body>
    <h1>BÁO GIÁ CHI TIẾT NHÀ Ở DÂN DỤNG</h1>
    <p><strong>BẢNG BÁO GIÁ THI CÔNG PHẦN THÔ & NHÂN CÔNG HOÀN THIỆN</strong></p>
    <p><strong>LOẠI CÔNG TRÌNH:</strong> NHÀ Ở DÂN DỤNG</p>
    <p><strong>CHỦ ĐẦU TƯ:</strong> " + request.AccountName + @"</p>
    <p><strong>DIỆN TÍCH XÂY DỰNG:</strong> " + request.Area + @"</p>
    <h2>BẢNG TỔNG HỢP CHI PHÍ XÂY DỰNG</h2>
    <h4>HẠNG MỤC THI CÔNG: " + request.ProjectType + @"</h4>
    <h4>ĐỊA CHỈ XÂY DỰNG: " + request.ProjectAddress + @"</h4>

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
        <td colspan='4'>Cộng (chưa VAT)</td>
        <td class='highlight'>{total:N0}</td>
    </tr>
    <tr class='total'>
        <td colspan='4'>Làm tròn (chưa VAT)</td>
        <td class='highlight'>{roundedTotal:N0}</td>
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
            <th>NỘI DUNG CÔNG VIỆC</th>
            <th>DVT</th>
            <th>KHỐI LƯỢNG</th>
            <th>ĐƠN GIÁ NHÂN CÔNG</th>
            <th>ĐƠN GIÁ VẬT TƯ THÔ</th>
            <th>ĐƠN GIÁ VẬT TƯ H.T</th>
            <th>THÀNH TIỀN NHÂN CÔNG</th>
            <th>THÀNH TIỀN VẬT TƯ THÔ</th>
            <th>THÀNH TIỀN VẬT TƯ H.T</th>
            <th>GHI CHÚ</th>
        </tr>
    </thead>
    <tbody>");

            int noCons = 0;

            // Handling "ROUGH" items
            var roughItems = request.FinalQuotationItems.Where(x => x.Type == "ROUGH").ToList();
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
                    <td>{item.WorkName}</td>
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
            var finishedItems = request.FinalQuotationItems.Where(x => x.Type == "FINISHED").ToList();
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
                    <td>{item.WorkName}</td>
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


        //public bool ValidateDuplicateFinalQuotationItems(List<FinalQuotationItemRequest> items, out string? duplicateDetails)
        //{
        //    var duplicateItems = items
        //        .GroupBy(item => new { item.ConstructionId, item.SubconstructionId })
        //        .Where(g => g.Count() > 1)
        //        .Select(g => g.Key)
        //        .ToList();

        //    if (duplicateItems.Any())
        //    {
        //        duplicateDetails = string.Join(", ", duplicateItems.Select(d =>
        //            $"ConstructionId: {d.ConstructionId}, SubconstructionId: {d.SubconstructionId}"));

        //        return false;
        //    }

        //    duplicateDetails = null;
        //    return true;
        //}

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
        //public bool ValidateDuplicateLaborInConstructionOrSubconstruction(
        //    List<FinalQuotationItemRequest> items,
        //    out string? duplicateLaborDetails)
        //{
        //    var duplicates = new List<string>();

        //    var groupedItems = items.GroupBy(item => new { item.ConstructionId, item.SubconstructionId });

        //    foreach (var group in groupedItems)
        //    {
        //        var quotationItems = group
        //            .SelectMany(item => item.QuotationItems ?? new List<QuotationItemRequest>())
        //            .ToList();

        //        var duplicateLabors = quotationItems
        //            .Where(qi => qi.LaborId.HasValue)
        //            .GroupBy(qi => qi.LaborId.Value)
        //            .Where(g => g.Count() > 1)
        //            .Select(g => g.Key)
        //            .ToList();

        //        if (duplicateLabors.Any())
        //        {
        //            duplicates.Add($"ConstructionId: {group.Key.ConstructionId}, SubconstructionId: {group.Key.SubconstructionId}, Duplicate LaborIds: {string.Join(", ", duplicateLabors)}");
        //        }
        //    }

        //    if (duplicates.Any())
        //    {
        //        duplicateLaborDetails = string.Join("\n", duplicates);
        //        return false;
        //    }

        //    duplicateLaborDetails = null;
        //    return true;
        //}

        //public bool ValidateDuplicateMaterialInConstructionOrSubconstruction(
        //    List<FinalQuotationItemRequest> items,
        //    out string? duplicateMaterialDetails)
        //{
        //    var duplicates = new List<string>();

        //    var groupedItems = items.GroupBy(item => new { item.ConstructionId, item.SubconstructionId });

        //    foreach (var group in groupedItems)
        //    {
        //        var quotationItems = group
        //            .SelectMany(item => item.QuotationItems ?? new List<QuotationItemRequest>())
        //            .ToList();

        //        var duplicateMaterials = quotationItems
        //            .Where(qi => qi.MaterialId.HasValue)
        //            .GroupBy(qi => qi.MaterialId.Value)
        //            .Where(g => g.Count() > 1)
        //            .Select(g => g.Key)
        //            .ToList();

        //        if (duplicateMaterials.Any())
        //        {
        //            duplicates.Add($"ConstructionId: {group.Key.ConstructionId}, SubconstructionId: {group.Key.SubconstructionId}, Duplicate MaterialIds: {string.Join(", ", duplicateMaterials)}");
        //        }
        //    }

        //    if (duplicates.Any())
        //    {
        //        duplicateMaterialDetails = string.Join("\n", duplicates);
        //        return false;
        //    }

        //    duplicateMaterialDetails = null;
        //    return true;
        //}
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
