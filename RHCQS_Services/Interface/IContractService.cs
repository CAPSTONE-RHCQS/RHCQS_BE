﻿using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request.Contract;
using RHCQS_BusinessObject.Payload.Response.App;
using RHCQS_BusinessObject.Payload.Response.Contract;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IContractService
    {
        Task<bool> CreateContractDesign(ContractDesignRequest request);
        Task<bool> CreateContractConstruction(ContractConstructionRequest request);
        Task<IPaginate<IpaginateContractResponse>> GetListContract(int page, int size);
        Task<ContractResponse> GetDetailContract(Guid contractId);
        Task<ContractResponse> GetDetailContractByType(string type);
        Task<ContractAppResponse> GetListContractApp(Guid projectId, string type);
        Task<string> UploadContractSign(Guid contractId, List<IFormFile> contractFile);
        Task<string> BillContractDesign(Guid paymentId, List<IFormFile> bills);
        Task<string> BillContractContruction(Guid paymentId, List<IFormFile> bills);
    }
}
