using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Helper;
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
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RHCQS_Services.Implement
{
    public class InitialQuotationService : IInitialQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InitialQuotationService> _logger;
        private readonly IConverter _converter;
        private readonly Cloudinary _cloudinary;

        public InitialQuotationService(IUnitOfWork unitOfWork, ILogger<InitialQuotationService> logger,
            IConverter converter, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _converter = converter;
            _cloudinary = cloudinary;
        }

        public async Task<IPaginate<InitialQuotationListResponse>> GetListInitialQuotation(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<InitialQuotation>().GetList(
                selector: x => new InitialQuotationListResponse(x.Id, x.Project.Customer!.Username!, x.Version, x.Area, x.Status),
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
                                        .Include(x => x.QuotationUtilities)
                                            .ThenInclude(x => x.UtilitiesSection)
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Payment!)
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Contract!)
                );

            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "ROUGH");

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "FINISHED");

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId == null ? null : roughPackage.PackageId,
                roughPackage?.Package.PackageName ?? string.Empty,
                roughPackage?.Package.Price ?? 0,
                finishedPackage?.PackageId == null ? null : finishedPackage.PackageId,
                finishedPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.Package.Price ?? 0,
                finishedPackage?.Package.Unit ?? string.Empty
            );
            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponse(
                            item.Id,
                            item.ConstructionItem?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId.HasValue ?
                              item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Name : item.ConstructionItem!.Name,
                            item.SubConstructionId,
                            item.Area,
                            item.AreaConstruction,
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.UtilitiesItemId ?? item.UtilitiesSectionId,
                            item.Name ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0,
                            item.Quanity ?? 0,
                            item.UtilitiesSection.UnitPrice ?? 0.0,
                            item.TotalPrice ?? 0.0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            List<BatchPaymentInfo> batchPaymentResponse = null;
            var contractInfo = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                predicate: c => c.BatchPayments.Any(c => c.Contract!.Type == AppConstant.ContractType.Construction.ToString()));

            //Case: Initial quotation version 0
            if (initialQuotation!.BatchPayments.Count == 0)
            {
                batchPaymentResponse = new List<BatchPaymentInfo>();
            }
            //Case: Initial quotation processing and not contract design
            else if  (contractInfo == null || initialQuotation.BatchPayments.Count != 0)
            {
                batchPaymentResponse = initialQuotation.BatchPayments
                .Where(bp =>
                    bp.ContractId == null
                )
                .OrderBy(bp => bp.NumberOfBatch)
                .Select(item => new BatchPaymentInfo(
                    item.PaymentId,
                    item.Payment.Description,
                    item.Payment.Percents ?? 0,
                    item.Payment.TotalPrice,
                    item.Payment.Unit,
                    item.Status,
                    item.NumberOfBatch,
                    item.Payment.PaymentDate,
                    item.Payment.PaymentPhase
                ))
                .ToList();
            }
            //Case: Contract design
            else if (contractInfo.Type == AppConstant.ContractType.Design.ToString())
            {
                batchPaymentResponse = new List<BatchPaymentInfo>();
            }
            //Case: Final quotation
            else
            {
                var firstBatchPayment = initialQuotation.BatchPayments?.FirstOrDefault(
                   predicate: c => c.Contract?.Type == AppConstant.ContractType.Construction.ToString());

                batchPaymentResponse = (await _unitOfWork.GetRepository<BatchPayment>()
                    .GetListAsync(
                        predicate: bp => bp.ContractId == firstBatchPayment!.ContractId,
                        include: bp => bp.Include(bp => bp.Payment),
                        selector: item => new BatchPaymentInfo(
                            item.PaymentId,
                            item.Payment.Description,
                            item.Payment.Percents ?? 0,
                            item.Payment.TotalPrice,
                            item.Payment.Unit,
                            item.Status,
                            item.NumberOfBatch,
                            item.Payment.PaymentDate,
                            item.Payment.PaymentPhase
                        )
                    ))?.ToList() ?? new List<BatchPaymentInfo>();
            }

            var result = new InitialQuotationResponse
            {
                ProjectType = initialQuotation.Project.Type!,
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.CustomerName!,
                PhoneNumber = initialQuotation.Project.Customer!.PhoneNumber!,
                Email = initialQuotation.Project.Customer.Email!,
                Address = initialQuotation.Project.Address!,
                ProjectId = initialQuotation.Project.Id,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                TimeRough = initialQuotation.TimeRough,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
                Note = initialQuotation.Note,
                TotalRough = initialQuotation.TotalRough,
                TotalUtilities = initialQuotation.TotalUtilities,
                TotalFinished = initialQuotation.TotalFinished,
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
                ReasonReject = initialQuotation.ReasonReject,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses,
                UtilityInfos = utiResponse,
                PromotionInfo = promotionResponse,
                BatchPaymentInfos = batchPaymentResponse
            };

            return result;
        }
        public async Task<InitialQuotationForDesignStaffResponse> GetDetailInitialQuotationByIdForDesignStaff(Guid accountId, Guid id)
        {
            var designProjects = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetListAsync(
                            predicate: x => x.AccountId == accountId,
                            include: q => q.Include(p => p.Project) 
                        );

            var projectIds = designProjects.Select(p => p.ProjectId).Distinct().ToList();


            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.Id.Equals(id) && projectIds.Contains(x.ProjectId) && x.Status == AppConstant.QuotationStatus.FINALIZED,
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
                                        .Include(x => x.QuotationUtilities)
                                            .ThenInclude(x => x.UtilitiesSection)
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Payment!)
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Contract!)
                );

            if(initialQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Scope_InitialQuotation);
            }

            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponseForDesign(
                            item.Id,
                            item.ConstructionItem?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId.HasValue ?
                              item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Name : item.ConstructionItem!.Name,
                            item.SubConstructionId,
                            item.Area,
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfoForDesign(
                            item.UtilitiesItemId ?? item.UtilitiesSectionId,
                            item.Name ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0,
                            item.Quanity ?? 0,
                            item.UtilitiesSection.UnitPrice ?? 0.0,
                            item.TotalPrice ?? 0.0
                )).ToList() ?? new List<UtilityInfoForDesign>();


            var result = new InitialQuotationForDesignStaffResponse
            {
                ProjectType = initialQuotation.Project.Type!,
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.CustomerName!,
                Address = initialQuotation.Project.Address!,
                ProjectId = initialQuotation.Project.Id,
                Area = initialQuotation.Area,
                TotalRough = initialQuotation.TotalRough,
                TotalUtilities = initialQuotation.TotalUtilities,
                ItemInitial = itemInitialResponses,
                UtilityInfos = utiResponse,
            };

            return result;
        }

        public async Task<InitialQuotationResponse> GetDetailInitialNewVersion(Guid projectId)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.ProjectId == projectId && x.Version == 0.0 && x.Project.Status != AppConstant.ProjectStatus.ENDED,
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
                                       .Include(x => x.QuotationUtilities)
                                            .ThenInclude(x => x.UtilitiesSection)
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Payment!)
                );

            if (initialQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Not_Found_InitialQuotaion);
            }

            #region Package
            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "ROUGH");

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "FINISHED");

            var packageInfo = new PackageQuotationList(
               roughPackage?.PackageId == null ? null : roughPackage.PackageId,
               roughPackage?.Package.PackageName ?? string.Empty,
               roughPackage?.Package.Price ?? 0,
               finishedPackage?.PackageId == null ? null : finishedPackage.PackageId,
               finishedPackage?.Package.PackageName ?? string.Empty,
               finishedPackage?.Package.Price ?? 0,
               finishedPackage?.Package.Unit ?? string.Empty
           );
            #endregion

            #region Initial quotation item
            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponse(
                            item.Id,
                            item.ConstructionItem?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId.HasValue ?
                              item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Name : item.ConstructionItem!.Name,
                            item.SubConstructionId,
                            item.Area,
                            item.AreaConstruction,
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();
            #endregion

            #region Utility
            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.UtilitiesItemId ?? item.UtilitiesSectionId,
                            item.Name ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0,
                            item.Quanity ?? 0,
                            item.UtilitiesSection.UnitPrice ?? 0.0,
                            item.TotalPrice ?? 0.0
                )).ToList() ?? new List<UtilityInfo>();
            #endregion

            #region Promotion
            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();
            #endregion

            #region Batchpayment
            var batchPaymentResponse = initialQuotation!.BatchPayments
                          .OrderBy(bp => bp.NumberOfBatch)
                            .Select(item => new BatchPaymentInfo(
                                         item.Id,
                                         item.Payment.Description,
                                         item.Payment.Percents ?? 0,
                                         item.Payment.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.NumberOfBatch,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                     )).ToList() ?? new List<BatchPaymentInfo>();
            #endregion

            var result = new InitialQuotationResponse
            {
                ProjectType = initialQuotation.Project.Type!,
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.CustomerName!,
                PhoneNumber = initialQuotation.Project.Customer!.PhoneNumber!,
                Email = initialQuotation.Project.Customer.Email!,
                Address = initialQuotation.Project.Address!,
                ProjectId = initialQuotation.Project.Id,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                TimeRough = initialQuotation.TimeRough,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
                Note = initialQuotation.Note,
                TotalRough = initialQuotation.TotalRough,
                TotalUtilities = initialQuotation.TotalUtilities,
                TotalFinished = initialQuotation.TotalFinished,
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
                ReasonReject = initialQuotation.ReasonReject,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses,
                UtilityInfos = utiResponse,
                PromotionInfo = promotionResponse,
                BatchPaymentInfos = batchPaymentResponse
            };

            return result;
        }

        public async Task<InitialQuotationResponse> GetDetailInitialQuotationByCustomerName(string name)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.Project!.Customer!.Username!.Equals(name),
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
                                            .ThenInclude(x => x.Payment!)

                );
            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "ROUGH");

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "FINISHED");

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId == null ? null : roughPackage.PackageId,
                roughPackage?.Package.PackageName ?? string.Empty,
                roughPackage?.Package.Price ?? 0,
                finishedPackage?.PackageId == null ? null : finishedPackage.PackageId,
                finishedPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.Package.Price ?? 0,
                finishedPackage?.Package.Unit ?? string.Empty
            );
            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponse(
                            item.Id,
                            item.ConstructionItem?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId.HasValue ?
                              item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Name : item.ConstructionItem!.Name,
                            item.SubConstructionId,
                            item.Area,
                            item.AreaConstruction,
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.UtilitiesSectionId,
                            item.Description ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0,
                            item.Quanity ?? 0,
                            item.UtilitiesItem.Section.UnitPrice ?? 0.0,
                            item.TotalPrice ?? 0.0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            var batchPaymentResponse = initialQuotation!.BatchPayments
                .OrderBy(bp => bp.NumberOfBatch)
                .Select(item => new BatchPaymentInfo(
                                         item.Id,
                                         item.Payment.Description,
                                         item.Payment.Percents ?? 0,
                                         item.Payment.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.NumberOfBatch,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                     )).ToList() ?? new List<BatchPaymentInfo>();


            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project.CustomerName!,
                Address = initialQuotation.Project.Address!,
                ProjectId = initialQuotation.Project.Id,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                TimeRough = initialQuotation.TimeRough,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
                Note = initialQuotation.Note,
                TotalRough = initialQuotation.TotalRough,
                TotalUtilities = initialQuotation.TotalUtilities,
                TotalFinished = initialQuotation.TotalFinished,
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
                ReasonReject = initialQuotation.ReasonReject,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses,
                UtilityInfos = utiResponse,
                PromotionInfo = promotionResponse,
                BatchPaymentInfos = batchPaymentResponse
            };

            return result;
        }

        public async Task<List<InitialQuotationAppResponse>> GetListInitialQuotationByProjectId(Guid projectId)
        {
            var paginatedList = await _unitOfWork.GetRepository<InitialQuotation>()
                .GetList(
                    predicate: x => x.ProjectId == projectId && x.Version != 0 &&
                               (x.Status == AppConstant.QuotationStatus.APPROVED ||
                                x.Status == AppConstant.QuotationStatus.FINALIZED ||
                                x.Status == AppConstant.QuotationStatus.UPDATING ||
                                x.Status == AppConstant.QuotationStatus.ENDED),
                    selector: x => new InitialQuotationAppResponse(
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

        public async Task<string> ApproveInitialFromManager(Guid initialId, ApproveQuotationRequest request)
        {
            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Id == initialId);

            if (initialItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                               AppConstant.ErrMessage.Not_Found_InitialQuotaion);

            if (request.Type == AppConstant.QuotationStatus.APPROVED)
            {
                initialItem.Status = AppConstant.QuotationStatus.APPROVED;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);
                var data = await GetDetailInitialQuotationById(initialItem.Id);
                try
                {
                    // Tạo HTML dựa trên dữ liệu nhận được
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
                            Folder = "InitialQuotation",
                            PublicId = $"Bao_gia_so_bo_{data.ProjectId}_{data.Version}",
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

                        var mediaInfo = new Medium
                        {
                            Id = Guid.NewGuid(),
                            HouseDesignVersionId = null,
                            Name = AppConstant.General.Initial,
                            Url = uploadResult.Url.ToString(),
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            SubTemplateId = null,
                            PaymentId = null,
                            InitialQuotationId = initialItem.Id
                        };

                        await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);
                    }
                    var isSuccessful = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Send_Fail;
                    return isSuccessful;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            else if (request.Type?.ToLower() == AppConstant.QuotationStatus.REJECTED.ToLower())
            {
                if (request.Reason == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                                               AppConstant.ErrMessage.Reason_Rejected_Required);
                }

                initialItem.Status = AppConstant.QuotationStatus.REJECTED;
                initialItem.ReasonReject = request.Reason;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);

                var isSuccessful = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Send_Fail;
                return isSuccessful;
            }
            return null;
        }

        private string GenerateHtmlContent(InitialQuotationResponse request)
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
                padding-left: 30px;
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
            }
            th,
            td {
                border: 1px solid black;
                padding: 8px;
                text-align: left;
            }
            th {
                background-color: #f0f0f0;
            }
            .center {
                text-align: right;
            }
            .total {
                background-color: #f2f2f2;
                font-weight: bold;
                color: red;
            }
        </style>
    </head>
    <body>
    <h1><strong>BẢNG BÁO GIÁ SƠ BỘ NHÀ Ở DÂN DỤNG</strong></h1>
    <p><strong>CÔNG TRÌNH:</strong> NHÀ Ở RIÊNG LẺ</p>
    <p><strong>ĐỊA ĐIỂM:</strong> " + request.Address + @"</p>
    <p><strong>CHỦ ĐẦU TƯ:</strong> " + request.AccountName + @"</p>
    <p><strong>SỐ ĐIỆN THOẠI:</strong> " + request.Version + @"</p>

    <h2>ĐIỀU 1. QUY MÔ CÔNG TRÌNH</h2>
    <p>Nhà ở dân dụng</p>
    <p>Xây dựng tại " + request.Address + @"</p>
    <p>Diện tích đất xây dựng: " + request.Area + @"</p>
    <p></p>

    <h2>ĐIỀU 2. GIÁ TRỊ HỢP ĐỒNG</h2>
    <h3>2.1. Đơn giá thi công phần thô trước thuế: " + request.PackageQuotationList.PackageRough + @" đồng/m²</h3>

    <h3>2.2. DIỆN TÍCH XÂY DỰNG THEO PHƯƠNG ÁN THIẾT KẾ:</h3>
    <table>
        <tr>
            <th colspan='6' style='text-align:left; font-weight:bold;'>Phần thô</th>
        </tr>
        <tr>
            <th>STT</th>
            <th>Hạng mục</th>
            <th>Diện tích thực tế</th>
            <th>Hệ số</th>
            <th>Diện tích</th>
            <th>Đơn vị</th>
        </tr>");

            int noCount = 1;
            foreach (var item in request.ItemInitial)
            {
                sb.Append($@"
        <tr>
            <td>{noCount}</td>
            <td>{item.Name}</td>
            <td>{item.Area}</td>
            <td>{item.Coefficient}</td>
            <td>{item.AreaConstruction}</td>
            <td>m²</td>
        </tr>");
                noCount++;
            }

            sb.Append($@"
        <tr>
            <td colspan='4' style='text-align:right;'><strong>Tổng diện tích xây dựng theo thiết kế:</strong></td>
            <td>{request.Area}</td>
            <td>m²</td>
        </tr>
    </table>

    <table>
        <tr>
            <th colspan=""4"" style=""text-align: left; font-weight: bold; border-bottom: 1px solid #000; padding-bottom: 5px;"">
            Phần hoàn thiện
        </th>
        </tr>
        <tr>
            <th>STT</th>
            <th>Hạng mục</th>
            <th>Diện tích</th>
            <th>Đơn vị</th>
        </tr>");

            int noCountFinished = 1;
                sb.Append($@"
        <tr>
            <td>{noCountFinished}</td>
            <td>Phần hoàn thiện</td>
            <td>{request.Area}</td>
            <td>m²</td>
        </tr>");
                noCountFinished++;

            sb.Append($@"
    </table>

    <h3>2.3.GIÁ TRỊ BÁO GIÁ SƠ BỘ XÂY DỰNG TRƯỚC THUẾ:</h3>
    <table>
        <tr>
            <th></th>
            <th>Tổng diện tích xây dựng</th>
            <th>x</th>
            <th>Đơn giá</th>
            <th>=</th>
            <th>Thành tiền</th>
            <th>Đơn vị</th>
        </tr>
        <tr>
            <td>Phần thô</td>
            <td>" + request.Area + @" m²</td>
            <td>x</td>
            <td>" + request.PackageQuotationList.UnitPackageRough.ToString("N0") + @"</td>
            <td>=</td>
            <td>" + (request.TotalRough?.ToString("N0") ?? "0") + @"</td>
            <td>VNĐ</td>
        </tr>
        <tr>
            <td>Phần hoàn thiện</td>
            <td>" + request.Area + @" m²</td>
            <td>x</td>
            <td>" + request.PackageQuotationList.UnitPackageFinished.ToString("N0") + @"</td>
            <td>=</td>
            <td>" + (request.TotalFinished?.ToString("N0") ?? "0") + @"</td>
            <td>VNĐ</td>
        </tr>
    </table>

    <h3>2.4. TÙY CHỌN & TIỆN ÍCH:</h3>
        <table>
            <tr>
                <th>Hạng mục</th>
                <th>Hệ số</th>
                <th>Số lượng</th>
                <th>Đơn giá</th>
                <th>Giá trị thanh toán</th>
                <th>Đơn vị</th>
            </tr>");

            foreach (var utility in request.UtilityInfos!)
            {
                sb.Append($@"
            <tr>
                <td>{utility.Description}</td>
                <td>{utility.Coefficient}</td>
                <td>{utility.Quantity}</td>
                <td>{utility.Price:N0}</td>
                <td>{(utility.Coefficient * utility.Price):N0}</td>
                <td>VNĐ</td>
            </tr>");
            }

            sb.Append("</table>");

            if (request.PromotionInfo != null && !string.IsNullOrEmpty(request.PromotionInfo.Name))
            {
                sb.Append($@"
                    <h3>2.5. KHUYẾN MÃI:</h3>
                    <table>
                        <tr>
                            <th>Tên khuyến mãi</th>
                            <th>Giá trị</th>
                            <th>Tổng giảm</th>
                        </tr>
                        <tr>
                            <td>{request.PromotionInfo.Name}</td>
                            <td>{request.PromotionInfo.Value}</td>
                            <td>{request.Discount}</td>
                        </tr>
                    </table>");
            }

            sb.Append($@"
    <h3>2.6. TỔNG GIÁ TRỊ HỢP ĐỒNG:</h3>
        <table>
            <tr>
                <th>Mô tả</th>
                <th>Giá trị</th>
                <th>Đơn vị</th>
            </tr>
            <tr>
                <td>Giá trị báo giá sơ bộ xây dựng trước thuế</td>
                <td>{request.TotalRough:N0}</td>
                <td>VNĐ</td>
            </tr>
            <tr>
                <td>Tùy chọn & Tiện ích</td>
                <td>{request.TotalUtilities:N0}</td>
                <td>VNĐ</td>
            </tr>
            <tr>
                <td>{request.PromotionInfo.Name}</td>
                <td>{request.Discount:N0}</td>
                <td>VNĐ</td>
            </tr>
        </table>
    
    <h2>ĐIỀU 3. PHƯƠNG THỨC THANH TOÁN</h2>
    <p>Tổng giá trị hợp đồng sẽ được thanh toán theo các đợt sau:</p>
    <table>
        <tr>
            <th>Đợt</th>
            <th>Nội dung thanh toán</th>
            <th>Giá trị (%)</th>
            <th>Giá trị (VND)</th>
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
        </tr>");
                stageCounter++;
            }

            sb.Append(@"
    </table>

    <h2>ĐIỀU 4. THỜI GIAN THI CÔNG</h2>
    <p>Thời gian hoàn thành công trình là: <strong>" + request.TimeProcessing + @"</strong> ngày</p>
    <p>Thời gian thi công phần thô: <strong>" + request.TimeRough + @"</strong> ngày</p>
    <p>Thời gian thi công phần hoàn thiện: <strong>" + request.TimeOthers + @" </strong></p>
            ");

            sb.Append(@"
    <h2>ĐIỀU 5. CÁC THỎA THUẬN KHÁC</h2>
    <ul>");


            if (!string.IsNullOrEmpty(request.OthersAgreement))
            {
                var agreements = request.OthersAgreement.Split('-')
                                    .Where(a => !string.IsNullOrWhiteSpace(a))
                                    .Select(a => a.Trim());

                foreach (var agreement in agreements)
                {
                    sb.Append($@"
            <li>{agreement}</li>");
                }
            }

            sb.Append("</ul>");

            sb.Append(@"
            <p class='center'>Ngày …… tháng …… năm ……</p>

            <div class='signature'>
                <div class='signature-row'>
                    <div class='signature-column'>
                        CHỦ ĐẦU TƯ<br />
                        <strong></strong>
                    </div>
                    <div class='signature-column'>
                        NHÀ THẦU<br />
                        <strong></strong>
                    </div>
                </div>
            </div>
            </body>
            </html>");

            return sb.ToString();
        }

        public async Task<bool> UpdateInitialQuotation(UpdateInitialRequest request)
        {
            try
            {
                #region Check version present duplicate
                double nextVersion = 1.0;
                var highestInitial = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                    predicate: x => x.ProjectId == request.ProjectId,
                                    orderBy: x => x.OrderByDescending(x => x.Version),
                                    include: x => x.Include(x => x.Project)
                                );

                if (highestInitial.Version >= AppConstant.General.MaxVersion)
                {
                    highestInitial.Project.Status = AppConstant.ProjectStatus.ENDED;
                    _unitOfWork.GetRepository<Project>().UpdateAsync(highestInitial.Project);
                    await _unitOfWork.CommitAsync();
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.MaxVersionQuotation);
                }

                if (highestInitial != null)
                {
                    nextVersion = highestInitial.Version + 1;

                    var duplicateVersion = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        predicate: x => x.ProjectId == request.ProjectId && x.Version == nextVersion
                    );

                    if (duplicateVersion != null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Conflict_Version);
                    }
                }
                #endregion

                #region Check promotion
                if (request.Promotions != null)
                {
                    var promotionInfo = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(
                                    predicate: p => p.Id == request.Promotions.Id && p.ExpTime >= LocalDateTime.VNDateTime() && p.IsRunning == true,
                                    include: p => p.Include(p => p.PackageMapPromotions)
                                                    .ThenInclude(p => p.Package));

                    if (promotionInfo == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.PromotionIllegal);
                    }

                    if (!request.Packages
                        .Any(package => promotionInfo.PackageMapPromotions
                            .Any(p => p.PackageId == package.PackageId)))
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.PromotionIllegal);
                    }

                    //double discountCheck = (double)request.Area * (double)promotionInfo.Value;
                    //if (discountCheck != request.Promotions.Discount)
                    //{
                    //    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.InvalidDiscount);
                    //}
                }
                #endregion

                #region Check request duplicate
                //Construction 
                var isValidContruction = ValidateDuplicateConstructionItems(request.Items, out var duplicateNames);
                if (!isValidContruction)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedConstruction);
                }

                //Utility
                var isValidUtility = ValidateDuplicateUtilities(request.Utilities!, out var duplicateIds);
                if (!isValidUtility)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.DuplicatedUtility);
                }


                #endregion

                #region Update project & version present
                var initialVersionPresent = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                predicate: x => x.Version == request.VersionPresent && x.ProjectId == request.ProjectId,
                    include: x => x.Include(x => x.Project));

                initialVersionPresent.Project.Area = (request.Area.HasValue && request.Area.Value != 0.0) ?
                                    request.Area.Value : initialVersionPresent.Project.Area;
                initialVersionPresent.Project.Address = string.IsNullOrEmpty(request.Address) ?
                                      initialVersionPresent.Project.Address : request.Address;
                initialVersionPresent.Project.UpsDate = LocalDateTime.VNDateTime();

                initialVersionPresent.Project.CustomerName = string.IsNullOrEmpty(request.AccountName) ?
                                      initialVersionPresent.Project.CustomerName : request.AccountName;
                //Note: Version initial quotation - PROCESSING
                initialVersionPresent.Status = AppConstant.QuotationStatus.ENDED;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialVersionPresent);
                #endregion

                #region Create initial quotation
                var initialItem = new InitialQuotation()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = request.ProjectId,
                    PromotionId = request.Promotions?.Id != null ? request.Promotions.Id : (Guid?)null,
                    Area = request.Area,
                    TimeProcessing = request.TimeProcessing,
                    TimeRough = request.TimeRough,
                    TimeOthers = request.TimeOthers,
                    OthersAgreement = request.OthersAgreement,
                    InsDate = LocalDateTime.VNDateTime(),
                    Status = AppConstant.QuotationStatus.REVIEWING,
                    Version = nextVersion,
                    IsTemplate = false,
                    Deflag = true,
                    Note = null,
                    TotalRough = request.TotalRough,
                    TotalUtilities = request.TotalUtilities,
                    TotalFinished = request.TotalFinished,
                    Unit = AppConstant.Unit.UnitPrice,
                    ReasonReject = null,
                    IsDraft = true,
                    Discount = request.Promotions?.Discount * request.Area
                };
                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);
                #endregion

                #region Create initial quotation item
                foreach (var item in request.Items!)
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
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        InitialQuotationId = initialItem.Id,
                        AreaConstruction =  item.AreaConstruction
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(itemInitial);
                }
                #endregion

                #region Create package quotation
                if (request.Packages.Count < 1)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                        AppConstant.ErrMessage.InvalidPackageQuotation);
                }

                foreach (var package in request.Packages!)
                {
                    var packageQuotation = new PackageQuotation
                    {
                        Id = Guid.NewGuid(),
                        PackageId = package.PackageId,
                        InitialQuotationId = initialItem.Id,
                        Type = package.Type,
                        InsDate = LocalDateTime.VNDateTime()
                    };

                    await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageQuotation);
                }
                #endregion

                #region Create Utility
                if (request.Utilities.Count > 0)
                {
                    foreach (var utl in request.Utilities!)
                    {
                        var utilityItem = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == utl.UtilitiesItemId);
                        Guid? sectionId = null;
                        QuotationUtility utlItem;
                        //UtilityItem - null => utl.UtilitiesItem = SectionId
                        //UtilityItem != null => utl.UltilitiesItemId = UtilityItem.Id, SectionId = UltilitiesItemId.SectionId
                        if (utilityItem == null)
                        {
                            sectionId = utl.UtilitiesItemId;
                            var sectionItem = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == sectionId);
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = null,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = sectionItem.Name!,
                                Coefficient = 0,
                                Price = utl.Price,
                                Description = sectionItem.Description,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = sectionItem.Id,
                                Quanity = utl.Quantity,
                                TotalPrice = utl.TotalPrice
                            };
                        }
                        else
                        {
                            sectionId = utilityItem.SectionId;
                            utl.UtilitiesItemId = utilityItem.Id;
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = utilityItem.Id,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = utilityItem.Name!,
                                Coefficient = utilityItem.Coefficient,
                                Price = utl.Price,
                                Description = null,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = utilityItem.SectionId,
                                Quanity = utl.Quantity,
                                TotalPrice = utl.TotalPrice
                            };
                        }

                        await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                    }
                }
                #endregion

                #region Create batch payment
                if (request.BatchPayments.Count < 1)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                        AppConstant.ErrMessage.InvalidBatchPayment);
                }
                var paymentType = await _unitOfWork.GetRepository<PaymentType>().FirstOrDefaultAsync(x => x.Name == AppConstant.General.PaymentDesign);
                if (paymentType == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Type_Not_Found);
                }

                //Create a batch payments
                DateTime? previousPaymentDate = null;
                foreach (var item in request.BatchPayments)
                {
                    if (item.PaymentDate >= item.PaymentPhase)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                                    $"Ngày bắt đầu hoặc ngày đáo hạn không hợp lệ {item.PaymentDate}!");
                    }

                    if (previousPaymentDate != null && item.PaymentDate < previousPaymentDate)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                            $"Thời gian thanh toán {item.PaymentPhase} ({item.PaymentDate}) không nhỏ hơn thời gian ({previousPaymentDate}).");
                    }

                    previousPaymentDate = item.PaymentDate;

                    int batch = 0;
                    var payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentTypeId = paymentType.Id,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        TotalPrice = item.Price,
                        PaymentDate = item.PaymentDate,
                        PaymentPhase = item.PaymentPhase,
                        Percents = item.Percents,
                        Description = item.Description,
                        Unit = AppConstant.Unit.UnitPrice
                    };
                    await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);

                    var payItem = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        ContractId = null,
                        InitialQuotationId = initialItem.Id,
                        InsDate = LocalDateTime.VNDateTime(),
                        FinalQuotationId = null,
                        PaymentId = payment.Id,
                        Status = AppConstant.PaymentStatus.PROGRESS,
                        NumberOfBatch = item.NumberOfBatch
                    };
                    await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(payItem);
                }
                #endregion

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }

        public async Task<string> ConfirmArgeeInitialFromCustomer(Guid quotationId)
        {
            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Id == quotationId,
                                    include: x => x.Include(x => x.Project)
                                                    .ThenInclude(x => x.Customer!));

            if (initialItem != null)
            {
                //if (initialItem.Status == AppConstant.QuotationStatus.ENDED)
                //{
                //    throw new AppConstant.MessageError(
                //       (int)AppConstant.ErrCode.Conflict,
                //       AppConstant.ErrMessage.Ended_Quotation
                //   );
                //}
                var projectQuotations = await _unitOfWork.GetRepository<InitialQuotation>()
                        .GetListAsync(predicate: x => x.ProjectId == initialItem.ProjectId);

                if (projectQuotations.Any(x => x.Status == AppConstant.QuotationStatus.FINALIZED))
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.Already_Finalized_Quotation
                    );
                }



                //Update status present quotation
                initialItem.Status = AppConstant.QuotationStatus.FINALIZED;
                //Update area project following finalized initial quotation
                initialItem.Project.Area = initialItem.Area;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);

                //Update all version quotation - version present
                foreach (var quotation in projectQuotations.Where(x => x.Id != initialItem.Id))
                {
                    quotation.Status = AppConstant.QuotationStatus.ENDED;
                    _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(quotation);
                }

                var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
                return isSuccessful;
            }
            else if (initialItem == null)
            {
                var finalInfo = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == quotationId,
                                            include: x => x.Include(x => x.Project)
                                                            .ThenInclude(x => x.Customer!));
                if (finalInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                #region Check quotation FINALIZED
                var projectQuotations = await _unitOfWork.GetRepository<FinalQuotation>()
                        .GetListAsync(predicate: x => x.ProjectId == finalInfo.ProjectId);

                if (projectQuotations.Any(x => x.Status == AppConstant.QuotationStatus.FINALIZED))
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.Already_Finalized_Quotation
                    );
                }
                #endregion

                //Update status present quotation
                finalInfo.Status = AppConstant.QuotationStatus.FINALIZED;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalInfo);

                //Update all version quotation - version present
                foreach (var quotation in projectQuotations.Where(x => x.Id != finalInfo.Id))
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

        public async Task<string> FeedbackFixInitialFromCustomer(Guid initialId, FeedbackQuotationRequest comment)
        {
            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Id == initialId);

            if (initialItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Quotation);
            }

            if(initialItem.Status == AppConstant.QuotationStatus.ENDED || 
                initialItem.Status == AppConstant.QuotationStatus.UPDATING)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Not_Comment_Quotation);
            }

            initialItem.Note = comment.Note;
            initialItem.Status = AppConstant.QuotationStatus.UPDATING;
            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }

        public bool ValidateDuplicateConstructionItems(List<InitialQuotaionItemUpdateRequest> items, out string? duplicateNames)
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

        public bool ValidateDuplicateUtilities(List<UtilitiesUpdateRequest> items, out List<Guid>? duplicateIds)
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

        public async Task<string> GetStatusInitialQuotation(Guid initialId)
        {
            var initialInfo = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(predicate: i => i.Id == initialId);
            if (initialInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Not_Found_InitialQuotaion);
            }
            string result = initialInfo.Status;
            return result;
        }
    }
}
