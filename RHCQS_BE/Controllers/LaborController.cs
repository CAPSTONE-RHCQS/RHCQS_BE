﻿using DocumentFormat.OpenXml.Drawing;
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
        ///    {
        ///    "name": "Thợ điện",
        ///    "price": 150000,
        ///    "deflag": true,
        ///    "type": "Finished"
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

        #region ImportLaborFromExcel
        /// <summary>
        /// Imports labor data from an Excel file.
        /// </summary>
        /// <remarks>
        /// Upload an Excel file with the following format:
        /// Column 1: Name (string)  
        /// Column 2: Price (double)  
        /// Column 3: Deflag (bool)  
        /// Column 4: Type (string)  
        /// Column 5: Code (string)
        /// </remarks>
        /// <param name="file">Excel file containing labor data</param>
        /// <returns>Returns true if the data is imported successfully, otherwise false</returns>
        /// <response code="200">Data imported successfully</response>
        /// <response code="400">Failed to import data</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Labor.ImportExcel)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportLaborFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var isImported = await _laborService.ImportLaborFromExcel(file);
            return isImported ? Ok(true) : BadRequest("Failed to import data");
        }

        #region UpdateLabor
        /// <summary>
        /// Updates an existing labor.
        /// Requires Manager role for authorization.
        /// </summary>
        /// <param name="id">The unique identifier of labor to be updated.</param>
        /// <param name="request">The request body containing the updated labor details.</param>
        /// <returns>A boolean value indicating the success or failure of the update operation.</returns>
        /// <response code="200">Returns true if the update is successful.</response>
        /// <response code="400">Returns BadRequest if the update fails or if validation issues occur.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Labor.LaborEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLabor(Guid id, [FromBody] LaborRequest request)
        {
            var isUpdated = await _laborService.UpdateLabor(id, request);
            return isUpdated ? Ok(isUpdated) : BadRequest();
        }

        #region SearchLaborByName
        /// <summary>
        /// Searches labors by name.
        /// </summary>
        /// <param name="name">The name or partial name of the labor.</param>
        /// <param name="packageId"></param>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Labor.SearchLaborEndpoint)]
        [ProducesResponseType(typeof(List<LaborResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchLaborByName(Guid packageId, string name)
        {
            var listSearchLabor = await _laborService.SearchLaborByName(packageId, name);
            var result = JsonConvert.SerializeObject(listSearchLabor, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region SearchLaborByNameWithoutPackage
        /// <summary>
        /// Searches labors by name.
        /// </summary>
        /// <param name="name">The name or partial name of the labor.</param>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Labor.SearchLaborWithoutPackageEndpoint)]
        [ProducesResponseType(typeof(List<LaborResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchLaborByNameWithoutPackage(string name)
        {
            var listSearchLabor = await _laborService.SearchLaborByNameWithoutPackage(name);
            var result = JsonConvert.SerializeObject(listSearchLabor, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region SearchLaborByNameWithPag
        /// <summary>
        /// Searches labors by name (pag).
        /// </summary>
        /// <param name="name">The name or partial name of the labor.</param>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Labor.SearchLaborWithPagEndpoint)]
        [ProducesResponseType(typeof(List<LaborResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchLaborByNameWithPag(string? name, int page, int size)
        {
            var listSearchLabor = await _laborService.SearchLaborByNameWithPag(name, page, size);
            var result = JsonConvert.SerializeObject(listSearchLabor, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
