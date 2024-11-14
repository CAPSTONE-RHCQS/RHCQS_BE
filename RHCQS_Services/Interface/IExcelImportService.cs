using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IExcelImportService
    {
        Task<List<EquiqmentExcelResponse>> ImportExcelAsync(Stream excelStream);
    }
}
