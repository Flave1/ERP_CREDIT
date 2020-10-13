using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditbureau
    {

        [Key]
        public int CreditBureauId { get; set; }

        [Required]
        [StringLength(300)]
        public string CreditBureauName { get; set; }

        public decimal CorporateChargeAmount { get; set; }

        public decimal IndividualChargeAmount { get; set; }

        public int GLAccountId { get; set; }

        public bool IsMandatory { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<credit_loancreditbureau> credit_loancreditbureau { get; set; }
    }
}
