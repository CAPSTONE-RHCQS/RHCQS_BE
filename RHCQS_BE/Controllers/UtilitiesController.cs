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
            return Ok(result);
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
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
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
            return Ok(result);
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
            return Ok(result);
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
            return Ok(result);
        }
    }
}
