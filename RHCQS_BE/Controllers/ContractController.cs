﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Request.Contract;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        #region CreateContractDesign
        /// <summary>
        /// Creates a contract design for a specified project.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/contract/design
        ///     {
        ///       "projectId": "4fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "type": "Design",                                     "Design": Hợp đồng tư vấn và thiết kế nhà ở, "Construction": Hợp đồng thi công
        ///       "startDate": "2024-10-15T00:00:00Z",                  Time start to proccessing
        ///       "endDate": "2025-10-15T00:00:00Z",                    Time end to proccessing
        ///       "validityPeriod": 12,                                 
        ///       "taxCode": "0123456789",                              Mã số thuế
        ///       "contractValue": 50000000,                            Giá trị hợp đồng
        ///       "urlFile": "https://example.com/contract-file.pdf",
        ///       "note": "This is a contract for project design."
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Contract design request model</param>
        /// <returns>Returns true if the contract design is created successfully, otherwise false.</returns>
        /// <response code="200">Contract design created successfully</response>
        /// <response code="400">Failed to create the contract design due to invalid input</response>
        /// 
        #endregion
        [HttpPost(ApiEndPointConstant.Contract.ContractDesignEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContractDesign(ContractDesignRequest request)
        {
            var isCreate = await _contractService.CreateContractDeisgn(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

    }
}
