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
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using System.ComponentModel.DataAnnotations;
using Xceed.Words.NET;
using static RHCQS_BusinessObjects.AppConstant;

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
        /// Role: SALES STAFF
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
        ///       "note": "This is a contract for project design."
        ///     }
        /// 
        /// FORM SAMPLE FILE THIET KE: https://res.cloudinary.com/de7pulfdj/raw/upload/v1729870312/iqnzobl2div6ddqo6tdj.docx
        /// </remarks>
        /// <param name="request">Contract design request model</param>
        /// <returns>Returns true if the contract design is created successfully, otherwise false.</returns>
        /// <response code="200">Contract design created successfully</response>
        /// <response code="400">Failed to create the contract design due to invalid input</response>
        /// 
        #endregion
        [Authorize(Roles = "SalesStaff")]
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
        /// Role: SALES STAFF
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
        ///       "note": null                                           // Additional notes (nullable)
        ///     }
        /// 
        /// FORM SAMPLE FILE THI CONG: https://res.cloudinary.com/de7pulfdj/raw/upload/v1729870312/fdoh3lrrg1sjyijtv3lj.docx
        /// </remarks>
        /// <param name="request">Contract construction request model</param>
        /// <returns>Returns true if the contract construction is created successfully, otherwise false.</returns>
        /// <response code="200">Contract construction created successfully</response>
        /// <response code="400">Failed to create the contract construction due to invalid input</response>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPost(ApiEndPointConstant.Contract.ContractConstructionEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContractConstruction(ContractConstructionRequest request)
        {
            var isCreate = await _contractService.CreateContractConstruction(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region GetListContractApp
        /// <summary>
        /// Get file contract for App mobile 
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// GET /api/v1/contracts/files
        ///
        ///
        /// Response Example:
        /// ```json
        /// {
        ///   "Id": "d5d29d51-d6e7-4c75-a9ff-dff7b63a828c",
        ///   "File": "http://example.com/contracts/design-file.pdf"
        /// }
        /// ```
        ///
        /// This endpoint allows users with roles 'Customer', 'SalesStaff', or 'Manager' 
        /// to retrieve contract files based on the project ID and contract type. 
        /// The `type` parameter can be either 'Design' or 'Construction'.
        /// </remarks>
        /// <param name="projectId">The unique identifier of the project.</param>
        /// <param name="type">The type of contract to retrieve (Design or Construction).</param>
        /// <returns>Returns the contract file details associated with the specified project and type.</returns>
        /// <response code="200">Contract file details retrieved successfully</response>
        /// <response code="404">Contract not found for the specified project and type</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Contract.ContractFileEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListContractApp(Guid projectId, string type)
        {
            var contractItem = await _contractService.GetListContractApp(projectId, type);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region UploadContractSign
        /// <summary>
        /// Sale Staff upload contract has signed
        /// 
        /// ROLE: SALE STAFF
        /// </summary>
        /// <remarks>
        /// **Response Example:**
        /// ```json
        /// {
        ///   "Id": "d5d29d51-d6e7-4c75-a9ff-dff7b63a828c",
        ///   "UrlFile": "http://example.com/contracts/contract-signature.pdf",
        /// }
        /// ```
        /// 
        /// **Notes:**
        /// - The `contractId` parameter is required to specify the target contract for the file upload.
        /// - Multiple files can be uploaded simultaneously.
        /// - The uploaded files will be stored under the contract folder and are accessible by their generated URLs.
        /// </remarks>
        /// <param name="contractId">The unique identifier of the contract for which files are being uploaded.</param>
        /// <param name="files">A list of files to upload for the specified contract.</param>
        /// <returns>Returns the details of the uploaded contract files, including their URLs.</returns>
        /// <response code="200">Files uploaded successfully and contract details returned</response>
        /// <response code="404">Contract not found for the specified ID</response>
        /// <response code="400">Invalid file format or missing required parameters</response>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPut(ApiEndPointConstant.Contract.ContractConstructionSignCompletedEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadContractSign(Guid contractId, List<IFormFile> files)
        {
            var contractItem = await _contractService.UploadContractSign(contractId, files);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region BillContractContruction
        /// <summary>
        /// Manager upload bill payment contract design
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="paymentId">Approve payment contract design</param>
        /// <param name="files"></param>
        /// <returns>Returns true if the payment contract is created successfully, otherwise false.</returns>
        /// <response code="200">Media created - Payment chanage status "Paid" successfully</response>
        /// <response code="400">Failed to approve contract due to invalid input</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Contract.PaymentBatchDesignConfirmEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BillContractDesign(Guid paymentId, List<IFormFile> files)
        {
            var result = await _contractService.UploadBillContractDesign(paymentId, files);
            return Ok(result);
        }

        #region BillContractContruction
        /// <summary>
        /// Manager upload bill payment contract design 
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="paymentId">Approve payment contract design</param>
        /// <param name="files"></param>
        /// <returns>Returns true if the payment contract is created successfully, otherwise false.</returns>
        /// <response code="200">Media created - Payment chanage status "Paid" successfully</response>
        /// <response code="400">Failed to approve contract due to invalid input</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Contract.PaymentBatchConstructionConfirmEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BillContractConstruction(Guid paymentId, List<IFormFile> files)
        {
            var result = await _contractService.UploadBillContractConstruction(paymentId, files);
            return Ok(result);
        }

        #region UploadContractAppendix
        /// <summary>
        /// Upload bill for contract appendix
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Contract.PaymentBatchAppendixConfirmEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadContractAppendix(Guid paymentId, List<IFormFile> files)
        {
            var contractItem = await _contractService.UploadBillContractAppendix(paymentId, files);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CloneInitialInfoToContract
        /// <summary>
        /// Clone initial quotation to contract design
        /// 
        /// Role: MANAGER - SALES STAFF
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Contract.FinalToContract)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloneInitialInfoToContract(Guid projectId)
        {
            var contractItem = await _contractService.CloneFinalInfoToContract(projectId);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region UploadFileContract
        /// <summary>
        /// Upload file contract (design + construction)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Manager, SalesStaff")]
        [HttpPost(ApiEndPointConstant.Contract.UploadFileContractEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFileContract(IFormFile file)
        {
            var contractItem = await _contractService.UploadFileContract(file);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ManagerApproverBillFromCustomer
        /// <summary>
        /// Manager confirm invoice bill from customer 
        /// If approver -> contract - project is finalized
        /// Else display upload bill from Manager
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="type">Approved</param>
        /// <returns></returns>
        #endregion
        //[Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ManagerApproveBillFromCustomerEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ManagerApproverBillFromCustomer(Guid paymentId, 
            [FromQuery][Required(ErrorMessage = "Type là cần thiết.")] string type)
        {
            var contractItem = await _contractService.ManagerApproverBillFromCustomer(paymentId, type);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateContractAppendix
        /// <summary>
        /// Create contract appendix (web)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.Contract.ContractAppendixEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContractAppendix(ContractAppendixRequest request)
        {
            var isCreate = await _contractService.CreateContractAppendix(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region DeleteCustomerBillPayment
        /// <summary>
        /// Customer delete bill when mistake bill
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpDelete(ApiEndPointConstant.Contract.CustomerDeleteBillEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCustomerBillPayment(Guid paymentId)
        {
            var contractItem = await _contractService.DeleteCustomerBillPayment(paymentId);
            var result = JsonConvert.SerializeObject(contractItem, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
