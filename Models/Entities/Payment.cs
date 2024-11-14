using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.Models.Entities
{
public class Payment
{
    public int Id { get; set; }
    public string UserId { get; set; }  // ارتباط با کاربر پرداخت‌کننده
    public int DebtId { get; set; }  // ارتباط با شناسه بدهی
    public string Title { get; set; }  // عنوان پرداختی
    public decimal Amount { get; set; }  // مقدار پرداخت شده
    public string? Description { get; set; }  // توضیحات پرداخت
    public string? AttachmentPath { get; set; }  // مسیر فایل پیوست مدرک
    public DateTime? PaymentDate { get; set; }  // تاریخ پرداخت


    /// <summary>
    /// /ارتباطات
    /// </summary>
    public Debt Debt { get; set; }
    public ApplicationUser User { get; set; }
}

}