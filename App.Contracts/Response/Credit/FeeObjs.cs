using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class FeeObjs
    {
        public class FeeObj
        {
            public int FeeId { get; set; }
            public string FeeName { get; set; }
            public bool IsIntegral { get; set; }
            public int? TotalFeeGL { get; set; }
            public bool? Active { get; set; }
            public bool? Deleted { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
        }

        public class RepaymentTypeObj
        {
            public int RepaymentTypeId { get; set; }
            public string RepaymentTypeName { get; set; }
            public bool? Active { get; set; }
            public bool? Deleted { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
        }

        public class AddUpdateFeeObj
        {
            public int FeeId { get; set; }
            [Required]
            [StringLength(50)]
            public string FeeName { get; set; }
            public bool IsIntegral { get; set; }
            public int? TotalFeeGL { get; set; }
        }

        public class FeeRegRespObj
        {
            public int FeeId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class FeeRespObj
        {
            public List<FeeObj> Fees { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class RepaymentTypeRespObj
        {
            public List<RepaymentTypeObj> RepaymentType { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class FeeSearchObj
        {
            public int FeeId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteFeeCommand : IRequest<DeleteRespObj>
        {
            public List<int> FeeIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
