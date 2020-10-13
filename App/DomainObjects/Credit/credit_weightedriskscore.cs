using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_weightedriskscore
    {
        [Key]
        public int WeightedRiskScoreId { get; set; }

        public int? CreditRiskAttributeId { get; set; }

        [StringLength(50)]
        public string FeildName { get; set; }

        public bool? UseAtOrigination { get; set; }

        public int? ProductId { get; set; }

        public int? CustomerTypeId { get; set; }

        public decimal? ProductMaxWeight { get; set; }

        public decimal? WeightedScore { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_creditriskattribute credit_creditriskattribute { get; set; }

        public virtual credit_product credit_product { get; set; }
    }
}
