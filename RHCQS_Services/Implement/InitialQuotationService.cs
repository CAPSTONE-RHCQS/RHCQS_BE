﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DinkToPdf;
using DinkToPdf.Contracts;
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
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.UtilitiesSectionId,
                            item.Name ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            var batchPaymentResponse = initialQuotation!.BatchPayments.Select(item => new BatchPaymentInfo(
                                         item.Id,
                                         item.Payment.Description,
                                         item.Payment.Percents,
                                         item.Payment.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                     )).ToList() ?? new List<BatchPaymentInfo>();


            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.Customer!.Username!,
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
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses,
                UtilityInfos = utiResponse,
                PromotionInfo = promotionResponse,
                BatchPaymentInfos = batchPaymentResponse
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
                                       .Include(x => x.BatchPayments)
                                        .ThenInclude(x => x.Payment!)
                );

            if (initialQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Not_Found_InitialQuotaion);
            }

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
                            item.Price,
                            item.UnitPrice,
                                item.ConstructionItem?.SubConstructionItems
                                .FirstOrDefault(s => s.Id == item.SubConstructionId)?.Coefficient,
                            item.ConstructionItem!.Coefficient
                            )).ToList();

            var utiResponse = initialQuotation.QuotationUtilities.Select(item => new UtilityInfo(
                            item.UtilitiesSectionId,
                            item.Name ?? string.Empty,
                            item.Coefficient ?? 0,
                            item.Price ?? 0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            var batchPaymentResponse = initialQuotation!.BatchPayments.Select(item => new BatchPaymentInfo(
                                         item.Id,
                                         item.Payment.Description,
                                         item.Payment.Percents,
                                         item.Payment.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                     )).ToList() ?? new List<BatchPaymentInfo>();


            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.Customer!.Username!,
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
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
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
                            item.Price ?? 0
                )).ToList() ?? new List<UtilityInfo>();

            var promotionResponse = initialQuotation?.Promotion != null
                                              ? new PromotionInfo(
                                                  initialQuotation.Promotion.Id,
                                                  initialQuotation.Promotion.Name,
                                                  initialQuotation.Promotion.Value
                                               )
                                              : new PromotionInfo();

            var batchPaymentResponse = initialQuotation!.BatchPayments.Select(item => new BatchPaymentInfo(
                                         item.Id,
                                         item.Payment.Description,
                                         item.Payment.Percents,
                                         item.Payment.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                     )).ToList() ?? new List<BatchPaymentInfo>();


            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project!.Customer!.Username!,
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
                Discount = initialQuotation.Discount ?? 0.0,
                Unit = initialQuotation.Unit,
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
                    predicate: x => x.ProjectId == projectId &&
                               (x.Status == AppConstant.QuotationStatus.APPROVED ||
                                x.Status == AppConstant.QuotationStatus.FINALIZED),
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
                        _unitOfWork.Commit();

                        return uploadResult.SecureUrl.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                       $"{ex}" +
                       $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalLibraries", "libwkhtmltox.dll")}"
);
                }
                var isSuccessful = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Send_Fail;
                return isSuccessful;
            }
            else if(request.Type?.ToLower() == AppConstant.QuotationStatus.REJECTED.ToLower())
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
        .left {
            text-align: left; 
        }

        .right {
            text-align: right; 
        }
    </style>
</head>
<body>
    <h1><strong>BẢNG BÁO GIÁ SƠ BỘ NHÀ Ở DÂN DỤNG</strong></h1>
    <p><strong>CÔNG TRÌNH:</strong> NHÀ Ở RIÊNG LẺ</p>
    <p><strong>ĐỊA ĐIỂM:</strong> " + request.AccountName + @"</p>
    <p><strong>CHỦ ĐẦU TƯ:</strong> " + request.AccountName + @"</p>

    <h2>ĐIỀU 1. QUY MÔ CÔNG TRÌNH</h2>
    <p>Nhà ở dân dụng</p>

    <h2>ĐIỀU 2. GIÁ TRỊ HỢP ĐỒNG</h2>
    <h3>2.1. Đơn giá thi công phần thô trước thuế: " + request.PackageQuotationList.PackageRough + @" đồng/m²</h3>

    <h3>2.2. Diện tích xây dựng theo phương án thiết kế:</h3>
    <table>
        <tr>
            <th>STT</th>
            <th>Hạng mục</th>
            <th>Diện tích (m²)</th>
            <th>Hệ số</th>
            <th>Diện tích tổng (m²)</th>
        </tr>");

            int noCount = 1;
            foreach (var item in request.ItemInitial)
            {
                sb.Append($@"
        <tr>
            <td>{noCount}</td>
            <td>{item.Name}</td>
            <td>{request.Area}</td>
            <td>{item.Coefficient}</td>
            <td>{item.Area}</td>
        </tr>");
                noCount++;
            }

            sb.Append($@"
        <tr>
            <td colspan='4' style='text-align:right;'><strong>Tổng diện tích xây dựng theo thiết kế:</strong></td>
            <td>{request.Area}</td>
        </tr>
    </table>

    <h3>2.3. Giá trị thi công phần thô trước thuế:</h3>
    <table>
        <tr>
            <th>Tổng diện tích xây dựng</th>
            <th>x</th>
            <th>Đơn giá</th>
            <th>=</th>
            <th>Thành tiền</th>
        </tr>
        <tr>
            <td>" + request.Area + @" m²</td>
            <td>x</td>
            <td>" + request.PackageQuotationList.UnitPackageRough.ToString("N0") + @"</td>
            <td>=</td>
            <td>" + (request.TotalRough?.ToString("N0") ?? "0") + @"</td>
        </tr>
    </table>

    <h3>2.4. Các chi phí khác:</h3>
        <table>
            <tr>
                <th>Hạng mục</th>
                <th>Hệ số</th>
                <th>Đơn giá (VND)</th>
                <th>=</th>
                <th>Thành tiền (VND)</th>
            </tr>");

            foreach (var utility in request.UtilityInfos!)
            {
                sb.Append($@"
            <tr>
                <td>{utility.Description}</td>
                <td>{utility.Coefficient}</td>
                <td>{utility.Price:N0}</td>
                <td>=</td>
                <td>{(utility.Coefficient * utility.Price):N0}</td>
            </tr>");
            }

            sb.Append("</table>");

            sb.Append($@"
    <h3>2.5. Tổng hợp giá trị hợp đồng:</h3>
        <table>
            <tr>
                <th>Hạng mục</th>
                <th>x</th>
                <th>Thành tiền</th>
                <th>Đơn giá</th>
            </tr>
            <tr>
                <td>Phần thô</td>
                <td>x</td>
                <td>{request.TotalRough:N0}</td>
                <td>{request.Unit}</td>
            </tr>
        </table>

    <h3>2.6. Tổng giá trị hợp đồng:</h3>
    <p>Giá trị hợp đồng trước thuế: " + (request.TotalRough?.ToString("N0") ?? "0") + "VNĐ" + @"</p>
    <p>Giá trị hợp đồng sau khuyến mãi: " + (request.PromotionInfo?.Value?.ToString("N0") ?? "0") + "VNĐ" + @" </p>
    
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
    <p>Thời gian thi công phần thô: <strong>" + request.TimeOthers + @"</strong> ngày</p>");

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

                    double discountCheck = (double)request.Area * (double)promotionInfo.Value;
                    if(discountCheck != request.Promotions.Discount)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.InvalidDiscount);
                    }
                }
                #endregion

                #region Update project
                var initialVersionPresent = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                predicate: x => x.Version == request.VersionPresent && x.ProjectId == request.ProjectId,
                    include: x => x.Include(x => x.Project));

                initialVersionPresent.Project.Area = (request.Area.HasValue && request.Area.Value != 0.0) ?
                                    request.Area.Value : initialVersionPresent.Project.Area;
                initialVersionPresent.Project.Address = string.IsNullOrEmpty(request.Address) ?
                                      initialVersionPresent.Project.Address : request.Address;
                //Note: Version initial quotation - PROCESSING
                initialVersionPresent.Status = AppConstant.QuotationStatus.PROCESSING;

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
                    Unit = AppConstant.Unit.UnitPrice,
                    ReasonReject = null,
                    IsDraft = true,
                    Discount = request.Promotions?.Discount ?? 0.0
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
                                UtilitiesSectionId = sectionItem.Id
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
                                UtilitiesSectionId = utilityItem.SectionId
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
                foreach (var item in request.BatchPayments)
                {
                    var payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentTypeId = paymentType.Id,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        TotalPrice = item.Price,
                        //PaymentDate = LocalDateTime.VNDateTime(),
                        //PaymentPhase = LocalDateTime.VNDateTime(),
                        Percents = item.Percents,
                        Description = item.Description,
                        Unit = AppConstant.Unit.UnitPrice
                    };
                    await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);

                    var payItem = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        ContractId = null,
                        IntitialQuotationId = initialItem.Id,
                        InsDate = LocalDateTime.VNDateTime(),
                        FinalQuotationId = null,
                        PaymentId = payment.Id,
                        Status = AppConstant.PaymentStatus.PROGRESS
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
                initialItem.Status = AppConstant.QuotationStatus.FINALIZED;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);

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
                finalInfo.Status = AppConstant.QuotationStatus.FINALIZED;
                _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(finalInfo);

                var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
                return isSuccessful;
            }
            else
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invail_Quotation);
            }
        }


        public async Task<string> FeedbackFixInitialFromCustomer(Guid initialId, FeedbackQuotationRequest comment)
        {
            var initialItem = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Id == initialId);

            if (initialItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invail_Quotation);
            }

            initialItem.Note = comment.Note;
            initialItem.Status = AppConstant.QuotationStatus.PROCESSING;
            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }
    }
}
