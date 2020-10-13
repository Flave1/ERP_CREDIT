using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditratingpd
    {
        [Key]
        public int CreditRiskRatingPDId { get; set; }

        public decimal PD { get; set; }

        public decimal MinRangeScore { get; set; }

        public decimal MaxRangeScore { get; set; }

        public int? ProductId { get; set; }

        public decimal? InterestRate { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

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
