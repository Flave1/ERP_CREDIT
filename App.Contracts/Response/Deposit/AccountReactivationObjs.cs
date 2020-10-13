using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class AccountReactivationObj
    {
        public int ReactivationId { get; set; }

        public int? Structure { get; set; }

        public int? Substructure { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public decimal? AccountBalance { get; set; }

        public int? Currency { get; set; }

        public decimal? Balance { get; set; }

        public string Reason { get; set; }

        public string Charges { get; set; }

        public string ApproverName { get; set; }

        public string ApproverComment { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateAccountReactivationObj
    {
        public int ReactivationId { get; set; }

        public int? Structure { get; set; }

        public int? Substructure { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public decimal? AccountBalance { get; set; }

        public int? Currency { get; set; }

        public decimal? Balance { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Charges { get; set; }

        [StringLength(50)]
        public string ApproverName { get; set; }

        [StringLength(50)]
        public string ApproverComment { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AccountReactivationRegRespObj
    {
        public int ReactivationId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountReactivationRespObj
    {
        public List<AccountReactivationObj> AccountReactivations { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

