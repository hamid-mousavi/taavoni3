namespace Taavoni.Models;

    public class FixedDebt
{
    public int Id { get; set; }
    public decimal Electricity { get; set; }
    public decimal ProofSupply { get; set; }
    public decimal Taxes { get; set; }
    public decimal Assessment { get; set; }
    public decimal ThreePersonAssessment { get; set; }
    public decimal WaterCompanyAssessment { get; set; }
    public decimal TotalDebt { get; set; } // مجموع بدهی
    public bool IsPaid { get; set; } // وضعیت پرداخت
}

