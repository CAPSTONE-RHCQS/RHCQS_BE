using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.ConstructionWork;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using static RHCQS_BusinessObjects.AppConstant;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Wordprocessing;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructionWorkController : ControllerBase
    {
        private readonly IConstructionWorkService _workService;

        public ConstructionWorkController(IConstructionWorkService workService)
        {
            _workService = workService;
        }

        #region GetListConstructionWork
        /// <summary>
        /// List construction work 
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.ConstructionWork.ConstructionWorkEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListConstructionWork(int page, int size)
        {
            var listConstructions = await _workService.GetListConstructionWork(page, size);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetConstructionWorkDetail
        /// <summary>
        /// Construction work detail by Id
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.ConstructionWork.ConstructionWorkDetailEndpoint)]
        [ProducesResponseType(typeof(ConstructionItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConstructionWorkDetail(Guid workId)
        {
            var listConstructions = await _workService.GetConstructionWorkDetail(workId);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.ConstructionWork.ConstructionWorkByConIdEndpoint)]
        [ProducesResponseType(typeof(List<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListConstructionWorkByConstructionId(Guid constructionId)
        {
            var listConstructions = await _workService.GetListConstructionWorkByConstructionId(constructionId);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };

        }
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.ConstructionWork.ConstructionWorkPriceEndpoint)]
        [ProducesResponseType(typeof(List<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkPriceByWorkId(Guid workId)
        {
            var listConstructions = await _workService.GetConstructionWorkPrice(workId);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };

        }

        #region CreateConstructionWork
        /// <summary>
        /// Creates a new construction work.
        /// </summary>
        /// <param name="request">The construction work creation request.</param>
        /// <returns>A result indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// **Sample Request:**
        /// 
        /// ```json
        /// {
        ///     "workName": "Bê tông nền SX bằng máy trộn, đổ bằng thủ công, M150, đá 1x2, PCB40 Test",
        ///     "constructionId": "43A5F15A-A5AF-4B82-9E20-AFAF16DB6DBA",
        ///     "unit": "m2",
        ///     "code": "AF.81111",
        ///     "resources": [
        ///         {
        ///             "materialSectionId": "BEDD5E81-0D35-4B6A-A12C-368593D42852",
        ///             "materialSectionNorm": 12,
        ///             "laborId": null,
        ///             "laborNorm": 0
        ///         }
        ///     ]
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">The construction work was created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        /// <response code="500">An internal server error occurred.</response>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.ConstructionWork.ConstructionWorkEndpoint)]
        [ProducesResponseType(typeof(List<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateConstructionWork(CreateConstructionWorkRequest request)
        {
            var listConstructions = await _workService.CreateConstructionWork(request);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };

        }

        #region CreateWorkTemplate
        /// <summary>
        /// Creates a new Work Template for a construction work.
        /// </summary>
        /// <param name="request">List of work template requests.</param>
        /// <returns>A result indicating success or failure.</returns>
        /// <remarks>
        /// **Sample Request:**
        /// 
        /// ```json
        /// [
        ///   {
        ///     "constructionWorkId": "6400e868-c11e-49fb-bdd8-62a8adde4022",
        ///     "packageId": "A1625F79-0AF9-4FE6-8021-2F317EE68B0D",
        ///     "laborCost": 0,
        ///     "materialCost": 218040,
        ///     "materialFinishedCost": 0,
        ///     "totalCost": 218040
        ///   }
        /// ]
        /// ```
        /// </remarks>
        /// <response code="200">The work templates were created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        /// <response code="500">An internal server error occurred.</response>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.ConstructionWork.WorkTemplateEndpoint)]
        [ProducesResponseType(typeof(List<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWorkTemplate(List<CreateWorkTemplateRequest> request)
        {
            var listConstructions = await _workService.CreateWorkTemplate(request);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };

        }

        [HttpPost(ApiEndPointConstant.ConstructionWork.ConstructionWorkFileEndpoint)]
        [ProducesResponseType(typeof(List<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportFileConstructionWork(IFormFile file)
        {
            var listConstructions = await _workService.ImportFileConstructionWork(file);
            var result = JsonConvert.SerializeObject(listConstructions, Formatting.Indented);

            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };

        }

        #region FilterConstructionWorkMultiParams
        /// <summary>
        /// Search multi params construction work in website 
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.ConstructionWork.FilterConstructionWorkMultiParamsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<ListConstructionWorkResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterConstructionWorkMultiParams(
            [FromQuery, Required(ErrorMessage = "Page is required")] int page,
            [FromQuery, Required(ErrorMessage = "Size is required")] int size,
            [FromQuery] string? code,
            [FromQuery] string? name,
            [FromQuery] string? unit)
        {
            var listConstructionWorks = await _workService.FilterConstructionWorkMultiParams(page, size,  code, name, unit);
            var result = JsonConvert.SerializeObject(listConstructionWorks, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
