using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
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

        Task<bool> CreateConstructionItem(ConstructionItemRequest item);
    }
}
