using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Payload.Request.Utility;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObject.Payload.Request.Utility.UpdateUtilityRequest;

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
                                                           s.Description
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
                                                                   s.Description)).ToList()),
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
                                                                   s.Description)).ToList()),
               include: x => x.Include(x => x.UtilitiesSections),
               orderBy: x => x.OrderBy(x => x.InsDate));
                return listConstruction.Items.ToList();
            }
           
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
                             s.Description)).ToList()
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
                       InsDate = DateTime.Now,
                       UpsDate = DateTime.Now
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
                            InsDate = DateTime.Now,
                            UpsDate = DateTime.Now,
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
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
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

        public async Task<bool> UpdateUtility(UpdateRequest request)
        {
            if (request.Utility != null) { 
                var utility = await _unitOfWork.GetRepository<UtilityOption>().FirstOrDefaultAsync(u => u.Id == request.Utility.Id);
                if (utility == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Utility_Not_Found);
                }
                var listNameUti = await _unitOfWork.GetRepository<UtilityOption>()
                                    .GetList(u => u.Name == request.Utility.Name);

                utility.Name = request.Utility.Name;
                utility.UpsDate = DateTime.Now;

            }
            if (request.Sections != null)
            {

            }

            if (request.Items != null)
            {

            }
            return true;
        }
    }
}
