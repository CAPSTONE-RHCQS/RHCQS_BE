using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class HouseTemplateRequestForUpdate
    {

        [Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public int? NumberOfFront { get; set; }

        public string? ImgURL { get; set; }

        [Required(ErrorMessage = "Danh sách mẫu phụ là bắt buộc phải có.")]
        public List<SubTemplatesRequest> SubTemplates { get; set; } = new List<SubTemplatesRequest>();
        public List<MediaRequest> ExteriorsUrls { get; set; } = new List<MediaRequest>();
    }

    public class SubTemplatesRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid Id { get; set; }

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public string? Size { get; set; }

        public string? ImgURL { get; set; }

        [Required(ErrorMessage = "Danh sách mục mẫu là bắt buộc phải có.")]
        public List<TemplateItemRequest> TemplateItems { get; set; } = new List<TemplateItemRequest>();
        public List<MediaRequest> Designdrawings { get; set; } = new List<MediaRequest>();
    }
    public class MediaRequest
    {
        [JsonIgnore]
        public string? Name { get; set; }

        public string? MediaImgURL { get; set; }

    }
    public class TemplateItemRequest
    {
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc phải có.")]
        public Guid ConstructionItemId { get; set; }

        public Guid SubConstructionItemId { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public double? Area { get; set; }

        public string? Unit { get; set; }
    }

    public class HouseTemplateRequestForCreate
    {

        [Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        [Required(ErrorMessage = "Gói thi công là bắt buộc phải có.")]
        public Guid PackageRoughId { get; set; }
        public string? DescriptionPackage { get; set; }

        [Required(ErrorMessage = "Danh sách mẫu phụ là bắt buộc phải có.")]
        public List<SubTemplatesRequestForCreate> SubTemplates { get; set; } = new List<SubTemplatesRequestForCreate>();
    }
    public class SubTemplatesRequestForCreate
    {
        [Required(ErrorMessage = "Diện tích xây dựng là bắt buộc phải có.")]
        public double BuildingArea { get; set; }
        [Required(ErrorMessage = "Số lầu là bắt buộc phải có.")]
        public double FloorArea { get; set; }
        [Required(ErrorMessage = "Kích thước là bắt buộc phải có.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc phải có.")]
        public double TotalRough { get; set; }

        [Required(ErrorMessage = "Danh sách mục mẫu là bắt buộc phải có.")]
        public List<TemplateItemRequestForCreate> TemplateItems { get; set; } = new List<TemplateItemRequestForCreate>();
    }
    public class TemplateItemRequestForCreate
    {

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc phải có.")]
        public Guid ConstructionItemId { get; set; }
        public Guid? SubConstructionItemId { get; set; }

        //[Required(ErrorMessage = "Tên là bắt buộc phải có.")]
        public string Name { get; set; } = null!;

        public double? Area { get; set; }

        public string? Unit { get; set; }
    }

}
