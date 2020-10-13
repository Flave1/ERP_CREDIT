using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_collateralcustomer
    {

        [Key]
        public int CollateralCustomerId { get; set; }

        public int CustomerId { get; set; }

        public int CollateralTypeId { get; set; }

        public int CurrencyId { get; set; }

        public decimal CollateralValue { get; set; }

        [Required]
        [StringLength(250)]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        public string CollateralCode { get; set; }

        public bool? CollateralVerificationStatus { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual cor_currency cor_currency { get; set; }

        public virtual credit_collateraltype credit_collateraltype { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }

        public virtual ICollection<credit_loanapplicationcollateral> credit_loanapplicationcollateral { get; set; }
    }
}
