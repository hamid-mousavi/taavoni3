using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taavoni.Services.Interfaces;

namespace Taavoni.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IReportService _reportService;

        public PaymentsController(IPaymentService paymentService, IReportService reportService)
        {
            _paymentService = paymentService;
            _reportService = reportService;
        }

        [HttpGet("user-payments")]
        public IActionResult GetUserPayments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payments = _paymentService.GetUserPayments(userId);
            return Ok(payments);
        }
        [HttpGet("user-dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dashboardData = await _reportService.GetUserDashboardAsync(uId);
            //var debts = dashboardData.DebtDetails;
            return Ok(dashboardData);
        }
         [HttpGet("user-dashboardchart")]
        public async Task<IActionResult> GetUserDashboardChart()
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dashboardData = await _reportService.GetUserDashboardChartAsync(uId);
            //var debts = dashboardData.DebtDetails;
            return Ok(dashboardData);
        }
    }

}