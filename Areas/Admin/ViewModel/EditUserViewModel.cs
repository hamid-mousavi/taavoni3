using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taavoni3.Areas.Admin.ViewModel
{
  public class EditUserViewModel
  {
    public string Id { get; set; } // شناسه کاربر که برای شناسایی کاربر در درخواست POST استفاده می‌شود

    [Required]
    [StringLength(256)]
    [Display(Name = "Username")]
    public string UserName { get; set; } // نام کاربری کاربر
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    [Display(Name = "Email")]
    public string Email { get; set; } // ایمیل کاربر


    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string? NewPassword { get; set; } // پسورد جدید

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; } // تایید پسورد جدید
  }
}
