using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class DailyAccrualObjs
    {
        public class DailyInterestAccrualObj : GeneralEntity

        {
            public string ReferenceNumber { get; set; }

            public string BaseReferenceNumber { get; set; }

            public int CategoryId { get; set; }

            public int TransactionTypeId { get; set; }

            public int ProductId { get; set; }

            public int CompanyId { get; set; }

            public int BranchId { get; set; }

            public int CurrencyId { get; set; }

            public double ExchangeRate { get; set; }

            public decimal MainAmount { get; set; }

            public double InterestRate { get; set; }

            public DateTime Date { get; set; }

            public int DayCountConventionId { get; set; }

            public double DailyAccuralAmount { get; set; }

            public decimal AvailableBalance { get; set; }

            public int DaysInAYear { get; set; }

        }
    }
}
