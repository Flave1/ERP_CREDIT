using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerIdentificationObj
    {
        public int CustomerIdentityId { get; set; }

        public int CustomerId { get; set; }

        public int MeansOfID { get; set; }

        public string Identification { get; set; }

        public string IDNumber { get; set; }

        public DateTime? DateIssued { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_accountopening deposit_accountopening { get; set; }
    }

    public class AddUpdateCustomerIdentificationObj
    {
        public int CustomerIdentityId { get; set; }

        public int CustomerId { get; set; }

        public int MeansOfID { get; set; }

        [StringLength(50)]
        public string IDNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateIssued { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
    }

    public class CustomerIdentificationRegRespObj
    {
        public int CustomerIdentityId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerIdentificationRespObj
    {
        public List<CustomerIdentificationObj> CustomerIdentifications { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

