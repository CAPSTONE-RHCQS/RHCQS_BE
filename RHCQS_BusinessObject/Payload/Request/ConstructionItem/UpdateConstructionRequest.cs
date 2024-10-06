using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.ConstructionItem
{
    public class UpdateConstructionRequest
    {
        [Required(ErrorMessage = "Tên hạng mục là bắt buộc")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Hệ số là bắt buộc")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Hệ số phải lớn hơn 0.0")]
        public double? Coefficient { get; set; }

        public List<SubContructionUpdateRequest>? SubRequests { get; set; }
    }

    public class SubContructionUpdateRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Hệ số là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hệ số phải lớn hơn 0.0")]

        public double? Coefficient { get; set; }
    }
}
