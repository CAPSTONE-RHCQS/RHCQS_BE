using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
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
        [HttpGet(ApiEndPointConstant.HouseTemplate.HouseTemplateEndpoint)]
        [ProducesResponseType(typeof(HouseTemplateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListHouseTemplate(int page, int size)
        {
            var listHouseTemplates = await _houseService.GetListHouseTemplateAsync(page, size);
            var result = JsonConvert.SerializeObject(listHouseTemplates, Formatting.Indented);
            return Ok(result);
        }
        #region SearchHouseTemplate
        /// <summary>
        /// Search housetemplate by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The housetemplate that match the search criteria.</returns>
        // GET: api/Account/Search
        #endregion
        [HttpGet(ApiEndPointConstant.HouseTemplate.SearchHouseTemplateEndpoint)]
        public async Task<ActionResult<DesignTemplate>> SearchHouseTemplateByName(string name)
        {
            var housetemplate = await _houseService.SearchHouseTemplateByNameAsync(name);
            if (housetemplate == null)
            {
                return NotFound();
            }

            var searchHouseTempalte = new DesignTemplate
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
            return Ok(result);
        }
    }
}
