using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customerloanpd_history
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CreditRiskAttribute { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(250)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(250)]
        public string ProductCode { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

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
