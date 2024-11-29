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
            Guid constructionWorkId,
            string constructionWorkName,
            Guid laborId,
            double unitLabor,
            Guid? materialRoughId,
            double? unitMaterialRough,
            Guid? materialFinishedId,
            double? unitMaterialFinished)
        {
            ConstructionWorkId = constructionWorkId;
            ConstructionWorkName = constructionWorkName;
            LaborId = laborId;
            UnitLabor = unitLabor;
            MaterialRoughId = materialRoughId;
            UnitMaterialRough = unitMaterialRough;
            MaterialFinishedId = materialFinishedId;
            UnitMaterialFinished = unitMaterialFinished;
        }
        public Guid ConstructionWorkId { get; set; }
        public string ConstructionWorkName { get; set; }
        public Guid LaborId { get; set; }
        public double UnitLabor { get; set; }
        public Guid? MaterialRoughId { get; set; }
        public double? UnitMaterialRough { get; set; }
        public Guid? MaterialFinishedId { get; set; }
        public double? UnitMaterialFinished { get; set; }
    }
}
