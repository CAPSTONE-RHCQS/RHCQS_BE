using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_Services.Interface;

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
    }
}
