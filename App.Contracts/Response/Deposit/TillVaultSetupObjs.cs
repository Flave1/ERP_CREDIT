using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TillVaultSetupObj
    {
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public string StructureTillIdPrefix { get; set; }

        public string TellerTillIdPrefix { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateTillVaultSetupObj
    {
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        [StringLength(50)]
        public string StructureTillIdPrefix { get; set; }

        [StringLength(50)]
        public string TellerTillIdPrefix { get; set; }
    }

    public class TillVaultSetupRegRespObj
    {
        public int TillVaultSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TillVaultSetupRespObj
    {
        public List<TillVaultSetupObj> TillVaultSetups { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

