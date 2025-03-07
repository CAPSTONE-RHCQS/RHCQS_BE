﻿using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IConstructionItemService
    {
        Task<IPaginate<ConstructionItemResponse>> GetListConstruction(int page, int size);
        Task<List<ConstructionItemResponse>> GetListConstructionRough(string type);
        Task<ConstructionItemResponse> GetDetailConstructionItem(Guid id);
        Task<IPaginate<ConstructionItemResponse>> GetDetailConstructionItemByName(string name, int page, int size);
        Task<List<AutoConstructionResponse>> GetDetailConstructionItemByContainName(string name);
        Task<List<ConstructionItemResponse>> GetListConstructionByPackageAndType(Guid packageId, string type);
        Task<bool> CreateConstructionItem(ConstructionItemRequest item);
        Task<bool> UpdateConstruction(Guid id, UpdateConstructionRequest request);
        Task<List<AutoConstructionWorkResponse>> SearchConstructionWorkByContain(Guid packageId, Guid constructionItemId, string work);
        Task<List<AutoConstructionWorkResponse>> GetConstructionWorkByPacakgeAndConstruction(Guid packageId, Guid constructionItemId);
        Task<List<AutoConstructionItemHaveWorkResponse>> SearchConstructionItemHaveWork(string name);
    }
}
