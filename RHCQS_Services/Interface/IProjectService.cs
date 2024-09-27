using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IProjectService
    {
        Task<IPaginate<ProjectResponse>> GetProjects(int page, int size);
        Task<ProjectDetail> GetDetailProjectById(Guid id);
        Task<List<ProjectResponse>> SearchProjectByPhone(string phoneNumber);
    }
}
