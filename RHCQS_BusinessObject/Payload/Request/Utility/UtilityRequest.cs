using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_BusinessObject.Payload.Request.Utility
{
    public class UtilityRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public List<UtilitySectionRequest>? Sections { get; set; }

        public List<UtilityItemRequest>? Items { get; set; }
    }

    public class UtilitySectionRequest
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Tên phần là bắt buộc.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá đơn vị phải lớn hơn 0.")]
        public double? UnitPrice { get; set; }

        public string? Unit { get; set; }
    }

    public class UtilityItemRequest
    {
        [Required(ErrorMessage = "Tên mục là bắt buộc.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Hệ số là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hệ số phải lớn hơn 0.")]
        public double? Coefficient { get; set; }
    }
}
