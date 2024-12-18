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
        [HttpGet(ApiEndPointConstant.Payment.PaymentBatchEndpoint)]
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

        #region GetListBatchResponse
        /// <summary>
        /// List batch payment for customer - App
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <returns>List of payment in the system</returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.Payment.PaymentBatchForCustomerEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListBatchResponse(Guid projectId)
        {
            var listPayment = await _paymentService.GetListBatchResponse(projectId);
            var result = JsonConvert.SerializeObject(listPayment, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ConfirmBatchPaymentFromCustomer
        /// <summary>
        /// Confirm payment for Customer - App
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <returns>Message</returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.Payment.PaymentConfirmEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmBatchPaymentFromCustomer(Guid paymentId, IFormFile TransferInvoice)
        {
            var listPayment = await _paymentService.ConfirmBatchPaymentFromCustomer(paymentId, TransferInvoice);
            var result = JsonConvert.SerializeObject(listPayment, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetBillImage
        /// <summary>
        /// Get bill url for app 
        /// 
        /// Role: CUSTOMER
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.Payment.PaymentBillUrlEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillImage(Guid paymentId)
        {
            var listPayment = await _paymentService.GetBillImage(paymentId);
            var result = JsonConvert.SerializeObject(listPayment, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
