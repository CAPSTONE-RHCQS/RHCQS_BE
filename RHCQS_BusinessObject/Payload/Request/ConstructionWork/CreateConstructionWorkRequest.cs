using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.ConstructionWork
{
    public class CreateConstructionWorkRequest
    {
        public string? WorkName { get; set; }

        public Guid? ConstructionId { get; set; }

        public string? Unit { get; set; }

        public string? Code { get; set; }

        public List<CreateConstructionWorkResourceRequest> Resources { get; set; }
    }

    public class CreateConstructionWorkResourceRequest
    {
        public Guid? MaterialSectionId { get; set; }

        public double? MaterialSectionNorm { get; set; }

        public Guid? LaborId { get; set; }

        public double? LaborNorm { get; set; }
    }
}
