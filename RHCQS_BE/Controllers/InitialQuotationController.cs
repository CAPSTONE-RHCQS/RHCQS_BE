using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialQuotationController : ControllerBase
    {
        private readonly IInitialQuotationService _initialService;

        public InitialQuotationController(IInitialQuotationService initialService)
        {
            _initialService = initialService;
        }

        #region GetListInitialQuotation
        /// <summary>
        /// Retrieves a paginated list of initial quotations.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/v1/initial-quotations?page={page}&size={size}
        /// 
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve a paginated list of initial quotations. 
        /// Users can specify the page number and the page size for the response. Each entry in the list includes basic information about 
        /// the initial quotation, such as the ID, version, area, and status.
        /// 
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
        /// <param name="page">The page number to retrieve. Default is 1.</param>
        /// <param name="size">The number of items per page. Default is 10.</param>
        /// <returns>Returns a paginated list of initial quotations, or a 404 Not Found response if no quotations are found.</returns>
        /// <response code="200">Initial quotations retrieved successfully</response>
        /// <response code="404">No initial quotations found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.InitialQuotation.InitialQuotationEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListInitialQuotation(int page, int size)
        {
            var list = await _initialService.GetListInitialQuotation(page, size);
            if (list == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_InitialQuotaion });
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return Ok(result);
        }

        #region GetDetailInitialQuotation
        /// <summary>
        /// Retrieves the details of a specific initial quotation by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/v1/initial-quotation/{id}
        ///
        /// Request Example:
        /// ```json
        /// {
        ///   "Id": "bd31c4e5-e549-42ea-aec7-0f08446f089d",
        ///   "AccountName": "Trần Ngân",
        ///   "ProjectId": "b81935a8-4482-43f5-ad68-558abde58d58",
        ///   "PromotionId": "41b828b2-7b06-4389-9003-daac937158dd",
        ///   "PackageId": "00000000-0000-0000-0000-000000000000",
        ///   "Area": 125.0,
        ///   "InsDate": "2024-10-04T12:40:31.853",
        ///   "Status": "Pending",
        ///   "Version": 1.0,
        ///   "TotalRough": 1500000000.0,           //Tổng tiền phần hoàn thiện
        ///   "TotalUtilities": 20000000.0,         //Tổng tiền phần tiện ích - giấy phép nằm luôn trong tiện ích
        ///   "Unit": "VNĐ",
        ///   "PackageQuotationList": {
        ///   "IdPackageRough": "59f5fd78-b895-4d60-934a-4727c219b2d9",
        ///   "PackageRough": "Gói tiêu chuẩn",
        ///   "UnitPackageRough": 3550000.0,                                //Giá thi công phần thô
        ///   "IdPackageFinished": "0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5",
        ///   "PackageFinished": "Gói tiêu chuẩn",
        ///   "UnitPackageFinished": 3450000.0,                             //Giá thi công phần hoàn thiện
        ///   "Unit": "m2"
        ///   },
        ///   "ItemInitial": [
        ///     {
        ///       "Id": "5b792ed1-a726-44ce-9820-9629d459ed8b",
        ///       "Name": "Mái che",
        ///       "SubConstruction": "Mái BTCT",
        ///       "Area": 49.5,
        ///       "Price": 66330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.5,            //Hê số của mục con - Mái BTCT
        ///       "Coefficient": 0.0                //Hệ số mục cha - Không có hệ số
        ///     },
        ///     {
        ///       "Id": "1bf45d93-ddb9-40ff-b1a4-b01180bc5955",
        ///       "Name": "Trệt",
        ///       "SubConstruction": null,
        ///       "Area": 99.0,
        ///       "Price": 331650000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": null,           //Hệ số mục con - Trệt không có
        ///       "Coefficient": 1.0                //Hệ số mục cha 
        ///     },
        ///     {
        ///       "Id": "3c414d7a-58d0-488c-9e48-ccef30093ea1",
        ///       "Name": "Móng",
        ///       "SubConstruction": "Móng đơn",
        ///       "Area": 49.5,
        ///       "Price": 66330000.0,
        ///       "UnitPrice": "đ",
        ///       "SubCoefficient": 0.2,            //Hệ số mục con - Móng đơn có hệ số 0.2
        ///       "Coefficient": 0.0                //Hạng mục cha có nhiều hạng mục con => Hạng mục cha không có hệ số như Móng, Mái che, Hầm,...
        ///     }
        ///   ],
        ///   "PromotionInfo": {
        ///     "Id": "41b828b2-7b06-4389-9003-daac937158dd",
        ///     "Name": "Giảm 10% cho khách hàng may mắn",
        ///     "Value": 10                                     //Phần trăm khuyến mãi
        ///   },
        ///   "BatchPaymentInfos": [
        ///     {
        ///       "Id": "d165e833-2e68-45ad-a657-a222d01e205c",
        ///       "Description": "Đợt 1 thanh toán 50%",
        ///       "Percents": "50",                             //Phần trăm thanh toán
        ///       "Price": 15000000.0,
        ///       "Unit": "VNĐ"
        ///     },
        ///     {
        ///       "Id": "9f29dc1f-c94d-4078-94ad-b3ebf48a6f8a",
        ///       "Description": "Đợt 2 thanh toán 50% nghiệm thu bản vẽ thiết kế",
        ///       "Percents": "50",                             //Phần trăm thanh toán
        ///       "Price": 15000000.0,
        ///       "Unit": "VNĐ"
        ///     }
        ///   ]
        /// }
        /// ```
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve the details of an initial quotation, 
        /// including information about the construction items and related data. The ID of the quotation is passed as a parameter in the URL.
        /// </remarks>
        /// <param name="id">The unique identifier of the initial quotation.</param>
        /// <returns>Returns the details of the initial quotation, or a 404 Not Found response if the ID does not exist.</returns>
        /// <response code="200">Initial quotation details retrieved successfully</response>
        /// <response code="404">Initial quotation not found</response>
        #endregion
        //[Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.InitialQuotation.InitialQuotationDetailEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailInitialQuotation(Guid id)
        {
            var quotation = await _initialService.GetDetailInitialQuotationById(id);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_InitialQuotaion });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return Ok(result);
        }

        #region
        /// <summary>
        /// Assign a quotation to a customer based on the request payload.
        /// </summary>
        /// <param name="request">The request payload containing accountId and initialQuotationId.</param>
        /// <returns>Assigned quotation details or an error message.</returns>
        /// <response code="200">Returns the details of the assigned quotation.</response>
        /// <response code="404">If no quotation is found or staff overload error occurs.</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/initial-quotation
        ///     {
        ///         "accountId": "d287e991-5b2b-4569-b0c4-7e81d9e75b78",
        ///         "initialQuotationId": "3f63e5b2-632f-48fa-ae9d-1c123456abcd"
        ///     }
        ///     
        /// </remarks>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.InitialQuotation.AssignInitialQuotationEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignQuotation([FromBody] AssignQuotaionInitial request)
        {
            var quotation = await _initialService.AssignQuotation(request.accountId, request.initialQuotationId);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.OverloadStaff });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return Ok(result);
        }

    }
}
