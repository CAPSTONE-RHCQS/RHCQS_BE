using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_Services.Implement
{
    public class HouseDesignDrawingService : IHouseDesignDrawingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseDesignDrawingService> _logger;

        public HouseDesignDrawingService(IUnitOfWork unitOfWork, ILogger<HouseDesignDrawingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<HouseDesignDrawingResponse>> GetListHouseDesignDrawings(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetList(
                        selector: x => new HouseDesignDrawingResponse(x.Id, x.ProjectId, x.Name, x.Step, x.Status,
                                                                      x.Type, x.IsCompany, x.InsDate,
                                                                      x.HouseDesignVersions.Select(
                                                                          v => new HouseDesignVersionResponse(
                                                                              v.Id,
                                                                              v.Name,
                                                                              v.Version,
                                                                              v.Status,
                                                                              v.InsDate,
                                                                              v.UpVersion,
                                                                              v.Note)).ToList()),
                        include: x => x.Include(x => x.HouseDesignVersions),
                        orderBy: x => x.OrderBy(x => x.InsDate),
                        page: page,
                        size: size
                );
            return list;
        }

        public async Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawing(Guid id)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignDrawing>().FirstOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.HouseDesignVersions)
                                .Include(x => x.AssignTask)
                );

            if (drawingItem != null)
            {
                var resutl = new HouseDesignDrawingResponse(
                       drawingItem.Id,
                       drawingItem.ProjectId,
                       drawingItem.Name,
                       drawingItem.Step,
                       drawingItem.Status,
                       drawingItem.Type,
                       drawingItem.IsCompany,
                       drawingItem.InsDate,
                       drawingItem.HouseDesignVersions.Select(version => new HouseDesignVersionResponse(
                            version.Id,
                            version.Name,
                            version.Version,
                            version.Status,
                            version.InsDate,
                            version.UpVersion,
                            version.Note
                           )).ToList()
                    );
                return resutl;
            }

            return null;
        }

        public async Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawingByType(string type)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignDrawing>().FirstOrDefaultAsync(
                predicate: x => x.Type.Equals(type),
                include: x => x.Include(x => x.HouseDesignVersions)
                                .Include(x => x.AssignTask)
                );

            if (drawingItem != null)
            {
                var resutl = new HouseDesignDrawingResponse(
                       drawingItem.Id,
                       drawingItem.ProjectId,
                       drawingItem.Name,
                       drawingItem.Step,
                       drawingItem.Status,
                       drawingItem.Type,
                       drawingItem.IsCompany,
                       drawingItem.InsDate,
                       drawingItem.HouseDesignVersions.Select(version => new HouseDesignVersionResponse(
                            version.Id,
                            version.Name,
                            version.Version,
                            version.Status,
                            version.InsDate,
                            version.UpVersion,
                            version.Note
                           )).ToList()
                    );
                return resutl;
            }

            return null;
        }

        public async Task<bool> CreateHouseDesignDrawing(HouseDesignDrawingRequest item)
        {
            int stepDrawing = 1;
            var statusDrawing = "Pending";
            foreach (DesignDrawing designType in Enum.GetValues(typeof(DesignDrawing)))
            {
                statusDrawing = designType.GetEnumDescription() == "Phối cảnh" ? "Proccessing" : "Pending";

                var houseDrawing = new HouseDesignDrawing
                {
                    Id = Guid.NewGuid(),
                    ProjectId = item.ProjectId,
                    Name = designType.GetEnumDescription(),
                    Step = stepDrawing++,
                    Status = statusDrawing,
                    Type = designType.ToTypeString(),
                    IsCompany = false,
                    InsDate = DateTime.Now
                };
                await _unitOfWork.GetRepository<HouseDesignDrawing>().InsertAsync(houseDrawing);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                return false;
            }
            return true;
        }

    }
}
