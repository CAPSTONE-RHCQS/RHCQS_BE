using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.MateSec
{
    public class MaterialSectionRequest
    {
        public string? Name { get; set; }
        public string Code { get; set; }
        public string? Type { get; set; }
    }
}
