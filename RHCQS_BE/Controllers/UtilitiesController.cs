using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.Utility;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilitiesService _utilitiesService;

        public UtilitiesController(IUtilitiesService utilitiesService)
        {
            _utilitiesService = utilitiesService;
        }

        #region GetListUtilities
        /// <summary>
        /// Retrieves the list of all utilities item.
        /// </summary>
        /// <returns>List of utilities in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Utility.UtilityEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListUtilities(int page, int size)
        {
            var listUtilities = await _utilitiesService.GetListUtilities(page, size);
            var result = JsonConvert.SerializeObject(listUtilities, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListUtilitiesByType
        /// <summary>
        /// Retrieves the list of utilities based on the specified type.
        /// </summary>
        /// <remarks>
        /// This API returns a list of utilities items based on the provided `type`. The possible values for `type` are:
        /// 
        /// - "ROUGH": Retrieves all rough utility items.
        /// - "FINISHED": Retrieves all finished utility items.
        /// - "ALL": Retrieves all utility items.
        /// 
        /// Example request:
        /// 
        ///     GET /api/v1/utility/type?type=ROUGH 
        ///     GET /api/v1/utility/type?type=FINISHED
        ///     GET /api/v1/utility/type?type=ALL
        ///     
        /// </remarks>
        /// <param name="type">
        /// The type of utility items to retrieve. Must be one of the following values:
        /// - ROUGH: Retrieves rough utility items.
        /// - FINISHED: Retrieves finished utility items.
        /// - ALL: Retrieves all utility items.
        /// </param>
        /// <returns>
        /// A list of utility items of the specified type.
        /// </returns>
        /// <response code="200">Returns the list of utility items successfully.</response>
        /// <response code="400">If the `type` is invalid or not provided.</response>
        /// <response code="500">If there is an internal server error while processing the request.</response>
        #endregion
        //[Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Utility.UtilityByTypeEndpoint)]
        [ProducesResponseType(typeof(List<UtilityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListUtilitiesByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type) ||
                (type != AppConstant.Type.ROUGH && type != AppConstant.Type.FINISHED && type != AppConstant.Type.ALL))
            {
                return BadRequest("Invalid type. Allowed values are ROUGH, FINISHED, or ALL.");
            }

            var listUtilities = await _utilitiesService.GetListUtilitiesByType(type.ToUpper());
            var result = JsonConvert.SerializeObject(listUtilities, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailUtilityItem
        /// <summary>
        /// Retrieves the utility item.
        /// </summary>
        /// <returns>Item utility in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Utility.UtilityDetaidEndpoint)]
        [ProducesResponseType(typeof(UtilityResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailUtilityItem(Guid id)
        {
            var utilityItem = await _utilitiesService.GetDetailUtilityItem(id);
            var result = JsonConvert.SerializeObject(utilityItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailUtilitySection
        /// <summary>
        /// Retrieves the utility section item.
        /// </summary>
        /// <returns>Item utility section in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Utility.UtilitySectionDEndpoint)]
        [ProducesResponseType(typeof(UtilityResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailUtilitySection(Guid id)
        {
            var utilityItem = await _utilitiesService.GetDetailUtilitySection(id);
            var result = JsonConvert.SerializeObject(utilityItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region
        /// <summary>
        /// Creates or updates a utility along with its sections and items.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     //002E459A-E010-493F-8585-D729D3CF357B  - 1 - Điều kiện thi công không thuận lợi
        ///     //2367DEC1-E649-4549-B81B-701F2DBC1A7B  - 2 - Nâng cao chất lượng phần thô
        ///     //04F30A66-9758-45DB-88A7-6F098EDC4837  - 3 - Dịch vụ tiện ích thêm
        ///     //05430765-97FE-4186-900D-D5DC850E8CDB  - 4 - Tiện ích công trình
        ///     POST /api/utility/create
        ///     {
        ///       "id": "2367DEC1-E649-4549-B81B-701F2DBC1A7B",     If Id null -> Create new utility 
        ///       "name": "string",
        ///       "type": "string",
        ///       "sections": [
        ///         {
        ///           "id": null,
        ///           "name": "Chuyển đổi phần vách hầm nổi trên mặt đất từ xây gạch đinh sang vách đổ bê tông, cốt thép",
        ///           "description": "Chuyển đổi phần vách hầm nổi trên mặt đất từ xây gạch đinh sang vách đổ bê tông, cốt thép",
        ///           "unitPrice": 1000000,
        ///           "unit": "đ"
        ///         }
        ///       ],
        ///       "items": null
        ///     }
        /// </remarks>
        /// <param name="request">The request body containing the utility details.</param>
        /// <returns>A JSON response with the created or updated utility details.</returns>
        /// <response code="200">Returns the newly created or updated utility</response>
        /// <response code="400">If the request is invalid</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Utility.UtilityEndpoint)]
        [ProducesResponseType(typeof(UtilityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUtility([FromBody] UtilityRequest request)
        {
            var utilityItem = await _utilitiesService.CreateUtility(request);
            var result = JsonConvert.SerializeObject(utilityItem, Formatting.Indented);
            return Ok(result);
        }
    }
}
