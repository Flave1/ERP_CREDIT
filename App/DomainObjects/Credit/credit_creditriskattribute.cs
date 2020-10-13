using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditriskattribute
    {
        [Key]
        public int CreditRiskAttributeId { get; set; }

        [Required]
        [StringLength(500)]
        public string CreditRiskAttribute { get; set; }

        public int CreditRiskCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string AttributeField { get; set; }

        [StringLength(500)]
        public string FriendlyName { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual ICollection<credit_autopopulateattributemapping> credit_autopopulateattributemapping { get; set; }

        public virtual ICollection<credit_weightedriskscore> credit_weightedriskscore { get; set; }
    }
}
