using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public class inf_investorfund_topup_website
    {
        [Key]
        public int InvestorFundIdWebsiteTopupId { get; set; }
        public int InvestorFundId { get; set; }
        public decimal? TopUpAmount { get; set; }
        public int? ApprovalStatus { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
