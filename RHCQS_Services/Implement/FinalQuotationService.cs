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

        public Task<string> ApproveFinalFromManager(Guid initialId, ApproveQuotationRequest request)
        {
            throw new NotImplementedException();
        }

        /*        public async Task<string> ApproveFinalFromManager(Guid finalId, ApproveQuotationRequest request)
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
                }*/


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
            try
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
            }catch(Exception ex) { throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message); }
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

        public async Task<bool> UpdateFinalQuotation(UpdateFinalRequest request)
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
                    Version = request.VersionPresent,
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
        .signature-block {
            display: flex;
            justify-content: space-between;
            margin: 50px 0;
        }
        .signature {
            width: 50%;
            text-align: center;
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
    <h1><strong>BẢNG BÁO GIÁ THI CÔNG PHẦN THÔ & NHÂN CÔNG HOÀN THIỆN</strong></h1>
    <p><strong>CÔNG TRÌNH:</strong> NHÀ Ở RIÊNG LẺ</p>
    <p><strong>ĐỊA ĐIỂM:</strong> " + request.AccountName + @"</p>
    <p><strong>CHỦ ĐẦU TƯ:</strong> " + request.AccountName + @"</p>

    <h2>ĐIỀU 1. QUY MÔ CÔNG TRÌNH</h2>
    <p>Bổ sung</p>

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
            <td>" + request.PackageQuotationList.UnitPackageRough + @"</td>
            <td>=</td>
            <td>" + request.TotalRough + @"</td>
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

            foreach (var utility in request.UtilityInfos)
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
                <td>{request.TotalRough}</td>
                <td>{request.Unit}</td>
            </tr>
        </table>

    <h3>2.6. Tổng giá trị hợp đồng:</h3>
    <p>Giá trị hợp đồng trước thuế: " + request.TotalRough + @"</p>
    <p>Giá trị hợp đồng sau khuyến mãi: " + request.PromotionInfo.Value + @"</p>
    
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
            <td>{payment.Price}</td>
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

    <div class='signature-block'>
        <div class='signature left'>
            <p><strong>Chủ đầu tư</strong></p>
        </div>

    <div class='signature right'>
        <p><strong>Nhà thầu</strong></p>
    </div>
</div>
</body>
</html>");

            return sb.ToString();
        }
    }
}
