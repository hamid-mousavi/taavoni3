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
            else
            {
                // نمایش خطاها
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  // یا از یک لاگ برای مشاهده خطاها استفاده کنید
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
            if (id != model.Id)  // مقایسه با شناسه کاربر
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

                // اگر نام کاربری تغییر کرده باشد، آن را به‌روزرسانی می‌کنیم
                if (user.UserName != model.UserName)
                {
                    user.UserName = model.UserName;
                }

                // اگر ایمیل تغییر کرده باشد، آن را به‌روزرسانی می‌کنیم
                if (user.Email != model.Email)
                {
                    user.Email = model.Email;
                }

                // اگر ایمیل تغییر کرده باشد، آن را به‌روزرسانی می‌کنیم
                if (user.Name != model.Name)
                {
                    user.Name = model.Name;
                }

                // اگر پسورد جدید وارد شده باشد، پسورد را تغییر می‌دهیم
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    // ابتدا یک توکن برای تغییر پسورد تولید می‌کنیم
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (!result.Succeeded)
                    {
                        // اگر تغییر پسورد با خطا مواجه شد، خطاها را در مدل اضافه می‌کنیم
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                // به‌روزرسانی اطلاعات کاربر (بدون تغییر پسورد اگر تغییر نکرده باشد)
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index)); // هدایت به صفحه لیست کاربران
                }

                // اگر خطای دیگری در به روز رسانی وجود داشت، آن‌ها را در مدل اضافه می‌کنیم
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model); // اگر اعتبارسنجی ناموفق باشد، فرم دوباره نشان داده می‌شود
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



    }


}