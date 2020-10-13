using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditriskscorecard
    {
        [Key]
        public int CreditRiskScoreCardId { get; set; }

        public int CreditRiskAttributeId { get; set; }

        public int CustomerTypeId { get; set; }

        [Required]
        [StringLength(250)]
        public string Value { get; set; }

        public decimal Score { get; set; }

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
