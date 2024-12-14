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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class LaborService : ILaborService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LaborService> _logger;

        public LaborService(IUnitOfWork unitOfWork, ILogger<LaborService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<LaborResponse>> GetListLabor(int page, int size)
        {
            return await _unitOfWork.GetRepository<Labor>().GetList(
                selector: x => new LaborResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    Type = x.Type,
                    Code = x.Code
                },
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<IPaginate<LaborResponse>> SearchLaborByNameWithPag(string? name, int page, int size)
        {
            return await _unitOfWork.GetRepository<Labor>().GetList(
                selector: x => new LaborResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    Type = x.Type,
                    Code = x.Code
                },
                predicate : x => x.Name.Contains(name) || string.IsNullOrWhiteSpace(name),
                orderBy: x => x.OrderBy(x => x.InsDate),
                page: page,
                size: size
            );
        }

        public async Task<LaborResponse> GetDetailLabor(Guid id)
        {
            var labor = await _unitOfWork.GetRepository<Labor>().FirstOrDefaultAsync(
                predicate: m => m.Id == id);
            if (labor == null)
                return new LaborResponse();

            return new LaborResponse
            {
                Id = labor.Id,
                Name = labor.Name,
                Price = labor.Price,
                InsDate = labor.InsDate,
                UpsDate = labor.UpsDate,
                Deflag = labor.Deflag,
                Type = labor.Type,
                Code = labor.Code
            };
        }

        public async Task<bool> CreateLabor(LaborRequest request)
        {
            try
            {
                var newLabor = new Labor
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Price = request.Price,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    Deflag = request.Deflag,
                    Type = request.Type,
                    Code = request.Code
                };
                await _unitOfWork.GetRepository<Labor>().InsertAsync(newLabor);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Có lỗi khi tạo mới 1 nhân công."
                );
            }
        }

        public async Task<bool> UpdateLabor(Guid id, LaborRequest request)
        {
            try
            {
                var labor = await _unitOfWork.GetRepository<Labor>()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (labor == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.NotFound,
                        "Nhân công không tồn tại."
                    );
                }

                labor.Name = request.Name ?? labor.Name;
                labor.Price = request.Price.HasValue ? (double)request.Price.Value : labor.Price;
                labor.Deflag = request.Deflag ?? labor.Deflag;
                labor.Type = request.Type ?? labor.Type;
                labor.Code = request.Code ?? labor.Code;

                labor.UpsDate = LocalDateTime.VNDateTime();

                _unitOfWork.GetRepository<Labor>().UpdateAsync(labor);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    "Xuất hiện lỗi khi cập nhật nhân công."
                );
            }
        }

        public async Task<List<LaborResponse>> SearchLaborByName(Guid packageId, string? name)
        {
            var result = (await _unitOfWork.GetRepository<PackageLabor>()
                .GetListAsync(
                    selector: x => new LaborResponse
                    {
                        Id = x.Labor.Id,
                        Name = x.Labor.Name,
                        Price = x.Labor.Price,
                        InsDate = x.Labor.InsDate,
                        UpsDate = x.Labor.UpsDate,
                        Deflag = x.Labor.Deflag,
                        Type = x.Labor.Type,
                        Code = x.Labor.Code,
                    },
                    predicate: pl => pl.Package.Id == packageId &&
                                     pl.Labor.Name.Contains(name),
                    include: x => x.Include(pl => pl.Labor)
                                   .Include(pl => pl.Package),
                    orderBy: x => x.OrderBy(pl => pl.Labor.InsDate)
                )).ToList();
            return result;
        }

        public async Task<List<LaborResponse>> SearchLaborByNameWithoutPackage(string name)
        {
            return (List<LaborResponse>)await _unitOfWork.GetRepository<Labor>().GetListAsync(
                selector: x => new LaborResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    InsDate = x.InsDate,
                    UpsDate = x.UpsDate,
                    Deflag = x.Deflag,
                    Type = x.Type,
                    Code = x.Code
                },
                predicate: m => m.Name.Contains(name),
                orderBy: x => x.OrderBy(x => x.InsDate)
            );
        }

        public async Task<bool> ImportLaborFromExcel(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null || excelFile.Length == 0)
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, "Không tìm thấy file được tải.");

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; 
                        var rowCount = worksheet.Dimension.Rows;

                        var newCodes = new HashSet<string>(); 

                        for (int row = 2; row <= rowCount; row++) 
                        {
                            var code = worksheet.Cells[row, 5].Value?.ToString(); 

                            if (await _unitOfWork.GetRepository<Labor>().AnyAsync(l => l.Code == code) || newCodes.Contains(code))
                            {
                                throw new AppConstant.MessageError(
                                    (int)AppConstant.ErrCode.Conflict,
                                    $"Tìm thấy mã {code} bị nhập trùng. Hãy chọn lại file khác để tải lên."
                                );
                            }

                            newCodes.Add(code);

                            var labor = new Labor
                            {
                                Id = Guid.NewGuid(),
                                Name = worksheet.Cells[row, 1].Value?.ToString(),
                                Price = Convert.ToDouble(worksheet.Cells[row, 2].Value ?? 0),
                                Deflag = Convert.ToBoolean(worksheet.Cells[row, 3].Value ?? false),
                                Type = worksheet.Cells[row, 4].Value?.ToString(),
                                Code = code,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime()
                            };

                            await _unitOfWork.GetRepository<Labor>().InsertAsync(labor);
                        }


                        return await _unitOfWork.CommitAsync() > 0;
                    }
                }
            }
            catch (AppConstant.MessageError ex) 
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, $"Tìm thấy mã bị nhập trùng. Hãy chọn lại file khác để tải lên..");
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, $"Lỗi khi nhập data {ex.Message}");
            }
        }
    }
}
