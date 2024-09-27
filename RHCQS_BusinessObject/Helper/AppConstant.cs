﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RHCQS_BusinessObjects;

public static class AppConstant
{
    public class MessageError : Exception
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public MessageError(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
    public class Role
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Staff = "Staff";
        public const string Customer = "Customer";
    }
    public enum ErrCode
    {
        Success = 200,
        Internal_Server_Error = 500,
        Not_Found = 404,
        Conflict = 409,
    }
    public class ErrMessage
    {
        public const string Internal_Server_Error = "Hệ thống xảy ra lỗi, vui lòng thử lại";
        public const string Oops = "Oops !!! Something Wrong. Try Again.";
        public const string Not_Found_Resource = "The server can not find the requested resource.";
        public const string Bad_Request = "The server could not understand the request due to invalid syntax.";
        public const string Unauthorized = "Unauthorized.";
        public const string PromotionIllegal = "Khuyến mãi không hợp lệ";
        public const string ProjectNotExit = "Dự án không tồn tại!";
        public const string PhoneIsEmpty = "Nhập lại số điện thoại của khách hàng";
    }
}

