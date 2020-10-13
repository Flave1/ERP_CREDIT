using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;

namespace Banking.Contracts.Response.Credit
{
    public class LoanReviewListObj : GeneralEntity
    {
        public int LoanId { get; set; }

        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public string CustomerName { get; set; }

        public int ProductId { get; set; }

        public int OperationId { get; set; }

        public int? ProductTypeId { get; set; }

        public string ProductName { get; set; }
        public string OperationType { get; set; }

        public int LoanApplicationId { get; set; }

        public string LoanRefNumber { get; set; }

        public string Status { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal OutstandingBalance { get; set; }

        public decimal? Prepayment { get; set; }

        public decimal? PropposedAmount { get; set; }

        public double? PropposedInterestRate { get; set; }

        public decimal? PropposedTenor { get; set; }

        public DateTime BookingDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal? CreditScore { get; set; }

        public decimal? ProbabilityOfDefault { get; set; }

        public int? LoanReviewApplicationId { get; set; }

        public DateTime? FirstPrincipalPaymentDate { get; set; }

        public DateTime? FirstInterestPaymentDate { get; set; }

        public int? PrincipalFrequency { get; set; }

        public int? InterestFrequency { get; set; }

        public string PrincipalFrequencyName { get; set; }

        public string InterestFrequencyName { get; set; }
        public string WorkflowToken { get; set; }
    }

    
    public class LoanReviewListObjRespObj
    {
        public IEnumerable<LoanReviewListObj> LoanReviewList { get; set; }
        public IEnumerable<LoanReviewApplicationObj> LoanReviewApplication { get; set; }
        public ApprovalRecommendationObj LoanApprovalRecommendationLog { get; set; }
        public IEnumerable<LoanRecommendationLogObj> LoanRecommendationLog { get; set; }
        public APIResponseStatus Status { get; set; }
        public int ResponseId { get; set; }
    }

    public class SearchObj
    {
        public int? CustomerTypeId { get; set; }
        public int? ProductTypeId { get; set; }
        public string SearchString { get; set; }
    }

    public class SearchQueryObj
    {
        public int CustomerId { get; set; }
        public int LoanReviewApplicationId { get; set; }
        public int LoanId { get; set; }
    }
}
