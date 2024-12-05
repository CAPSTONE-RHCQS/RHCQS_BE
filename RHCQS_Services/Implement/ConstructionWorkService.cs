using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.Y2022.FeaturePropertyBag;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.ConstructionWork;
using RHCQS_BusinessObject.Payload.Response.Construction;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;


namespace RHCQS_Services.Implement
{
    public class ConstructionWorkService : IConstructionWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConstructionWorkService> _logger;

        public ConstructionWorkService(IUnitOfWork unitOfWork, ILogger<ConstructionWorkService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<ListConstructionWorkResponse>> GetListConstructionWork(int page, int size)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionWork>().GetList(
                selector: x => new ListConstructionWorkResponse(x.Id, x.WorkName, x.ConstructionId, x.InsDate, x.Unit,
                                                            x.Code),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);
            return listConstruction;
        }

        public async Task<ConstructionWorkItemResponse> GetConstructionWorkDetail(Guid workId)
        {
            var workInfo = await _unitOfWork.GetRepository<ConstructionWork>().FirstOrDefaultAsync(
                    predicate: x => x.Id == workId,
                    include: x => x.Include(x => x.ConstructionWorkResources)
                                        .ThenInclude(x => x.MaterialSection)
                                    .Include(x => x.ConstructionWorkResources)
                                        .ThenInclude(x => x.Labor)
                                    .Include(x => x.WorkTemplates)
                                      .ThenInclude(x => x.Package!)
                                      .Include(x => x.Construction!));
            if (workInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Construction_Work_Not_Found);
            }
            var response = new ConstructionWorkItemResponse
            {
                ConstructionItemName = workInfo.Construction.Name!,
                Id = workInfo.Id,
                WorkName = workInfo.WorkName,
                ConstructionId = workInfo.ConstructionId,
                InsDate = workInfo.InsDate,
                Unit = workInfo.Unit,
                Code = workInfo.Code,
                Resources = workInfo.ConstructionWorkResources.Select(resource => new ConstructionWorkResourceItem
                {
                    Id = resource.Id,
                    MaterialSectionId = resource.MaterialSectionId,
                    MaterialSectionName = resource.MaterialSection?.Name ?? null,
                    MaterialSectionNorm = resource.MaterialSectionNorm,
                    LaborId = resource.LaborId,
                    LaborName = resource.Labor?.Name ?? null,
                    LaborNorm = resource.LaborNorm,
                    InsDate = resource.InsDate
                }).ToList(),
                WorkTemplates = workInfo.WorkTemplates.Select(work => new WorkTemplateItem
                {
                    Id = work.Id,
                    PackageId = (Guid)work.PackageId!,
                    PackageName = work.Package.PackageName ?? null,
                    LaborCost = work.LaborCost ?? 0.0,
                    MaterialCost = work.MaterialCost ?? 0.0,
                    MaterialFinishedCost = work.MaterialFinishedCost ?? 0.0,
                    TotalCost = work.TotalCost ?? 0.0,
                    InsDate = work.InsDate
                }).ToList()
            };

            return response;
        }
        public async Task<WorkTemplateItem> GetConstructionWorkPrice(Guid workId)
        {
            var workInfo = await _unitOfWork.GetRepository<WorkTemplate>().FirstOrDefaultAsync(
                    predicate: x => x.ContructionWorkId == workId);
            if (workInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Construction_Work_Not_Found);
            }
            var response = new WorkTemplateItem
            {
                Id = workInfo.Id,
                LaborCost = workInfo.LaborCost,
                MaterialCost = workInfo.MaterialCost,
                MaterialFinishedCost = workInfo.MaterialFinishedCost,
                InsDate = workInfo.InsDate
            };

            return response;
        }
        public async Task<List<ListConstructionWorkResponse>> GetListConstructionWorkByConstructionId(Guid constructionId)
        {
            var listConstruction = await _unitOfWork.GetRepository<ConstructionWork>().GetList(
                selector: x => new ListConstructionWorkResponse(x.Id, x.WorkName, x.ConstructionId, x.InsDate, x.Unit, x.Code),
                predicate: x => x.ConstructionId == constructionId,
                orderBy: x => x.OrderBy(x => x.InsDate)
            );

            return listConstruction.Items.ToList();
        }

        public async Task<Guid> CreateConstructionWork(CreateConstructionWorkRequest request)
        {
            try
            {
                var findConstruction = await _unitOfWork.GetRepository<ConstructionWork>().FirstOrDefaultAsync(
                                        predicate: c => c.Code!.ToLower() == request.Code!.ToLower()
                                               || c.WorkName!.ToLower() == request.WorkName!.ToLower());
                if (findConstruction != null && request.Code!.ToLower() == findConstruction.Code!.ToLower())
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ConstructionWorkExisting);
                }

                if (findConstruction != null && request.WorkName.ToLower() == findConstruction.WorkName!.ToLower())
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.Duplicated_WorkName);
                }

                var constructionWorkItem = new ConstructionWork
                {
                    Id = Guid.NewGuid(),
                    WorkName = request.WorkName,
                    ConstructionId = request.ConstructionId ?? null,
                    InsDate = LocalDateTime.VNDateTime(),
                    Unit = request.Unit,
                    Code = request.Code ?? GenerateRandom.GenerateRandomString(5),
                };

                await _unitOfWork.GetRepository<ConstructionWork>().InsertAsync(constructionWorkItem);

                foreach (var item in request.Resources)
                {
                    ConstructionWorkResource constructionResource;

                    if (item.MaterialSectionId != null)
                    {
                        constructionResource = new ConstructionWorkResource
                        {
                            Id = Guid.NewGuid(),
                            ConstructionWorkId = constructionWorkItem.Id,
                            MaterialSectionId = item.MaterialSectionId,
                            MaterialSectionNorm = item.MaterialSectionNorm,
                            LaborId = null,
                            LaborNorm = null,
                            InsDate = LocalDateTime.VNDateTime()
                        };
                    }
                    else
                    {
                        constructionResource = new ConstructionWorkResource
                        {
                            Id = Guid.NewGuid(),
                            ConstructionWorkId = constructionWorkItem.Id,
                            MaterialSectionId = null,
                            MaterialSectionNorm = null,
                            LaborId = item.LaborId,
                            LaborNorm = item.LaborNorm,
                            InsDate = LocalDateTime.VNDateTime()
                        };
                    }
                    await _unitOfWork.GetRepository<ConstructionWorkResource>().InsertAsync(constructionResource);
                }

                var result = await _unitOfWork.CommitAsync() > 0 ?
                    AppConstant.Message.SUCCESSFUL_CREATE : AppConstant.ErrMessage.Fail_Save;
                return constructionWorkItem.Id;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError((int)ex.Code, ex.Message);
            }
        }

        public async Task<string> CreateWorkTemplate(List<CreateWorkTemplateRequest> request)
        {
            var workTemplateItems = new List<WorkTemplate>();

            foreach (var item in request)
            {
                var isDuplicate = await _unitOfWork.GetRepository<WorkTemplate>()
                 .GetList(
                     selector: x => x,
                     predicate: w => w.PackageId == item.PackageId && w.ContructionWorkId == item.ConstructionWorkId,
                     orderBy: null,
                     include: null
                 );


                if (isDuplicate.Items.Any())
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.No_New_Data);
                }

                var totalCost = (item.LaborCost ?? 0) + (item.MaterialCost ?? 0) + (item.MaterialFinishedCost ?? 0);

                var workTemplateItem = new WorkTemplate
                {
                    Id = Guid.NewGuid(),
                    PackageId = item.PackageId,
                    InsDate = LocalDateTime.VNDateTime(),
                    ContructionWorkId = item.ConstructionWorkId,
                    LaborCost = item.LaborCost ?? 0,
                    MaterialCost = item.MaterialCost ?? 0,
                    MaterialFinishedCost = item.MaterialFinishedCost ?? 0,
                    TotalCost = totalCost
                };

                workTemplateItems.Add(workTemplateItem);
            }

            if (!workTemplateItems.Any())
            {
                return AppConstant.ErrMessage.No_New_Data;
            }

            await _unitOfWork.GetRepository<WorkTemplate>().InsertRangeAsync(workTemplateItems);

            var result = await _unitOfWork.CommitAsync() > 0
                ? AppConstant.Message.SUCCESSFUL_CREATE
                : AppConstant.ErrMessage.Fail_Save;

            return result;
        }

        public async Task<string> ImportFileConstructionWork(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return "File is invalid.";
            }

            var constructionWorkItems = new List<ConstructionWork>();
            var constructionWorkResources = new List<ConstructionWorkResource>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        if (worksheet == null)
                        {
                            return "The file does not contain a valid worksheet.";
                        }

                        int rowCount = worksheet.Dimension.Rows;
                        string currentStt = null;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var stt = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            if (!string.IsNullOrEmpty(stt) && stt != currentStt)
                            {
                                var workCode = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                var category = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                                var workName = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                                var unit = worksheet.Cells[row, 6].Value?.ToString()?.Trim();


                                if (string.IsNullOrEmpty(workCode) || string.IsNullOrEmpty(workName))
                                {
                                    continue;
                                }

                                var existingWork = await _unitOfWork.GetRepository<ConstructionWork>()
                                    .FirstOrDefaultAsync(x => x.Code == workCode);

                                Guid constructionWorkId;
                                if (existingWork != null)
                                {
                                    existingWork.WorkName = workName;
                                    existingWork.Unit = unit;
                                    existingWork.InsDate = LocalDateTime.VNDateTime();
                                    _unitOfWork.GetRepository<ConstructionWork>().UpdateAsync(existingWork);


                                    constructionWorkId = existingWork.Id;
                                }
                                else
                                {
                                    Guid? constructionId = null;
                                    if (!string.IsNullOrEmpty(category))
                                    {
                                        var categoryEntity = await _unitOfWork.GetRepository<ConstructionItem>()
                                            .FirstOrDefaultAsync(x => x.Name == category);
                                        if (categoryEntity != null)
                                        {
                                            constructionId = categoryEntity.Id;
                                        }
                                        else
                                        {
                                            throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                                                AppConstant.ErrMessage.ConstructionIdNotfound);
                                        }
                                    }
                                    var constructionWorkItem = new ConstructionWork
                                    {
                                        Id = Guid.NewGuid(),
                                        WorkName = workName,
                                        ConstructionId = constructionId,
                                        Unit = unit,
                                        Code = workCode,
                                        InsDate = LocalDateTime.VNDateTime()
                                    };
                                    constructionWorkItems.Add(constructionWorkItem);
                                    constructionWorkId = constructionWorkItem.Id;
                                }

                                for (int subRow = row + 1; subRow <= rowCount; subRow++)
                                {
                                    var resourceCode = worksheet.Cells[subRow, 4].Value?.ToString()?.Trim();
                                    var normValue = worksheet.Cells[subRow, 7].Value?.ToString()?.Trim();

                                    if (string.IsNullOrEmpty(resourceCode))
                                    {
                                        break;
                                    }

                                    Guid? materialSectionId = null;
                                    Guid? laborId = null;
                                    double? materialSectionNorm = null;
                                    double? laborNorm = null;

                                    if (resourceCode.StartsWith("V"))
                                    {
                                        var materialSection = await _unitOfWork.GetRepository<MaterialSection>()
                                            .FirstOrDefaultAsync(x => x.Code.Trim().ToLower().Contains(resourceCode.Trim().ToLower()));

                                        if (materialSection == null)
                                        {
                                            throw new AppConstant.MessageError(
                                            (int)AppConstant.ErrCode.NotFound,
                                            $"Không tìm thấy mã vật tư: {resourceCode}!");
                                        }
                                        materialSectionId = materialSection?.Id;

                                        if (double.TryParse(normValue, out double parsedNorm))
                                        {
                                            materialSectionNorm = parsedNorm;
                                        }
                                    }
                                    else if (resourceCode.StartsWith("N"))
                                    {
                                        var labor = await _unitOfWork.GetRepository<Labor>()
                                            .FirstOrDefaultAsync(x => x.Code.Trim().ToLower().Contains(resourceCode.Trim().ToLower()));
                                        laborId = labor?.Id;

                                        if (labor == null)
                                        {
                                            throw new AppConstant.MessageError(
                                                 (int)AppConstant.ErrCode.NotFound,
                                                 $"Không tìm thấy mã nhân công: {resourceCode}!");
                                        }
                                        // Lấy định mức cho nhân công
                                        if (double.TryParse(normValue, out double parsedNorm))
                                        {
                                            laborNorm = parsedNorm;
                                        }
                                    }

                                    // Kiểm tra trùng lặp trong database hoặc danh sách tạm thời
                                    bool isDuplicateInDatabase = false;

                                    // Nếu công tác đã tồn tại trong database
                                    if (existingWork != null)
                                    {
                                        if (materialSectionId.HasValue) // Trường hợp là vật tư
                                        {
                                            isDuplicateInDatabase = await _unitOfWork.GetRepository<ConstructionWorkResource>()
                                                .AnyAsync(x =>
                                                    x.ConstructionWorkId == constructionWorkId &&
                                                    x.MaterialSectionId == materialSectionId);
                                        }

                                        if (!isDuplicateInDatabase && laborId.HasValue) // Trường hợp là nhân công
                                        {
                                            isDuplicateInDatabase = await _unitOfWork.GetRepository<ConstructionWorkResource>()
                                                .AnyAsync(x =>
                                                    x.ConstructionWorkId == constructionWorkId &&
                                                    x.LaborId == laborId);
                                        }
                                    }
                                    else
                                    {
                                        if (materialSectionId.HasValue)
                                        {
                                            isDuplicateInDatabase = constructionWorkResources
                                                .Any(x =>
                                                    x.ConstructionWorkId == constructionWorkItems.LastOrDefault()?.Id &&
                                                    x.MaterialSectionId == materialSectionId);
                                        }

                                        if (!isDuplicateInDatabase && laborId.HasValue) 
                                        {
                                            isDuplicateInDatabase = constructionWorkResources
                                                .Any(x =>
                                                    x.ConstructionWorkId == constructionWorkItems.LastOrDefault()?.Id &&
                                                    x.LaborId == laborId);
                                        }
                                    }

                                    if (isDuplicateInDatabase)
                                    {
                                        continue; 
                                    }

                                    var constructionResource = new ConstructionWorkResource
                                    {
                                        Id = Guid.NewGuid(),
                                        ConstructionWorkId = constructionWorkId,
                                        MaterialSectionId = materialSectionId,
                                        MaterialSectionNorm = materialSectionNorm,
                                        LaborId = laborId,
                                        LaborNorm = laborNorm,
                                        InsDate = LocalDateTime.VNDateTime()
                                    };

                                    constructionWorkResources.Add(constructionResource);
                                }
                            }
                        }
                    }
                }
                await _unitOfWork.GetRepository<ConstructionWork>().InsertRangeAsync(constructionWorkItems);
                await _unitOfWork.GetRepository<ConstructionWorkResource>().InsertRangeAsync(constructionWorkResources);

                var result = await _unitOfWork.CommitAsync() > 0
                    ? AppConstant.Message.SUCCESSFUL_CREATE
                    : AppConstant.ErrMessage.Fail_Save;

                return result;
            }
            catch (AppConstant.MessageError ex)
            {

                throw new AppConstant.MessageError((int)ex.Code, ex.Message);
            }
        }
    }
}
