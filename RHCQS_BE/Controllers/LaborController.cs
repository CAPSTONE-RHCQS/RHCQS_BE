using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborController : ControllerBase
    {
        private readonly ILaborService _laborService;

        public LaborController(ILaborService laborService)
        {
            _laborService = laborService;
        }

        #region GetListLabor
        /// <summary>
        /// Retrieves the list of all labors.
        /// </summary>
        /// <returns>List of labor in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Labor.LaborEndpoint)]
        [ProducesResponseType(typeof(LaborResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListLabor(int page, int size)
        {
            var listLabors = await _laborService.GetListLabor(page, size);
            var result = JsonConvert.SerializeObject(listLabors, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        
    }
}
