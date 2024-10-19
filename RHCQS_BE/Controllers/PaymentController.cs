﻿using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Customer, SalesStaff, Manager")]
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

        //#region GetDetailPayment
        ///// <summary>
        ///// Retrieves the list of all payment.
        ///// </summary>
        ///// <returns>List of payment in the system</returns>
        //#endregion
        ////[Authorize(Roles = "Customer, SalesStaff, Manager")]
        //[HttpGet(ApiEndPointConstant.Payment.PaymnetBatchEndpoint)]
        //[ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetDetailPayment(Guid paymentId)
        //{
        //    var listBatch = await _paymentService.GetDetailPayment(paymentId);
        //    var result = JsonConvert.SerializeObject(listBatch, Formatting.Indented);
        //    return new ContentResult()
        //    {
        //        Content = result,
        //        StatusCode = StatusCodes.Status200OK,
        //        ContentType = "application/json"
        //    };
        //}
    }
}
