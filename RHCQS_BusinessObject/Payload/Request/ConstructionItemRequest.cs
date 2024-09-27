using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class ConstructionItemRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Coefficient is required.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Coefficient must be greater than 0.")]
        public double? Coefficient { get; set; }


        [Required(ErrorMessage = "Unit is required.")]
        public string? Unit { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; }    
        public List<SubConstructionRequest>? subConstructionRequests { get; set; }

    }

    public class SubConstructionRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Coefficient is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be greater than 0.")]

        public double? Coefficient { get; set; }
        [Required(ErrorMessage = "Unit is required.")]
        public string? Unit { get; set; }
    }
}
