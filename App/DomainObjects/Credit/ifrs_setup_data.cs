using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_setup_data
    {
        [Key]
        public int SetUpId { get; set; }

        public double? Threshold { get; set; }

        public int? Deteroriation_Level { get; set; }

        public int? Classification_Type { get; set; }

        public int? Historical_PD_Year_Count { get; set; }

        public bool? PDBasis { get; set; }

        public int? Ltpdapproach { get; set; }

        public double? CCF { get; set; }

        [StringLength(50)]
        public string GroupBased { get; set; }

        public DateTime? RunDate { get; set; }

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
