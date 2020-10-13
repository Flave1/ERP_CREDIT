using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects
{
    public partial class deposit_bankclosure
    {
        [Key]
        public int BankClosureId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public bool? Status { get; set; }

        [StringLength(50)]
        public string AccountBalance { get; set; }

        public int? Currency { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ClosingDate { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        public decimal? Charges { get; set; }

        [StringLength(50)]
        public string FinalSettlement { get; set; }

        [StringLength(50)]
        public string Beneficiary { get; set; }

        public bool? ModeOfSettlement { get; set; }

        [StringLength(50)]
        public string TransferAccount { get; set; }

        [StringLength(50)]
        public string ApproverName { get; set; }

        [StringLength(50)]
        public string ApproverComment { get; set; }

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
