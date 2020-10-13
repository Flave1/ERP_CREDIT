using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_collateraltype
    {

        [Key]
        public int CollateralTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Details { get; set; }

        public bool? RequireInsurancePolicy { get; set; }

        public int? ValuationCycle { get; set; }

        public int? HairCut { get; set; }

        public bool? AllowSharing { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<credit_collateralcustomer> credit_collateralcustomer { get; set; }
    }
}
