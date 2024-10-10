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
    public interface IInitialQuotationService
    {
        Task<IPaginate<InitialQuotationListResponse>> GetListInitialQuotation(int page, int size);
        Task<InitialQuotationResponse> GetDetailInitialQuotationById(Guid id);
        Task<InitialQuotationResponse> GetDetailInitialQuotationByCustomerName(string name);
        Task<string> ApproveInitialFromManager(Guid initialId, ApproveQuotationRequest request);

        Task<bool> UpdateInitialQuotation(UpdateInitialRequest request);
    }
}
