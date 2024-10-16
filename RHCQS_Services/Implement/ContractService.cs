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

        //Create design contract
        public async Task<bool> CreateContractDeisgn(ContractDesignRequest request)
        {
            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                predicate: x => x.Id == request.ProjectId,
                                include: x => x.Include(x => x.InitialQuotations)
                                                .ThenInclude(x => x.PackageQuotations)
                                                .ThenInclude(x => x.Package)
                                                .ThenInclude(x => x.PackageType)
                                                .Include(x => x.Customer!));

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
                    // Create the contract
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


                    bool isSuccessful = _unitOfWork.Commit() > 0;
                    return isSuccessful;
                }
                else
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invail_Quotation);
                }
            }
            else
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.Invail_Quotation);
            }
        }

        //Confirm design contract's payment of customer
    }
}
