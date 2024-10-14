using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class HouseTemplateRequest
    {

        [Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public int? NumberOfFront { get; set; }

        public string? ImgUrl { get; set; }

        [Required(ErrorMessage = "Danh sách mẫu phụ là bắt buộc phải có.")]
        public List<SubTemplatesRequest> SubTemplates { get; set; } = new List<SubTemplatesRequest>();
    }

    public class SubTemplatesRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid Id { get; set; }

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public string? Size { get; set; }

        [Required(ErrorMessage = "Danh sách mục mẫu là bắt buộc phải có.")]
        public List<TemplateItemRequest> TemplateItems { get; set; } = new List<TemplateItemRequest>();
        public List<MediaRequest> Media { get; set; } = new List<MediaRequest>();
    }
    public class MediaRequest
    {
        public string? Name { get; set; }

        public string? Url { get; set; }
    }
    public class TemplateItemRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc phải có.")]
        public Guid ConstructionItemId { get; set; }
        [Required(ErrorMessage = "SubConstructionItemId là bắt buộc phải có.")]
        public Guid SubConstructionItemId { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public double? Area { get; set; }

        public string? Unit { get; set; }
    }
}
