using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalQuotationController : ControllerBase
    {
        private readonly IFinalQuotationService _finalQuotationService;

        public FinalQuotationController(IFinalQuotationService finalQuotationService)
        {
            _finalQuotationService = finalQuotationService;
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
        #region
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
                return Ok(new { Url = pdfUrl });
            }

            return BadRequest(AppConstant.Message.ERROR);
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

        #region GetDetailFinalQuotationById
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

        #region GetDetailFinalQuotationByCustomerName
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
        }

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
        ///   "projectId": "7f1e0a03-91a5-46ab-aae8-65cd4092eb8b",
        ///   "promotionId": null,
        ///   "totalPrice": 0,
        ///   "note": null,
        ///   "batchPaymentInfos": [],
        ///   "equipmentItems": [],
        ///   "utilities": [
        ///     {
        ///       "utilitiesItemId": "b069d992-d30d-42a8-91d5-000e749c67eb",
        ///       "coefficient": 0.03,
        ///       "price": 23611500,
        ///       "description": ""
        ///     },
        ///     {
        ///       "utilitiesItemId": "a6dbeb8b-2853-4df4-8069-598171488373",
        ///       "coefficient": 0.04,
        ///       "price": 31482000,
        ///       "description": ""
        ///     },
        ///     {
        ///       "utilitiesItemId": "58559293-f989-4d06-af88-e318a370cd2e",
        ///       "coefficient": 0,
        ///       "price": 250000,
        ///       "description": ""
        ///     }
        ///   ],
        ///   "finalQuotationItems": [
        ///     {
        ///       "constructionItemId": "40AA0395-A4EF-423C-91F2-11D2889DC306",
        ///       "quotationItems": [
        ///         {
        ///           "unit": "cuộn",
        ///           "weight": 11,
        ///           "unitPriceLabor": 0,
        ///           "unitPriceRough": 13000.0,
        ///           "unitPriceFinished": 0,
        ///           "totalPriceLabor": 0,
        ///           "totalPriceRough": 143000.0,
        ///           "totalPriceFinished": 0,
        ///           "note": "test",
        ///           "quotationLabors": [],
        ///           "quotationMaterials": [
        ///             {
        ///               "materialId": "4232C02F-912C-41C9-AAEF-0101A39DE9FD",
        ///               "unit": "cuộn",
        ///               "materialPrice": 13000.0
        ///             }
        ///           ]
        ///         }
        ///       ]
        ///     },
        ///     {
        ///       "constructionItemId": "9696145F-BEBF-4120-8354-C8AEF681B351",
        ///       "quotationItems": [
        ///         {
        ///           "unit": "m2",
        ///           "weight": 11,
        ///           "unitPriceLabor": 350000.0,
        ///           "unitPriceRough": 0,
        ///           "unitPriceFinished": 0,
        ///           "totalPriceLabor": 3850000,
        ///           "totalPriceRough": 0,
        ///           "totalPriceFinished": 0,
        ///           "note": "test",
        ///           "quotationLabors": [
        ///             {
        ///               "laborId": "0D207781-EEBB-4A03-96CE-005434963F44",
        ///               "laborPrice": 350000.0
        ///             }
        ///           ],
        ///           "quotationMaterials": []
        ///         }
        ///       ]
        ///     }
        ///   ]
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
            bool quotation = await _finalQuotationService.UpdateFinalQuotation(request);

            return Ok(quotation ? AppConstant.Message.SUCCESSFUL_FINAL : AppConstant.Message.ERROR);
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
            bool quotation = await _finalQuotationService.CreateFinalQuotation(projectid);

            return Ok(quotation ? AppConstant.Message.SUCCESSFUL_CREATEFINAL : AppConstant.Message.ERROR);
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
            if (list == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_FinalQuotaion});
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

    }
}
