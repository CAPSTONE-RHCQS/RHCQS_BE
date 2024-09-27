using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        #region GetProjects
        /// <summary>
        /// Retrieves the list of all project.
        /// </summary>
        /// <returns>List of project in the system</returns>
        #endregion
        [HttpGet(ApiEndPointConstant.Project.ProjectEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjects(int page, int size)
        {
            var listProjects = await _projectService.GetProjects(page, size);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return Ok(result);
        }

        #region GetDetailProjectById
        /// <summary>
        /// Retrieves the details of a specific project by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>A detailed project response including project information such as account, name, type, status, and more.</returns>
        /// <response code="200">Returns the project details successfully.</response>
        /// <response code="404">If the project is not found.</response>
        /// <response code="400">If the input is invalid.</response>
        #endregion
        [HttpGet(ApiEndPointConstant.Project.ProjectDetailEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailProjectById(Guid id)
        {
            var listProjects = await _projectService.GetDetailProjectById(id);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return Ok(result);
        }

        #region GetListProjectByPhone
        /// <summary>
        /// Retrieves the list of projects associated with a customer's phone number.
        /// </summary>
        /// <param name="phone">The phone number of the customer.</param>
        /// <returns>A list of projects associated with the customer's phone number.</returns>
        /// <response code="200">Returns the list of projects successfully.</response>
        /// <response code="404">If no projects are found for the provided phone number.</response>
        /// <response code="400">If the phone number input is invalid.</response>
        #endregion
        [HttpGet(ApiEndPointConstant.Project.ProjectByNumberPhone)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListProjectByPhone(string phone)
        {
            var listProjects = await _projectService.SearchProjectByPhone(phone);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return Ok(result);
        }
    }
}
