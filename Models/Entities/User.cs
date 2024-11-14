using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Taavoni.Models.Entities
{
public class ApplicationUser : IdentityUser
{
     public string? Name { get; set; }
    public ICollection<Debt> Debts { get; set; }
    public ICollection<Payment> payments { get; set; }
}
}