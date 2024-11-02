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
    public class MaterialTypeController : ControllerBase
    {
        private readonly IMaterialTypeService _materialTypeService;

        public MaterialTypeController(IMaterialTypeService materialTypeService)
        {
            _materialTypeService = materialTypeService;
        }

        #region GetListMaterialType
        /// <summary>
        /// Retrieves the list of all material types.
        /// </summary>
        /// <returns>List of material type in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.MaterialType.MaterialTypeEndpoint)]
        [ProducesResponseType(typeof(MaterialTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListMaterialType(int page, int size)
        {
            var listMaterialTypes = await _materialTypeService.GetListMaterialType(page, size);
            var result = JsonConvert.SerializeObject(listMaterialTypes, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailMaterialType
        /// <summary>
        /// Retrieves the material type.
        /// </summary>
        /// <returns>Material type in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.MaterialType.MaterialTypeDetailEndpoint)]
        [ProducesResponseType(typeof(MaterialTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailMaterialType(Guid id)
        {
            var materialtype = await _materialTypeService.GetDetailMaterialType(id);
            var result = JsonConvert.SerializeObject(materialtype, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateMaterialType
        /// <summary>
        /// Creates a new labor in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/materialtype
        ///    {
        ///    "name": "Test",
        ///    "deflag": true
        ///     }
        /// </remarks>
        /// <param name="request">Material type request model</param>
        /// <returns>Returns true if the material type is created successfully, otherwise false.</returns>
        /// <response code="200">Material type created successfully</response>
        /// <response code="400">Failed to create the material type</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.MaterialType.MaterialTypeEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMaterialType([FromBody] MaterialTypeRequest request)
        {
            var isCreated = await _materialTypeService.CreateMaterialType(request);
            return isCreated ? Ok(isCreated) : BadRequest();
        }

    }
}
