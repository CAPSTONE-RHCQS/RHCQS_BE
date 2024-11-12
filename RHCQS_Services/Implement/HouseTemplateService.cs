using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using RHCQS_BusinessObject.Payload.Request.DesignTemplate;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Runtime.InteropServices;
using Azure.Core;
using Microsoft.AspNetCore.Routing.Template;
using RHCQS_BusinessObject.Helper;

namespace RHCQS_Services.Implement
{
    public class HouseTemplateService : IHouseTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseTemplateService> _logger;
        private readonly Cloudinary _cloudinary;
        private readonly IUploadImgService _uploadImgService;
        private readonly IMediaService _mediaService;

        public HouseTemplateService(IUnitOfWork unitOfWork,
            ILogger<HouseTemplateService> logger, 
            Cloudinary cloudinary, 
            IUploadImgService uploadImgService,
            IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
            _uploadImgService = uploadImgService;
            _mediaService = mediaService;
        }
        public async Task<List<HouseTemplateResponse>> GetListHouseTemplate()
        {
            var listHouseTemplate = await _unitOfWork.GetRepository<DesignTemplate>().GetListAsync(
                selector: x => new HouseTemplateResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.NumberOfFloor,
                    x.NumberOfBed,
                    x.ImgUrl,
                    x.InsDate,
                    x.SubTemplates.Select(
                        sub => new SubTemplatesResponse(
                            sub.Id,
                            sub.BuildingArea,
                            sub.FloorArea,
                            sub.InsDate,
                            sub.Size,
                            sub.ImgUrl,
                            sub.TemplateItems.Select(
                                item => new TemplateItemReponse(
                                    item.Id,
                                    item.SubConstructionId == null
                                        ? item.ConstructionItem.Name
                                        : item.SubConstruction.Name,
                                    item.ConstructionItem.Id,
                                    item.SubConstructionId,
                                    item.SubConstructionId == null
                                        ? item.ConstructionItem.Coefficient
                                        : item.SubConstruction.Coefficient,
                                    item.Area,
                                    item.Unit,
                                    item.InsDate
                                )
                            ).ToList(),
                            sub.Media
                                .Where(media => media.Name == AppConstant.Template.Drawing)
                                .Select(media => new MediaResponse(
                                    media.Id,
                                    media.Name,
                                    media.Url,
                                    media.InsDate,
                                    media.UpsDate
                                )).ToList()
                        )
                    ).ToList(),
                    x.PackageHouses.Select(
                        pgk => new PackageHouseResponse(
                            pgk.Id,
                            pgk.PackageId,
                            pgk.Package.PackageName,
                            pgk.ImgUrl,
                            pgk.InsDate,
                            pgk.Description
                        )
                    ).ToList(),
                    x.Media
                        .Where(media => media.Name == AppConstant.Template.Exteriorsdrawings)
                        .Select(media => new MediaResponse(
                            media.Id,
                            media.Name,
                            media.Url,
                            media.InsDate,
                            media.UpsDate
                        )).ToList()
                ),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );

            return listHouseTemplate.ToList();
        }


        public async Task<Guid> CreateHouseTemplate(HouseTemplateRequestForCreate templ)
        {
            try
            {
                var templateRepo = _unitOfWork.GetRepository<DesignTemplate>();

                if (await templateRepo.AnyAsync(x => x.Name == templ.Name))
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.DesignTemplate
                    );
                }

                var houseTemplate = new DesignTemplate
                {
                    Id = Guid.NewGuid(),
                    Name = templ.Name,
                    Description = templ.Description,
                    NumberOfFloor = templ.NumberOfFloor,
                    NumberOfBed = templ.NumberOfBed,
                    ImgUrl = null,
                    InsDate = LocalDateTime.VNDateTime(),
                    SubTemplates = templ.SubTemplates.Select(sub => new SubTemplate
                    {
                        Id = Guid.NewGuid(),
                        BuildingArea = sub.BuildingArea,
                        FloorArea = sub.FloorArea,
                        Size = sub.Size,
                        ImgUrl = null,
                        InsDate = LocalDateTime.VNDateTime(),
                        TotalRough = sub.TotalRough,
                        TemplateItems = sub.TemplateItems.Select(item => new TemplateItem
                        {
                            Id = Guid.NewGuid(),
                            ConstructionItemId = item.ConstructionItemId,
                            SubConstructionId = item.SubConstructionItemId != Guid.Empty ? item.SubConstructionItemId : (Guid?)null,
                            Area = item.Area,
                            Unit = item.Unit,
                            InsDate = LocalDateTime.VNDateTime(),
                        }).ToList()
                    }).ToList(),
                    PackageHouses = new List<PackageHouse>()
                };

                //Rough package
                var packageHouse = new PackageHouse
                {
                    Id = Guid.NewGuid(),
                    PackageId = templ.PackageRoughId,
                    DesignTemplateId = houseTemplate.Id,
                    InsDate = LocalDateTime.VNDateTime(),
                    Description = templ.DescriptionPackage
                };

                houseTemplate.PackageHouses.Add(packageHouse);
                await _unitOfWork.GetRepository<DesignTemplate>().InsertAsync(houseTemplate);

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return houseTemplate.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreatePackageHouse(PackageHouseRequest request)
        {

            var packageItem = new PackageHouse
            {
                Id = Guid.NewGuid(),
                PackageId = request.PackageId,
                DesignTemplateId = request.DesginTemplateId,
                InsDate = LocalDateTime.VNDateTime(),
                Description = request.Description,
                ImgUrl = request.PackageHouseImage
            };

            await _unitOfWork.GetRepository<PackageHouse>().InsertAsync(packageItem);


            var saveSuccessful = await _unitOfWork.CommitAsync() > 0;
            return saveSuccessful;
        }

        public async Task<string> UpdateSubTemplate(Guid subTemplateId, UpdateSubTemplateRequest request)
        {
            bool result = await UpdateTemplateArea(subTemplateId, request);
            return result ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Fail_Save;
        }

        private async Task<bool> UpdateTemplateArea(Guid subTemplateId, UpdateSubTemplateRequest request)
        {
            var templateItem = await _unitOfWork.GetRepository<SubTemplate>().FirstOrDefaultAsync(
                                predicate: s => s.Id == subTemplateId,
                                include: s => s.Include(s => s.TemplateItems));

            templateItem.BuildingArea = request.BuildingArea ?? templateItem.BuildingArea;
            templateItem.FloorArea = request.FloorArea ?? templateItem.FloorArea;
            templateItem.Size = request.Size ?? templateItem.Size;
            templateItem.TotalRough = request.TotalRough.HasValue ? request.TotalRough.Value : templateItem.TotalRough;

            _unitOfWork.GetRepository<SubTemplate>().UpdateAsync(templateItem);

            foreach(var item in templateItem.TemplateItems)
            {
                var temItem = await _unitOfWork.GetRepository<TemplateItem>().FirstOrDefaultAsync(
                                predicate: t => t.ConstructionItemId == item.ConstructionItemId 
                                || t.SubConstructionId == item.SubConstructionId);
                if (temItem != null)
                {
                    temItem.Area = item.Area ?? temItem.Area;
                    _unitOfWork.GetRepository<TemplateItem>().UpdateAsync(temItem);

                } else
                {
                    var newTemplate = new TemplateItem
                    {
                        Id = Guid.NewGuid(),
                        ConstructionItemId = item.ConstructionItemId,
                        SubConstructionId = item.SubConstructionId,
                        Area = item.Area,
                        Unit = item.Unit
                    };
                    await _unitOfWork.GetRepository<TemplateItem>().InsertAsync(newTemplate);
                }
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.TemplateItemNotFound);
            }

            return isSuccessful;
        }

        public async Task<HouseTemplateResponse> GetHouseTemplateDetail(Guid id)
        {
            var template = await _unitOfWork.GetRepository<DesignTemplate>().FirstOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.SubTemplates)
                               .ThenInclude(st => st.TemplateItems)
                               .ThenInclude(ti => ti.ConstructionItem)
                               .ThenInclude(ti => ti.SubConstructionItems)
                               .Include(x => x.SubTemplates)
                               .ThenInclude(st => st.Media)
                               .Include(x => x.PackageHouses)
                               .ThenInclude(p => p.Package)
                               .ThenInclude(p => p.PackageType)
                               .Include(x => x.Media)
            );

            if (template != null)
            {
                var result = new HouseTemplateResponse(
                    template.Id,
                    template.Name,
                    template.Description,
                    template.NumberOfFloor,
                    template.NumberOfBed,
                    template.ImgUrl,
                    template.InsDate,
                    template.SubTemplates.Select(sub => new SubTemplatesResponse(
                        sub.Id,
                        sub.BuildingArea,
                        sub.FloorArea,
                        sub.InsDate,
                        sub.Size,
                        sub.ImgUrl,
                        sub.TemplateItems.Select(item => new TemplateItemReponse(
                            item.Id,
                            item.SubConstructionId == null
                                ? item.ConstructionItem.Name
                                : item.ConstructionItem.SubConstructionItems.FirstOrDefault(sci => sci.Id == item.SubConstructionId)?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId,
                            item.SubConstructionId == null
                                ? item.ConstructionItem.Coefficient
                                : item.ConstructionItem.SubConstructionItems.FirstOrDefault(sci => sci.Id == item.SubConstructionId)?.Coefficient,
                            item.Area,
                            item.Unit,
                            item.InsDate
                        )).ToList(),
                        template.Media
                            .Where(media => media.Name.Equals(AppConstant.Template.Drawing))
                            .Select(media => new MediaResponse(
                                media.Id,
                                media.Name,
                                media.Url,
                                media.InsDate,
                                media.UpsDate
                            )).ToList() // Designdrawings
                    )).ToList(),
                    template.PackageHouses
                    .Where(pkg => pkg.Package.PackageType.Name != "ROUGH")
                    .Select(pkg => new PackageHouseResponse(
                        pkg.Id,
                        pkg.PackageId,
                        pkg.Package.PackageName,
                        pkg.ImgUrl,
                        pkg.InsDate,
                        pkg.Description

                    )).ToList(),
                    template.Media
                        .Where(media => media.Name == AppConstant.Template.Exteriorsdrawings)
                        .Select(media => new MediaResponse(
                            media.Id,
                            media.Name,
                            media.Url,
                            media.InsDate,
                            media.UpsDate
                        )).ToList() // ExteriorsUrls
                );
                return result;
            }
            return null;
        }
        public async Task<IPaginate<HouseTemplateResponseCustom>> GetListHouseTemplateForShortVersionAsync(int page, int size)
        {
            var listHouseTemplate = await _unitOfWork.GetRepository<DesignTemplate>().GetList(
                selector: x => new HouseTemplateResponseCustom(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.NumberOfFloor,
                    x.NumberOfBed,
                    x.ImgUrl,
                    x.InsDate
                ),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );

            return listHouseTemplate;
        }
        public async Task<IPaginate<HouseTemplateResponse>> GetListHouseTemplateAsync(int page, int size)
        {
            var listHouseTemplate = await _unitOfWork.GetRepository<DesignTemplate>().GetList(
                selector: x => new HouseTemplateResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.NumberOfFloor,
                    x.NumberOfBed,
                    x.ImgUrl,
                    x.InsDate,
                    x.SubTemplates.Select(
                        sub => new SubTemplatesResponse(
                            sub.Id,
                            sub.BuildingArea,
                            sub.FloorArea,
                            sub.InsDate,
                            sub.Size,
                            sub.ImgUrl,
                            sub.TemplateItems.Select(
                                item => new TemplateItemReponse(
                                    item.Id,
                                    item.SubConstructionId == null
                                        ? item.ConstructionItem.Name
                                        : item.SubConstruction.Name,
                                    item.ConstructionItem.Id,
                                    item.SubConstructionId,
                                    item.SubConstructionId == null
                                        ? item.ConstructionItem.Coefficient
                                        : item.SubConstruction.Coefficient,
                                    item.Area,
                                    item.Unit,
                                    item.InsDate
                                )
                            ).ToList(),
                            sub.Media
                                .Where(media => media.Name == AppConstant.Template.Drawing)
                                .Select(media => new MediaResponse(
                                    media.Id,
                                    media.Name,
                                    media.Url,
                                    media.InsDate,
                                    media.UpsDate
                                )).ToList() // Designdrawings
                        )
                    ).ToList(),
                    x.PackageHouses.Select(
                        pgk => new PackageHouseResponse(
                            pgk.Id,
                            pgk.PackageId,
                            pgk.Package.PackageName,
                            pgk.ImgUrl,
                            pgk.InsDate,
                            pgk.Description
                        )
                    ).ToList(),
                    x.Media
                        .Where(media => media.Name == AppConstant.Template.Exteriorsdrawings)
                        .Select(media => new MediaResponse(
                            media.Id,
                            media.Name,
                            media.Url,
                            media.InsDate,
                            media.UpsDate
                        )).ToList() // ExteriorsUrls
                ),
                include: x => x.Include(x => x.PackageHouses)
                                .ThenInclude(p => p.Package)
                               .Include(x => x.Media)
                               .Include(x => x.SubTemplates)
                               .ThenInclude(st => st.Media)
                               .Include(x => x.SubTemplates)
                               .ThenInclude(st => st.TemplateItems),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );

            return listHouseTemplate;
        }

        public async Task<HouseTemplateResponse> SearchHouseTemplateByNameAsync(string name)
        {
            var template = await _unitOfWork.GetRepository<DesignTemplate>().FirstOrDefaultAsync(
                predicate: x => x.Name.Equals(name),
                include: x => x.Include(x => x.SubTemplates)
                               .ThenInclude(st => st.TemplateItems)
                               .ThenInclude(ti => ti.ConstructionItem)
                               .ThenInclude(ti => ti.SubConstructionItems)
                               .Include(x => x.SubTemplates)
                               .ThenInclude(st => st.Media)
                               .Include(x => x.PackageHouses)
                               .ThenInclude(p => p.Package)
                               .Include(x => x.Media)
            );

            if (template != null)
            {
                var result = new HouseTemplateResponse(
                    template.Id,
                    template.Name,
                    template.Description,
                    template.NumberOfFloor,
                    template.NumberOfBed,
                    template.ImgUrl,
                    template.InsDate,
                    template.SubTemplates.Select(sub => new SubTemplatesResponse(
                        sub.Id,
                        sub.BuildingArea,
                        sub.FloorArea,
                        sub.InsDate,
                        sub.Size,
                        sub.ImgUrl,
                        sub.TemplateItems.Select(item => new TemplateItemReponse(
                            item.Id,
                            item.SubConstructionId == null
                                ? item.ConstructionItem.Name
                                : item.ConstructionItem.SubConstructionItems.FirstOrDefault(sci => sci.Id == item.SubConstructionId)?.Name,
                            item.ConstructionItemId,
                            item.SubConstructionId,
                            item.SubConstructionId == null
                                ? item.ConstructionItem.Coefficient
                                : item.ConstructionItem.SubConstructionItems.FirstOrDefault(sci => sci.Id == item.SubConstructionId)?.Coefficient,
                            item.Area,
                            item.Unit,
                            item.InsDate
                        )).ToList(),
                        sub.Media
                            .Where(media => media.Name == AppConstant.Template.Drawing)
                            .Select(media => new MediaResponse(
                                media.Id,
                                media.Name,
                                media.Url,
                                media.InsDate,
                                media.UpsDate
                            )).ToList() // Designdrawings
                    )).ToList(),
                    template.PackageHouses.Select(pkg => new PackageHouseResponse(
                        pkg.Id,
                        pkg.PackageId,
                        pkg.Package.PackageName,
                        pkg.ImgUrl,
                        pkg.InsDate,
                        pkg.Description
                    )).ToList(),
                    template.Media
                        .Where(media => media.Name == AppConstant.Template.Exteriorsdrawings)
                        .Select(media => new MediaResponse(
                            media.Id,
                            media.Name,
                            media.Url,
                            media.InsDate,
                            media.UpsDate
                        )).ToList() // ExteriorsUrls
                );
                return result;
            }
            return null;
        }

        public async Task<DesignTemplate> UpdateHouseTemplate(HouseTemplateRequestForUpdate templ, Guid templateId)
        {
            if (templ == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var templateRepo = _unitOfWork.GetRepository<DesignTemplate>();

            var houseTemplate = await templateRepo.FirstOrDefaultAsync(
                x => x.Id == templateId,
                include: x => x
                    .Include(x => x.SubTemplates)
                    .ThenInclude(st => st.TemplateItems)
                    .Include(x => x.Media)
            );

            if (houseTemplate == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Resource
                );
            }

            houseTemplate.Name = templ.Name;
            houseTemplate.Description = templ.Description;
            houseTemplate.NumberOfFloor = templ.NumberOfFloor;
            houseTemplate.NumberOfBed = templ.NumberOfBed;
            houseTemplate.ImgUrl = templ.ImgURL;

            foreach (var media in templ.ExteriorsUrls)
            {
                var existingMedia = houseTemplate.Media.FirstOrDefault(m => m.DesignTemplateId == templateId);
                if (existingMedia != null)
                {
                    existingMedia.Url = media.MediaImgURL;
                    existingMedia.UpsDate = LocalDateTime.VNDateTime();
                }
                else
                {
                    continue;
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.Not_Found_Media
                    );
                }
            }

            foreach (var sub in templ.SubTemplates)
            {
                var existingSubTemplate = houseTemplate.SubTemplates.FirstOrDefault(st => st.Id == sub.Id);
                if (existingSubTemplate != null)
                {
                    existingSubTemplate.BuildingArea = sub.BuildingArea ?? 0;
                    existingSubTemplate.FloorArea = sub.FloorArea;
                    existingSubTemplate.Size = sub.Size;
                    existingSubTemplate.ImgUrl = sub.ImgURL;

                    foreach (var item in sub.TemplateItems)
                    {
                        var existingTemplateItem = existingSubTemplate.TemplateItems.FirstOrDefault(ti => ti.Id == item.Id);
                        if (existingTemplateItem != null)
                        {
                            existingTemplateItem.Area = item.Area;
                            existingTemplateItem.Unit = item.Unit;
                            existingTemplateItem.ConstructionItemId = item.ConstructionItemId;
                        }
                        else
                        {
                            continue;
                            throw new AppConstant.MessageError(
                                (int)AppConstant.ErrCode.Conflict,
                                AppConstant.ErrMessage.TemplateItemNotFound
                            );
                        }
                    }
                    foreach (var media in sub.Designdrawings)
                    {
                        var existingMedia = existingSubTemplate.Media.FirstOrDefault(m => m.SubTemplateId == sub.Id);
                        if (existingMedia != null)
                        {
                            existingMedia.Url = media.MediaImgURL;
                            existingMedia.UpsDate = LocalDateTime.VNDateTime();
                        }
                        else
                        {
                            continue;
                            throw new AppConstant.MessageError(
                                (int)AppConstant.ErrCode.Conflict,
                                AppConstant.ErrMessage.Not_Found_Media
                            );
                        }
                    }
                }
                else
                {
                    continue;
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.SubTemplateNotFound
                    );
                }
            }

            templateRepo.UpdateAsync(houseTemplate);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.UpdateTempalte
                );
            }

            return houseTemplate;
        }
        private string GenerateDescription(PackageHouse package, DesignTemplate templ, SubTemplate sub)
        {
            return $"Gói nhà {package.Package.PackageName} với đơn giá áp dụng cho nhà phố biệt thự tiêu chuẩn. " +
                   $"Diện tích thi công: {sub.BuildingArea} m², gồm {templ.NumberOfFloor} tầng và {templ.NumberOfBed} phòng. " +
                   "Đơn giá trên chưa bao gồm VAT.";
        }

        //Create object first -> upload url image design template
        public async Task<bool> CreateImageDesignTemplate(Guid designTemplateId, ImageDesignDrawingRequest files, List<PackageHouseRequestForCreate> package)
        {
            var uploadResults = new List<string>();
            string nameImage = null;
            // Upload Overall Image
            if (files.OverallImage != null)
            {
                nameImage = AppConstant.Template.OverallDrawing;
                var overallImageUrl = await _uploadImgService.UploadFile(designTemplateId, files.OverallImage, "DesignHouse", nameImage);
                var designInfo = await _unitOfWork.GetRepository<DesignTemplate>().FirstOrDefaultAsync(x => x.Id == designTemplateId);
                designInfo.ImgUrl = overallImageUrl;
                _unitOfWork.GetRepository<DesignTemplate>().UpdateAsync(designInfo);
                await _unitOfWork.CommitAsync();
                uploadResults.Add(overallImageUrl);
            }

            // Upload OutSide Images
            foreach (var file in files.OutSideImage)
            {
                nameImage = AppConstant.Template.Exteriorsdrawings;
                var outSideImageUrl = await _uploadImgService.UploadFile(designTemplateId, file, "DesignHouse", nameImage);
                uploadResults.Add(outSideImageUrl);
            }

            // Upload Design Drawing Images
            foreach (var file in files.DesignDrawingImage)
            {
                nameImage = AppConstant.Template.Drawing;
                var designDrawingImageUrl = await _uploadImgService.UploadFile(designTemplateId, file, "DesignHouse", nameImage);
                uploadResults.Add(designDrawingImageUrl);
            }

            //Upload image package - Create package finished house
            if (files.PackageFinishedImage.Count != package.Count)
            {
                throw new Exception("Số lượng ảnh và số lượng gói package không khớp.");
            }

            for (int i = 0; i < package.Count; i++)
            {
                var file = files.PackageFinishedImage[i];
                var pack = package[i];

                nameImage = AppConstant.Template.PackageFinished[i].ToString();
                var finishedPackageImageUrl = await _uploadImgService.UploadImageFolder(file, nameImage, "PackageHouse");

                var isPackageExist = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(
                                        predicate: x => x.Id == pack.PackageId);
                if (isPackageExist == null)
                {
                    throw new Exception($"PackageId {pack.PackageId} does not exist.");
                }

                var packageItem = new PackageHouse
                {
                    Id = Guid.NewGuid(),
                    PackageId = pack.PackageId,
                    DesignTemplateId = designTemplateId,
                    InsDate = LocalDateTime.VNDateTime(),
                    Description = pack.Description,
                    ImgUrl = finishedPackageImageUrl.ToString()
                };

                await _unitOfWork.GetRepository<PackageHouse>().InsertAsync(packageItem);

                uploadResults.Add(finishedPackageImageUrl);
            }
            await _unitOfWork.CommitAsync();



            return uploadResults.Count > 0;
        }

        private async Task<string> UploadFile(Guid designTemplateId, IFormFile file, string folder, string nameImage)
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

        public async Task<string> UploadFileNoMedia(IFormFile file, string folder)
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

        public async Task<string> UploadImageSubTemplate(Guid subTemplateId, IFormFile file)
        {
            var subTemplate = await _unitOfWork.GetRepository<SubTemplate>().FirstOrDefaultAsync(
                                    predicate: x => x.Id == subTemplateId);

            if (subTemplate == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.SubTemplateNotFound);
            }

            var urlTemplate = await _mediaService.UploadImageSubTemplate(file, "DesignHouse");
            subTemplate.ImgUrl = urlTemplate.ToString();

             _unitOfWork.GetRepository<SubTemplate>().UpdateAsync(subTemplate);
            var resutl = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return resutl;
        }

    }
}
