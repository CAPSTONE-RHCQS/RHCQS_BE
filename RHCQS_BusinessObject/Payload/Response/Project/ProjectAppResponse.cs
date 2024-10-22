using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Project
{
    public class ProjectAppResponse
    {
        public ProjectAppResponse(InitialAppResponse? initial, ContractDesignAppResponse? design,
            FinalAppResponse? final, ContractProcessingAppResponse? processing)
        {
            InitialResponse = initial;
            ContractDesignResponse = design;
            FinalAppResponse = final;
            ContractProcessingResponse = processing;
        }
        public InitialAppResponse? InitialResponse { get; set; }
        public ContractDesignAppResponse? ContractDesignResponse { get; set; }
        public FinalAppResponse? FinalAppResponse { get; set; }
        public ContractProcessingAppResponse? ContractProcessingResponse { get; set; }

    }

    public class InitialAppResponse
    {
        public string Status { get; set; }
    }

    public class ContractDesignAppResponse
    {
        public string Status { get; set; }
    }

    public class FinalAppResponse
    {
        public string Status { get; set; }
    }

    public class ContractProcessingAppResponse
    {
        public string Status { get; set; }
    }
}
