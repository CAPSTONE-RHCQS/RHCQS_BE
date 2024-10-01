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
    public class HouseDesignDrawingController : ControllerBase
    {
        private readonly IHouseDesignDrawingService _houseService;

        public HouseDesignDrawingController(IHouseDesignDrawingService houseService)
        {
            _houseService = houseService;
        }

        #region GetListHouseDesignDrawing
        /// <summary>
        /// Retrieves the list of all house design drawing item.
        /// </summary>
        /// <returns>List of house design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListHouseDesignDrawing(int page, int size)
        {
            var listHouseDesignDrawings = await _houseService.GetListHouseDesignDrawings(page, size);
            var result = JsonConvert.SerializeObject(listHouseDesignDrawings, Formatting.Indented);
            return Ok(result);
        }

        #region GetDetailHouseDesignDrawing
        /// <summary>
        /// Retrieves the house design drawing item.
        /// </summary>
        /// <returns>Item constructhouse design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, Sales Staff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingDetailEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailHouseDesignDrawing(Guid id)
        {
            var design = await _houseService.GetDetailHouseDesignDrawing(id);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return Ok(result);
        }

        #region GetListTaskByAccount
        /// <summary>
        /// Retrieves the house design drawing item.
        /// </summary>
        /// <returns>Item constructhouse design drawing in the system</returns>
        #endregion
        //[Authorize(Roles = "Customer, Sales Staff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingTask)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListTaskByAccount(Guid id)
        {
            var design = await _houseService.GetListTaskByAccount(id);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return Ok(result);
        }

        #region GetDetailHouseDesignDrawingByType
        /// <summary>
        /// Retrieves the list of house design drawings based on the specified type.
        /// </summary>
        /// <remarks>
        /// This API returns a list of house design drawings filtered by the provided `type`. 
        /// The possible values for `type` are:
        /// 
        /// - **PHOICANH**: Retrieves all construction items related to perspectives.
        /// - **KIENTRUC**: Retrieves all architectural construction items.
        /// - **KETCAU**: Retrieves all structural construction items.
        /// - **DIENNUOC**: Retrieves all electrical and plumbing construction items.
        /// 
        /// Example request:
        /// 
        ///     GET /api/v1/housedesign/type?type=PHOICANH
        ///     GET /api/v1/housedesign/type?type=KIENTRUC
        ///     GET /api/v1/housedesign/type?type=KETCAU
        ///     GET /api/v1/housedesign/type?type=DIENNUOC
        ///     
        /// If no valid `type` is provided, a 400 Bad Request response will be returned.
        /// </remarks>
        /// <param name="type">
        /// The type of house design drawings to retrieve. Must be one of the following values:
        /// - PHOICANH: For perspective-related design drawings.
        /// - KIENTRUC: For architectural-related design drawings.
        /// - KETCAU: For structural-related design drawings.
        /// - DIENNUOC: For electrical and plumbing-related design drawings.
        /// </param>
        /// <returns>
        /// A list of house design drawings of the specified type.
        /// </returns>
        /// <response code="200">Returns the list of house design drawings successfully.</response>
        /// <response code="400">Returns when the `type` is invalid or not provided.</response>
        /// <response code="500">Returns if there is an internal server error during the request processing.</response>
        #endregion
        [Authorize(Roles = "Design Staff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingTypeEndpoint)]
        [ProducesResponseType(typeof(List<HouseDesignDrawingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDetailHouseDesignDrawingByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type) ||
                (type != AppConstant.Type.PHOICANH &&
                type != AppConstant.Type.KIENTRUC &&
                type != AppConstant.Type.KETCAU &&
                type != AppConstant.Type.DIENNUOC))
            {
                return BadRequest("Invalid type. Allowed values are house design drawing");
            }

            var design = await _houseService.GetDetailHouseDesignDrawingByType(type.ToUpper());
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return Ok(result);
        }

        #region CreateHouseDesignDrawing
        /// <summary>
        /// Creates a new house design drawing in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/housedesigndrawing
        ///     {
        ///       "projectId": "b21a5a33-0b24-4c1a-8a55-6f29d6ef9e12"
        ///     }
        /// 
        /// </remarks>
        /// <param name="item">Request model containing the house design drawing details</param>
        /// <returns>Returns true if the house design drawing is created successfully, otherwise false.</returns>
        /// <response code="200">House design drawing created successfully</response>
        /// <response code="400">Failed to create the house design drawing</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHouseDesignDrawing([FromBody] HouseDesignDrawingRequest item)
        {
            var isCreate = await _houseService.CreateHouseDesignDrawing(item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
