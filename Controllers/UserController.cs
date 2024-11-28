using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taavoni.Models.Entities;
using Taavoni.Services.Interfaces;

namespace Taavoni.Controllers
{
    [Authorize]
    public class UserController(IDebtService debtService, IPaymentService paymentService, IReportService reportService) : Controller
    {
        private readonly IDebtService _debtService = debtService;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly IReportService _reportService = reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IActionResult index()
        {
            //   return View();
            return Redirect("/user/dashboard");
        }
        public async Task<IActionResult> MyDebts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var debts = _debtService.GetUserDebts(userId);
            return View(debts);
        }


        public IActionResult MyPayments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var payments = _paymentService.GetUserPayments(userId);
            return View(payments);
        }
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dashboardData = await _reportService.GetUserDashboardAsync(userId);
            return View(dashboardData);
        }








    }


}