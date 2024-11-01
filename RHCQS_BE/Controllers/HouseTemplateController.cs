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
                NumberOfFront = housetemplate.NumberOfFront,
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
        ///     POST /api/housetemplate
        ///     {
        ///         "name": "Mẫu test",
        ///         "description": "Test mẫu nhà",
        ///         "numberOfFloor": 2,
        ///         "numberOfBed": 3,
        ///         "numberOfFront": 0,
        ///         "subTemplates": [
        ///             {
        ///                 "buildingArea": 200,
        ///                 "floorArea": 120,
        ///                 "size": "R8 x D15",
        ///                 "templateItems": [
        ///                     {
        ///                         "constructionItemId": "BE6C6DB7-CEA1-4275-9B18-2FBCFE9B2353",
        ///                         "subConstructionItemId": "00000000-0000-0000-0000-000000000000",
        ///                         "name": "Trệt",
        ///                         "area": 120,
        ///                         "unit": "m2"
        ///                     },
        ///                     {
        ///                         "constructionItemId": "EBA29420-A8DB-455C-86B0-B325A1DA4E1E",
        ///                         "subConstructionItemId": "00000000-0000-0000-0000-000000000000",
        ///                         "name": "Lầu 1",
        ///                         "area": 80,
        ///                         "unit": "m2"
        ///                     }
        ///                 ]
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The house template details to be created.</param>
        /// <returns>Returns `true` if the creation is successful, otherwise `false`.</returns>
        /// <response code="200">Returns `true` indicating successful creation of the house template.</response>
        /// <response code="400">Returns the validation errors or `false` if creation failed.</response>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
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


        //#region CreateSubTemplate
        //#endregion
        ////[Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        //[HttpPost(ApiEndPointConstant.HouseTemplate.SubTemplateDesignEndpoint)]
        //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        //public async Task<IActionResult> CreateSubTemplate([FromBody] TemplateRequestForCreateArea request)
        //{
        //    var isCreate = await _houseService.CreateSubTemplate(request);
        //    return isCreate ? Ok(isCreate) : BadRequest();
        //}


        #region UpdateHouseTemplate
        /// <summary>
        /// Update a house tempalte.
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateHouseTemplate([FromBody] HouseTemplateRequestForUpdate templ, Guid id)
        {
            if (!string.IsNullOrEmpty(templ.ImgURL))
            {
                string imageUrl = await _uploadImgService.UploadImageAsync(templ.ImgURL, "DesignHouse");
                templ.ImgURL = imageUrl;
            }
            foreach (var exterior in templ.ExteriorsUrls)
            {
                if (!string.IsNullOrEmpty(exterior.MediaImgURL))
                {
                    string mediaUrl = await _uploadImgService.UploadImageAsync(exterior.MediaImgURL, "DesignHouse");
                    exterior.MediaImgURL = mediaUrl;
                }
            }
            foreach (var subTemplate in templ.SubTemplates)
            {
                foreach (var media in subTemplate.Designdrawings)
                {
                    if (!string.IsNullOrEmpty(media.Name))
                    {
                        string mediaUrl = await _uploadImgService.UploadImageAsync(media.MediaImgURL!, "DesignHouse");
                        media.MediaImgURL = mediaUrl;
                    }
                }
            }
            var update = await _houseService.UpdateHouseTemplate(templ, id);
            return Ok(update);
        }

        #region CreateImageDesignTemplate
        /// <summary>
        /// Upload Image drawing house template
        /// 
        /// Role: MANAGER
        /// </summary>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.UploadImageDrawingEndpoint)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImageDesignTemplate([FromQuery] Guid designTemplateId, [FromForm] ImageDesignDrawingRequest request)
        {
            if (request.OverallImage == null && (request.OutSideImage == null || request.OutSideImage.Count == 0)
                && (request.DesignDrawingImage == null || request.DesignDrawingImage.Count == 0))
            {
                return BadRequest("At least one image file is required.");
            }

            try
            {
                var isUploaded = await _houseService.CreateImageDesignTemplate(designTemplateId, request);
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
    }
}
