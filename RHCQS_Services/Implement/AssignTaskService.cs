﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class AssignTaskService : IAssignTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAssignTaskService> _logger;

        public AssignTaskService(IUnitOfWork unitOfWork, ILogger<IAssignTaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //public async Task<IPaginate<AssignTaskResponse>> GetListAssignTaskAll(int page, int size)
        //{
        //    var listTask = await _unitOfWork.GetRepository<AssignTask>().GetList(
        //        selector: a => new AssignTaskResponse (a.Id, a.AccountId, a.Account.Username, a.Name, a.Status, a.InsDate),
        //        include: a => a.Include( a => a.Account),
        //        page: page,
        //        size: size
        //        );
        //    return listTask;
        //}

        //public async Task<bool> AssignWork(List<AssignTaskRequest> request)
        //{
        //    foreach(var itemTask in request)
        //    {
        //        var task = new AssignTask
        //        {
        //            Id = Guid.NewGuid(),
        //            AccountId = itemTask.AccountId,
        //            Name = "",
        //            Status = "Pending",
        //            InsDate = LocalDateTime.VNDateTime()
        //        };
        //        await _unitOfWork.GetRepository<AssignTask>().InsertAsync(task);

        //        var drawingItem = await _unitOfWork.GetRepository<HouseDesignDrawing>()
        //            .FirstOrDefaultAsync(x => x.Id.Equals(itemTask.HouseDesignDrawingId));

        //        //Update AssignTaskId in table HouseDesignDrawing
        //        if (drawingItem != null)
        //        {
        //            drawingItem.AssignTaskId = task.Id;
        //            _unitOfWork.GetRepository<HouseDesignDrawing>().UpdateAsync(drawingItem);
        //        }
        //    }


        //    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        //    if (isSuccessful)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<List<DesignStaffWorkResponse>> ListDesignStaffWorkAvailable()
        {
            var listDesign = await _unitOfWork.GetRepository<Account>().GetListAsync(
                predicate: x => x.RoleId == Guid.Parse("7AF0D75E-1157-48B4-899D-3196DEED5FAD"),
                include: x => x.Include(x => x.AssignTasks!)
                               .ThenInclude(assignTask => assignTask.Project!)
                               .Include(x => x.Role)
            );

            var listResult = listDesign
                .Where(account => account.AssignTasks
                    .Select(assignTask => assignTask.ProjectId)
                    .Distinct().Count() < 2)
                .Select(account => new DesignStaffWorkResponse
                {
                    Id = account.Id,
                    ImgUrl = account.ImageUrl,
                    Name = account.Username!,
                    RoleName = account.Role.RoleName!,
                    Phone = account.PhoneNumber
                })
                .ToList();

            return listResult;
        }

    }
}
