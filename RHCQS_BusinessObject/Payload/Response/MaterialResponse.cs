using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class MaterialResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public double? Price { get; set; }
        public string Unit { get; set; }
        public string Size { get; set; }
        public string Shape { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public bool? IsAvailable { get; set; }
        public string UnitPrice { get; set; }
        public Guid? MaterialSectionId { get; set; }
        public Guid SupplierId { get; set; }
        public string MaterialSectionName { get; set; }
        public string SupplierName { get; set; }
        public string? Code { get; set; }
        public string? Type { get; set; }
    }
}
