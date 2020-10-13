using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_regress_macro_variable_lgd
    {
        [Key]
        public int RegressMacroVariableId { get; set; }

        public double? LGD { get; set; }

        public double? Variable1 { get; set; }

        public double? Variable2 { get; set; }

        public double? Variable3 { get; set; }

        public double? Variable4 { get; set; }

        public double? Variable5 { get; set; }

        public double? Variable6 { get; set; }

        public double? Variable7 { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        public int? Year { get; set; }

        public DateTime? Rundate { get; set; }
    }
}
