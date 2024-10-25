using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        #region GetListPayment
        /// <summary>
        /// Retrieves the list of all payment.
        /// </summary>
        /// <returns>List of payment in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Payment.PaymentEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListPayment(int page, int size)
        {
            var listPayment = await _paymentService.GetListPayment(page, size);
            var result = JsonConvert.SerializeObject(listPayment, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailPayment
        /// <summary>
        /// Retrieves the list of all payment.
        /// 
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// </summary>
        /// <returns>List of payment in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Payment.PaymnetBatchEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailPayment(Guid projectId)
        {
            var listBatch = await _paymentService.GetDetailPayment(projectId);
            var result = JsonConvert.SerializeObject(listBatch, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ApproveContractDesin
        /// <summary>
        /// Manager approve payment contract design "Approved"
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
        [HttpPost(ApiEndPointConstant.Payment.PaymentBatchDesignConfirmEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveContractDesin(Guid paymentId, List<IFormFile> files)
        {
            var result = await _paymentService.ApproveContractDesign(paymentId, files);
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
        [HttpPost(ApiEndPointConstant.Payment.PaymentBatchConstructionConfirmEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveContractContruction(Guid paymentId, List<IFormFile> files)
        {
            var result = await _paymentService.ApproveContractContruction(paymentId, files);
            return Ok(result);
        }
    }
}
