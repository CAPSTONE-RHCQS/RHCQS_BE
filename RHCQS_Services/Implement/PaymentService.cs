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
using RHCQS_BusinessObject.Helper;

namespace RHCQS_Services.Implement
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IPaymentService> _logger;
        private readonly Cloudinary _cloudinary;
        private readonly IMediaService _mediaService;

        public PaymentService(IUnitOfWork unitOfWork, 
            ILogger<IPaymentService> logger, 
            Cloudinary cloudinary,
            IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
            _mediaService = mediaService;
        }

        public async Task<IPaginate<PaymentResponse>> GetListPayment(int page, int size)
        {
            var listPaymet = await _unitOfWork.GetRepository<Payment>().GetList(
                            selector: p => new PaymentResponse(p.Priority, p.Id, p.PaymentType.Name!, p.BatchPayments.FirstOrDefault(b => b.Id == p.Id)!.Status!, p.InsDate,
                                                               p.UpsDate, p.TotalPrice, p.PaymentDate, p.PaymentPhase,
                                                               p.Unit, (int)p.Percents, p.Description, p.IsConfirm),
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
                bp.Payment.Percents ?? 0,
                bp.Payment.Description,
                bp.Payment.IsConfirm
            )).ToList();

            return paymentResponses;
        }

        public async Task<List<PaymentResponse>> GetListBatchResponse(Guid projectId)
        {
            var allBatches = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                predicate: x => x.Contract!.ProjectId == projectId, 
                //&&
                //                (x.Contract.Type == AppConstant.ContractType.Construction.ToString() ||
                //                 x.Contract.Type == AppConstant.ContractType.Appendix.ToString()),
                include: x => x.Include(x => x.Payment!)
                               .ThenInclude(x => x.PaymentType)
                               .Include(x => x.Contract!),
                orderBy: x => x.OrderBy(x => x.Payment.Priority)
            );

            if (allBatches == null || !allBatches.Any())
            {
                return new List<PaymentResponse>();
            }

            var result = allBatches.Select(batch => new PaymentResponse(
                priorty: batch.Payment?.Priority,
                id: batch.Payment!.Id,
                type: batch.Payment.PaymentType.Name!,
                status: batch.Status ?? "",
                insDate: batch.Payment.InsDate,
                upsDate: batch.Payment.UpsDate,
                totalprice: batch.Payment.TotalPrice,
                paymentDate: batch.Payment.PaymentDate,
                paymentPhase: batch.Payment.PaymentPhase,
                unit: batch.Payment.Unit,
                percents: batch.Payment.Percents ?? 0,
                description: batch.Payment.Description,
                isConfirm: batch.Payment.IsConfirm
            )).ToList();

            return result;
        }


        public async Task<string> ConfirmBatchPaymentFromCustomer(Guid paymentId, IFormFile TransferInvoice)
        {

            var paymentInfo = await _unitOfWork.GetRepository<Payment>()
                                     .FirstOrDefaultAsync(x => x.Id == paymentId,
                                     include: x => x.Include(x => x.BatchPayments));
            if (paymentInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                         AppConstant.ErrMessage.Invalid_Payment);
            }
            var result = await _mediaService.UploadImageAsync(TransferInvoice, "Invoice", null);

            var invoiceMedia = new Medium
            {
                Id = Guid.NewGuid(),
                Name = "Hoa_don_thanh_toan_" + paymentId,
                Url = result.ToString(),
                InsDate = LocalDateTime.VNDateTime(),
                UpsDate = LocalDateTime.VNDateTime(),
                PaymentId = paymentId
            };
            await _unitOfWork.GetRepository<Medium>().InsertAsync(invoiceMedia);

            paymentInfo.IsConfirm = true;

            _unitOfWork.GetRepository<Payment>().UpdateAsync(paymentInfo);

            var saveResutl = await _unitOfWork.CommitAsync() > 0
                ? AppConstant.Message.SUCCESSFUL_SAVE
                : AppConstant.ErrMessage.Fail_Save;

            return saveResutl;
        }

        public async Task<string> GetBillImage(Guid paymentId)
        {
            var paymentInfo = await _unitOfWork.GetRepository<Payment>().FirstOrDefaultAsync(
                            predicate: p => p.Id == paymentId,
                            include: p => p.Include(p => p.Media));

            if (paymentInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Invalid_Payment);
            }

            if (paymentInfo.Media.Count == 0)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Not_Found_Bill);
            }

            string urlImage = paymentInfo.Media.First().Url!;
            return urlImage;
        }

    }
}
