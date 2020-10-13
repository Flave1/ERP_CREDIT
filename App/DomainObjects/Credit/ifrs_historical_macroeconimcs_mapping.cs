using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_historical_macroeconimcs_mapping
    {
        [Key]
        public int HistoricalMacroEconomicMappingId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(150)]
        public string Variable { get; set; }

        public int? Position { get; set; }

        public int? Type { get; set; }

        public double? Value { get; set; }

        public int? Year { get; set; }

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
