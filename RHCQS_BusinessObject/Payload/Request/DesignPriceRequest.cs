using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class DesignPriceRequest
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Diện tích nhập phải lớn hơn 0.")]
        public double? AreaFrom { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Diên tích nhập phải lớn hơn 0.")]
        public double? AreaTo { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public double? Price { get; set; }
    }
}
