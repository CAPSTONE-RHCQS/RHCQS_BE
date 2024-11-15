using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.DesignTemplate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.HouseTemplate;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IHouseTemplateService
    {
        Task<IPaginate<IPaginateHouseTemplateResponse>> GetListHouseTemplateAsync(int page, int size);
        public Task<IPaginate<HouseTemplateResponseCustom>> GetListHouseTemplateForShortVersionAsync(int page, int size);
        public Task<List<HouseTemplateResponse>> GetListHouseTemplate();
        Task<HouseTemplateResponse> SearchHouseTemplateByNameAsync(string name);
        Task<HouseTemplateResponse> GetHouseTemplateDetail(Guid id);
        Task<string> UpdateHouseTemplate(HouseTemplateRequestForUpdate templ, Guid id);
        Task<Guid> CreateHouseTemplate(HouseTemplateRequestForCreate templ);
        Task<string> UpdateSubTemplate(Guid subTemplateId, UpdateSubTemplateRequest request);
        Task<bool> CreateImageDesignTemplate(Guid designTemplateId, ImageDesignDrawingRequest files, List<PackageHouseRequestForCreate> package);
        Task<bool> CreatePackageHouse(PackageHouseRequest request);
        Task<string> UploadFileNoMedia(IFormFile file, string folder);
        Task<string> UploadImageSubTemplate(Guid subTemplateId, IFormFile file);
        Task<string> UploadImageOutSide(IFormFile image, Guid designTemplateId);
        Task<string> UploadImagePackageHouse(Guid packageId, IFormFile file);
    }
}
