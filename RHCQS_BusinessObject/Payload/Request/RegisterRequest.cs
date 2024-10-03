﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc phải có.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc phải có.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc phải có.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoleForRegister
    {
        Customer,
        Manager,
        SalesStaff,
        DesignStaff
    }
}
