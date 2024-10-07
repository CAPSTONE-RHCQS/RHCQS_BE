using RHCQS_BusinessObject.Payload.Request.Utility;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IUtilitiesService
    {
        Task<IPaginate<UtilityResponse>> GetListUtilities(int page, int size);
        Task<List<UtilityResponse>> GetListUtilitiesByType(string type);
        Task<UtilityResponse> GetDetailUtilityItem(Guid id);
        Task<UtilitiesSectionResponse> GetDetailUtilitySection(Guid idUtilitySection);
        Task<bool> CreateUtility(UtilityRequest request);
    }
}
