﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class DeviceTokenRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; }
        [Required(ErrorMessage = "DeviceToken là bắt buộc")]
        public string DeviceToken { get; set; }
    }
}
