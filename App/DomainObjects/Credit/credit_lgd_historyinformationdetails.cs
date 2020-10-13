using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_lgd_historyinformationdetails
    {
        [Key]
        public int HistoricalLGDId { get; set; }

        public decimal LoanAmount { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(250)]
        public string LoanReferenceNumber { get; set; }

        [StringLength(250)]
        public string ProductCode { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        [Required]
        [StringLength(50)]
        public string AttributeField { get; set; }

        public decimal Amount { get; set; }

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
