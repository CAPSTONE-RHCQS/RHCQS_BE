using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using RHCQS_BusinessObjects;
using RHCQS_Services.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class UploadImgService : IUploadImgService
    {
        private readonly Cloudinary _cloudinary;

        public UploadImgService(IConfiguration configuration)
        {
            var account = new Account(
                cloud: configuration["Cloudinary:Cloudname"],
                apiKey: configuration["Cloudinary:ApiKey"],
                apiSecret: configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
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
    }
}
