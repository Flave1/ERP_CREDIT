using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanReviewApplicationObj : GeneralEntity
    {
        public int LoanReviewApplicationId { get; set; }

        public int LoanId { get; set; }

        public int ProductId { get; set; }

        public int OperationId { get; set; }

        public int CustomerId { get; set; }

        public string ReviewDetails { get; set; }

        public int ApprovalStatusId { get; set; }

        public bool GenerateOfferLetter { get; set; }

        public bool? OperationPerformed { get; set; }

        public int ProposedTenor { get; set; }

        public double ProposedRate { get; set; }

        public decimal ProposedAmount { get; set; }

        public int ApprovedTenor { get; set; }

        public double ApprovedRate { get; set; }

        public decimal ApprovedAmount { get; set; }

        public decimal? Prepayment { get; set; }

        public string CustomerName { get; set; }

        public string ProductName { get; set; }

        public DateTime? FirstPrincipalPaymentDate { get; set; }

        public DateTime? FirstInterestPaymentDate { get; set; }

        public int? PrincipalFrequency { get; set; }

        public int? InterestFrequency { get; set; }

        public string PrincipalFrequencyName { get; set; }

        public string InterestFrequencyName { get; set; }
    }
}
