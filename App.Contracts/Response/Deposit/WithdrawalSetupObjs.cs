using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class WithdrawalSetupObj
    {
        public int WithdrawalSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public decimal? DailyWithdrawalLimit { get; set; }

        public bool? WithdrawalCharges { get; set; }

        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateWithdrawalSetupObj
    {
        public int WithdrawalSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public decimal? DailyWithdrawalLimit { get; set; }

        public bool? WithdrawalCharges { get; set; }

        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }
    }

    public class WithdrawalSetupRegRespObj
    {
        public int WithdrawalSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WithdrawalSetupRespObj
    {
        public List<WithdrawalSetupObj> WithdrawalSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

