using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Contract;
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

        public ContractService(IUnitOfWork unitOfWork, ILogger<IContractService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
                            TaxCode = request.TaxCode,
                            Area = infoProject.Area,
                            UnitPrice = AppConstant.Unit.UnitPrice,
                            ContractValue = request.ContractValue,
                            UrlFile = request.UrlFile,
                            Note = request.Note,
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
                                Price = pay.Price,
                                PaymentDate = pay.PaymentDate,
                                PaymentPhase = pay.PaymentPhase,
                                IntitialQuotationId = initialInfo!.Id,
                                Percents = pay.Percents,
                                InsDate = DateTime.Now,
                                FinalQuotationId = null,
                                Description = pay.Description,
                                Unit = AppConstant.Unit.UnitPrice
                            };

                            await _unitOfWork.GetRepository<BatchPayment>().InsertAsync(batchPay);

                            // Lấy PaymentType từ bảng PaymentType
                            var paymentType = await _unitOfWork.GetRepository<PaymentType>()
                                        .FirstOrDefaultAsync(pt => pt.Name == EnumExtensions.GetEnumDescription(contractType));

                            var payInfo = new Payment
                            {
                                Id = Guid.NewGuid(),
                                PaymentTypeId = paymentType.Id,
                                BatchPaymentId = batchPay.Id,
                                Status = AppConstant.PaymentStatus.PROGRESS,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                TotalPrice = request.ContractValue,
                                Type = request.Type,
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
    }
}
