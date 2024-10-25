using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IPaymentService
    {
        Task<IPaginate<PaymentResponse>> GetListPayment(int page, int size);
        Task<List<PaymentResponse>> GetDetailPayment(Guid projectId);
        Task<string> ApproveContractDesign(Guid paymentId, List<IFormFile> bills);
        Task<string> ApproveContractContruction(Guid paymentId, List<IFormFile> bills);
    }
}
