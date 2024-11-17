using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Mate
{
    public class MaterialRequest
    {
        public Guid SupplierId { get; set; }
        public Guid? MaterialSectionId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string? Unit { get; set; }
        public string? Size { get; set; }
        public string? Shape { get; set; }
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public bool? IsAvailable { get; set; }
        public string UnitPrice { get; set; }
        [JsonIgnore]
        public IFormFile? Image { get; set; }
    }
}
