using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public class inf_investorfund_rollover_website
    {
        [Key]
        public int InvestorFundIdWebsiteRolloverId { get; set; }
        public int InvestorFundId { get; set; }

        public decimal? ApprovedTenor { get; set; }

        public decimal? RollOverAmount { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? ApprovalStatus { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
