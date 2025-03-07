﻿using Microsoft.EntityFrameworkCore;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Repositories.Repo.Implement
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }
    }
}
