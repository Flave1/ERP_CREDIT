using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class SelectedTransactionChargeObj
    {
        public int SelectedTransChargeId { get; set; }

        public int? TransactionChargeId { get; set; }

        public int? AccountId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_transactioncharge deposit_transactioncharge { get; set; }
    }

    public class AddUpdateSelectedTransactionChargeObj
    {
        public int SelectedTransChargeId { get; set; }

        public int? TransactionChargeId { get; set; }

        public int? AccountId { get; set; }
    }

    public class SelectedTransactionChargeRegRespObj
    {
        public int SelectedTransChargeId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SelectedTransactionChargeRespObj
    {
        public List<SelectedTransactionChargeObj> SelectedTransactionCharges { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

