using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public partial class inf_liquidationrecommendationLog
    {
        [Key]
        public int LiquidationRecommendationLogId { get; set; }

        public int InvInvestorFundId { get; set; }

        public int ApprovedProductId { get; set; }

        public int ApprovedTenor { get; set; }

        public decimal ApprovedRate { get; set; }

        [Column(TypeName = "money")]
        public decimal ApprovedAmount { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
