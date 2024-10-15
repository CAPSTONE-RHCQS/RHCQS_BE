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
                UpsDate = DateTime.UtcNow,
                Status = request.Status,
                Deflag = true,
                BatchPayments = request.BatchPaymentInfos.Select(bp => new BatchPayment
                {
                    Id = Guid.NewGuid(),
                    Price = bp.Price,
                    IntitialQuotationId = bp.InitIntitialQuotationId,
                    Percents = bp.Percents,
                    Description = bp.Description,
                    InsDate = DateTime.Now,
                    Unit = AppConstant.Unit.UnitPrice
                }).ToList(),

                EquipmentItems = request.EquipmentItems.Select(ei => new EquipmentItem
                {
                    Id = Guid.NewGuid(),
                    Name = ei.Name,
                    Unit = ei.Unit,
                    Quantity = ei.Quantity,
                    UnitOfMaterial = ei.UnitOfMaterial,
                    TotalOfMaterial = ei.TotalOfMaterial,
                    Note = ei.Note
                }).ToList(),

                FinalQuotationItems = request.FinalQuotationItems.Select(fqi => new FinalQuotationItem
                {
                    Id = Guid.NewGuid(),
                    ConstructionItemId = fqi.ConstructionItemId,
                    Name = fqi.Name,
                    Unit = fqi.Unit,
                    Weight = fqi.Weight,
                    UnitPriceLabor = fqi.UnitPriceLabor,
                    UnitPriceRough = fqi.UnitPriceRough,
                    UnitPriceFinished = fqi.UnitPriceFinished,
                    TotalPriceLabor = fqi.TotalPriceLabor,
                    TotalPriceRough = fqi.TotalPriceRough,
                    TotalPriceFinished = fqi.TotalPriceFinished
                }).ToList()
            };

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

                    var pdf = _converter.Convert(doc);
                    //Upload cloudinary
                    using (var pdfStream = new MemoryStream(pdf))
                    {
                        // Tạo tham số để upload lên Cloudinary
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription($"{data.AccountName}_Quotation.pdf", pdfStream),
                            Folder = "FinalQuotation",
                            PublicId = $"Bao_gia_chi_tiet_{data.AccountName}_{data.Version}",
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
                    x => x.Project.Customer.Username.Equals(name),
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer)
                                   .Include(x => x.Promotion)
                                   .Include(x => x.QuotationUtilities)
                                       .ThenInclude(qu => qu.UtilitiesItem)
                                   .Include(x => x.EquipmentItems)
                                   .Include(x => x.FinalQuotationItems)
                                       .ThenInclude(co => co.ConstructionItem)
                                   .Include(x => x.BatchPayments)
                );

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                       AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                var batchPaymentsList = finalQuotation.BatchPayments.Select(bp => new BatchPaymentResponse(
                    bp.Id,
                    bp.Description,
                    bp.Percents,
                    bp.Price,
                    bp.Unit,
                    bp.PaymentDate,
                    bp.PaymentPhase
                )).ToList();

                var equipmentItemsList = finalQuotation.EquipmentItems.Select(ei => new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note
                )).ToList();

                var finalQuotationItemsList = finalQuotation.FinalQuotationItems.Select(fqi => new FinalQuotationItemResponse(
                    fqi.Id,
                    fqi.Name,
                    fqi.ConstructionItem?.Type,
                    fqi.Unit,
                    fqi.Weight,
                    fqi.UnitPriceLabor,
                    fqi.UnitPriceRough,
                    fqi.UnitPriceFinished,
                    fqi.TotalPriceLabor,
                    fqi.TotalPriceRough,
                    fqi.TotalPriceFinished,
                    fqi.InsDate
                )).ToList();

                var promotionInfo = finalQuotation.Promotion != null
                    ? new PromotionInfo(
                        finalQuotation.Promotion.Id,
                        finalQuotation.Promotion.Name,
                        finalQuotation.Promotion.Value
                    )
                    : null;

                var utilityInfoList = finalQuotation.QuotationUtilities != null
                    ? new List<UtilityInf>
                    {
                new UtilityInf(
                    finalQuotation.QuotationUtilities.Id,
                    finalQuotation.QuotationUtilities.Description,
                    finalQuotation.QuotationUtilities.Coefiicient ?? 0,
                    finalQuotation.QuotationUtilities.Price ?? 0,
                    finalQuotation.QuotationUtilities.UtilitiesItem.Section.UnitPrice ?? 0,
                    finalQuotation.QuotationUtilities.UtilitiesItem.Section.Unit
                )
                    }
                    : new List<UtilityInf>();
                var constructionRoughList = finalQuotationItemsList
                    .Where(item => item.Type == "ROUGH")
                    .GroupBy(item => item.Type)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(item => item.TotalPriceRough ?? 0),
                        group.Sum(item => item.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();


                var constructionFinishedList = finalQuotationItemsList
                    .Where(item => item.Type == "FINISHED")
                    .GroupBy(item => item.Type)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(item => item.TotalPriceRough ?? 0),
                        group.Sum(item => item.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();
                var response = new FinalQuotationResponse(
                    finalQuotation.Id,
                    finalQuotation.Project.Customer.Username,
                    finalQuotation.ProjectId,
                    finalQuotation.Project.Type,
                    finalQuotation.Project.Address,
                    finalQuotation.PromotionId,
                    finalQuotation.TotalPrice,
                    finalQuotation.Note,
                    finalQuotation.Version,
                    finalQuotation.InsDate,
                    finalQuotation.UpsDate,
                    finalQuotation.Status,
                    finalQuotation.Deflag,
                    finalQuotation.QuotationUtilitiesId,
                    finalQuotation.ReasonReject,
                    batchPaymentsList,
                    equipmentItemsList,
                    finalQuotationItemsList,
                    promotionInfo,
                    utilityInfoList,
                    constructionRoughList,
                    constructionFinishedList
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
                    x => x.Id == id,
                    include: x => x.Include(x => x.Project)
                                   .ThenInclude(x => x.Customer)
                                   .Include(x => x.Promotion)
                                   .Include(x => x.QuotationUtilities)
                                       .ThenInclude(qu => qu.UtilitiesItem)
                                       .ThenInclude(qu => qu.Section)
                                   .Include(x => x.EquipmentItems)
                                   .Include(x => x.FinalQuotationItems)
                                       .ThenInclude(co => co.ConstructionItem)
                                   .Include(x => x.BatchPayments)
                );

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                       AppConstant.ErrMessage.Not_Found_FinalQuotaion);
                }

                var batchPaymentsList = finalQuotation.BatchPayments.Select(bp => new BatchPaymentResponse(
                    bp.Id,
                    bp.Description,
                    bp.Percents,
                    bp.Price,
                    bp.Unit,
                    bp.PaymentDate,
                    bp.PaymentPhase
                )).ToList();

                var equipmentItemsList = finalQuotation.EquipmentItems.Select(ei => new EquipmentItemsResponse(
                    ei.Id,
                    ei.Name,
                    ei.Unit,
                    ei.Quantity,
                    ei.UnitOfMaterial,
                    ei.TotalOfMaterial,
                    ei.Note
                )).ToList();

                var finalQuotationItemsList = finalQuotation.FinalQuotationItems.Select(fqi => new FinalQuotationItemResponse(
                    fqi.Id,
                    fqi.Name,
                    fqi.ConstructionItem?.Type,
                    fqi.Unit,
                    fqi.Weight,
                    fqi.UnitPriceLabor,
                    fqi.UnitPriceRough,
                    fqi.UnitPriceFinished,
                    fqi.TotalPriceLabor,
                    fqi.TotalPriceRough,
                    fqi.TotalPriceFinished,
                    fqi.InsDate
                )).ToList();

                var promotionInfo = finalQuotation.Promotion != null
                    ? new PromotionInfo(
                        finalQuotation.Promotion.Id,
                        finalQuotation.Promotion.Name,
                        finalQuotation.Promotion.Value
                    )
                    : null;

                var utilityInfoList = finalQuotation.QuotationUtilities != null
                    ? new List<UtilityInf>
                    {
                new UtilityInf(
                    finalQuotation.QuotationUtilities.Id,
                    finalQuotation.QuotationUtilities.Description,
                    finalQuotation.QuotationUtilities.Coefiicient ?? 0,
                    finalQuotation.QuotationUtilities.Price ?? 0,
                    finalQuotation.QuotationUtilities.UtilitiesItem.Section.UnitPrice ?? 0,
                    finalQuotation.QuotationUtilities.UtilitiesItem.Section.Unit
                )
                    }
                    : new List<UtilityInf>();
                var constructionRoughList = finalQuotationItemsList
                    .Where(item => item.Type == "ROUGH")
                    .GroupBy(item => item.Type)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(item => item.TotalPriceRough ?? 0),
                        group.Sum(item => item.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();


                var constructionFinishedList = finalQuotationItemsList
                    .Where(item => item.Type == "FINISHED")
                    .GroupBy(item => item.Type)
                    .Select(group => new ConstructionSummary(
                        group.Key,
                        group.Sum(item => item.TotalPriceRough ?? 0),
                        group.Sum(item => item.TotalPriceLabor ?? 0)
                    )).FirstOrDefault();
                var response = new FinalQuotationResponse(
                    finalQuotation.Id,
                    finalQuotation.Project.Customer.Username,
                    finalQuotation.ProjectId,
                    finalQuotation.Project.Type,
                    finalQuotation.Project.Address,
                    finalQuotation.PromotionId,
                    finalQuotation.TotalPrice,
                    finalQuotation.Note,
                    finalQuotation.Version,
                    finalQuotation.InsDate,
                    finalQuotation.UpsDate,
                    finalQuotation.Status,
                    finalQuotation.Deflag,
                    finalQuotation.QuotationUtilitiesId,
                    finalQuotation.ReasonReject,
                    batchPaymentsList,
                    equipmentItemsList,
                    finalQuotationItemsList,
                    promotionInfo,
                    utilityInfoList,
                    constructionRoughList,
                    constructionFinishedList
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
                var finalQuotation = new FinalQuotation()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = request.ProjectId,
                    PromotionId = request.PromotionId,
                    TotalPrice = request.TotalPrice,
                    Note = request.Note,
                    Version = request.VersionPresent + 1,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    Status = request.Status,
                    Deflag = true
                };

                await _unitOfWork.GetRepository<FinalQuotation>().InsertAsync(finalQuotation);

                foreach (var payment in request.BatchPaymentInfos)
                {
                    var batchPayment = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        Price = payment.Price,
                        Percents = payment.Percents,
                        Description = payment.Description,
                        InsDate = DateTime.Now,
                        IntitialQuotationId = payment.InitIntitialQuotationId,
                        FinalQuotationId = finalQuotation.Id,
                        Unit = AppConstant.Unit.UnitPrice
                    };
                    await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(batchPayment);
                }

                foreach (var equipment in request.EquipmentItems)
                {
                    var equipmentItem = new EquipmentItem
                    {
                        Id = Guid.NewGuid(),
                        Name = equipment.Name,
                        Unit = equipment.Unit,
                        Quantity = equipment.Quantity,
                        UnitOfMaterial = equipment.UnitOfMaterial,
                        TotalOfMaterial = equipment.TotalOfMaterial,
                        Note = equipment.Note,
                        FinalQuotationId = finalQuotation.Id
                    };
                    await _unitOfWork.GetRepository<EquipmentItem>().InsertAsync(equipmentItem);
                }

                foreach (var finalItem in request.FinalQuotationItems)
                {
                    var finalQuotationItem = new FinalQuotationItem
                    {
                        Id = Guid.NewGuid(),
                        ConstructionItemId = finalItem.ConstructionItemId,
                        Name = finalItem.Name,
                        Unit = finalItem.Unit,
                        Weight = finalItem.Weight,
                        UnitPriceLabor = finalItem.UnitPriceLabor,
                        UnitPriceRough = finalItem.UnitPriceRough,
                        UnitPriceFinished = finalItem.UnitPriceFinished,
                        TotalPriceLabor = finalItem.TotalPriceLabor,
                        TotalPriceRough = finalItem.TotalPriceRough,
                        TotalPriceFinished = finalItem.TotalPriceFinished,
                        InsDate = DateTime.Now,
                        FinalQuotationId = finalQuotation.Id
                    };
                    await _unitOfWork.GetRepository<FinalQuotationItem>().InsertAsync(finalQuotationItem);
                }

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
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

            decimal total = roughTotalAmount + finishedTotalAmount + utilityTotal;
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
            sb.Append($@"
    <h2>BẢNG BÁO CHI TIẾT</h2>");

            var roughItems = request.FinalQuotationItems.Where(x => x.Type == "ROUGH").ToList();
            if (roughItems.Any())
            {
                sb.Append($@"
    <h4>CHI PHÍ THÔ</h4>
    <table border='1' cellpadding='5' cellspacing='0'>
        <thead>
            <tr>
                <th>STT</th>
                <th>NỘI DUNG CÔNG VIỆC</th>
                <th>DVT</th>
                <th>KHỐI LƯỢNG</th>
                <th>ĐƠN GIÁ NHÂN CÔNG</th>
                <th>ĐƠN GIÁ VẬT TƯ</th>
                <th>THÀNH TIỀN NHÂN CÔNG</th>
                <th>THÀNH TIỀN VẬT TƯ</th>
            </tr>
        </thead>
        <tbody>");

                int noCons = 0;
                foreach (var item in roughItems)
                {
                    sb.Append($@"
        <tr>
            <td>{++noCons}</td>
            <td>{item.Name}</td>
            <td>{item.Unit}</td>
            <td>{item.Weight:N0}</td>
            <td>{item.UnitPriceLabor:N0}</td>
            <td>{item.UnitPriceRough:N0}</td>
            <td>{item.TotalPriceLabor:N0}</td>
            <td>{item.TotalPriceRough:N0}</td>
        </tr>");
                }

                sb.Append("</tbody></table>");
            }

            var finishedItems = request.FinalQuotationItems.Where(x => x.Type == "FINISHED").ToList();
            if (finishedItems.Any())
            {
                sb.Append($@"
    <h4>CHI PHÍ HOÀN THIỆN</h4>
    <table border='1' cellpadding='5' cellspacing='0'>
        <thead>
            <tr>
                <th>STT</th>
                <th>NỘI DUNG CÔNG VIỆC</th>
                <th>DVT</th>
                <th>KHỐI LƯỢNG</th>
                <th>ĐƠN GIÁ NHÂN CÔNG</th>
                <th>ĐƠN GIÁ VẬT TƯ</th>
                <th>THÀNH TIỀN NHÂN CÔNG</th>
                <th>THÀNH TIỀN VẬT TƯ</th>
            </tr>
        </thead>
        <tbody>");

                int noCons = roughItems.Count;
                foreach (var item in finishedItems)
                {
                    sb.Append($@"
        <tr>
            <td>{++noCons}</td>
            <td>{item.Name}</td>
            <td>{item.Unit}</td>
            <td>{item.Weight:N0}</td>
            <td>{item.UnitPriceLabor:N0}</td>
            <td>{item.UnitPriceFinished:N0}</td>
            <td>{item.TotalPriceLabor:N0}</td>
            <td>{item.TotalPriceFinished:N0}</td>
        </tr>");
                }

                sb.Append("</tbody></table>");
            }

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
    }
}
