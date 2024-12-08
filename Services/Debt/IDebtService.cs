using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Taavoni.DTOs;
using Taavoni.Models;

public interface IDebtService
{
    Task<List<DebtDetailDTO>> GetAllDebtsAsync();
    Task<DebtDetailDTO> GetDebtByIdAsync(int id);
    Task ApplyDailyPenalty();
    Task AddDebtDetailAsync(DebtDetailDTO debtDetailDTO);
    Task CreateDebtDetailAsync(CreateDebtDetailDTO createDebtDetailDTO);
    Task<bool> DeleteDebtDetailAsync(int Id);
    Task<bool> UpdateDebtDetailAsync(EditDebtlDTO editDebtlDTO);
    Task<IEnumerable<DebtTitle>> GetDebtTitlesAsync();
    List<DebtDetailDTO> GetUserDebts(string userId);
    // می‌توانید سایر متدها را نیز اضافه کنید
    Task AddDebtsForAllUsersAsync(CreateAllDebtDto dto);
    List<SelectListItem> GetDebtTitles();
    Task<List<DebtSummaryDto>> GetDebtSummariesAsync();
}
