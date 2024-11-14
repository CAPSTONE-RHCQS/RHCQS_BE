using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.DesignTemplate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using static RHCQS_BE.Extenstion.ApiEndPointConstant;
using Account = CloudinaryDotNet.Account;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseTemplateController : ControllerBase
    {
        private readonly IHouseTemplateService _houseService;
        private readonly IUploadImgService _uploadImgService;
        public HouseTemplateController(IHouseTemplateService houseService, IUploadImgService uploadImgService)
        {
            _houseService = houseService;
            _uploadImgService = uploadImgService;
        }
        #region GetListHouseTemplates
        /// <summary>
        /// Retrieves the list of all housetemplate.
        /// </summary>
        /// <returns>List of housetemplate in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(HouseTemplateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListHouseTemplate(int page, int size)
        {
            var listHouseTemplates = await _houseService.GetListHouseTemplateAsync(page, size);
            var result = JsonConvert.SerializeObject(listHouseTemplates, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region GetListHouseTemplates
        /// <summary>
        /// Retrieves the list of all housetemplate shorter version response.
        /// </summary>
        /// <returns>List of housetemplate in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseTemplate.HouseTemplateShorterEndpoint)]
        [ProducesResponseType(typeof(HouseTemplateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListHouseTemplateForShorterResponse(int page, int size)
        {
            var listHouseTemplates = await _houseService.GetListHouseTemplateForShortVersionAsync(page, size);
            var result = JsonConvert.SerializeObject(listHouseTemplates, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region GetListHouseTemplate
        /// <summary>
        /// Retrieves housetemplate list without entering page and size.
        /// </summary>
        /// <returns>List of housetemplate in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseTemplate.HouseTemplateListEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<HouseTemplateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DesignTemplate>>> GetListPackage()
        {
            var listHouseTemplates = await _houseService.GetListHouseTemplate();
            var response = JsonConvert.SerializeObject(listHouseTemplates, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region SearchHouseTemplate
        /// <summary>
        /// Search housetemplate by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The housetemplate that match the search criteria.</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseTemplate.SearchHouseTemplateEndpoint)]
        public async Task<ActionResult<HouseTemplateResponse>> SearchHouseTemplateByName(string name)
        {
            var housetemplate = await _houseService.SearchHouseTemplateByNameAsync(name);

            var searchHouseTempalte = new HouseTemplateResponse
            {
                Id = housetemplate.Id,
                Name = housetemplate.Name,
                Description = housetemplate.Description,
                NumberOfBed = housetemplate.NumberOfBed,
                NumberOfFloor = housetemplate.NumberOfFloor,
                ImgUrl = housetemplate.ImgUrl,
                InsDate = housetemplate.InsDate,
            };
            var result = JsonConvert.SerializeObject(searchHouseTempalte, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region SearchHouseTemplate
        /// <summary>
        /// Get detailhousetemplate by id.
        /// </summary>
        /// <param id="id">The id to get for.</param>
        /// <returns>The housetemplate match with id.</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.HouseTemplate.HouseTemplateDetail)]
        public async Task<ActionResult<HouseTemplateResponse>> GetHouseTemplateDetail(Guid id)
        {
            var housetemplate = await _houseService.GetHouseTemplateDetail(id);
            var result = JsonConvert.SerializeObject(housetemplate, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region CreateHouseTemplate
        /// <summary>
        /// Creates a new house template.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// ```json
        /// POST /api/housetemplate
        /// {
        ///   "name": "Sample Template",
        ///   "description": "Test house template from Puanticuocsong",
        ///   "numberOfFloor": 2,
        ///   "numberOfBed": 3,
        ///   "packageRoughId": "AA5057D8-9B30-4F17-A8EE-7AAD655B63CC",
        ///   "descriptionPackage": "Test house template API",
        ///   "subTemplates": [
        ///     {
        ///       "buildingArea": 200,
        ///       "floorArea": 120,
        ///       "size": "R8 x D15",
        ///       "templateItems": [
        ///         {
        ///           "constructionItemId": "BE6C6DB7-CEA1-4275-9B18-2FBCFE9B2353",
        ///           "subConstructionItemId": null,
        ///           "name": "Ground Floor",
        ///           "area": 120,
        ///           "unit": "m2"
        ///         },
        ///         {
        ///           "constructionItemId": "EBA29420-A8DB-455C-86B0-B325A1DA4E1E",
        ///           "subConstructionItemId": null,
        ///           "name": "First Floor",
        ///           "area": 80,
        ///           "unit": "m2",
        ///           "price": 180000000
        ///         }
        ///       ]
        ///     }
        ///   ],
        ///   "packageFinished": [
        ///     {
        ///       "packageId": "ABEEDDF8-487D-4DEA-AFB9-173B3FEB0338",
        ///       "description": "Economical finishing package"
        ///     }
        ///   ]
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">The house template details to be created.</param>
        /// <returns>Returns `true` if the creation is successful, otherwise `false`.</returns>
        /// <response code="200">Returns `true` indicating successful creation of the house template.</response>
        /// <response code="400">Returns the validation errors or `false` if creation failed.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateHouseTemplate([FromBody] HouseTemplateRequestForCreate request)
        {
            var housetemplate = await _houseService.CreateHouseTemplate(request);
            var result = JsonConvert.SerializeObject(housetemplate, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region UpdateSubTemplate
        /// <summary>
        /// Update sub-template with template items.
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// Request mẫu:
        /// 
        /// ```json
        /// {
        ///   "buildingArea": 12,
        ///   "floorArea": 12,
        ///   "size": "D14xR9",
        ///   "templateItems": [
        ///     {
        ///       "constructionItemId": "75922602-9153-4cc3-a7dc-225c9bc30a5e",
        ///       "subConstructionItemId": null,
        ///       "name": "Lầu 2",
        ///       "area": 10,
        ///       "unit": "m2",
        ///       "price: 180000000
        ///     }
        ///   ]
        /// }
        /// ```
        /// </remarks>
        /// <param name="subTemplateId">ID của sub template cần được cập nhật</param>
        /// <param name="request">Yêu cầu cập nhật thông tin cho sub template</param>
        /// <returns>Trả về thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu yêu cầu không hợp lệ hoặc cập nhật thất bại</response>
        /// <response code="401">Không có quyền truy cập</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.HouseTemplate.SubTemplateDesignDetailEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSubTemplate([FromQuery]Guid subTemplateId, [FromBody] UpdateSubTemplateRequest request)
        {
            var result = await _houseService.UpdateSubTemplate(subTemplateId, request);
            return Ok(result);
        }


        #region UpdateHouseTemplate
        /// <summary>
        /// Update a house tempalte.
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.HouseTemplate.HouseTemplateDetail)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateHouseTemplate([FromForm] HouseTemplateRequestForUpdate request, Guid id)
        {
            var update = await _houseService.UpdateHouseTemplate(request, id);
            return Ok(update);
        }

        #region CreateImageDesignTemplate
        /// <summary>
        /// Upload Image drawing house template
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <remarks>
        /// 
        /// JSON: 
        /// ```json
        /// [
        ///   {
        ///     "packageId": "ABEEDDF8-487D-4DEA-AFB9-173B3FEB0338",
        ///     "description": "Gói hoàn thiện cơ bản"
        ///   },
        ///   {
        ///     "packageId": "A1625F79-0AF9-4FE6-8021-2F317EE68B0D",
        ///     "description": "Gói hoàn thiện tiêu chuẩn"
        ///   }
        /// ]
        /// ```
        /// </remarks>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.UploadImageDrawingEndpoint)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImageDesignTemplate([FromQuery] Guid designTemplateId, 
                                        [FromForm] ImageDesignDrawingRequest request, [FromForm] string packageJson)
        {
            if (request.OverallImage == null && (request.OutSideImage == null || request.OutSideImage.Count == 0)
                && (request.DesignDrawingImage == null || request.DesignDrawingImage.Count == 0))
            {
                return BadRequest("At least one image file is required.");
            }

            try
            {
                var package = JsonConvert.DeserializeObject<List<PackageHouseRequestForCreate>>(packageJson);
                var isUploaded = await _houseService.CreateImageDesignTemplate(designTemplateId, request, package);
                if (isUploaded)
                {
                    return Ok("Images uploaded successfully.");
                }
                else
                {
                    return BadRequest("Image upload failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading images: {ex.Message}");
            }
        }

        #region CreatePackageHouse
        /// <summary>
        /// Creates a new package for a house design template.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// ```json
        /// {
        ///   "packageId": "5245F485-C546-4051-B04E-954FD773811E",
        ///   "designTemplateId": "AC0E955C-3F03-4788-9DB8-C9C32069FC3F",
        ///   "description": "This price applies to standard urban villas. It includes construction area over 365 m², with 1 floor and 2 rooms. Note that the listed price does not include VAT.",
        ///   "packageHouseImage": "http://res.cloudinary.com/de7pulfdj/image/upload/v1730463742/PackageHouse/Flexible_package.png"
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">An object containing details for creating the house package</param>
        /// <returns>A URL of the created package house image if successful</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.PackageHouseEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePackageHouse([FromBody] PackageHouseRequest request)
        {
            var housetemplate = await _houseService.CreatePackageHouse(request);
            var result = JsonConvert.SerializeObject(housetemplate, Formatting.Indented);
            return new ContentResult
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region UploadImgPackageHouse
        /// <summary>
        /// Upload image package house design template - package finished
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.UploadImagePackHouseEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadImgPackageHouse(IFormFile request)
        {
            var imgUrl = await _houseService.UploadFileNoMedia(request, "PackageHouse");
            return Ok(imgUrl);
        }

        #region UploadImageSubTemplate
        /// <summary>
        /// Upload image sub house design template
        /// 
        /// Role: MANAGER
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.HouseTemplate.UploadImageSubTemplateEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadImageSubTemplate(Guid subTempateId, IFormFile request)
        {
            var imgUrl = await _houseService.UploadImageSubTemplate(subTempateId, request);
            return Ok(imgUrl);
        }
    }
}
