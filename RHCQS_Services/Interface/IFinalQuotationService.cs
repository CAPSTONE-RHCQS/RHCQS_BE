    using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.App;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IFinalQuotationService
    {
        Task<IPaginate<FinalQuotationListResponse>> GetListFinalQuotation(int page, int size);
        Task<FinalQuotationResponse> GetDetailFinalQuotationById(Guid id);
        Task<FinalQuotationResponse> GetDetailFinalQuotationByProjectId(Guid projectid);
/*        Task<FinalQuotationResponse> GetDetailFinalQuotationByCustomerName(string name);*/
        Task<string> ApproveFinalFromManager(Guid Id, ApproveQuotationRequest request);
        Task<Guid?> UpdateFinalQuotation(FinalRequest request);
        Task<FinalQuotationResponse> CreateFinalQuotation(Guid projectId);
        Task<bool> CancelFinalQuotation(Guid Id, CancelQuotation reason);
        Task<List<FinalAppResponse>> GetListFinalQuotationByProjectId(Guid projectId);
        Task<string> FeedbackFixFinalFromCustomer(Guid finalId, FeedbackQuotationRequest comment);
        Task<string> ConfirmArgeeFinalFromCustomer(Guid finalId);
        Task DeleteFinalQuotation(Guid finalQuotationId);
    }
}
