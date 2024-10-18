using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Promotion
{
    
    public class UpdatePromotionRequest
    {
        [Range(1, 100, ErrorMessage = "Khuyến mãi có giá trị > 0 và nhỏ hơn < 100")]
        public int? Value { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Name { get; set; }

        public DateTime? ExpTime { get; set; }
    }
}
