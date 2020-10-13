using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TillVaultOpeningCloseObj
    {
        public int TillVaultOpeningCloseId { get; set; }

        public DateTime? Date { get; set; }

        public int? Currency { get; set; }

        public decimal? AmountPerSystem { get; set; }

        public decimal? CashAvailable { get; set; }

        public decimal? Shortage { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTillVaultOpeningCloseObj
    {
        public int TillVaultOpeningCloseId { get; set; }

        public DateTime? Date { get; set; }

        public int? Currency { get; set; }

        public decimal? AmountPerSystem { get; set; }

        public decimal? CashAvailable { get; set; }

        public decimal? Shortage { get; set; }
    }

    public class TillVaultOpeningCloseRegRespObj
    {
        public int TillVaultOpeningCloseId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TillVaultOpeningCloseRespObj
    {
        public List<TillVaultOpeningCloseObj> TillVaultOpeningCloses { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

