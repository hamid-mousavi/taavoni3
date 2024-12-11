using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs;
using Taavoni.Models;
using taavoni3.Extention;

public class DebtService : IDebtService
{
    private readonly ApplicationDbContext _context;

    public DebtService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DebtDetailDTO>> GetAllDebtsAsync()
    {
        var Debts = await _context.Debts.Include(d => d.User).Include(d => d.debtTitle).ToListAsync();
        return Debts.Select(d => new DebtDetailDTO
        {
            Id = d.Id,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            Amount = d.Amount,
            DebtTitleId = d.DebtTitleId,
            DebtTitleName = d.debtTitle.Title,
            IsPaid = d.IsPaid,
            Name = d.User?.Name,
            UserName = d.User?.UserName,
            UserId = d.UserId,
            DueDate = d.DueDate,
            PenaltyRate = d.PenaltyRate,
            RemainingAmount = d.RemainingAmount,
            AmountWithPenaltyRate = d.AmountWithPenaltyRate


        }).ToList();
    }

    public async Task<DebtDetailDTO> GetDebtByIdAsync(int id)
    {
        //  var debtDetail = await _context.Debts.Include(d=>d.User).FindAsync(id);
        //   if (debtDetail == null) return null;
        var debtDetail = await _context.Debts
        .Include(d => d.User).Include(d => d.debtTitle)
        .FirstOrDefaultAsync(d => d.Id == id);

        if (debtDetail == null) return null;
        return new DebtDetailDTO
        {
            DebtTitleId = debtDetail.DebtTitleId,
            Id = debtDetail.Id,
            StartDate = debtDetail.StartDate,
            EndDate = debtDetail.EndDate,
            Amount = debtDetail.Amount,
            IsPaid = debtDetail.IsPaid,
            UserId = debtDetail.UserId,
            UserName = debtDetail.User?.Name,
            RemainingAmount = debtDetail.RemainingAmount,
            DueDate = debtDetail.DueDate,
            PenaltyRate = debtDetail.PenaltyRate
        };
    }

    // public async Task AddDebtDetailAsync(DebtDetailDTO debtDetailDTO)
    // {
    //     var debtDetail = new Debt
    //     {
    //         StartDate = debtDetailDTO.StartDate,
    //         EndDate = debtDetailDTO.EndDate,
    //         Amount = debtDetailDTO.Amount,
    //         IsPaid = debtDetailDTO.IsPaid,
    //         UserId = debtDetailDTO.UserId,
    //         DueDate = debtDetailDTO.DueDate,
    //         PenaltyRate = debtDetailDTO.PenaltyRate,
    //         RemainingAmount = debtDetailDTO.RemainingAmount


    //     };
    //     await _context.Debts.AddAsync(debtDetail);
    //     await _context.SaveChangesAsync();
    // }

    public async Task CreateDebtDetailAsync(CreateDebtDetailDTO createDebtDetailDTO)
    {


        var PersianDueDate = PersianDateTime.Parse(createDebtDetailDTO.DueDate.PersianToEnglish());
        var PersianStartDate = PersianDateTime.Parse(createDebtDetailDTO.StartDate.PersianToEnglish());
        var PersianEndDate = PersianDateTime.Parse(createDebtDetailDTO.EndDate.PersianToEnglish());


        // ایجاد شیء بدهی
        var debtDetail = new Debt
        {
            DebtTitleId = createDebtDetailDTO.DebtTitleId,
            StartDate = PersianStartDate.ToDateTime(),
            EndDate = PersianEndDate.ToDateTime(),
            Amount = createDebtDetailDTO.Amount,
            IsPaid = createDebtDetailDTO.IsPaid,
            UserId = createDebtDetailDTO.UserId,
            DueDate = PersianDueDate.ToDateTime(),  // ذخیره تاریخ میلادی
            PenaltyRate = createDebtDetailDTO.PenaltyRate,
            RemainingAmount = createDebtDetailDTO.Amount
        };

        // افزودن بدهی به دیتابیس
        _context.Debts.Add(debtDetail);
        await _context.SaveChangesAsync();
        ApplyDailyPenalty();
    }

    public async Task<bool> UpdateDebtDetailAsync(EditDebtlDTO dto)
    {
        var debtDetail = await _context.Debts.FindAsync(dto.Id);
        if (debtDetail == null)
        {
            return false;
        }

        var PersianDueDate = PersianDateTime.Parse(dto.DueDate.PersianToEnglish());
        var PersianStartDate = PersianDateTime.Parse(dto.StartDate.PersianToEnglish());
        var PersianEndDate = PersianDateTime.Parse(dto.EndDate.PersianToEnglish());

        debtDetail.StartDate = PersianStartDate.ToDateTime();
        debtDetail.DebtTitleId = dto.DebtTitleId;
        debtDetail.EndDate = PersianEndDate.ToDateTime();
        debtDetail.Amount = dto.Amount;
        debtDetail.IsPaid = dto.IsPaid;
        debtDetail.DueDate = PersianDueDate.ToDateTime();
        debtDetail.PenaltyRate = dto.PenaltyRate;

        // debtDetail.UserId = dto.UserId;

        _context.Debts.Update(debtDetail);
        await _context.SaveChangesAsync();
        ApplyDailyPenalty();

        return true;
    }

    public async Task<bool> DeleteDebtDetailAsync(int id)
    {
        var debtDetail = await _context.Debts.FindAsync(id);
        if (debtDetail == null)
        {
            return false;
        }

        _context.Debts.Remove(debtDetail);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<IEnumerable<DebtTitle>> GetDebtTitlesAsync()
    {
        return await _context.debtTitles.ToListAsync();
    }
    public async Task ApplyDailyPenalty()
    {
        var debts = await _context.Debts
            .Where(d => d.DueDate < DateTime.Now)
            .ToListAsync();

        foreach (var debt in debts)
        {
            // اگر جریمه امروز برای این بدهی اعمال نشده باشد
            if (debt.LastPenaltyAppliedDate != DateTime.Today)
            {
                var daysDelayed = (DateTime.Now - debt.DueDate).Days;

                // محاسبه جریمه به ازای هر روز با درصد صحیح
                var penalty = debt.Amount * debt.PenaltyRate * daysDelayed;

                // به‌روزرسانی مقدار با جریمه
                debt.RemainingAmount += penalty;
                debt.AmountWithPenaltyRate = debt.Amount + penalty;  // مبلغ با جریمه

                // بروزرسانی تاریخ آخرین اعمال جریمه
                debt.LastPenaltyAppliedDate = DateTime.Today;
            }
        }

        await _context.SaveChangesAsync();
    }


    public List<DebtDetailDTO> GetUserDebts(string userId)
    {
        return _context.Debts
           .Where(d => d.UserId == userId).Include(d => d.debtTitle)
           .Select(d => new DebtDetailDTO
           {
               Id = d.Id,
               DebtTitleName = d.debtTitle.Title,
               Amount = d.Amount
           })
           .ToList();
    }


    public async Task AddDebtsForAllUsersAsync(CreateAllDebtDto dto)
    {
        var users = _context.Users.ToList();

        var PersianDueDate = PersianDateTime.Parse(dto.DueDate.PersianToEnglish());
        var PersianStartDate = PersianDateTime.Parse(dto.FromDate.PersianToEnglish());
        var PersianEndDate = PersianDateTime.Parse(dto.ToDate.PersianToEnglish());

        foreach (var user in users)
        {
            var debt = new Debt
            {
                DebtTitleId = dto.DebtTitleId,
                Amount = dto.Amount,
                PenaltyRate = dto.PenaltyRate,
                StartDate = PersianStartDate.ToDateTime(),
                EndDate = PersianEndDate.ToDateTime(),
                DueDate = PersianDueDate.ToDateTime(),
                UserId = user.Id
            };
            _context.Debts.Add(debt);
        }

        await _context.SaveChangesAsync();
        ApplyDailyPenalty();

    }

    public List<SelectListItem> GetDebtTitles()
    {
        return _context.debtTitles
            .Select(dt => new SelectListItem
            {
                Value = dt.Id.ToString(),
                Text = dt.Title
            })
            .ToList();
    }
    public async Task<List<DebtSummaryDto>> GetDebtSummariesAsync()
    {
        var summaries = await _context.Debts
            .GroupBy(d => new { d.DebtTitleId, d.debtTitle.Title, d.DueDate, d.StartDate, d.EndDate })
            .Select(g => new DebtSummaryDto
            {
                DebtTitleId = g.Key.DebtTitleId,
                DebtTitleName = g.Key.Title,
                DueDate = g.Key.DueDate,
                StartDate = g.Key.StartDate,
                EndData = g.Key.EndDate,
                TotalAmount = g.Sum(d => (double)d.Amount),
                UserCount = g.Count()
            })
            .ToListAsync();

        return summaries;
    }


}
