using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanRecommendationLogObj
    {
        public int LoanApplicationId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Tenor { get; set; }

        public double Rate { get; set; }

        public decimal Amount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? FirstPrincipalPaymentDate { get; set; }

        public DateTime? FirstInterestPaymentDate { get; set; }

        public int? PrincipalFrequency { get; set; }

        public int? InterestFrequency { get; set; }

        public string PrincipalFrequencyName { get; set; }

        public string InterestFrequencyName { get; set; }
    }
}
