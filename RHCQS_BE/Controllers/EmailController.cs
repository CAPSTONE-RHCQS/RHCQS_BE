using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IGmailSenderService _gmailSenderService;

        public EmailController(IGmailSenderService gmailSenderService)
        {
            _gmailSenderService = gmailSenderService;
        }

        [HttpPost(ApiEndPointConstant.Email.SendEmailEndpoint)]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null)
            {
                return BadRequest("Invalid email request");
            }

            try
            {
                await _gmailSenderService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
                return Ok("Email sent successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Error sending email: " + e.Message);
            }
        }
    }
}
