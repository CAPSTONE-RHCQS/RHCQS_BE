using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                ProjectId = initialQuotation.Project.Id,
                PromotionId = initialQuotation.PromotionId,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
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

        public async Task<InitialQuotationResponse> GetDetailInitialNewVersion(Guid projectId)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.ProjectId == projectId && x.Version == 0.0,
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
                ProjectId = initialQuotation.Project.Id,
                PromotionId = initialQuotation.PromotionId,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
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
                );
            if (initialQuotation == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                      AppConstant.ErrMessage.Not_Found_InitialQuotaion);
            }
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

            var batchPaymentResponse = initialQuotation!.BatchPayments.Select(item => new BatchPaymentInfo(
                                       item.Id,
                                         item.Payment?.Description,
                                         item.Payment!.Percents,
                                         item.Payment!.TotalPrice,
                                         item.Payment.Unit,
                                         item.Status,
                                         item.Payment.PaymentDate,
                                         item.Payment.PaymentPhase
                                   )).ToList() ?? new List<BatchPaymentInfo>();


            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project.Customer!.Username!,
                ProjectId = initialQuotation.Project.Id,
                PromotionId = initialQuotation.PromotionId,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag!,
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

        public async Task<List<InitialQuotationAppResponse>> GetListInitialQuotationByProjectId(Guid projectId)
        {
            var paginatedList = await _unitOfWork.GetRepository<InitialQuotation>()
                .GetList(
                    predicate: x => x.ProjectId == projectId,
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
                            InsDate = DateTime.Now,
                            UpsDate = DateTime.Now,
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
                    Console.WriteLine($"Error converting HTML to PDF: {ex.Message}");
                    var message = $"{ex.Message}\nStackTrace: {ex.StackTrace}\nInnerException: {ex.InnerException?.Message}";
                    throw new Exception(message);
                }
            }
            else
            {
                if (initialItem.ReasonReject == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request,
                                               AppConstant.ErrMessage.Reason_Rejected_Required);
                }

                initialItem.Status = AppConstant.QuotationStatus.REJECTED;
                initialItem.ReasonReject = request.Reason;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialItem);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return null!;
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
                //Check version present dulicapte
                double nextVersion = 1.0;
                var highestInitial = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                    predicate: x => x.ProjectId == request.ProjectId,
                                    orderBy: x => x.OrderByDescending(x => x.Version)
                                );

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
                    InsDate = DateTime.Now,
                    Status = AppConstant.QuotationStatus.REVIEWING,
                    Version = nextVersion,
                    IsTemplate = false,
                    Deflag = true,
                    Note = null,
                    TotalRough = request.TotalRough,
                    TotalUtilities = request.TotalUtilities,
                    Unit = AppConstant.Unit.UnitPrice,
                    ReasonReject = null
                };
                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);

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
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        InitialQuotationId = initialItem.Id,
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(itemInitial);
                }

                foreach (var package in request.Packages!)
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
                    //Check UtilityItem or UtilitySection
                    var utilitiesItem = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(x => x.Id == utl.UtilitiesItemId);

                    //If utilitiesItem  == null => UtilitiesSectionId = utl.UtilitiesItemId
                    //Else utilitiesItem  != null => UtilitiesSectionId = utilitiesItem
                    var sectionId = utilitiesItem?.SectionId ?? utl.UtilitiesItemId;
                    var itemId = utilitiesItem?.Id ?? null;

                    var utlItem = new QuotationUtility
                    {
                        Id = Guid.NewGuid(),
                        UtilitiesItemId = itemId,
                        UtilitiesSectionId = sectionId,
                        FinalQuotationId = null,
                        InitialQuotationId = initialItem.Id,
                        Name = utl.Description!,
                        Coefiicient = utl.Coefficient,
                        Price = utl.Price,
                        Description = utl.Description,
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                    };
                    await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
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
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        TotalPrice = item.Price,
                        //PaymentDate = DateTime.Now,
                        //PaymentPhase = DateTime.Now,
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
                        InsDate = DateTime.Now,
                        FinalQuotationId = null,
                        PaymentId = payment.Id,
                        Status = AppConstant.PaymentStatus.PROGRESS
                    };
                    await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(payItem);
                }

                var initialVersionPresent = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                            predicate: x => x.Version == request.VersionPresent
                    );

                initialVersionPresent.Status = AppConstant.QuotationStatus.REJECTED;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialVersionPresent);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (Exception ex)
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

                //Contract design is coming ....
                var contractDrawing = new Contract
                {
                    Id = Guid.NewGuid(),
                    ProjectId = initialItem.ProjectId,
                    Name = EnumExtensions.GetEnumDescription(AppConstant.ContractType.Design),
                    CustomerName = initialItem.Project.Customer!.Username,
                    ContractCode = null,
                    StartDate = null,
                    EndDate = null,
                    ValidityPeriod = null,
                    TaxCode = null,
                    Area = initialItem.Area,
                    UnitPrice = AppConstant.Unit.UnitPrice,
                    ContractValue = null,
                    UrlFile = null,
                    Note = null,
                    Deflag = true,
                    RoughPackagePrice = 0,
                    FinishedPackagePrice = 0,
                    Status = AppConstant.ConstractStatus.PROCESSING,
                    Type = AppConstant.ContractType.Design.ToString(),
                };
                await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);

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

                //Contract construction is coming ....
                var contractDrawing = new Contract
                {
                    Id = Guid.NewGuid(),
                    ProjectId = finalInfo.ProjectId,
                    Name = EnumExtensions.GetEnumDescription(AppConstant.ContractType.Construction),
                    CustomerName = finalInfo.Project.Customer!.Username,
                    ContractCode = null,
                    StartDate = null,
                    EndDate = null,
                    ValidityPeriod = null,
                    TaxCode = null,
                    Area = finalInfo.Project.Area,
                    UnitPrice = AppConstant.Unit.UnitPrice,
                    ContractValue = null,
                    UrlFile = null,
                    Note = null,
                    Deflag = true,
                    RoughPackagePrice = 0,
                    FinishedPackagePrice = 0,
                    Status = AppConstant.ConstractStatus.PROCESSING,
                    Type = AppConstant.ContractType.Construction.ToString(),
                };
                await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);

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
