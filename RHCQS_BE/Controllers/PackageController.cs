using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IUploadImgService _uploadImgService;
        public PackageController(IPackageService packageService, IUploadImgService uploadImgService)
        {
            _packageService = packageService;
            _uploadImgService = uploadImgService;
        }
        #region GetListPackageAsync
        /// <summary>
        /// Retrieves the list of all package.
        /// </summary>
        /// <returns>List of packages in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Package.PackageEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<Package>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Package>>> GetListPackageAsync(int page, int size)
        {
            var package = await _packageService.GetListPackageAsync(page, size);
            var response = JsonConvert.SerializeObject(package, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region GetListPackage
        /// <summary>
        /// Retrieves package list without entering page and size.
        /// </summary>
        /// <returns>List of packages in the system</returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.Package.PackageListEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<Package>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PackageResponseForMobile>>> GetListPackage()
        {
            var package = await _packageService.GetListPackage();
            var response = JsonConvert.SerializeObject(package, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region PackageDetail
        /// <summary>
        /// Get detailpackage by id.
        /// </summary>
        /// <param id="id">The id to get for.</param>
        /// <returns>The detailpackage match with id.</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Package.PackageDetailEndpoint)]
        public async Task<ActionResult<PackageResponse>> GetPackageDetail(Guid id)
        {
            var packagedetail = await _packageService.GetPackageDetail(id);
            var result = JsonConvert.SerializeObject(packagedetail, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region SearchPackageByName
        /// <summary>
        /// Get detailpackage by name.
        /// </summary>
        /// <param id="id">The name to get for.</param>
        /// <returns>The detailpackage match with name.</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Package.PackageByNameEndpoint)]
        public async Task<ActionResult<PackageResponse>> GetPackageDetailByName(string name)
        {
            var packagedetail = await _packageService.GetPackageByName(name);
            var result = JsonConvert.SerializeObject(packagedetail, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region CreatePackage
        /// <summary>
        /// Creates a new Package.
        /// </summary>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.Package.PackageEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePackage([FromBody] PackageRequest package)
        {
            //foreach (var packageHouse in package.PackageHouses)
            //{
            //    if (!string.IsNullOrEmpty(packageHouse.ImgUrl))
            //    {
            //        string packageURL = await _uploadImgService.UploadImageAsync(packageHouse.ImgUrl, "PackageHouse");
            //        packageHouse.ImgUrl = packageURL;
            //    }
            //}
            var isCreate = await _packageService.CreatePackage(package);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
        #region UpdatePackage
        /// <summary>
        /// Update a Package.
        /// </summary>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.Package.PackageEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageRequest package, Guid packageid)
        {
            //foreach (var packageHouse in package.PackageHouses)
            //{
            //    if (!string.IsNullOrEmpty(packageHouse.ImgUrl))
            //    {
            //        string packageURL = await _uploadImgService.UploadImageAsync(packageHouse.ImgUrl, "PackageHouse");
            //        packageHouse.ImgUrl = packageURL;
            //    }
            //}
            var update = await _packageService.UpdatePackage(package, packageid);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var result = JsonConvert.SerializeObject(update, Formatting.Indented, settings);

            // Return the result as a JSON response
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region GetDetailPackageByContainName
        /// <summary>
        /// Search name package by character
        /// 
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// </summary>
        /// <returns>List of packages in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Package.PackageAutoCharacter)]
        [ProducesResponseType(typeof(IEnumerable<Package>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Package>>> GetDetailPackageByContainName(string name)
        {
            var package = await _packageService.GetDetailPackageByContainName(name);
            var response = JsonConvert.SerializeObject(package, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region PackageDetail PDF
        /// <summary>
        /// create detailpackage pdf by id.
        /// </summary>
        /// <param id="id">The id to get for.</param>
        /// <returns>The detailpackage match with id.</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Package.PackageDetailPDFEndpoint)]
        public async Task<IActionResult> GeneratePackagePdf(Guid packageId)
        {
            var pdfUrl = await _packageService.GeneratePackagePdf(packageId);
            var response = JsonConvert.SerializeObject(pdfUrl, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };

        }
        #region delete package
        /// <summary>
        /// delete package by ID.
        /// </summary>
        /// <param name="id">ID of the package need to delete</param>
        /// <returns>return result or error</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpDelete(ApiEndPointConstant.Package.DeletePackageEndpoint)]
        public async Task<IActionResult> DeletePackage(Guid id)
        {

            var result = await _packageService.DeletePackage(id);

            if (result)
                return NoContent();

            return BadRequest(new { message = "Xóa gói thất bại." });

        }

       
    }
}
