using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.HaveDrawing
{
    public class FileDrawingRequest
    {
        [Required(ErrorMessage = "Bản vẽ phối cảnh là bắt buộc")]
        public List<IFormFile> PerspectiveFile { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Bản vẽ kiến trúc là bắt buộc")]
        public List<IFormFile> ArchitectureFile { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Bản vẽ kết cấu là bắt buộc")]
        public List<IFormFile> StructureFile { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Bản vẽ điện nước là bắt buộc")]
        public List<IFormFile> ElectricityWaterFile { get; set; } = new List<IFormFile>();
    }
}
