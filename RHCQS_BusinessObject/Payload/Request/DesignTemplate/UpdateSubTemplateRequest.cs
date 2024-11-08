using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.DesignTemplate
{
    public class UpdateSubTemplateRequest
    {

        public double? BuildingArea { get; set; }

        public double? FloorArea { get; set; }
        public string? Size { get; set; }

        public double? TotalRough { get; set; }

        [Required(ErrorMessage = "Danh sách mục mẫu là bắt buộc phải có.")]
        public List<TemplateItemRequestForUpdate> TemplateItems { get; set; } = new List<TemplateItemRequestForUpdate>();
    }

    public class TemplateItemRequestForUpdate
    {

        [Required(ErrorMessage = "ConstructionItemId là bắt buộc phải có.")]
        public Guid ConstructionItemId { get; set; }
        public Guid? SubConstructionItemId { get; set; }
        public string Name { get; set; } = null!;

        public double? Area { get; set; }

        public string? Unit { get; set; }
    }
}
