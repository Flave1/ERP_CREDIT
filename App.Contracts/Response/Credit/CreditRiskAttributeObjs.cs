using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditRiskAttributeObjs
    {
        public class CreditRiskAttibuteObj : GeneralEntity
        {
            public int CreditRiskAttributeId { get; set; }

            public string CreditRiskAttribute { get; set; }

            public int CreditRiskCategoryId { get; set; }

            public string CreditRiskCategoryName { get; set; }


            public string AttributeField { get; set; }

            public string SystemAttribute { get; set; }
        }

        public class MappedCreditRiskAttibuteObj
        {
            public int CreditRiskAttributeId { get; set; }

            public string CreditRiskAttribute { get; set; }

            public int CustomerTypeId { get; set; }

        }

        public class SystemAttributeObj
        {
            public int SystemAttributeId { get; set; }

            public string SystemAttributeName { get; set; }

        }

        public class ApplicationCreditRiskAttibuteObj
        {
            public ApplicationCreditRiskAttibuteObj()
            {
                Options = new List<OptionLists>();
            }
            public string FieldName { get; set; }
            public int FieldId { get; set; }
            public int? FieldValue { get; set; }
            public bool ShowField { get; set; }
            public bool DisableField { get; set; }
            public string LabelName { get; set; }
            public string LabelName1 { get; set; }
            public List<OptionLists> Options { get; set; }
        }
        public class OptionLists
        {
            public decimal Score { get; set; }
            public int CreditRiskAttributeId { get; set; }
            public string Value { get; set; }
            public decimal? MaxScore { get; set; }

        }

        public class CreditRiskCategoryObj
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
            public bool UseInOrigination { get; set; }
            public string CreatedBy { get; set; }

        }

        public class CreditRiskAttibuteObjRespObj
        {
            public IEnumerable<CreditRiskAttibuteObj> CreditRiskAttibutes { get; set; }
            public IEnumerable<SystemAttributeObj> SystemAttibutes { get; set; }
            public IEnumerable<MappedCreditRiskAttibuteObj> MappedAttibutes { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CreditRiskCategoryObjRespObj
        {
            public IEnumerable<CreditRiskCategoryObj> CreditRiskCategory { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ApplicationCreditRiskAttibuteObjRespObj
        {
            public IEnumerable<ApplicationCreditRiskAttibuteObj> ApplicationCreditRiskAttibute { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
