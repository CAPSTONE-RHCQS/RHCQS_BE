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

        //Manager confirm bill payment in contract design
        //Bill hóa đơn 
        public async Task<string> ApproveContractDesign(Guid contractId, List<IFormFile> bills)
        {
            //Check list batch payment 
            var payBatchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                predicate: c => c.ContractId == contractId);
            if (payBatchInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
            }
            int imageCount = Math.Min(bills.Count, payBatchInfo.Count());

            for (int i = 0; i < imageCount; i++)
            {
                var file = bills[i];

                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var publicId = $"Hoa_don_thiet_ke_{contractId}_{i}";

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = publicId,
                    Folder = "Contract",
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                }

                // Tạo mới Media cho mỗi hình ảnh và gán PaymentId tương ứng
                var mediaInfo = new Medium
                {
                    Id = Guid.NewGuid(),
                    HouseDesignVersionId = null,
                    Name = AppConstant.General.Bill,
                    Url = uploadResult.Url.ToString(),
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    SubTemplateId = null,
                    PaymentId = payBatchInfo.ElementAt(i).PaymentId
                };

                await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);

                payBatchInfo.ElementAt(i).Status = AppConstant.PaymentStatus.PAID;
                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(payBatchInfo);
            }
            string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return result;
        }

        //Manager update bill 
        public async Task<string> ApproveContractContruction(Guid contractId, List<IFormFile> bills)
        {
            try
            {
                var payBatchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                       predicate: c => c.ContractId == contractId);
                if (payBatchInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
                }

                int imageCount = Math.Min(bills.Count, payBatchInfo.Count);

                var payBatchList = payBatchInfo.ToList();
                for (int i = 0; i < imageCount; i++)
                {
                    var file = bills[i];

                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    var publicId = $"Hoa_don_thi_cong_{contractId}_{i}";

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        PublicId = publicId,
                        Folder = "Contract",
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                    }

                    // Tạo mới Media cho mỗi hình ảnh và gán PaymentId tương ứng
                    var mediaInfo = new Medium
                    {
                        Id = Guid.NewGuid(),
                        HouseDesignVersionId = null,
                        Name = AppConstant.General.Bill,
                        Url = uploadResult.Url.ToString(),
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        SubTemplateId = null,
                        PaymentId = payBatchList[i].PaymentId
                    };

                    await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);

                }

                //Update batch payment status 
                payBatchInfo.ToList().ForEach(pay => pay.Status = AppConstant.PaymentStatus.PAID);
                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(payBatchInfo);

                string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
                return result;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }

        }
    }
}
