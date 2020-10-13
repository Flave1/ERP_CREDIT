using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customerloanpd
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CreditRiskAttribute { get; set; }

        public int? CustomerId { get; set; }

        public int? LoanApplicationId { get; set; }

        [StringLength(250)]
        public string AttributeField { get; set; }

        public double? Score { get; set; }

        public double? Coefficients { get; set; }

        public double? AverageCoefficients { get; set; }

        public int? Year { get; set; }

        public DateTime? Date { get; set; }
    }
}
