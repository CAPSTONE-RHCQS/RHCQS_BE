﻿using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response.Project;
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
        Task<IPaginate<ProjectResponse>> GetListProjectBySalesStaff(Guid accountId, int page, int size);
        Task<ProjectDetail> GetDetailProjectById(Guid id);
        Task<List<ProjectResponse>> SearchProjectByPhone(string phoneNumber);

        Task<bool> CreateProjectQuotation(ProjectRequest projectRequest);
        Task<List<ProjectResponse>> GetListProjectByEmail(string email);

        Task<string> AssignQuotation(Guid accountId, Guid initialQuotationId);

        Task<bool> CancelProject(Guid projectId);
        Task<ProjectAppResponse> TrackingProject(Guid projectId);

    }
}
