using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditWeightedRiskScoreCardObjs
    {
        public class CreditWeightedRiskScoreObj : GeneralEntity
        {
            public int WeightedRiskScoreId { get; set; }

            public int? CreditRiskAttributeId { get; set; }

            public string AttributeName { get; set; }

            public int? ProductId { get; set; }

            public string ProductName { get; set; }

            public int? CustomerTypeId { get; set; }

            public string CustomerTypeName { get; set; }

            public decimal? ProductMaxWeight { get; set; }

            public decimal? WeightedScore { get; set; }

            public bool? UseAtOrigination { get; set; }

        }

        public class WeightedScoreTreeNode
        {
            public WeightedScoreTreeNode()
            {
                children = new List<CreditWeightedRiskScoreObj>();
            }
            public CreditWeightedRiskScoreObj data { get; set; }
            public List<CreditWeightedRiskScoreObj> children { get; set; }
        }

        public class CreditWeightedRiskScoreObjRespObj
        {
            public IEnumerable<CreditWeightedRiskScoreObj> CreditWeightedRiskScore { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
