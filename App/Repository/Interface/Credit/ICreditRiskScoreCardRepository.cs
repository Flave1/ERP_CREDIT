using Banking.Contracts.Response.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CreditBureauObjs;
using static Banking.Contracts.Response.Credit.CreditRiskAttributeObjs;
using static Banking.Contracts.Response.Credit.CreditRiskRatingObjs;
using static Banking.Contracts.Response.Credit.CreditRiskScoreCardObjs;
using static Banking.Contracts.Response.Credit.CreditWeightedRiskScoreCardObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ICreditRiskScoreCardRepository
    {
        bool AddUpdateCreditRiskScoreCard(List<CreditRiskScoreCardObj> model);
        IEnumerable<CreditRiskScoreCardObj> GetAllCreditRiskScoreCard();
        IEnumerable<CreditRiskScoreCardObj> GetCreditRiskScoreCard(int creditRiskAttributeId);
        bool DeleteCreditRiskScoreCard(int creditRiskScoreCardId);
        IEnumerable<CreditRiskScoreCardObj> GetDistinctAttribute();
        int AddUpdateAppicationScoreCard(LoanApplicationScoreCardObj entity);

        bool AddUpdateCreditAttribute(CreditRiskAttibuteObj entity);
        IEnumerable<CreditRiskAttibuteObj> GetAllCreditRiskAttribute();
        CreditRiskAttibuteObj GetCreditRiskAttribute(int creditRiskAttributeId);
        bool DeleteCreditRiskAttribute(int creditRiskAttributeId);
        IEnumerable<MappedCreditRiskAttibuteObj> GetAllMappedCreditRiskAttribute();
        IEnumerable<SystemAttributeObj> GetAllSystemCreditRiskAttribute();
        bool DeleteMultipleCreditRiskAttribute(int id);
        byte[] GenerateExportCreditRiskAttribute();
        bool UploadCreditRiskAttribute(List<byte[]> record, string createdBy);


        bool AddUpdateCreditCategory(CreditRiskCategoryObj entity);
        IEnumerable<CreditRiskCategoryObj> GetAllCreditRiskCategory();
        CreditRiskCategoryObj GetCreditRiskCategory(int creditRiskCategoryId);
        bool DeleteCreditRiskCategory(int creditRiskCategoryId);

        IEnumerable<ApplicationCreditRiskAttibuteObj> GetApplicationCreditRiskAttribute(int loanApplicatonId);

        bool AddUpdateCreditRiskRating(CreditRiskRatingObj entity);
        IEnumerable<CreditRiskRatingObj> GetAllCreditRiskRating();
        CreditRiskRatingDetailObj GetCreditRiskRatingDetail(int loanApplicationId);
        CreditRiskRatingObj GetCreditRiskRating(int creditRiskRatingId);
        bool DeleteCreditRiskRating(int creditRiskRatingId);
        bool UploadCreditRiskRate(List<byte[]> record, string createdBy);
        byte[] GenerateExportCreditRiskRate();


        bool AddUpdateCreditWeightedRiskScore(List<CreditWeightedRiskScoreObj> model);
        IEnumerable<CreditWeightedRiskScoreObj> GetAllCreditWeightedRiskScore();
        IEnumerable<CreditWeightedRiskScoreObj> GetCreditWeightedRiskScore(int productId);
        IEnumerable<CreditWeightedRiskScoreObj> GetCreditWeightedRiskScoreByCustomerType(int productId, int customerTypeId);
        bool DeleteCreditWeightedRiskScore(int weightedRiskScoreId);

        bool AddUpdateCreditBureau(CreditBureauObj entity);
        IEnumerable<CreditBureauObj> GetAllCreditBureau();
        CreditBureauObj GetCreditBureau(int creditBureauId);
        byte[] GenerateExportCreditBureau();
        bool UploadCreditBureau(List<byte[]> record, string createdBy);
        bool DeleteCreditBureau(int creditBureauId);
        Task DeleteListOfCreditBureau(params int[] ids);

        Task<bool> AddUpdateLoanCreditBureau(LoanCreditBureauObj entity);
        IEnumerable<LoanCreditBureauObj> GetAllLoanCreditBureau();
        LoanCreditBureauObj GetLoanCreditBureau(int loanCreditBureauId);
        bool DeleteLoanCreditBureau(int loanCreditBureauId);
        LoanCreditBureauObj GetLoanApplicationCreditBureau(int loanApplicationId);

        IEnumerable<GroupedCreditRiskAttibuteObj> GetGroupedAttribute();

        bool AddUpdateCreditRiskRatingPD(CreditRiskRatingPDObj entity);
        bool UploadCreditRiskRatingPD(List<byte[]> record, string createdBy);
        byte[] GenerateExportCreditRiskRatingPD();
        IEnumerable<CreditRiskRatingPDObj> GetAllCreditRiskRatingPD();
        IEnumerable<GroupedCreditRiskRatingPDObj> GetGroupedCreditRiskRatingPD();
        //CreditRiskRatingDetailViewModel GetCreditRiskRatingDetail(int loanApplicationId);
        CreditRiskRatingPDObj GetCreditRiskRatingPD(int creditRiskRatingPDId);
        bool DeleteCreditRiskRatingPD(int creditRiskRatingPDId);
    }
}
