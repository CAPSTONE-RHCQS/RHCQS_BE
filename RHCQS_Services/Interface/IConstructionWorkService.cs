using RHCQS_BusinessObject.Payload.Request.ConstructionItem;
using RHCQS_BusinessObject.Payload.Request.ConstructionWork;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IConstructionWorkService
    {
        Task<IPaginate<ListConstructionWorkResponse>> GetListConstructionWork(int page, int size);
        Task<ConstructionWorkItemResponse> GetConstructionWorkDetail(Guid workId);
        Task<List<ListConstructionWorkResponse>> GetListConstructionWorkByConstructionId(Guid constructionId);
        Task<WorkTemplateItem> GetConstructionWorkPrice(Guid workId);
        Task<Guid> CreateConstructionWork(CreateConstructionWorkRequest request);
        Task<string> CreateWorkTemplate(List<CreateWorkTemplateRequest> request);
    }
}
