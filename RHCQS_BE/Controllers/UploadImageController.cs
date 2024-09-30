using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public UploadImageController(IConfiguration configuration)
        {
            var account = new Account(
                cloud: configuration["Cloudinary:Cloudname"],
                apiKey: configuration["Cloudinary:ApiKey"],
                apiSecret: configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new { uploadResult.Url });
            }
            else
            {
                return StatusCode((int)uploadResult.StatusCode, uploadResult.Error.Message);
            }
        }

        [HttpPost]
        [Route("api/upload/filename")]
        public async Task<ActionResult> Upload(IFormFile file, string customName = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            // Generate a custom name for the file if provided, otherwise use the original filename without extension
            var publicId = customName ?? Path.GetFileNameWithoutExtension(file.FileName);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = publicId,
                Folder = "profile",
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new { uploadResult.Url });
            }
            else
            {
                return StatusCode((int)uploadResult.StatusCode, uploadResult.Error.Message);
            }
        }
    }
}
