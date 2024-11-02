using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class ImageForAccount
    {
        [Required(ErrorMessage = "Hình ảnh là bắt buộc")]
        public IFormFile AccountImage { get; set; }
    }
}
