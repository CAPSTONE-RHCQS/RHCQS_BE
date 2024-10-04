using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IHouseDesignVersionService
    {
        Task<bool> CreateHouseDesignVersion(HouseDesignVersionRequest request);
        Task<bool> UploadDesignDrawing(List<IFormFile> files, Guid versionId);
    }
}
