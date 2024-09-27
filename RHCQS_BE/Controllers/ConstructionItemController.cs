using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
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
        [HttpGet(ApiEndPointConstant.Construction.ConstructionEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListConstruction(int page, int size)
        {
            var listConstructions = await _constructionService.GetListConstruction(page, size);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return Ok(result);
        }

        #region GetDetailConstructionItem
        /// <summary>
        /// Retrieves the construction item.
        /// </summary>
        /// <returns>Item construction in the system</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Construction.ConstructionDetailEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailConstructionItem(Guid id)
        {
            var listConstructions = await _constructionService.GetDetailConstructionItem(id);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return Ok(result);
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
