using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.ConstructionWork
{
    public class CreateWorkTemplateRequest
    {
        public Guid ConstructionWorkId { get; set; }
        public Guid PackageId { get; set; }

        public double? LaborCost { get; set; }

        public double? MaterialCost { get; set; }

        public double? MaterialFinishedCost { get; set; }

        public double? TotalCost { get; set; }

    }
}
