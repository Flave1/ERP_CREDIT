using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TransferFormObj
    {
        public int TransferFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? ValueDate { get; set; }

        public string ExternalReference { get; set; }

        public string TransactionReference { get; set; }

        public string PayingAccountNumber { get; set; }

        public string PayingAccountName { get; set; }

        public string PayingAccountCurrency { get; set; }

        public decimal? Amount { get; set; }

        public string BeneficiaryAccountNumber { get; set; }

        public string BeneficiaryAccountName { get; set; }

        public string BeneficiaryAccountCurrency { get; set; }

        public string TransactionNarration { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTransferFormObj
    {
        public int TransferFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValueDate { get; set; }

        [StringLength(50)]
        public string ExternalReference { get; set; }

        [StringLength(50)]
        public string TransactionReference { get; set; }

        [StringLength(50)]
        public string PayingAccountNumber { get; set; }

        [StringLength(50)]
        public string PayingAccountName { get; set; }

        [StringLength(50)]
        public string PayingAccountCurrency { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountNumber { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountName { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountCurrency { get; set; }

        [StringLength(50)]
        public string TransactionNarration { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; }
    }

    public class TransferFormRegRespObj
    {
        public int TransferFormId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransferFormRespObj
    {
        public List<TransferFormObj> TransferForms { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

