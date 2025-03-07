﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IGmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, IFormFile pdfFile);
    }
}
