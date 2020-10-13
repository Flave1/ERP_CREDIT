using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LookUpViewObjs
    {
        public class LookupObj : GeneralEntity
        {
            public int LookupId { get; set; }
            public string LookupName { get; set; }
            public int LookupTypeId { get; set; }
            public string LookupTypeName { get; set; }
        }

        public class LookupRespObj
        {
            public IEnumerable<LookupObj> LookUp { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
