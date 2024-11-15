using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IMediaService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName, string publicId);
        Task<string> UploadImageSubTemplate(IFormFile file, string folder);
        Task<string> UploadFileAsync(IFormFile file, string folderName, string publicId = null);
        string GetImageUrl(ImageUploadResult result);
    }
}
