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

            finalQuotation.Status = AppConstant.QuotationStatus.CANCELED;
            finalQuotation.ReasonReject = reason.ReasonCancel;
            finalQuotation.UpsDate = DateTime.UtcNow;
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

        public async Task<bool> CreateFinalQuotation(FinalRequest request)
        {
            if (request == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();
            if (await finalQuotationRepo.AnyAsync(p => p.ProjectId == request.ProjectId && p.Version == request.VersionPresent))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PackageExists
                );
            }

            var finalQuotation = new FinalQuotation
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                PromotionId = request.PromotionId,
                TotalPrice = request.TotalPrice,
                Note = request.Note,
                Version = request.VersionPresent,
                InsDate = DateTime.UtcNow,
                Status = request.Status,
                Deflag = true,
                BatchPayments = new List<BatchPayment>()
            };

            foreach (var bp in request.BatchPaymentInfos)
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    PaymentTypeId = bp.PaymentTypeId,
                    InsDate = DateTime.UtcNow,
                    TotalPrice = bp.Price,
                    Percents = bp.Percents,
                    Description = bp.Description,
                    Unit = AppConstant.Unit.UnitPrice
                };

                var batchPayment = new BatchPayment
                {
                    Id = Guid.NewGuid(),
                    IntitialQuotationId = bp.InitIntitialQuotationId,
                    ContractId = bp.ContractId,
                    InsDate = DateTime.UtcNow,
                    FinalQuotationId = finalQuotation.Id,
                    Payment = payment,
                    Status = bp.Status
                };

                finalQuotation.BatchPayments.Add(batchPayment);
            }

            finalQuotation.EquipmentItems = request.EquipmentItems.Select(ei => new EquipmentItem
            {
                Id = Guid.NewGuid(),
                Name = ei.Name,
                Unit = ei.Unit,
                Quantity = ei.Quantity,
                UnitOfMaterial = ei.UnitOfMaterial,
                TotalOfMaterial = ei.TotalOfMaterial,
                Note = ei.Note
            }).ToList();

            finalQuotation.FinalQuotationItems = request.FinalQuotationItems.Select(fqi => new FinalQuotationItem
            {
                Id = Guid.NewGuid(),
                ConstructionItemId = fqi.ConstructionItemId,
                QuotationItems = fqi.QuotationItems.Select(qi => new QuotationItem
                {
                    Unit = qi.Unit,
                    Weight = qi.Weight,
                    UnitPriceLabor = qi.UnitPriceLabor,
                    UnitPriceRough = qi.UnitPriceRough,
                    UnitPriceFinished = qi.UnitPriceFinished,
                    TotalPriceLabor = qi.TotalPriceLabor,
                    TotalPriceRough = qi.TotalPriceRough,
                    TotalPriceFinished = qi.TotalPriceFinished,
                    Note = qi.Note,
                    QuotationLabors = qi.QuotationLabors.Select(ql => new QuotationLabor
                    {
                        LaborId = ql.LaborId,
                        LaborPrice = ql.LaborPrice
                    }).ToList(),
                    QuotationMaterials = qi.QuotationMaterials.Select(qm => new QuotationMaterial
                    {
                        MaterialId = qm.MaterialId,
                        Unit = qm.Unit,
                        MaterialPrice = qm.MaterialPrice
                    }).ToList()
                }).ToList()
            }).ToList();

            await finalQuotationRepo.InsertAsync(finalQuotation);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CreatePackage
                );
            }
            return isSuccessful;
        }

        public async Task<string> ApproveFinalFromManager(Guid finalId, ApproveQuotationRequest request)
        {
            var finalItem = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(x => x.Id == finalId);

            if (finalItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                               AppConstant.ErrMessage.Not_Found_FinalQuotaion);

            if (request.Type == AppConstant.QuotationStatus.APPROVED)
            {
                finalItem.Status = AppConstant.QuotationStatus.APPROVED;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalItem);
                var data = await GetDetailFinalQuotationById(finalItem.Id);
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

/*                    string dllPath = Path.Combine(AppContext.BaseDirectory, "ExternalLibraries", "libwkhtmltox.dll");
                    NativeLibrary.Load(dllPath);*/

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
                            InsDate = DateTime.Now,
                            UpsDate = DateTime.Now,
                            SubTemplateId = null,
                            PaymentId = null,
                            FinalQuotationId = finalItem.Id,
                            InitialQuotationId = null,
                        };

                        await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);
                        _unitOfWork.Commit();

                        return uploadResult.SecureUrl.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                if (finalItem.ReasonReject == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                                               AppConstant.ErrMessage.Reason_Rejected_Required);
                }
                finalItem.Status = AppConstant.QuotationStatus.REJECTED;
                finalItem.ReasonReject = request.Reason;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalItem);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return null;
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByCustomerName(string name)
        {
            try
            {
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                    x => x.Project.Customer.Username.Equals(name) && (x.Deflag == true),
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer!)
                                   .Include(x => x.Promotion)
                                   .Include(x => x.QuotationUtilities)
                                       .ThenInclude(qu => qu.UtilitiesItem)
                                   .Include(x => x.EquipmentItems)
                                   .Include(x => x.FinalQuotationItems)
                                       .ThenInclude(co => co.ConstructionItem)
                                       .ThenInclude(sb => sb.SubConstructionItems)
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
                    );

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                       AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                var BatchPayments = () => finalQuotation.BatchPayments.Select(bp =>
                    new BatchPaymentResponse(
                        bp?.Payment.Id ?? Guid.Empty,
                        bp.InsDate,
                        bp.Status,
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
                        ei.Note
                    )
                ).ToList();

                var QuotationLabors = (QuotationItem qi) => qi.QuotationLabors.Select(ql =>
                    new QuotationLaborResponse(
                        ql.Id,
                        ql.Labor.Name,
                        ql.LaborPrice
                    )
                ).ToList();

                var QuotationMaterials = (QuotationItem qi) => qi.QuotationMaterials.Select(qm =>
                    new QuotationMaterialResponse(
                        qm.Id,
                        qm.Material.Name,
                        qm.Unit,
                        qm.MaterialPrice
                    )
                ).ToList();

                var QuotationItems = (List<QuotationItem> quotationItems) => quotationItems.Select(qi =>
                {
                    var displayName = qi.QuotationLabors.Any()
                        ? qi.QuotationLabors.FirstOrDefault()?.Labor.Name
                        : (qi.QuotationMaterials.Any()
                            ? qi.QuotationMaterials.FirstOrDefault()?.Material.Name
                            : null);

                    return new QuotationItemResponse(
                        qi.Id,
                        displayName,
                        qi.Unit,
                        qi.Weight,
                        qi.UnitPriceLabor,
                        qi.UnitPriceRough,
                        qi.UnitPriceFinished,
                        qi.TotalPriceLabor,
                        qi.TotalPriceRough,
                        qi.TotalPriceFinished,
                        qi.InsDate,
                        qi.UpsDate,
                        qi.Note,
                        QuotationLabors(qi),
                        QuotationMaterials(qi)
                    );
                }).ToList();


                var finalQuotationItemsList = finalQuotation.FinalQuotationItems.Select(fqi =>
                {
                    var subConstructionItem = fqi.ConstructionItem?.SubConstructionItems.FirstOrDefault();

                    return new FinalQuotationItemResponse(
                        fqi.Id,
                        subConstructionItem != null ? subConstructionItem.Name : fqi.ConstructionItem?.Name,
                        fqi.ConstructionItem?.Type,
                        subConstructionItem != null ? subConstructionItem.Coefficient : fqi.ConstructionItem?.Coefficient,
                        fqi.InsDate,
                        QuotationItems(fqi.QuotationItems.ToList())
                    );
                }).ToList();


                var batchPaymentsList = BatchPayments();
                var equipmentItemsList = EquipmentItems();

                var promotionInfo = finalQuotation.Promotion != null
                    ? new PromotionInfo(
                        finalQuotation.Promotion.Id,
                        finalQuotation.Promotion.Name,
                        finalQuotation.Promotion.Value
                    )
                    : null;

                var utilityInfoList = finalQuotation.QuotationUtilities != null && finalQuotation.QuotationUtilities.Any()
                    ? finalQuotation.QuotationUtilities.Select(qUtility => new UtilityInf(
                        qUtility.Id,
                        qUtility.Description,
                        qUtility.Coefiicient ?? 0,
                        qUtility.Price ?? 0,
                        qUtility.UtilitiesItem.Section.UnitPrice ?? 0,
                        qUtility.UtilitiesItem.Section.Unit
                    )).ToList()
                    : new List<UtilityInf>();

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
                    (double)equipmentCost,
                    0
                );
                var response = new FinalQuotationResponse(
                    finalQuotation.Id,
                    finalQuotation.Project.Customer.Username,
                    finalQuotation.ProjectId,
                    finalQuotation.Project.Type,
                    finalQuotation.Project.Address,
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
                    constructionRough,
                    constructionFinished,
                    equipmentCostSummary
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }


        public async Task<FinalQuotationResponse> GetDetailFinalQuotationById(Guid id)
        {
            try
            {
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                    x => x.Id.Equals(id) && (x.Deflag == true),
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer!)
                                   .Include(x => x.Promotion)
                                   .Include(x => x.QuotationUtilities)
                                       .ThenInclude(qu => qu.UtilitiesItem)
                                   .Include(x => x.EquipmentItems)
                                   .Include(x => x.FinalQuotationItems)
                                       .ThenInclude(co => co.ConstructionItem)
                                       .ThenInclude(sb => sb.SubConstructionItems)
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
                    );

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                       AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                var BatchPayments = () => finalQuotation.BatchPayments.Select(bp =>
                    new BatchPaymentResponse(
                        bp?.Payment!.Id ?? Guid.Empty,
                        bp.InsDate,
                        bp.Status,
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
                        ei.Note
                    )
                ).ToList();

                var QuotationLabors = (QuotationItem qi) => qi.QuotationLabors.Select(ql =>
                    new QuotationLaborResponse(
                        ql.Id,
                        ql.Labor.Name,
                        ql.LaborPrice
                    )
                ).ToList();

                var QuotationMaterials = (QuotationItem qi) => qi.QuotationMaterials.Select(qm =>
                    new QuotationMaterialResponse(
                        qm.Id,
                        qm.Material.Name,
                        qm.Unit,
                        qm.MaterialPrice
                    )
                ).ToList();

                var QuotationItems = (List<QuotationItem> quotationItems) => quotationItems.Select(qi =>
                {
                    var displayName = qi.QuotationLabors.Any()
                        ? qi.QuotationLabors.FirstOrDefault()?.Labor.Name
                        : (qi.QuotationMaterials.Any()
                            ? qi.QuotationMaterials.FirstOrDefault()?.Material.Name
                            : null);

                    return new QuotationItemResponse(
                        qi.Id,
                        displayName,
                        qi.Unit,
                        qi.Weight,
                        qi.UnitPriceLabor,
                        qi.UnitPriceRough,
                        qi.UnitPriceFinished,
                        qi.TotalPriceLabor,
                        qi.TotalPriceRough,
                        qi.TotalPriceFinished,
                        qi.InsDate,
                        qi.UpsDate,
                        qi.Note,
                        QuotationLabors(qi),
                        QuotationMaterials(qi)
                    );
                }).ToList();

                var finalQuotationItemsList = finalQuotation.FinalQuotationItems.Select(fqi =>
                {
                    var subConstructionItem = fqi.ConstructionItem?.SubConstructionItems.FirstOrDefault();

                    return new FinalQuotationItemResponse(
                        fqi.Id,
                        subConstructionItem != null ? subConstructionItem.Name : fqi.ConstructionItem?.Name,
                        fqi.ConstructionItem?.Type,
                        subConstructionItem != null ? subConstructionItem.Coefficient : fqi.ConstructionItem?.Coefficient,
                        fqi.InsDate,
                        QuotationItems(fqi.QuotationItems.ToList())
                    );
                }).ToList();


                var batchPaymentsList = BatchPayments();
                var equipmentItemsList = EquipmentItems();

                var promotionInfo = finalQuotation.Promotion != null
                    ? new PromotionInfo(
                        finalQuotation.Promotion.Id,
                        finalQuotation.Promotion.Name,
                        finalQuotation.Promotion.Value
                    )
                    : null;

                var utilityInfoList = finalQuotation.QuotationUtilities != null && finalQuotation.QuotationUtilities.Any()
                    ? finalQuotation.QuotationUtilities.Select(qUtility => new UtilityInf(
                        qUtility.Id,
                        qUtility.Description,
                        qUtility.Coefiicient ?? 0,
                        qUtility.Price ?? 0,
                        qUtility.UtilitiesItem.Section.UnitPrice ?? 0,
                        qUtility.UtilitiesItem.Section.Unit
                    )).ToList()
                    : new List<UtilityInf>();

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
                    (double)equipmentCost,
                    0             
                );
                var response = new FinalQuotationResponse(
                    finalQuotation.Id,
                    finalQuotation.Project.Customer.Username,
                    finalQuotation.ProjectId,
                    finalQuotation.Project.Type,
                    finalQuotation.Project.Address,
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
                    constructionRough,
                    constructionFinished,
                    equipmentCostSummary
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }

        public async Task<FinalQuotationResponse> GetDetailFinalQuotationByProjectId(Guid projectid)
        {

            try
            {
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                    x => x.ProjectId.Equals(projectid) && (x.Deflag == true),
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer!)
                                   .Include(x => x.Promotion)
                                   .Include(x => x.QuotationUtilities)
                                       .ThenInclude(qu => qu.UtilitiesItem)
                                   .Include(x => x.EquipmentItems)
                                   .Include(x => x.FinalQuotationItems)
                                       .ThenInclude(co => co.ConstructionItem)
                                       .ThenInclude(sb => sb.SubConstructionItems)
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
                    );

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                       AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                var BatchPayments = () => finalQuotation.BatchPayments.Select(bp =>
                    new BatchPaymentResponse(
                        bp?.Payment.Id ?? Guid.Empty,
                        bp.InsDate,
                        bp.Status,
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
                        ei.Note
                    )
                ).ToList();

                var QuotationLabors = (QuotationItem qi) => qi.QuotationLabors.Select(ql =>
                    new QuotationLaborResponse(
                        ql.Id,
                        ql.Labor.Name,
                        ql.LaborPrice
                    )
                ).ToList();

                var QuotationMaterials = (QuotationItem qi) => qi.QuotationMaterials.Select(qm =>
                    new QuotationMaterialResponse(
                        qm.Id,
                        qm.Material.Name,
                        qm.Unit,
                        qm.MaterialPrice
                    )
                ).ToList();

                var QuotationItems = (List<QuotationItem> quotationItems) => quotationItems.Select(qi =>
                {
                    var displayName = qi.QuotationLabors.Any()
                        ? qi.QuotationLabors.FirstOrDefault()?.Labor.Name
                        : (qi.QuotationMaterials.Any()
                            ? qi.QuotationMaterials.FirstOrDefault()?.Material.Name
                            : null);

                    return new QuotationItemResponse(
                        qi.Id,
                        displayName,
                        qi.Unit,
                        qi.Weight,
                        qi.UnitPriceLabor,
                        qi.UnitPriceRough,
                        qi.UnitPriceFinished,
                        qi.TotalPriceLabor,
                        qi.TotalPriceRough,
                        qi.TotalPriceFinished,
                        qi.InsDate,
                        qi.UpsDate,
                        qi.Note,
                        QuotationLabors(qi),
                        QuotationMaterials(qi)
                    );
                }).ToList();

                var finalQuotationItemsList = finalQuotation.FinalQuotationItems.Select(fqi =>
                {
                    var subConstructionItem = fqi.ConstructionItem?.SubConstructionItems.FirstOrDefault();

                    return new FinalQuotationItemResponse(
                        fqi.Id,
                        subConstructionItem != null ? subConstructionItem.Name : fqi.ConstructionItem?.Name,
                        fqi.ConstructionItem?.Type,
                        subConstructionItem != null ? subConstructionItem.Coefficient : fqi.ConstructionItem?.Coefficient,
                        fqi.InsDate,
                        QuotationItems(fqi.QuotationItems.ToList())
                    );
                }).ToList();


                var batchPaymentsList = BatchPayments();
                var equipmentItemsList = EquipmentItems();

                var promotionInfo = finalQuotation.Promotion != null
                    ? new PromotionInfo(
                        finalQuotation.Promotion.Id,
                        finalQuotation.Promotion.Name,
                        finalQuotation.Promotion.Value
                    )
                    : null;

                var utilityInfoList = finalQuotation.QuotationUtilities != null && finalQuotation.QuotationUtilities.Any()
                    ? finalQuotation.QuotationUtilities.Select(qUtility => new UtilityInf(
                        qUtility.Id,
                        qUtility.Description,
                        qUtility.Coefiicient ?? 0,
                        qUtility.Price ?? 0,
                        qUtility.UtilitiesItem.Section.UnitPrice ?? 0,
                        qUtility.UtilitiesItem.Section.Unit
                    )).ToList()
                    : new List<UtilityInf>();

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
                    (double)equipmentCost,
                    0
                );
                var response = new FinalQuotationResponse(
                    finalQuotation.Id,
                    finalQuotation.Project.Customer.Username,
                    finalQuotation.ProjectId,
                    finalQuotation.Project.Type,
                    finalQuotation.Project.Address,
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
                    constructionRough,
                    constructionFinished,
                    equipmentCostSummary
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }

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

        public async Task<bool> UpdateFinalQuotation(FinalRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Bad_Request,
                        AppConstant.ErrMessage.NullValue
                    );
                }

                var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();
                if (await finalQuotationRepo.AnyAsync(p => p.ProjectId == request.ProjectId && p.Version == request.VersionPresent))
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.PackageExists
                    );
                }

                var finalQuotation = new FinalQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = request.ProjectId,
                    PromotionId = request.PromotionId,
                    TotalPrice = request.TotalPrice,
                    Note = request.Note,
                    Version = request.VersionPresent + 1,
                    InsDate = DateTime.UtcNow,
                    Status = request.Status,
                    Deflag = true,
                    BatchPayments = new List<BatchPayment>()
                };

                foreach (var bp in request.BatchPaymentInfos)
                {
                    var payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentTypeId = bp.PaymentTypeId,
                        InsDate = DateTime.UtcNow,
                        TotalPrice = bp.Price,
                        Percents = bp.Percents,
                        Description = bp.Description,
                        Unit = AppConstant.Unit.UnitPrice
                    };

                    var batchPayment = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        IntitialQuotationId = bp.InitIntitialQuotationId,
                        ContractId = bp.ContractId,
                        InsDate = DateTime.UtcNow,
                        FinalQuotationId = finalQuotation.Id,
                        Payment = payment,
                        Status = bp.Status
                    };

                    finalQuotation.BatchPayments.Add(batchPayment);
                }

                finalQuotation.EquipmentItems = request.EquipmentItems.Select(ei => new EquipmentItem
                {
                    Id = Guid.NewGuid(),
                    Name = ei.Name,
                    Unit = ei.Unit,
                    Quantity = ei.Quantity,
                    UnitOfMaterial = ei.UnitOfMaterial,
                    TotalOfMaterial = ei.TotalOfMaterial,
                    Note = ei.Note
                }).ToList();

                finalQuotation.FinalQuotationItems = request.FinalQuotationItems.Select(fqi => new FinalQuotationItem
                {
                    Id = Guid.NewGuid(),
                    ConstructionItemId = fqi.ConstructionItemId,
                    QuotationItems = fqi.QuotationItems.Select(qi => new QuotationItem
                    {
                        Unit = qi.Unit,
                        Weight = qi.Weight,
                        UnitPriceLabor = qi.UnitPriceLabor,
                        UnitPriceRough = qi.UnitPriceRough,
                        UnitPriceFinished = qi.UnitPriceFinished,
                        TotalPriceLabor = qi.TotalPriceLabor,
                        TotalPriceRough = qi.TotalPriceRough,
                        TotalPriceFinished = qi.TotalPriceFinished,
                        Note = qi.Note,
                        QuotationLabors = qi.QuotationLabors.Select(ql => new QuotationLabor
                        {
                            LaborId = ql.LaborId,
                            LaborPrice = ql.LaborPrice
                        }).ToList(),
                        QuotationMaterials = qi.QuotationMaterials.Select(qm => new QuotationMaterial
                        {
                            MaterialId = qm.MaterialId,
                            Unit = qm.Unit,
                            MaterialPrice = qm.MaterialPrice
                        }).ToList()
                    }).ToList()
                }).ToList();

                await finalQuotationRepo.InsertAsync(finalQuotation);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.CreatePackage
                    );
                }
                return isSuccessful;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
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
            string roughTypeDisplay = rough.Type == "ROUGH" ? "Phần thô" : rough.Type;
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
            string finishedTypeDisplay = finished.Type == "FINISHED" ? "Phần hoàn thiện" : finished.Type;
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
            string equipmentTypeDisplay = equipmentCostSummary.Type == "EQUIPMENT" ? "Phần thiết bị" : equipmentCostSummary.Type;

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

                    string utilityTypeDisplay = "Phần tiện ích";

                    sb.Append($@"
            <tr>
                <td>{noCount++}</td>
                <td>{utilityTypeDisplay}</td>
                <td></td>
                <td></td>
                <td class='highlight'>{utilityAmount:N0}</td>
            </tr>");
                }
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
                    <td>{item.Name}</td>
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
                    <td>{item.Name}</td>
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
            </tr>
        </thead>
        <tbody>");

            int noUti = 0;
            foreach (var item in request.UtilityInfos)
            {
                sb.Append($@"
            <tr>
                <td>{++noUti}</td>
                <td>{item.Description}</td>
                <td>{item.Coefficient:N0}</td>
                <td>{item.UnitPrice:N0}</td>
                <td>{item.Price:N0}</td>
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
                        <td>{roundedTotal * promotion.Value / 100:N0}</td>
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
        <li><strong>Ghi chú về VAT:</strong> Đơn giá báo trên chưa bao gồm thuế VAT.</li>
        <li><strong>Hạng mục không bao gồm:</strong> Bể bơi, tiểu cảnh sân vườn...</li>
        <li><strong>Chi phí thêm cho chiều cao móng nền:</strong> Phát sinh khi cao hơn 500mm.</li>
    </ul>

</body>
</html>");

            return sb.ToString();
        }

        public async Task<List<FinalAppResponse>> GetListFinalQuotationByProjectId(Guid projectId)
        {
            var paginatedList = await _unitOfWork.GetRepository<FinalQuotation>()
                .GetList(
                    predicate: x => x.ProjectId == projectId,
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
    }
}
