using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.InvestorFund
{
    public class LiquidationObj
    {
        public int LiquidationId { get; set; }
        public int WebsiteLiquidationOperationId { get; set; }
        public int InvestorFundId { get; set; }
        public int? CustomerTypeId { get; set; }
        public string InvestorName { get; set; }
        public string CustomerTypeName { get; set; }
        public string RefNumber { get; set; }
        public string CurrencyName { get; set; }
        public string ExchangeRate { get; set; }
        public int? InvestorFundCustomerId { get; set; }

        public int? ProductId { get; set; }

        public int? ProposedTenor { get; set; }

        public decimal? ProposedRate { get; set; }

        public int? FrequencyId { get; set; }

        public int? Period { get; set; }

        public decimal? ProposedAmount { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string InvestmentPurpose { get; set; }

        public DateTime? LiquidationDate { get; set; }

        public decimal? EarlyTerminationCharge { get; set; }

        public decimal? AmountPayable { get; set; }

        public int? DrProductPrincipal { get; set; }

        public int? CrIntExpense { get; set; }

        public int? DrIntPayable { get; set; }

        public int? CrReceiverPrincipalGL { get; set; }

        public int? ApprovalStatus { get; set; }

        public string PaymentAccount { get; set; }
        public string FrequencyName { get; set; }
        public string RelationshipManager { get; set; }

        public string Account { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? S_date { get; set; }
    }

    public class AddUpdateLiquidationObj
    {
        public int WebsiteLiquidationOperationId { get; set; }
        public int LiquidationId { get; set; }

        public int InvestorFundId { get; set; }

        public int? InvestorFundCustomerId { get; set; }

        public int? ProductId { get; set; }

        public int? ProposedTenor { get; set; }

        public decimal? ProposedRate { get; set; }

        public int? FrequencyId { get; set; }

        public int? Period { get; set; }

        public decimal? ProposedAmount { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [StringLength(50)]
        public string InvestmentPurpose { get; set; }

        public DateTime? LiquidationDate { get; set; }

        public decimal? EarlyTerminationCharge { get; set; }

        public decimal? AmountPayable { get; set; }

        public int? DrProductPrincipal { get; set; }

        public int? CrIntExpense { get; set; }

        public int? DrIntPayable { get; set; }

        public int? CrReceiverPrincipalGL { get; set; }

        public int? ApprovalStatus { get; set; }

        [StringLength(500)]
        public string PaymentAccount { get; set; }

        [StringLength(500)]
        public string Account { get; set; }
    }

    public class LiquidationRegRespObj
    {
        public int LiquidationId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LiquidationRespObj
    {
        public IEnumerable<LiquidationObj> Liquidations { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
        public IEnumerable<LiquidationRecommendationLogObj> LiquidationRecommendationLogs { get; set; }
        public IEnumerable<InvestmentListObj> InvestmentLists { get; set; }
    }

    public class LiquidationApprovalRecommendationObj
    {
        public int InvInvestorFundId { get; set; }
        public int ApprovedProductId { get; set; }
        public int ApprovedTenor { get; set; }
        public decimal ApprovedRate { get; set; }
        public decimal ApprovedAmount { get; set; }
        public DateTime? FirstPrincipalPaymentDate { get; set; }
        public DateTime? FirstInterestPaymentDate { get; set; }
        public int? PrincipalFrequency { get; set; }
        public int? InterestFrequency { get; set; }
        public string PrincipalFrequencyName { get; set; }
        public string InterestFrequencyName { get; set; }
        public List<LiquidationApprovalRecommendationObj> LiquidationApprovalRecommendations { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class LiquidationRecommendationLogObj
    {
        public int InvInvestorFundId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Tenor { get; set; }

        public decimal? Rate { get; set; }

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
