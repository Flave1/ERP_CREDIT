using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loanapplication_website
    {
        [Key]
        public int WebsiteLoanApplicationId { get; set; }

        public int CustomerId { get; set; }

        public int ProposedProductId { get; set; }

        public int ProposedTenor { get; set; }

        public double ProposedRate { get; set; }
        public decimal ProposedAmount { get; set; }

        public int ApprovedProductId { get; set; }

        public int ApprovedTenor { get; set; }

        public double ApprovedRate { get; set; }

        public decimal ApprovedAmount { get; set; }

        public int CurrencyId { get; set; }

        public double ExchangeRate { get; set; }

        public int ApprovalStatusId { get; set; }

        public bool HasDoneChecklist { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime MaturityDate { get; set; }

        public DateTime? FirstPrincipalDate { get; set; }
        public DateTime? FirstInterestDate { get; set; }

        public int? LoanApplicationStatusId { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string ApplicationRefNumber { get; set; }

        public decimal? Score { get; set; }

        public decimal? PD { get; set; }

        public bool GenerateOfferLetter { get; set; }

        [StringLength(2000)]
        public string Purpose { get; set; }

        public bool Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
