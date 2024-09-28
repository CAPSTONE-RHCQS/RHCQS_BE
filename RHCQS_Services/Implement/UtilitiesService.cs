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
    }
}
