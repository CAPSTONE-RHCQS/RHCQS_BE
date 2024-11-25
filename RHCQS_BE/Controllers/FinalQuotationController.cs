using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalQuotationController : ControllerBase
    {
        private readonly IFinalQuotationService _finalQuotationService;
        private readonly IFirebaseService _firebaseService;
        private readonly IAccountService _accountService;
        public FinalQuotationController(IFinalQuotationService finalQuotationService, IFirebaseService firebaseService, IAccountService accountService)
        {
            _finalQuotationService = finalQuotationService;
            _firebaseService = firebaseService;
            _accountService = accountService;
        }

        #region GetListFinalQuotation
        /// <summary>
        /// Retrieves a paginated list of final quotations.
        /// </summary>
        /// <remarks>
        /// Example response:
        /// {
        ///   "Size": 10,
        ///   "Page": 1,
        ///   "Total": 50,
        ///   "TotalPages": 5,
        ///   "Items": [
        ///     {
        ///       "Id": "f6f03971-5a01-47ce-869a-c3f63d70fbb9",
        ///       "Version": "v1.0",
        ///       "Area": 125.0,
        ///       "Status": "Proccessing"
        ///     }
        ///   ]
        /// }
        /// </remarks>
        /// <returns>Returns a paginated list of final quotations, or a 404 Not Found response if no quotations are found.</returns>
        /// <response code="200">Final quotations retrieved successfully</response>
        /// <response code="404">No final quotations found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.FinalQuotation.FinalQuotationEndpoint)]
        [ProducesResponseType(typeof(FinalQuotationListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListInitialQuotation(int page, int size)
        {
            var list = await _finalQuotationService.GetListFinalQuotation(page, size);
            if (list == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_FinalQuotaion });
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
        #region Approvedandrejectfinal
        /// <summary>
        /// Approve or reject a final quotation by a Manager.
        /// </summary>
        /// <remarks>
        /// This API allows a Manager to approve or reject a final quotation. 
        /// 
        /// ### Request Examples:
        /// 
        /// **PUT** `/api/quotation/final/approve?finalId=5e6321c8-fc09-4b45-8a64-d72f91c19b7f`
        /// 
        /// **Body**:
        /// ```json
        /// {
        ///   "reason": "The quotation does not meet the necessary requirements."
        /// }
        /// ```
        /// 
        /// ### Responses:
        /// 
        /// **200 OK** - Quotation Approved or Rejected
        /// 
        /// Example success response (Approved):
        /// ```json
        /// {
        ///   "Url": "http://example.com/quotation.pdf"
        /// }
        /// ```
        /// 
        /// Example success response (Rejected):
        /// ```json
        /// {
        ///   "message": "An error occurred during approval."
        /// }
        /// ```
        /// 
        /// **404 Not Found** - The final quotation ID was not found.
        /// 
        /// **400 Bad Request** - An error occurred during approval or rejection.
        /// </remarks>
        /// <param name="finalId">The unique ID of the final quotation to be approved or rejected.</param>
        /// <param name="request">The request body containing the reason for approval or rejection.</param>
        /// <returns>Returns the PDF URL if successful, or an error message.</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.FinalQuotation.ApproveFinalQuotationEndpoint)]
        [ProducesResponseType(typeof(ApproveQuotationRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveFinalFromManager([FromQuery] Guid finalId, [FromBody] ApproveQuotationRequest request)
        {
            var pdfUrl = await _finalQuotationService.ApproveFinalFromManager(finalId, request);

            if (!string.IsNullOrEmpty(pdfUrl))
            {
                var result = JsonConvert.SerializeObject(pdfUrl, Formatting.Indented);

                var customerEmail = await _accountService.GetEmailByQuotationIdAsync(finalId);
                var deviceToken = await _firebaseService.GetDeviceTokenAsync(customerEmail);
                var notificationRequest = new NotificationRequest
                {
                    Email = customerEmail,
                    DeviceToken = deviceToken,
                    Title = "Báo giá chi tiết",
                    Body = $"Báo giá chi tiết có cập nhật mới bạn cần xem."
                };
                await _firebaseService.SendNotificationAsync(
                    notificationRequest.Email,
                    notificationRequest.DeviceToken,
                    notificationRequest.Title,
                    notificationRequest.Body
                );
                return new ContentResult()
                {
                    Content = result,
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json"
                };
            }
            if (pdfUrl == AppConstant.Message.REJECTED)
            {
                var result = JsonConvert.SerializeObject(pdfUrl, Formatting.Indented);
                return new ContentResult()
                {
                    Content = result,
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json"
                };
            }
            else
            {
                var result = JsonConvert.SerializeObject(AppConstant.Message.ERROR, Formatting.Indented);
                return new ContentResult()
                {
                    Content = result,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "application/json"
                };

            }
        }
        #region GetDetailFinalQuotationById
            /// <summary>
            /// Retrieves the details of a specific final quotation by id.
            /// </summary>
            /// <remarks>
            /// Sample request:
            /// 
            ///     GET /api/v1/quotation/final/id
            ///
            /// Request Example:
            /// ```json
            /// {
            ///   "Id": "6d31c4e5-e549-42ea-aec7-0f08446f089d",
            ///   "AccountName": "Nguyễn Văn A",
            ///   "ProjectId": "b12345a8-4482-43f5-ad68-558abde58d58",
            ///   "PromotionId": "50b828b2-7b06-4389-9003-daac937158dd",
            ///   "PackageId": "11111111-1111-1111-1111-111111111111",
            ///   "Area": 150.0,
            ///   "InsDate": "2024-10-04T12:40:31.853",
            ///   "Status": "Completed",
            ///   "Version": 1.0,
            ///   "TotalRough": 2000000000.0,           // Tổng tiền phần hoàn thiện
            ///   "TotalUtilities": 25000000.0,         // Tổng tiền phần tiện ích - giấy phép nằm luôn trong tiện ích
            ///   "Unit": "VNĐ",
            ///   "PackageQuotationList": {
            ///   "IdPackageRough": "59f5fd78-b895-4d60-934a-4727c219b2d9",
            ///   "PackageRough": "Gói tiêu chuẩn",
            ///   "UnitPackageRough": 4500000.0,                                // Giá thi công phần thô
            ///   "IdPackageFinished": "0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5",
            ///   "PackageFinished": "Gói cao cấp",
            ///   "UnitPackageFinished": 4500000.0,                             // Giá thi công phần hoàn thiện
            ///   "Unit": "m2"
            ///   },
            ///   "ItemFinal": [
            ///     {
            ///       "Id": "5b792ed1-a726-44ce-9820-9629d459ed8b",
            ///       "Name": "Mái che",
            ///       "SubConstruction": "Mái BTCT",
            ///       "Area": 49.5,
            ///       "Price": 76330000.0,
            ///       "UnitPrice": "đ",
            ///       "SubCoefficient": 0.5,            // Hệ số của mục con - Mái BTCT
            ///       "Coefficient": 0.0                // Hệ số mục cha - Không có hệ số
            ///     },
            ///     {
            ///       "Id": "1bf45d93-ddb9-40ff-b1a4-b01180bc5955",
            ///       "Name": "Trệt",
            ///       "SubConstruction": null,
            ///       "Area": 99.0,
            ///       "Price": 431650000.0,
            ///       "UnitPrice": "đ",
            ///       "SubCoefficient": null,           // Hệ số mục con - Trệt không có
            ///       "Coefficient": 1.0                // Hệ số mục cha 
            ///     },
            ///     {
            ///       "Id": "3c414d7a-58d0-488c-9e48-ccef30093ea1",
            ///       "Name": "Móng",
            ///       "SubConstruction": "Móng đơn",
            ///       "Area": 49.5,
            ///       "Price": 76330000.0,
            ///       "UnitPrice": "đ",
            ///       "SubCoefficient": 0.2,            // Hệ số mục con - Móng đơn có hệ số 0.2
            ///       "Coefficient": 0.0                // Hạng mục cha có nhiều hạng mục con => Hạng mục cha không có hệ số như Móng, Mái che, Hầm,...
            ///     }
            ///   ],
            ///   "PromotionInfo": {
            ///     "Id": "50b828b2-7b06-4389-9003-daac937158dd",
            ///     "Name": "Giảm 15% cho khách hàng VIP",
            ///     "Value": 15                                     // Phần trăm khuyến mãi
            ///   },
            ///   "BatchPaymentInfos": [
            ///     {
            ///       "Id": "e165e833-2e68-45ad-a657-a222d01e205c",
            ///       "Description": "Đợt 1 thanh toán 50% trước khi thi công",
            ///       "Percents": "50",                             // Phần trăm thanh toán
            ///       "Price": 20000000.0,
            ///       "Unit": "VNĐ"
            ///     },
            ///     {
            ///       "Id": "8f29dc1f-c94d-4078-94ad-b3ebf48a6f8a",
            ///       "Description": "Đợt 2 thanh toán 50% nghiệm thu hoàn công",
            ///       "Percents": "50",                             // Phần trăm thanh toán
            ///       "Price": 20000000.0,
            ///       "Unit": "VNĐ"
            ///     }
            ///   ]
            /// }
            /// ```
            /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve the details of a final quotation, 
            /// including information about the construction items and related data. The ID of the quotation is passed as a parameter in the URL.
            /// </remarks>
            /// <param name="id">The unique identifier of the final quotation.</param>
            /// <returns>Returns the details of the final quotation, or a 404 Not Found response if the ID does not exist.</returns>
            /// <response code="200">Final quotation details retrieved successfully</response>
            /// <response code="404">Final quotation not found</response>
            #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.FinalQuotation.FinalQuotationDetailEndpoint)]
        [ProducesResponseType(typeof(FinalQuotationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailFinalQuotation(Guid id)
        {
            var quotation = await _finalQuotationService.GetDetailFinalQuotationById(id);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_FinalQuotaion });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailFinalQuotationByProjectId
        /// <summary>
        /// Retrieves the details of a specific final quotation by projectid.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/v1/quotation/final/projectid
        ///
        /// Request Example:
        /// ```json
        /// {
        ///   "Id": "6d31c4e5-e549-42ea-aec7-0f08446f089d",
        ///   "AccountName": "Nguyễn Văn A",
        ///   "ProjectId": "b12345a8-4482-43f5-ad68-558abde58d58",
        ///   "PromotionId": "50b828b2-7b06-4389-9003-daac937158dd",
        ///   "PackageId": "11111111-1111-1111-1111-111111111111",
        ///   "Area": 150.0,
        ///   "InsDate": "2024-10-04T12:40:31.853",
        ///   "Status": "Completed",
        ///   "Version": 1.0,
        ///   "TotalRough": 2000000000.0,           // Tổng tiền phần hoàn thiện
        ///   "TotalUtilities": 25000000.0,         // Tổng tiền phần tiện ích - giấy phép nằm luôn trong tiện ích
        ///   "Unit": "VNĐ",
        ///   "PackageQuotationList": {
        ///   "IdPackageRough": "59f5fd78-b895-4d60-934a-4727c219b2d9",
        ///   "PackageRough": "Gói tiêu chuẩn",
        ///   "UnitPackageRough": 4500000.0,                                // Giá thi công phần thô
        ///   "IdPackageFinished": "0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5",
        ///   "PackageFinished": "Gói cao cấp",
        ///   "UnitPackageFinished": 4500000.0,                             // Giá thi công phần hoàn thiện
        ///   "Unit": "m2"
        ///   },
        ///   "ItemFinal": [
        ///     {
        ///       "Id": "5b792ed1-a726-44ce-9820-9629d459ed8b",
        ///       "Name": "Mái che",
        ///       "SubConstruction": "Mái BTCT",
        ///       "Area": 49.5,
        ///       "Price": 76330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.5,            // Hệ số của mục con - Mái BTCT
        ///       "Coefficient": 0.0                // Hệ số mục cha - Không có hệ số
        ///     },
        ///     {
        ///       "Id": "1bf45d93-ddb9-40ff-b1a4-b01180bc5955",
        ///       "Name": "Trệt",
        ///       "SubConstruction": null,
        ///       "Area": 99.0,
        ///       "Price": 431650000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": null,           // Hệ số mục con - Trệt không có
        ///       "Coefficient": 1.0                // Hệ số mục cha 
        ///     },
        ///     {
        ///       "Id": "3c414d7a-58d0-488c-9e48-ccef30093ea1",
        ///       "Name": "Móng",
        ///       "SubConstruction": "Móng đơn",
        ///       "Area": 49.5,
        ///       "Price": 76330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.2,            // Hệ số mục con - Móng đơn có hệ số 0.2
        ///       "Coefficient": 0.0                // Hạng mục cha có nhiều hạng mục con => Hạng mục cha không có hệ số như Móng, Mái che, Hầm,...
        ///     }
        ///   ],
        ///   "PromotionInfo": {
        ///     "Id": "50b828b2-7b06-4389-9003-daac937158dd",
        ///     "Name": "Giảm 15% cho khách hàng VIP",
        ///     "Value": 15                                     // Phần trăm khuyến mãi
        ///   },
        ///   "BatchPaymentInfos": [
        ///     {
        ///       "Id": "e165e833-2e68-45ad-a657-a222d01e205c",
        ///       "Description": "Đợt 1 thanh toán 50% trước khi thi công",
        ///       "Percents": "50",                             // Phần trăm thanh toán
        ///       "Price": 20000000.0,
        ///       "Unit": "VNĐ"
        ///     },
        ///     {
        ///       "Id": "8f29dc1f-c94d-4078-94ad-b3ebf48a6f8a",
        ///       "Description": "Đợt 2 thanh toán 50% nghiệm thu hoàn công",
        ///       "Percents": "50",                             // Phần trăm thanh toán
        ///       "Price": 20000000.0,
        ///       "Unit": "VNĐ"
        ///     }
        ///   ]
        /// }
        /// ```
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve the details of a final quotation, 
        /// including information about the construction items and related data. The ID of the quotation is passed as a parameter in the URL.
        /// </remarks>
        /// <param name="projectId">The unique identifier of the final quotation.</param>
        /// <returns>Returns the details of the final quotation, or a 404 Not Found response if the ID does not exist.</returns>
        /// <response code="200">Final quotation details retrieved successfully</response>
        /// <response code="404">Final quotation not found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.FinalQuotation.FinalQuotationDetailByProjectIdEndpoint)]
        [ProducesResponseType(typeof(FinalQuotationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailFinalQuotationByProjectId(Guid projectId)
        {
            var quotation = await _finalQuotationService.GetDetailFinalQuotationByProjectId(projectId);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_FinalQuotaion });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

/*        #region GetDetailFinalQuotationByCustomerName
        /// <summary>
        /// Retrieves the details of a specific final quotation by customer name.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/v1/quotation/final/customer/name
        ///
        /// Request Example:
        /// ```json
        /// {
        ///   "Id": "6d31c4e5-e549-42ea-aec7-0f08446f089d",
        ///   "AccountName": "Nguyễn Văn A",
        ///   "ProjectId": "b12345a8-4482-43f5-ad68-558abde58d58",
        ///   "PromotionId": "50b828b2-7b06-4389-9003-daac937158dd",
        ///   "PackageId": "11111111-1111-1111-1111-111111111111",
        ///   "Area": 150.0,
        ///   "InsDate": "2024-10-04T12:40:31.853",
        ///   "Status": "Completed",
        ///   "Version": 1.0,
        ///   "TotalRough": 2000000000.0,           // Tổng tiền phần hoàn thiện
        ///   "TotalUtilities": 25000000.0,         // Tổng tiền phần tiện ích - giấy phép nằm luôn trong tiện ích
        ///   "Unit": "VNĐ",
        ///   "PackageQuotationList": {
        ///   "IdPackageRough": "59f5fd78-b895-4d60-934a-4727c219b2d9",
        ///   "PackageRough": "Gói tiêu chuẩn",
        ///   "UnitPackageRough": 4500000.0,                                // Giá thi công phần thô
        ///   "IdPackageFinished": "0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5",
        ///   "PackageFinished": "Gói cao cấp",
        ///   "UnitPackageFinished": 4500000.0,                             // Giá thi công phần hoàn thiện
        ///   "Unit": "m2"
        ///   },
        ///   "ItemFinal": [
        ///     {
        ///       "Id": "5b792ed1-a726-44ce-9820-9629d459ed8b",
        ///       "Name": "Mái che",
        ///       "SubConstruction": "Mái BTCT",
        ///       "Area": 49.5,
        ///       "Price": 76330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.5,            // Hệ số của mục con - Mái BTCT
        ///       "Coefficient": 0.0                // Hệ số mục cha - Không có hệ số
        ///     },
        ///     {
        ///       "Id": "1bf45d93-ddb9-40ff-b1a4-b01180bc5955",
        ///       "Name": "Trệt",
        ///       "SubConstruction": null,
        ///       "Area": 99.0,
        ///       "Price": 431650000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": null,           // Hệ số mục con - Trệt không có
        ///       "Coefficient": 1.0                // Hệ số mục cha 
        ///     },
        ///     {
        ///       "Id": "3c414d7a-58d0-488c-9e48-ccef30093ea1",
        ///       "Name": "Móng",
        ///       "SubConstruction": "Móng đơn",
        ///       "Area": 49.5,
        ///       "Price": 76330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.2,            // Hệ số mục con - Móng đơn có hệ số 0.2
        ///       "Coefficient": 0.0                // Hạng mục cha có nhiều hạng mục con => Hạng mục cha không có hệ số như Móng, Mái che, Hầm,...
        ///     }
        ///   ],
        ///   "PromotionInfo": {
        ///     "Id": "50b828b2-7b06-4389-9003-daac937158dd",
        ///     "Name": "Giảm 15% cho khách hàng VIP",
        ///     "Value": 15                                     // Phần trăm khuyến mãi
        ///   },
        ///   "BatchPaymentInfos": [
        ///     {
        ///       "Id": "e165e833-2e68-45ad-a657-a222d01e205c",
        ///       "Description": "Đợt 1 thanh toán 50% trước khi thi công",
        ///       "Percents": "50",                             // Phần trăm thanh toán
        ///       "Price": 20000000.0,
        ///       "Unit": "VNĐ"
        ///     },
        ///     {
        ///       "Id": "8f29dc1f-c94d-4078-94ad-b3ebf48a6f8a",
        ///       "Description": "Đợt 2 thanh toán 50% nghiệm thu hoàn công",
        ///       "Percents": "50",                             // Phần trăm thanh toán
        ///       "Price": 20000000.0,
        ///       "Unit": "VNĐ"
        ///     }
        ///   ]
        /// }
        /// ```
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve the details of a final quotation, 
        /// including information about the construction items and related data. The ID of the quotation is passed as a parameter in the URL.
        /// </remarks>
        /// <param name="customerName">The unique identifier of the final quotation.</param>
        /// <returns>Returns the details of the final quotation, or a 404 Not Found response if the ID does not exist.</returns>
        /// <response code="200">Final quotation details retrieved successfully</response>
        /// <response code="404">Final quotation not found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.FinalQuotation.FinalQuotationDetailByCustomerEndpoint)]
        [ProducesResponseType(typeof(FinalQuotationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailFinalQuotationByCustomerName(string customerName)
        {
            var quotation = await _finalQuotationService.GetDetailFinalQuotationByCustomerName(customerName);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_FinalQuotaion });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }*/

        #region UpdateFinalQuotation
        /// <summary>
        /// Updates a final quotation.
        /// </summary>
        /// <remarks>
        /// This API allows users to update the final quotation information.
        /// To perform this request, please provide the necessary information in the request body.
        ///
        /// **Sample Request:**
        /// ```json
        /// {
        ///     "projectId": "BDD78FB2-E758-4674-84E9-4E8B4406826E",
        ///     "promotionId": null,
        ///     "note": "gg",
        ///     "batchPaymentInfos": [
        ///         {
        ///             "initInitialQuotationId": "bd31c4e5-e549-42ea-aec7-0f08446f089d",
        ///             "paymentTypeId": "2d4a2343-d102-4dc9-8a4f-6647ea397e6c",
        ///             "contractId": "9d864292-9768-46d4-82f5-6b26cb1b9a3f",
        ///             "price": 292261500,
        ///             "percents": "50",
        ///             "description": "Đợt 1 thanh toán 50%",
        ///             "status": "Progress"
        ///         },
        ///         {
        ///             "initInitialQuotationId": "bd31c4e5-e549-42ea-aec7-0f08446f089d",
        ///             "paymentTypeId": "2d4a2343-d102-4dc9-8a4f-6647ea397e6c",
        ///             "contractId": "9d864292-9768-46d4-82f5-6b26cb1b9a3f",
        ///             "price": 685552500,
        ///             "percents": "50",
        ///             "description": "Đợt 2 thanh toán 50% nghiệm thu bản vẽ thiết kế",
        ///             "status": "Progress"
        ///         }
        ///     ],
        ///     "equipmentItems": [
        ///         {
        ///             "name": "Lavabo màu trắng âm bàn, MS: L5125\n\n",
        ///             "unit": "Bộ",
        ///             "quantity": 6,
        ///             "unitOfMaterial": 1111000,
        ///             "note": "gg",
        ///             "type": "SANITATION"
        ///         }
        ///     ],
        ///     "utilities": [
        ///         {
        ///             "utilitiesItemId": "422bb684-c541-47f5-ae3b-7f8f38e91e84",
        ///             "price": 777777
        ///         }
        ///     ],
        ///     "finalQuotationItems": [
        ///         {
        ///             "ConstructionId": "708BAD20-AF57-4AB6-96E9-C22A22273724",
        ///             "quotationItems": [
        ///                 {
        ///                     "laborId": null,
        ///                     "materialId": "8961731d-9389-4b9a-a86e-23ad1f8211c5",
        ///                     "unit": "bao",
        ///                     "weight": 170,
        ///                     "note": "gg"
        ///                 },
        ///                 {
        ///                     "laborId": "5ecffef5-6441-437c-903b-ed469e4a819a",
        ///                     "materialId": null,
        ///                     "unit": "m2",
        ///                     "weight": 170,
        ///                     "note": "gg"
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">The request containing the updated final quotation information.</param>
        /// <returns>A result indicating whether the update was successful.</returns>
        /// <response code="200">Returns success message if the update was successful.</response>
        /// <response code="404">Returns if the specified quotation was not found.</response>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPut(ApiEndPointConstant.FinalQuotation.FinalQuotationEndpoint)]
        [ProducesResponseType(typeof(FinalRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFinalQuotation([FromBody] FinalRequest request)
        {

            Guid? finalQuotationId = await _finalQuotationService.UpdateFinalQuotation(request);
            if (finalQuotationId == null) return NotFound(new { message = AppConstant.Message.ERROR });
            var response = new
            {
                Newid = finalQuotationId,
                message = AppConstant.Message.SUCCESSFUL_UPDATE
            };
            var result = JsonConvert.SerializeObject(response, Formatting.Indented);

            //var projectdetail = await _projectService.GetDetailProjectById(request.ProjectId);
            //var accountdetail = await _accountService.SearchAccountsByNameAsync(projectdetail.AccountName);
            //var sanitizedEmail = accountdetail.Email.Replace("@", "_at_").Replace(".", "_dot_");
            //var deviceToken = await _firebaseService.GetDeviceTokenAsync(sanitizedEmail);
            //var notificationRequest = new NotificationRequest
            //{
            //    Email = sanitizedEmail,
            //    DeviceToken = deviceToken,
            //    Title = "Quotation Updated",
            //    Body = $"Quotation has been successfully updated."
            //};
            //await _firebaseService.SendNotificationAsync(
            //    notificationRequest.Email,
            //    notificationRequest.DeviceToken,
            //    notificationRequest.Title,
            //    notificationRequest.Body
            //);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CancelFinalQuotation
        /// <summary>
        /// cancel final quotation.
        /// </summary>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.FinalQuotation.CancelFinalQuotationEndpoint)]
        [ProducesResponseType(typeof(FinalRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelFinalQuotationFromManager([FromQuery] Guid finalQuotationId,[FromBody] CancelQuotation reason)
        {
            bool quotation = await _finalQuotationService.CancelFinalQuotation(finalQuotationId, reason);
            return Ok(quotation ? AppConstant.Message.SUCCESSFUL_CANCELFINAL : AppConstant.Message.ERROR);
        }
        #region ConfirmArgeeFinalFromCustomer
        /// <summary>
        /// Confirms the agreement of an final quotation from a customer.
        /// 
        /// ROLE: CUSTOMER
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.FinalQuotation.FinalQuotationCustomerAgree)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmArgeeInitialFromCustomer(Guid quotationId)
        {
            var result = await _finalQuotationService.ConfirmArgeeFinalFromCustomer(quotationId);

            return Ok(result);
        }
        #region FeedbackFixFinalFromCustomer
        /// <summary>
        /// When customer need to fix quotation -> Customer comment and click "Gửi"
        /// 
        /// ROLE: CUSTOMER
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.FinalQuotation.FinalQuotationCustomerComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FeedbackFixInitialFromCustomer(Guid finalid, FeedbackQuotationRequest request)
        {
            var result = await _finalQuotationService.FeedbackFixFinalFromCustomer(finalid, request);

            return Ok(result);
        }
        #region CreateFinalQuotation
        /// <summary>
        /// Creates the first version of a Final Quotation based on a project
        /// that has an Initial Quotation with the status of FINALIZED.
        /// </summary>
        /// <param name="projectid">The unique identifier of the project.</param>
        /// <returns>A response indicating the success or failure of the creation process.</returns>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPost(ApiEndPointConstant.FinalQuotation.FinalQuotationDetailByProjectIdEndpoint)]
        [ProducesResponseType(typeof(FinalRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateFinalQuotation([FromQuery] Guid projectid)
        {
            var quotation = await _finalQuotationService.CreateFinalQuotation(projectid);
            if (quotation == null) return NotFound(new { message = AppConstant.Message.ERROR });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
/*            return Ok(quotation ? AppConstant.Message.SUCCESSFUL_CREATEFINAL : AppConstant.Message.ERROR);*/
        }

        #region GetListFinalQuotationByProjectId
        /// <summary>
        /// Show file PDF final quotation for App
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <remarks>
        /// projectId demo: B81935A8-4482-43F5-AD68-558ABDE58D58
        /// </remarks>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.FinalQuotation.FinalQuotationProjectEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListFinalQuotationByProjectId(Guid projectId)
        {
            var list = await _finalQuotationService.GetListFinalQuotationByProjectId(projectId);
            if (list == null) return NotFound(new { message = AppConstant.Message.ERROR });
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete(ApiEndPointConstant.FinalQuotation.FinalQuotationEndpoint)]
        public async Task<IActionResult> DeleteFinalQuotation(Guid id)
        {
                await _finalQuotationService.DeleteFinalQuotation(id);
                return NoContent(); // 204 No Content on successful deletion
        }
    }
}
