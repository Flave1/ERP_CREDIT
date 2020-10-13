namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_depositform
    {
        [Key]
        public int DepositFormId { get; set; }

        public int Structure { get; set; }

        public int? Operation { get; set; }

        public int? TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValueDate { get; set; }

        public DateTime TransactionDate { get; set; }

        [StringLength(50)]
        public string TransactionDescription { get; set; }

        [StringLength(50)]
        public string TransactionParticulars { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string ModeOfTransaction { get; set; }

        [StringLength(50)]
        public string InstrumentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InstrumentDate { get; set; }

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
