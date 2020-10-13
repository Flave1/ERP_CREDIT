using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TillVaultSummaryObj
    {
        public int TillVaultSummaryId { get; set; }

        public int? TillVaultId { get; set; }

        public int? TransactionCount { get; set; }

        public decimal? TotalAmountCurrency { get; set; }

        public decimal? TransferAmount { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTillVaultSummaryObj
    {
        public int TillVaultSummaryId { get; set; }

        public int? TillVaultId { get; set; }

        public int? TransactionCount { get; set; }

        public decimal? TotalAmountCurrency { get; set; }

        public decimal? TransferAmount { get; set; }
    }

    public class TillVaultSummaryRegRespObj
    {
        public int TillVaultSummaryId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TillVaultSummaryRespObj
    {
        public List<TillVaultSummaryObj> TillVaultSummaries { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

