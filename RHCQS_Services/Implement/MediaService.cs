using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObjects;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MediaService> _logger;
        private readonly Cloudinary _cloudinary;

        public MediaService(IUnitOfWork unitOfWork, ILogger<MediaService> logger, Cloudinary cloudinary, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName, string publicId)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = publicId,
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
            }

            return uploadResult.Url?.ToString();
        }

        public async Task<string> UploadImageSubTemplate(IFormFile file, string folder)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("The file is either null or empty.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true,
                        Folder = folder
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    stream.Dispose();

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return uploadResult.Url.ToString();
                    }
                    else
                    {
                        throw new AppConstant.MessageError(
                            (int)AppConstant.ErrCode.Bad_Request,
                            uploadResult.Error.Message
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during file upload.");
                throw;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string publicId = null)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("The file is either null or empty.");
            }

            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId,
                    Folder = folderName,
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Image uploaded successfully: {uploadResult.Url}");
                    return uploadResult.Url.ToString();
                }
                else
                {
                    _logger.LogError($"Upload failed: {uploadResult.Error?.Message}");
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Bad_Request,
                        uploadResult.Error?.Message
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during file upload.");
                throw;
            }
        }

        public string GetImageUrl(ImageUploadResult result)
        {
            return result.Url?.ToString() ?? string.Empty;
        }
    }
}
