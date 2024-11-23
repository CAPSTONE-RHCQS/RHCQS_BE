using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class PaymentResponse
    {
        public class PaymentInfoResponse
        {
            public PaymentInfoResponse(PaymentResponse paymentResponse, List<BatchResponse> batchResponses)
            {
                PaymentResponse = paymentResponse;
                BatchResponse = batchResponses;
            }
            public PaymentResponse PaymentResponse { get; set; }
            public List<BatchResponse> BatchResponse { get; set; }
        }

        public PaymentResponse(int? priorty, Guid id, string type, string status, DateTime? insDate, DateTime? upsDate, 
            double? totalprice, DateTime? paymentDate, DateTime? paymentPhase, string? unit,
            int percents, string? description)
        {
            Priority = priorty;
            Id = id;
            Type = type;
            Status = status;
            InsDate = insDate;
            UpsDate = upsDate;
            TotalPrice = totalprice;
            PaymentDate = paymentDate;
            PaymentPhase = paymentPhase;
            Unit = unit;
            Percents = percents;
            Description = description;
        }
        public int? Priority { get; set; }
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public double? TotalPrice { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? PaymentPhase { get; set; }

        public string? Unit { get; set; }

        public int Percents { get; set; }

        public string? Description { get; set; }

    }

    public class BatchResponse
    {
        public BatchResponse(PaymentResponse paymentResponse, Guid id, Guid? contractId, double? price, DateTime? paymentDate, DateTime? paymentPhase, string? percents, DateTime? insDate, string? description, string? unit)
        {
            PaymentResponse = paymentResponse;
            Id = id;
            ContractId = contractId;
            Price = price;
            PaymentDate = paymentDate;
            PaymentPhase = paymentPhase;
            Percents = percents;
            InsDate = insDate;
            Description = description;
            Unit = unit;
        }

        public PaymentResponse PaymentResponse { get; set; }
        public Guid Id { get; set; }

        public Guid? ContractId { get; set; }

        public double? Price { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? PaymentPhase { get; set; }

        public string? Percents { get; set; }

        public DateTime? InsDate { get; set; }
        public string? Description { get; set; }

        public string? Unit { get; set; }
    }
}
