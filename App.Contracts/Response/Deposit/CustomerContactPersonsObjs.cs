using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerContactPersonsObj
    {
        public int ContactPersonId { get; set; }

        public int CustomerId { get; set; }

        public string Title { get; set; }

        public string SurName { get; set; }

        public string FirstName { get; set; }

        public string OtherName { get; set; }

        public string Relationship { get; set; }

        public int? GenderId { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_accountopening deposit_accountopening { get; set; }
    }

    public class AddUpdateCustomerContactPersonsObj
    {
        public int ContactPersonId { get; set; }

        public int CustomerId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string SurName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string OtherName { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        public int? GenderId { get; set; }

        [StringLength(50)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Address { get; set; }
    }

    public class CustomerContactPersonsRegRespObj
    {
        public int ContactPersonId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerContactPersonsRespObj
    {
        public List<CustomerContactPersonsObj> ContactPersons { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

