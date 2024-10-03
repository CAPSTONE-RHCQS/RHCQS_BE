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
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
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
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Project.ProjectByNumberPhone)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListProjectByPhone(string phone)
        {
            var listProjects = await _projectService.SearchProjectByPhone(phone);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return Ok(result);
        }

        #region CreateProject
        /// <summary>
        /// Creates a new project with its associated initial quotation and optional utilities.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/project
        ///     {
        ///       "customerId": "e0f86523-13d8-4d89-b6d6-32fbf580e911",
        ///       "name": "Residential Project",
        ///       "type": "Residential",
        ///       "address": "123 Main Street",
        ///       "area": 250.5,
        ///       "initialQuotation": {
        ///         "accountId": "d2f86523-13d8-4d89-b6d6-32fbf580e912",
        ///         "projectId": "a3f86523-13d8-4d89-b6d6-32fbf580e913",
        ///         "promotionId": "b4f86523-13d8-4d89-b6d6-32fbf580e914",
        ///         "packageId": "c5f86523-13d8-4d89-b6d6-32fbf580e915",
        ///         "area": 200.0,
        ///         "isTemplate": true,
        ///         "note": "Initial quotation for construction",
        ///         "initialQuotationItemRequests": [
        ///           {
        ///             "name": "Foundation Work",
        ///             "constructionItemId": "c6f86523-13d8-4d89-b6d6-32fbf580e916",
        ///             "subConstructionId": null,
        ///             "area": 100.0,
        ///             "price": 50000.0,
        ///             "unitPrice": "USD"
        ///           },
        ///           {
        ///             "name": "Roofing",
        ///             "constructionItemId": "e8f86523-13d8-4d89-b6d6-32fbf580e918",
        ///             "subConstructionId": null,
        ///             "area": 50.0,
        ///             "price": 15000.0,
        ///             "unitPrice": "USD"
        ///           }
        ///         ]
        ///       },
        ///       "quotationUtilitiesRequest": [
        ///         {
        ///           "ultilitiesItemId": "f1f86523-13d8-4d89-b6d6-32fbf580e919",
        ///           "finalQuotationId": null,
        ///           "initialQuotationId": "d7f86523-13d8-4d89-b6d6-32fbf580e917",
        ///           "name": "Electricity",
        ///           "coefiicient": 1.5,
        ///           "price": 1000.0,
        ///           "description": "Utility cost for electricity"
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        /// <param name="request">Project creation request model</param>
        /// <returns>Returns true if the project is created successfully, otherwise false.</returns>
        /// <response code="200">Project created successfully</response>
        /// <response code="400">Failed to create the project</response>
        #endregion
        //[Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Project.ProjectEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromBody] ProjectRequest request)
        {
            var isCreate = await _projectService.CreateProjectQuotation(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
