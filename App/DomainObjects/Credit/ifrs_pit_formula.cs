using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_pit_formula
    {
        [Key]
        public int PitFormularId { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        public string Equation { get; set; }

        public double? ComputedPd { get; set; }

        public int? Type { get; set; }

        public DateTime? Rundate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        public DateTime? Updatedon { get; set; }

        public DateTime? Createdon { get; set; }
    }
}
