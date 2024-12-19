using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request.Mate;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #region GetTotalAccount
        /// <summary>
        /// Get Total accounts.
        /// </summary>
        /// <returns>List of accounts.</returns>
        // GET: api/Account
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalAccountCount()
        {
            var totalAccountCount = await _dashboardService.GetTotalAccountCountAsync();
            return Ok(totalAccountCount);
        }

        #region GetTotalSStaffAccount
        /// <summary>
        /// Get total sales staff accounts.
        /// </summary>
        /// <returns>Number of sales staff accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalSStaffAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalSStaffAccountCount()
        {
            var totalSStaffAccountCount = await _dashboardService.GetSStaffAccountCountAsync();
            return Ok(totalSStaffAccountCount);
        }

        #region GetTotalDStaffAccount
        /// <summary>
        /// Get total design staff accounts.
        /// </summary>
        /// <returns>Number of design staff accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalDStaffAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalDStaffAccountCount()
        {
            var totalDStaffAccountCount = await _dashboardService.GetDStaffAccountCountAsync();
            return Ok(totalDStaffAccountCount);
        }

        #region GetTotalCustomerAccount
        /// <summary>
        /// Get total customer accounts.
        /// </summary>
        /// <returns>Number of customer accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalCustomerAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalCustomerAccount()
        {
            var totalCustomerAccountCount = await _dashboardService.GetCustomerAccountCountAsync();
            return Ok(totalCustomerAccountCount);
        }

        #region GetTotalCustomerAccountToday
        /// <summary>
        /// Get total customer accounts today.
        /// </summary>
        /// <returns>Number of customer accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalCustomerTodayAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalCustomerAccountToday()
        {
            var totalCustomerAccountCount = await _dashboardService.GetCustomerAccountsCreatedTodayAsync();
            return Ok(totalCustomerAccountCount);
        }

        #region GetTotalProject
        /// <summary>
        /// Get total projects.
        /// </summary>
        /// <returns>Number of projects.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalProjectEndpoint)]
        public async Task<ActionResult<int>> GetTotalProject()
        {
            var totalProjectCount = await _dashboardService.GetTotalProjectCountAsync();
            return Ok(totalProjectCount);
        }

        #region GetTotalProjectOfStaff
        /// <summary>
        /// Get total projects.
        /// </summary>
        /// <returns>Number of projects.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalProjectOfStaffEndpoint)]
        public async Task<ActionResult<int>> GetTotalProjectOfStaff(Guid accountId)
        {
            var totalProjectCount = await _dashboardService.GetTotalProjectBySalesStaff(accountId);
            return Ok(totalProjectCount);
        }

        #region GetTotalPrice
        /// <summary>
        /// Retrieves the list of all batch payments across all projects.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalPriceEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPrice()
        {
            var totalPrice = await _dashboardService.GetTotalPriceOfBatchPayments();
            return Ok(new { TotalPrice = totalPrice });
        }

        #region GetTotalPriceProgress
        /// <summary>
        /// Retrieves the list of all batch payments progress across all projects.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total progress price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalProgressPriceEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPriceProgress()
        {
            var totalPrice = await _dashboardService.GetTotalPriceProgressOfBatchPayments();
            return Ok(new { TotalPrice = totalPrice });
        }

        #region GetTotalPricePaid
        /// <summary>
        /// Retrieves the list of all batch payments paid across all projects.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total paid price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalPaidPriceEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPricePaid()
        {
            var totalPrice = await _dashboardService.GetTotalPricePaidOfBatchPayments();
            return Ok(new { TotalPrice = totalPrice });
        }

        #region GetTotalPriceByMonth
        /// <summary>
        /// Retrieves the list of all batch payments across all projects by month.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalPriceByMonthEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPriceByMonth(int month, int year)
        {
            var totalPrice = await _dashboardService.GetTotalPriceOfBatchPaymentsByMonth(month,year);
            return Ok(new { TotalPrice = totalPrice });
        }

        #region GetTotalPriceProgressByMonth
        /// <summary>
        /// Retrieves the list of all batch payments progress across all projects by month.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total progress price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalProgressPriceByMonthEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPriceProgressByMonth(int month, int year)
        {
            var totalPrice = await _dashboardService.GetTotalPriceProgressOfBatchPaymentsByMonth(month, year);
            return Ok(new { TotalPrice = totalPrice });
        }

        #region GetTotalPricePaidByMonth
        /// <summary>
        /// Retrieves the list of all batch payments paid across all projects by month.
        /// 
        /// Role: SALE STAFF - MANAGER
        /// </summary>
        /// <returns>Amount of total paid price</returns>
        #endregion
        [Authorize(Roles = "SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Dashboard.TotalPaidPriceByMonthEndpoint)]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalPricePaidByMonth(int month, int year)
        {
            var totalPrice = await _dashboardService.GetTotalPricePaidOfBatchPaymentsByMonth(month, year);
            return Ok(new { TotalPrice = totalPrice });
        }
    }
}
