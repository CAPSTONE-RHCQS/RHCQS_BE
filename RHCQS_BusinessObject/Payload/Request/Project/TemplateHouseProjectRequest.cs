using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Project
{
    public class TemplateHouseProjectRequest
    {
        [Required(ErrorMessage = "Mẫu diện tích nhà là bắt buộc.")]
        public Guid SubTemplateId { get; set; }

        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gói thi công hoàn thiện là bắt buộc.")]
        public Guid PackgeFinsihed { get; set; }

        public List<QuotationUtilitiesRequest>? QuotationUtilitiesRequest { get; set; }
    }
}
