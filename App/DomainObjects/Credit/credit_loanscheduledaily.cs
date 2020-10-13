using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanscheduledaily
    {
        [Key]
        public int LoanScheduleDailyId { get; set; }

        public int LoanId { get; set; }

        public int PaymentNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal StartPrincipalAmount { get; set; }

        public decimal DailyPaymentAmount { get; set; }
        public decimal DailyInterestAmount { get; set; }
        public decimal DailyPrincipalAmount { get; set; }

        public decimal ClosingBalance { get; set; }
        public decimal EndPrincipalAmount { get; set; }

        public decimal AccruedInterest { get; set; }

        public decimal AmortisedCost { get; set; }

        public double InterestRate { get; set; }

        public decimal AmortisedOpeningBalance { get; set; }

        public decimal AmortisedStartPrincipalAmount { get; set; }

        public decimal AmortisedDailyPaymentAmount { get; set; }

        public decimal AmortisedDailyInterestAmount { get; set; }

        public decimal AmortisedDailyPrincipalAmount { get; set; }

        public decimal AmortisedClosingBalance { get; set; }

        public decimal AmortisedEndPrincipalAmount { get; set; }

        public decimal AmortisedAccruedInterest { get; set; }

        public decimal Amortised_AmortisedCost { get; set; }

        public decimal UnearnedFee { get; set; }

        public decimal EarnedFee { get; set; }

        public double EffectiveInterestRate { get; set; }

        public int? StaffId { get; set; }

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
