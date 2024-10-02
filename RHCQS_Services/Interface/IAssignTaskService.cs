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
    public interface IAssignTaskService
    {
        Task<IPaginate<AssignTaskResponse>> GetListAssignTaskAll(int page, int size);
        Task<bool> AssignWork(List<AssignTaskRequest> request);
    }
}
