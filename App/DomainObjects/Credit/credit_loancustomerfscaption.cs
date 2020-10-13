using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancustomerfscaption
    {

        [Key]
        public int FSCaptionId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FSCaptionName { get; set; }

        public int? FSCaptionGroupId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loancustomerfscaptiongroup credit_loancustomerfscaptiongroup { get; set; }
        public virtual ICollection<credit_loancustomerfscaptiondetail> credit_loancustomerfscaptiondetail { get; set; }
    }
}
