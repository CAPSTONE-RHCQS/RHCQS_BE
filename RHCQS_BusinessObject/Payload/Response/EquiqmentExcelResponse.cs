using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class EquiqmentExcelResponse
    {
        public string? STT { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Unit { get; set; }

        public int? Quantity { get; set; }

        public double? UnitOfMaterial { get; set; }

        public double? TotalOfMaterial { get; set; }

        public string? Note { get; set; }

        public string? Type { get; set; }
    }
    public class WorkTemplateExcelResponse
    {
        public string Code { get; set; }
        public string ConstructionWorkName { get; set; }
        public Guid ConstructionId { get; set; }
        public string ConstructionName { get; set; }
        public double Weight { get; set; }
        public double LaborCost { get; set; }
        public double MaterialCost { get; set; }
        public double MaterialFinishedCost { get; set; }
        public string Unit { get; set; }
    }
    public class GroupedConstructionResponse
    {
        public Guid ConstructionId { get; set; }
        public string ConstructionName { get; set; }
        public List<WorkTemplateExcelShow> WorkTemplates { get; set; } = new List<WorkTemplateExcelShow>();
    }
    public class WorkTemplateExcelShow
    {
        public string Code { get; set; }
        public string ConstructionWorkName { get; set; }
        public double Weight { get; set; }
        public double LaborCost { get; set; }
        public double MaterialCost { get; set; }
        public double MaterialFinishedCost { get; set; }
        public string Unit { get; set; }
    }
}
