using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.HouseDesign
{
    public class HouseDesignDrawingRequest
    {
        public Guid ProjectId { get; set; }

        public Guid DesignerPerspective {get; set;}

        public Guid DesignerArchitecture {  get; set;}

        public Guid DesignerStructure { get; set;}

        public Guid DesignerElectricityWater { get; set;}

        //public bool? IsCompany { get; set; }
    }
}
