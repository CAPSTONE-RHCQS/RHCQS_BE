﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlogService> _logger;

        public BlogService(IUnitOfWork unitOfWork, ILogger<BlogService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<BlogResponse>> GetListBlogAsync(int page, int size)
        {
            var listBlog = await _unitOfWork.GetRepository<Blog>().GetList(
                selector: x => new BlogResponse(
                    x.Id,
                    x.Account.Username,
                    x.Account.Role.RoleName,
                    x.Heading,
                    x.SubHeading,
                    x.Context,
                    x.ImgUrl,
                    x.InsDate,
                    x.UpsDate
                ),
                include: x => x.Include(x => x.Account)
                               .ThenInclude(x => x.Role),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size);

            return listBlog;
        }

        public async Task<BlogResponse> GetBlogById(Guid id)
        {
            var blog = await _unitOfWork.GetRepository<Blog>().FirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: x => x.Include(b => b.Account)
                               .ThenInclude(a => a.Role)
            );

            if (blog == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Blog
                );
            }

            return new BlogResponse(
                blog.Id,
                blog.Account.Username,
                blog.Account.Role.RoleName,
                blog.Heading,
                blog.SubHeading,
                blog.Context,
                blog.ImgUrl,
                blog.InsDate,
                blog.UpsDate
            );
        }

        public async Task<BlogResponse> GetBlogByAccountId(Guid accountId)
        {
            var blog = await _unitOfWork.GetRepository<Blog>().FirstOrDefaultAsync(
                predicate: x => x.AccountId == accountId,
                include: x => x.Include(b => b.Account)
                               .ThenInclude(a => a.Role)
            );

            if (blog == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Blog
                );
            }

            return new BlogResponse(
                blog.Id,
                blog.Account.Username,
                blog.Account.Role.RoleName,
                blog.Heading,
                blog.SubHeading,
                blog.Context,
                blog.ImgUrl,
                blog.InsDate,
                blog.UpsDate
            );
        }

        public async Task<BlogResponse> GetBlogByAccountName(string accountName)
        {
            var blog = await _unitOfWork.GetRepository<Blog>().FirstOrDefaultAsync(
                predicate: x => x.Account.Username == accountName,
                include: x => x.Include(b => b.Account)
                               .ThenInclude(a => a.Role)
            );

            if (blog == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Blog
                );
            }

            return new BlogResponse(
                blog.Id,
                blog.Account.Username,
                blog.Account.Role.RoleName,
                blog.Heading,
                blog.SubHeading,
                blog.Context,
                blog.ImgUrl,
                blog.InsDate,
                blog.UpsDate
            );
        }
        public async Task<bool> CreateBlogAsync(BlogRequest request)
        {
            var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(x => x.Id == request.AccountId);
            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }

            var blog = new Blog
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                Heading = request.Heading,
                SubHeading = request.SubHeading,
                Context = request.Context,
                ImgUrl = request.ImgUrl,
                InsDate = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Blog>().InsertAsync(blog);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;       
        }

        public async Task<bool> UpdateBlogAsync(Guid id, BlogRequest request)
        {
            var blog = await _unitOfWork.GetRepository<Blog>().FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Blog
                );
            }

            blog.Heading = request.Heading;
            blog.SubHeading = request.SubHeading;
            blog.Context = request.Context;
            blog.ImgUrl = request.ImgUrl;
            blog.UpsDate = DateTime.UtcNow;

            _unitOfWork.GetRepository<Blog>().UpdateAsync(blog);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
