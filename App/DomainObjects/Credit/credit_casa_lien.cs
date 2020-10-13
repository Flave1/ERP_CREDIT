using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_casa_lien
    {
        [Key]
        public int LienId { get; set; }

        [Required]
        [StringLength(50)]
        public string SourceReferenceNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductAccountNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string LienReferenceNumber { get; set; }

        public int CompanyId { get; set; }

        public decimal LienCreditAmount { get; set; }
        public decimal LienDebitAmount { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int LienTypeId { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }
    }
}
