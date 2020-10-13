namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_transferform
    {
        [Key]
        public int TransferFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValueDate { get; set; }

        [StringLength(50)]
        public string ExternalReference { get; set; }

        [StringLength(50)]
        public string TransactionReference { get; set; }

        [StringLength(50)]
        public string PayingAccountNumber { get; set; }

        [StringLength(50)]
        public string PayingAccountName { get; set; }

        [StringLength(50)]
        public string PayingAccountCurrency { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountNumber { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountName { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountCurrency { get; set; }

        [StringLength(50)]
        public string TransactionNarration { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; }

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
