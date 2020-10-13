using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class ChangeOfRateSetupObj
    {
        public int ChangeOfRateSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? CanApply { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateChangeOfRateSetupObj
    {
        public int ChangeOfRateSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? CanApply { get; set; }
    }

    public class ChangeOfRateSetupRegRespObj
    {
        public int ChangeOfRateSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ChangeOfRateSetupRespObj
    {
        public List<ChangeOfRateSetupObj> ChangeOfRateSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

