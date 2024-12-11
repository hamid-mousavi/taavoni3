using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taavoni.DTOs.Payment
{
    public class PaymentDto
    {
        public int id { get; set; }
        public DateTime? PaymentDate { get; set; }  // تاریخ پرداخت

        [Required(ErrorMessage = "عنوان پرداختی لازم است")]
        public string Title { get; set; }
        [Required(ErrorMessage = "مقدار پرداختی لازم است")]
        [Range(0.01, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد")]
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public int DebtId { get; set; }  // ارتباط با شناسه بدهی
        public decimal DebtAmount { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AttachmentPath { get; set; }
        public string? UserName { get; set; }
    }
    public class CreatePaymentDto
    {

        [Required(ErrorMessage = "عنوان پرداختی لازم است")]

        public string Title { get; set; }
        [Required(ErrorMessage = "مقدار پرداختی لازم است")]

        [Range(1, double.MaxValue, ErrorMessage = "مقدار باید بیشتر از صفر باشد")]
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public int DebtId { get; set; }  // ارتباط با شناسه بدهی
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AttachmentPath { get; set; }
        public string? PaymentDate { get; set; }  // تاریخ پرداخت
    }
    public class UpdatePaymentDto
    {
        public int id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public int DebtId { get; set; }  // ارتباط با شناسه بدهی

        public string? Description { get; set; }
    }

}