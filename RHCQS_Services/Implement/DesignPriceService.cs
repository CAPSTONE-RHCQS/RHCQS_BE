using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Packaging.Ionic.Zip;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class DesignPriceService : IDesignPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DesignPriceService> _logger;

        public DesignPriceService(IUnitOfWork unitOfWork, ILogger<DesignPriceService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<DesignPriceResponse>> GetListDesignPrice()
        {
            var result = (await _unitOfWork.GetRepository<DesignPrice>()
                 .GetListAsync(
                     selector: x => new DesignPriceResponse
                     {
                         Id = x.Id,
                         AreaFrom = x.AreaFrom,
                         AreaTo = x.AreaTo,
                         Price = x.Price,
                         InsDate = x.InsDate,
                         UpsDate = x.UpsDate
                     },
                     orderBy: x => x.OrderBy(dp => dp.InsDate)
                 )).ToList();
            return result;
        }

        public async Task<DesignPriceResponse> GetDetailDesignPrice(Guid id)
        {
            var designPrice = await _unitOfWork.GetRepository<DesignPrice>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (designPrice == null)
                return new DesignPriceResponse();

            return new DesignPriceResponse
            {
                Id = designPrice.Id,
                AreaFrom = designPrice.AreaFrom,
                AreaTo = designPrice.AreaTo,
                Price = designPrice.Price,
                InsDate = designPrice.InsDate,
                UpsDate = designPrice.UpsDate
            };
        }

        public async Task<bool> CreateDesignPrice(DesignPriceRequest request)
        {
            try
            {
                var newDesignPrice = new DesignPrice
                {
                    Id = Guid.NewGuid(),
                    AreaFrom = request.AreaFrom,
                    AreaTo = request.AreaTo,
                    Price = request.Price,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime()
                };
                await _unitOfWork.GetRepository<DesignPrice>().InsertAsync(newDesignPrice);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Có lỗi khi tạo mới giá thiết kế."
                );
            }
        }

        public async Task<bool> UpdateDesignPrice(Guid id, DesignPriceRequest request)
        {
            try
            {
                var designPrice = await _unitOfWork.GetRepository<DesignPrice>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (designPrice == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Giá thiết kế không tồn tại."
                    );
                }

                designPrice.AreaFrom = request.AreaFrom ?? designPrice.AreaFrom;
                designPrice.AreaTo = request.AreaTo ?? designPrice.AreaTo;
                designPrice.Price = request.Price.HasValue ? (double)request.Price.Value : designPrice.Price;

                designPrice.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<DesignPrice>().UpdateAsync(designPrice);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi cập nhật giá thiết kế."
                );
            }
        }
    }
}
