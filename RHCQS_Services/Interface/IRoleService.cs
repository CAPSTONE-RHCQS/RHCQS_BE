﻿using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IRoleService
    {
        Task<IPaginate<RoleResponse>> GetAllRolesAsync(int page, int size);
        Task<int> GetTotalRoleCountAsync();
        Task<Role> GetRoleByIdAsync(Guid? id);
    }
}
