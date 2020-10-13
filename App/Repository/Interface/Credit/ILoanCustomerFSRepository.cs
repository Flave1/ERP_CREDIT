using Banking.Contracts.Response.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;

namespace Banking.Repository.Interface.Credit
{
   public interface ILoanCustomerFSRepository
    {
        #region Loan Customer FS Caption Group
        bool AddUpdateLoanCustomerFSCaptionGroup(LoanCustomerFSCaptionGroupObj entity);
        IEnumerable<LoanCustomerFSCaptionGroupObj> GetAllLoanCustomerFSCaptionGroup();
        LoanCustomerFSCaptionGroupObj GetLoanCustomerFSCaptionGroup(int fSCaptionGroupId);
        IEnumerable<LoanCustomerFSCaptionGroupObj> GetLoanCustomerFSCaptionGroup();
        bool DeleteLoanCustomerFSCaptionGroup(int fSCaptionGroupId);
        Task MultipleDeleteLoanCustomerFSCaptionGroup(params int[] targetIds);
        Task MultipleDeleteLoanCustomerFSCaption(params int?[] targetIds);
        IList<LoanCustomerFSCaptionObj> GetLoanCustomerFSCaptionGroupByGroupId(int fSCaptionGroupId);

        #endregion


        #region Loan Customer FS Caption 
        bool AddUpdateLoanCustomerFSCaption(LoanCustomerFSCaptionObj entity);
        IEnumerable<LoanCustomerFSCaptionObj> GetAllLoanCustomerFSCaption();
        LoanCustomerFSCaptionObj GetLoanCustomerFSCaptionByCaptionId(int fSCaptionId);
        IEnumerable<LoanCustomerFSCaptionObj> GetLoanCustomerFSCaptionByCaptionGroupId(int fSCaptionGroupId);
        IEnumerable<LoanCustomerFSCaptionObj> GetUnmappedLoanCustomerFSCaption(short fSCaptionGroupId, int customerId, DateTime fsDate);
        bool DeleteLoanCustomerFSCaption(int fSCaptionId);

        #endregion

        #region Loan Customer FS Ratio Detail
        bool AddUpdateLoanCustomerFSCaptionDetail(LoanCustomerFSCaptionDetailObj entity);
        IEnumerable<LoanCustomerFSCaptionDetailObj> GetAllLoanCustomerFSCaptionDetail();
        LoanCustomerFSCaptionDetailObj GetLoanCustomerFSCaptionDetailById(int fsDetailId);
        IEnumerable<LoanCustomerFSCaptionDetailObj> GetMappedCustomerFsCaptionDetail(int customerId, short fsCaptionGroupId, DateTime fsDate);
        IEnumerable<LoanCustomerFSCaptionDetailObj> GetMappedCustomerFsCaptions(int customerId);
        bool DeleteLoanCustomerFSCaptionDetail(int fSDetailId);
        #endregion

        #region Loan Customer FS Ratio Detail
        bool AddUpdateLoanCustomerFSRatioDetail(LoanCustomerFSRatioDetailObj entity);
        IEnumerable<LoanCustomerFSRatioDetailObj> GetAllLoanCustomerFSRatioDetail();
        IEnumerable<LoanCustomerFSRatioCaptionObj> GetFSRatioCaption();
        LoanCustomerFSRatioDetailObj GetLoanCustomerFSRatioDetail(int ratioDetailId);
        bool DeleteLoanCustomerFSRatioDetail(int ratioDetailId);
        IEnumerable<LoanCustomerFSRatioDetailObj> GetLoanCustomerFSRatioDetail(int ratioCaptionId, int fsCaptionGroupId);
        List<LoanCustomerFSRatioCaptionReportObj> GetLoanCustomerFSRatioValues(int customerId);
        IList<LoanCustomerFSRatiosCalculationObj> GetLoanCustomerFSRatiosCalculation(int customerId);
        IEnumerable<LookupObj> GetAllLoanCustomerFSDivisorType();
        IEnumerable<LookupObj> GetAllLoanCustomerFSValueType();
        #endregion

        bool ValidateFSCaptionGroup(string captionGroupName);
        bool ValidateFSCaption(string captionName);
    }
}
