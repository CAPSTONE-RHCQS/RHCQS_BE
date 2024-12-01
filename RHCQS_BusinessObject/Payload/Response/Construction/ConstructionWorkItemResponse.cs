using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Construction
{
    public class ConstructionWorkItemResponse
    {
        public Guid Id { get; set; }

        public string? WorkName { get; set; }

        public Guid? ConstructionId { get; set; }

        public DateTime? InsDate { get; set; }

        public string? Unit { get; set; }

        public string? Code { get; set; }
        public List<ConstructionWorkResourceItem> Resources { get; set; }
        public List<WorkTemplateItem>? WorkTemplates { get; set; }
    }

    public class ConstructionWorkResourceItem
    {
        public Guid Id { get; set; }

        public Guid? MaterialSectionId { get; set; }
        public string? MaterialSectionName { get; set; }

        public double? MaterialSectionNorm { get; set; }

        public Guid? LaborId { get; set; }
        public string? LaborName { get; set; }

        public double? LaborNorm { get; set; }

        public DateTime? InsDate { get; set; }
    }
    public class WorkTemplateItem
    {
        public Guid Id { get; set; }

        public Guid PackageId { get; set; }
        public string PackageName { get; set; } 
        public double? LaborCost { get; set; }

        public double? MaterialCost { get; set; }

        public double? MaterialFinishedCost { get; set; }
        public double TotalCost {  get; set; }

        public DateTime? InsDate { get; set; }
    }

}
