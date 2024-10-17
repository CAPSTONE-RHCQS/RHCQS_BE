using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IContractService
    {
        Task<bool> CreateContractDeisgn(ContractDesignRequest request);
        Task<string> ApproveContractDesin(Guid paymentId, List<IFormFile> bills);
    }
}
