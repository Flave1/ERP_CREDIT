using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerSignatureObj
    {
        public int SignatureId { get; set; }

        public int CustomerId { get; set; }

        public int SignatoryId { get; set; }

        public string Name { get; set; }

        public string DocumentName { get; set; }

        public byte[] SignatureImg { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        /*public virtual deposit_accountopening deposit_accountopening { get; set; }

        public virtual deposit_customersignatory deposit_customersignatory { get; set; }*/
    }

    public class AddUpdateCustomerSignatureObj
    {
        public int SignatureId { get; set; }

        public int CustomerId { get; set; }

        public int SignatoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public byte[] SignatureImg { get; set; }
    }

    public class CustomerSignatureRegRespObj
    {
        public int SignatureId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerSignatureRespObj
    {
        public List<CustomerSignatureObj> CustomerSignatures { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

