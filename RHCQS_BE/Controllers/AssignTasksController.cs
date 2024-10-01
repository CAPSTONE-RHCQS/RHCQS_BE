using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignTasksController : ControllerBase
    {
        private readonly IAssignTaskService _assignService;

        public AssignTasksController(IAssignTaskService assignService)
        {
            _assignService = assignService;
        }

        #region GetListTask
        /// <summary>
        /// Retrieves the list of all task item.
        /// </summary>
        /// <returns>List of task in the system</returns>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.AssignTask.AssignTaskEndpoint)]
        [ProducesResponseType(typeof(AssignTaskResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListTask(int page, int size)
        {
            var listAssignTask = await _assignService.GetListAssignTaskAll(page, size);
            var result = JsonConvert.SerializeObject(listAssignTask, Formatting.Indented);
            return Ok(result);
        }

        #region AssignWork
        /// <summary>
        /// Creates a new construction item and its sub-items in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/construction
        ///     {
        ///       "name": "Mái phụ",
        ///       "coefficient": 0,
        ///       "unit": "m2",
        ///       "type": "ROUGH",
        ///       "subConstructionRequests": [
        ///         {
        ///           "name": "Mái BTCT",
        ///           "coefficient": 0.5,
        ///           "unit": "m2"
        ///         },
        ///         {
        ///           "name": "Mái Tole",
        ///           "coefficient": 0.3,
        ///           "unit": "m2"
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="item">Construction item request model</param>
        /// <returns>Returns true if the construction item is created successfully, otherwise false.</returns>
        /// <response code="200">Construction item created successfully</response>
        /// <response code="400">Failed to create the construction item</response>
        /// 
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.AssignTask.AssignTaskEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignTask([FromBody]List<AssignTaskRequest>  item)
        {
            var isCreate = await _assignService.AssignWork(item);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
