using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.ConstructionWork
{
    public class UpdateConstructionWorkResourceRequest
    {
        public Guid? MaterialSectionId { get; set; }

        public double? MaterialSectionNorm { get; set; }

        public Guid? LaborId { get; set; }

        public double? LaborNorm { get; set; }

        public DateTime? InsDate { get; set; }
    }
}
