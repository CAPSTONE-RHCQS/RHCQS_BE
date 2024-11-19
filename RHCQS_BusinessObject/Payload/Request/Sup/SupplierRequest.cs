using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Sup
{
    public class SupplierRequest
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ConstractPhone { get; set; }
        [JsonIgnore]
        public string? ImgUrl { get; set; }

        public bool? Deflag { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }
        public string? Code { get; set; }

        [JsonIgnore]
        public IFormFile? Image { get; set; }
    }
}
