namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_selectedTransactioncharge
    {
        [Key]
        public int SelectedTransChargeId { get; set; }

        public int? TransactionChargeId { get; set; }

        public int? AccountId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_transactioncharge deposit_transactioncharge { get; set; }
    }
}
