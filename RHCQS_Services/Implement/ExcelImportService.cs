using ClosedXML.Excel;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using Microsoft.EntityFrameworkCore;


namespace RHCQS_Services.Implement
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExcelImportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<EquiqmentExcelResponse>> ImportExcelAsync(Stream excelStream)
        {
            var result = new List<EquiqmentExcelResponse>();
            var seenNames = new HashSet<string>();
            var errorMessages = new List<string>();

            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);
            var materials = await _unitOfWork.GetRepository<Material>().GetListAsync(
                include: query => query.Include(m => m.MaterialSection)
            );

            foreach (var row in worksheet.RowsUsed().Skip(4))
            {
                var name = row.Cell(3).GetValue<string>();

                if (seenNames.Contains(name))
                {
                    errorMessages.Add($"Dòng {row.RowNumber()} trùng tên: {name}");
                }
                else
                {
                    var data = new EquiqmentExcelResponse
                    {
                        STT = row.Cell(1).GetValue<string>(),
                        Code = row.Cell(2).GetValue<string>(),
                        Name = name,
                        Unit = row.Cell(4).GetValue<string>(),
                        Quantity = row.Cell(5).GetValue<int?>(),
                        UnitOfMaterial = row.Cell(6).GetValue<double?>(),
                        TotalOfMaterial = row.Cell(7).GetValue<double?>(),
                        Note = row.Cell(8).GetValue<string?>(),
                        Type = row.Cell(9).GetValue<string?>()
                    };
                    var material = materials.FirstOrDefault(m => m.Code == data.Code && m.MaterialSection.Name.ToLower() == AppConstant.Equiment.EQUIPMENT.ToLower());
                    if (material == null)
                    {
                        errorMessages.Add($"Dòng {row.RowNumber()} không tìm thấy trong bảng vật tư: {name}");
                    }
                    else
                    {
                        seenNames.Add(name);
                        result.Add(data);
                    }
                }
            }

            if (errorMessages.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, errorMessages);
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
$"{errorMessage}");
            }

            return result;
        }
        public async Task<List<GroupedConstructionResponse>> ProcessWorkTemplateFileAsync(Stream excelStream, Guid packageId)
        {
            var result = new List<GroupedConstructionResponse>();
            var errorMessages = new List<string>();

            var workTemplatesDb = await _unitOfWork.GetRepository<WorkTemplate>()
                .GetListAsync(
                    predicate: wt => wt.PackageId == packageId,
                    selector: wt => new
                    {
                        wt.Id,
                        wt.ContructionWork.Code,
                        wt.InsDate,
                        wt.LaborCost,
                        wt.MaterialCost,
                        wt.MaterialFinishedCost,
                        wt.TotalCost,
                        ConstructionWorkName = wt.ContructionWork.WorkName,
                        Unit = wt.ContructionWork.Unit,
                        ConstructionItemId = wt.ContructionWork.Construction.Id,
                        ConstructionItemName = wt.ContructionWork.Construction.Name
                    }
                );

            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);
            var workTemplateListFromFile = new List<WorkTemplateExcelResponse>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var constructionWorkName = row.Cell(1).GetValue<string>().Trim();
                var code = row.Cell(2).GetValue<string>().Trim();
                var constructionName = row.Cell(3).GetValue<string>().Trim();

                var dbTemplate = workTemplatesDb.FirstOrDefault(dbTemplate =>
                    string.Equals(dbTemplate.Code, code, StringComparison.OrdinalIgnoreCase));

                if (dbTemplate == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
$"Dòng {row.RowNumber()}: Không tìm thấy WorkTemplate với Code '{code}' trong cơ sở dữ liệu.");
                }

                var workTemplate = new WorkTemplateExcelResponse
                {
                    WorkTemplateId = dbTemplate.Id,
                    Code = code,
                    ConstructionId = dbTemplate.ConstructionItemId,
                    ConstructionName = constructionName,
                    ConstructionWorkName = constructionWorkName,
                    Weight = row.Cell(4).GetValue<double>(),
                    LaborCost = row.Cell(5).GetValue<double>(),
                    MaterialCost = row.Cell(6).GetValue<double>(),
                    MaterialFinishedCost = row.Cell(7).GetValue<double>(),
                    Unit = row.Cell(8).GetValue<string>()
                };

                workTemplateListFromFile.Add(workTemplate);
            }

            if (errorMessages.Any())
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
$"Có lỗi với các dòng:\n{errorMessages}");
            }

            var groupedResult = workTemplateListFromFile
                .GroupBy(wt => new { wt.ConstructionId, wt.ConstructionName })
                .Select(group => new GroupedConstructionResponse
                {
                    ConstructionId = group.Key.ConstructionId,
                    ConstructionName = group.Key.ConstructionName,
                    WorkTemplates = group.Select(wt => new WorkTemplateExcelShow
                    {
                        WorkTemplateId = wt.WorkTemplateId,
                        Code = wt.Code,
                        ConstructionWorkName = wt.ConstructionWorkName,
                        Weight = wt.Weight,
                        LaborCost = wt.LaborCost,
                        MaterialCost = wt.MaterialCost,
                        MaterialFinishedCost = wt.MaterialFinishedCost,
                        Unit = wt.Unit

                    }).ToList()
                })
                .ToList();

            return groupedResult;
        }

    }
}
