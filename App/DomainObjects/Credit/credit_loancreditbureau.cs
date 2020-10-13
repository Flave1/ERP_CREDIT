using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancreditbureau
    {
        [Key]
        public int LoanCreditBureauId { get; set; }

        public int LoanApplicationId { get; set; }

        public int CreditBureauId { get; set; }

        public decimal? ChargeAmount { get; set; }

        public bool? ReportStatus { get; set; }

        public byte[] SupportDocument { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_creditbureau credit_creditbureau { get; set; }

        public virtual credit_loanapplication credit_loanapplication { get; set; }
    }
}
