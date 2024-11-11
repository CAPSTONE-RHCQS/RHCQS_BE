using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.DesignTemplate
{
    public class ImageDesignDrawingRequest
    {
        [Required(ErrorMessage = "Hình ảnh tổng quan ngôi nhà là bắt buộc")]
        public IFormFile OverallImage { get; set; } = default!;
        public List<IFormFile> OutSideImage { get; set; } = new List<IFormFile>();
        public List<IFormFile> DesignDrawingImage { get; set; } = new List<IFormFile>();
        public List<IFormFile> PackageFinishedImage { get; set; } = new List<IFormFile>();
        //public List<PackageHouseRequestForCreate> Package {  get; set; }
    }
}
