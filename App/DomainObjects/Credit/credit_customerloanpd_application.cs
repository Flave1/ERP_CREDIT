using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customerloanpd_application
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CreditRiskAttribute { get; set; }

        public int? CustomerId { get; set; }
        public int? LoanApplicationId_ { get; set; }

        public int? ProductId { get; set; }

        [StringLength(250)]
        public string AttributeField { get; set; }

        public double? Score { get; set; }

        public double? Coefficients { get; set; }

        public double? AverageCoefficients { get; set; }

        public double? PD { get; set; }
        public DateTime? Date { get; set; }

        public int? Year { get; set; }
    }
}
