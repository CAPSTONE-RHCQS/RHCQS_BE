using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Contract;
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
using static RHCQS_BusinessObjects.AppConstant;
using Payment = RHCQS_DataAccessObjects.Models.Payment;

namespace RHCQS_Services.Implement
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IContractService> _logger;
        private readonly Cloudinary _cloudinary;

        public ContractService(IUnitOfWork unitOfWork, ILogger<IContractService> logger, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
        }

        public async Task<IPaginate<ContractResponse>> GetListContract(int page, int size)
        {
            var listContract = await _unitOfWork.GetRepository<Contract>().GetList(
                selector: c => new ContractResponse(c.ProjectId, c.Name, c.CustomerName, c.ContractCode, c.StartDate, c.EndDate,
                                                    c.ValidityPeriod, c.TaxCode, c.Area, c.UnitPrice, c.ContractValue, c.UrlFile,
                                                    c.Note, c.Deflag, c.RoughPackagePrice, c.FinishedPackagePrice, c.Status, c.Type),
                include: c => c.Include(c => c.Project)
                );
            return listContract;
        }

        public async Task<ContractResponse> GetDetailContract(Guid contractId)
        {
            var contractItem = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                predicate: c => c.Id == contractId,
                                include: c => c.Include(c => c.Project)
                );

            if (contractItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
            }

            return new ContractResponse(contractItem.ProjectId, contractItem.Name, contractItem.CustomerName, contractItem.ContractCode, contractItem.StartDate, contractItem.EndDate,
                                        contractItem.ValidityPeriod, contractItem.TaxCode, contractItem.Area, contractItem.UnitPrice,contractItem.ContractValue, 
                                        contractItem.UrlFile, contractItem.Note, contractItem.Deflag, contractItem.RoughPackagePrice, 
                                        contractItem.FinishedPackagePrice, contractItem.Status, contractItem.Type);
        }

        public async Task<ContractResponse> GetDetailContractByType(string type)
        {
            var contractItem = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                predicate: c => c.Type == type,
                                include: c => c.Include(c => c.Project)
                );

            if (contractItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
            }

            return new ContractResponse(contractItem.ProjectId, contractItem.Name, contractItem.CustomerName, contractItem.ContractCode, contractItem.StartDate, contractItem.EndDate,
                                        contractItem.ValidityPeriod, contractItem.TaxCode, contractItem.Area, contractItem.UnitPrice, contractItem.ContractValue,
                                        contractItem.UrlFile, contractItem.Note, contractItem.Deflag, contractItem.RoughPackagePrice,
                                        contractItem.FinishedPackagePrice, contractItem.Status, contractItem.Type);
        }

        //Create design contract -  Create batch payment design drawing
        public async Task<bool> CreateContractDeisgn(ContractDesignRequest request)
        {
            try
            {
                var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                    predicate: x => x.Id == request.ProjectId,
                                    include: x => x.Include(x => x.InitialQuotations)
                                                    .ThenInclude(x => x.PackageQuotations)
                                                    .ThenInclude(x => x.Package)
                                                    .ThenInclude(x => x.PackageType)
                                                    .Include(x => x.Customer!));

                if (infoProject == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
                }

                bool isInitialFinalized = infoProject.InitialQuotations.Any(x => x.Status == AppConstant.ProjectStatus.FINALIZED);

                if (isInitialFinalized)
                {
                    var packageInfo = infoProject.InitialQuotations
                        .SelectMany(x => x.PackageQuotations)
                        .Where(pq => pq.Package.PackageType.Name == AppConstant.Type.ROUGH
                                  || pq.Package.PackageType.Name == AppConstant.Type.FINISHED)
                        .Select(pq => new
                        {
                            TypeName = pq.Package.PackageType.Name,
                            Price = pq.Package.Price
                        })
                        .ToList();

                    if (Enum.TryParse<ContractType>(request.Type, out var contractType))
                    {
                        // Tạo hợp đồng
                        var contractDrawing = new Contract
                        {
                            Id = Guid.NewGuid(),
                            ProjectId = infoProject.Id,
                            Name = EnumExtensions.GetEnumDescription(contractType),
                            CustomerName = infoProject.Customer!.Username,
                            ContractCode = GenerateRandom.GenerateRandomString(10),
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            ValidityPeriod = request.ValidityPeriod,
                            TaxCode = null,
                            Area = infoProject.Area,
                            UnitPrice = AppConstant.Unit.UnitPrice,
                            ContractValue = request.ContractValue,
                            UrlFile = request.UrlFile,
                            Note = null,
                            Deflag = true,
                            RoughPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.ROUGH)?.Price,
                            FinishedPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.FINISHED)?.Price,
                            Status = AppConstant.ConstractStatus.PROCESSING,
                            Type = request.Type,
                        };

                        await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);

                        var initialInfo = infoProject.InitialQuotations.FirstOrDefault(x => x.Status == AppConstant.ProjectStatus.FINALIZED);

                        // Tạo batch payment
                        foreach (var pay in request.BatchPaymentRequests!)
                        {
                            var batchPay = new BatchPayment
                            {
                                Id = Guid.NewGuid(),
                                ContractId = contractDrawing.Id,
                                IntitialQuotationId = initialInfo!.Id,
                                InsDate = DateTime.Now,
                                FinalQuotationId = null
                            };

                            await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(batchPay);

                            // Lấy PaymentType từ bảng PaymentType
                            var paymentType = await _unitOfWork.GetRepository<PaymentType>()
                                        .FirstOrDefaultAsync(pt => pt.Name == EnumExtensions.GetEnumDescription(contractType));

                            var payInfo = new Payment
                            {
                                Id = Guid.NewGuid(),
                                PaymentTypeId = paymentType.Id,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                TotalPrice = request.ContractValue
                            };

                            await _unitOfWork.GetRepository<Payment>().InsertAsync(payInfo);
                        }

                        bool isSuccessful = _unitOfWork.Commit() > 0;
                        return isSuccessful;
                    }
                    else
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, "Invalid contract type.");
                    }
                }
                else
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, "Initial quotation is not finalized.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateContractDeisgn: {ex.Message}", ex);
            }
        }

        //Manager approve bill payment in contract design
        //Bill hóa đơn 
        public async Task<string> ApproveContractDesin(Guid paymentId, List<IFormFile> bills)
        {
            foreach (var file in bills)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var publicId = "Hoa_don_thiet_ke_" + $"{paymentId}" ?? Path.GetFileNameWithoutExtension(file.FileName);

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

                //Save bill payment in Media table
                var mediaInfo = new Medium
                {
                    Id = Guid.NewGuid(),
                    HouseDesignVersionId = null,
                    Name = AppConstant.General.Bill,
                    Url = uploadResult.Url.ToString(),
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    SubTemplateId = null,
                    PaymentId = paymentId
                };

                await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);
            }

            //Update status payyment
            var paymentInfo = await _unitOfWork.GetRepository<Payment>().FirstOrDefaultAsync(x => x.Id == paymentId);
            //paymentInfo.Status = AppConstant.PaymentStatus.PAID;

            _unitOfWork.GetRepository<Payment>().UpdateAsync(paymentInfo);
            string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return result;
        }
    }
}
