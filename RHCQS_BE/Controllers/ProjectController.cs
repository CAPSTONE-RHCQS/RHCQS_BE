using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Request.Project;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static RHCQS_BusinessObjects.AppConstant;

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
        /// Role: MANAGER
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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region FilterProjects
        /// <summary>
        /// Filter project by status
        /// 
        /// Role: MANAGER - SALE STAFF
        /// Retrieves the list of all project.
        /// </summary>
        /// <returns>List of project in the system</returns>
        #endregion
        [Authorize(Roles = "Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Project.ProjectFilterByStatusEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> FilterProjects(int page, int size, string type)
        {
            var listProjects = await _projectService.FilterProjects(page, size, type);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region SearchProjectByName
        /// <summary>
        /// Search project by contain name
        /// 
        /// Role: MANAGER - SALE STAFF
        /// Retrieves the list of all project.
        /// </summary>
        /// <returns>List of project in the system</returns>
        #endregion
        [Authorize(Roles = "Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Project.ProjectSearchByContainName)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchProjectByName(string name, int page, int size)
        {
            var listProjects = await _projectService.SearchProjectByName(name, page, size);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListProjectBySalesStaff
        /// <summary>
        /// Role: SALE STAFF
        /// Gets a list of projects for a sales staff member.
        /// </summary>
        /// <remarks>
        /// This API returns a list of projects managed by the sales staff member, with pagination support.
        /// </remarks>
        /// <param name="page">The page number for pagination (required).</param>
        /// <param name="size">The number of records per page (required).</param>
        /// <returns>A list of projects related to the sales staff member.</returns>
        /// <response code="200">Returns the list of projects successfully.</response>
        /// <response code="400">Returns an error message if parameters are invalid.</response>
        /// <response code="401">Returns an error message if the token is invalid.</response>
        /// <response code="500">Returns an error message if an error occurs on the server.</response>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpGet(ApiEndPointConstant.Project.ProjectSalesStaffEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListProjectBySalesStaff([FromQuery]int page, int size)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var listProjects = await _projectService.GetListProjectBySalesStaff(accountId, page, size);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListProjectForCustomer
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// Retrieve a list of projects for a specific customer by their email.
        /// </summary>
        /// <param name="email">The email of the customer whose projects are being requested.</param>
        /// <returns>A list of projects associated with the given customer's email.</returns>
        /// <response code="200">Returns the list of projects.</response>
        /// <response code="401">If the user is not authenticated or does not have the necessary role.</response>
        /// <response code="403">If the user is forbidden from accessing this resource.</response>
        /// <response code="404">If no projects are found for the provided email.</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Project.ProjectListForCustomerEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListProjectForCustomer(string email)
        {
            var listProjects = await _projectService.GetListProjectByEmail(email);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailProjectById
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF - MANAGER
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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetDetailProjectByIdForDesignStaff
        /// <summary>
        /// Project detail for designer staff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #endregion
        [Authorize(Roles = "DesignStaff")]
        [HttpGet(ApiEndPointConstant.Project.ProjectDesignStaffEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailProjectByIdForDesignStaff(Guid id)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var listProjects = await _projectService.GetDetailProjectByIdForDesignStaff(id, accountId);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetListProjectByPhone
        /// <summary>
        /// Role: SALE STAFF - MANAGER
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
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateProject
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF
        /// Creates a new project along with its initial quotation and optional utilities.
        /// </summary>
        /// <remarks>
        /// **Sample request:**
        /// 
        /// ```json
        /// POST /api/v1/project
        /// {
        ///    "customerId": "08B10FF5-E37D-40BF-947C-80CBF78FA411",
        ///    "name": "Test 1 báo giá",
        ///    "type": "ALL",
        ///    "address": "Vĩnh thuận, Long bình, Thủ đức",
        ///    "area": 123,
        ///    "packageQuotations": [
        ///        {
        ///            "packageId": "aa5057d8-9b30-4f17-a8ee-7aad655b63cc",
        ///            "type": "ROUGH"
        ///        },
        ///        {
        ///            "packageId": "abeeddf8-487d-4dea-afb9-173b3feb0338",
        ///            "type": "FINISHED"
        ///        }
        ///    ],
        ///    "initialQuotation": {
        ///    "promotionId": null,
        ///        "initialQuotationItemRequests": [
        ///            {
        ///        "constructionItemId": "a6ce35ee-d19c-40ac-8044-cbecdb54f8d9",
        ///                "subConstructionId": "40a79928-c9bb-4338-a8fb-0b0e9389e40e",
        ///                "area": 123,
        ///                "price": 56160000
        ///            }
        ///        ]
        ///    },
        ///    "quotationUtilitiesRequest": [
        ///        {
        ///            "ultilitiesItemId": "0A76055D-3AA0-41BF-868F-637EF0C7B19B",
        ///            "name": "Chi phí thi công trình hẻm nhỏ",
        ///            "price": 1684800
        ///        }
        ///    ]
        ///}
        /// ```
        /// </remarks>
        /// <param name="request">The request model for creating a project</param>
        /// <returns>
        /// Returns true if the project is created successfully; otherwise, false.
        /// </returns>
        /// <response code="200">Project created successfully</response>
        /// <response code="400">Failed to create the project due to validation errors</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff")]
        [HttpPost(ApiEndPointConstant.Project.ProjectEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> CreateProject([FromBody] ProjectRequest request)
        {
            var isCreate = await _projectService.CreateProjectQuotation(request);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region AssignQuotation
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// Assign a quotation to a customer based on the request payload.
        /// </summary>
        /// <param name="request">The request payload containing accountId and initialQuotationId.</param>
        /// <returns>Assigned quotation details or an error message.</returns>
        /// <response code="200">Returns the details of the assigned quotation.</response>
        /// <response code="404">If no quotation is found or staff overload error occurs.</response>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/initial-quotation
        ///     {
        ///         "accountId": "d287e991-5b2b-4569-b0c4-7e81d9e75b78",
        ///         "projectId": "3f63e5b2-632f-48fa-ae9d-1c123456abcd"
        ///     }
        ///     
        /// </remarks>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.Project.ProjectAssignEndpoint)]
        [ProducesResponseType(typeof(HouseDesignDrawingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignQuotation([FromBody] AssignProject request)
        {
            var quotation = await _projectService.AssignQuotation(request.accountId, request.projectId);
            if (quotation == null) return NotFound(new { message = AppConstant.ErrMessage.OverloadStaff });
            var result = JsonConvert.SerializeObject(quotation, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CancelProject
        /// <summary>
        /// Role: MANAGER - CUSTOMER
        /// Cancels an existing project along with its associated initial quotations, final quotations, and house design drawings.
        /// </summary>
        /// <remarks>
        /// **Description:**
        /// 
        /// This endpoint allows a manager to cancel a project by updating its status to "ENDED" and also canceling its related entities such as initial quotations, final quotations, and house design drawings. Once canceled, these items will be marked as inactive or canceled.
        /// 
        /// **Sample Request:**
        /// 
        /// ```json
        /// PUT /api/projects/cancel?projectId=123e4567-e89b-12d3-a456-426614174000
        /// ```
        /// 
        /// **Request Parameters:**
        /// - `projectId` (Guid, required): The unique identifier of the project to be canceled.
        /// 
        /// **Sample Response:**
        /// 
        /// - **Success (200 OK):**
        /// ```json
        /// true
        /// ```
        /// - **Failure (400 Bad Request):**
        /// ```json
        /// false
        /// ```
        /// 
        /// **Authorization:**
        /// 
        /// This endpoint requires the user to have the role of `Manager`.
        /// </remarks>
        /// <param name="projectId">The ID of the project to be canceled.</param>
        /// <returns>Returns a boolean indicating whether the project was successfully canceled or not.</returns>
        #endregion
        [Authorize(Roles = "Manager, Customer")]
        [HttpPut(ApiEndPointConstant.Project.ProjectCancelEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelProject([FromQuery] Guid projectId, string reasonCanceled)
        {
            var isCreate = await _projectService.CancelProject(projectId, reasonCanceled);
            return isCreate ? Ok(isCreate) : BadRequest();
        }

        #region TrackingProject
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF - MANAGER
        /// Retrieve a list of projects for a specific customer by their email.
        /// </summary>
        /// <param name="projectId">The email of the customer whose projects are being requested.</param>
        /// <returns>A list of projects associated with the given customer's email.</returns>
        /// <response code="200">Returns the list of projects.</response>
        /// <response code="401">If the user is not authenticated or does not have the necessary role.</response>
        /// <response code="403">If the user is forbidden from accessing this resource.</response>
        /// <response code="404">If no projects are found for the provided email.</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Project.ProjectTrackingEndpoint)]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> TrackingProject(Guid projectId)
        {
            var listProjects = await _projectService.TrackingProject(projectId);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateTemplateHouseProject
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF  
        /// Creates a new house project template with a specific sub-template and package.
        /// </summary>
        /// <remarks>
        /// **Sample request:**
        /// 
        /// ```json
        /// POST /api/v1/project
        /// {
        ///   "subTemplateId": "420E61D1-CD1F-4D90-8E93-1C2EEE62F086",
        ///   "accountId": "84412C77-ECD0-4D08-ADA8-90D4623CD5C0",
        ///   "address": "Số 123, Đường Lê Lợi, Phường Bến Nghé, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",
        ///   "packgeFinsihed": "5245F485-C546-4051-B04E-954FD773811E",
        ///   "quotationUtilitiesRequest": null
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">The request model for creating a project</param>
        /// <returns>
        /// Returns true if the project is created successfully; otherwise, false.
        /// </returns>
        /// <response code="200">Project created successfully</response>
        /// <response code="400">Failed to create the project due to validation errors</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff")]
        [HttpPost(ApiEndPointConstant.Project.ProjectTemplateHouseEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTemplateHouseProject([FromBody] TemplateHouseProjectRequest request)
        {
            var isCreate = await _projectService.CreateProjectTemplateHouse(request);
            return Ok(isCreate);
        }

        #region CreateProjectHaveDrawing
        /// <summary>
        /// Role: CUSTOMER - SALE STAFF  
        /// Creates a new house project template with a specific sub-template and package.
        /// </summary>
        /// <param name="request">The request model for creating a project</param>
        /// <returns>
        /// Returns true if the project is created successfully; otherwise, false.
        /// </returns>
        /// <response code="200">Project created successfully</response>
        /// <response code="400">Failed to create the project due to validation errors</response>
        #endregion
        [Authorize(Roles = "Customer, SalesStaff")]
        [HttpPost(ApiEndPointConstant.Project.ProjectHaveDrawingEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProjectHaveDrawing([FromForm] ProjectHaveDrawingRequest request)
        {
            var isCreate = await _projectService.ProjectHaveDrawing(request);
            return Ok(isCreate);
        }

        #region FilterProjectsMultiParams
        /// <summary>
        /// Filters projects based on multiple parameters.
        /// </summary>
        /// <param name="page">The page number for pagination (required).</param>
        /// <param name="size">The number of items per page for pagination (required).</param>
        /// <param name="startTime">The start time(optional).</param>
        /// <param name="status">The project status(optional).</param>
        /// <param name="type">The project type (optional): TEMPLATE(Mẫu nhà), FINISHED(Hoàn thiện), ROUGH(Thô), ALL(Thô + Hoàn thiện)</param>
        /// <param name="code">The project code(optional).</param>
        /// <param name="phone">The customer's phone number(optional).</param>
        /// <returns>
        /// A paginated list of projects that match the filtering criteria.
        /// Returns a JSON object with the paginated results.
        /// </returns>
        /// <response code="200">Returns the list of filtered projects in JSON format.</response>
        /// <response code="400">If any invalid parameter is provided.</response>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager, DesignStaff")]
        [HttpGet(ApiEndPointConstant.Project.FilterProjectsMultiParams)]
        [ProducesResponseType(typeof(IPaginate<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterProjectsMultiParams(
            [FromQuery, Required(ErrorMessage = "Page is required")] int page,
            [FromQuery, Required(ErrorMessage = "Size is required")] int size,
            [FromQuery] DateTime? startTime,
            [FromQuery] string? status,
            [FromQuery] string? type,
            [FromQuery] string? code,
            [FromQuery] string? phone)
        {
            var listProjects = await _projectService.FilterProjectsMultiParams(page, size, startTime, status, type, code, phone);
            var result = JsonConvert.SerializeObject(listProjects, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetStatusProjectDetail
        /// <summary>
        /// Get status projects for Web
        /// 
        /// Role: DESIGNSTAFF - SALES STAFF - MANAGER
        /// </summary>
        /// <returns>Number of projects.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Project.ProjectStatusEndpoint)]
        public async Task<ActionResult<int>> GetStatusProjectDetail(Guid projectId)
        {
            var totalProjectCount = await _projectService.GetStatusProjectDetail(projectId);
            return Ok(totalProjectCount);
        }
    }
}
