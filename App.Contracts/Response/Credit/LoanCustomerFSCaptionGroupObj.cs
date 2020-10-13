using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;

namespace Banking.Contracts.Response.Credit
{
    public class LoanCustomerFSCaptionGroupObj : GeneralEntity
    {
        public int FSCaptionGroupId { get; set; }
        public string FSCaptionGroupName { get; set; }
    }

    public class LoanCustomerFSCaptionGroupRespObj
    {
        public IEnumerable<LoanCustomerFSCaptionGroupObj> LoanCustomerFSGroup { get; set; }
        public IEnumerable<LoanCustomerFSCaptionObj> LoanCustomerFSCaption { get; set; }
        public IEnumerable<LoanCustomerFSCaptionDetailObj> LoanCustomerFSCaptionDetail { get; set; }
        public IEnumerable<LoanCustomerFSRatioDetailObj> LoanCustomerFSRatioDetail { get; set; }
        public IEnumerable<LoanCustomerFSRatioCaptionObj> LoanCustomerFSRatioCaption{ get; set; }
        public IEnumerable<LoanCustomerFSRatiosCalculationObj> LoanCustomerFSRatioCalculation{ get; set; }
        public IEnumerable<LoanCustomerFSRatioCaptionReportObj> LoanCustomerFSRatioCaptionReport{ get; set; }
        public IEnumerable<LookupObj> Types{ get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerFSSearchObj
    {
        public int FSCaptionGroupId { get; set; }
        public int FSCaptionId { get; set; }
        public int FSDetailId { get; set; }
        public int RatioDetailId { get; set; }
        public int RatioCaptionId { get; set; }
    }

    public class LoanCustomerFSCaptionObj : GeneralEntity
    {
        public int FSCaptionId { get; set; }
        public string FSCaptionName { get; set; }
        public int? FSCaptionGroupId { get; set; }
        public string FSCaptionGroupName { get; set; }
        public int RatioDetailId { get; set; }
        public short RatioCaptionId { get; set; }
    }
    public class LoanCustomerFSCaptionDetailObj : GeneralEntity
    {
        public int FSDetailId { get; set; }

        public int CustomerId { get; set; }

        public int FSCaptionId { get; set; }

        public DateTime FSDate { get; set; }

        public decimal Amount { get; set; }

        public string FSCaptionName { get; set; }

        public string FSGroupName { get; set; }
    }

    public class LoanCustomerFSRatioDetailObj : GeneralEntity
    {
        public string RatioName { get; set; }

        public int RatioDetailId { get; set; }

        public string Description { get; set; }
    }

    public class LoanCustomerFSRatioCaptionReportObj
    {
        public short RatioCaptionId { get; set; }
        public string RatioCaptionName { get; set; }
        public bool Annualised { get; set; }
        public int Position { get; set; }
        public DateTime FsDate1 { get; set; }
        public DateTime FsDate2 { get; set; }
        public DateTime FsDate3 { get; set; }
        public DateTime FsDate4 { get; set; }
        public string RatioValue1 { get; set; }
        public string RatioValue2 { get; set; }
        public string RatioValue3 { get; set; }
        public string RatioValue4 { get; set; }
        public string FsGroupCaption { get; set; }
    }
    public class LoanCustomerFSRatioCaptionObj
    {
        public int RatioCaptionId { get; set; }
        public string RatioCaptionName { get; set; }

    }
    public class FSQueryObj
    {
        public int CustomerId { get; set; }
        public short FsCaptionGroupId { get; set; }
        public DateTime FsDate { get; set; }
    }

    public class LoanCustomerFSRatiosCalculationObj
    {
        public int RatioDetailId { get; set; }
        public string RatioName { get; set; }
        public DateTime FsDate1 { get; set; }
        public DateTime FsDate2 { get; set; }
        public DateTime FsDate3 { get; set; }
        public DateTime FsDate4 { get; set; }
        public string Ratio1 { get; set; }
        public string Ratio2 { get; set; }
        public string Ratio3 { get; set; }
        public string Ratio4 { get; set; }
    }

    public class DeleteCommand 
    {
        public List<int> Ids { get; set; }
    }
}
