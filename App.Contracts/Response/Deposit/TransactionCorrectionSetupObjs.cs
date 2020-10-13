using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TransactionCorrectionSetupObj
    {
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTransactionCorrectionSetupObj
    {
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; }
    }

    public class TransactionCorrectionSetupRegRespObj
    {
        public int TransactionCorrectionSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransactionCorrectionSetupRespObj
    {
        public List<TransactionCorrectionSetupObj> TransactionCorrectionSetups { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}


