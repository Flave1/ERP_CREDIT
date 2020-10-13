using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_customercarddetails
    {
        [Key]
        public int CustomerCardDetailsId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(250)]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Cvv { get; set; }

        [Required]
        [StringLength(550)]
        public string ExpiryMonth { get; set; }

        [Required]
        [StringLength(550)]
        public string ExpiryYear { get; set; }

        [Required]
        public string currencyCode { get; set; }
        public string IssuingBank { get; set; }

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
