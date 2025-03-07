﻿using RHCQS_BusinessObject.Payload.Request.Utility;
using RHCQS_BusinessObject.Payload.Response.Utility;
using RHCQS_BusinessObjects;

namespace RHCQS_Services.Interface
{
    public interface IUtilitiesService
    {
        Task<IPaginate<UtilityResponse>> GetListUtilities(int page, int size);
        Task<List<UtilityResponse>> GetListUtilitiesByType(string type);
        Task<UtilitiesSectionResponse> SearchUtilityItem(string name);
        Task<UtilityResponse> GetDetailUtilityItem(Guid id);
        Task<UtilitiesSectionResponse> GetDetailUtilitySection(Guid idUtilitySection);
        Task<bool> CreateUtility(UtilityRequest request);
        Task<bool> UpdateUtility(UpdateUtilityRequest request);
        Task<string> BanUtility(Guid utilityId);

        Task<List<AutoUtilityResponse>> GetDetailUtilityByContainName(string projectType, string name);
    }
}
