using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public partial class inf_investdailyschedule
    {
        [Key]
        public int InvestmentDailyScheduleId { get; set; }

        public int? Period { get; set; }

        public decimal? OB { get; set; }

        public decimal? InterestAmount { get; set; }

        public decimal? CB { get; set; }

        public decimal? Repayment { get; set; }

        public DateTime? PeriodDate { get; set; }

        public int? InvestorFundId { get; set; }

        public int? PeriodId { get; set; }

        public bool? EndPeriod { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Updatedon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Createdon { get; set; }
    }
}
