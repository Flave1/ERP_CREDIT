using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_computed_forcasted_pd_lgd
    {
        [Key]
        public int ComputedPDId { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        public int? Year { get; set; }

        public int? Type { get; set; }

        public double? PD_LGD { get; set; }

        public double? PD { get; set; }

        public DateTime? Rundate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
