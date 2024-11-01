using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IUploadImgService
    {
        Task<string> UploadImageAsync(string imagePathOrUrl, string folder);
        Task<string> UploadFile(Guid designTemplateId, IFormFile file, string folder, string nameImage);
        Task<string> UploadFileForImageAccount(Guid accountid, IFormFile file, string folder, string nameImage);
        Task<List<string>> UploadImageDesignTemplate(List<IFormFile> files);
    }
}
