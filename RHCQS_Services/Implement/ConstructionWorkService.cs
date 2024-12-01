using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class ConstructionWorkService : IConstructionWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConstructionWorkService> _logger;

        public ConstructionWorkService(IUnitOfWork unitOfWork, ILogger<ConstructionWorkService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        public async Task<IPaginate<ListConstructionWorkResponse>> GetListConstructionWork(int page, int size)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionWork>().GetList(
                selector: x => new ListConstructionWorkResponse(x.Id, x.WorkName, x.ConstructionId,x.InsDate, x.Unit,
                                                            x.Code),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);
            return listConstruction;
        }

        public async Task<ConstructionWorkItemResponse> GetConstructionWorkDetail(Guid workId)
        {
            var workInfo = await _unitOfWork.GetRepository<ConstructionWork>().FirstOrDefaultAsync(
                    predicate: x => x.Id == workId,
                    include: x => x.Include(x => x.ConstructionWorkResources));
            if (workInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Construction_Work_Not_Found);
            }
            var response = new ConstructionWorkItemResponse
            {
                Id = workInfo.Id,
                WorkName = workInfo.WorkName,
                ConstructionId = workInfo.ConstructionId,
                InsDate = workInfo.InsDate,
                Unit = workInfo.Unit,
                Code = workInfo.Code,
                Resources = workInfo.ConstructionWorkResources.Select(resource => new ConstructionWorkResourceItem
                {
                    Id = resource.Id,
                    MaterialSectionId = resource.MaterialSectionId,
                    MaterialSectionNorm = resource.MaterialSectionNorm,
                    LaborId = resource.LaborId,
                    LaborNorm = resource.LaborNorm,
                    InsDate = resource.InsDate
                }).ToList()
            };

            return response;
        }
        public async Task<List<ListConstructionWorkResponse>> GetListConstructionWorkByConstructionId(Guid constructionId)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionWork>().GetList(
                selector: x => new ListConstructionWorkResponse(x.Id, x.WorkName, x.ConstructionId, x.InsDate, x.Unit, x.Code),
                predicate: x => x.ConstructionId == constructionId,
                orderBy: x => x.OrderBy(x => x.InsDate)
            );

            return listConstruction.Items.ToList();
        }

    }
}
