using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs
{
    public class CreateDebtDetailDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public bool IsPaid { get; set; }
        public int DebtTitleId {get;set;}

        public decimal RemainingAmount { get; set; }  // مقدار باقیمانده
        public DateTime DueDate { get; set; }  // تاریخ سررسید
        public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
    }
}