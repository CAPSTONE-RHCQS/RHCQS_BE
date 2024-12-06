using RHCQS_BusinessObject.Helper;
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
        [Required(ErrorMessage = "Tên chủ đầu tư là bắt buộc.")]
        [NotEmptyOrWhitespace(ErrorMessage = "Tên chủ đầu tư không được để trống hoặc chỉ chứa khoảng trắng.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Mẫu diện tích nhà là bắt buộc.")]
        public Guid SubTemplateId { get; set; }

        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gói thi công hoàn thiện là bắt buộc.")]
        public Guid PackageFinished { get; set; }

        public List<QuotationUtilitiesRequest>? QuotationUtilitiesRequest { get; set; }
    }
}
