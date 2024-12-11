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
        [Range(0, 9999999999999999.99, ErrorMessage = "Please enter valid integer Number")]
        public decimal RemainingAmount { get; set; } // مقدار باقیمانده
        public string DueDate { get; set; }  // تاریخ سررسید
        public decimal PenaltyRate { get; set; } = 0; // درصد جریمه روزانه
    }

    public class CreateAllDebtDto
    {
        public int DebtTitleId { get; set; }
        public decimal Amount { get; set; }
        [Range(0, 1, ErrorMessage = "مقدار بین صفر و یک باشد")]
        public decimal PenaltyRate { get; set; } = 0; // درصد جریمه روزانه
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DueDate { get; set; }
    }
    public class DebtSummaryDto
    {
        public int DebtTitleId { get; set; }
        public string DebtTitleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndData { get; set; }
        public DateTime DueDate { get; set; }
        public double TotalAmount { get; set; }
        public int UserCount { get; set; }
    }


}