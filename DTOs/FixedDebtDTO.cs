using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs;

    public class FixedDebtDTO
    {
        public int Id { get; set; }
    public decimal Electricity { get; set; }
    public decimal ProofSupply { get; set; }
    public decimal Taxes { get; set; }
    public decimal Assessment { get; set; }
    public decimal ThreePersonAssessment { get; set; }
    public decimal WaterCompanyAssessment { get; set; }
    public decimal TotalDebt { get; set; }
    public bool IsPaid { get; set; }
    }
