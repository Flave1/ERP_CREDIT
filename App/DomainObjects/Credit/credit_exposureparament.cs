using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_exposureparament
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExposureParameterId { get; set; }

        public int? CustomerTypeId { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public decimal? Percentage { get; set; }

        public decimal? ShareHolderAmount { get; set; }

        public decimal? Amount { get; set; }

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
