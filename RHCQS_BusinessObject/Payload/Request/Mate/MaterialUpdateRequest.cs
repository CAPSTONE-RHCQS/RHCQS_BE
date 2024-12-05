using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Mate
{
    public class MaterialUpdateRequest
    {
        public Guid? SupplierId { get; set; }
        public Guid? MaterialSectionId { get; set; }
        public string? Name { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal? Price { get; set; }
        public string? Unit { get; set; }
        public string? Size { get; set; }
        public string? Shape { get; set; }
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public bool? IsAvailable { get; set; }
        public string? UnitPrice { get; set; }
        [MaxLength(50, ErrorMessage = "Mã Code không được vượt quá 10 ký tự.")]
        public string? Code { get; set; }
        public string? Type { get; set; }
        [JsonIgnore]
        public IFormFile? Image { get; set; }
    }
}
