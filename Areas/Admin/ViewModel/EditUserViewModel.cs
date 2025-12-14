using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace taavoni3.Areas.Admin.ViewModel
{
  public class EditUserViewModel
  {
    public string Id { get; set; } // شناسه کاربر که برای شناسایی کاربر در درخواست POST استفاده می‌شود

    [Required]
    [Remote(action: "IsUserNameUnique", controller: "Users", areaName: "Admin", ErrorMessage = "این نام کاربری قبلاً ثبت شده است.")]
    public string UserName { get; set; } // نام کاربری کاربر
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "IsEmailUnique", controller: "Users", areaName: "Admin", ErrorMessage = "این ایمیل قبلاً ثبت شده است.")]

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
