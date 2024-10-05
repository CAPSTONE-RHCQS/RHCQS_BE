using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        #region GetListPackageAsync
        /// <summary>
        /// Retrieves the list of all package.
        /// </summary>
        /// <returns>List of packages in the system</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Package.PackageEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<Package>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PackageType>>> GetListPackageAsync(int page, int size)
        {
            var roles = await _packageService.GetListPackageAsync(page, size);
            var response = JsonConvert.SerializeObject(roles, Formatting.Indented);
            return Ok(response);
        }
    }
}
