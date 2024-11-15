using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taavoni.Services.Interfaces;

namespace Taavoni.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly IPaymentService _paymentService;
        private readonly IReportService _reportService;


        public UserController(IDebtService debtService, IPaymentService paymentService, IReportService reportService)
        {
            _debtService = debtService;
            _paymentService = paymentService;
            _reportService = reportService;
        }
        public IActionResult index()
        {
            return View();
        }
        public IActionResult MyDebts()
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
        







    }


}