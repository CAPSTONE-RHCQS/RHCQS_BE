using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.ConstructionWork
{
    public class UpdateConstructionWorkRequest
    {
        public string? NameConstructionWork {  get; set; }
        public List<UpdateConstructionWorkResourceRequest> Resources {  get; set; }
    }
}
