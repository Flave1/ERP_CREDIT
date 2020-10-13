using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loan_cheque
    {
        [Key]
        public int LoanChequeId { get; set; }
        public int LoanId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string ChequeNo { get; set; }
        public byte[] GeneralUpload { get; set; }
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
