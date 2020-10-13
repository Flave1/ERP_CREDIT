using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loan
    {
        [Key]
        public int LoanId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int LoanApplicationId { get; set; }

        public int? PrincipalFrequencyTypeId { get; set; }

        public int? InterestFrequencyTypeId { get; set; }

        public int? ScheduleTypeId { get; set; }

        public int CurrencyId { get; set; }

        public double ExchangeRate { get; set; }

        public int ApprovalStatusId { get; set; }

        public int ApprovedBy { get; set; }

        [StringLength(50)]
        public string ApprovedComments { get; set; }

        public DateTime ApprovedDate { get; set; }

        public DateTime BookingDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime MaturityDate { get; set; }

        public int? LoanStatusId { get; set; }

        public bool IsDisbursed { get; set; }

        public int? DisbursedBy { get; set; }

        [StringLength(50)]
        public string DisbursedComments { get; set; }

        public DateTime? DisbursedDate { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(50)]
        public string LoanRefNumber { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal? EquityContribution { get; set; }

        public DateTime? FirstPrincipalPaymentDate { get; set; }

        public DateTime? FirstInterestPaymentDate { get; set; }

        public decimal? OutstandingAmortisedPrincipal { get; set; }

        public decimal? OutstandingPrincipal { get; set; }
        public decimal? OutstandingOldPrincipal { get; set; }

        public decimal? OutstandingOldInterest { get; set; }

        public decimal? OutstandingInterest { get; set; }

        public int? AccrualBasis { get; set; }

        public int? FirstDayType { get; set; }

        public DateTime? NPLDate { get; set; }

        public int? CustomerRiskRatingId { get; set; }

        public int? LoanOperationId { get; set; }

        public int? StaffId { get; set; }

        public int? CasaAccountId { get; set; }

        public int? BranchId { get; set; }

        public decimal? PastDuePrincipal { get; set; }

        public decimal? PastDueInterest { get; set; }

        public double InterestRate { get; set; }

        public decimal? InterestOnPastDueInterest { get; set; }

        public decimal? InterestOnPastDuePrincipal { get; set; }

        public decimal? IntegralFeeAmount { get; set; }

        public string WorkflowToken { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
