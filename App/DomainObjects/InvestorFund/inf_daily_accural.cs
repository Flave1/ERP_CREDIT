using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public class inf_daily_accural
    {
        [Key]
        public int InvestmentDailyAccuralId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public int CategoryId { get; set; }

        public int TransactionTypeId { get; set; }

        public int ProductId { get; set; }

        public int CompanyId { get; set; }

        public int BranchId { get; set; }

        public int CurrencyId { get; set; }

        public double ExchangeRate { get; set; }

        public decimal Amount { get; set; }

        public double InterestRate { get; set; }

        public DateTime Date { get; set; }

        public decimal DailyAccuralAmount { get; set; }
    }
}
