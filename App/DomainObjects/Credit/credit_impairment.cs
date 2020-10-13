using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_impairment
    {
        [Key]
        public int ImpairmentId { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string ECLType { get; set; }

        public decimal C12MonthPD { get; set; }

        public decimal LifeTimePD { get; set; }

        public decimal C12MonthLGD { get; set; }

        public decimal LifeTimeLGD { get; set; }

        public decimal C12MonthEAD { get; set; }

        public decimal LifeTimeEAD { get; set; }

        public decimal? C12MonthECL { get; set; }

        public decimal? LifeTimeECL { get; set; }
        public DateTime Date { get; set; }
    }
}
