using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taavoni.DTOs.DebtTitle;
using Taavoni.Services.Interfaces;

namespace Taavoni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DebtTitleController : Controller
    {
        private readonly IDebtTitleService _debtTitleService;

        public DebtTitleController(IDebtTitleService debtTitleService)
        {
            _debtTitleService = debtTitleService;
        }

        public async Task<IActionResult> Index()
        {
            var debtTitles = await _debtTitleService.GetAllDebtTitlesAsync();
            return View(debtTitles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DebtTitleDto DebtTitleDto)
        {
            if (ModelState.IsValid)
            {
                await _debtTitleService.CreateDebtTitleAsync(DebtTitleDto);
                return RedirectToAction("Index");
            }
            return View(DebtTitleDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var debtTitle = await _debtTitleService.GetDebtTitleByIdAsync(id);
            if (debtTitle == null) return NotFound();
            return View(debtTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DebtTitleDto DebtTitleDto)
        {
            if (ModelState.IsValid)
            {
                await _debtTitleService.UpdateDebtTitleAsync(id, DebtTitleDto);
                return RedirectToAction("Index");
            }
            return View(DebtTitleDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var debtTitle = await _debtTitleService.GetDebtTitleByIdAsync(id);
            if (debtTitle == null) return NotFound();
            return View(debtTitle);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _debtTitleService.DeleteDebtTitleAsync(id);
            return RedirectToAction("Index");
        }
    }

}