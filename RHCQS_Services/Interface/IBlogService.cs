using Microsoft.AspNetCore.Http;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IBlogService
    {
        public Task<IPaginate<BlogResponse>> GetListBlogAsync(int page, int size);
        public Task<List<BlogResponse>> GetListBlog();
        Task<BlogResponse> GetBlogById(Guid id);
        Task<BlogResponse> GetBlogByAccountId(Guid accountid);
        Task<BlogResponse> GetBlogByAccountName(string accountname);
        Task<bool> CreateBlogAsync(BlogRequest request, Guid curAccountId);
        Task<bool> UpdateBlogAsync(Guid id, BlogRequest request, Guid curAccountId);
        Task<bool> DeleteBlog(Guid id);

    }
}
