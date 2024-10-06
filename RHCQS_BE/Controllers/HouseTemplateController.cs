using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseTemplateController : ControllerBase
    {
        private readonly IHouseTemplateService _houseService;

        public HouseTemplateController(IHouseTemplateService houseService)
        {
            _houseService = houseService;
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
        /// Creates a new house tempalte.
        /// </summary>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateHouseTemplate([FromBody] HouseTemplateRequest templ)
        {
            var isCreate = await _houseService.CreateHouseTemplate(templ);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
        #region UpdateHouseTemplate
        /// <summary>
        /// Update a house tempalte.
        /// </summary>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateHouseTemplate([FromBody] HouseTemplateRequest templ)
        {
            var update = await _houseService.UpdateHouseTemplate(templ);
            return Ok(update);
        }
    }
}
