using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet.Core;
using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Layout;
using System.IO;
using Aspose.Words;
using Xceed.Words.NET;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePDFController : ControllerBase
    {
        //private readonly IConverter _converter;

        public CreatePDFController()
        {
            //    _converter = converter;
        }



        private void AddCustomerInfoToWord(string filePath, string customerName, string customerAddress, string customerPhone, decimal price)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;

                // Tìm và thay thế các placeholder trong file Word
                foreach (var text in body.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                {
                    if (text.Text.Contains("{customer_name}"))
                    {
                        text.Text = text.Text.Replace("{customer_name}", customerName);
                    }
                    if (text.Text.Contains("{customer_address}"))
                    {
                        text.Text = text.Text.Replace("{customer_address}", customerAddress);
                    }
                    if (text.Text.Contains("{customer_phone}"))
                    {
                        text.Text = text.Text.Replace("{customer_phone}", customerPhone);
                    }
                    if (text.Text.Contains("{price}"))
                    {
                        text.Text = text.Text.Replace("{price}", price.ToString("N0"));
                    }
                }

                wordDoc.MainDocumentPart.Document.Save();
            }
        }

        //[HttpPost("generate-pdf")]
        //public IActionResult GeneratePdf([FromBody] CustomerInfo request)
        //{
        //    // Đường dẫn đến file Word mẫu
        //    string templatePath = @"D:\\hop-dong-thiet-ke-kien-truc_format.docx";

        //    // Dùng MemoryStream để lưu file PDF trong bộ nhớ
        //    using (MemoryStream pdfStream = new MemoryStream())
        //    {
        //        // Thêm thông tin vào file Word
        //        AddCustomerInfoToWord(templatePath, request.CustomerName, request.CustomerAddress, request.CustomerPhone, request.Price);

        //        // Chuyển đổi file Word sang PDF và lưu vào MemoryStream
        //        ConvertWordToPdf(templatePath, pdfStream);

        //        var pdfBytes = pdfStream.ToArray();


        //        // Trả về file PDF trực tiếp từ MemoryStream
        //        return File(pdfBytes, "application/pdf", "contract.pdf");
        //    }
        //}

        //private void ConvertWordToPdf(string wordFilePath, MemoryStream pdfStream)
        //{
        //    using (PdfWriter writer = new PdfWriter(pdfStream))
        //    {
        //        using (PdfDocument pdf = new PdfDocument(writer)) // Sử dụng using cho PdfDocument
        //        {
        //            using (iText.Layout.Document pdfDoc = new iText.Layout.Document(pdf)) // Sử dụng using cho Document của iText
        //            {
        //                // Mở file Word
        //                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(wordFilePath, false))
        //                {
        //                    var body = wordDoc.MainDocumentPart.Document.Body;

        //                    // Lấy từng đoạn văn bản trong Word
        //                    foreach (var para in body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
        //                    {
        //                        var paragraphText = para.InnerText;

        //                        // Thêm từng đoạn văn bản vào file PDF
        //                        if (!string.IsNullOrWhiteSpace(paragraphText))
        //                        {
        //                            pdfDoc.Add(new iText.Layout.Element.Paragraph(paragraphText)); // iText Paragraph
        //                        }
        //                    }
        //                }
        //            } // Document sẽ tự động đóng tại đây
        //        } // PdfDocument sẽ tự động đóng tại đây
        //    } // PdfWriter sẽ tự động đóng tại đây
        //}

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

        //    [HttpPost("generate-pdf")]
        //    public IActionResult GeneratePdf([FromBody] CustomerData data)
        //    {
        //        try
        //        {
        //            // Đường dẫn đến file Word template
        //            string templatePath = @"D:\\hop-dong-thiet-ke-kien-truc_format.docx";

        //            // Chuẩn bị dữ liệu
        //            var placeholders = new Dictionary<string, string>
        //    {
        //        { "customer_name", data.CustomerName },
        //        { "customer_address", data.CustomerAddress },
        //        { "customer_phone", data.CustomerPhone },
        //        { "price", data.Price.ToString("C") } // Convert price thành dạng tiền tệ
        //    };

        //            // Điền dữ liệu vào file Word và lưu vào MemoryStream
        //            using (var outputDocxStream = new MemoryStream())
        //            {
        //                try
        //                {
        //                    FillTemplate(templatePath, outputDocxStream, placeholders);
        //                }
        //                catch (Exception ex)
        //                {
        //                    return StatusCode(500, "Error filling Word template.");
        //                }

        //                outputDocxStream.Position = 0; // Reset vị trí của stream trước khi sử dụng

        //                // Chuyển file Word thành PDF và lưu vào MemoryStream
        //                using (var outputPdfStream = new MemoryStream())
        //                {
        //                    try
        //                    {
        //                        ConvertToPdf(outputDocxStream, outputPdfStream);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        return StatusCode(500, "Error converting Word to PDF.");
        //                    }

        //                    outputPdfStream.Position = 0; // Reset vị trí của stream

        //                    // Trả về file PDF cho client
        //                    return File(outputPdfStream.ToArray(), "application/pdf", "output.pdf");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //    }

        //    // Hàm thay thế các placeholder trong template Word
        //    private void FillTemplate(string templatePath, Stream outputStream, Dictionary<string, string> data)
        //    {
        //        using (DocX document = DocX.Load(templatePath))
        //        {
        //            foreach (var item in data)
        //            {
        //                document.ReplaceText($"{{{{{item.Key}}}}}", item.Value); // Thay thế các placeholder
        //            }

        //            document.SaveAs(outputStream); // Lưu lại file đã được điền thông tin vào stream
        //        }
        //    }

        //    // Hàm chuyển file Word sang PDF bằng Aspose.Words
        //    private void ConvertToPdf(Stream docxStream, Stream pdfStream)
        //    {
        //        Document doc = new Document(docxStream); // Tạo Document từ MemoryStream
        //        doc.Save(pdfStream, SaveFormat.Pdf); // Chuyển Word thành PDF và lưu vào stream
        //    }
        //}


        // Model nhận dữ liệu từ request body
    }
}
