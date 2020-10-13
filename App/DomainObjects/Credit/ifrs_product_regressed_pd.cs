using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_product_regressed_pd
    {
        [Key]
        public int ProductRegressedPDId { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        public int Year { get; set; }

        public double AnnualPD { get; set; }

        public double? LifeTimePD { get; set; }

        public int? CompanyId { get; set; }

        public DateTime? RunDate { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
