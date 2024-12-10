using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Taavoni.Services.Interfaces;

namespace Taavoni.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
         private readonly IReportService _reportService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
             var users = await _reportService.GetUsersAsync();
              ViewBag.Users = new SelectList(users, "UserId", "UserName");
            return Redirect("/reports/api/TopTenUser");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}