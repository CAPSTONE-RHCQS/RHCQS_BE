using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
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
            var listConstruction = new List<ConstructionItemResponse>();

            Expression<Func<ConstructionItem, bool>> predicate = x => true;

            if (type == AppConstant.Type.ROUGH)
            {

                var listPaginate = await _unitOfWork.GetRepository<ConstructionItem>().GetListAsync(
                    predicate: x => x.Type == type,
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

                listConstruction = listPaginate.ToList();

                return listConstruction;
            }
            else if (type == AppConstant.Type.FINISHED)
            {
                var listPaginate = await _unitOfWork.GetRepository<ConstructionItem>().GetListAsync(
                    predicate: x => x.Type == type && x.IsFinalQuotation == true,
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

                listConstruction = listPaginate.ToList();

                return listConstruction;
            }
            else
            {
                var listPaginate = await _unitOfWork.GetRepository<ConstructionItem>().GetListAsync(
                    predicate: x => (bool)x.IsFinalQuotation! == false,
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

                listConstruction = listPaginate.ToList();

                return listConstruction;
            }
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

            return new ConstructionItemResponse();
        }

        public async Task<ConstructionItemResponse> GetDetailConstructionItemByName(string name)
        {
            var constructionItem = await _unitOfWork.GetRepository<ConstructionItem>().FirstOrDefaultAsync(
                predicate: con => con.Name!.Equals(name),
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

            return new ConstructionItemResponse();
        }

        public async Task<List<AutoConstructionResponse>> GetDetailConstructionItemByContainName(string type, string name)
        {
            try
            {
                var normalizedName = name.RemoveDiacritics().ToLower();

                var constructionItems = await _unitOfWork.GetRepository<ConstructionItem>()
                    .GetListAsync(predicate: con => con.Type.ToLower() == type.ToLower(),
                    include: con => con.Include(c => c.SubConstructionItems));

                var filteredItems = constructionItems
                  .Where(con =>
                      (!string.IsNullOrEmpty(con.Name) && con.Name.RemoveDiacritics().ToLower().Contains(normalizedName)) ||
                      con.SubConstructionItems.Any(sub => sub.Name.RemoveDiacritics().ToLower().Contains(normalizedName)))
                  .ToList();

                return filteredItems.SelectMany(constructionItem =>
                {

                    var matchingSubConstructions = constructionItem.SubConstructionItems
                        .Where(sub => sub.Name.RemoveDiacritics().ToLower().Contains(normalizedName))
                        .Select(subConstruction => new AutoConstructionResponse(
                            constructionItem.Id,
                            subConstructionId: subConstruction.Id,
                            name: subConstruction.Name,
                            coefficient: subConstruction.Coefficient
                        )).ToList();

                    if (!matchingSubConstructions.Any() && constructionItem.Name!.RemoveDiacritics().Contains(normalizedName))
                    {
                        matchingSubConstructions.Add(new AutoConstructionResponse(
                            constructionItem.Id,
                            subConstructionId: null,
                            name: constructionItem.Name,
                            coefficient: constructionItem.Coefficient
                        ));
                    }

                    return matchingSubConstructions;
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateConstructionItem(ConstructionItemRequest item)
        {
            try
            {
                var isCheckConstruction = await _unitOfWork.GetRepository<ConstructionItem>()
                                                .FirstOrDefaultAsync(i => i.Name!.Equals(item.Name));
                if (isCheckConstruction != null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.ConstructionExit);
                var constructionItem = new ConstructionItem()
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Coefficient = item.Coefficient,
                    Unit = item.Unit,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    Type = item.Type,
                    IsFinalQuotation = item.IsFinalQuotation
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
                            InsDate = LocalDateTime.VNDateTime(),
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
            catch (Exception)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionExit);
            }
        }

        public async Task<bool> UpdateConstruction(Guid id, UpdateConstructionRequest request)
        {
            var constructionItem = await _unitOfWork.GetRepository<ConstructionItem>().FirstOrDefaultAsync(
                x => x.Id == id,
                include: x => x.Include(x => x.SubConstructionItems)
            );

            if (constructionItem == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionNoExit);
            }

            var existingItems = await _unitOfWork.GetRepository<ConstructionItem>()
                .GetList(x => new { x.Id, x.Name }, x => x.Name == request.Name);

            var existingItemsList = existingItems.Items.ToList();

            if (existingItemsList.Any(item => item.Id != constructionItem.Id))
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionNameExit);
            }

            // Update 
            constructionItem.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : constructionItem.Name;

            if (constructionItem.SubConstructionItems == null || !constructionItem.SubConstructionItems.Any())
            {
                constructionItem.Coefficient = request.Coefficient;
            }

            _unitOfWork.GetRepository<ConstructionItem>().UpdateAsync(constructionItem);

            if (request.SubRequests != null && request.SubRequests.Any())
            {
                foreach (var subRequest in request.SubRequests)
                {
                    var existingSubItem = constructionItem.SubConstructionItems!.FirstOrDefault(x => x.Id == subRequest.Id);

                    if (existingSubItem != null && existingSubItem.Name != subRequest.Name)
                    {
                        existingSubItem.Name = subRequest.Name;
                        existingSubItem.Coefficient = subRequest.Coefficient;
                    }
                    else if (existingSubItem != null && existingSubItem.Name == subRequest.Name)
                    {
                        existingSubItem.Coefficient = subRequest.Coefficient;
                    }
                    else
                    {
                        continue;
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionNameExit);
                    }
                    _unitOfWork.GetRepository<SubConstructionItem>().UpdateAsync(existingSubItem);
                }
            }

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
