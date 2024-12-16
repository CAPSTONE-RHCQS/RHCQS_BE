using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Contract;
using RHCQS_BusinessObject.Payload.Response.App;
using RHCQS_BusinessObject.Payload.Response.Contract;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
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
        private readonly IMediaService _mediaService;

        public ContractService(IUnitOfWork unitOfWork, ILogger<IContractService> logger,
            Cloudinary cloudinary, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
            _mediaService = mediaService;
        }

        public async Task<IPaginate<IpaginateContractResponse>> GetListContract(int page, int size)
        {
            var listContract = await _unitOfWork.GetRepository<Contract>().GetList(
                selector: c => new IpaginateContractResponse(c.ProjectId, c.Name, c.CustomerName, c.ContractCode, c.StartDate, c.EndDate,
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
                                                .Include(c => c.BatchPayments)
                                                .ThenInclude(c => c.Payment!)
                                                .ThenInclude(c => c.Media));

            if (contractItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
            }

            DependOnQuotation dependOnQuotation = null;
            if (contractItem.Type == AppConstant.ContractType.Design.ToString())
            {
                var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                predicate: i => i.ProjectId == contractItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED,
                                include: i => i.Include(i => i.Media));

                if (initialQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_InitialQuotaion);
                }

                dependOnQuotation = new DependOnQuotation
                {
                    QuotationlId = initialQuotation.Id,
                    Version = initialQuotation.Version,
                    File = initialQuotation.Media?.FirstOrDefault(f => f.InitialQuotationId == initialQuotation.Id)?.Url ?? ErrMessage.InvalidFile
                };
            }
            else if (contractItem.Type == AppConstant.ContractType.Construction.ToString())
            {
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                                predicate: i => i.ProjectId == contractItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED,
                                include: i => i.Include(i => i.Media));

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Finalized_Final_Quotation);
                }

                dependOnQuotation = new DependOnQuotation
                {
                    QuotationlId = finalQuotation.Id,
                    Version = (double)finalQuotation.Version!,
                    File = finalQuotation.Media?.FirstOrDefault(f => f.InitialQuotationId == finalQuotation.Id)?.Url ?? ErrMessage.InvalidFile
                };
            }


            var batchPayments = contractItem.BatchPayments
                .Select(b => new BatchPaymentContract
                {
                    BatchPaymentId = b.Id,
                    PaymentId = b.PaymentId,
                    NumberOfBatch = b.NumberOfBatch,
                    Price = b.Payment.TotalPrice,
                    PaymentDate = b.Payment.PaymentDate,
                    PaymentPhase = b.Payment.PaymentPhase,
                    Percents = b.Payment.Percents ?? 0,
                    Description = b.Payment.Description,
                    Status = b.Status,
                    InvoiceImage = b.Payment.Media
                    .FirstOrDefault(m => m.PaymentId == b.PaymentId)?.Url
                    ?? "Chưa có hóa đơn"
                })
                .OrderBy(b => b.NumberOfBatch)
                .ToList();

            List<BatchPaymentAppendix>? batchPaymentAppendices = null;
            if (contractItem.Type != AppConstant.ContractType.Appendix.ToString())
            {
                var contractAppendix = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                    predicate: p => p.ProjectId == contractItem.ProjectId
                            && p.Type == AppConstant.ContractType.Appendix.ToString(),
                    include: p => p.Include(p => p.BatchPayments)
                                    .ThenInclude(p => p.Payment));
                if (contractAppendix != null)
                {
                    batchPaymentAppendices = contractAppendix.BatchPayments
                    .Select(b => new BatchPaymentAppendix
                    {
                        PaymentId = b.PaymentId,
                        NumberOfBatch = b.NumberOfBatch,
                        Price = b.Payment.TotalPrice,
                        PaymentDate = b.Payment.PaymentDate,
                        PaymentPhase = b.Payment.PaymentPhase,
                        Percents = b.Payment.Percents ?? 0,
                        Description = b.Payment.Description,
                        Status = b.Status,
                        InvoiceImage = b.Payment.Media
                        .FirstOrDefault(m => m.PaymentId == b.PaymentId)?.Url
                        ?? "Chưa có hóa đơn"
                    })
                    .OrderBy(b => b.NumberOfBatch)
                    .ToList();
                }
            }

            return new ContractResponse(contractItem.ProjectId, contractItem.Name, contractItem.Project.CustomerName, contractItem.ContractCode,
                                        contractItem.StartDate, contractItem.EndDate, contractItem.ValidityPeriod, contractItem.TaxCode,
                                        contractItem.Area, contractItem.UnitPrice, contractItem.ContractValue,
                                        contractItem.UrlFile, contractItem.Note, contractItem.Deflag, contractItem.RoughPackagePrice,
                                        contractItem.FinishedPackagePrice, contractItem.Status, contractItem.Type, contractItem.InsDate,
                                        dependOnQuotation, batchPayments, batchPaymentAppendices);
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
            DependOnQuotation dependOnQuotation = null;
            if (contractItem.Type == AppConstant.ContractType.Design.ToString())
            {
                var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                predicate: i => i.ProjectId == contractItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED,
                                include: i => i.Include(i => i.Media));

                if (initialQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Found_InitialQuotaion);
                }

                dependOnQuotation = new DependOnQuotation
                {
                    QuotationlId = initialQuotation.Id,
                    Version = initialQuotation.Version,
                    File = initialQuotation.Media?.FirstOrDefault(f => f.InitialQuotationId == initialQuotation.Id)?.Url ?? ErrMessage.InvalidFile
                };
            }
            else if (contractItem.Type == AppConstant.ContractType.Construction.ToString())
            {
                var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                                predicate: i => i.ProjectId == contractItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED,
                                include: i => i.Include(i => i.Media));

                if (finalQuotation == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Finalized_Final_Quotation);
                }

                dependOnQuotation = new DependOnQuotation
                {
                    QuotationlId = finalQuotation.Id,
                    Version = (double)finalQuotation.Version!,
                    File = finalQuotation.Media?.FirstOrDefault(f => f.InitialQuotationId == finalQuotation.Id)?.Url ?? ErrMessage.InvalidFile
                };
            }

            var batchPayments = contractItem.BatchPayments
             .Select(b => new BatchPaymentContract
             {
                 BatchPaymentId = b.Id,
                 PaymentId = b.PaymentId,
                 NumberOfBatch = b.NumberOfBatch,
                 Price = b.Payment.TotalPrice,
                 PaymentDate = b.Payment.PaymentDate,
                 PaymentPhase = b.Payment.PaymentPhase,
                 Percents = b.Payment.Percents ?? 0,
                 Description = b.Payment.Description,
                 Status = b.Status,
                 InvoiceImage = b.Payment.Media
                    .FirstOrDefault(m => m.PaymentId == b.PaymentId)?.Url
                    ?? "Hình ảnh chuyển khoản chưa có"
             })
             .OrderBy(b => b.NumberOfBatch)
             .ToList();
            List<BatchPaymentAppendix>? batchPaymentAppendices = null;
            var contractAppendix = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                predicate: p => p.ProjectId == contractItem.ProjectId
                        && p.Type == AppConstant.ContractType.Appendix.ToString(),
                include: p => p.Include(p => p.BatchPayments)
                                .ThenInclude(p => p.Payment));
            if (contractAppendix != null)
            {
                batchPaymentAppendices = contractAppendix.BatchPayments
                .Select(b => new BatchPaymentAppendix
                {
                    PaymentId = b.PaymentId,
                    NumberOfBatch = b.NumberOfBatch,
                    Price = b.Payment.TotalPrice,
                    PaymentDate = b.Payment.PaymentDate,
                    PaymentPhase = b.Payment.PaymentPhase,
                    Percents = b.Payment.Percents ?? 0,
                    Description = b.Payment.Description,
                    Status = b.Status,
                    InvoiceImage = b.Payment.Media
                    .FirstOrDefault(m => m.PaymentId == b.PaymentId)?.Url
                    ?? "Chưa có hóa đơn"
                })
                .OrderBy(b => b.NumberOfBatch)
                .ToList();
            }

            return new ContractResponse(contractItem.ProjectId, contractItem.Name, contractItem.CustomerName, contractItem.ContractCode, contractItem.StartDate, contractItem.EndDate,
                                        contractItem.ValidityPeriod, contractItem.TaxCode, contractItem.Area, contractItem.UnitPrice, contractItem.ContractValue,
                                        contractItem.UrlFile, contractItem.Note, contractItem.Deflag, contractItem.RoughPackagePrice,
                                        contractItem.FinishedPackagePrice, contractItem.Status, contractItem.Type, contractItem.InsDate, dependOnQuotation,
                                        batchPayments, batchPaymentAppendices);
        }

        public async Task<bool> CreateContractDesign(ContractDesignRequest request)
        {

            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                predicate: x => x.Id == request.ProjectId,
                                include: x => x.Include(x => x.InitialQuotations)
                                                .ThenInclude(x => x.PackageQuotations)
                                                .ThenInclude(x => x.Package)
                                                .Include(x => x.Customer!));

            if (infoProject == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
            }

            if (infoProject.Contracts.Count(c => c.Type == AppConstant.ContractType.Design.ToString()) > 1)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ContractOver);
            }

            bool isInitialFinalized = infoProject.InitialQuotations.Any(x => x.Status == AppConstant.ProjectStatus.FINALIZED);

            if (isInitialFinalized)
            {
                var packageInfo = infoProject.InitialQuotations
                    .SelectMany(x => x.PackageQuotations)
                    .Where(pq => pq.Package.Type == AppConstant.Type.ROUGH
                              || pq.Package.Type == AppConstant.Type.FINISHED)
                    .Select(pq => new
                    {
                        TypeName = pq.Package.Type,
                        Price = pq.Package.Price
                    })
                    .ToList();

                if (Enum.TryParse<ContractType>(request.Type, out var contractType))
                {
                    // Tạo hợp đồng
                    var avaibleContract = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(x => x.ProjectId == infoProject.Id);
                    Contract contractDrawing = null;
                    if (avaibleContract == null)
                    {
                        contractDrawing = new Contract
                        {
                            Id = Guid.NewGuid(),
                            ProjectId = infoProject.Id,
                            Name = EnumExtensions.GetEnumDescription(contractType),
                            CustomerName = infoProject.Customer!.Username,
                            ContractCode = GenerateRandom.GenerateRandomString(10),
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            ValidityPeriod = request.ValidityPeriod,
                            TaxCode = request.TaxCode ?? null,
                            Area = infoProject.Area,
                            UnitPrice = AppConstant.Unit.UnitPrice,
                            ContractValue = request.ContractValue,
                            UrlFile = request.UrlFile,
                            Note = request.Note ?? null,
                            Deflag = true,
                            RoughPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.ROUGH)?.Price,
                            FinishedPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.FINISHED)?.Price,
                            Status = AppConstant.ContractStatus.PROCESSING,
                            Type = request.Type,
                            InsDate = LocalDateTime.VNDateTime()
                        };
                        await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);
                    }
                    else
                    {
                        avaibleContract.ContractCode = GenerateRandom.GenerateRandomString(10);
                        avaibleContract.StartDate = request.StartDate;
                        avaibleContract.EndDate = request.EndDate;
                        avaibleContract.ValidityPeriod = request.ValidityPeriod;
                        avaibleContract.TaxCode = request.TaxCode ?? null;
                        avaibleContract.Area = infoProject.Area ?? avaibleContract.Area;
                        avaibleContract.ContractValue = request.ContractValue;
                        avaibleContract.UrlFile = request.UrlFile;
                        avaibleContract.Note = request.Note ?? null;
                        avaibleContract.Deflag = true;
                        avaibleContract.RoughPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.ROUGH)?.Price;
                        avaibleContract.FinishedPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.FINISHED)?.Price;
                        avaibleContract.Status = AppConstant.ContractStatus.COMPLETED;
                        avaibleContract.Type = avaibleContract.Type;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(avaibleContract);
                        contractDrawing = avaibleContract;
                    }

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
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            TotalPrice = pay.Price,
                            PaymentDate = pay.PaymentDate,
                            PaymentPhase = pay.PaymentPhase,
                            Unit = AppConstant.Unit.UnitPrice,
                            Percents = pay.Percents,
                            Description = pay.Description
                        };

                        await _unitOfWork.GetRepository<Payment>().InsertAsync(payInfo);

                        var batchPay = new BatchPayment
                        {
                            Id = Guid.NewGuid(),
                            ContractId = contractDrawing!.Id,
                            InitialQuotationId = initialInfo!.Id,
                            InsDate = LocalDateTime.VNDateTime(),
                            FinalQuotationId = null,
                            PaymentId = payInfo.Id,
                            Status = AppConstant.PaymentStatus.PROGRESS,
                            NumberOfBatch = pay.NumberOfBatches
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
            #region Check contract
            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                predicate: x => x.Id == request.ProjectId,
                                include: x => x.Include(x => x.InitialQuotations)
                                                .ThenInclude(x => x.PackageQuotations)
                                                .ThenInclude(x => x.Package)
                                                .Include(x => x.FinalQuotations)
                                                .Include(x => x.Customer!));

            if (infoProject == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
            }

            if (infoProject.Contracts.Count(c => c.Type == AppConstant.ContractType.Design.ToString()) > 1)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ContractOver);
            }
            #endregion

            bool isInitialFinalized = infoProject.InitialQuotations.Any(x => x.Status == AppConstant.ProjectStatus.FINALIZED);

            if (isInitialFinalized)
            {
                #region Query package
                var packageInfo = infoProject.InitialQuotations
                    .SelectMany(x => x.PackageQuotations)
                    .Where(pq => pq.Package.Type == AppConstant.Type.ROUGH
                              || pq.Package.Type == AppConstant.Type.FINISHED)
                    .Select(pq => new
                    {
                        TypeName = pq.Package.Type,
                        Price = pq.Package.Price
                    })
                    .ToList();
                #endregion

                #region Create contract
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
                    TaxCode = request.TaxCode,
                    Area = infoProject.Area,
                    UnitPrice = AppConstant.Unit.UnitPrice,
                    ContractValue = request.ContractValue,
                    UrlFile = request.UrlFile,
                    Note = request.Note,
                    Deflag = true,
                    RoughPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.ROUGH)?.Price,
                    FinishedPackagePrice = packageInfo.FirstOrDefault(x => x.TypeName == AppConstant.Type.FINISHED)?.Price,
                    Status = AppConstant.ContractStatus.PROCESSING,
                    Type = AppConstant.ContractType.Construction.ToString(),
                    InsDate = LocalDateTime.VNDateTime(),
                };

                await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);
                #endregion

                #region Update Batch payment
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
                #endregion

                bool isSuccessful = _unitOfWork.Commit() > 0;
                return isSuccessful;
            }
            else
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, "Initial quotation is not finalized.");
            }
        }

        //Sales Staff Upload Contract has sign
        public async Task<string> UploadContractSign(Guid contractId, List<IFormFile> contractFile)
        {
            string publicId = null;
            var contractInfo = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                predicate: x => x.Id == contractId);

            if (contractInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
            }

            if (contractInfo.Type == AppConstant.ContractType.Design.ToString())
            {
                publicId = $"Hop_dong_thiet_ke_{contractId}";
            }
            else
            {
                publicId = $"Hop_dong_thi_cong_{contractId}";
            }
            foreach (var file in contractFile)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

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
                contractInfo.UrlFile = uploadResult.Url.ToString();
                if (contractInfo.Type == AppConstant.ContractType.Construction.ToString())
                {
                    var projectInfo = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(predicate: p => p.Id == contractInfo.ProjectId);
                    if (projectInfo == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
                    }
                    projectInfo.Status = AppConstant.ProjectStatus.SIGNED_CONTRACT;
                    _unitOfWork.GetRepository<Project>().UpdateAsync(projectInfo);
                }

                _unitOfWork.GetRepository<Contract>().UpdateAsync(contractInfo);
            }

            string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return result;

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
                            && x.Type.ToLower() == typeQuery.ToLower());
            if (contractInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Waiting);
            }
            var resutl = new ContractAppResponse(contractInfo.Id, contractInfo.UrlFile!);

            return resutl;
        }

        //Manager update bill contract design
        public async Task<string> UploadBillContractDesign(Guid paymentId, List<IFormFile> bills)
        {
            try
            {
                BatchPayment currentBatch = null;
                //Check list batch payment 
                var payBatchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                    predicate: p => p.PaymentId == paymentId,
                                    include: p => p.Include(p => p.Contract!)
                                                    .ThenInclude(p => p.Project));

                if (payBatchInfo == null || !payBatchInfo.Any())
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
                }

                int imageCount = Math.Min(bills.Count, payBatchInfo.Count());

                var contractInfo = payBatchInfo.FirstOrDefault()?.Contract;
                string contractType = contractInfo.Type;

                for (int i = 0; i < imageCount; i++)
                {
                    var file = bills[i];

                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    var publicId = contractType == "Design"
                            ? $"Hoa_don_thiet_ke_{paymentId}_{i}"
                            : $"Hoa_don_thi_cong_{paymentId}_{i}";

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        PublicId = publicId,
                        Folder = "Invoice",
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                    }

                    var mediaInfo = new Medium
                    {
                        Id = Guid.NewGuid(),
                        HouseDesignVersionId = null,
                        Name = AppConstant.General.Bill,
                        Url = uploadResult.Url.ToString(),
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        SubTemplateId = null,
                        PaymentId = payBatchInfo.ElementAt(i).PaymentId
                    };

                    await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);

                    currentBatch = payBatchInfo.ElementAt(i);
                    if (currentBatch.Status == AppConstant.PaymentStatus.PROGRESS)
                    {
                        currentBatch.Status = AppConstant.PaymentStatus.PAID;
                    }
                }

                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(payBatchInfo);

                var contractId = payBatchInfo.First().ContractId;
                var allPaid = payBatchInfo.All(x => x.Status == AppConstant.PaymentStatus.PAID);
                var finalBatch = await _unitOfWork.GetRepository<BatchPayment>()
                 .FirstOrDefaultAsync(predicate: x => x.ContractId == contractId,
                                      orderBy: q => q.OrderByDescending(x => x.NumberOfBatch));


                if (finalBatch.NumberOfBatch == currentBatch.NumberOfBatch)
                {
                    string projectStatus = payBatchInfo.First().Contract.Project.Status.ToString();
                    if (projectStatus != AppConstant.ProjectStatus.DESIGNED
                    && projectStatus != AppConstant.ProjectStatus.UNDER_REVIEW
                    && projectStatus != AppConstant.ProjectStatus.SIGNED_CONTRACT
                    && projectStatus != AppConstant.ProjectStatus.FINALIZED)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Not_Completed_Design);
                    }

                    var contract = payBatchInfo.First().Contract;
                    if (contract != null)
                    {
                        contract.Status = AppConstant.ContractStatus.FINISHED;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(contract);
                    }
                }

                string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
                return result;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, ex.Message);
            }
        }

        //Manager update bill contract construction
        public async Task<string> UploadBillContractConstruction(Guid paymentId, List<IFormFile> bills)
        {
            try
            {
                BatchPayment currentBatch = null;
                var payBatchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                    predicate: p => p.PaymentId == paymentId,
                                    include: p => p.Include(p => p.Contract!)
                                                    .ThenInclude(p => p.Project));

                if (payBatchInfo == null || !payBatchInfo.Any())
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

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        PublicId = $"Hop_dong_{paymentId}_{i}",
                        Folder = "Invoice",
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
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        SubTemplateId = null,
                        PaymentId = payBatchInfo.ElementAt(i).PaymentId
                    };

                    await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);

                    currentBatch = payBatchInfo.ElementAt(i);
                    if (currentBatch.Status == AppConstant.PaymentStatus.PROGRESS)
                    {
                        currentBatch.Status = AppConstant.PaymentStatus.PAID;
                    }
                }

                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(payBatchInfo);

                var contractId = payBatchInfo.First().ContractId;
                var allPaid = payBatchInfo.All(x => x.Status == AppConstant.PaymentStatus.PAID);
                var finalBatch = await _unitOfWork.GetRepository<BatchPayment>()
                     .FirstOrDefaultAsync(predicate: x => x.ContractId == contractId,
                                          orderBy: q => q.OrderByDescending(x => x.NumberOfBatch));


                if (finalBatch.NumberOfBatch == currentBatch.NumberOfBatch)
                {
                    var contract = payBatchInfo.First().Contract;
                    if (contract != null)
                    {
                        contract.Status = AppConstant.ContractStatus.FINISHED;
                        contract.Project.Status = AppConstant.ProjectStatus.FINALIZED;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(contract);
                    }
                }

                string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
                return result;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }

        public async Task<string> UploadBillContractAppendix(Guid paymentId, List<IFormFile> bills)
        {
            try
            {
                BatchPayment currentBatch = null;
                var payBatchInfo = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                    predicate: p => p.PaymentId == paymentId,
                                    include: p => p.Include(p => p.Contract!));

                if (payBatchInfo == null || !payBatchInfo.Any())
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

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        PublicId = $"Hop_dong_{paymentId}_{i}",
                        Folder = "Invoice",
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
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        SubTemplateId = null,
                        PaymentId = payBatchInfo.ElementAt(i).PaymentId
                    };

                    await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaInfo);

                    currentBatch = payBatchInfo.ElementAt(i);
                    if (currentBatch.Status == AppConstant.PaymentStatus.PROGRESS)
                    {
                        currentBatch.Status = AppConstant.PaymentStatus.PAID;
                    }
                }

                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(payBatchInfo);

                var contractId = payBatchInfo.First().ContractId;
                var allPaid = payBatchInfo.All(x => x.Status == AppConstant.PaymentStatus.PAID);
                var finalBatch = await _unitOfWork.GetRepository<BatchPayment>()
                     .FirstOrDefaultAsync(predicate: x => x.ContractId == contractId,
                                          orderBy: q => q.OrderByDescending(x => x.NumberOfBatch));


                if (finalBatch.NumberOfBatch == currentBatch.NumberOfBatch)
                {
                    var contract = payBatchInfo.First().Contract;
                    if (contract != null)
                    {
                        contract.Status = AppConstant.ContractStatus.FINISHED;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(contract);

                        var contractMain = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                            predicate: x => x.ContractAppendix == contract.Id,
                            include: x => x.Include(x => x.Project));
                        contractMain.Status = AppConstant.ContractStatus.FINISHED;

                        //Update status project Signed Contract -> Finalized
                        contractMain.Project.Status = ProjectStatus.FINALIZED;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(contractMain);
                    }
                }

                string result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
                return result;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }

        //Clone final info to contract construction
        public async Task<FinalToContractResponse> CloneFinalInfoToContract(Guid projectId)
        {
            try
            {
                var finalInfo = await _unitOfWork.GetRepository<FinalQuotation>().FirstOrDefaultAsync(
                                    predicate: p => p.ProjectId == projectId && p.Status == AppConstant.QuotationStatus.FINALIZED,
                                    include: p => p.Include(p => p.BatchPayments)
                                                                    .ThenInclude(p => p.Payment!));

                if (finalInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.NotFinalizedQuotationInitial);
                }

                // Calculate contract value
                double contractValue = (finalInfo.TotalPrice ?? 0.0)
                                     - (finalInfo.Discount ?? 0.0);

                // BatchPaymentRequest
                var batchPaymentRequests = finalInfo.BatchPayments
                    .Select((batchPayment, index) => new InitialToBatchPayment
                    {
                        NumberOfBatches = index + 1,
                        Price = (double)batchPayment.Payment.TotalPrice!,
                        PaymentDate = batchPayment.Payment.PaymentDate,
                        PaymentPhase = batchPayment.Payment.PaymentPhase,
                        Percents = batchPayment.Payment.Percents ?? 0,
                        Description = batchPayment.Payment.Description
                    }).OrderBy(x => x.NumberOfBatches)
                    .ToList();

                var result = new FinalToContractResponse()
                {
                    ProjectId = projectId,
                    Type = AppConstant.ContractType.Design.ToString(),
                    StartDate = null,
                    EndDate = null,
                    ValidityPeriod = null,
                    TaxCode = null,
                    ContractValue = contractValue,
                    UrlFile = null,
                    Note = null,
                    BatchPaymentRequests = batchPaymentRequests
                };
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<string> UploadFileContract(IFormFile file)
        {
            var result = await _mediaService.UploadImageSubTemplate(file, "Contract");
            return result;
        }

        public async Task<bool> ManagerApproverBillFromCustomer(Guid paymentId, string type)
        {
            bool result = false;

            try
            {
                // Lấy thông tin đợt thanh toán hiện tại
                var currentBatch = await _unitOfWork.GetRepository<BatchPayment>().FirstOrDefaultAsync(
                                    predicate: b => b.PaymentId == paymentId);

                if (currentBatch == null)
                    return false;

                if (type == "Approved")
                {

                    currentBatch.Status = AppConstant.PaymentStatus.PAID;
                    _unitOfWork.GetRepository<BatchPayment>().UpdateAsync(currentBatch);

                    var contract = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                 predicate: c => c.Id == currentBatch.ContractId,
                                 include: c => c.Include(c => c.Project));

                    if (contract == null)
                        return false;

                    #region Check batch payment
                    var batchPayments = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                        predicate: b => b.ContractId == currentBatch.ContractId
                    );


                    bool allPaid = batchPayments.All(x => x.Status == AppConstant.PaymentStatus.PAID);

                    // Final last batch payment in contract
                    var finalBatch = batchPayments
                        .OrderByDescending(x => x.NumberOfBatch)
                        .FirstOrDefault();
                    #endregion

                    #region Update status in contract
                    if (finalBatch != null && finalBatch.NumberOfBatch == currentBatch.NumberOfBatch)
                    {
                        //Update status in contract appendix
                        contract.Status = AppConstant.ContractStatus.FINISHED;
                        if (contract.Type.ToString() == AppConstant.ContractType.Construction.ToString())
                        {
                            contract.Project.Status = AppConstant.ProjectStatus.FINALIZED;
                        }

                        if (contract.Type == AppConstant.Type.APPENDIX_CONSTRUCTION)
                        {
                            contract.Project.Status = AppConstant.ProjectStatus.FINALIZED;
                        }

                        _unitOfWork.GetRepository<Contract>().UpdateAsync(contract);

                        //Update status in contract main
                        if (contract.Type == AppConstant.ContractType.Appendix.ToString())
                        {
                            var contractMain = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                        predicate: x => x.ContractAppendix == contract.Id);
                            contractMain.Status = AppConstant.ContractStatus.FINISHED;
                            _unitOfWork.GetRepository<Contract>().UpdateAsync(contractMain);
                        }
                    }
                    #endregion

                    result = await _unitOfWork.CommitAsync() > 0;
                }
                return result;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError(ex.Code, ex.Message);
            }
        }

        public async Task<bool> CreateContractAppendix(ContractAppendixRequest request)
        {
            try
            {
                #region Check contract
                var contractInfo = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(
                                    predicate: x => x.Id == request.ContractId,
                                    include: x => x.Include(x => x.Project)
                                                    .Include(x => x.BatchPayments!));

                if (contractInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Contract_Not_Found);
                }

                if (contractInfo.ContractAppendix != null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Conflict_Contract_Appendix);
                }
                #endregion

                #region Cancel BatchPayment
                var cancelBatchPaymentIds = request.CancelBatchPaymnetContract.Select(x => x.BatchPaymentId).ToHashSet();
                var batchPaymentsToCancel = contractInfo.BatchPayments
                    .Where(bp => cancelBatchPaymentIds.Contains(bp.Id))
                    .ToList();
                Guid initialId = Guid.Empty;
                if (batchPaymentsToCancel.Count < 1)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.BatchPayment_Non_Coincidence);
                }

                foreach (var batchPayment in batchPaymentsToCancel)
                {
                    initialId = (Guid)batchPayment.InitialQuotationId;
                    batchPayment.Status = "Cancel";
                }

                _unitOfWork.GetRepository<BatchPayment>().UpdateRange(batchPaymentsToCancel);
                #endregion

                #region Create contract appendix
                string contractAppendixType = null;
                if (contractInfo.Type == AppConstant.ContractType.Construction.ToString())
                {
                    contractAppendixType = AppConstant.ContractType.Appendix_Construction.ToString();
                }
                if (contractInfo.Type == AppConstant.ContractType.Design.ToString())
                {
                    contractAppendixType = AppConstant.ContractType.Appendix_Design.ToString();
                }

                var contractDrawing = new Contract
                {
                    Id = Guid.NewGuid(),
                    ProjectId = contractInfo.ProjectId,
                    Name = EnumExtensions.GetEnumDescription(AppConstant.ContractType.Appendix),
                    CustomerName = contractInfo.CustomerName,
                    ContractCode = GenerateRandom.GenerateRandomString(10),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    ValidityPeriod = request.ValidityPeriod,
                    TaxCode = request.TaxCode,
                    Area = contractInfo.Area,
                    UnitPrice = AppConstant.Unit.UnitPrice,
                    ContractValue = request.ContractValue,
                    UrlFile = request.UrlFile,
                    Note = request.Note,
                    Deflag = true,
                    RoughPackagePrice = contractInfo.RoughPackagePrice,
                    FinishedPackagePrice = contractInfo.FinishedPackagePrice,
                    Status = AppConstant.ContractStatus.PROCESSING,
                    Type = contractAppendixType,
                    InsDate = LocalDateTime.VNDateTime(),
                };

                await _unitOfWork.GetRepository<Contract>().InsertAsync(contractDrawing);
                #endregion

                #region Update contract appendix in contract main
                contractInfo.ContractAppendix = contractDrawing.Id;
                _unitOfWork.GetRepository<Contract>().UpdateAsync(contractInfo);
                #endregion

                #region Create Batch payment appendix

                foreach (var pay in request.BatchPaymentRequests!)
                {
                    // Lấy PaymentType từ bảng PaymentType
                    var paymentType = await _unitOfWork.GetRepository<PaymentType>()
                                .FirstOrDefaultAsync(pt => pt.Name == EnumExtensions.GetEnumDescription(AppConstant.ContractType.Appendix));

                    var payInfo = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PaymentTypeId = paymentType.Id,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        TotalPrice = pay.Price,
                        PaymentDate = pay.PaymentDate,
                        PaymentPhase = pay.PaymentPhase,
                        Unit = AppConstant.Unit.UnitPrice,
                        Percents = pay.Percents,
                        Description = pay.Description
                    };

                    await _unitOfWork.GetRepository<Payment>().InsertAsync(payInfo);

                    var batchPay = new BatchPayment
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contractDrawing!.Id,
                        InitialQuotationId = initialId,
                        InsDate = LocalDateTime.VNDateTime(),
                        FinalQuotationId = null,
                        PaymentId = payInfo.Id,
                        Status = AppConstant.PaymentStatus.PROGRESS,
                        NumberOfBatch = pay.NumberOfBatches
                    };

                    await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(batchPay);
                }
                #endregion

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError(ex.Code, ex.Message);
            }
        }

        public async Task<string> DeleteCustomerBillPayment(Guid paymentId)
        {
            var paymentInfo = await _unitOfWork.GetRepository<Payment>().FirstOrDefaultAsync(
                            predicate: pay => pay.Id == paymentId,
                            include: pay => pay.Include(pay => pay.Media));
            if (paymentInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Invalid_Payment);
            }

            Medium itemMedia = (Medium)paymentInfo.Media.First();

            _unitOfWork.GetRepository<Medium>().DeleteAsync(itemMedia);

            var result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_DELETE : AppConstant.ErrMessage.Fail_Delete;
            return result;
        }

        public async Task<double> GetPriceContractDesignByAreaInitialQuotation(Guid projectId)
        {
            var initialQuotaiton = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                   predicate: i => i.ProjectId == projectId && i.Status == QuotationStatus.FINALIZED);
            if (initialQuotaiton == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.NotFinalizedQuotationInitial);
            }

            var priceDesign = await _unitOfWork.GetRepository<DesignPrice>().FirstOrDefaultAsync(
                                           predicate: d => d.AreaFrom <= initialQuotaiton.Area && d.AreaTo >= initialQuotaiton.Area);
            if (priceDesign == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Invalid_Area);
            }
            double result = (double)priceDesign.Price! * (double)initialQuotaiton.Area!;
            return result;
        }
    }
}
