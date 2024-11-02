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

        
    }
}
