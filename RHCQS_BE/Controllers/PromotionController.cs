using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Request.Promotion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        #region GetListPromotion
        /// <summary>
        /// Retrieves the list of all promotion.
        /// Role required: MANAGER - CUSTOMER - SALE STAFF
        /// </summary>
        /// <returns>List of promotion in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Promotion.PromotionEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListPromotion(int page, int size)
        {
            var listPromotions = await _promotionService.GetListPromotion(page, size);
            var result = JsonConvert.SerializeObject(listPromotions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListPromotion
        /// <summary>
        /// (MOBILE) Retrieves the list of all promotion.
        /// Role required: CUSTOMER
        /// </summary>
        /// <returns>List of promotion in the system</returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.Promotion.PromotionValidEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListPromotionVaild()
        {
            var listPromotions = await _promotionService.GetListPromotionVaild();
            var result = JsonConvert.SerializeObject(listPromotions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }


        #region GetDetailPromotion
        /// <summary>
        /// Retrieves the details of a specific promotion.
        /// 
        /// Role required: MANAGER - CUSTOMER - SALE STAFF
        /// </summary>
        /// <remarks>
        /// This endpoint fetches detailed information about a promotion based on its unique ID.
        /// 
        /// <b>Roles allowed:</b> MANAGER, CUSTOMER, SALE STAFF
        /// </remarks>
        /// <param name="promotionId">The unique identifier of the promotion</param>
        /// <returns>Detailed information about the promotion</returns>
        /// <response code="200">Returns the details of the promotion</response>
        /// <response code="404">If the promotion is not found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Promotion.PromotionDetailEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailPromotion(Guid promotionId)
        {
            var promotionItem = await _promotionService.GetDetailPromotion(promotionId);
            var result = JsonConvert.SerializeObject(promotionItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region SearchPromotion
        /// <summary>
        /// Retrieves the details of a specific promotion.
        /// 
        /// Role required: MANAGER - CUSTOMER - SALE STAFF
        /// </summary>
        /// <remarks>
        /// This endpoint fetches detailed information about a promotion based on its unique ID.
        /// 
        /// <b>Roles allowed:</b> MANAGER, CUSTOMER, SALE STAFF
        /// </remarks>
        /// <param name="promotionId">The unique identifier of the promotion</param>
        /// <returns>Detailed information about the promotion</returns>
        /// <response code="200">Returns the details of the promotion</response>
        /// <response code="404">If the promotion is not found</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Promotion.PromotionNameEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPromotion(string name)
        {
            var promotionItem = await _promotionService.SearchPromotionByName(name);
            var result = JsonConvert.SerializeObject(promotionItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreatePromotion
        /// <summary>
        /// Creates a new promotion in the system.
        /// 
        /// Role required: MANAGER
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/promotion
        ///     {
        ///       "value": 20,
        ///       "startTime": "2024-10-18T00:00:00",
        ///       "expTime": "2024-10-20T23:59:59",
        ///       "name": "20% discount for all orders"
        ///     }
        /// 
        /// Notes:
        /// - `value` represents the discount value.
        /// - `startTime` is the start time of the promotion.
        /// - `expTime` is the expiration time of the promotion.
        /// - `name` is the name of the promotion.
        /// </remarks>
        /// <param name="item">The promotion request model</param>
        /// <returns>Returns true if the promotion is created successfully, otherwise false.</returns>
        /// <response code="200">Promotion created successfully</response>
        /// <response code="400">Failed to create the promotion</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Promotion.PromotionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionRequest item)
        {
            var isCreate = await _promotionService.CreatePromotion(item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region UpdatePromotion
        /// <summary>
        /// Updates an existing promotion in the system.
        /// 
        /// Role required: MANAGER
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/v1/promotion/{promotionId}
        ///     {
        ///       "value": 5,
        ///       "startTime": "2024-10-20T13:06:23.599",
        ///       "expTime": "2024-10-23T13:06:23.599",
        ///       "name": "Khuyến mãi 20/10 giảm 5% gạch ốp WC khi xây dựng trên 125m2"
        ///     }
        /// 
        /// Notes:
        /// - `value` represents the discount value and must be greater than 0.
        /// - `startTime` is the start time of the promotion and must be a future date.
        /// - `expTime` is the expiration time of the promotion and must be greater than `startTime`.
        /// - `name` is the name of the promotion and cannot be empty.
        /// </remarks>
        /// <param name="promotionId">The unique identifier of the promotion to update.</param>
        /// <param name="item">The promotion request model containing updated values.</param>
        /// <returns>Returns true if the promotion is updated successfully, otherwise false.</returns>
        /// <response code="200">Promotion updated successfully</response>
        /// <response code="400">Failed to update the promotion due to bad request</response>
        /// <response code="422">Failed to update the promotion due to validation errors</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Promotion.PromotionDetailEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePromotion([FromQuery] Guid promotionId, [FromBody] UpdatePromotionRequest item)
        {
            var isCreate = await _promotionService.UpdatePromotion(promotionId, item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region BanPromotion
        /// <summary>
        /// Bans an existing promotion in the system.
        /// 
        /// Role required: MANAGER
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/v1/promotion/ban/{promotionId}
        /// 
        /// Notes:
        /// - This operation will deactivate the specified promotion and prevent it from being applied.
        /// - Ensure that the promotion is not already banned before attempting this action.
        /// </remarks>
        /// <param name="promotionId">The unique identifier of the promotion to ban.</param>
        /// <returns>Returns true if the promotion is banned successfully, otherwise false.</returns>
        /// <response code="200">Promotion banned successfully</response>
        /// <response code="400">Failed to ban the promotion due to bad request</response>
        /// <response code="404">Promotion not found</response>
        /// <response code="403">User does not have permission to ban promotions</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Promotion.PromotionBanEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BanPromotion([FromQuery] Guid promotionId)
        {
            var result = await _promotionService.BanPromotion(promotionId);
            return Ok(result);
        }
    }
}
