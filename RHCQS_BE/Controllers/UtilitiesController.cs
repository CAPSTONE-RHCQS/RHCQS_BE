using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilitiesService _utilitiesService;

        public UtilitiesController(IUtilitiesService utilitiesService)
        {
            _utilitiesService = utilitiesService;
        }

        #region GetListUtilities
        /// <summary>
        /// Retrieves the list of all utilities item.
        /// </summary>
        /// <returns>List of utilities in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, Sales Staff, Manager")]
        [HttpGet(ApiEndPointConstant.Utility.UtilityEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListUtilities(int page, int size)
        {
            var listConstructions = await _utilitiesService.GetListUtilities(page, size);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return Ok(result);
        }
    }
}
