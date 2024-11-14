using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.Payment;
using Taavoni.Models.Entities;

namespace Taavoni.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentsAsync(int id);
        Task<List<PaymentDto>> GetAllPaymentsDetailsAsync();
        Task CreatePaymentDetailAsync(CreatePaymentDto createPaymentDto, IFormFile attachment, int debtId);
        Task<bool> DeleteDebtDetailAsync(int Id);
        Task<bool> UpdatePaymentDetailAsync(UpdatePaymentDto createPaymentDto);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task CreatePaymentDetailAsync(CreatePaymentDto dto, IFormFile attachment, int debtId)
        {
            var debt = await _context.Debts.FindAsync(debtId);
            if (debt != null && dto.Amount > 0)
            {

                var payment = new Payment
                {
                    Title = dto.Title,
                    UserId = dto.UserId,
                    DebtId = dto.DebtId,
                    Amount = dto.Amount,
                    Description = dto.Description,
                    PaymentDate = DateTime.UtcNow,

                };

                if (attachment != null)
                {
                    var filePath = Path.Combine("wwwroot/attachments", attachment.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await attachment.CopyToAsync(stream);
                    }
                    payment.AttachmentPath = filePath;
                }

                _context.Payments.Add(payment);
                debt.RemainingAmount -= dto.Amount;
                await _context.SaveChangesAsync();
            }
        }


        public Task<bool> DeleteDebtDetailAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaymentDto>> GetAllPaymentsDetailsAsync()
        {
            var payment = await _context.Payments.Include(d => d.User).ToListAsync();
            return payment.Select(d => new PaymentDto
            {
                id = d.Id,
                Amount = d.Amount,
                AttachmentPath = d.AttachmentPath,
                Description = d.Description,
                Name = d.User?.Name,
                UserId = d.UserId,
                Title = d.Title,
                DebtId = d.DebtId

            }).ToList();
        }

        public async Task<PaymentDto> GetPaymentsAsync(int id)
        {
            var payment = await _context.Payments
            .Include(u => u.User).Include(u => u.Debt)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (payment == null) return null;
            return new PaymentDto
            {
                Amount = payment.Amount,
                AttachmentPath = payment.AttachmentPath,
                Description = payment.Description,
                Name = payment.User.Name,
                Title = payment.Title,
                id = payment.Id,
                UserId = payment.UserId,
                DebtId = payment.DebtId
            };

        }

        public Task<bool> UpdatePaymentDetailAsync(UpdatePaymentDto createPaymentDto)
        {
            throw new NotImplementedException();
        }
    }

}