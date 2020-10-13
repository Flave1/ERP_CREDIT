using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_directorshareholder
    {

        [Key]
        public int DirectorShareHolderId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(250)]
        public string CompanyName { get; set; }

        public double PercentageHolder { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }
    }
}
