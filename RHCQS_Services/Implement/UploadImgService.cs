using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class UploadImgService : IUploadImgService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UploadImgService> _logger;
        private readonly IConverter _converter;
        private readonly Cloudinary _cloudinary;

        public UploadImgService(IUnitOfWork unitOfWork, ILogger<UploadImgService> logger,
            IConverter converter, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _converter = converter;
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(string imagePathOrUrl, string folder)
        {
            if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
            {
                return await UploadFromUrl(imagePathOrUrl, folder);
            }
            else
            {
                return await UploadFromLocalPath(imagePathOrUrl, folder);
            }
        }

        private async Task<string> UploadFromUrl(string imageUrl, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageUrl),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = folder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

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

        private async Task<string> UploadFromLocalPath(string localPath, string folder)
        {
            if (!File.Exists(localPath))
                throw new FileNotFoundException("File not found.", localPath);

            using (var stream = new FileStream(localPath, FileMode.Open))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(localPath, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true,
                    Folder = folder
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

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
        public async Task<string> UploadFile(Guid designTemplateId, IFormFile file, string folder, string nameImage)
        {
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

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var mediaItem = new Medium
                    {
                        Id = Guid.NewGuid(),
                        Name = nameImage,
                        Url = uploadResult.Url.ToString(),
                        InsDate = LocalDateTime.VNDateTime(),
                        DesignTemplateId = designTemplateId
                    };
                    await _unitOfWork.GetRepository<Medium>().InsertAsync(mediaItem);
                    _unitOfWork.Commit();
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
        public async Task<string> UploadFileForImageAccount(Guid accountid, IFormFile file, string folder, string nameImage)
        {
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

                return uploadResult.Url.ToString();
            }
        }
        public async Task<List<string>> UploadImageDesignTemplate(List<IFormFile> files)
        {
            var uploadResults = new List<string>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        UseFilename = true,
                        UniqueFilename = false,
                        Overwrite = true,
                        Folder = "DesignHouse" 
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        uploadResults.Add(uploadResult.Url.ToString());
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

            return uploadResults;
        }

        public async Task<string> UploadImageFolder(IFormFile file, string fileName, string folder)
        {
            if (file == null || file.Length == 0)
                return "File is null or empty";

            try
            {
                var publicId = fileName ?? Path.GetFileNameWithoutExtension(file.FileName);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = publicId,
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.StatusCode == System.Net.HttpStatusCode.OK
                    ? uploadResult.Url.ToString()
                    : "Fail";
            }
            catch (Exception ex)
            {
                // Log error details for debugging
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> UploadImage(IFormFile file, string fileName)
        {
            if (file == null || file.Length == 0)
                return "File is null or empty";

            try
            {
                var publicId = fileName ?? Path.GetFileNameWithoutExtension(file.FileName);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = publicId,
                    Folder = "HouseDesignDrawing",
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.StatusCode == System.Net.HttpStatusCode.OK
                    ? uploadResult.Url.ToString()
                    : "Fail";
            }
            catch (Exception ex)
            {
                // Log error details for debugging
                return $"Error: {ex.Message}";
            }
        }
    }
}
