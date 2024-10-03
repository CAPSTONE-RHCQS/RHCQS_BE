using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class InitialQuotationRequest
    {
        public Guid AccountId { get; set; }

        public Guid? PromotionId { get; set; }

        public Guid PackageId { get; set; }

        public List<InitialQuotationItemRequest>? InitialQuotationItemRequests { get; set; }
    }


    public class InitialQuotationItemRequest
    {

        public string? Name { get; set; }

        public Guid ConstructionItemId { get; set; }

        public Guid? SubConstructionId { get; set; }

        public double? Area { get; set; }

        public double? Price { get; set; }

    }
}
