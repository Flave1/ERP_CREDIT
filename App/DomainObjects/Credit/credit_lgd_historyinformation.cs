using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_lgd_historyinformation
    {
        [Key]
        public int HistoricalLGDId { get; set; }

        public decimal LoanAmount { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(250)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(250)]
        public string ProductCode { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public double? CR { get; set; }

        public double? EIR { get; set; }

        [StringLength(50)]
        public string Frequency { get; set; }
        public decimal? Field1 { get; set; }

        public decimal? Field2 { get; set; }

        public decimal? Field3 { get; set; }

        public decimal? Field4 { get; set; }

        public decimal? Field5 { get; set; }
        public decimal? Field6 { get; set; }
        public decimal? Field7 { get; set; }

        public decimal? Field8 { get; set; }
        public decimal? Field9 { get; set; }

        public decimal? Field10 { get; set; }
        public decimal? Field11 { get; set; }
        public decimal? Field12 { get; set; }

        public decimal? Field13 { get; set; }

        public decimal? Field14 { get; set; }

        public decimal? Field15 { get; set; }

        public decimal? Field16 { get; set; }

        public decimal? Field17 { get; set; }

        public decimal? Field18 { get; set; }

        public decimal? Field19 { get; set; }

        public decimal? Field20 { get; set; }

        public decimal? Field21 { get; set; }

        public decimal? Field22 { get; set; }

        public decimal? Field23 { get; set; }
        public decimal? Field24 { get; set; }

        public decimal? Field25 { get; set; }

        public decimal? Field26 { get; set; }

        public decimal? Field27 { get; set; }

        public decimal? Field28 { get; set; }
        public decimal? Field29 { get; set; }

        public decimal? Field30 { get; set; }

        public decimal? Field31 { get; set; }

        public decimal? Field32 { get; set; }

        public decimal? Field33 { get; set; }

        public decimal? Field34 { get; set; }
        public decimal? Field35 { get; set; }

        public decimal? Field36 { get; set; }

        public decimal? Field37 { get; set; }

        public decimal? Field38 { get; set; }

        public decimal? Field39 { get; set; }

        public decimal? Field40 { get; set; }
        public decimal? Field41 { get; set; }

        public decimal? Field42 { get; set; }

        public decimal? Field43 { get; set; }

        public decimal? Field44 { get; set; }

        public decimal? Field45 { get; set; }

        public decimal? Field46 { get; set; }

        public decimal? Field47 { get; set; }
        public decimal? Field48 { get; set; }

        public decimal? Field49 { get; set; }

        public decimal? Field50 { get; set; }

        public decimal? Field51 { get; set; }

        public decimal? Field52 { get; set; }
        public decimal? Field53 { get; set; }

        public decimal? Field54 { get; set; }

        public decimal? Field55 { get; set; }

        public decimal? Field56 { get; set; }

        public decimal? Field57 { get; set; }

        public decimal? Field58 { get; set; }

        public decimal? Field59 { get; set; }

        public decimal? Field60 { get; set; }

        public decimal? Field61 { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
