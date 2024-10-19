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
                                                               p.UpsDate, p.TotalPrice, p.PaymentDate, p.PaymentPhase,
                                                               p.Unit, p.Percents, p.Description),
                            include: p => p.Include(p => p.PaymentType),
                            page: page,
                            size: size
                            );
            return listPaymet;
        }

        public async Task<List<PaymentResponse>> GetDetailPayment(Guid projectId)
        {
            var batchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                predicate: p => p.Contract!.Project.Id == projectId,
                include: p => p.Include(p => p.Payment!)
                                .ThenInclude(p => p.PaymentType)
                                .Include(p => p.Contract!)
                                .ThenInclude(p => p.Project!)
            );

            var paymentResponses = batchInfo.Select(bp => new PaymentResponse(
                bp.Payment.Id,                          
                bp.Payment.PaymentType.Name,                         
                bp.InsDate,
                bp.Payment.UpsDate,
                bp.Payment.TotalPrice,
                bp.Payment.PaymentDate,
                bp.Payment.PaymentPhase,
                bp.Payment.Unit,                                 
                bp.Payment.Percents,                             
                bp.Payment.Description                            
            )).ToList();

            return paymentResponses;
        }



        //public async Task<BatchResponse> GetPaymentForCustomer(Guid projectId)
        //{

        //}
    }
}
