namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_transactioncorrectionform
    {
        [Key]
        public int TransactionCorrectionId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        public DateTime? QueryStartDate { get; set; }

        public DateTime? QueryEndDate { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; }

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
