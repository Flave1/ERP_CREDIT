using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Mail
{
    public class MailObj
    {
        public string subject { get; set; }
        public string content { get; set; }
        public List<ToAddress> toAddresses { get; set; }
        public List<FromAddress> fromAddresses { get; set; }
        public bool sendIt { get; set; }
        public bool saveIt { get; set; }
        public int template { get; set; }
        public string callBackUri { get; set; }
    }

    public class ToAddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class FromAddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class MailRespObj
    {
        public int ResponseStatus { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
