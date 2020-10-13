using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TransactionCorrectionFormObj
    {
        public int TransactionCorrectionId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        public DateTime? QueryStartDate { get; set; }

        public DateTime? QueryEndDate { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTransactionCorrectionFormObj
    {
        public int TransactionCorrectionId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        public DateTime? QueryStartDate { get; set; }

        public DateTime? QueryEndDate { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; }
    }

    public class TransactionCorrectionFormRegRespObj
    {
        public int TransactionCorrectionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransactionCorrectionFormRespObj
    {
        public List<TransactionCorrectionFormObj> TransactionCorrectionForms { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

