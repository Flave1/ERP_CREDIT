using Finance.Contracts.Response.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface IReportService
    {
        List<LoanPaymentSchedulePeriodicObj> GetOfferLeterPeriodicSchedule(string applicationRefNumber);
        List<LoanPaymentSchedulePeriodicObj> GetOfferLeterPeriodicScheduleLMS(string applicationRefNumber);
        OfferLetterDetailObj GenerateOfferLetterLMS(string loanRefNumber);
        List<ProductFeeObj> GetLoanApplicationFee(string applicationRefNumber);


        List<LoansObj> GetLoan(DateTime? date1, DateTime? date2);
        List<CorporateCustomerObj> GetCreditCustomerCorporate(DateTime? date1, DateTime? date2, int ct);
        List<IndividualCustomerObj> GetCreditCustomerIndividual(DateTime? date1, DateTime? date2, int ct);


        List<CorporateInvestorCustomerObj> GetInvestmentCustomerCorporate(DateTime? date1, DateTime? date2, int ct);
        List<IndividualInvestorCustomerObj> GetInvestmentCustomerIndividual(DateTime? date1, DateTime? date2, int ct);
        List<InvestmentObj> GetInvestment(DateTime? date1, DateTime? date2);
        List<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleInvestmentCertificate(string RefNumber);
        OfferLetterDetailObj GenerateInvestmentCertificate(string RefNumber);


        List<FSReportObj> GetFSReport(DateTime? date1, DateTime? date2);
        List<FSReportObj> GetPLReport(DateTime? date1, DateTime? date2);
    }
}
