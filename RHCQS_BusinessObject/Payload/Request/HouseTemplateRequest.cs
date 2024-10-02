using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class HouseTemplateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? NumberOfFloor { get; set; }

        public int? NumberOfBed { get; set; }

        public int? NumberOfFront { get; set; }

        public string? ImgUrl { get; set; }

        [Required]
        public List<SubTemplatesRequest> SubTemplates { get; set; } = new List<SubTemplatesRequest>();
    }

    public class SubTemplatesRequest
    {
        [Required]
        public Guid Id { get; set; }

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }

        public string? Size { get; set; }

        [Required]
        public List<TemplateItemRequest> TemplateItems { get; set; } = new List<TemplateItemRequest>();
    }

    public class TemplateItemRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ConstructionItemId is required.")]
        public Guid ConstructionItemId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        public double? Area { get; set; }

        public string? Unit { get; set; }
    }
}
