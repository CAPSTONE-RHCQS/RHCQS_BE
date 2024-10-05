using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
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
            var package = await _packageService.GetListPackageAsync(page, size);
            var response = JsonConvert.SerializeObject(package, Formatting.Indented);
            return Ok(response);
        }
        #region PackageDetail
        /// <summary>
        /// Get detailpackage by id.
        /// </summary>
        /// <param id="id">The id to get for.</param>
        /// <returns>The detailpackage match with id.</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Package.PackageDetailEndpoint)]
        public async Task<ActionResult<PackageResponse>> GetPackageDetail(Guid id)
        {
            var packagedetail = await _packageService.GetPackageDetail(id);
            var result = JsonConvert.SerializeObject(packagedetail, Formatting.Indented);
            return Ok(result);
        }
        #region SearchPackageByName
        /// <summary>
        /// Get detailpackage by name.
        /// </summary>
        /// <param id="id">The name to get for.</param>
        /// <returns>The detailpackage match with name.</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Package.PackageByNameEndpoint)]
        public async Task<ActionResult<PackageResponse>> GetPackageDetailByName(string name)
        {
            var packagedetail = await _packageService.GetPackageByName(name);
            var result = JsonConvert.SerializeObject(packagedetail, Formatting.Indented);
            return Ok(result);
        }
    }
}
