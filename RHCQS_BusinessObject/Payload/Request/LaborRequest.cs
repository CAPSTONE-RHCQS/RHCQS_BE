using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class LaborRequest
    {
        public string? Name { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public double? Price { get; set; }

        public bool? Deflag { get; set; }

        public string? Type { get; set; }
        [MaxLength(50, ErrorMessage = "Mã Code không được vượt quá 10 ký tự.")]
        public string? Code { get; set; }
    }
}
