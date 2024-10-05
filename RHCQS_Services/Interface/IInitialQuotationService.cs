using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IInitialQuotationService
    {
        Task<IPaginate<InitialQuotationListResponse>> GetListInitialQuotation(int page, int size);
        Task<InitialQuotationResponse> GetDetailInitialQuotationById(Guid id);
        Task<string> AssignQuotation(Guid accountId, Guid initialQuotationId);
        Task<bool> ApproveInitialFromManager(Guid initialId, ApproveQuotationRequest request);
    }
}
