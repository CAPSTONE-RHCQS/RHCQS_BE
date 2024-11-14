using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IUploadImgService _uploadImgService;

        public BlogController(IBlogService blogService, IUploadImgService uploadImgService)
        {
            _blogService = blogService;
            _uploadImgService = uploadImgService;
        }

        #region GetListBlog
        /// <summary>
        /// Get a paginated list of blogs.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="size">Size of the page</param>
        /// <returns>List of blogs</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Blog.BlogEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<BlogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListBlogAsync(int page, int size)
        {
            var blogs = await _blogService.GetListBlogAsync(page, size);
            var response = JsonConvert.SerializeObject(blogs, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region GetAllBlog
        /// <summary>
        /// Retrieves blog list without entering page and size.
        /// </summary>
        /// <returns>List of blog in the system</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Blog.BlogListEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<BlogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBlogAsync()
        {
            var blogs = await _blogService.GetListBlog();
            var response = JsonConvert.SerializeObject(blogs, Formatting.Indented);
            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region GetBlogById
        /// <summary>
        /// Get a blog by its ID.
        /// </summary>
        /// <param name="id">Blog ID</param>
        /// <returns>Blog details</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Blog.BlogDetailEndpoint)]
        [ProducesResponseType(typeof(BlogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            var blog = await _blogService.GetBlogById(id);
            var result = JsonConvert.SerializeObject(blog, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region GetBlogByAccountId
        /// <summary>
        /// Get a blog by account ID.
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <returns>Blog details</returns>
        #endregion
        [Authorize(Roles = "Customer, Manager, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Blog.BlogByAccountEndpoint)]
        [ProducesResponseType(typeof(BlogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlogByAccountId(Guid accountId)
        {
            var blog = await _blogService.GetBlogByAccountId(accountId);
            var result = JsonConvert.SerializeObject(blog, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region CreateBlog
        /// <summary>
        /// Create a new blog.
        /// </summary>
        /// <param name="blogRequest">Blog creation details</param>
        /// <returns>Creation status</returns>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPost(ApiEndPointConstant.Blog.BlogEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequest blogRequest)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blogRequest.AccountId = Guid.Parse(accountId);
            if (!string.IsNullOrEmpty(blogRequest.ImgUrl))
            {
                string imgUrl = await _uploadImgService.UploadImageAsync(blogRequest.ImgUrl, "Blog");
                blogRequest.ImgUrl = imgUrl;
            }

            var isCreated = await _blogService.CreateBlogAsync(blogRequest);
            return Ok(isCreated ? AppConstant.Message.SUCCESSFUL_CREATE : AppConstant.Message.ERROR);
        }

        #region UpdateBlog
        /// <summary>
        /// Update an existing blog.
        /// </summary>
        /// <param name="blogRequest">Blog update details</param>
        /// <param name="blogId">Blog ID</param>
        /// <returns>Update status</returns>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPut(ApiEndPointConstant.Blog.BlogEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBlog([FromBody] BlogRequest blogRequest, Guid blogId)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            blogRequest.AccountId = Guid.Parse(accountId);
            if (!string.IsNullOrEmpty(blogRequest.ImgUrl))
            {
                string imgUrl = await _uploadImgService.UploadImageAsync(blogRequest.ImgUrl, "Blog");
                blogRequest.ImgUrl = imgUrl;
            }

            var isUpdated = await _blogService.UpdateBlogAsync(blogId, blogRequest);
            return Ok(isUpdated ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.Message.ERROR);
        }
        #region DeleteBlog
        /// <summary>
        ///Delete a blog.
        /// </summary>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpDelete(ApiEndPointConstant.Blog.BlogEndpoint)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var isDeleted = await _blogService.DeleteBlog(id);
            return Ok(isDeleted ? AppConstant.Message.SUCCESSFUL_DELETE : AppConstant.Message.ERROR);
        }

    }
}
