﻿using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task<Account> GetAccountByEmail(string email, string password);
        Task<Account> RegisterAsync(RegisterRequest registerRequest, UserRoleForRegister role);
        TokenResponse DecodeToken(string token);
        Task<string> RefreshTokenAsync(string expiredToken);
    }
}
