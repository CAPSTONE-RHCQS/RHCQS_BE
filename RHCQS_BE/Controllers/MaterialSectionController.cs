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
    public class MaterialSectionController : ControllerBase
    {
        private readonly IMaterialSectionService _materialSectionService;

        public MaterialSectionController(IMaterialSectionService materialSectionService)
        {
            _materialSectionService = materialSectionService;
        }

        #region GetListMaterialSection
        /// <summary>
        /// Retrieves the list of all material sections.
        /// </summary>
        /// <returns>List of material section in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.MaterialSection.MaterialSectionEndpoint)]
        [ProducesResponseType(typeof(MaterialTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListMaterialSection(int page, int size)
        {
            var listMaterialSections = await _materialSectionService.GetListMaterialSection(page, size);
            var result = JsonConvert.SerializeObject(listMaterialSections, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailMaterialSection
        /// <summary>
        /// Retrieves the material section.
        /// </summary>
        /// <returns>Material section in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.MaterialSection.MaterialSectionDetailEndpoint)]
        [ProducesResponseType(typeof(MaterialSectionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailMaterialSection(Guid id)
        {
            var materialSection = await _materialSectionService.GetDetailMaterialSection(id);
            var result = JsonConvert.SerializeObject(materialSection, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateMaterialSection
        /// <summary>
        /// Creates a new labor in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/materialsection
        ///    {
        ///    "name": "Test"
        ///     }
        /// </remarks>
        /// <param name="request">Material section request model</param>
        /// <returns>Returns true if the material section is created successfully, otherwise false.</returns>
        /// <response code="200">Material section created successfully</response>
        /// <response code="400">Failed to create the material section</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.MaterialSection.MaterialSectionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMaterialSection([FromBody] MaterialSectionRequest request)
        {
            var isCreated = await _materialSectionService.CreateMaterialSection(request);
            return isCreated ? Ok(isCreated) : BadRequest();
        }

        #region UpdateMaterialSection
        /// <summary>
        /// Updates an existing material section.
        /// Requires Manager role for authorization.
        /// </summary>
        /// <param name="id">The unique identifier of material section to be updated.</param>
        /// <param name="request">The request body containing the updated material section details.</param>
        /// <returns>A boolean value indicating the success or failure of the update operation.</returns>
        /// <response code="200">Returns true if the update is successful.</response>
        /// <response code="400">Returns BadRequest if the update fails or if validation issues occur.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.MaterialSection.MaterialSectionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMaterialType(Guid id, [FromBody] MaterialSectionRequest request)
        {
            var isUpdated = await _materialSectionService.UpdateMaterialSection(id, request);
            return isUpdated ? Ok(isUpdated) : BadRequest();
        }

        
    }
}
