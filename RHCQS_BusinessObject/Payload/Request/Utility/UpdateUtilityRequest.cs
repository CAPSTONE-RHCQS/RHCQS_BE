using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Payload.Request.Utility
{
    public class UpdateUtilityRequest
    {
        public UpdateUtilityOptionRequest? Utility {  get; set; }
        public UpdateUtilitySectionRequest? Sections { get; set; }

        public UpdateUtilityItemRequest? Items { get; set; }
    }
    public class UpdateUtilityOptionRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
    }

    public class UpdateUtilitySectionRequest
    {
        public Guid? Id { get; set; }

        //[Required(ErrorMessage = "Tên phần là bắt buộc.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá đơn vị phải lớn hơn 0.")]
        public double? UnitPrice { get; set; }

        public string? Unit { get; set; }
    }

    public class UpdateUtilityItemRequest
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        [Required(ErrorMessage = "Hệ số là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hệ số phải lớn hơn 0.")]
        public double? Coefficient { get; set; }
    }
}
