using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanapplicationrecommendationlog
    {
        [Key]
        public int LoanApplicationRecommendationLogId { get; set; }

        public int LoanApplicationId { get; set; }

        public int ApprovedProductId { get; set; }

        public int ApprovedTenor { get; set; }

        public double ApprovedRate { get; set; }

        public decimal ApprovedAmount { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loanapplication credit_loanapplication { get; set; }

        public virtual credit_product credit_product { get; set; }
    }
}
