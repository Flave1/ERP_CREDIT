using Banking.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class AddUpdateAccountSetupObj
    {
        public int DepositAccountId { get; set; }

        [Required]
        [StringLength(500)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int AccountTypeId { get; set; }

        public int? CurrencyId { get; set; }

        [Required]
        [StringLength(50)]
        public string DormancyDays { get; set; }

        public decimal InitialDeposit { get; set; }

        public int CategoryId { get; set; }

        public int? BusinessCategoryId { get; set; }

        public int? GLMapping { get; set; }

        public int? BankGl { get; set; }

        public decimal? InterestRate { get; set; }

        [Required]
        [StringLength(50)]
        public string InterestType { get; set; }

        public bool? CheckCollecting { get; set; }

        [Required]
        [StringLength(50)]
        public string MaturityType { get; set; }

        public int? ApplicableTaxId { get; set; }

        public int? ApplicableChargesId { get; set; }

        public bool? PreTerminationLiquidationCharge { get; set; }

        public int? InterestAccrual { get; set; }

        public bool? Status { get; set; }

        public bool? OperatedByAnother { get; set; }

        public bool? CanNominateBenefactor { get; set; }

        public bool? UsePresetChartofAccount { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionPrefix { get; set; }

        [Required]
        [StringLength(50)]
        public string CancelPrefix { get; set; }

        [Required]
        [StringLength(50)]
        public string RefundPrefix { get; set; }

        public bool? Useworkflow { get; set; }

        public bool? CanPlaceOnLien { get; set; }

        public bool? InUse { get; set; }
    }

    public class AccountSetupRegRespObj
    {
        public int DepositAccountId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountSetupRespObj
    {
        public IEnumerable<DepositAccountObj> DepositAccounts { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DepositAccountObj : GeneralEntity
    {
        public int DepositAccountId { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypename { get; set; }
        public string DormancyDays { get; set; }
        public decimal InitialDeposit { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? BusinessCategoryId { get; set; }
        public decimal? InterestRate { get; set; }
        public string InterestType { get; set; }
        public bool? CheckCollecting { get; set; }
        public string MaturityType { get; set; }
        public int[] ApplicableTaxId { get; set; }
        public int[] ApplicableChargesId { get; set; }
        public bool? PreTerminationLiquidationCharge { get; set; }
        public int? InterestAccrual { get; set; }
        public string InterestAccrualName { get; set; }
        public bool? Status { get; set; }
        public bool? OperatedByAnother { get; set; }
        public bool? CanNominateBenefactor { get; set; }
        public bool? UsePresetChartofAccount { get; set; }
        public string TransactionPrefix { get; set; }
        public string CancelPrefix { get; set; }
        public string RefundPrefix { get; set; }
        public bool? Useworkflow { get; set; }
        public bool? CanPlaceOnLien { get; set; }
        public int? GLMapping { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int? BankGl { get; set; }

    }


    public partial class DepositformObj : GeneralEntity
    {
        public int DepositFormId { get; set; }
        public int Structure { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public int? Operation { get; set; }
        public int? TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string AcountName { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionParticulars { get; set; }
        public string Remark { get; set; }
        public string ModeOfTransaction { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? InstrumentDate { get; set; }
    }

    public class DepositFormRespObj
    {
        public IEnumerable<DepositformObj> DepositForm { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

