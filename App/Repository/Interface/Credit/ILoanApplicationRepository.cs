using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanApplicationRepository
    {
        Task<LoanApplicationRespObj> UpdateLoanApplication(LoanApplicationObj entity);
        string UpdateLoanApplicationByCustomer(LoanApplicationObj entity);
        IEnumerable<LoanApplicationObj> GetAllLoanApplication();
        Task<LoanApplicationObj> GetLoanApplication(int loanApplicationId);
        LoanApplicationObj GetWebsiteLoanApplicationById(int id);
        bool DeleteLoanApplication(int loanApplicationId);
        IEnumerable<LoanApplicationObj> GetLoanApplicationByCustomer(int customerId);
        IEnumerable<LoanApplicationObj> GetWebsiteLoanApplicationList();

        IEnumerable<LoanApplicationObj> GetRunningLoanApplicationByCustomer(int customerId);

        IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetter();

        IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetterReview();

        IEnumerable<LoanApplicationObj> GetLoanApplicationByRefNumber(string applicationRefNumber);

        Task<LoanApplicationRespObj> SubmitLoanForCreditAppraisal(int applicationId);

        IEnumerable<LoanApplicationObj> GetLoanApplicationList();

        Task<bool> UploadOfferLetter(LoanApplicationObj entity);
        Task<bool> OfferLetterDecision(LoanApplicationObj entity);

        ApprovalRecommendationObj UpdateLoanApplicationRecommendation(ApprovalRecommendationObj entity, string user);

        IEnumerable<LoanRecommendationLogObjs> GetLoanRecommendationLog(int loanApplicationId);

        IList<CollateralManagementObj> GetCollateralManagementAsync();
        LoanApplicationObj GetLoanApplicationIdByPID(int cid, int pid);
        bool GetOfferletterDecisionStatus(int loanApplicationId);

        Task<credit_loanapplication> GetLoanapplicationOfferLeterAsync(string applicationRefNumber);
        credit_offerletter GetOfferletterDecision(int loanApplicationId);
    }
}
