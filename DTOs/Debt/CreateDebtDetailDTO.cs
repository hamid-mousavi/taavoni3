using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs
{
    public class CreateDebtDetailDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public bool IsPaid { get; set; }
        public int DebtTitleId {get;set;}
        public decimal RemainingAmount { get; set; }  // مقدار باقیمانده
        public string DueDate { get; set; }  // تاریخ سررسید
        public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
    }
}