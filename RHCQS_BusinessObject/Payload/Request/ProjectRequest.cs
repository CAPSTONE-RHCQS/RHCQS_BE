using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    using System.ComponentModel.DataAnnotations;
    using RHCQS_BusinessObject.Payload.Request.InitialQuotation;

    public class ProjectRequest
    {
        public Guid? CustomerId { get; set; }

        [Required(ErrorMessage = "Tên dự án là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên dự án không được vượt quá 100 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Loại dự án là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Loại dự án không được vượt quá 50 ký tự.")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [MaxLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự.")]
        public string? Address { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0.")]
        public double? Area { get; set; }

        [Required(ErrorMessage = "Gói báo giá không được bỏ trống.")]
        public List<PackageQuotationRequest> PackageQuotations { get; set; }

        [Required(ErrorMessage = "Báo giá ban đầu là bắt buộc.")]
        public InitialQuotationRequest InitialQuotation { get; set; }

        public List<QuotationUtilitiesRequest>? QuotationUtilitiesRequest { get; set; }
    }

    public class PackageQuotationRequest
    {
        [Required(ErrorMessage = "PackageId là bắt buộc.")]
        public Guid PackageId { get; set; }

        [Required(ErrorMessage = "Loại gói là bắt buộc.")]
        [RegularExpression("ROUGH|FINISHED", ErrorMessage = "Loại gói phải là 'Thô' hoặc 'Hoàn thiện'.")]
        public string? Type { get; set; }
    }

    public class AssignProject
    {
        [Required(ErrorMessage = "AccountId là bắt buộc.")]
        public Guid accountId { get; set; }
        [Required(ErrorMessage = "Báo giá sơ bộ là bắt buộc.")]
        public Guid projectId { get; set; }
    }
}
