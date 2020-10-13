using Banking.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.InvestorFund
{
    public class InvestorFundObj : GeneralEntity
    {
        public int InvestorFundId { get; set; }

        public int InvestorFundCustomerId { get; set; }

        public int? ProductId { get; set; }

        public int WebsiteInvestorFundId { get; set; }
        public int InvestorFundIdWebsiteRolloverId { get; set; }
        public int InvestorFundIdWebsiteTopupId { get; set; }

        public decimal? ProposedTenor { get; set; }

        public decimal? ProposedRate { get; set; }

        public int? FrequencyId { get; set; }

        public string Period { get; set; }

        public decimal? ProposedAmount { get; set; }
        public decimal? TopUpAmount { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string InvestmentPurpose { get; set; }

        public string FrequencyName { get; set; }
        public string CurrencyName { get; set; }

        public bool? EnableRollOver { get; set; }

        public int? InstrumentId { get; set; }

        [StringLength(50)]
        public string InstrumentNumer { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public int? CustomerNameId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal? TerminationCharge { get; set; }

        public string InvestorName { get; set; }

        public int? CustomerTypeId { get; set; }

        public string CustomerTypeName { get; set; }
        public string RelationshipManager { get; set; }

        public string AccountNumber { get; set; }

        public decimal? Payout { get; set; }

        public decimal? InterestEarned { get; set; }
        public long? ExchangeRate { get; set; }

        public decimal? ApprovedTenor { get; set; }

        public decimal? ApprovedRate { get; set; }

        public int? ApprovedProductId { get; set; }

        public string ApprovedProductName { get; set; }

        public DateTime? FirstPrincipalDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public decimal? ExpectedPayout { get; set; }

        public decimal? ExpectedInterest { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? InvestmentStatus { get; set; }

        public bool? GenerateCertificate { get; set; }

        public string RefNumber { get; set; }

        public DateTime? S_date { get; set; }
        public DateTime ApplicationDate { get; set; }

    }

    public class AddUpdateInvestorFundObj
    {
        public int InvestorFundId { get; set; }

        public int InvestorFundCustomerId { get; set; }

        public int? ProductId { get; set; }

        public int WebsiteInvestorFundId { get; set; }

        public decimal? ProposedTenor { get; set; }

        public decimal? ProposedRate { get; set; }

        public int? FrequencyId { get; set; }

        [StringLength(50)]
        public string Period { get; set; }

        public decimal? ProposedAmount { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [StringLength(50)]
        public string InvestmentPurpose { get; set; }

        public bool? EnableRollOver { get; set; }

        public int? InstrumentId { get; set; }

        [StringLength(50)]
        public string InstrumentNumer { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public int? CustomerNameId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal? ApprovedTenor { get; set; }

        public decimal? ApprovedRate { get; set; }

        public int? ApprovedProductId { get; set; }

        public DateTime? FirstPrincipalDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public decimal? ExpectedPayout { get; set; }

        public decimal? ExpectedInterest { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? InvestmentStatus { get; set; }

        public bool? GenerateCertificate { get; set; }

        [StringLength(10)]
        public string RefNumber { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? S_date { get; set; }
    }

    public class InvestorFundRegRespObj
    {
        public int InvestorFundId { get; set; }
        public string Link { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class InvestorFundRespObj
    {
        public IEnumerable<InvestorFundObj> InvestorFunds { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
        public decimal CurrentBalance { get; set; }
        public IEnumerable<InvestFundRollOverObj> RollOver { get; set; }
        public IEnumerable<InvestFundTopUpObj> TopUp { get; set; }
        public IEnumerable<InvestmentRecommendationLogObj> InvestmentRecommendationLogs { get; set; }
        public IEnumerable<InvestorRunningFacilitiesObj> InvestorRunningFacilities { get; set; }
        public IEnumerable<InvestmentListObj> InvestmentLists { get; set; }
        public IEnumerable<InvestorListObj> InvestorLists { get; set; }
    }

    public class InvestmentApprovalRecommendationObj
    {
        public int InvestorFundId { get; set; }
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
        public List<InvestmentApprovalRecommendationObj> InvestmentApprovalRecommendations { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class InvestmentRecommendationLogObj
    {
        public int InvestorFundId { get; set; }

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

    public class InvestorRunningFacilitiesObj : GeneralEntity
    {
        public int? InvestorFundCustomerId { get; set; }

        public int? CustomerNameId { get; set; }

        public string CustomerName { get; set; }
        public string ApprovalStatus { get; set; }

        public string ProductName { get; set; }

        public decimal? ApprovedTenor { get; set; }

        public decimal? ApprovedRate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public IEnumerable<InvestorRunningFacilitiesObj> InvestorRunningFacilities { get; set; }
        public APIResponseStatus Status { get; set; }

    }

    public class InvestmentListObj
    {
        public int InvestorFundId { get; set; }
        public int LiquidationId { get; set; }
        public int? ProductId { get; set; }

        public int? InvestorFundCustomerId { get; set; }
        public DateTime? ApplicationDate { get; set; }

        public string InvestorName { get; set; }
        public string ProductName { get; set; }
        public string workflowToken { get; set; }

        public string RefNumber { get; set; }

        public decimal? ProposedAmount { get; set; }

        public int? ApprovalStatus { get; set; }
    }

    public class InvestorListObj
    {
        public int InvestorFundId { get; set; }

        public int? InvestorFundCustomerId { get; set; }

        public int? CustomerTypeId { get; set; }

        public string CustomerTypeName { get; set; }

        public string CustomerName { get; set; }

        public decimal? CurrentBalance { get; set; }

        public decimal? ExpectedInterest { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }

    public class InvestFundTopUpObj
    {
        public int InvestorFundIdWebsiteTopupId { get; set; }
        public int InvestorFundId { get; set; }
        public decimal? TopUpAmount { get; set; }
        public string InvestorName { get; set; }
        public string RefNumber { get; set; }
        public int? ApprovalStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class InvestFundRollOverObj
    {
        public int InvestorFundIdWebsiteRolloverId { get; set; }
        public int InvestorFundId { get; set; }
        public string InvestorName { get; set; }
        public string RefNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public decimal? ApprovedTenor { get; set; }
        public decimal? RollOverAmount { get; set; }
        public int? ApprovalStatus { get; set; }
    }
    public class CurrencyExchangeRateObj : GeneralEntity
    {
        public int currencyId { get; set; }

        public DateTime date { get; set; }

        public double buyingRate { get; set; }

        public double sellingRate { get; set; }

        public int baseCurrencyId { get; set; }

        public bool isBaseCurrency { get; set; }

        public string webRequestStatus { get; set; }
    }

    public class DailyInterestAccrualObj : GeneralEntity

    {
        public string referenceNumber { get; set; }

        public string baseReferenceNumber { get; set; }

        public int categoryId { get; set; }

        public int transactionTypeId { get; set; }

        public int productId { get; set; }

        public int companyId { get; set; }

        public int branchId { get; set; }

        public int currencyId { get; set; }

        public double exchangeRate { get; set; }

        public decimal mainAmount { get; set; }

        public double interestRate { get; set; }

        public DateTime date { get; set; }

        public int dayCountConventionId { get; set; }

        public double dailyAccuralAmount { get; set; }

        public decimal availableBalance { get; set; }

        public int daysInAYear { get; set; }

    }
}
