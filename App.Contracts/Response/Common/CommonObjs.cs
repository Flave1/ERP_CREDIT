using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Common
{
    public partial class CommonObj
    { 
        public long LookupId { get; set; }
         
        public long ParentId { get; set; }
         
        public string LookupName { get; set; }
         
        public string Code { get; set; }
         
        public object ParentName { get; set; }
         
        public object Description { get; set; }
         
        public object SkillDescription { get; set; }
         
        public object Skills { get; set; }
 
        public long SellingRate { get; set; }
         
        public long BuyingRate { get; set; }
         
        public bool BaseCurrency { get; set; }
         
        public DateTime Date { get; set; }
         
        public long CorporateChargeAmount { get; set; }
         
        public long IndividualChargeAmount { get; set; }
         
        public long GlAccountId { get; set; }
         
        public bool IsMandatory { get; set; }
    }

    public class CommonRespObj
    {
        public List<CommonObj> commonLookups { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SubGLObj
    {
        public int SubGLId { get; set; }
        public string SubGLCode { get; set; }
        public string SubGLName { get; set; }
    }

    public class SubGLRespObj
    {
        public List<SubGLObj> subGls { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
