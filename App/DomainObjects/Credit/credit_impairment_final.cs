using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_impairment_final
    {
        [Key]
        public int ImpairmentFinalId { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(50)]
        public string ECLType { get; set; }

        [StringLength(50)]
        public string Stage { get; set; }

        [StringLength(50)]
        public string Scenario { get; set; }

        public decimal? Likelihood { get; set; }

        public decimal? Rate { get; set; }

        public decimal PD { get; set; }

        public decimal LGD { get; set; }

        public decimal? EAD { get; set; }

        public decimal? ECL { get; set; }
        public DateTime Date { get; set; }
    }
}
