using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class ProjectRequest
    {
        public Guid? CustomerId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Address { get; set; }

        public double? Area { get; set; }

        public InitialQuotationRequest InitialQuotation { get; set; }

        public List<QuotationUtilitiesRequest>? QuotationUtilitiesRequest { get; set; }
    }
}
