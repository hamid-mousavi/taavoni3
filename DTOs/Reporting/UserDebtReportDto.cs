using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs.Reporting
{
    public class UserDebtReportDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal? TotalPayd { get; set; }
        public decimal RemainingDebt { get; set; }
    }
    public class UserDebtsReportDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UserDebtReportDto> Debts { get; set; }
    }

    public class UserPaymentReportDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<PaymentDetailDto> Payments { get; set; }
    }

    public class PaymentDetailDto
    {
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
    public class UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    public class PaymentSummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
    }






    public class DebtDetailDto
    {
        // public decimal TotalDeptWithPenaltyRate { get; set; }

        public decimal RemainingAmount { get; set; }  // مقدار باقی
        public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }


    public class DashboardDto
    {

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal TotalPaid { get; set; }
        // public decimal TotalDeptWithPenaltyRate { get; set; }
        public decimal RemainingAmount => TotalDebt - TotalPaid;
        public List<DebtDetailDto> DebtDetails { get; set; }
        public List<PaymentDetailDto> PaymentDetails { get; set; }
    }
    public class DashboardChartDto
    {
        public List<DebtChartDto> ChartData { get; set; }
    }

    public class DebtChartDto
    {
        public string Title { get; set; }
        public double DebtAmount { get; set; }
        public double PaymentAmount { get; set; }
    }
    





}