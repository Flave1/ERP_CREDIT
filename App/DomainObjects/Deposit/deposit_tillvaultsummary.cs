namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultsummary
    {
        [Key]
        public int TillVaultSummaryId { get; set; }

        public int? TillVaultId { get; set; }

        public int? TransactionCount { get; set; }

        public decimal? TotalAmountCurrency { get; set; }

        public decimal? TransferAmount { get; set; }

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
