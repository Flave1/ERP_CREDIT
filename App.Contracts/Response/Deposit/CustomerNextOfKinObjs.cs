using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerNextOfKinObj
    {
        public int NextOfKinId { get; set; }

        public int CustomerId { get; set; }

        public string Title { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string OtherName { get; set; }

        public DateTime? DOB { get; set; }

        public int? GenderId { get; set; }

        public string Relationship { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_accountopening deposit_accountopening { get; set; }
    }

    public class AddUpdateCustomerNextOfKinObj
    {
        public int NextOfKinId { get; set; }

        public int CustomerId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string OtherName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        public int? GenderId { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        [StringLength(50)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }
    }

    public class CustomerNextOfKinRegRespObj
    {
        public int NextOfKinId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerNextOfKinRespObj
    {
        public List<CustomerNextOfKinObj> customerNextOfKins { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

