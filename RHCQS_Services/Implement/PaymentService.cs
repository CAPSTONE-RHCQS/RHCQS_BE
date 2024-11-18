using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
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
        private readonly Cloudinary _cloudinary;

        public PaymentService(IUnitOfWork unitOfWork, ILogger<IPaymentService> logger, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
        }

        public async Task<IPaginate<PaymentResponse>> GetListPayment(int page, int size)
        {
            var listPaymet = await _unitOfWork.GetRepository<Payment>().GetList(
                            selector: p => new PaymentResponse(p.Priority, p.Id, p.PaymentType.Name!, p.BatchPayments.FirstOrDefault(b => b.Id == p.Id)!.Status!, p.InsDate,
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
                bp.Payment!.Priority,
                bp.Payment!.Id,
                bp.Payment.PaymentType.Name!,
                bp.Status!,
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

        public async Task<List<PaymentResponse>> GetListBatchResponse(Guid projectId)
        {
            var listBatch = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                            predicate: x => x.Contract!.ProjectId == projectId,
                            include: x => x.Include(x => x.Payment!)
                                           .ThenInclude(x => x.PaymentType)
                                           .Include(x => x.Contract!),
                            orderBy: x => x.OrderBy(x => x.Payment.Priority)
                            );

            if (listBatch == null || !listBatch.Any())
            {
                return new List<PaymentResponse>();
            }

            var payments = listBatch.Select(batch => batch.Payment).Distinct();
            var result = payments.Select(payment => new PaymentResponse(
                priorty: payment.Priority,
                id: payment!.Id,
                type: payment.PaymentType.Name!,
                status: payment.BatchPayments.FirstOrDefault(batch => batch.Contract!.ProjectId == projectId)?.Status ?? "",
                insDate: payment.InsDate,
                upsDate: payment.UpsDate,
                totalprice: payment.TotalPrice,
                paymentDate: payment.PaymentDate,
                paymentPhase: payment.PaymentPhase,
                unit: payment.Unit,
                percents: payment.Percents,
                description: payment.Description
            )).ToList();


            return result;
        }

        public async Task<string> ConfirmBatchPaymentFromCustomer(Guid paymentId)
        {

            var paymentInfo = await _unitOfWork.GetRepository<Payment>()
                                     .FirstOrDefaultAsync(x => x.Id == paymentId,
                                     include: x => x.Include(x => x.BatchPayments));
            if (paymentInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                         AppConstant.ErrMessage.Invalid_Payment);
            }

            paymentInfo.IsConfirm = true;

            //var contractIds = paymentInfo.BatchPayments
            //                    .Select(b => b.ContractId)
            //                    .Distinct()
            //                    .ToList();

            //var listBatch = await _unitOfWork.GetRepository<BatchPayment>()
            //                                 .GetListAsync(predicate: p => contractIds.Contains(p.ContractId));

            //bool allPaid = listBatch.All(b => b.Status == AppConstant.PaymentStatus.PAID);

            //if (allPaid)
            //{
            //    var contracts = await _unitOfWork.GetRepository<Contract>()
            //                                     .FirstOrDefaultAsync(predicate: c => contractIds.Contains(c.Id));

            //    contracts.Status = AppConstant.ContractStatus.FINISHED;
            //    _unitOfWork.GetRepository<Contract>().UpdateAsync(contracts);
            //}

            _unitOfWork.GetRepository<Payment>().UpdateAsync(paymentInfo);

            var saveResutl = await _unitOfWork.CommitAsync() > 0
                ? AppConstant.Message.SUCCESSFUL_SAVE
                : AppConstant.ErrMessage.Fail_Save;

            return saveResutl;
        }

    }
}
