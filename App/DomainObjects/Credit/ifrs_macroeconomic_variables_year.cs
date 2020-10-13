using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_macroeconomic_variables_year
    {
        [Key]
        public int MacroEconomicVariableId { get; set; }

        public int? Year { get; set; }

        public double? GDP { get; set; }

        public double? Unemployement { get; set; }

        public double? Inflation { get; set; }

        public double? erosion { get; set; }

        public double? ForegnEx { get; set; }

        public double? Others { get; set; }

        public double? otherfactor { get; set; }

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
