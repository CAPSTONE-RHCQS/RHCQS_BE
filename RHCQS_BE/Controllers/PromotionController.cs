using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
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
        //[Authorize(Roles = "Customer, SalesStaff, Manager")]
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
    }
}
