using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User is required")]
        public User User { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters")]
        public string Comment { get; set; }
    }
}