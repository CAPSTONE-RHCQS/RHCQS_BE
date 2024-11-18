using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Contract
{
    public class ContractResponse
    {
        public ContractResponse(
            Guid projectId,
            string? name,
            string? customerName,
            string? contractCode,
            DateTime? startDate,
            DateTime? endDate,
            int? validityPeriod,
            string? taxCode,
            double? area,
            string? unitPrice,
            double? contractValue,
            string? urlFile,
            string? note,
            bool? deflag,
            double? roughPackagePrice,
            double? finishedPackagePrice,
            string status,
            string type,
            DependOnQuotation quotation)
        {
            ProjectId = projectId;
            Name = name;
            CustomerName = customerName;
            ContractCode = contractCode;
            StartDate = startDate;
            EndDate = endDate;
            ValidityPeriod = validityPeriod;
            TaxCode = taxCode;
            Area = area;
            UnitPrice = unitPrice;
            ContractValue = contractValue;
            UrlFile = urlFile;
            Note = note;
            Deflag = deflag;
            RoughPackagePrice = roughPackagePrice;
            FinishedPackagePrice = finishedPackagePrice;
            Status = status;
            Type = type;
            Quotation = quotation;
        }

        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public string? CustomerName { get; set; }
        public string? ContractCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ValidityPeriod { get; set; }
        public string? TaxCode { get; set; }
        public double? Area { get; set; }
        public string? UnitPrice { get; set; }
        public double? ContractValue { get; set; }
        public string? UrlFile { get; set; }
        public string? Note { get; set; }
        public bool? Deflag { get; set; }
        public double? RoughPackagePrice { get; set; }
        public double? FinishedPackagePrice { get; set; }
        public string Status { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DependOnQuotation Quotation { get; set; }
    }

    public class DependOnQuotation
    {
        public Guid QuotationlId { get; set; }
        public double Version { get; set; }
        public string File { get; set; }
    }
}
