using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace taavoni3.Areas.Admin.ViewModel
{
  public class UserCreateViewModel
  {
    [Required]
    [Remote(action: "IsUserNameUnique", controller: "Users", areaName: "Admin", ErrorMessage = "این نام کاربری قبلاً ثبت شده است.")]
    public string UserName { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    [Remote(action: "IsEmailUnique", controller: "Users", areaName: "Admin", ErrorMessage = "این ایمیل قبلاً ثبت شده است.")]

    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
  }

}