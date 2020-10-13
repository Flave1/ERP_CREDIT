using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditRiskScoreCardObjs
    {
        public class CreditRiskScoreCardObj : GeneralEntity
        {
            public int CreditRiskScoreCardId { get; set; }
            public int CreditRiskAttributeId { get; set; }
            public int CustomerTypeId { get; set; }
            public string CreditRiskAttributeName { get; set; }
            public string CustomerTypeName { get; set; }
            public string Value { get; set; }
            public decimal Score { get; set; }
        }

        public class GroupedCreditRiskAttibuteObj
        {
            public GroupedCreditRiskAttibuteObj()
            {
                children = new List<ChildrenDataObj>();
            }
            public CreditRiskScoreCardObj data { get; set; }

            public List<ChildrenDataObj> children { get; set; }
        }

        public class ChildrenDataObj
        {
            public CreditRiskScoreCardObj data { get; set; }
        }

        public class CreditRiskScoreCardObjRespObj
        {
            public IEnumerable<CreditRiskScoreCardObj> CreditRiskScoreCard { get; set; }
            public int CreditRiskScoreCardId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class GroupedCreditRiskAttibuteObjRespObj
        {
            public IEnumerable<GroupedCreditRiskAttibuteObj> GroupedCreditRiskAttibute { get; set; }
            public APIResponseStatus Status { get; set; }
        }


        public class CreditRiskScoreCardSearchObj
        {
            public int CreditRiskScoreCardId { get; set; }
            public int CreditRiskAttributeId { get; set; }
            public int CreditRiskCategoryId { get; set; }
            public int CreditRiskRatingId { get; set; }
            public int LoanApplicationId { get; set; }
            public int WeightedRiskScoreId { get; set; }
            public int ProductId { get; set; }
            public int CustomerTypeId { get; set; }
            public int CreditBureauId { get; set; }
            public int LoanCreditBureauId { get; set; }
            public int CreditRiskRatingPDId { get; set; }
        }

        public class DeleteCreditRiskScoreCommand
        {
            public List<int> Ids { get; set; }
            public List<string> TargetIds { get; set; }
        }

        public class LoanApplicationScoreCardObj : GeneralEntity
        {
            public LoanApplicationScoreCardObj()
            {
                AttributeList = new List<AttributeList>();
            }
            public int ApplicationScoreCardId { get; set; }
            public int LoanApplicationId { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public string Field1 { get; set; }
            public string Field2 { get; set; }
            public string Field3 { get; set; }
            public string Field4 { get; set; }
            public string Field5 { get; set; }
            public string Field6 { get; set; }
            public string Field7 { get; set; }
            public string Field8 { get; set; }
            public string Field9 { get; set; }
            public string Field10 { get; set; }
            public string Field11 { get; set; }
            public string Field12 { get; set; }
            public string Field13 { get; set; }
            public string Field14 { get; set; }
            public string Field15 { get; set; }
            public string Field16 { get; set; }
            public string Field17 { get; set; }
            public string Field18 { get; set; }
            public string Field19 { get; set; }
            public string Field20 { get; set; }
            public string Field21 { get; set; }
            public string Field22 { get; set; }
            public string Field23 { get; set; }
            public string Field24 { get; set; }
            public string Field25 { get; set; }
            public string Field26 { get; set; }
            public string Field27 { get; set; }
            public string Field28 { get; set; }
            public string Field29 { get; set; }
            public string Field30 { get; set; }
            public List<AttributeList> AttributeList { get; set; }

        }
        public class AttributeList
        {
            public string AttributeField { get; set; }
            public int Score { get; set; }
        }
    }



}
