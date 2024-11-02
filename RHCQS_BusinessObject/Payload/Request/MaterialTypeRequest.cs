using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class MaterialTypeRequest
    {
        public string? Name { get; set; }

        public bool? Deflag { get; set; }
    }
}
