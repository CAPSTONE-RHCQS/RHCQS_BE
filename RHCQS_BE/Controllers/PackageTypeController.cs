using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTypeController : ControllerBase
    {
        private readonly IPackageTypeService _packageService;

        public PackageTypeController(IPackageTypeService packageService)
        {
            _packageService = packageService;
        }
        #region GetListPackageTypeAsync
        /// <summary>
        /// Retrieves the list of all packagetype.
        /// </summary>
        /// <returns>List of packagetypes in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.PackageType.PackageTypeEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<PackageType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PackageType>>> GetListPackageAsync(int page, int size)
        {
            var roles = await _packageService.GetAllPackageTypesAsync(page, size);
            var response = JsonConvert.SerializeObject(roles, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region CreatePackageTypeAsync
        /// <summary>
        /// Creates a new package type.
        /// </summary>
        /// <param name="packageType">The package type to create.</param>
        /// <returns>The created package type.</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.PackageType.PackageTypeEndpoint)]
        public async Task<IActionResult> CreatePackageTypeAsync([FromBody] PackageTypeRequest packageType)
        {
            if (packageType == null || string.IsNullOrEmpty(packageType.Name))
            {
                return BadRequest("Invalid package type data.");
            }

            var createdPackageType = await _packageService.CreatePackageTypeAsync(packageType);

            return Ok(new { message = "Tạo packagetype thành công!" });
        }
    }
}
