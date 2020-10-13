using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_forecasted_pd
    {
        [Key]
        public int ForeCastedId { get; set; }

        public int? Year { get; set; }

        public decimal? PD1 { get; set; }

        public decimal? PD2 { get; set; }

        public decimal? PD3 { get; set; }

        public decimal? PD4 { get; set; }

        public decimal? PD5 { get; set; }

        public decimal? PD6 { get; set; }

        public decimal? PD7 { get; set; }

        public decimal? LifeTimePD { get; set; }

        [StringLength(50)]
        public string PDType { get; set; }

        [StringLength(50)]
        public string Stage { get; set; }

        public decimal? ApplicablePD { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(50)]
        public string CompanyCode { get; set; }

        public DateTime? RunDate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
