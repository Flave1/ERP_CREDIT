using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CreditClassificationObjs
    {
        public class CreditClassificationObj
        {
            public int CreditClassificationId { get; set; }
            public string Description { get; set; }
            public int ProvisioningRequirement { get; set; }
            public int? UpperLimit { get; set; }
            public int? LowerLimit { get; set; }
            public bool? Active { get; set; }
            public bool? Deleted { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }
        }

        public class AddUpdateCreditClassification
        {
            public int CreditClassificationId { get; set; }
            public string Description { get; set; }
            public int ProvisioningRequirement { get; set; }
            public int? UpperLimit { get; set; }
            public int? LowerLimit { get; set; }
        }

        public class CreditClassificationRegRespObj
        {
            public int CreditClassificationId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CreditClassificationRespObj
        {
            public List<CreditClassificationObj> CreditClassification { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CreditClassificationSearchObj
        {
            public int CreditClassificationId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteCreditClassificationCommand : IRequest<DeleteRespObj>
        {
            public List<int> CreditClassificationIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }


        public class OperatingAccountObj : GeneralEntity
        {
            public int OperatingAccountId { get; set; }

            public string OperatingAccountName { get; set; }

            public int? CasaGL { get; set; }

            public int? CashAndBankGL { get; set; }

            public bool? InUse { get; set; }
        }

        public class OperatingAccountRespObj
        {
            public OperatingAccountObj OperatingAccount { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
