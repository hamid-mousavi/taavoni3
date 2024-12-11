using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taavoni.Models.Entities;
using taavoni3.Areas.Admin.ViewModel;
using taavoni3.Services.User;

namespace taavoni3.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            return View(await users.ToListAsync());
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName, Name, Email, Password, ConfirmPassword")] UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // بررسی وجود نام کاربری در دیتابیس
                var existingUserName = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserName != null)
                {
                    ModelState.AddModelError("UserName", "این نام کاربری قبلاً ثبت شده است.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Users/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,  // ارسال شناسه کاربر
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name
            };

            return View(model);
        }


        // POST: Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // بررسی تکراری بودن نام کاربری فقط در صورت تغییر
                if (user.UserName != model.UserName)
                {
                    var existingUserName = await _userManager.Users
                        .AnyAsync(u => u.UserName == model.UserName && u.Id != id); // چک تکرار به‌جز کاربر فعلی
                    if (existingUserName)
                    {
                        ModelState.AddModelError("UserName", "این نام کاربری قبلاً ثبت شده است.");
                        return View(model);
                    }
                    user.UserName = model.UserName;
                }

                // بررسی تکراری بودن ایمیل فقط در صورت تغییر
                if (user.Email != model.Email)
                {
                    var existingEmail = await _userManager.Users
                        .AnyAsync(u => u.Email == model.Email && u.Id != id); // چک تکرار به‌جز کاربر فعلی
                    if (existingEmail)
                    {
                        ModelState.AddModelError("Email", "این ایمیل قبلاً ثبت شده است.");
                        return View(model);
                    }
                    user.Email = model.Email;
                }

                // به‌روزرسانی سایر فیلدها
                user.Name = model.Name;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Users/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Users/ChangePassword
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        // POST: Users/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // به صفحه اصلی هدایت می‌شود
                }

                // اگر تغییر پسورد با خطا مواجه شد، خطاها را در مدل اضافه می‌کنیم
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model); // اگر اعتبارسنجی ناموفق باشد، فرم دوباره نشان داده می‌شود
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Json(false);
            }
            return Json(true);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsUserNameUnique(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return Json(false);
            }
            return Json(true);
        }

    }


}