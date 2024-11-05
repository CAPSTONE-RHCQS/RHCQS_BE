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

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
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

        
    }
}
