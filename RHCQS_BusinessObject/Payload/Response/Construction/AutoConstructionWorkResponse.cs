using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Construction
{
    public class AutoConstructionWorkResponse
    {
        public AutoConstructionWorkResponse(
            Guid workTemplateId,
            Guid constructionWorkId,
            string constructionWorkName,
            double laborCost,
            double? materialRoughCost,
            double? materialFinishedCost)
        {
            WorkTemplateId = workTemplateId;
            ConstructionWorkId = constructionWorkId;
            ConstructionWorkName = constructionWorkName;
            LaborCost = laborCost;
            MaterialRoughCost = materialRoughCost;
            MaterialFinishedCost = materialFinishedCost;
        }
        public Guid WorkTemplateId { get; set; }
        public Guid ConstructionWorkId { get; set; }
        public string ConstructionWorkName { get; set; }
        public double LaborCost { get; set; }
        public double? MaterialRoughCost { get; set; }
        public double? MaterialFinishedCost { get; set; }

    }
}
