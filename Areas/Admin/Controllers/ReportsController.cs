
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Taavoni.Services.Interfaces;

namespace Taavoni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Reports")]

    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("DebtReport")]
        public async Task<IActionResult> DebtReport()
        {
            var report = await _reportService.GetUserDebtsReportAsync();
            return View(report);
        }

        [HttpGet("PaymentReport")]
        public async Task<IActionResult> PaymentReport()
        {
            var users = await _reportService.GetUsersAsync();
            ViewBag.Users = new SelectList(users, "UserId", "UserName");
            return View();
        }

        [HttpPost("PaymentReport")]
        public async Task<IActionResult> PaymentReport(string userId)
        {
            var report = await _reportService.GetUserPaymentsReportAsync(userId);
            var users = await _reportService.GetUsersAsync();
            ViewBag.Users = new SelectList(users, "UserId", "UserName", userId);
            return View(report);
        }


        [HttpGet("TopDebtsReport")]
        public async Task<IActionResult> TopDebtsReport()
        {
            var report = await _reportService.GetUserDebtsReportAsync();
            return View(report.OrderByDescending(r => r.TotalDebt).ToList());
        }



        [HttpGet("api/UserPayments/{userId}")]
        public async Task<IActionResult> GetUserPaymentsData(string userId)
        {
            var report = await _reportService.GetUserPaymentsReportAsync(userId);
            var data = report.Payments.Select(p => new
            {
                PaymentDate = p.PaymentDate,
                Amount = p.Amount
            });
            return Json(data);
        }

        [HttpGet("api/UserDebts")]
        public async Task<IActionResult> GetUserDebtsData()
        {
            var debtsReport = await _reportService.GetUserDebtsReportAsync();
            var data = debtsReport.Select(d => new
            {
                UserName = d.UserName,
                TotalDebt = d.TotalDebt,
                RemainingDebt = d.RemainingDebt
            });
            return Json(data);
        }

    }


}