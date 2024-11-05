using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class SupplierRequest
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ConstractPhone { get; set; }

        public string? ImgUrl { get; set; }

        public bool? Deflag { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
    }
}
