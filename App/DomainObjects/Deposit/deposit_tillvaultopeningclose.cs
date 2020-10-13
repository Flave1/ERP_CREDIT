namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultopeningclose
    {
        [Key]
        public int TillVaultOpeningCloseId { get; set; }

        public DateTime? Date { get; set; }

        public int? Currency { get; set; }

        public decimal? AmountPerSystem { get; set; }

        public decimal? CashAvailable { get; set; }

        public decimal? Shortage { get; set; }

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
