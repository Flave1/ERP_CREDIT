using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanapplicationcollateraldocument
    {
        [Key]
        public int LoanApplicationCollateralDocumentId { get; set; }

        public int? LoanApplicationId { get; set; }

        public int? CollateralTypeId { get; set; }

        public byte[] Document { get; set; }

        [StringLength(256)]
        public string DocumentName { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CollateralCustomerId { get; set; }
    }
}
