using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customeridentitydetails
    {
        [Key]
        public int CustomerIdentityDetailsId { get; set; }

        public int CustomerId { get; set; }

        public int IdentificationId { get; set; }

        [Required]
        [StringLength(250)]
        public string Number { get; set; }

        [Required]
        [StringLength(50)]
        public string Issuer { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual cor_identification cor_identification { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }
    }
}
