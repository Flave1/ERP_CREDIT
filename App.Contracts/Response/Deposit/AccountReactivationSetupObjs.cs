using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class AccountReactivationSetupObj
    {
        public int ReactivationSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? ChargesApplicable { get; set; }

        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }

        public bool? PresetChart { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateAccountReactivationSetupObj
    {
        public int ReactivationSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? ChargesApplicable { get; set; }

        [Required]
        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string ChargeType { get; set; }

        public bool? PresetChart { get; set; }

        public bool? Active { get; set; }
    }

    public class AccountReactivationSetupRegRespObj
    {
        public int ReactivationSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountReactivationSetupRespObj
    {
        public List<AccountReactivationSetupObj> ReactivationSetup { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

