﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_Services.Implement
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<ProjectResponse>> GetProjects(int page, int size)
        {
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer.Username, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer),
                orderBy: x => x.OrderBy(w => w.InsDate),
                page: page,
                size: size
                );
            return listProjects;
        }

        public async Task<ProjectDetail> GetDetailProjectById(Guid id)
        {
            var projectItem = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(w => w.Id == id,
                                include: w => w.Include(p => p.Customer)
                                                .Include(p => p.InitialQuotations)
                                                    .ThenInclude(p => p.Account)
                                                .Include(p => p.FinalQuotations)
                                                    .ThenInclude(p => p.Account)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(h => h.HouseDesignVersions)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(h => h.AssignTask!));
            if (projectItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);

            if (projectItem == null) return null;

            var initialItem = projectItem.InitialQuotations?
                                        .Select(i => new InitialInfo
                                        {
                                            Id = i.Id,
                                            AccountName = i.Account?.Username,
                                            Version = i.Version,
                                            InsDate = i.InsDate,
                                            Status = i.Status
                                        }).ToList() ?? new List<InitialInfo>();

            var finalItem = projectItem.FinalQuotations?
                                        .Select(d => new FinalInfo  
                                        {
                                            Id = d.Id,
                                            AccountName = d.Account?.Username,
                                            Version = d.Version,
                                            InsDate = d.InsDate,
                                            Status = d.Status
                                        }).ToList() ?? new List<FinalInfo>();

            var houseDesignItem = projectItem.HouseDesignDrawings?
                                                                     .Select(h => new HouseDesignDrawingInfo
                                                                     {
                                                                         Id = h.Id,
                                                                         //Version = h.HouseDesignVersions,
                                                                         Name = h.Name,
                                                                         InsDate = h.InsDate,
                                                                         Status = h.Status
                                                                     }).ToList() ?? new List<HouseDesignDrawingInfo>();


            var projectDetailItem = new ProjectDetail
            {
                Id = projectItem.Id,
                Name = projectItem.Name,
                AccountName = projectItem.Customer.Username,
                Address = projectItem.Address,
                Area = projectItem.Area,
                Type = projectItem.Type,
                Status = projectItem.Status,
                InsDate = projectItem.InsDate,
                UpsDate = projectItem.UpsDate,
                ProjectCode = projectItem.ProjectCode,
                InitialInfo = initialItem,
                HouseDesignDrawingInfo = houseDesignItem,
                FinalInfo = finalItem
            };

            return projectDetailItem;
        }

        public async Task<List<ProjectResponse>> SearchProjectByPhone(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.PhoneIsEmpty);
            }

            var projectPaginate = await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer.Username, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer),
                predicate: x => x.Customer.PhoneNumber == phoneNumber,
                orderBy: x => x.OrderBy(w => w.InsDate));

            return projectPaginate.Items.ToList();
        }

        public async Task<bool> CreateProjectQuotation(ProjectRequest projectRequest)
        {
            try
            {
                var projectItem = new Project
                {
                    Id = Guid.NewGuid(),
                    CustomerId = projectRequest.CustomerId,
                    Name = projectRequest.Name,
                    Type = projectRequest.Type,
                    Status = AppConstant.ProjectStatus.PROCCESSING,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    ProjectCode = GenerateRandom.GenerateRandomString(5),
                    Address = projectRequest.Address,
                    Area = projectRequest.Area
                };
                await _unitOfWork.GetRepository<Project>().InsertAsync(projectItem);

                //Gãy
                var initialItem = new InitialQuotation
                {
                    Id = Guid.NewGuid(),
                    AccountId = projectRequest.InitialQuotation.AccountId,
                    ProjectId = projectItem.Id,
                    PromotionId = projectRequest.InitialQuotation.PromotionId,
                    PackageId = Guid.Parse("3F58EAFC-772E-4D14-9ADC-04E0736BDF2C"),
                    Area = projectRequest.Area,
                    TimeProcessing = null,
                    TimeRough = null,
                    TimeOthers = null,
                    InsDate = DateTime.Now,
                    Status = AppConstant.InitialQuotationStatus.PENDING,
                    Version = 1.0,
                    IsTemplate = false,
                    Deflag = true,
                    Note = null
                };

                //await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);
                //var packageQuotation = new PackageQuotation
                //{
                //    Id = Guid.NewGuid(),
                //    PackageId = Guid.Parse("3F58EAFC-772E-4D14-9ADC-04E0736BDF2C"),
                //    InitialQuotationId = initialItem.Id,
                //    Type = "ROUGH",
                //    InsDate = DateTime.Now
                //};

                //await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageQuotation);

                //foreach (var request in projectRequest.InitialQuotation.InitialQuotationItemRequests)
                //{
                //    var initialQuotationItem = new InitialQuotationItem
                //    {
                //        Id = Guid.NewGuid(),
                //        Name = "",
                //        ConstructionItemId = request.ConstructionItemId,
                //        SubConstructionId = request.SubConstructionId,
                //        Area = request.Area,
                //        Price = request.Price,
                //        UnitPrice = "đ",
                //        InsDate = DateTime.Now,
                //        UpsDate = DateTime.Now,
                //        InitialQuotationId = initialItem.Id
                //    };
                //    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(initialQuotationItem);
                //}

                //foreach (var utl in projectRequest.QuotationUtilitiesRequest)
                //{
                //    var utlItem = new QuotationUtility
                //    {
                //        Id = Guid.NewGuid(),
                //        UltilitiesItemId = utl.UltilitiesItemId,
                //        FinalQuotationId = null,
                //        InitialQuotationId = initialItem.Id,
                //        Name = "",
                //        Coefiicient = utl.Coefiicient,
                //        Price = utl.Price,
                //        Description = utl.Description,
                //        InsDate = DateTime.Now,
                //        UpsDate = DateTime.Now,
                //    };
                //    await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                //}

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return false;
            }
        }

    }
}
