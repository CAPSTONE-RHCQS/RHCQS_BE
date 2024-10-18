using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Promotion
{
    public class PromotionRequest
    {

        [Required(ErrorMessage = "Giá trị là bắt buộc.")]
        [Range(1, 100, ErrorMessage = "Khuyến mãi có giá trị > 0 và nhỏ hơn < 100")]
        public int? Value { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc.")]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Thời gian hết hạn là bắt buộc.")]
        public DateTime? ExpTime { get; set; }

    }
}
