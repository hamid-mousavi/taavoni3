using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs
{
    public class CreateDebtDetailDTO
    {

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public bool IsPaid { get; set; } = false;
        [Required]
        public int DebtTitleId { get; set; }
        [Range(0, 9999999999999999.99,ErrorMessage = "Please enter valid integer Number")]
        public decimal RemainingAmount { get; set; } // مقدار باقیمانده
        public string DueDate { get; set; }  // تاریخ سررسید
        public decimal PenaltyRate { get; set; } = 0; // درصد جریمه روزانه
    }
}