using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CashierTellerSetupObj
    {
        public int DepositCashierTellerSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? PresetChart { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateCashierTellerSetupObj
    {
        public int DepositCashierTellerSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? PresetChart { get; set; }
    }

    public class CashierTellerSetupRegRespObj
    {
        public int DepositCashierTellerSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CashierTellerSetupRespObj
    {
        public List<CashierTellerSetupObj> DepositCashierTellerSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

