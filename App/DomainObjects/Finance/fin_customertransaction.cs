using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Finance
{
    public class fin_customertransaction
    {
        [Key]
        public int CustomerTransactionId { get; set; }

        [StringLength(50)]
        public string TransactionCode { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? ValueDate { get; set; }

        public decimal? Amount { get; set; }

        public decimal? DebitAmount { get; set; }

        public decimal? CreditAmount { get; set; }

        public decimal? AvailableBalance { get; set; }

        [StringLength(50)]
        public string Beneficiary { get; set; }

        [StringLength(50)]
        public string BatchNo { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }
    }
}
