using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class SupplierResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ConstractPhone { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
    }
}
