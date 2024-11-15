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
    public decimal RemainingDebt { get; set; }
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


}