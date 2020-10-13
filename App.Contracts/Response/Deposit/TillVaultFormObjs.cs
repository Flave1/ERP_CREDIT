using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TillVaultFormObj
    {
        public int TillVaultId { get; set; }

        public int? TillId { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? IncomingCash { get; set; }

        public decimal? OutgoingCash { get; set; }

        public decimal? ClosingBalance { get; set; }

        public decimal? CashAvailable { get; set; }

        public string Shortage { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTillVaultFormObj
    {
        public int TillVaultId { get; set; }

        public int? TillId { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? IncomingCash { get; set; }

        public decimal? OutgoingCash { get; set; }

        public decimal? ClosingBalance { get; set; }

        public decimal? CashAvailable { get; set; }

        [StringLength(10)]
        public string Shortage { get; set; }
    }

    public class TillVaultFormRegRespObj
    {
        public int TillVaultId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TillVaultFormRespObj
    {
        public List<TillVaultFormObj> TillVaultForms { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

