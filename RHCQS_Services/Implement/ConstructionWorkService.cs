using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.ConstructionWork;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
                selector: x => new ListConstructionWorkResponse(x.Id, x.WorkName, x.ConstructionId, x.InsDate, x.Unit,
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
                    include: x => x.Include(x => x.ConstructionWorkResources)
                                        .ThenInclude(x => x.MaterialSection)
                                    .Include(x => x.ConstructionWorkResources)
                                        .ThenInclude(x => x.Labor)
                                    .Include(x => x.WorkTemplates)
                                      .ThenInclude(x => x.Package!));
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
                    MaterialSectionName = resource.MaterialSection?.Name ?? null,
                    MaterialSectionNorm = resource.MaterialSectionNorm,
                    LaborId = resource.LaborId,
                    LaborName = resource.Labor?.Name ?? null,
                    LaborNorm = resource.LaborNorm,
                    InsDate = resource.InsDate
                }).ToList(),
                WorkTemplates = workInfo.WorkTemplates.Select(work => new WorkTemplateItem
                {
                    Id = work.Id,
                    PackageId = (Guid)work.PackageId!,
                    PackageName = work.Package.PackageName ?? null,
                    LaborCost = work.LaborCost ?? 0.0,
                    MaterialCost = work.MaterialCost ?? 0.0,
                    MaterialFinishedCost = work.MaterialFinishedCost ?? 0.0,
                    TotalCost = work.TotalCost ?? 0.0,
                    InsDate = work.InsDate
                }).ToList()
            };

            return response;
        }
        public async Task<WorkTemplateItem> GetConstructionWorkPrice(Guid workId)
        {
            var workInfo = await _unitOfWork.GetRepository<WorkTemplate>().FirstOrDefaultAsync(
                    predicate: x => x.ContructionWorkId == workId);
            if (workInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Construction_Work_Not_Found);
            }
            var response = new WorkTemplateItem
            {
                Id = workInfo.Id,
                LaborCost = workInfo.LaborCost,
                MaterialCost = workInfo.MaterialCost,
                MaterialFinishedCost = workInfo.MaterialFinishedCost,
                InsDate = workInfo.InsDate
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

        public async Task<string> CreateConstructionWork(CreateConstructionWorkRequest request)
        {
            try
            {
                var constructionWorkItem = new ConstructionWork
                {
                    Id = Guid.NewGuid(),
                    WorkName = request.WorkName,
                    ConstructionId = request.ConstructionId ?? null,
                    InsDate = LocalDateTime.VNDateTime(),
                    Unit = request.Unit,
                    Code = request.Code ?? GenerateRandom.GenerateRandomString(5),
                };

                await _unitOfWork.GetRepository<ConstructionWork>().InsertAsync(constructionWorkItem);

                var constructionResources = new List<ConstructionWorkResource>();

                foreach (var item in request.Resources)
                {
                    if (item.MaterialSectionId != null)
                    {
                        constructionResources.Add(new ConstructionWorkResource
                        {
                            Id = Guid.NewGuid(),
                            ConstructionWorkId = constructionWorkItem.Id,
                            MaterialSectionId = item.MaterialSectionId,
                            MaterialSectionNorm = item.MaterialSectionNorm,
                            LaborId = null,
                            LaborNorm = null,
                            InsDate = LocalDateTime.VNDateTime()
                        });
                    }
                    else
                    {
                        constructionResources.Add(new ConstructionWorkResource
                        {
                            Id = Guid.NewGuid(),
                            ConstructionWorkId = constructionWorkItem.Id,
                            MaterialSectionId = null,
                            MaterialSectionNorm = null,
                            LaborId = item.LaborId,
                            LaborNorm = item.LaborNorm,
                            InsDate = LocalDateTime.VNDateTime()
                        });
                    }
                }

                await _unitOfWork.GetRepository<ConstructionWorkResource>().InsertRangeAsync(constructionResources);

                var result = await _unitOfWork.CommitAsync() > 0 ?
                    AppConstant.Message.SUCCESSFUL_CREATE : AppConstant.ErrMessage.Fail_Save;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> CreateWorkTemplate(List<CreateWorkTemplateRequest> request)
        {
            var workTemplateItems = new List<WorkTemplate>();

            foreach (var item in request)
            {
                var totalCost = (item.LaborCost ?? 0) + (item.MaterialCost ?? 0) + (item.MaterialFinishedCost ?? 0);

                var workTemplateItem = new WorkTemplate
                {
                    Id = Guid.NewGuid(),
                    PackageId = item.PackageId,
                    InsDate = LocalDateTime.VNDateTime(),
                    ContructionWorkId = item.ConstructionWorkId,
                    LaborCost = item.LaborCost ?? 0,
                    MaterialCost = item.MaterialCost ?? 0,
                    MaterialFinishedCost = item.MaterialFinishedCost ?? 0,
                    TotalCost = totalCost
                };

                workTemplateItems.Add(workTemplateItem);
            }

            await _unitOfWork.GetRepository<WorkTemplate>().InsertRangeAsync(workTemplateItems);

            var result = await _unitOfWork.CommitAsync() > 0
                ? AppConstant.Message.SUCCESSFUL_CREATE
                : AppConstant.ErrMessage.Fail_Save;

            return result;
        }
    }
}
