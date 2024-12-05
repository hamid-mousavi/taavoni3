using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using taavoni3.Services.User;

namespace taavoni3.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePasswordAsync(userId, newPassword);
                if (result)
                {
                    ViewBag.Message = "Password changed successfully!";
                }
                else
                {
                    ViewBag.Message = "Error changing password.";
                }
            }

            return View();
        }
    }
}