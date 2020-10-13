using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_individualapplicationscorecarddetails
    {
        [Key]
        public int ApplicationCreditScoreId { get; set; }

        public int LoanApplicationId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string AttributeField { get; set; }

        public decimal Score { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loanapplication credit_loanapplication { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }

        public virtual credit_product credit_product { get; set; }
    }
}
