﻿using RHCQS_BusinessObject.Payload;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.CurrentUserModel;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IAccountService
    {
        Task<Account> RegisterForStaffAsync(RegisterRequest registerRequest, UserRoleForManagerRegister selectedrole);
        Task<SendEmailAndNotiReponse> GetEmailByQuotationIdAsync(Guid quotationId);
        Task<string> GetEmailByProjectIdAsync(Guid projectId);
        Task<SendEmailAndNotiReponse> GetEmailByProjectIdForSendNotiAsync(Guid versionId);
        Task<AccountCustomerResponse> GetAccountOrCustomerByIdAsync(Guid id);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task<int> GetActiveAccountCountAsync();
        public Task<IPaginate<AccountResponse>> GetListAccountAsync(int page, int size);
        public Task<IPaginate<AccountResponse>> GetListAccountByRoleIdAsync(Guid id,int page, int size);
        Task<Account> SearchAccountsByNameAsync(string name);
        Task<IPaginate<AccountResponse>> SearchAccountsByKeyAsync(string key, int page, int size);
        Task<Account> UpdateAccountAsync(Guid id, AccountRequestForUpdate account);
        Task<Account> UpdateProfileAsync(Guid id, AccountRequestForUpdateProfile account);
        Task<Account> UpdateDeflagAccountAsync(Guid id);
        Task<bool> UpdatePasswordAsync(Guid id, string currentPassword, string newPassword);
        Task<string> CreateImageAccount(Guid accountId, ImageForAccount files);
        Task<CurrentUserModel> GetCurrentLoginUser();
    }
}
