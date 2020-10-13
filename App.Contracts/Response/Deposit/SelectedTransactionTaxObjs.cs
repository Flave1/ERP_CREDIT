using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class SelectedTransactionTaxObj
    {
        public int SelectedTransTaxId { get; set; }

        public int? TransactionTaxId { get; set; }

        public int? AccountId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_transactiontax deposit_transactiontax { get; set; }
    }

    public class AddUpdateSelectedTransactionTaxObj
    {
        public int SelectedTransTaxId { get; set; }

        public int? TransactionTaxId { get; set; }

        public int? AccountId { get; set; }
    }

    public class SelectedTransactionTaxRegRespObj
    {
        public int SelectedTransTaxId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SelectedTransactionTaxRespObj
    {
        public List<SelectedTransactionTaxObj> selectedTransactionTaxes { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

