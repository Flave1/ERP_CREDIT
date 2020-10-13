using Banking.Contracts.Response.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.DailyAccrualObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanOperationsRepository
    {
        IEnumerable<DailyInterestAccrualObj> ProcessDailyLoansInterestAccrual(DateTime applicationDate);
        IEnumerable<LoanRepaymentObj> ProcessLoanRepaymentPostingPastDue(DateTime applicationDate);
        IEnumerable<DailyInterestAccrualObj> ProcessDailyPastDueInterestAccrual(DateTime applicationDate);
        IEnumerable<DailyInterestAccrualObj> ProcessDailyPastDuePrincipalAccrual(DateTime applicationDate);


        bool InterestRateReview(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId);
        bool DateChange(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId);
        bool FrequencyChange(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId);
        bool LoanPrepayment(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId);
        bool TenorExtension(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId);
        LoanObj GetRunningLoans(int companyId, string refNo);
        IEnumerable<LoanObj> GetApprovedLoanReview();
        IEnumerable<LoanObj> GetApprovedLoanReviewRemedial();
        IEnumerable<LoanPaymentSchedulePeriodicObj> GetLoanScheduleByLoanId(int loanId);
        IEnumerable<LoanOperationTypeObj> GetRemedialOperationType();
        //IEnumerable<LoanReviewOperationApprovalViewModel> GetLoanOperationAwaitingApproval(string userName);
        IEnumerable<LoanReviewOperationApprovalObj> GetApprovedLoanOperationReview();
        int GoForApproval(ApprovalObj entity);
        void updateHistoricalLoanBalance();
        ApprovalRegRespObj AddOperationReview(LoanReviewOperationObj model);
        Task<IEnumerable<LoanOperationTypeObj>> GetOperationType();
        Task<IEnumerable<LoanOperationTypeObj>> GetOperationTypeRestructure();
    }
}
