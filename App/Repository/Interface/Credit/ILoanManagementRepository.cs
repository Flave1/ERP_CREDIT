using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanManagementRepository
    {
        IEnumerable<LoanReviewListObj> GetAllRunningLoan(string searchQuery, int? customerTypeId, int? productTypeId);
        IEnumerable<LoanReviewListObj> GetAllRunningLoanByCustomerId(int customerId);
        IEnumerable<LoanReviewApplicationObj> GetAllLoanReviewApplication();
        IEnumerable<LoanReviewApplicationObj> GetLoanReviewApplicationList();
        LoanReviewApplicationObj GetSingleLoanReviewApplication(int loanReviewApplicationId);
        LoanReviewApplicationObj GetLoanReviewApplicationbyLoanId(int loanId);
        int GoForApproval(LoanObj entity);
        Task<bool> AddTempLoanBooking(LoanObj entity);
        IEnumerable<LoanRecommendationLogObj> GetLoanReviewApplicationLog(int loanReviewApplicationId);
        ApprovalRecommendationObj UpdateLoanReviewApplicationLog(ApprovalRecommendationObj entity, string user);


        IEnumerable<LoanReviewListObj> GetAllLoanReviewApplicationOfferLetter();
        Task<bool> UploadLoanReviewOfferLetter(LoanApplicationObj entity);

        ApprovalRegRespObj LoanReviewApplicationApproval(ApprovalObj entity, int staffId, string username);

        Task<LoanReviewListObjRespObj> AddUpdateLMSApplication(LoanReviewApplicationObj entity);
        Task<LoanReviewListObjRespObj> GetLoanReviewAwaitingApproval();
    }
}
