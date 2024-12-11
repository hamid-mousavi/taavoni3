
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taavoni.DTOs.Reporting;
using Taavoni.Models.Entities;
using Taavoni.Services.Interfaces;

namespace Taavoni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Reports")]

    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;


        public ReportsController(IReportService reportService, UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
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
        [HttpGet("api/allreports")]
        public async Task<IActionResult> GetAllUserDashboard()
        {
            List<DashboardDto> dtos = [];
            var users = await _userManager.Users.ToListAsync();
            foreach (var item in users)
            {
                var report = await _reportService.GetUserDashboardAsync(item.Id);
                dtos.Add(report);
            }
            return View(dtos);

        }
        [HttpGet("api/TopTenUser")]
        public async Task<IActionResult> GetTopTenUser()
        {
            List<DashboardDto> dtos = [];
            var users = await _userManager.Users.ToListAsync();
            foreach (var item in users)
            {
                var report = await _reportService.GetUserDashboardAsync(item.Id);
                dtos.Add(report);
            }
            return View(dtos.Take(10).OrderBy(t=> t.TotalDeptWithPenaltyRate).ToList());

        }
        [HttpGet("api/BadTenUser")]
        public async Task<IActionResult> GetBadTenUser()
        {
            List<DashboardDto> dtos = [];
            var users = await _userManager.Users.ToListAsync();
            foreach (var item in users)
            {
                var report = await _reportService.GetUserDashboardAsync(item.Id);
                dtos.Add(report);
            }
            return View(dtos.Take(10).OrderBy(t=> t.TotalDeptWithPenaltyRate).OrderDescending());

        }

    }


}