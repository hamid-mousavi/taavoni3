using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs;

public class DebtDetailDTO
{
    public int Id { get; set; }
        public int DebtTitleId {get;set;}
        public string? DebtTitleName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public bool IsPaid { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public decimal RemainingAmount { get; set; }  // مقدار باقیمانده
    public DateTime DueDate { get; set; }  // تاریخ سررسید
    public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
}
public class EditDebtlDTO
{
    public int Id { get; set; }
        public int DebtTitleId {get;set;}

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public bool IsPaid { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public decimal RemainingAmount { get; set; }  // مقدار باقیمانده
    public DateTime DueDate { get; set; }  // تاریخ سررسید
    public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
}
