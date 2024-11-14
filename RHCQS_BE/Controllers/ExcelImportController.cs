using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
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
    }
}
