using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class QuotationUtilitiesRequest
    {
        public Guid UltilitiesItemId { get; set; }

        public string Name { get; set; } = null!;

        public double? Coefiicient { get; set; }

        public double? Price { get; set; }

        public string? Description { get; set; }
    }
}
