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

        
    }
}
