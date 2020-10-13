using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditrating
    {
        [Key]
        public int CreditRiskRatingId { get; set; }

        [Required]
        [StringLength(50)]
        public string Rate { get; set; }

        public decimal MinRange { get; set; }

        public decimal MaxRange { get; set; }

        public decimal? AdvicedRange { get; set; }

        [StringLength(1000)]
        public string RateDescription { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
