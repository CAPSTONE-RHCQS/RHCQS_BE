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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region AssignWork
        /// <summary>
        /// Assigns work tasks to a list of accounts for a specific house design drawing.
        /// </summary>
        /// <remarks>
        /// This endpoint allows managers to assign tasks to multiple accounts related to a house design drawing.
        /// Each task includes the account ID and the house design drawing ID that the account will work on.
        ///
        /// **Sample Request:**
        ///
        ///     POST /api/v1/assign
        ///     [
        ///         {
        ///             "accountId": "BF339E88-5303-45C4-A6F4-33A79681766C",
        ///             "houseDesignDrawingId": "bB528F8BC-3992-499B-AF7D-67510D730087"
        ///         },
        ///         {
        ///             "accountId": "990773A2-1817-47F5-9116-301E97435C44",
        ///             "houseDesignDrawingId": "a09ed901-92c9-4c5e-950b-d9c5a7a99f4c"
        ///         }
        ///     ]
        ///
        /// **Request Fields:**
        /// - **accountId** (Guid, Required): The unique identifier of the account being assigned the task.
        /// - **houseDesignDrawingId** (Guid, Required): The unique identifier of the house design drawing associated with the task.
        ///
        /// </remarks>
        /// <param name="item">A list of assign task request models containing account and house design drawing details</param>
        /// <returns>Returns true if the tasks are assigned successfully, otherwise false.</returns>
        /// <response code="200">Tasks assigned successfully</response>
        /// <response code="400">Failed to assign tasks due to validation errors or missing required fields</response>
        /// <response code="401">Unauthorized, only managers are allowed to assign tasks</response>
        /// <response code="500">Internal server error</response>
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
