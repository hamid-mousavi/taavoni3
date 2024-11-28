using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Taavoni.Models.Entities
{
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "نام الزامی است.")]
    [StringLength(50, ErrorMessage = "نام نمی‌تواند بیش از ۵۰ کاراکتر باشد.")]
    public string Name { get; set; }
    public ICollection<Debt> Debts { get; set; }
    public ICollection<Payment> payments { get; set; }
}
}