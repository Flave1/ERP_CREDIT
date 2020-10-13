namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_accountsetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepositAccountId { get; set; }

        [StringLength(500)]
        public string AccountName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int AccountTypeId { get; set; }

        public int? CurrencyId { get; set; }

        [StringLength(50)]
        public string DormancyDays { get; set; }

        public decimal InitialDeposit { get; set; }

        public int CategoryId { get; set; }

        public int? BusinessCategoryId { get; set; }

        public int? GLMapping { get; set; }

        public int? BankGl { get; set; }

        public decimal? InterestRate { get; set; }

        [StringLength(50)]
        public string InterestType { get; set; }

        public bool? CheckCollecting { get; set; }

        [StringLength(50)]
        public string MaturityType { get; set; }

        [NotMapped]
        public int[] ApplicableTaxId { get; set; }

        [NotMapped]
        public int[] ApplicableChargesId { get; set; }

        public bool? PreTerminationLiquidationCharge { get; set; }

        public int? InterestAccrual { get; set; }

        public bool? Status { get; set; }

        public bool? OperatedByAnother { get; set; }

        public bool? CanNominateBenefactor { get; set; }

        public bool? UsePresetChartofAccount { get; set; }

        [StringLength(50)]
        public string TransactionPrefix { get; set; }

        [StringLength(50)]
        public string CancelPrefix { get; set; }

        [StringLength(50)]
        public string RefundPrefix { get; set; }

        public bool? Useworkflow { get; set; }

        public bool? CanPlaceOnLien { get; set; }

        public bool? InUse { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual deposit_accountype deposit_accountype { get; set; }

        public virtual deposit_category deposit_category { get; set; }

        public virtual deposit_transactioncharge deposit_transactioncharge { get; set; }

        public virtual deposit_transactiontax deposit_transactiontax { get; set; }
    }
}
