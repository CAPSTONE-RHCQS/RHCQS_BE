using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using iText.IO.Font;
using Microsoft.Azure.WebJobs;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Text;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePDFController : ControllerBase
    {
        private readonly IConverter _converter;

        public CreatePDFController(IConverter converter)
        {
            _converter = converter;
        }

        //[HttpPost("generate-drink")]
        //public IActionResult GeneratePdf([FromBody] QuotationRequest request)
        //{
        //    try
        //    {
        //        // Tạo HTML dựa trên dữ liệu nhận được
        //        var htmlContent = GenerateHtmlContent(request);

        //        var doc = new HtmlToPdfDocument()
        //        {
        //            GlobalSettings = {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4
        //    },
        //            Objects = {
        //        new ObjectSettings() {
        //            PagesCount = true,
        //            HtmlContent = htmlContent,
        //            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = null }
        //        }
        //    }
        //        };

        //        var pdf = _converter.Convert(doc);
        //        return File(pdf, "application/pdf", "GeneratedDocument.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}





        public class QuotationRequest
        {
            public string ProjectName { get; set; }
            public string ClientName { get; set; }
            public string ProjectLocation { get; set; }
            public string ProjectScale { get; set; } // Quy mô công trình
            public decimal BasePrice { get; set; } // Đơn giá thi công phần thô trước thuế
            public List<Item> Items { get; set; } // Danh sách các hạng mục xây dựng
            public decimal TotalArea { get; set; } // Tổng diện tích xây dựng
            public decimal UnitPrice { get; set; } // Đơn giá trên m²
            public decimal GrossPrice { get; set; } // Thành tiền tổng trước thuế
            public decimal ConcreteReinforcementCost { get; set; } // Chi phí gia cố nền trệt
            public decimal UnitOthers {  get; set; }
            public decimal Discount { get; set; } // Khuyến mãi thành viên
            public decimal ContractValueBeforeTax { get; set; } // Giá trị hợp đồng trước thuế
            public decimal ContractValueAfterDiscount { get; set; } // Giá trị hợp đồng sau khuyến mãi
            public List<PaymentRequest> PaymentSchedule { get; set; } // Lịch thanh toán
            public int ConstructionDuration { get; set; } // Thời gian hoàn thành công trình
            public int RoughConstructionDuration { get; set; } // Thời gian thi công phần thô
        }

        public class Item
        {
            public int Index { get; set; }
            public string Description { get; set; }
            public decimal Area { get; set; } // Diện tích m²
            public decimal Coefficient { get; set; } // Hệ số
            public decimal TotalArea { get; set; } // Diện tích tổng m²
        }

        public class PaymentRequest
        {
            public int Stage { get; set; } // Đợt thanh toán
            public string Description { get; set; } // Nội dung thanh toán
            public decimal Percentage { get; set; } // Giá trị thanh toán (%)
            public decimal Amount { get; set; } // Giá trị thanh toán (VND)
        }


        //[HttpGet("generate-itext")]
        //public IActionResult GeneratePdfIText()
        //{
        //    // Define the file path for the generated PDF
        //    string fileName = "GeneratedDocument.pdf";
        //    var memoryStream = new MemoryStream();

        //    // Create a new PDF writer to write to memory stream
        //    PdfWriter writer = new PdfWriter(memoryStream);
        //    PdfDocument pdfDoc = new PdfDocument(writer);
        //    Document document = new Document(pdfDoc);

        //    // Use a relative path based on the application's root directory
        //    //string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "Fonts", "times new roman.ttf");
        //    string fontPath = @"D:\Graduates\RHCQS_BE\Fonts\Times New Roman\times new roman.ttf";
        //    string fontNotoPath = @"D:\Graduates\RHCQS_BE\Fonts\NotoSans\static\NotoSans-Regular.ttf";


        //    // Check if the file exists at the specified path for debugging
        //    if (!System.IO.File.Exists(fontNotoPath))
        //    {
        //        throw new IOException($"Font file not found at path: {fontPath}");
        //    }
        //    PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);


        //    // Set font for the document
        //    document.SetFont(font);

        //    // Add content to the PDF
        //    document.Add(new Paragraph("THIET THACH Group").SetBold().SetFontSize(18));
        //    document.Add(new Paragraph("NHÀ Ở RIÊNG LẺ").SetFontSize(14));
        //    document.Add(new Paragraph("Q2"));
        //    document.Add(new Paragraph("ANH NHÀN"));

        //    // Section for construction details
        //    document.Add(new Paragraph("2.2. DIỆN TÍCH XÂY DỰNG THEO PHƯƠNG ÁN THIẾT KẾ:").SetBold().SetFontSize(14));

        //    // Create a table with 5 columns
        //    Table table = new Table(5);
        //    table.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100));

        //    // Add header row
        //    table.AddHeaderCell("STT");
        //    table.AddHeaderCell("Hạng mục");
        //    table.AddHeaderCell("D-Tích");
        //    table.AddHeaderCell("Hệ số");
        //    table.AddHeaderCell("Diện tích (m²)");

        //    // Add data rows
        //    string[,] data = new string[,]
        //    {
        //        { "1", "Móng:", "117", "0.3", "35.1" },
        //        { "2", "Lầu trệt:", "117", "1", "117" },
        //        { "3", "Sân:", "169", "0.6", "101.4" },
        //        { "4", "Lỗ trống lầu 1:", "22.65", "0.5", "11.325" },
        //        { "5", "Lầu 1:", "110.05", "1", "110.05" },
        //        { "6", "Lầu 2:", "132.27", "1", "132.27" },
        //        { "7", "Mái BTCT:", "166", "0.5", "83" }
        //    };

        //    for (int i = 0; i < data.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < data.GetLength(1); j++)
        //        {
        //            table.AddCell(new Cell().Add(new Paragraph(data[i, j])));
        //        }
        //    }

        //    // Add the table to the document
        //    document.Add(table);
        //    document.Add(new Paragraph("Tổng diện tích xây dựng theo thiết kế: 590.15 m²").SetBold());

        //    // Section for construction value
        //    document.Add(new Paragraph("2.3. GIÁ TRỊ THI CÔNG PHẦN THÔ TRƯỚC THUẾ:").SetBold().SetFontSize(14));
        //    document.Add(new Paragraph("Tổng diện tích xây dựng x Đơn giá = Thành tiền"));
        //    document.Add(new Paragraph("590.15 x 3,350,000 = 1,976,985,750"));

        //    // Close the document
        //    document.Close();

        //    // Return the PDF file as a stream
        //    var pdfBytes = memoryStream.ToArray();

        //    // Return the PDF file as a stream
        //    return File(pdfBytes, "application/pdf", fileName);
        //}
    }
}
