using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public partial class inf_investmentperiodicschedule
    {
        [Key]
        public int InvestmentPeriodicScheduleId { get; set; }

        public int? Period { get; set; }

        public decimal? OB { get; set; }

        public decimal? InterestAmount { get; set; }

        public decimal? CB { get; set; }

        public DateTime? PeriodDate { get; set; }

        public decimal? Repayment { get; set; }

        public int? InvestorFundId { get; set; }

        public int? FrequencyType { get; set; }
    }
}
