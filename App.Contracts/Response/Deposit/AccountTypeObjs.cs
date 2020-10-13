using GOSLibraries.GOS_API_Response; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class AccountTypeObj
    { 
        public int AccountTypeId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public bool? Active { get; set; } 
        public bool? Deleted { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; } 
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateAccountTypeObj
    {
        public int AccountTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }

    public class AccountTypeRegRespObj
    {
        public int AccountTypeId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountTypeRespObj
    {
        public List<AccountTypeObj> AccountTypes { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountTypeSearchObj
    {
        public int AccountTypeId { get; set; }
        public string SearchWord { get; set; }
    }
}



