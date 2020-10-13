using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditRiskRatingObjs
    {
        public class CreditRiskRatingObj : GeneralEntity
        {
            public int CreditRiskRatingId { get; set; }

            public string Rate { get; set; }

            public decimal MinRange { get; set; }

            public decimal MaxRange { get; set; }

            public decimal? AdvicedRange { get; set; }

            public string RateDescription { get; set; }

        }

        public class CreditRiskRatingPDObj : GeneralEntity
        {
            public int CreditRiskRatingPDId { get; set; }

            public int? ProductId { get; set; }

            public decimal Pd { get; set; }

            public decimal MinRange { get; set; }

            public decimal MaxRange { get; set; }

            public decimal? InterestRate { get; set; }

            public string Description { get; set; }

            public string ProductName { get; set; }

        }

        public class GroupedCreditRiskRatingPDObj
        {
            public GroupedCreditRiskRatingPDObj()
            {
                children = new List<ChildrenObj>();
            }
            public CreditRiskRatingPDObj data { get; set; }

            public List<ChildrenObj> children { get; set; }
        }

        public class ChildrenObj
        {
            public CreditRiskRatingPDObj data { get; set; }
        }

        public class CreditRiskRatingDetailObj
        {
            public decimal? CreditScore { get; set; }
            public decimal? ProbabilityOfDefault { get; set; }
            public decimal? ProductWeighterScore { get; set; }
            public string CreditRating { get; set; }
        }

        public class CreditRiskRatingObjRespObj
        {
            public IEnumerable<CreditRiskRatingObj> CreditRiskRating { get; set; }
            public CreditRiskRatingDetailObj CreditRiskRatingDetails { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CreditRiskRatingPDObjRespObj
        {
            public IEnumerable<CreditRiskRatingPDObj> CreditRiskRatingPD { get; set; }
            public IEnumerable<GroupedCreditRiskRatingPDObj> GroupedCreditRiskRatingPD { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
