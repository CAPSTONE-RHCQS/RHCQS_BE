using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class DesignPriceResponse
    {
        public Guid Id { get; set; }

        public double? AreaFrom { get; set; }

        public double? AreaTo { get; set; }

        public double? Price { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }
    }
}
