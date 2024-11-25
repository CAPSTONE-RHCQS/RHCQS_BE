using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.Utility;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObject.Payload.Response.Utility;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;


namespace RHCQS_Services.Implement
{
    public class UtilitiesService : IUtilitiesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UtilitiesService> _logger;

        public UtilitiesService(IUnitOfWork unitOfWork, ILogger<UtilitiesService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<UtilityResponse>> GetListUtilities(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<UtilityOption>().GetList(
                    selector: x => new UtilityResponse(x.Id, x.Name, x.Type, x.Deflag, x.InsDate, x.UpsDate,
                                                       x.UtilitiesSections.Select(s => new UtilitiesSectionResponse(
                                                           s.Id,
                                                           s.Name,
                                                           s.Deflag,
                                                           s.InsDate,
                                                           s.UpsDate,
                                                           s.Description,
                                                           s.UnitPrice,
                                                           s.Unit
                )).ToList()),
                    page: page,
                    size: size);
            return list;
        }

        public async Task<List<UtilityResponse>> GetListUtilitiesByType(string type)
        {
            if (AppConstant.Type.ROUGH == type || AppConstant.Type.FINISHED == type)
            {
                var listConstruction = await _unitOfWork.GetRepository<UtilityOption>().GetList(
               predicate: x => x.Type!.Equals(type.ToUpper()),
               selector: x => new UtilityResponse(x.Id, x.Name, x.Type, x.Deflag, x.InsDate,
                                                           x.UpsDate,
                                                           x.UtilitiesSections.Select(
                                                               s => new UtilitiesSectionResponse(
                                                                   s.Id,
                                                                   s.Name,
                                                                   s.Deflag,
                                                                   s.InsDate,
                                                                   s.UpsDate,
                                                                   s.Description,
                                                                   s.UnitPrice,
                                                                   s.Unit)).ToList()),
               include: x => x.Include(x => x.UtilitiesSections),
               orderBy: x => x.OrderBy(x => x.InsDate));
                return listConstruction.Items.ToList();
            }
            else
            {
                var listConstruction = await _unitOfWork.GetRepository<UtilityOption>().GetList(
               selector: x => new UtilityResponse(x.Id, x.Name, x.Type, x.Deflag, x.InsDate,
                                                           x.UpsDate,
                                                           x.UtilitiesSections.Select(
                                                               s => new UtilitiesSectionResponse(
                                                                   s.Id,
                                                                   s.Name,
                                                                   s.Deflag,
                                                                   s.InsDate,
                                                                   s.UpsDate,
                                                                   s.Description,
                                                                   s.UnitPrice,
                                                                   s.Unit)).ToList()),
               include: x => x.Include(x => x.UtilitiesSections),
               orderBy: x => x.OrderBy(x => x.InsDate));
                return listConstruction.Items.ToList();
            }

        }

        public async Task<UtilitiesSectionResponse> SearchUtilityItem(string name)
        {
            var utiSection = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(
                predicate: con => con.Name!.ToUpper() == name.ToUpper(),
                include: con => con.Include(con => con.UtilitiesItems)
            );

            if (utiSection == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);

            }

            var utilityItemsResponse = utiSection.UtilitiesItems.Select(item =>
                                     new UtilityItemResponse(
                                         item.Id,
                                         item.Name,
                                         item.Coefficient,
                                         item.InsDate,
                                         item.UpsDate
                                     )
                                 ).ToList();

            return new UtilitiesSectionResponse(
                utiSection.Id,
                utiSection.Name,
                utiSection.Deflag,
                utiSection.InsDate,
                utiSection.UpsDate,
                utiSection.Description,
                utiSection.UnitPrice,
                utiSection.Unit,
                utilityItemsResponse
            );
        }

        public async Task<UtilityResponse> GetDetailUtilityItem(Guid id)
        {
            var utiItem = await _unitOfWork.GetRepository<UtilityOption>().FirstOrDefaultAsync(
                predicate: con => con.Id == id,
                include: con => con.Include(con => con.UtilitiesSections)
            );

            if (utiItem != null)
            {
                return new UtilityResponse(
                    utiItem.Id,
                    utiItem.Name,
                    utiItem.Type,
                    utiItem.Deflag,
                    utiItem.InsDate,
                    utiItem.UpsDate,
                    utiItem.UtilitiesSections.Select(
                        s => new UtilitiesSectionResponse(
                             s.Id,
                             s.Name,
                             s.Deflag,
                             s.InsDate,
                             s.UpsDate,
                             s.Description,
                             s.UnitPrice,
                             s.Unit)).ToList()
                );
            }

            return null;
        }

        public async Task<UtilitiesSectionResponse> GetDetailUtilitySection(Guid idUtilitySection)
        {
            var utiItem = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(
                x => x.Id == idUtilitySection,
                include: x => x.Include(x => x.UtilitiesItems)
            );

            if (utiItem != null)
            {
                return new UtilitiesSectionResponse(
                    utiItem.Id,
                    utiItem.Name,
                    utiItem.Deflag,
                    utiItem.InsDate,
                    utiItem.UpsDate,
                    utiItem.Description,
                    utiItem.UnitPrice,
                    utiItem.Unit,
                    utiItem.UtilitiesItems?.Select(
                        s => new UtilityItemResponse(
                            s.Id,
                            s.Name,
                            s.Coefficient,
                            s.InsDate,
                            s.UpsDate
                        )
                    ).ToList() ?? new List<UtilityItemResponse>()
                );
            }

            return null;
        }

        public async Task<bool> CreateUtility(UtilityRequest request)
        {
            try
            {
                //Create all utility - section - item
                UtilityOption utility = request.Id.HasValue ? await _unitOfWork.GetRepository<UtilityOption>().FirstOrDefaultAsync(x => x.Id == request.Id)
                   : new UtilityOption
                   {
                       Id = Guid.NewGuid(),
                       Name = request.Name,
                       Type = request.Type,
                       Deflag = true,
                       InsDate = LocalDateTime.VNDateTime(),
                       UpsDate = LocalDateTime.VNDateTime()
                   };
                if (!request.Id.HasValue)
                {
                    await _unitOfWork.GetRepository<UtilityOption>().InsertAsync(utility);
                }

                foreach (var s in request.Sections!)
                {
                    UtilitiesSection section = s.Id.HasValue ? await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(x => x.Id == s.Id)
                        : new UtilitiesSection
                        {
                            Id = Guid.NewGuid(),
                            UtilitiesId = utility.Id,
                            Name = s.Name,
                            Deflag = true,
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime(),
                            Description = s.Description,
                            UnitPrice = s.UnitPrice,
                            Unit = s.Unit,
                        };

                    if (!s.Id.HasValue)
                    {
                        await _unitOfWork.GetRepository<UtilitiesSection>().InsertAsync(section);
                    }

                    if (request.Items != null)
                    {
                        foreach (var i in request.Items!)
                        {
                            var item = new UtilitiesItem
                            {
                                Id = Guid.NewGuid(),
                                SectionId = section.Id,
                                Name = i.Name,
                                Coefficient = i.Coefficient,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                Deflag = true
                            };
                            await _unitOfWork.GetRepository<UtilitiesItem>().InsertAsync(item);
                        }
                    }
                    else
                    {
                        continue;
                    }

                }

                var isCreate = await _unitOfWork.CommitAsync() > 0;
                return isCreate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateUtility(UpdateUtilityRequest request)
        {
            if (request.Utility != null)
            {
                var utility = await _unitOfWork.GetRepository<UtilityOption>().FirstOrDefaultAsync(u => u.Id == request.Utility.Id);
                if (utility == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);
                }
                var listNameUti = await _unitOfWork.GetRepository<UtilityOption>()
                                  .GetList(predicate: u => u.Name!.ToLower() == request.Utility.Name!.ToLower(),
                                  selector: u => new UpdateUtilityOptionRequest
                                  {
                                      Id = u.Id,
                                      Name = u.Name
                                  });


                if (listNameUti.Items.Count > 0)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Duplicate);
                }

                utility.Name = request.Utility.Name ?? utility.Name;
                utility.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<UtilityOption>().UpdateAsync(utility);

            }
            if (request.Sections != null)
            {
                var section = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == request.Sections.Id);
                if (section == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);
                }
                var listNameSection = await _unitOfWork.GetRepository<UtilitiesSection>()
                                    .GetList(predicate: u => u.Name!.ToLower() == request.Sections.Name!.ToLower(),
                                              selector: u => new UpdateUtilityOptionRequest
                                              {
                                                  Id = u.Id,
                                                  Name = u.Name
                                              });
                if (listNameSection.Items.Count > 0)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Duplicate);
                }

                section.Name = request.Sections.Name ?? section.Name;
                section.Description = request.Sections.Description ?? section.Description;
                section.UpsDate = LocalDateTime.VNDateTime();
                section.UnitPrice = request.Sections.UnitPrice ?? section.UnitPrice;
                section.Unit = request.Sections.Unit ?? section.Unit;

                _unitOfWork.GetRepository<UtilitiesSection>().UpdateAsync(section);
            }

            if (request.Items != null)
            {
                var item = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == request.Items.Id);
                if (item == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);
                }
                var listNameItem = await _unitOfWork.GetRepository<UtilitiesItem>()
                                    .GetList(predicate: u => u.Name!.ToLower() == request.Items.Name!.ToLower(),
                                              selector: u => new UpdateUtilityOptionRequest
                                              {
                                                  Id = u.Id,
                                                  Name = u.Name
                                              });
                if (listNameItem.Items.Count > 0)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Duplicate);
                }

                item.Name = request.Items.Name ?? item.Name;
                item.Coefficient = request.Items.Coefficient ?? item.Coefficient;
                item.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<UtilitiesItem>().UpdateAsync(item);
            }

            bool isUpdate = await _unitOfWork.CommitAsync() > 0;
            return isUpdate;
        }

        public async Task<string> BanUtility(Guid utilityId)
        {
            var utilityInfo = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == utilityId);

            if (utilityInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);
            }
            utilityInfo.Deflag = !utilityInfo.Deflag;

            _unitOfWork.GetRepository<UtilitiesItem>().UpdateAsync(utilityInfo);

            var save = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Fail_Save;
            return save;
        }

        public async Task<List<AutoUtilityResponse>> GetDetailUtilityByContainName(string name)
        {
            try
            {
                var normalizedName = name.RemoveDiacritics().ToLower();

                var utilitySections = await _unitOfWork.GetRepository<UtilitiesSection>()
                    .GetListAsync(include: con => con.Include(c => c.UtilitiesItems));

                var filteredItems = utilitySections.SelectMany(utilitySection =>
                {
                    var matchingUtilityItems = utilitySection.UtilitiesItems
                        .Where(item => !string.IsNullOrEmpty(item.Name) &&
                                       item.Name.RemoveDiacritics().ToLower().Contains(normalizedName))
                        .Select(utilityItem => new AutoUtilityResponse(
                            utilitySectionId: utilityItem.SectionId,
                            utilityItemId: utilityItem.Id,
                            name: utilityItem.Name!,
                            coefficient: utilityItem.Coefficient,
                            unitPrice: 0
                        )).ToList();

                    if (!matchingUtilityItems.Any() && !string.IsNullOrEmpty(utilitySection.Name) &&
                        utilitySection.Name.RemoveDiacritics().ToLower().Contains(normalizedName))
                    {
                        matchingUtilityItems.Add(new AutoUtilityResponse(
                            utilitySectionId: utilitySection.Id,
                            utilityItemId: null,
                            name: utilitySection.Name!,
                            coefficient: 0,
                            unitPrice: utilitySection.UnitPrice
                        ));
                    }

                    return matchingUtilityItems;
                }).ToList();

                return filteredItems;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching utility details: {ex.Message}");
            }
        }
    }
}
