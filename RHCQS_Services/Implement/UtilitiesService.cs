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

namespace RHCQS_Services.Implement
{
    public class UtilitiesService : IUtilitiesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UtilitiesService> _logger;

        public UtilitiesService(IUnitOfWork unitOfWork, ILogger<UtilitiesService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<UtilityResponse>> GetListUtilities(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<Utility>().GetList(
                    selector: x => new UtilityResponse(x.Id, x.Name, x.Type, x.Status, x.InsDate, x.UpsDate,
                                                       x.UtilitiesSections.Select(s => new UtilitiesSectionResponse(
                                                           s.Id,
                                                           s.Name,
                                                           s.Status,
                                                           s.InsDate,
                                                           s.UpsDate,
                                                           s.Description
                )).ToList()),
                    page: page,
                    size: size);
            return list;
        }

        public async Task<List<UtilityResponse>> GetListUtilitiesByType(string type)
        {
            var listConstruction = await _unitOfWork.GetRepository<Utility>().GetList(
                predicate: x => x.Type.Equals(type.ToUpper()),
                selector: x => new UtilityResponse(x.Id, x.Name, x.Type, x.Status, x.InsDate,
                                                            x.UpsDate,
                                                            x.UtilitiesSections.Select(
                                                                s => new UtilitiesSectionResponse(
                                                                    s.Id,
                                                                    s.Name,
                                                                    s.Status,
                                                                    s.InsDate,
                                                                    s.UpsDate,
                                                                    s.Description)).ToList()),
                include: x => x.Include(x => x.UtilitiesSections),
                orderBy: x => x.OrderBy(x => x.InsDate));
            return listConstruction.Items.ToList();
        }

        public async Task<UtilityResponse> GetDetailUtilityItem(Guid id)
        {
            var utiItem = await _unitOfWork.GetRepository<Utility>().FirstOrDefaultAsync(
                predicate: con => con.Id == id,
                include: con => con.Include(con => con.UtilitiesSections)
            );

            if (utiItem != null)
            {
                return new UtilityResponse(
                    utiItem.Id,
                    utiItem.Name,
                    utiItem.Type,
                    utiItem.Status,
                    utiItem.InsDate,
                    utiItem.UpsDate,
                    utiItem.UtilitiesSections.Select(
                        s => new UtilitiesSectionResponse(
                             s.Id,
                             s.Name,
                             s.Status,
                             s.InsDate,
                             s.UpsDate,
                             s.Description)).ToList()
                );
            }

            return null;
        }

        public async Task<UtilitiesSectionResponse> GetDetailUtilitySection(Guid idUtilitySection)
        {
            var utiItem = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(
                x => x.Id == idUtilitySection
            );

            if (utiItem != null)
            {
                return new UtilitiesSectionResponse(
                    utiItem.Id,
                    utiItem.Name,
                    utiItem.Status,
                    utiItem.InsDate,
                    utiItem.UpsDate,
                    utiItem.Description, 
                    utiItem.UtilitiesItems?.Select( 
                        s => new UtilityItemResponse(
                            s.Id,
                            s.Name,
                            s.Coefficient,
                            s.InsDate,
                            s.UpsDate
                        )
                    ).ToList() ?? new List<UtilityItemResponse>() 
                );
            }

            return null;
        }
    }
}
