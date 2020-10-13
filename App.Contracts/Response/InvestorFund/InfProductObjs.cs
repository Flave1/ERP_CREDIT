using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.InvestorFund
{
    public class InfProductObj
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public string InterestRepaymentTypeName { get; set; }
        public string ProductTypeName { get; set; }
        public string ScheduleMethodName { get; set; }
        public string FrequencyName { get; set; }

        public decimal? Rate { get; set; }

        public int? ProductTypeId { get; set; }

        public int? ProductLimit { get; set; }

        public decimal? InterestRateMax { get; set; }

        public int? InterestRepaymentTypeId { get; set; }

        public int? ScheduleMethodId { get; set; }

        public int? FrequencyId { get; set; }

        public decimal? MaximumPeriod { get; set; }

        public decimal? InterestRateAnnual { get; set; }

        public decimal? InterestRateFrequency { get; set; }

        public int? ProductPrincipalGl { get; set; }

        public int? ReceiverPrincipalGl { get; set; }

        public int? InterstExpenseGl { get; set; }

        public int? InterestPayableGl { get; set; }

        public int? ProductLimitId { get; set; }

        public decimal? EarlyTerminationCharge { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string Updatedby { get; set; }

        public string Createdby { get; set; }

        public DateTime? Updatedon { get; set; }

        public DateTime? Createdon { get; set; }
    }

    public class AddUpdateInfProductObj
    {
        public int ProductId { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal? Rate { get; set; }

        public int? ProductTypeId { get; set; }

        public int? ProductLimit { get; set; }

        public decimal? InterestRateMax { get; set; }

        public int? InterestRepaymentTypeId { get; set; }

        public int? ScheduleMethodId { get; set; }

        public int? FrequencyId { get; set; }

        public decimal? MaximumPeriod { get; set; }

        public decimal? InterestRateAnnual { get; set; }

        public decimal? InterestRateFrequency { get; set; }

        public int? ProductPrincipalGl { get; set; }

        public int? ReceiverPrincipalGl { get; set; }

        public int? InterstExpenseGl { get; set; }

        public int? InterestPayableGl { get; set; }

        public int? ProductLimitId { get; set; }

        public decimal? EarlyTerminationCharge { get; set; }
    }

    public class InfProductRegRespObj
    {
        public int ProductId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class InfProductRespObj
    {
        public List<InfProductObj> InfProducts { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }


  

    //public class DeleteRequest
    //{
    //    public List<int> ItemIds { get; set; }
    //}

    //public class DeleteRespObjt
    //{
    //    public bool Deleted { get; set; }
    //    public APIResponseStatus Status { get; set; }
    //}
}
