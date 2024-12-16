using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelImportController : ControllerBase
    {
        private readonly IExcelImportService _excelImportService;

        public ExcelImportController(IExcelImportService excelImportService)
        {
            _excelImportService = excelImportService;
        }
        #region Import equiment
        /// <summary>
        /// Import excel equiment
        /// </summary>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPost(ApiEndPointConstant.EquiqmentExcel.EquimentExcelEndpoint)]
        [ProducesResponseType(typeof(EquiqmentExcelResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var stream = file.OpenReadStream();
            var data = await _excelImportService.ImportExcelAsync(stream);

            var dataList = JsonConvert.SerializeObject(data, Formatting.Indented);
            return new ContentResult
            {
                Content = dataList,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region Import Worktemplate
        /// <summary>
        /// Import excel worktemplate
        /// </summary>
        #endregion
        [Authorize(Roles = "SalesStaff")]
        [HttpPost(ApiEndPointConstant.EquiqmentExcel.WorkTemplateExcelEndpoint)]
        [ProducesResponseType(typeof(EquiqmentExcelResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> WorkTemplateExcel(Guid packageid,IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var workTemplates = await _excelImportService.ProcessWorkTemplateFileAsync(stream, packageid);

            var dataList = JsonConvert.SerializeObject(workTemplates, Formatting.Indented);
            return new ContentResult
            {
                Content = dataList,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
