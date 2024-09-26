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
    }
}
