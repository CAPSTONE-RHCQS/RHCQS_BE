using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObject.Payload.Response.PaymentResponse;

namespace RHCQS_Services.Implement
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPaymentService> _logger;

        public PaymentService(IUnitOfWork unitOfWork, ILogger<IPaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<PaymentResponse>> GetListPayment(int page, int size)
        {
            var listPaymet = await _unitOfWork.GetRepository<Payment>().GetList(
                            selector: p => new PaymentResponse(p.Id, p.PaymentType!.Name, p.InsDate, 
                                                               p.UpsDate, p.TotalPrice),
                            include: p => p.Include(p => p.PaymentType),
                            page: page,
                            size: size
                            );
            return listPaymet;
        }

        //public async Task<PaymentInfoResponse> GetDetailPayment(Guid paymentId)
        //{
        //    var paymentItem = await _unitOfWork.GetRepository<Payment>().GetListAsync(
        //                       predicate: p => p.Id == paymentId,
        //                       include: p => p.Include(p => p.BatchPayment)
        //                                       .Include(p => p.PaymentType)
        //         );

        //    if (paymentItem == null || !paymentItem.Any())
        //    {
        //        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Payment);
        //    }

        //    var payment = paymentItem.FirstOrDefault();

        //    // Create PaymentResponse object
        //    var paymentResponse = new PaymentResponse(
        //        payment.Id,
        //        payment.PaymentType?.Name, // Assuming PaymentType has a Name property
        //        payment.Status,
        //        payment.InsDate,
        //        payment.UpsDate,
        //        payment.TotalPrice
        //    );

        //    // Assuming payment.BatchPayment is a collection, mapping multiple BatchPayment items to BatchResponse
        //    var batchResponses = payment.BatchPayment.Select(batchPayment => new BatchResponse(
        //        paymentResponse,
        //        batchPayment.Id,
        //        batchPayment.ContractId,
        //        batchPayment.Price,
        //        batchPayment.PaymentDate,
        //        batchPayment.PaymentPhase,
        //        batchPayment.Percents,
        //        batchPayment.InsDate,
        //        batchPayment.Description,
        //        batchPayment.Unit
        //    )).ToList();

        //    // Returning the PaymentInfoResponse
        //    return new PaymentInfoResponse(paymentResponse, batchResponses);
        //}


        //public async Task<BatchResponse> GetPaymentForCustomer(Guid projectId)
        //{

        //}
    }
}
