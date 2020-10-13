using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customerloanscorecard
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CreditRiskAttribute { get; set; }

        public int CustomerId { get; set; }

        public int LoanApplicationId { get; set; }

        [Required]
        [StringLength(250)]
        public string AttributeField { get; set; }

        public decimal Score { get; set; }

        public decimal AttributeWeightedScore { get; set; }

        public decimal AverageWeightedScore { get; set; }

        public DateTime? Date { get; set; }
    }
}
