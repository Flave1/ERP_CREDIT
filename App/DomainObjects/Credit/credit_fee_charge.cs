using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_fee_charge
    {
        [Key]
        public int fee_charge_id { get; set; }

        public int? fee_id { get; set; }

        [StringLength(50)]
        public string feename { get; set; }

        public decimal? feecharge { get; set; }

        public int loanreviewId { get; set; }
    }
}
