using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loan_past_due
    {
        [Key]
        public int PastDueId { get; set; }

        public int LoanId { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateWithDefault { get; set; }

        public int TransactionTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string PastDueCode { get; set; }

        [StringLength(50)]
        public string Parent_PastDueCode { get; set; }

        [Required]
        [StringLength(800)]
        public string Description { get; set; }

        public decimal DebitAmount { get; set; }

        public decimal CreditAmount { get; set; }
    }
}
