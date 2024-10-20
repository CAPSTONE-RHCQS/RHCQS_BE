using Aspose.Words;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Request.Contract;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Interface;
using Xceed.Words.NET;

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

        #region GetListContract
        /// <summary>
        /// Retrieves the list of all contract item.
        /// 
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// </summary>
        /// <returns>List of contract in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Contract.ContractEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListContract(int page, int size)
        {
            var listContract = await _contractService.GetListContract(page, size);
            var result = JsonConvert.SerializeObject(listContract, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailContract
        /// <summary>
        /// Retrieves the list of all contract item.
        /// 
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// </summary>
        /// <returns>List of contract in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Contract.ContractDesignDetailEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailContract(Guid contractId)
        {
            var contractItem = await _contractService.GetDetailContract(contractId);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailContractByType
        /// <summary>
        /// Retrieves the contract details based on the contract type.
        /// 
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// </summary>
        /// <param name="type">The type of contract. Valid values: 'Design' for design contracts, 'Construction' for construction contracts.</param>
        /// <returns>A contract item matching the specified contract type.</returns>
        /// <response code="200">Returns the details of the contract for the specified type.</response>
        /// <response code="400">If the provided type is invalid or not found.</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Contract.ContractDesignTypeEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailContractByType(string type)
        {
            var contractItem = await _contractService.GetDetailContractByType(type);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateContractDesign
        /// <summary>
        /// Creates a contract design for a specified project.
        /// 
        /// Role: MANAGER
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
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ContractDesignEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContractDesign(ContractDesignRequest request)
        {
            var isCreate = await _contractService.CreateContractDesign(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region CreateContractConstruction
        /// <summary>
        /// Creates a contract construction for a specified project.
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/contract/construction
        ///     {
        ///       "projectId": "fa91e187-b0c8-46c3-b6d5-747d32bd3540",
        ///       "startDate": "2024-10-30T16:13:16.868Z",               // Time start for processing               Required
        ///       "endDate": "2025-04-20T16:13:16.868Z",                 // Time end for processing                 Required
        ///       "validityPeriod": 300,                                  // Validity period in days                Required
        ///       "taxCode": null,                                       // Tax code (nullable)
        ///       "contractValue": 1123500000,                           // Contract value                          Required
        ///       "urlFile": "https://res.cloudinary.com/de7pulfdj/raw/upload/v1729439219/ppvcvg4h9wlzpsj0pu8b.docx", Required
        ///       "note": null                                           // Additional notes (nullable)
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Contract construction request model</param>
        /// <returns>Returns true if the contract construction is created successfully, otherwise false.</returns>
        /// <response code="200">Contract construction created successfully</response>
        /// <response code="400">Failed to create the contract construction due to invalid input</response>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ContractConstructionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContractConstruction(ContractConstructionRequest request)
        {
            var isCreate = await _contractService.CreateContractConstruction(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region ApproveContractDesin
        /// <summary>
        /// Manager approve payment contract design 
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request">Approve payment contract design</param>
        /// <returns>Returns true if the payment contract is created successfully, otherwise false.</returns>
        /// <response code="200">Media created - Payment chanage status "Paid" successfully</response>
        /// <response code="400">Failed to approve contract due to invalid input</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ContractDesignApproveEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveContractDesin(Guid paymentId, List<IFormFile> files)
        {
            var result = await _contractService.ApproveContractDesign(paymentId, files);
            return Ok(result);
        }

        #region ApproveContractContruction
        /// <summary>
        /// Manager approve payment contract design 
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request">Approve payment contract design</param>
        /// <returns>Returns true if the payment contract is created successfully, otherwise false.</returns>
        /// <response code="200">Media created - Payment chanage status "Paid" successfully</response>
        /// <response code="400">Failed to approve contract due to invalid input</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ContractConstructionApproveEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveContractContruction(Guid paymentId, List<IFormFile> files)
        {
            var result = await _contractService.ApproveContractContruction(paymentId, files);
            return Ok(result);
        }
    }
}
