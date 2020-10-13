using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanApplicationObjs
    {
        public class LoanApplicationObj : GeneralEntity
        {
            public int LoanApplicationId { get; set; }
            public int WebsiteLoanApplicationId { get; set; }
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
            public int StatusId { get; set; }
            public bool HasDoneChecklist { get; set; }
            public DateTime ApplicationDate { get; set; }
            public DateTime EffectiveDate { get; set; }
            public DateTime MaturityDate { get; set; }
            public DateTime? FirstInterestDate { get; set; }
            public DateTime? FirstPrincipalDate { get; set; }
            public string CustomerName { get; set; }
            public string Filepath1 { get; set; }
            public string Filepath2 { get; set; }
            public string ProductFrequency { get; set; }
            public string RiskBasedDescription { get; set; }
            public int? ProductTenor { get; set; }
            public string CurrencyName { get; set; }
            public string ProposedProductName { get; set; }
            public string ApprovedProductName { get; set; }
            public int? LoanApplicationStatusId { get; set; }
            public string ApplicationRefNumber { get; set; }
            public string WorkflowToken { get; set; }
            public string RelationOfficer1st { get; set; }
            public string RelationOfficer2nd { get; set; }
            public int? CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string ReportStatus { get; set; }
            public byte[] SupportDocument { get; set; }
            public int OfferLetterId { get; set; }
            public decimal? CreditScore { get; set; }
            public decimal? ProbabilityOfDefault { get; set; }
            public decimal? ProductWeightedScore { get; set; }
            public string Purpose { get; set; }
            public decimal? IntegralFee { get; set; }
            public int? FrequencyTypeId { get; set; }
            public int? Period { get; set; }
            public DateTime FirstPaymentDate { get; set; }
            public DateTime? DateCreated { get; set; }
            public int? PaymentMode { get; set; }
            public int? RepaymentMode { get; set; }
            public string PaymentAccount { get; set; }
            public string RepaymentAccount { get; set; }
        }

        public class AddUpdateLoanApplicationObj
        {
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

            public string ApplicationRefNumber { get; set; }

            public decimal? Score { get; set; }

            public decimal? PD { get; set; }

            public bool GenerateOfferLetter { get; set; }

            public string Purpose { get; set; }
        }

        public class LoanApplicationRegRespObj
        {
            public int LoanApplicationId { get; set; }         
            public string ApplicationRefNumber { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanApplicationRespObj
        {
            public int ResponseId { get; set; }
            public int LoanApplicationId { get; set; }
            public IEnumerable<LoanApplicationObj> LoanApplications { get; set; }
            
            public IEnumerable<ApprovalRecommendationObj> ApprovalRecommendations { get; set; }
            public IEnumerable<LoanRecommendationLogObjs> LoanRecommendationLogs { get; set; }
            public string ApplicationRefNumber { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanApplicationSearchObj
        {
            public int LoanApplicationId { get; set; }
            public int CustomerTypeId { get; set; }
            public int CustomerId { get; set; }
            public string refNumber { get; set; }
        }

        public class DeleteLoanApplicationCommand : IRequest<DeleteRespObj>
        {
            public List<int> LoanApplicationIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ApprovalRecommendationObj
        {
            public int LoanApplicationId { get; set; }
            public int ApprovedProductId { get; set; }
            public int ApprovedTenor { get; set; }
            public double ApprovedRate { get; set; }
            public decimal ApprovedAmount { get; set; }
            public DateTime? FirstPrincipalPaymentDate { get; set; }
            public DateTime? FirstInterestPaymentDate { get; set; }
            public int? PrincipalFrequency { get; set; }
            public int? InterestFrequency { get; set; }
            public string PrincipalFrequencyName { get; set; }
            public string InterestFrequencyName { get; set; }
        }

        public class LoanRecommendationLogObjs
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
}
