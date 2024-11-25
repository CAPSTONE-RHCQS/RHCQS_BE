using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Implement;
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

        #region ListDesignStaffWorkAvailable
        /// <summary>
        /// Retrieves a list of available design staff work assignments.
        /// 
        /// Role: MANAGER
        /// </summary>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.AssignTask.AssignTaskDesignStaffAvailableEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListDesignStaffWorkAvailable(int page, int size)
        {
            var list = await _assignService.ListDesignStaffWorkAvailable(page, size);
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ListStaffSalesAvailable
        /// <summary>
        /// Retrieves a list of available sale staff work assignments.
        /// 
        /// Role: MANAGER
        /// </summary>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.AssignTask.AssignTaskSaleStaffAvailableEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListStaffSalesAvailable(int page, int size)
        {
            var list = await _assignService.ListStaffSalesAvailable(page, size);
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
