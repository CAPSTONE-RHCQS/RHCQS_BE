using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_Services.Interface;

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
        ///       "accountId": "990773A2-1817-47F5-9116-301E97435C44",
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
        [Authorize(Roles = "Design Staff")]
        [HttpPost(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHouseDesignVersion([FromBody] HouseDesignVersionRequest item)
        {
            var isCreate = await _designVersionService.CreateHouseDesignVersion(item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region UpdateWorkDesignStaff
        /// <summary>
        /// Updates an existing house design version with new details or file uploads.
        /// </summary>
        /// <remarks>
        /// This endpoint allows design staff to update details or upload files for an existing house design version.
        /// The request must include the new information for the version along with the `versionId` as a URL parameter.
        /// 
        /// **Sample Request:**
        /// 
        ///     PUT /api/v1/design/id
        ///     {
        ///       "fileUrl": "http://example.com/updated-design-file.pdf"
        ///     }
        /// 
        /// **Request Fields:**
        /// - **fileUrl** (string, Optional): The updated URL to the design version file (must be a valid URL).
        /// 
        /// **URL Parameters:**
        /// - **versionId** (Guid, Required): The unique identifier of the house design version to be updated.
        /// 
        /// **Authorization:** Only users with the "Design Staff" role can perform this action.
        /// 
        /// </remarks>
        /// <param name="item">Request model containing the updated house design version details</param>
        /// <param name="versionId">The ID of the house design version to be updated</param>
        /// <returns>Returns true if the house design version is updated successfully, otherwise false.</returns>
        /// <response code="200">House design version updated successfully</response>
        /// <response code="400">Bad request, validation failed or missing required fields</response>
        /// <response code="401">Unauthorized, only design staff can update design versions</response>
        /// <response code="404">Not found, the house design version with the specified ID was not found</response>
        /// <response code="500">Internal server error</response>
        #endregion
        [Authorize(Roles = "Design Staff")]
        [HttpPut(ApiEndPointConstant.HouseDesignVersion.HouseDesignVersionDetailEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateWorkDesignStaff([FromBody] HouseDesignVersionUpdateRequest item, Guid versionId)
        {
            var isCreate = await _designVersionService.UploadDesignDrawing(item, versionId);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
