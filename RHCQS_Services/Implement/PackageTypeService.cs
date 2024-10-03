using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
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
    public class PackageTypeService : IPackageTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PackageTypeService> _logger;

        public PackageTypeService(IUnitOfWork unitOfWork, ILogger<PackageTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<PackageTypeResponse>> GetAllPackageTypesAsync(int page, int size)
        {
            IPaginate<PackageTypeResponse> listRoles =
            await _unitOfWork.GetRepository<PackageType>().GetList(
                selector: x => new PackageTypeResponse(x.Id, x.Name, x.InsDate),
                page: page,
                size: size
                );
            return listRoles;
        }
        public async Task<PackageType> CreatePackageTypeAsync(PackageTypeRequest packageType)
        {
            try
            {
                if (packageType == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Bad_Request,
                        AppConstant.ErrMessage.NullValue
                    );
                }

                var existingPackageType = await _unitOfWork.GetRepository<PackageType>().FirstOrDefaultAsync(
                    x => x.Name.Equals(packageType.Name));

                if (existingPackageType != null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.PackageTypeExists
                    );
                }
                var newPackageType = new PackageType
                {
                    Id = Guid.NewGuid(),
                    Name = packageType.Name,
                    InsDate = DateTime.UtcNow,
                };

                await _unitOfWork.GetRepository<PackageType>().InsertAsync(newPackageType);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Conflict,
                        AppConstant.ErrMessage.CreatePackageType
                    );
                }

                return newPackageType;
            }catch(Exception ex) { throw; }
        }
    }
}
