using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loantransactiontype
    {
        [Key]
        public byte LoanTransactionTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanTransactionTypeName { get; set; }
    }
}
