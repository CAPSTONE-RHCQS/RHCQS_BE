using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignPriceController : ControllerBase
    {
        private readonly IDesignPriceService _designPriceService;

        public DesignPriceController(IDesignPriceService designPriceService)
        {
            _designPriceService = designPriceService;
        }

        #region GetListDesignPrice
        /// <summary>
        /// Retrieves the list of all design prices.
        /// </summary>
        /// <returns>List of design price in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager")]
        [HttpGet(ApiEndPointConstant.DesignPrice.DesignPriceListEndpoint)]
        [ProducesResponseType(typeof(DesignPriceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListDesignPrice()
        {
            var listDesignPrices = await _designPriceService.GetListDesignPrice();
            var result = JsonConvert.SerializeObject(listDesignPrices, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailDesignPrice
        /// <summary>
        /// Retrieves the design price.
        /// </summary>
        /// <returns>Design price in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager")]
        [HttpGet(ApiEndPointConstant.DesignPrice.DesignPriceDetailEndpoint)]
        [ProducesResponseType(typeof(DesignPriceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailDesignPrice(Guid id)
        {
            var designPrice = await _designPriceService.GetDetailDesignPrice(id);
            var result = JsonConvert.SerializeObject(designPrice, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateDesignPrice
        /// <summary>
        /// Creates a new design price in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/designprice
        ///    {
        ///    "areaform": 40,
        ///    "areaform": 100,
        ///    "price": 150000
        ///     }
        /// </remarks>
        /// <param name="request">Design price request model</param>
        /// <returns>Returns true if the design price is created successfully, otherwise false.</returns>
        /// <response code="200">Design price created successfully</response>
        /// <response code="400">Failed to create the design price</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.DesignPrice.DesignPriceEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDesignPrice([FromBody] DesignPriceRequest request)
        {
            var isCreated = await _designPriceService.CreateDesignPrice(request);
            return isCreated ? Ok(isCreated) : BadRequest();
        }

        #region UpdateDesignPrice
        /// <summary>
        /// Updates an existing design price.
        /// Requires Manager role for authorization.
        /// </summary>
        /// <param name="id">The unique identifier of design price to be updated.</param>
        /// <param name="request">The request body containing the updated design price details.</param>
        /// <returns>A boolean value indicating the success or failure of the update operation.</returns>
        /// <response code="200">Returns true if the update is successful.</response>
        /// <response code="400">Returns BadRequest if the update fails or if validation issues occur.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.DesignPrice.DesignPriceEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDesignPrice(Guid id, [FromBody] DesignPriceRequest request)
        {
            var isUpdated = await _designPriceService.UpdateDesignPrice(id, request);
            return isUpdated ? Ok(isUpdated) : BadRequest();
        }
    }
}
