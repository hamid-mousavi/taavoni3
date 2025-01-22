
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
            var users = await _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name
            }).ToListAsync();
            ViewBag.DebtId = new SelectList(await _debtService.GetAllDebtsAsync(), "Id", "Name");
            ViewBag.Users = new MultiSelectList(users, "Id", "DisplayName");
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
            if (payment == null)
            {
                return NotFound(); // یا یک صفحه خطا نمایش دهید
            }
            // یافتن کاربر بر اساس شناسه
            var user = await _userManager.FindByIdAsync(payment.UserId.ToString());
            if (user == null)
            {
                return NotFound(); // یا یک صفحه خطا نمایش دهید
            }

            var dto = new UpdatePaymentDto
            {
                id = payment.id, // اضافه کردن شناسه پرداخت
                Title = payment.Title,
                Amount = payment.Amount,
                Description = payment.Description,
                DebtId = payment.DebtId,
                AttachmentPath = payment.AttachmentPath, // اضافه کردن مسیر فایل پیوست
                UserName = user.Name // نام کاربر
            };

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName", payment.id);

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePaymentDto dto, IFormFile? attachment)
        {
            if (id != dto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _paymentService.UpdatePaymentDetailAsync(dto, attachment);
                if (result)
                {
                    TempData["SuccessMessage"] = "ویرایش با موفقیت انجام شد.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "مشکلی در به‌روزرسانی اطلاعات پیش آمده است.");
                }
            }
            else
            {
                ModelState.AddModelError("", "لطفاً اطلاعات را به درستی وارد کنید.");
            }

            // اگر مدل معتبر نباشد، دوباره فرم را نمایش دهید
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName", dto.id);
            return View(dto);
        }
        public async Task<IActionResult> GetDebtsByUserId(string userId)
        {
            var debts = _debtService.GetUserDebts(userId);
            return Json(debts);
        }


        // GET: payments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetPaymentsAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Debts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _paymentService.DeletePaymentDetailAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("مشکلی در حذف اطلاعات پیش آمده است.");
        }




        public async Task<IActionResult> Download(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
            }
            return NotFound();
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
        {
            { ".pdf", "application/pdf" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".doc", "application/vnd.ms-word" },
            { ".docx", "application/vnd.ms-word" }
        };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }


    }


}