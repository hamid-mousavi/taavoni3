using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs.DebtTitle
{
    public class DebtTitleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
        public class CreateDebtTitleDto
    {
        public string Title { get; set; }
    }
}