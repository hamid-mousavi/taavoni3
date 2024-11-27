using Taavoni.Models.Entities;

namespace Taavoni.Models;

public class Debt
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }= DateTime.Now;
   
    public decimal Amount { get; set; }
    public decimal AmountWithPenaltyRate { get; set; }=0;
    public bool IsPaid { get; set; } = true;// وضعیت پرداخت
    
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; } 
    public DateTime LastPenaltyAppliedDate { get; set; } // تاریخ آخرین اعمال جریمه
    public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
    /// <summary>
    /// ارتباطات 
    /// </summary>
    public List<Payment> Payments { get; set; }
//
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    // ارتباط با عنوان بدهی
    public int DebtTitleId {get;set;}
    public DebtTitle debtTitle {get;set;}
}
public class DebtTitle{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Debt> debts {get;set;}
}
