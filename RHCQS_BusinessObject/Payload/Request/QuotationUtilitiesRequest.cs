using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class QuotationUtilitiesRequest
    {
        public Guid UtilitiesItemId { get; set; }

        public string Name { get; set; } = null!;

        public double? Price { get; set; }
    }
}
