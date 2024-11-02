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

        #region GetDetailLabor
        /// <summary>
        /// Retrieves the labor.
        /// </summary>
        /// <returns>Labor in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Labor.LaborDetailEndpoint)]
        [ProducesResponseType(typeof(LaborResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailLabor(Guid id)
        {
            var labor = await _laborService.GetDetailLabor(id);
            var result = JsonConvert.SerializeObject(labor, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateLabor
        /// <summary>
        /// Creates a new labor in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/labor
        ///     {
        ///  "supplierId": "5542A8DE-2E1C-4C44-B990-795AB05E3685",
        ///  "materialTypeId": "FB2F4D82-F30E-4963-967A-156F763DEB94",
        ///  "materialSectionId": "EB96AE7B-7D96-4AFF-850C-16D618E37EE4",
        ///  "name": "Ngói xi măng màu xanh",
        ///  "price": 16000,
        ///  "unit": "viên",
        ///  "size": "32cm x 32cm",
        ///  "shape": "Hình vuông",
        ///  "imgUrl": null,
        ///  "description": "Ngói xi măng phủ sơn màu xanh, độ bền cao, chống thấm tốt.",
        ///  "isAvailable": true,
        ///  "unitPrice": "vnđ"
        ///     }
        /// </remarks>
        /// <param name="request">Labor request model</param>
        /// <returns>Returns true if the labor is created successfully, otherwise false.</returns>
        /// <response code="200">Labor created successfully</response>
        /// <response code="400">Failed to create the labor</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Labor.LaborEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLabor([FromBody] LaborRequest request)
        {
            var isCreated = await _laborService.CreateLabor(request);
            return isCreated ? Ok(isCreated) : BadRequest();
        }


    }
}
