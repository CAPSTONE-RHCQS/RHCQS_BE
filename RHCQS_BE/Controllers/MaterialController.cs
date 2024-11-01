using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Request.Material;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Material;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        #region GetListMaterial
        /// <summary>
        /// Retrieves the list of all material.
        /// </summary>
        /// <returns>List of material in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Material.MaterialEndpoint)]
        [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListMaterial(int page, int size)
        {
            var listMaterials = await _materialService.GetListMaterial(page, size);
            var result = JsonConvert.SerializeObject(listMaterials, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailMaterial
        /// <summary>
        /// Retrieves the material.
        /// </summary>
        /// <returns>Material in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Material.MaterialDetailEndpoint)]
        [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailMaterial(Guid id)
        {
            var material = await _materialService.GetDetailMaterial(id);
            var result = JsonConvert.SerializeObject(material, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateMaterial
        /// <summary>
        /// Creates a new material in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/material
        ///     {
        ///  "supplierId": "5542A8DE-2E1C-4C44-B990-795AB05E3685",
        ///  "materialTypeId": "FB2F4D82-F30E-4963-967A-156F763DEB94",
        ///  "materialSectionId": "EB96AE7B-7D96-4AFF-850C-16D618E37EE4",
        ///  "name": "Ngói xi măng màu xanh",
        ///  "price": 16000,
        ///  "unit": "viên",
        ///  "size": "32cm x 32cm",
        ///  "shape": "Hình vuông",
        ///  "imgUrl": null,
        ///  "description": "Ngói xi măng phủ sơn màu xanh, độ bền cao, chống thấm tốt.",
        ///  "isAvailable": true,
        ///  "unitPrice": "vnđ"
        ///     }
        /// </remarks>
        /// <param name="request">Material request model</param>
        /// <returns>Returns true if the material is created successfully, otherwise false.</returns>
        /// <response code="200">Material created successfully</response>
        /// <response code="400">Failed to create the material</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Material.MaterialEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMaterial([FromBody] MaterialRequest request)
        {
            var isCreated = await _materialService.CreateMaterial(request);
            return isCreated ? Ok(isCreated) : BadRequest();
        }

        #region UpdateMaterial
        /// <summary>
        /// Updates an existing material.
        /// Requires Manager role for authorization.
        /// </summary>
        /// <param name="id">The unique identifier of material to be updated.</param>
        /// <param name="request">The request body containing the updated construction details.</param>
        /// <returns>A boolean value indicating the success or failure of the update operation.</returns>
        /// <response code="200">Returns true if the update is successful.</response>
        /// <response code="400">Returns BadRequest if the update fails or if validation issues occur.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Material.MaterialEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMaterial(Guid id, [FromBody] MaterialUpdateRequest request)
        {
            var isUpdated = await _materialService.UpdateMaterial(id, request);
            return isUpdated ? Ok(isUpdated) : BadRequest();
        }

        #region SearchMaterialByName
        /// <summary>
        /// Searches materials by name.
        /// </summary>
        /// <param name="name">The name or partial name of the material.</param>
        #endregion 
        [Authorize(Roles = "Manager")]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IPaginate<MaterialResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchMaterialByName(string name, int page, int size)
        {
            var listSearchMaterial = await _materialService.SearchMaterialByName(name, page, size);
            var result = JsonConvert.SerializeObject(listSearchMaterial, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
        

        #region FilterMaterialByType
        /// <summary>
        /// Filters materials by material type.
        /// </summary>
        /// <param name="materialTypeId">The ID of the material type to filter by.</param>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet("filter")]
        [ProducesResponseType(typeof(IPaginate<MaterialResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FilterMaterialByType(Guid materialTypeId, int page, int size)
        {
            var filterMaterial = await _materialService.FilterMaterialByType(materialTypeId, page, size);
            var result = JsonConvert.SerializeObject(filterMaterial, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
        
    }
}
