using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class PackageTypeRequest
    {
        [Required(ErrorMessage = "Name là bắt buộc phải có.")]
        public string? Name { get; set; }
    }
}
