using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanoperation
    {
        [Key]
        public byte LoanOperationId { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanOperationName { get; set; }
    }
}
