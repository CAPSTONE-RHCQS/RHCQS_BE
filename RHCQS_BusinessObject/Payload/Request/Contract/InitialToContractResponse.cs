using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.Contract
{
    public class FinalToContractResponse
    {
        public Guid ProjectId { get; set; }

        public string Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ValidityPeriod { get; set; }

        public string? TaxCode { get; set; }

        public double? ContractValue { get; set; }

        public string? UrlFile { get; set; }

        public string? Note { get; set; }
        public List<InitialToBatchPayment>? BatchPaymentRequests { get; set; }
    }

    public class InitialToBatchPayment
    {
        public int NumberOfBatches { get; set; }

        public double Price { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? PaymentPhase { get; set; }
        public string? Percents { get; set; }
        public string? Description { get; set; }
    }
}
