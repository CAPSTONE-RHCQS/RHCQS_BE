using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace RHCQS_Services.Implement
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardService> _logger;
        public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> GetTotalAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            return await accountRepository.CountAsync();
        }

        public async Task<int> GetSStaffAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.SalesStaff && x.Deflag == true);

            return count;
        }
        public async Task<int> GetDStaffAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.DesignStaff && x.Deflag == true);

            return count;
        }
        public async Task<int> GetCustomerAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.Customer && x.Deflag == true);

            return count;
        }
        public async Task<int> GetCustomerAccountsCreatedTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(
                x => x.Role.RoleName == AppConstant.Role.Customer &&
                     x.InsDate.HasValue &&
                     x.InsDate.Value.Date == today
            );
            return count;
        }
        public async Task<int> GetTotalProjectBySalesStaff(Guid accountId)
        {
            int totalProjectCount = await _unitOfWork.GetRepository<AssignTask>().CountAsync(predicate: x => x.AccountId == accountId);

            return totalProjectCount;
        }
        public async Task<int> GetTotalProjectCountAsync()
        {
            var projectCount = _unitOfWork.GetRepository<Project>();
            return await projectCount.CountAsync();
        }
        public async Task<double> GetTotalPriceOfBatchPayments()
        {
            var totalPrice = await _unitOfWork.GetRepository<BatchPayment>()
                .GetListAsync(
                    predicate: x => x.Status == "Progress" || x.Status == "Paid",
                    include: x => x.Include(x => x.Payment!)
                );

            if (totalPrice == null || !totalPrice.Any())
            {
                return 0;
            }

            var total = totalPrice.Sum(batch => batch.Payment?.TotalPrice ?? 0);
            return total;
        }

        public async Task<double> GetTotalPriceProgressOfBatchPayments()
        {
            var totalPrice = await _unitOfWork.GetRepository<BatchPayment>()
                .GetListAsync(
                    predicate: x => x.Status == "Progress",
                    include: x => x.Include(x => x.Payment!)
                );

            if (totalPrice == null || !totalPrice.Any())
            {
                return 0;
            }

            var total = totalPrice.Sum(batch => batch.Payment?.TotalPrice ?? 0);
            return total;
        }

        public async Task<double> GetTotalPricePaidOfBatchPayments()
        {
            var totalPrice = await _unitOfWork.GetRepository<BatchPayment>()
                .GetListAsync(
                    predicate: x => x.Status == "Paid",
                    include: x => x.Include(x => x.Payment!)
                );

            if (totalPrice == null || !totalPrice.Any())
            {
                return 0;
            }

            var total = totalPrice.Sum(batch => batch.Payment?.TotalPrice ?? 0);
            return total;
        }
    }
}
