using Banking.Contracts.Response.InvestorFund;
using Banking.DomainObjects.InvestorFund;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.InvestorFund
{
    public interface IInvestorFundService
    {
        #region InvestorFund
        Task<InvestorFundRespObj> UpdateInvestorFund(InvestorFundObj entity);
        Task<InvestorFundRespObj> UpdateRollOverInvestorFund(InvestorFundObj entity);
        Task<InvestorFundRespObj> UpdateTopUpInvestorFund(InvestorFundObj entity);
        Task<ActionResult<InvestorFundRespObj>> GetInvestmentForAppraisalAsync();

        bool DeleteInvestorFund(int id);

        byte[] GenerateExportInvestorFund();

        Task<InvestorFundObj> GetInvestorFundAsync(int investorFundId);

        Task<InvestorFundRegRespObj> UpdateInvestorFundByCustomer(InvestorFundObj entity);

        InvestmentApprovalRecommendationObj UpdateInvestmentRecommendation(InvestmentApprovalRecommendationObj entity, string user);

        IEnumerable<InvestmentRecommendationLogObj> GetInvestmentRecommendationLog(int InvestorFundId);

        IEnumerable<InvestorFundObj> GetAllInvestorFundWebsiteList();

        InvestorFundObj GetAllInvestorFundWebsiteById(int Id);

        IEnumerable<InvestorRunningFacilitiesObj> GetAllInvestorRunningFacilities(int customerId);

        IEnumerable<InvestorRunningFacilitiesObj> GetAllInvestmentByCustomerId(int customerId);

        IEnumerable<InvestmentListObj> GetAllInvestmentList();

        IEnumerable<InvestorListObj> GetAllInvestorList();

        IEnumerable<InvestorFundObj> GetAllInvestmentCertificates();

        IEnumerable<InvestorFundObj> GetAllRunningInvestmenByCustomer(int customerId);

        IEnumerable<InvestorFundObj> GetAllRunningInvestment();

        IEnumerable<InvestorFundObj> GetAllRunningInvestmentByStatus(int Id);
        decimal GetCurrentBalance(DateTime date, int id);
        Task<bool> InvestmentApproval(int targetId, int approvalStatusId);
        IEnumerable<DailyInterestAccrualObj> ProcessDailyInvestmentInterestAccrual(DateTime applicationDate);
        IEnumerable<DailyInterestAccrualObj> ProcessMaturedInvestmentPosting(DateTime applicationDate);
        void ProcessRollOverPosting(DateTime applicationDate);
        Task<InvestorFundRegRespObj> UpdateInvestorFundRollOverByCustomer(InvestFundRollOverObj entity);
        InvestorFundObj GetAllInvestorFundRollOverWebsiteById(int Id);
        IEnumerable<InvestFundRollOverObj> GetAllInvestorFundRollOverWebsiteList();
        Task<InvestorFundRegRespObj> UpdateInvestorFundTopUpByCustomer(InvestFundTopUpObj entity);
        InvestorFundObj GetAllInvestorFundTopUpWebsiteById(int Id);
        IEnumerable<InvestFundTopUpObj> GetAllInvestorFundTopUpWebsiteList();

        //bool UploadInvestorFund(byte[] record, string createdBy);
        #endregion


        #region Collection
        Task<CollectionRespObj> AddUpdateCollection(CollectionObj entity);

        bool DeleteCollection(int id);

        byte[] GenerateExportCollection();

        List<CollectionObj> GetAllCollection();

        CollectionObj GetCollection(int Id);

        Task<CollectionRegRespObj> UpdateCollectionOperationByCustomer(CollectionObj entity);

        CollectionApprovalRecommendationObj UpdateCollectionRecommendation(CollectionApprovalRecommendationObj entity, string user);

        IEnumerable<CollectionRecommendationLogObj> GetCollectionRecommendationLog(int InvInvestorFundId);

        IEnumerable<CollectionObj> GetAllCollectionOperationWebsiteList();

        CollectionObj GetCollectionOperationWebsiteById(int id);

        Task<ActionResult<CollectionRespObj>> GetCollectionForAppraisalAsync();

        Task<bool> CollectionApproval(int targetId, int approvalStatusId);
        #endregion


        #region Liquidation
        Task<LiquidationRespObj> AddUpdateLiquidation(LiquidationObj entity);

        bool DeleteLiquidation(int id);

        byte[] GenerateExportLiquidation();

        List<LiquidationObj> GetAllLiquidation();

        Task<LiquidationObj> GetLiquidation(int Id);

        Task<LiquidationRegRespObj> UpdateLiquidationOperationByCustomer(LiquidationObj entity);

        LiquidationApprovalRecommendationObj UpdateLiquidationRecommendation(LiquidationApprovalRecommendationObj entity, string users);

        IEnumerable<LiquidationRecommendationLogObj> GetLiquidationRecommendationLog(int InvestorFundId);

        IEnumerable<LiquidationObj> GetAllLiquidationOperationWebsiteList();

        LiquidationObj GetLiquidationOperationWebsitebyId(int id);

        Task<ActionResult<LiquidationRespObj>> GetLiquidationForAppraisalAsync();

        Task<bool> LiquidationApproval(int targetId, int approvalStatusId);
        #endregion

    }
}
