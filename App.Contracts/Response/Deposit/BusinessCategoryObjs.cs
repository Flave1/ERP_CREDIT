using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class BusinessCategoryObj
    {
        public int BusinessCategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class AddUpdateBusinessCategoryObj
    {
        public int BusinessCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }

    public class BusinessCategoryRegRespObj
    {
        public int BusinessCategoryId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class BusinessCategoryRespObj
    {
        public List<BusinessCategoryObj> BusinessCategories { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SearchObj
    {
        public int SearchId { get; set; }
        public int CustomerId { get; set; }
        public string SearchWord { get; set; }
        public DateTime SearchDate { get; set; }
    }

    public class DeleteRequest
    {
        public List<int> ItemIds { get; set; }
    }

    public class DeleteRespObjt
    {
        public bool Deleted { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}




