using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;
using System.Security.Claims;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseDesignVersionController : ControllerBase
    {
        private readonly IHouseDesignVersionService _designVersionService;

        public HouseDesignVersionController(IHouseDesignVersionService designVersionService)
        {
            _designVersionService = designVersionService;
        }

        #region GetDetailVersionById
        /// <summary>
        /// Return version item by versionId - App mobile
        /// 
        /// ROLE: CUSTOMER - DESIGNSTAFF - MANAGER
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Customer, Manager, DesignStaff")]
        [HttpGet(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionDetailEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetailVersionById(Guid versionId)
        {
            var versionItem = await _designVersionService.GetDetailVersionById(versionId);
            var result = JsonConvert.SerializeObject(versionItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateHouseDesignVersion
        /// <summary>
        /// Creates a new version of a house design drawing in the system or Create by available version
        /// </summary>
        /// <remarks>
        /// This endpoint allows managers to create a new version of a house design drawing. <br/>
        /// **Example:** <br/>
        /// Phối cảnh version 1.0 -> Update Phối cảnh version 2.0 <br/><br/>
        /// **Sample Request:**
        ///
        ///     POST /api/v1/design
        ///     {
        ///       "name": "Kiến trúc",
        ///       "houseDesignDrawingId": "EAB8D4FF-C6D4-452A-AB89-DEDC8D3F5153",
        ///       "fileUrl": "https://designfile....",
        ///       "relatedDrawingId": "3905068B-8E24-4FC1-8663-A6697903DE08",
        ///       "previousDrawingId": "3905068B-8E24-4FC1-8663-A6697903DE08"
        ///     }
        ///
        /// **Request Fields:**
        /// - **accountId** (Guid, Required): The unique identifier of the account creating the design.
        /// - **name** (string, Required): The name of the design version (max 100 characters).
        /// - **houseDesignDrawingId** (Guid, Required): The unique identifier of the associated house design drawing.
        /// - **fileUrl** (string, Optional): A URL to the design version file (must be a valid URL).
        /// - **relatedDrawingId** (Guid, Optional): The ID of the related drawing, if applicable.
        /// - **previousDrawingId** (Guid, Optional): The ID of the previous design version, if this version builds upon it.
        ///
        /// </remarks>
        /// <param name="item">Request model containing the house design version details</param>
        /// <returns>Returns true if the house design version is created successfully, otherwise false.</returns>
        /// <response code="200">House design version created successfully</response>
        /// <response code="400">Bad request, validation failed or missing required fields</response>
        /// <response code="401">Unauthorized, only managers are allowed to create design versions</response>
        /// <response code="500">Internal server error</response>
        #endregion
        [Authorize(Roles = "DesignStaff")]
        [HttpPost(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHouseDesignVersion([FromBody] HouseDesignVersionRequest item)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isCreate = await _designVersionService.CreateHouseDesignVersion(accountId, item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region UploadDesignFiles
        /// <summary>
        /// Uploads multiple PDF files for a house design version.
        /// </summary>
        /// <remarks>
        /// This endpoint allows design staff to upload multiple PDF files related to a house design version.
        /// The request must include the `versionId` as a URL parameter and the PDF files as form data.
        /// 
        /// **Sample Request:**
        /// 
        ///     POST /api/v1/design/upload-files/{versionId}
        ///     Content-Type: multipart/form-data
        ///     Files: [file1.pdf, file2.pdf, file3.pdf]
        /// 
        /// **Request Fields:**
        /// - **files** (IFormFile[], Required): The list of PDF files to upload.
        /// 
        /// **URL Parameters:**
        /// - **versionId** (Guid, Required): The unique identifier of the house design version.
        /// 
        /// **Authorization:** Only users with the "Design Staff" role can perform this action.
        /// 
        /// </remarks>
        /// <param name="files">List of PDF files to upload</param>
        /// <param name="versionId">The ID of the house design version to associate the files with</param>
        /// <returns>Returns true if the files are uploaded successfully, otherwise false.</returns>
        /// <response code="200">Files uploaded successfully</response>
        /// <response code="400">Bad request, validation failed or missing required fields</response>
        /// <response code="401">Unauthorized, only design staff can upload files</response>
        /// <response code="404">Not found, the house design version with the specified ID was not found</response>
        /// <response code="500">Internal server error</response>
        #endregion
        [Authorize(Roles = "DesignStaff")]
        [HttpPut(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionDetailEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateWorkDesignStaff([FromForm] List<IFormFile> files, Guid versionId)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files provided.");
            }
            var isCreate = await _designVersionService.UploadDesignDrawing(files, versionId);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region AssignHouseDrawing
        /// <summary>
        /// Assigns a specific house drawing to a house design version.
        /// </summary>
        /// <remarks>
        /// type = "Approved"  - Manager approved drawing -> Send to customer
        /// type = "Updated" - Manager reject drawing -> Design Staff work again
        /// </remarks>
        /// <param name="Id">The ID of the house design version.</param>
        /// <param name="request">Details of the house drawing to assign.</param>
        /// <returns>True if successful, false otherwise.</returns>
        /// <response code="200">If the house drawing was assigned successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="404">If the house design version or drawing was not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.HouseDesignVersion.ApproveHouseDesignVersionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignHouseDrawing([FromQuery] Guid Id, [FromBody] AssignHouseDrawingRequest request)
        {
            var isApprove = await _designVersionService.ApproveHouseDrawing(Id, request);
            return Ok(isApprove ? AppConstant.Message.SUCCESSFUL_INITIAL : AppConstant.Message.ERROR);
        }

        #region ConfirmDesignDrawingFromCustomer
        /// <summary>
        /// Confirms the agreement of an house desing drawings from a customer.
        /// 
        /// ROLE: CUSTOMER
        /// </summary>
        #endregion
        //[Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.HouseDesignVersion.HouseDesignVerisonConfirmEndpoint)]
        [ProducesResponseType(typeof(UpdateInitialRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmDesignDrawingFromCustomer(Guid versionId)
        {
            var result = await _designVersionService.ConfirmDesignDrawingFromCustomer(versionId);

            return Ok(result);
        }

        #region CommentDesignDrawingFromCustomer
        /// <summary>
        /// When customer need to fix house desgin drawing -> Customer comment and click "Gửi"
        /// 
        /// ROLE: CUSTOMER
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionFeedbackEndpoint)]
        [ProducesResponseType(typeof(UpdateInitialRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FeedbackFixInitialFromCustomer(Guid versionId, FeedbackHouseDesignDrawingRequest request)
        {
            var result = await _designVersionService.CommentDesignDrawingFromCustomer(versionId, request);

            return Ok(result);
        }
    }
}
