using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructionItemController : ControllerBase
    {
        private readonly IConstructionItemService _constructionService;

        public ConstructionItemController(IConstructionItemService constructionService)
        {
            _constructionService = constructionService;
        }

        #region GetListConstruction
        /// <summary>
        /// Retrieves the list of all construction item.
        /// </summary>
        /// <returns>List of construction in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Construction.ConstructionEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListConstruction(int page, int size)
        {
            var listConstructions = await _constructionService.GetListConstruction(page, size);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListConstructionRough
        /// <summary>
        /// Retrieves the list of construction items based on the specified type.
        /// </summary>
        /// <remarks>
        /// This API returns a list of construction items based on the provided `type`. The possible values for `type` are:
        /// 
        /// - "ROUGH": Retrieves all rough construction items.
        /// - "FINISHED": Retrieves all finished construction items.
        /// - "ALL": Retrieves all construction items.
        /// 
        /// Example request:
        /// 
        ///     GET /api/v1/construction/rough?type=ROUGH 
        ///     GET /api/v1/construction/rough?type=FINISHED
        ///     GET /api/v1/construction/rough?type=ALL
        ///     
        /// </remarks>
        /// <param name="type">
        /// The type of construction items to retrieve. Must be one of the following values:
        /// - ROUGH: Retrieves rough construction items.
        /// - FINISHED: Retrieves finished construction items.
        /// - ALL: Retrieves all construction items.
        /// </param>
        /// <returns>
        /// A list of construction items of the specified type.
        /// </returns>
        /// <response code="200">Returns the list of construction items successfully.</response>
        /// <response code="400">If the `type` is invalid or not provided.</response>
        /// <response code="500">If there is an internal server error while processing the request.</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Construction.ConstructionRoughEndpoint)]
        [ProducesResponseType(typeof(List<ConstructionItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListConstructionRough(string type)
        {
            if (string.IsNullOrWhiteSpace(type) ||
                (type != AppConstant.Type.ROUGH && type != AppConstant.Type.FINISHED && type != AppConstant.Type.ALL))
            {
                return BadRequest("Invalid type. Allowed values are ROUGH, FINISHED, or ALL.");
            }

            var listConstructions = await _constructionService.GetListConstructionRough(type.ToUpper());
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }


        #region GetDetailConstructionItem
        /// <summary>
        /// Retrieves the construction item.
        /// </summary>
        /// <returns>Item construction in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Construction.ConstructionDetailEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailConstructionItem(Guid id)
        {
            var construction = await _constructionService.GetDetailConstructionItem(id);
            var result = JsonConvert.SerializeObject(construction, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailConstructionItemByName
        /// <summary>
        /// Retrieves the construction item by name.
        /// </summary>
        /// <returns>Item construction in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Construction.COnstructionDetailByNameEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailConstructionItemByName(string name)
        {
            var construction = await _constructionService.GetDetailConstructionItemByName(name);
            var result = JsonConvert.SerializeObject(construction, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateConstructionItem
        /// <summary>
        /// Creates a new construction item and its sub-items in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/construction
        ///     {
        ///       "name": "Mái phụ",
        ///       "coefficient": 0,
        ///       "unit": "m2",
        ///       "type": "ROUGH",
        ///       "subConstructionRequests": [
        ///         {
        ///           "name": "Mái BTCT",
        ///           "coefficient": 0.5,
        ///           "unit": "m2"
        ///         },
        ///         {
        ///           "name": "Mái Tole",
        ///           "coefficient": 0.3,
        ///           "unit": "m2"
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="item">Construction item request model</param>
        /// <returns>Returns true if the construction item is created successfully, otherwise false.</returns>
        /// <response code="200">Construction item created successfully</response>
        /// <response code="400">Failed to create the construction item</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Construction.ConstructionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateConstructionItem([FromBody] ConstructionItemRequest item)
        {
            var isCreate = await _constructionService.CreateConstructionItem(item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
