using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class DeleteRespObj
    {
        public bool Deleted { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
