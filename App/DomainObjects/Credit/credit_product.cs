using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        public int PaymentType { get; set; }

        public bool CollateralRequired { get; set; }

        public bool QuotedInstrument { get; set; }

        public double Rate { get; set; }

        public bool LateRepayment { get; set; }

        public int? Period { get; set; }

        public int? CleanUpCircle { get; set; }

        public decimal? WeightedMaxScore { get; set; }

        public int? Default { get; set; }

        public int? DefaultRange { get; set; }

        public decimal? Significant2 { get; set; }

        public decimal? Significant3 { get; set; }

        public int? ProductTypeId { get; set; }

        public int? PrincipalGL { get; set; }

        public int? InterestIncomeExpenseGL { get; set; }

        public int? InterestReceivablePayableGL { get; set; }

        public int? FrequencyTypeId { get; set; }

        public int? TenorInDays { get; set; }

        public int? ScheduleTypeId { get; set; }

        public decimal? CollateralPercentage { get; set; }

        public decimal? ProductLimit { get; set; }

        public double? LateTerminationCharge { get; set; }

        public double? EarlyTerminationCharge { get; set; }

        public double? LowRiskDefinition { get; set; }

        public int? FeeIncomeGL { get; set; }

        public int? InterestType { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<credit_corporateapplicationscorecard> credit_corporateapplicationscorecard { get; set; }

        public virtual ICollection<credit_corporateapplicationscorecarddetails> credit_corporateapplicationscorecarddetails { get; set; }

        public virtual ICollection<credit_individualapplicationscorecard> credit_individualapplicationscorecard { get; set; }

        public virtual ICollection<credit_individualapplicationscorecarddetails> credit_individualapplicationscorecarddetails { get; set; }
        public virtual ICollection<credit_loanapplication> credit_loanapplication { get; set; }
        public virtual ICollection<credit_loanapplicationrecommendationlog> credit_loanapplicationrecommendationlog { get; set; }
        public virtual credit_repaymenttype credit_repaymenttype { get; set; }
        public virtual ICollection<credit_productfee> credit_productfee { get; set; }
        public virtual ICollection<credit_weightedriskscore> credit_weightedriskscore { get; set; }
    }
}
