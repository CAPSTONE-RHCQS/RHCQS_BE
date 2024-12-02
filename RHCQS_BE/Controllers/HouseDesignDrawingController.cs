using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using System.Security.Claims;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseDesignDrawingController : ControllerBase
    {
        private readonly IHouseDesignDrawingService _houseService;
        private readonly IFirebaseService _firebaseService;
        private readonly IAccountService _accountService;
        public HouseDesignDrawingController(IHouseDesignDrawingService houseService, IFirebaseService firebaseService, IAccountService accountService)
        {
            _houseService = houseService;
            _firebaseService = firebaseService;
            _accountService = accountService;
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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListHouseDesignDrawingsForDesignStaff
        /// <summary>
        /// Retrieves the list of all house design drawing item.
        /// </summary>
        /// <returns>List of house design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "DesignStaff")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingDesignStaffEndpont)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListHouseDesignDrawingsForDesignStaff(int page, int size)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var listHouseDesignDrawings = await _houseService.GetListHouseDesignDrawingsForDesignStaff(page, size, accountId);
            var result = JsonConvert.SerializeObject(listHouseDesignDrawings, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ViewDrawingPreviousStep
        /// <summary>
        /// Retrieves the list of all house design drawing item.
        /// </summary>
        /// <returns>List of house design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "DesignStaff")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingPreviousEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewDrawingPreviousStep(Guid projectId)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var listHouseDesignDrawings = await _houseService.ViewDrawingPreviousStep(accountId, projectId);
            object result;

            if (listHouseDesignDrawings == null || !listHouseDesignDrawings.Any())
            {
                result = new { message = AppConstant.ErrMessage.DesignNoAccess };
            }
            else
            {
                result = listHouseDesignDrawings;
            }

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(result, Formatting.Indented),
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }


        #region GetDetailHouseDesignDrawing
        /// <summary>
        /// Retrieves the house design drawing item.
        /// </summary>
        /// <returns>Item constructhouse design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, DesignStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingDetailEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailHouseDesignDrawing(Guid id)
        {
            var design = await _houseService.GetDetailHouseDesignDrawing(id);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListTaskByAccount
        /// <summary>
        /// Retrieves the house design drawing item.
        /// </summary>
        /// <returns>Item constructhouse design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingTask)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListTaskByAccount()
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var design = await _houseService.GetListTaskByAccount(accountId);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
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
        [Authorize(Roles = "DesignStaff, Manager")]
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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateHouseDesignDrawing
        /// <summary>
        /// Creates a new set of house design drawings in the system for the specified project.
        /// </summary>
        /// <remarks>
        /// 
        /// 4 DesignerId for 4 Design Drawing
        /// 
        /// Sample request:
        /// 
        ///     POST /api/v1/housedesigndrawing
        ///     {
        ///       "projectId": "B81935A8-4482-43F5-AD68-558ABDE58D58",
        ///       "designerPerspective": "BF339E88-5303-45C4-A6F4-33A79681766C",
        ///       "designerArchitecture": "990773A2-1817-47F5-9116-301E97435C44",
        ///       "designerStructure": "28247CD1-67CA-439D-BEF5-FCA9A9A777C5",
        ///       "designerElectricityWater": "28247CD1-67CA-439D-BEF5-FCA9A9A777C6"
        ///     }
        /// 
        /// This API creates multiple design drawings (Perspective, Architecture, Structure, and ElectricityWater)
        /// for a given project based on the provided designer details. It checks if a maximum of 4 drawings already exist 
        /// for the project, and prevents creating more. It also ensures that no designer has more than 2 drawings that are not accepted.
        /// </remarks>
        /// <param name="item">Request model containing the house design drawing and designer details</param>
        /// <returns>Returns success message if the house design drawing is created successfully, otherwise returns an error message.</returns>
        /// <response code="200">House design drawings created successfully</response>
        /// <response code="400">Failed to create house design drawings, either because the project has reached its limit or designers are overloaded</response>
        /// <response code="409">Conflict error if a designer is overloaded with pending drawings</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHouseDesignDrawing([FromBody] HouseDesignDrawingRequest item)
        {
            var result = await _houseService.CreateListTaskHouseDesignDrawing(item);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        #region ViewDrawingByProjectId
        /// <summary>
        /// Retrieves the house design drawing item.
        /// </summary>
        /// <returns>Item constructhouse design drawing in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, DesignStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDrawingListEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewDrawingByProjectId(Guid projectId)
        {
            var design = await _houseService.ViewDrawingByProjectId(projectId);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        [Authorize(Roles = "Customer, SalesStaff, DesignStaff, Manager")]
        [HttpPut(ApiEndPointConstant.HouseDesignDrawing.HouseDesignConfirmProjectHaveDrawing)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmDrawingAvaliable(Guid versionId, AssignHouseDrawingRequest request)
        {
            var design = await _houseService.ConfirmDrawingAvaliable(versionId, request);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);

            var detail = await _houseService.GetDetailHouseDesignDrawing(versionId);

            var customerEmail = await _accountService.GetEmailByProjectIdAsync(detail.ProjectId);
            var deviceToken = await _firebaseService.GetDeviceTokenAsync(customerEmail);
            var notificationRequest = new NotificationRequest
            {
                Email = customerEmail,
                DeviceToken = deviceToken,
                Title = "Bản vẽ có cập nhật",
                Body = $"Bản vẽ có cập nhật có cập nhật mới bạn cần xem."
            };
            await _firebaseService.SendNotificationAsync(
                notificationRequest.Email,
                notificationRequest.DeviceToken,
                notificationRequest.Title,
                notificationRequest.Body
            );
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region DesignRequirements
        /// <summary>
        /// Customers request design when initial quotation has finalized
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <remarks>
        /// ProjectId
        /// </remarks>
        /// <param name="projectId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPost(ApiEndPointConstant.HouseDesignDrawing.HouseDesignDesignRequiment)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DesignRequirements([FromBody] Guid projectId)
        {
            var design = await _houseService.DesignRequirements(projectId);
            if (design == null) return NotFound(new { message = AppConstant.ErrMessage.HouseDesignDrawing });
            var result = JsonConvert.SerializeObject(design, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetStatusHouseDesign
        /// <summary>
        /// Get status projects for Web
        /// 
        /// Role: DESIGNSTAFF - SALES STAFF - MANAGER
        /// </summary>
        /// <returns>Number of projects.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseDesignDrawing.HouseDesignStatusEndpoint)]
        public async Task<ActionResult<int>> GetStatusHouseDesign(Guid houseDesignId)
        {
            var totalProjectCount = await _houseService.GetStatusHouseDesign(houseDesignId);
            return Ok(totalProjectCount);
        }
    }
}
