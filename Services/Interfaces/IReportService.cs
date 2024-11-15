using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.Reporting;

namespace Taavoni.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<UserDebtReportDto>> GetUserDebtsReportAsync();
        Task<List<UserDto>> GetUsersAsync();
        Task<UserPaymentReportDto> GetUserPaymentsReportAsync(string userId);
        Task<UserDebtReportDto> GetTopDebtsReportAsync();
    }

    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
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






    }

}