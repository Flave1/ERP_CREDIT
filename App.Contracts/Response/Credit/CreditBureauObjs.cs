using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditBureauObjs
    {
        public class CreditBureauObj : GeneralEntity
        {
            public int CreditBureauId { get; set; }

            public string CreditBureauName { get; set; }

            public decimal CorporateChargeAmount { get; set; }

            public decimal IndividualChargeAmount { get; set; }

            public int GLAccountId { get; set; }

            public bool IsMandatory { get; set; }

        }

        public class LoanCreditBureauObj : GeneralEntity
        {
            public int LoanCreditBureauId { get; set; }

            public int LoanApplicationId { get; set; }

            public string LoanApplicationRefNo { get; set; }

            public int CreditBureauId { get; set; }

            public string CreditBureauName { get; set; }

            public decimal? ChargeAmount { get; set; }

            public bool? ReportStatus { get; set; }

            public byte[] SupportDocument { get; set; }

        }

        public class CreditBureauObjRespObj
        {
            public IEnumerable<CreditBureauObj> CreditBureau { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanCreditBureauObjRespObj
        {
            public IEnumerable<LoanCreditBureauObj> LoanCreditBureau { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
