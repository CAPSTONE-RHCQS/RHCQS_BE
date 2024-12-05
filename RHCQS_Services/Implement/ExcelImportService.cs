using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

                    seenNames.Add(name);
                    result.Add(data);
                }
            }

            if (errorMessages.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, errorMessages);
                throw new Exception($"Có lỗi với các dòng trùng tên:\n{errorMessage}");
            }

            return result;
        }
        public async Task<List<GroupedConstructionResponse>> ProcessWorkTemplateFileAsync(Stream excelStream, Guid packageId)
        {
            var result = new List<GroupedConstructionResponse>();
            var seenIds = new HashSet<Guid>();
            var errorMessages = new List<string>();

            // Lấy danh sách từ cơ sở dữ liệu
            var workTemplatesDb = await _unitOfWork.GetRepository<WorkTemplate>()
                .GetListAsync(
                    predicate: wt => wt.PackageId == packageId,
                    selector: wt => new
                    {
                        wt.Id,
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

            // Lấy danh sách từ file Excel
            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheet(1);
            var workTemplateListFromFile = new List<WorkTemplateExcelResponse>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
//                var workTemplateIdValue = row.Cell(1).Value.ToString();
//                if (!Guid.TryParse(workTemplateIdValue, out Guid workTemplateId))
//                {
//                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
//$"Dòng {row.RowNumber()} có giá trị WorkTemplateId không hợp lệ: {workTemplateIdValue}");
//                    continue;
//                }

                var constructionWorkName = row.Cell(1).GetValue<string>();
//                var constructionIdValue = row.Cell(3).Value.ToString();
//                if (!Guid.TryParse(constructionIdValue, out Guid constructionId))
//                {
//                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
//$"Dòng {row.RowNumber()} có giá trị ConstructionId không hợp lệ: {constructionIdValue}");
//                    continue;
//                }

                var constructionName = row.Cell(2).GetValue<string>();

//                if (seenIds.Contains(workTemplateId))
//                {
//                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
//$"Dòng {row.RowNumber()} trùng WorkTemplateId: {workTemplateId}");
//                }
                //else
                //{
                    var workTemplate = new WorkTemplateExcelResponse
                    {
                        ConstructionWorkName = constructionWorkName,
                        ConstructionName = constructionName,
                        Weight = row.Cell(3).GetValue<double>(),
                        LaborCost = row.Cell(4).GetValue<double>(),
                        MaterialCost = row.Cell(5).GetValue<double>(),
                        MaterialFinishedCost = row.Cell(6).GetValue<double>(),
                        Unit = row.Cell(7).GetValue<string>()
                    };
                var dbTemplate = workTemplatesDb.FirstOrDefault(dbTemplate =>
                    string.Equals(dbTemplate.ConstructionItemName, constructionName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(dbTemplate.ConstructionWorkName, constructionWorkName, StringComparison.OrdinalIgnoreCase));

                if (dbTemplate != null)
                {
                    // Gán ConstructionItemId và WorkTemplateId
                    workTemplate.ConstructionId = dbTemplate.ConstructionItemId;
                    workTemplate.WorkTemplateId = dbTemplate.Id; // Đây là WorkTemplateId từ cơ sở dữ liệu
                }
                //seenIds.Add(workTemplateId);
                workTemplateListFromFile.Add(workTemplate);
                //}
            }

            var differences = workTemplatesDb
                .Where(dbTemplate => !workTemplateListFromFile.Any(fileTemplate =>
                    fileTemplate.ConstructionName == dbTemplate.ConstructionItemName))
                .ToList();

            if (differences.Any())
            {
                foreach (var difference in differences)
                {
                    var fileTemplate = workTemplateListFromFile.FirstOrDefault(fileTemplate =>
                        fileTemplate.ConstructionName == difference.ConstructionItemName);

                    if (fileTemplate != null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
$"Mẫu công việc {difference.ConstructionItemName} không khớp với tên: {difference.ConstructionItemName} (File: {fileTemplate.ConstructionName})");
                    }
                    else
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict,
$"Không tìm thấy mẫu công việc trong file: {difference.ConstructionWorkName}");
                    }
                }
            }
            // Nếu có lỗi, ném exception
            if (errorMessages.Any())
            {
                var errorMessage = string.Join(Environment.NewLine, errorMessages);
                throw new Exception($"Có lỗi với các dòng:\n{errorMessage}");
            }

            // Group lại các WorkTemplate theo ConstructionId và ConstructionName
            var groupedResult = workTemplateListFromFile
                .GroupBy(wt => new { wt.ConstructionId, wt.ConstructionName })
                .Select(group => new GroupedConstructionResponse
                {
                    ConstructionId = group.Key.ConstructionId,
                    ConstructionName = group.Key.ConstructionName,
                    WorkTemplates = group.Select(wt => new WorkTemplateExcelShow
                    {
                        WorkTemplateId = wt.WorkTemplateId,
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
