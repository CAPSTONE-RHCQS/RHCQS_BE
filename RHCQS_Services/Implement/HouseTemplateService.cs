using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
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

        public async Task<IPaginate<HouseTemplateResponse>> GetListHouseTemplateAsync(int page, int size)
        {
            var listHouseTemplate = await _unitOfWork.GetRepository<DesignTemplate>().GetList(
                selector: x => new HouseTemplateResponse(x.Id, x.Name, x.Description, x.NumberOfFloor, x.NumberOfBed,
                                                            x.NumberOfFront, x.ImgUrl, x.InsDate,
                                                            x.SubTemplates.Select(
                                                                sub => new SubTemplatesResponse(
                                                                    sub.Id,
                                                                    sub.BuildingArea,
                                                                    sub.FloorArea,
                                                                    sub.Size,
                                                                    sub.InsDate,
                                                                    (List<TemplateItemReponse>)sub.TemplateItems.Select(
                                                                        item => new TemplateItemReponse(
                                                                            item.Id,
                                                                            item.ConstructionItem.Name,
                                                                            item.ConstructionItem.Coefficient,
                                                                            item.Area,
                                                                            item.Unit,
                                                                            item.InsDate)))).ToList()),
                include: x => x.Include(x => x.PackageHouses),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);
            return listHouseTemplate;
        }

        public async Task<DesignTemplate> SearchHouseTemplateByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return await _unitOfWork.GetRepository<DesignTemplate>().FirstOrDefaultAsync(include: s => s.Include(p => p.SubTemplates));
                }
                else
                {
                    return await _unitOfWork.GetRepository<DesignTemplate>().FirstOrDefaultAsync(p => p.Name.Contains(name), include: s => s.Include(p => p.SubTemplates));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the housetemplate by name.");
                throw;
            }
        }
    }
}
