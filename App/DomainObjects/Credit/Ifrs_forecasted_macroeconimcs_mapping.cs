using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class Ifrs_forecasted_macroeconimcs_mapping
    {
        [Key]
        public int ForecastedMacroEconomicMappingId { get; set; }

        public int? Year { get; set; }

        public int? Position { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        public int? Type { get; set; }

        [StringLength(50)]
        public string Variable { get; set; }

        public double? value { get; set; }

        public DateTime? Rundate { get; set; }

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
