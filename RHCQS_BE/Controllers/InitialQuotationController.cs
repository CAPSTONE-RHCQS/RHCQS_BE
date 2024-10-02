using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
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
        //[Authorize(Roles = "Customer, SalesStaff, Manager")]
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
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' to retrieve the details of an initial quotation, 
        /// including information about the construction items and related data. The ID of the quotation is passed as a parameter in the URL.
        /// </remarks>
        /// <param name="id">The unique identifier of the initial quotation.</param>
        /// <returns>Returns the details of the initial quotation, or a 404 Not Found response if the ID does not exist.</returns>
        /// <response code="200">Initial quotation details retrieved successfully</response>
        /// <response code="404">Initial quotation not found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.InitialQuotation.InitialQuotationDetailEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailInitialQuotation(Guid id)
        {
            var quotation = await _initialService.GetDetailInitialQuotationById(id);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.Not_Found_InitialQuotaion });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return Ok(result);
        }
    }
}
