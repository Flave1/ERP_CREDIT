using Banking.Contracts.Response.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Credit
{
    public interface IDashboardRepository
    {
        DashboardObj GetPerformanceMatrics();
        CustomerDashboardObj GetCustomerTransactionSummary(string accountNumber);

        LoanApplicationDetailObj GetLoansApplicationDetails();

        LoanConcentrationObj GetLoanConcentrationDetails();

        DashboardObj GetPARForDashboard();

        LoanConcentrationObj GetOverDueForDashboard();
        CustomerDashboardObj LoanStaging();

        #region InvestmentDashboard
        InvestmentApplicationDetailObj GetInvestmentApplicationDetails();
        LoanConcentrationObj GetInvestmentConcentrationDetails();
        DashboardObj GetInvestmentPerformanceChart();
        #endregion
    }
}
