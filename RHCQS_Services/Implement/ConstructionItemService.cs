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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class ConstructionItemService : IConstructionItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConstructionItemService> _logger;

        public ConstructionItemService(IUnitOfWork unitOfWork, ILogger<ConstructionItemService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<ConstructionItemResponse>> GetListConstruction(int page, int size)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionItem>().GetList(
                selector: x => new ConstructionItemResponse(x.Id, x.Name, x.Coefficient, x.Unit,
                                                            x.InsDate, x.UpsDate, x.Type,
                                                            x.SubConstructionItems.Select(
                                                                sub => new SubConstructionItemResponse(
                                                                    sub.Id,
                                                                    sub.Name,
                                                                    sub.Coefficient,
                                                                    sub.Unit,
                                                                    sub.InsDate)).ToList()),
                include: x => x.Include(x => x.InitialQuotationItems),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);
            return listConstruction;
        }

        public async Task<List<ConstructionItemResponse>> GetListConstructionRough(string type)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionItem>().GetList(
                predicate: x => x.Type.Equals(type.ToUpper()),
                selector: x => new ConstructionItemResponse(x.Id, x.Name, x.Coefficient, x.Unit,
                                                            x.InsDate, x.UpsDate, x.Type,
                                                            x.SubConstructionItems.Select(
                                                                sub => new SubConstructionItemResponse(
                                                                    sub.Id,
                                                                    sub.Name,
                                                                    sub.Coefficient,
                                                                    sub.Unit,
                                                                    sub.InsDate)).ToList()),
                include: x => x.Include(x => x.InitialQuotationItems),
                orderBy: x => x.OrderBy(x => x.InsDate));
            return listConstruction.Items.ToList();
        }

        public async Task<ConstructionItemResponse> GetDetailConstructionItem(Guid id)
        {
            var constructionItem = await _unitOfWork.GetRepository<ConstructionItem>().FirstOrDefaultAsync(
                predicate: con => con.Id == id,
                include: con => con.Include(con => con.SubConstructionItems)
            );

            if (constructionItem != null)
            {
                return new ConstructionItemResponse(
                    constructionItem.Id,
                    constructionItem.Name,
                    constructionItem.Coefficient,
                    constructionItem.Unit,
                    constructionItem.InsDate,
                    constructionItem.UpsDate,
                    constructionItem.Type,
                    constructionItem.SubConstructionItems.Select(
                        sub => new SubConstructionItemResponse(
                            sub.Id,
                            sub.Name,
                            sub.Coefficient,
                            sub.Unit,
                            sub.InsDate
                        )
                    ).ToList()
                );
            }

            return null;
        }

        public async Task<bool> CreateConstructionItem(ConstructionItemRequest item)
        {
            try
            {
                var isCheckConstruction = await _unitOfWork.GetRepository<ConstructionItem>()
                                                .FirstOrDefaultAsync(i => i.Name.Equals(item.Name));
                if (isCheckConstruction != null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionExit);
                var constructionItem = new ConstructionItem()
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Coefficient = item.Coefficient,
                    Unit = item.Unit,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    Type = item.Type
                };
                await _unitOfWork.GetRepository<ConstructionItem>().InsertAsync(constructionItem);
                if (item.subConstructionRequests != null)
                {
                    foreach (var sub in item.subConstructionRequests)
                    {
                        var subContructionItem = new SubConstructionItem()
                        {
                            Id = Guid.NewGuid(),
                            ConstructionItemsId = constructionItem.Id,
                            Name = sub.Name,
                            Coefficient = sub.Coefficient,
                            Unit = sub.Unit,
                            InsDate = DateTime.Now,
                        };
                        await _unitOfWork.GetRepository<SubConstructionItem>().InsertAsync(subContructionItem);
                    }
                }
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionExit);
            }
        }
    }
}
