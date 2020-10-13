using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancustomerfscaptiondetail
    {
        [Key]
        public int FSDetailId { get; set; }

        public int CustomerId { get; set; }

        public int FSCaptionId { get; set; }
        public DateTime FSDate { get; set; }
        public decimal Amount { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }

        public virtual credit_loancustomerfscaption credit_loancustomerfscaption { get; set; }
    }
}
