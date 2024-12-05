using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.Reporting;
using Taavoni.Models.Entities;

namespace Taavoni.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<UserDebtReportDto>> GetUserDebtsReportAsync();
        Task<List<UserDto>> GetUsersAsync();
        Task<UserPaymentReportDto> GetUserPaymentsReportAsync(string userId);
        Task<UserDebtReportDto> GetTopDebtsReportAsync();
        Task<DashboardDto> GetUserDashboardAsync(string userId);
        Task<UserDebtsReportDto> GetUserDebtsReportAsync(string userId);
        Task<DashboardChartDto> GetUserDashboardChartAsync(string userId);

    }

    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportService(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }
        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.Id,
                    UserName = u.UserName
                })
                .ToListAsync();
        }
        public async Task<List<UserDebtReportDto>> GetUserDebtsReportAsync()
        {
            var users = await _context.Users
                .Include(u => u.Debts)
                .Include(u => u.payments)
                .ToListAsync();

            return users.Select(u => new UserDebtReportDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                TotalDebt = u.Debts.Sum(d => d.Amount),
                RemainingDebt = u.Debts.Sum(d => d.Amount) - u.payments.Sum(p => p.Amount)
            }).ToList();
        }


        public async Task<UserPaymentReportDto> GetUserPaymentsReportAsync(string userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserPaymentReportDto
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Payments = u.payments.Select(p => new PaymentDetailDto
                    {
                        PaymentDate = p.PaymentDate,
                        Amount = p.Amount,
                        Description = p.Description
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserDebtReportDto> GetTopDebtsReportAsync()
        {
            var result = await _context.Users
                .Select(u => new UserDebtReportDto
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    TotalDebt = u.Debts.Sum(d => d.Amount),
                    RemainingDebt = u.Debts.Sum(d => d.Amount) - u.payments.Sum(p => p.Amount)
                })
                .OrderByDescending(r => r.TotalDebt)
                .FirstOrDefaultAsync();
            if (result == null) return null;
            return result;
        }

        public async Task<DashboardDto> GetUserDashboardAsync(string userId)
        {
            var users = await _userManager.Users.ToListAsync();
            
            var totalDebt = await _context.Debts
             .Where(d => d.UserId == userId)
             .SumAsync(d => (double)d.Amount);
            var totalDebtWithPenaltyRate = await _context.Debts
           .Where(d => d.UserId == userId)
           .SumAsync(d => (double)d.AmountWithPenaltyRate);

            var totalPaid = await _context.Payments
                .Where(p => p.UserId == userId)
                .SumAsync(p => (double)p.Amount);

            var debtDetails = await _context.Debts
                .Where(d => d.UserId == userId)
                .Select(d => new DebtDetailDto
                {
                    Title = d.debtTitle.Title,
                    Amount = d.Amount,
                    DueDate = d.DueDate,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    PenaltyRate = d.PenaltyRate,
                    RemainingAmount = d.RemainingAmount
                }).ToListAsync();

            var paymentDetails = await _context.Payments
                .Where(p => p.UserId == userId)
                .Select(p => new PaymentDetailDto
                {
                    Title = p.Title,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate
                }).ToListAsync();


            return new DashboardDto
            {
                UserName = users.FirstOrDefault(u => u.Id == userId)!.Name,
                Email =  users.FirstOrDefault(u => u.Id == userId)!.Email,
                TotalDebt = (decimal)totalDebt,
                TotalPaid = (decimal)totalPaid,
                TotalDeptWithPenaltyRate = (decimal)totalDebtWithPenaltyRate,
                DebtDetails = debtDetails,
                PaymentDetails = paymentDetails


            };
        }
        public async Task<UserDebtsReportDto> GetUserDebtsReportAsync(string userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDebtsReportDto
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Debts = u.Debts.Select(d => new UserDebtReportDto
                    {
                        TotalDebt = d.Amount,

                    }).ToList()

                })
                .FirstOrDefaultAsync();

            return user;
        }




        public async Task<DashboardChartDto> GetUserDashboardChartAsync(string userId)
        {
            
            var debts = await _context.Debts
                .Where(d => d.UserId == userId)
                .Select(d => new
                {
                    DebtId = d.Id,
                    Title = d.debtTitle.Title,
                    Amount = d.Amount,
                    Payments = _context.Payments
                        .Where(p => p.DebtId == d.Id) // فرض بر این است که DebtId در پرداخت‌ها وجود دارد
                        .Sum(p => (double?)p.Amount) ?? 0  // مجموع پرداخت‌های مربوط به این بدهی
                })
                .ToListAsync();

            var chartData = debts.Select(d => new DebtChartDto
            {
                Title = d.Title,
                DebtAmount = (double)d.Amount,
                PaymentAmount = d.Payments  // مجموع پرداخت‌ها برای این بدهی
            }).ToList();

            return new DashboardChartDto
            {
                ChartData = chartData
            };
        }


    }


}