using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs.DebtTitle;
using Taavoni.Models;

namespace Taavoni.Services.Interfaces
{
    public interface IDebtTitleService
{
    Task<IEnumerable<DebtTitleDto>> GetAllDebtTitlesAsync();
    Task<DebtTitleDto> GetDebtTitleByIdAsync(int id);
    Task CreateDebtTitleAsync(DebtTitleDto DebtTitleDto);
    Task UpdateDebtTitleAsync(int id, DebtTitleDto DebtTitleDto);
    Task DeleteDebtTitleAsync(int id);
}

public class DebtTitleService : IDebtTitleService
{
    private readonly ApplicationDbContext _context;

    public DebtTitleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DebtTitleDto>> GetAllDebtTitlesAsync()
    {
        return await _context.debtTitles
            .Select(dt => new DebtTitleDto
            {
                Id = dt.Id,
                Title = dt.Title
            })
            .ToListAsync();
    }

    public async Task<DebtTitleDto> GetDebtTitleByIdAsync(int id)
    {
        var debtTitle = await _context.debtTitles.FindAsync(id);
        if (debtTitle == null) return null;

        return new DebtTitleDto
        {
            Id = debtTitle.Id,
            Title = debtTitle.Title
        };
    }

    public async Task CreateDebtTitleAsync(DebtTitleDto DebtTitleDto)
    {
        var debtTitle = new DebtTitle
        {
            Title = DebtTitleDto.Title
        };

        _context.debtTitles.Add(debtTitle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDebtTitleAsync(int id, DebtTitleDto DebtTitleDto)
    {
        var debtTitle = await _context.debtTitles.FindAsync(id);
        if (debtTitle == null) return;

        debtTitle.Title = DebtTitleDto.Title;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDebtTitleAsync(int id)
    {
        var debtTitle = await _context.debtTitles.FindAsync(id);
        if (debtTitle == null) return;

        _context.debtTitles.Remove(debtTitle);
        await _context.SaveChangesAsync();
    }
}

}
