using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customerloanpd_history_final
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(250)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(250)]
        public string ProductCode { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public decimal? PD { get; set; }

        public DateTime? Date { get; set; }

        public int? Year { get; set; }

        public decimal? Variable1 { get; set; }

        public decimal? Variable2 { get; set; }

        public decimal? Variable3 { get; set; }

        public decimal? Variable4 { get; set; }

        public decimal? Variable5 { get; set; }

        public decimal? Variable6 { get; set; }

        public decimal? Variable7 { get; set; }
    }
}
