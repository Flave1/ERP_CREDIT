using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanreviewoperation
    {
        [Key]
        public int LoanReviewOperationId { get; set; }

        public int LoanId { get; set; }

        public int ProductId { get; set; }

        public int OperationId { get; set; }

        public DateTime EffectiveDate { get; set; }

        [Required]
        public string Comment { get; set; }

        public double? InterestRate { get; set; }

        public decimal? Prepayment { get; set; }

        public int? PrincipalFrequencyTypeId { get; set; }

        public int? InterestFrequencyTypeId { get; set; }

        public DateTime? PrincipalFirstPaymentDate { get; set; }

        public DateTime? InterestFirstPaymentDate { get; set; }
        public DateTime? MaturityDate { get; set; }

        public int? Tenor { get; set; }

        public int? CASA_AccountId { get; set; }

        public decimal? OverDraftTopUp { get; set; }

        public decimal? FeeCharges { get; set; }

        public int? ScheduleTypeId { get; set; }

        public int? ScheduleDayCountConventionId { get; set; }

        public int? SchedultDayInterestTypeId { get; set; }

        public int ApprovalStatusId { get; set; }

        public bool IsManagementInterest { get; set; }

        public bool OperationComleted { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<credit_loanreviewoperationirregularinput> credit_loanreviewoperationirregularinput { get; set; }
    }
}
