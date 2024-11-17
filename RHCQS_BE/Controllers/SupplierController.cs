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
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IUploadImgService _uploadImgService;

        public SupplierController(ISupplierService supplierService, IUploadImgService uploadImgService)
        {
            _supplierService = supplierService;
            _uploadImgService=uploadImgService;
        }

        #region GetListSupplier
        /// <summary>
        /// Retrieves the list of all suppliers.
        /// </summary>
        /// <returns>List of supplier in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Supplier.SupplierEndpoint)]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListSupplier(int page, int size)
        {
            var listSuppliers = await _supplierService.GetListSupplier(page, size);
            var result = JsonConvert.SerializeObject(listSuppliers, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailSupplier
        /// <summary>
        /// Retrieves the supplier.
        /// </summary>
        /// <returns>Supplier in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Supplier.SupplierDetailEndpoint)]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailSupplier(Guid id)
        {
            var supplier = await _supplierService.GetDetailSupplier(id);
            var result = JsonConvert.SerializeObject(supplier, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateSupplier
        /// <summary>
        /// Creates a new supplier in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/supplier
        ///     {
        ///     "name": "string",
        ///     "email": "string",
        ///     "constractPhone": "string",
        ///     "imgUrl": "string",
        ///     "deflag": true,
        ///     "shortDescription": "string",
        ///     "description": "string"
        ///     }
        /// </remarks>
        /// <param name="request">Supplier request model</param>
        /// <returns>Returns true if the supplier is created successfully, otherwise false.</returns>
        /// <response code="200">Supplier created successfully</response>
        /// <response code="400">Failed to create the supplier</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Supplier.SupplierEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSupplier([FromForm] SupplierRequest request)
        {
            try
            {
                string? imageUrl = null;
                if (request.Image != null && request.Image.Length > 0)
                {
                    imageUrl = await _supplierService.UploadSupplierImage(request.Image);
                }
                request.ImgUrl = imageUrl;

                var isCreated = await _supplierService.CreateSupplier(request);
                return isCreated ? Ok(isCreated) : BadRequest("Failed to create supplier.");
            }
            catch (AppConstant.MessageError ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        #region UpdateSupplier
        /// <summary>
        /// Updates an existing supplier.
        /// Requires Manager role for authorization.
        /// </summary>
        /// <param name="id">The unique identifier of supplier to be updated.</param>
        /// <param name="request">The request body containing the updated supplier details.</param>
        /// <returns>A boolean value indicating the success or failure of the update operation.</returns>
        /// <response code="200">Returns true if the update is successful.</response>
        /// <response code="400">Returns BadRequest if the update fails or if validation issues occur.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Supplier.SupplierEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] SupplierRequest request)
        {
            var isUpdated = await _supplierService.UpdateSupplier(id, request);
            return isUpdated ? Ok(isUpdated) : BadRequest();
        }

        #region SearchSupplierByName
        /// <summary>
        /// Searches suppliers by name.
        /// </summary>
        /// <param name="name">The name or partial name of the supplier.</param>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<SupplierResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSupplierByName(string name)
        {
            var listSearchSupplier = await _supplierService.SearchSupplierByName(name);
            var result = JsonConvert.SerializeObject(listSearchSupplier, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
