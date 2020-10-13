using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class WithdrawalFormObj
    {
        public int WithdrawalFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public string TransactionReference { get; set; }

        public string AccountNumber { get; set; }

        public int? AccountType { get; set; }

        public int? Currency { get; set; }

        public decimal? Amount { get; set; }

        public string TransactionDescription { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? ValueDate { get; set; }

        public string WithdrawalType { get; set; }

        public string InstrumentNumber { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateWithdrawalFormObj
    {
        public int WithdrawalFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        [StringLength(50)]
        public string TransactionReference { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? AccountType { get; set; }

        public int? Currency { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string TransactionDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValueDate { get; set; }

        [StringLength(50)]
        public string WithdrawalType { get; set; }

        [StringLength(50)]
        public string InstrumentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InstrumentDate { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; }
    }

    public class WithdrawalFormRegRespObj
    {
        public int WithdrawalFormId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WithdrawalFormRespObj
    {
        public List<WithdrawalFormObj> WithdrawalForms { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

