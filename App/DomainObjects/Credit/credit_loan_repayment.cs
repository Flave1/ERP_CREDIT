using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loan_repayment
    {
        [Key]
        public int LoanRepaymentId { get; set; }

        public int LoanId { get; set; }
        public DateTime Date { get; set; }

        public decimal InterestAmount { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}
