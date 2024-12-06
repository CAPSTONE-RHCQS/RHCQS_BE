using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

public class GmailSenderService : IGmailSenderService
{
    private readonly EmailSettings _emailSettings;

    public GmailSenderService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, IFormFile pdfFile)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("RHCQS", _emailSettings.Username));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder { TextBody = body };

        if (pdfFile != null && pdfFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await pdfFile.CopyToAsync(memoryStream);
                bodyBuilder.Attachments.Add(pdfFile.FileName, memoryStream.ToArray());
            }
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.AppPassword);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending email: {e.Message}");
                throw new Exception("An error occurred while sending the email: " + e.Message, e);
            }
        }
    }
}
