using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanapplication
    {
        [Key]
        public int LoanApplicationId { get; set; }

        public int CustomerId { get; set; }

        public int ProposedProductId { get; set; }

        public int ProposedTenor { get; set; }

        public double ProposedRate { get; set; }
        public decimal ProposedAmount { get; set; }

        public int ApprovedProductId { get; set; }

        public int ApprovedTenor { get; set; }

        public double ApprovedRate { get; set; }
        public decimal ApprovedAmount { get; set; }

        public int CurrencyId { get; set; }

        public double ExchangeRate { get; set; }

        public int ApprovalStatusId { get; set; }

        public bool HasDoneChecklist { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime MaturityDate { get; set; }

        public DateTime? FirstPrincipalDate { get; set; }

        public DateTime? FirstInterestDate { get; set; }

        public int? LoanApplicationStatusId { get; set; }

        public int? CompanyId { get; set; }
        public int? PaymentMode { get; set; }
        public int? RepaymentMode { get; set; }
        public string PaymentAccount { get; set; }
        public string RepaymentAccount { get; set; }

        [Required]
        [StringLength(50)]
        public string ApplicationRefNumber { get; set; }

        public decimal? Score { get; set; }

        public decimal? PD { get; set; }

        public bool GenerateOfferLetter { get; set; }

        [StringLength(2000)]
        public string Purpose { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string WorkflowToken { get; set; }

        public virtual ICollection<credit_corporateapplicationscorecard> credit_corporateapplicationscorecard { get; set; }
        public virtual ICollection<credit_corporateapplicationscorecarddetails> credit_corporateapplicationscorecarddetails { get; set; }
        public virtual ICollection<credit_individualapplicationscorecarddetails> credit_individualapplicationscorecarddetails { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }
        public virtual ICollection<credit_loanapplicationcollateral> credit_loanapplicationcollateral { get; set; }

        public virtual ICollection<credit_loanapplicationrecommendationlog> credit_loanapplicationrecommendationlog { get; set; }

        public virtual ICollection<credit_loancreditbureau> credit_loancreditbureau { get; set; }
    }
}
