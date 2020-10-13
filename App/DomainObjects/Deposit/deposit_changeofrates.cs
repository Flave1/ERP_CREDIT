namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_changeofrates
    {
        [Key]
        public int ChangeOfRateId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public decimal? CurrentRate { get; set; }

        public decimal? ProposedRate { get; set; }

        [StringLength(500)]
        public string Reasons { get; set; }

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
