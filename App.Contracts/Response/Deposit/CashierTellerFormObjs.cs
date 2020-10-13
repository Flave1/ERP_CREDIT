using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CashierTellerFormObj
    {
        public int DepositCashierTellerId { get; set; }

        public int Structure { get; set; }

        public int? SubStructure { get; set; }

        public int? Currency { get; set; }

        public DateTime? Date { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateCashierTellerFormObj
    {
        public int DepositCashierTellerId { get; set; }

        public int Structure { get; set; }

        public int? SubStructure { get; set; }

        public int? Currency { get; set; }

        public DateTime? Date { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; }
    }

    public class CashierTellerFormRegRespObj
    {
        public int DepositCashierTellerId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CashierTellerFormRespObj
    {
        public List<CashierTellerFormObj> BusinessCategories { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

