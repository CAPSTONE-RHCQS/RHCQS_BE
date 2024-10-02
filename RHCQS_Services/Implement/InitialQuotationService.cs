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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class InitialQuotationService : IInitialQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InitialQuotationService> _logger;

        public InitialQuotationService(IUnitOfWork unitOfWork, ILogger<InitialQuotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<InitialQuotationListResponse>> GetListInitialQuotation(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<InitialQuotation>().GetList(
                selector: x => new InitialQuotationListResponse(x.Id, x.Project.Customer.Username ,x.Version, x.Area, x.Status),
                include: x => x.Include(x => x.Project)
                                .ThenInclude(x => x.Customer!),
                page: page,
                size:size
                );
            return list;
        }

        public async Task<InitialQuotationResponse> GetDetailInitialQuotationById(Guid id)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                        x => x.Id.Equals(id),
                        include: x => x.Include(x => x.InitialQuotationItems)
                                       .ThenInclude(x => x.ConstructionItem)
                                       .ThenInclude(x => x.SubConstructionItems!)
                                       .Include(x => x.Project)
                                       .ThenInclude(x => x.Customer!)
                                       .Include(x => x.PackageQuotations)
                                       .ThenInclude(x => x.Package)
                );
            var roughPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "ROUGH");

            var finishedPackage = initialQuotation.PackageQuotations
                .FirstOrDefault(item => item.Type == "FINISHED");

            var packageInfo = new PackageQuotationList(
                roughPackage?.PackageId ?? Guid.Empty,
                roughPackage?.Package.PackageName ?? string.Empty,
                finishedPackage?.PackageId ?? Guid.Empty,
                finishedPackage?.Package.PackageName ?? string.Empty
            );

            var itemInitialResponses = initialQuotation.InitialQuotationItems.Select(item => new InitialQuotationItemResponse(
                                       item.Id,
                                       item.ConstructionItem?.Name,
                                       item.ConstructionItem?.SubConstructionItems != null ? 
                                        string.Join(", ", item.ConstructionItem.SubConstructionItems.Select(s => s.Name)) : null,
                                       item.Area,
                                       item.Price,
                                       item.UnitPrice,
                                       item.ConstructionItem?.Name
                                       )).ToList();

            var result = new InitialQuotationResponse
            {
                Id = initialQuotation.Id,
                AccountName = initialQuotation.Project.Customer.Username,
                ProjectId = initialQuotation.Project.Id,
                PromotionId = initialQuotation.PromotionId,
                PackageId = initialQuotation.PackageId,
                Area = initialQuotation.Area,
                TimeProcessing = initialQuotation.TimeProcessing,
                TimeOthers = initialQuotation.TimeOthers,
                OthersAgreement = initialQuotation.OthersAgreement,
                InsDate = initialQuotation.InsDate,
                Status = initialQuotation.Status,
                Version = initialQuotation.Version,
                Deflag = (bool)initialQuotation.Deflag,
                Note = initialQuotation.Note,
                PackageQuotationList = packageInfo,
                ItemInitial = itemInitialResponses
            };

            return result;
        }
    }
}
