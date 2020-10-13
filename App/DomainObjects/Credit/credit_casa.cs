using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_casa
    {
        [Key]
        public int CasaAccountId { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int BranchId { get; set; }

        public int CurrencyId { get; set; }

        public bool IsCurrentAccount { get; set; }

        public int? Tenor { get; set; }

        public decimal? InterestRate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? TerminalDate { get; set; }

        public int? ActionBy { get; set; }

        public DateTime? ActionDate { get; set; }

        public int AccountStatusId { get; set; }

        public int? OperationId { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal LedgerBalance { get; set; }

        public int? RelationshipOfficerId { get; set; }

        public int? RelationshipManagerId { get; set; }

        [StringLength(50)]
        public string MISCode { get; set; }

        [StringLength(50)]
        public string TEAMMISCode { get; set; }
        public decimal? OverdraftAmount { get; set; }

        public decimal? OverdraftInterestRate { get; set; }

        public DateTime? OverdraftExpiryDate { get; set; }

        public bool? HasOverdraft { get; set; }

        public decimal LienAmount { get; set; }

        public bool HasLien { get; set; }

        public int? PostNoStatusId { get; set; }

        [StringLength(50)]
        public string OldAccountNumber1 { get; set; }

        [StringLength(50)]
        public string OldAccountNumber2 { get; set; }

        [StringLength(50)]
        public string OldAccountNumber3 { get; set; }

        public int? AprovalStatusId { get; set; }

        public int? CustomerSensitivityLevelId { get; set; }

        public bool? FromDeposit { get; set; }

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
