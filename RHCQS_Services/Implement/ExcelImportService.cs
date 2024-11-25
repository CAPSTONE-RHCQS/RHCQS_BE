using ClosedXML.Excel;
using RHCQS_BusinessObject.Payload.Response;
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
        public async Task<List<EquiqmentExcelResponse>> ImportExcelAsync(Stream excelStream)
        {
            var result = new List<EquiqmentExcelResponse>();
            var seenNames = new HashSet<string>();
            var errorMessages = new List<string>(); // Danh sách lưu trữ các lỗi

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


    }
}
