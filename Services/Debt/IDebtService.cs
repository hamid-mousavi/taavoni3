using System.Collections.Generic;
using System.Threading.Tasks;
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
}
