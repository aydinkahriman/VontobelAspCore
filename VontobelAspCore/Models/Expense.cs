using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VontobelAspCore.Models
{
    // we need to replace the magic strings and get them from Resources

    public enum Currency { USD, EUR, CHF }
    public enum ExpenseType { Food, Drinks, Transportation, Grocery, Others }
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date of Expense")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Expense Amount")]
        [Range(1, 1000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public Currency Currency { get; set; }

        [Required]
        [Display(Name = "Expense Type")]
        public ExpenseType Type { get; set; }
    }
}
