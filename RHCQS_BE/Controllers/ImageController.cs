using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUploadImgService _uploadImgService;
        public ImageController(IUploadImgService uploadImgService)
        {
            _uploadImgService = uploadImgService;
        }

        #region Upload
        /// <summary>
        /// Upload file house design 
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        #endregion
        [HttpPost(ApiEndPointConstant.General.UploadImageDrawingEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Upload(IFormFile file, string fileName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is null or empty.");
            }

            try
            {
                var url = await _uploadImgService.UploadImage(file, fileName);
                if (url == "Fail")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "File upload failed.");
                }

                return Ok(new { url });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog, NLog, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        #region UploadAvatar
        /// <summary>
        /// Upload avatar for customer's profile
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        #endregion
        [HttpPost(ApiEndPointConstant.General.UploadAvatarEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UploadAvatar(IFormFile file, string imageName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is null or empty.");
            }

            try
            {
                var url = await _uploadImgService.UploadImageFolder(file, imageName, "Profile");
                if (url == "Fail")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "File upload failed.");
                }

                return Ok(new { url });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog, NLog, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
