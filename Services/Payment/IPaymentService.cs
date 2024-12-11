using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.Payment;
using Taavoni.DTOs.Reporting;
using Taavoni.Models.Entities;
using taavoni3.Extention;

namespace Taavoni.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentsAsync(int id);
        Task<List<PaymentDto>> GetAllPaymentsDetailsAsync();
        Task CreatePaymentDetailAsync(CreatePaymentDto createPaymentDto, IFormFile attachment, int debtId);
        Task<bool> DeletePaymentDetailAsync(int Id);
        Task<bool> UpdatePaymentDetailAsync(UpdatePaymentDto dto);
        List<PaymentDto> GetUserPayments(string userId);
        List<PaymentSummaryDto> GetUserPaymentsSummery(string userId);
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
            string fileExtension = "";
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (attachment != null)
            {
                fileExtension = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Invalid file format. Only .jpg, .jpeg, .png files are allowed.");
                }
            }

            var debt = await _context.Debts.FindAsync(debtId);
            if (debt != null && dto.Amount > 0)
            {
                var PersianPaymentDate = PersianDateTime.Parse(dto.PaymentDate!.PersianToEnglish());

                var payment = new Payment
                {
                    Title = dto.Title,
                    UserId = dto.UserId,
                    DebtId = dto.DebtId,
                    Amount = dto.Amount,
                    Description = dto.Description,
                    PaymentDate = PersianPaymentDate.ToDateTime(),

                };

                if (attachment != null)
                {
                    var todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    var directoryPath = Path.Combine("wwwroot/attachments", todayDate);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    var fileName = Path.GetRandomFileName() + fileExtension; // Generate a unique file name
                    var filePath = Path.Combine(directoryPath, fileName);
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

        public async Task<bool> DeletePaymentDetailAsync(int Id)
        {
            var result = await _context.Payments.FindAsync(Id);
            if (result != null)
            {
                // حذف فایل پیوست اگر وجود داشته باشد
                if (!string.IsNullOrEmpty(result.AttachmentPath) && System.IO.File.Exists(result.AttachmentPath))
                {
                    System.IO.File.Delete(result.AttachmentPath);
                }else{}
                _context.Payments.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<List<PaymentDto>> GetAllPaymentsDetailsAsync()
        {
            var payment = await _context.Payments.Include(d => d.User).Include(d => d.Debt).ToListAsync();
            return payment.Select(d => new PaymentDto
            {
                id = d.Id,
                Amount = d.Amount,
                DebtAmount = d.Debt.Amount,
                AttachmentPath = d.AttachmentPath,
                Description = d.Description,
                Name = d.User?.Name,
                UserName = d.User?.UserName,
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

        public List<PaymentDto> GetUserPayments(string userId)
        {

            return _context.Payments
           .Where(p => p.UserId == userId)
           .Select(p => new PaymentDto
           {
               Amount = p.Amount,
               Description = p.Description,
               PaymentDate = p.PaymentDate

           })
           .ToList();
        }

        public List<PaymentSummaryDto> GetUserPaymentsSummery(string userId)
        {

            var result = _context.Payments
                .Where(p => p.UserId == userId)
                .GroupBy(p => new { p.PaymentDate.Value.Year, p.PaymentDate.Value.Month }).AsEnumerable()
                .Select(g => new PaymentSummaryDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(p => p.Amount) // تبدیل به double
                })
                .OrderBy(p => p.Year)
                .ThenBy(p => p.Month)
                .ToList();
            return result;

        }

        public async Task<bool> UpdatePaymentDetailAsync(UpdatePaymentDto dto)
        {
            var model = await _context.Payments.FindAsync(dto.id);
            if (model == null)
            {
                return false;
            }



            model.Title = dto.Title;
            model.DebtId = dto.DebtId;
            model.Amount = dto.Amount;
            model.Description = dto.Description;


            _context.Payments.Update(model);
            await _context.SaveChangesAsync();

            return true;
        }

    }

}