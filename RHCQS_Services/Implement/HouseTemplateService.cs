using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class HouseTemplateService : IHouseTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseTemplateService> _logger;

        public HouseTemplateService(IUnitOfWork unitOfWork, ILogger<HouseTemplateService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
                    x.NumberOfFront,
                    x.ImgUrl,
                    x.InsDate,
                    x.PackageHouses.Select(
                        pgk => new PackageHouseResponse(
                            pgk.Id,
                            pgk.DesignTemplateId,
                            pgk.ImgUrl,
                            pgk.InsDate
                        )).ToList(),
                    x.SubTemplates.Select(
                        sub => new SubTemplatesResponse(
                            sub.Id,
                            sub.BuildingArea,
                            sub.FloorArea,
                            sub.InsDate,
                            sub.Size,
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
                            sub.Media.Select(media => new MediaResponse(
                                media.Id,
                                media.Name,
                                media.Url,
                                media.InsDate,
                                media.UpsDate
                            )).ToList()
                        )
                    ).ToList()
                ),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );

            return listHouseTemplate.ToList();
        }

        public async Task<bool> CreateHouseTemplate(HouseTemplateRequestForCretae templ)
        {
            if (templ == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

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
                NumberOfFront = templ.NumberOfFront,
                ImgUrl = templ.ImgURL,
                InsDate = DateTime.Now,
                SubTemplates = templ.SubTemplates.Select(sub => new SubTemplate
                {
                    Id = Guid.NewGuid(),
                    BuildingArea = sub.BuildingArea,
                    FloorArea = sub.FloorArea,
                    Size = sub.Size,
                    InsDate = DateTime.Now,
                    TemplateItems = sub.TemplateItems.Select(item => new TemplateItem
                    {
                        Id = Guid.NewGuid(),
                        ConstructionItemId = item.ConstructionItemId,
                        SubConstructionId = item.SubConstructionItemId,
                        Area = item.Area,
                        Unit = item.Unit,
                        InsDate = DateTime.Now,
                    }).ToList(),
                    Media = sub.Media.Select(media => new Medium
                    {
                        Id = Guid.NewGuid(),
                        Name = media.Name,
                        Url = media.MediaImgURL,
                        InsDate = DateTime.Now,
                    }).ToList()
                }).ToList()
            };

            await _unitOfWork.GetRepository<DesignTemplate>().InsertAsync(houseTemplate);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
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
                               .Include(x => x.PackageHouses)
            );

            if (template != null)
            {
                var result = new HouseTemplateResponse(
                    template.Id,
                    template.Name,
                    template.Description,
                    template.NumberOfFloor,
                    template.NumberOfBed,
                    template.NumberOfFront,
                    template.ImgUrl,
                    template.InsDate,
                    template.PackageHouses.Select(pkg => new PackageHouseResponse(
                        pkg.Id,
                        pkg.DesignTemplateId,
                        pkg.ImgUrl,
                        pkg.InsDate
                    )).ToList(),
                    template.SubTemplates.Select(sub => new SubTemplatesResponse(
                        sub.Id,
                        sub.BuildingArea,
                        sub.FloorArea,
                        sub.InsDate,
                        sub.Size,
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
                        sub.Media.Select(media => new MediaResponse(
                        media.Id,
                        media.Name,
                        media.Url,
                        media.InsDate,
                        media.UpsDate
                    )).ToList()
                    )).ToList()
                );
                return result;
            }
            return null;
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
                    x.NumberOfFront,
                    x.ImgUrl,
                    x.InsDate,
                    x.PackageHouses.Select(
                        pgk => new PackageHouseResponse(
                            pgk.Id,
                            pgk.DesignTemplateId,
                            pgk.ImgUrl,
                            pgk.InsDate
                        )).ToList(),
                    x.SubTemplates.Select(
                        sub => new SubTemplatesResponse(
                            sub.Id,
                            sub.BuildingArea,
                            sub.FloorArea,
                            sub.InsDate,
                            sub.Size,
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
                            sub.Media.Select(media => new MediaResponse(
                                    media.Id,
                                    media.Name,
                                    media.Url,
                                    media.InsDate,
                                    media.UpsDate
                            )).ToList()
                        )
                    ).ToList()
                ),
                include: x => x.Include(x => x.PackageHouses),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);

            return listHouseTemplate;
        }


        public async Task<DesignTemplate> SearchHouseTemplateByNameAsync(string name)
        {
            var houseTemplate = await _unitOfWork.GetRepository<DesignTemplate>()
                .FirstOrDefaultAsync(p => p.Name.Contains(name), include: s => s.Include(p => p.SubTemplates));
            if (houseTemplate == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_HouseTemplate
                );
            }
            return houseTemplate;
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
            houseTemplate.NumberOfFront = templ.NumberOfFront;
            houseTemplate.ImgUrl = templ.ImgURL;

            foreach (var sub in templ.SubTemplates)
            {
                var existingSubTemplate = houseTemplate.SubTemplates.FirstOrDefault(st => st.Id == sub.Id);
                if (existingSubTemplate != null)
                {
                    existingSubTemplate.BuildingArea = sub.BuildingArea;
                    existingSubTemplate.FloorArea = sub.FloorArea;
                    existingSubTemplate.Size = sub.Size;

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
                    foreach (var media in sub.Media)
                    {
                        var existingMedia = existingSubTemplate.Media.FirstOrDefault(m => m.SubTemplateId == sub.Id);
                        if (existingMedia != null)
                        {
                            existingMedia.Name = media.Name;
                            existingMedia.Url = media.MediaImgURL;
                            existingMedia.UpsDate = DateTime.Now;
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
    }
}
