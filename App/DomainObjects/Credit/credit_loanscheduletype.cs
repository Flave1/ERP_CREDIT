using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Banking.DomainObjects.Credit
{
    public class credit_loanscheduletype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoanScheduleTypeId { get; set; }

        [Required]
        [StringLength(250)]
        public string LoanScheduleTypeName { get; set; }

        public int LoanScheduleCategoryId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loanschedulecategory credit_loanschedulecategory { get; set; }
    }
}
