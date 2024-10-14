    using RHCQS_BusinessObject.Payload.Request.FinalQuotation;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
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
        Task<FinalQuotationResponse> GetDetailFinalQuotationByCustomerName(string name);
        Task<string> ApproveFinalFromManager(Guid Id, ApproveQuotationRequest request);
        Task<bool> UpdateFinalQuotation(FinalRequest request);
        Task<bool> CreateFinalQuotation(FinalRequest request);
        Task<bool> CancelFinalQuotation(Guid Id, CancelQuotation reason);
    }
}
