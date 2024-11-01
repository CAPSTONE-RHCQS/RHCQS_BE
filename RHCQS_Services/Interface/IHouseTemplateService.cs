using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.DesignTemplate;
using RHCQS_BusinessObject.Payload.Response;
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
        public Task<IPaginate<HouseTemplateResponse>> GetListHouseTemplateAsync(int page, int size);
        public Task<IPaginate<HouseTemplateResponseCustom>> GetListHouseTemplateForShortVersionAsync(int page, int size);
        public Task<List<HouseTemplateResponse>> GetListHouseTemplate();
        Task<HouseTemplateResponse> SearchHouseTemplateByNameAsync(string name);
        Task<HouseTemplateResponse> GetHouseTemplateDetail(Guid id);
        Task<DesignTemplate> UpdateHouseTemplate(HouseTemplateRequestForUpdate templ, Guid id);
        Task<Guid> CreateHouseTemplate(HouseTemplateRequestForCreate templ);
        //Task<bool> CreateSubTemplate(TemplateRequestForCreateArea request);
        Task<bool> CreateImageDesignTemplate(Guid designTemplateId, ImageDesignDrawingRequest files);
        Task<bool> CreatePackageHouse(PackageHouseRequest request);
        Task<string> UploadFileNoMedia(IFormFile file, string folder);
    }
}
