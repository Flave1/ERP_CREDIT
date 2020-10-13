namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_transactioncorrectionsetup
    {
        [Key]
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; }

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
