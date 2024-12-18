using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IDashboardService
    {
        Task<int> GetTotalAccountCountAsync();
        Task<int> GetSStaffAccountCountAsync();
        Task<int> GetDStaffAccountCountAsync();
        Task<int> GetCustomerAccountCountAsync();
        Task<int> GetCustomerAccountsCreatedTodayAsync();
        Task<int> GetTotalProjectCountAsync();
        Task<int> GetTotalProjectBySalesStaff(Guid accountId);
        Task<double> GetTotalPriceOfBatchPayments();
        Task<double> GetTotalPriceProgressOfBatchPayments();
        Task<double> GetTotalPricePaidOfBatchPayments();
    }
}
