using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanStagingObjs
    {
        public class LoanStagingObj
        {
            public int LoanStagingId { get; set; }
            public int ProbationPeriod { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public bool? Active { get; set; }
            public bool? Deleted { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
        }

        public class AddUpdateLoanStagingObj: GeneralEntity
        {
            public int LoanStagingId { get; set; }
            public int ProbationPeriod { get; set; }
            public int From { get; set; }
            public int To { get; set; }
        }

        public class LoanStagingRegRespObj
        {
            public int LoanStagingId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanStagingRespObj
        {
            public List<LoanStagingObj> LoanStagings { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanStagingSearchObj
        {
            public int LoanStagingId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteLoanStagingCommand : IRequest<DeleteRespObj>
        {
            public List<int> LoanStagingIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
