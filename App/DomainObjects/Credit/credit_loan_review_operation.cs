using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loan_review_operation
    {
        [Key]
        public int LoanReviewOperationId { get; set; }

        public int LoanId { get; set; }

        public int ProductTypeId { get; set; }

        public int OperationTypeId { get; set; }
        public DateTime EffectiveDate { get; set; }

        [Required]
        public string ReviewDatials { get; set; }

        public double? InterestRate { get; set; }

        public decimal? Prepayment { get; set; }

        public int? PrincipalFrequencyTypeId { get; set; }

        public int? InterestFrequencyTypeId { get; set; }

        public DateTime? PrincipalFirstPaymentDate { get; set; }

   
        public DateTime? InterestFirstPaymentDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public int? Tenor { get; set; }

        public int? CasaAccountId { get; set; }

        public decimal? FeeCharges { get; set; }

        public int ApprovalStatusId { get; set; }

        public bool ISManagementRate { get; set; }

        public bool OperationCompleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
