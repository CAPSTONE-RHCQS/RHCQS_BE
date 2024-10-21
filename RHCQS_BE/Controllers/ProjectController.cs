using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.InitialQuotation;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;
using System.Security.Claims;

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

        #region GetListProjectBySalesStaff
        /// <summary>
        /// Role: SALE STAFF
        /// Gets a list of projects for a sales staff member.
        /// </summary>
        /// <remarks>
        /// This API returns a list of projects managed by the sales staff member, with pagination support.
        /// </remarks>
        /// <param name="token">JWT token of the sales staff member.</param>
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
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
        ///     "customerId": "1E6FA320-6945-4B76-8426-AD6DFDB1AD74",
        ///     "name": "Báo giá sơ bộ - TP.Thủ Đức",
        ///     "type": "ALL",
        ///     "address": "382 D2, Phường Tân Phú, TP.Thủ Đức",
        ///     "area": 125,
        ///     "packageQuotations": [
        ///         {
        ///             "packageId": "59F5FD78-B895-4D60-934A-4727C219B2D9",
        ///             "type": "ROUGH"
        ///         },
        ///         {
        ///             "packageId": "0BFB83DD-04AF-4F8C-A6D0-2CD8EE1FF0F5",
        ///             "type": "FINISHED"
        ///         }
        ///     ],
        ///     "initialQuotation": {
        ///         "accountId": "D63A2A80-CDEA-46DF-8419-E5C70A7632EE",
        ///         "promotionId": "00000000-0000-0000-0000-000000000000",
        ///         "packageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "initialQuotationItemRequests": [
        ///             {
        ///                 "name": null,
        ///                 "constructionItemId": "75922602-9153-4CC3-A7DC-225C9BC30A5E",
        ///                 "subConstructionId": "06FF2D3D-2F14-4ACC-BA2E-0C4EE659CA81",
        ///                 "area": 49.5,
        ///                 "price": 66330000.0
        ///             },
        ///             {
        ///                 "name": null,
        ///                 "constructionItemId": "BE6C6DB7-CEA1-4275-9B18-2FBCFE9B2353",
        ///                 "subConstructionId": "06FF2D3D-2F14-4ACC-BA2E-0C4EE659CA81",
        ///                 "area": 99,
        ///                 "price": 331650000.0
        ///             },
        ///             {
        ///                 "name": null,
        ///                 "constructionItemId": "BD101AF5-AC48-43BA-A474-957A20A933BD",
        ///                 "subConstructionId": "7E442652-EEFC-43B7-918B-A264A10E679D",
        ///                 "area": 49.5,
        ///                 "price": 66330000.0
        ///             }
        ///         ]
        ///     },
        ///     "quotationUtilitiesRequest": [
        ///         {
        ///             "ultilitiesItemId": "2EC103AA-AA83-4D58-9E85-22A6247F4CD6",
        ///             "name": "Sàn từ 30m2 ~ 40m2",
        ///             "coefficient": 0.05,
        ///             "price": 110617000,
        ///             "description": "Sàn từ 30m2 ~ 40m2"
        ///         }
        ///     ]
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
        //[Authorize(Roles = "Customer, SalesStaff")]
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
        ///         "initialQuotationId": "3f63e5b2-632f-48fa-ae9d-1c123456abcd"
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
        /// Role: MANAGER
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
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Project.ProjectCancelEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelProject([FromQuery] Guid projectId)
        {
            var isCreate = await _projectService.CancelProject(projectId);
            return isCreate ? Ok(isCreate) : BadRequest();
        }
    }
}
