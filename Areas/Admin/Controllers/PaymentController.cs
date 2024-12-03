
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.Payment;
using Taavoni.Models.Entities;
using Taavoni.Services.Interfaces;

namespace Taavoni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IDebtService _debtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;



        public PaymentController(IPaymentService paymentService, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDebtService debtService)
        {
            _paymentService = paymentService;
            _context = context;
            _userManager = userManager;
            _debtService = debtService;
        }
        public async Task<ActionResult> Index()
        {
            var model = await _paymentService.GetAllPaymentsDetailsAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CreatePayment()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewBag.DebtId = new SelectList(await _debtService.GetAllDebtsAsync(), "Id", "Name");
            ViewBag.Users = new MultiSelectList(users, "Id", "UserName");
            var dto = new CreatePaymentDto(); // مقداردهی اولیه به مدل
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentDto dto, IFormFile attachment)
        {


            var user = await _context.Users.FindAsync(dto.UserId); // دسترسی مستقیم به کاربران
            if (user != null)
            {
                await _paymentService.CreatePaymentDetailAsync(dto, attachment, dto.DebtId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserPayments(int userId)
        {
            //  var payments = await _paymentService.GetUserPaymentsAsync(userId);
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {

            var payment = await _paymentService.GetPaymentsAsync(id);
            var dto = new UpdatePaymentDto
            {
                Title = payment.Title,
                Amount = payment.Amount,
                Description = payment.Description,
                DebtId = payment.DebtId
            };
            if (payment == null)
                return null;
            var users = await _userManager.Users.ToListAsync();
            // ارسال کاربران به ویو
            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName", payment.id);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePaymentDto dto)
        {

            if (id != dto.id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var result = await _paymentService.UpdatePaymentDetailAsync(dto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "مشکلی در به‌روزرسانی اطلاعات پیش آمده است.");
            }
            else
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error in {modelStateKey}: {error.ErrorMessage}");
                    }
                }
                ModelState.AddModelError("", "");
            }

            return View(dto);
        }


        public async Task<IActionResult> GetDebtsByUserId(string userId)
        {
            var debts = _debtService.GetUserDebts(userId);
            return Json(debts);
        }


    }


}