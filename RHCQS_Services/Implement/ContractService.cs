using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Contract;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.App;
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
                                        contractItem.ValidityPeriod, contractItem.TaxCode, contractItem.Area, contractItem.UnitPrice, contractItem.ContractValue,
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

        //Create design -  Create batch payment design drawing
        public async Task<bool> CreateContractDesign(ContractDesignRequest request)
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

                    // Tạo payment thiết kế
                    foreach (var pay in request.BatchPaymentRequests!)
                    {
                        // Lấy PaymentType từ bảng PaymentType
                        var paymentType = await _unitOfWork.GetRepository<PaymentType>()
                                    .FirstOrDefaultAsync(pt => pt.Name == EnumExtensions.GetEnumDescription(contractType));

                        var payInfo = new Payment
                        {
                            Id = Guid.NewGuid(),
                            PaymentTypeId = paymentType.Id,
                            InsDate = DateTime.Now,
                            UpsDate = DateTime.Now,
                            TotalPrice = request.ContractValue,
                            PaymentDate = DateTime.Now,
                            PaymentPhase = DateTime.Now,
                            Unit = AppConstant.Unit.UnitPrice,
                            Percents = pay.Percents,
                            Description = pay.Description,
                        };

                        await _unitOfWork.GetRepository<Payment>().InsertAsync(payInfo);

                        var batchPay = new BatchPayment
                        {
                            Id = Guid.NewGuid(),
                            ContractId = contractDrawing.Id,
                            IntitialQuotationId = initialInfo!.Id,
                            InsDate = DateTime.Now,
                            FinalQuotationId = null,
                            PaymentId = payInfo.Id,
                            Status = AppConstant.PaymentStatus.PROGRESS
                        };

                        await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(batchPay);
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

        public async Task<bool> CreateContractConstruction(ContractConstructionRequest request)
        {

            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                predicate: x => x.Id == request.ProjectId,
                                include: x => x.Include(x => x.InitialQuotations)
                                                .ThenInclude(x => x.PackageQuotations)
                                                .ThenInclude(x => x.Package)
                                                .ThenInclude(x => x.PackageType)
                                                .Include(x => x.FinalQuotations)
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


                // Tạo hợp đồng
                var contractDrawing = new Contract
                {
                    Id = Guid.NewGuid(),
                    ProjectId = infoProject.Id,
                    Name = EnumExtensions.GetEnumDescription(AppConstant.ContractType.Construction),
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
                    Type = AppConstant.ContractType.Construction.ToString(),
                };

                await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);

                var finalInfo = infoProject.FinalQuotations.FirstOrDefault(x => x.Status == AppConstant.ProjectStatus.FINALIZED);

                if (finalInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Finalized_Final_Quotation);
                }
                // Tìm batch payment - payment đã tạo -> Update batch paymet - contractId
                var listPayment = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                predicate: p => p.FinalQuotationId == finalInfo.Id,
                                include: p => p.Include(p => p.Payment!));

                if (listPayment == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Payment);
                }

                foreach (var pay in listPayment)
                {
                    pay.ContractId = contractDrawing.Id;
                    _unitOfWork.GetRepository<BatchPayment>().UpdateAsync(pay);
                }

                bool isSuccessful = _unitOfWork.Commit() > 0;
                return isSuccessful;
            }
            else
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, "Initial quotation is not finalized.");
            }
        }

        //Manager approve bill payment in contract design
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
            }
            string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return result;
        }

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

        public async Task<ContractAppResponse> GetListContractApp(Guid projectId, string type)
        {
            string typeQuery = null;
            if (type == AppConstant.ContractType.Design.ToString())
            {
                typeQuery = AppConstant.ContractType.Design.ToString();
            }
            else
            {
                typeQuery = AppConstant.ContractType.Construction.ToString();
            }
            var contractInfo = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(x => x.ProjectId == projectId 
            && type == typeQuery);

            var resutl = new ContractAppResponse(contractInfo.Id, contractInfo.UrlFile);

            return resutl;
        }
    }
}
