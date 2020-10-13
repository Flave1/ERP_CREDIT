using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.Response.Finance;
using Banking.Contracts.Response.IdentityServer;
using Banking.Contracts.Response.Mail;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationCollateralObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IAllowableCollateralRepository _allowableCollateralRepository;
        public LoanApplicationRepository(
            DataContext dataContext, 
            ICommonService commonService,
            IIdentityService identityService, 
            IAllowableCollateralRepository allowableCollateralRepository,
            IIdentityServerRequest serverRequest)

        {
            _dataContext = dataContext;
            _allowableCollateralRepository = allowableCollateralRepository;
            _serverRequest = serverRequest;
        }
        public bool DeleteLoanApplication(int loanApplicationId)
        {
            try
            {
                var loanApplication = _dataContext.credit_loanapplication.Find(loanApplicationId);
                if (loanApplication != null)
                {
                    loanApplication.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanApplicationObj> GetAllLoanApplication()
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = a.cor_currency.CurrencyName,
                                           CustomerName = a.credit_loancustomer.FirstName + " " + a.credit_loancustomer.LastName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           LoanApplicationStatusId = a.LoanApplicationStatusId,
                                           CompanyId = a.CompanyId,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetter()
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false && a.ApprovalStatusId == (int)ApprovalStatus.Approved
                                       && a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           CompanyId = (int)a.CompanyId,
                                           //companyName = _dataContext.cor_company.Where(x => x.CompanyId == a.CompanyId).FirstOrDefault().Name,
                                           LoanApplicationStatusId = (int)a.LoanApplicationStatusId,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetterReview()
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false && a.ApprovalStatusId == (int)ApprovalStatus.Approved
                                       && a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter
                                       //&& a.GenerateOfferLetter == true
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           CompanyId = (int)a.CompanyId,
                                           //companyName = _dataContext.cor_company.Where(x => x.CompanyId == a.CompanyId).FirstOrDefault().Name,
                                           LoanApplicationStatusId = (int)a.LoanApplicationStatusId,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IList<CollateralManagementObj> GetCollateralManagementAsync()
        {
            try
            {
                var loanCollaterals = (from a in _dataContext.credit_collateralcustomerconsumption
                                   join b in _dataContext.credit_collateralcustomer on a.CollateralCustomerId equals b.CollateralCustomerId
                                   join c in _dataContext.credit_loanapplication on a.LoanApplicationId equals c.LoanApplicationId
                                   join d in _dataContext.credit_product on c.ApprovedProductId equals d.ProductId
                                   //join e in _dataContext.credit_loan on a.LoanApplicationId equals d.LoanApplicationId
                                   where a.Deleted == false
                                   select

                                   new LoanApplicationCollateralObj
                                   {
                                       CollateralCustomerConsumptionId = a.CollateralCustomerConsumptionId,
                                       CollateralCustomerId = a.CollateralCustomerId,
                                       LoanApplicationId = a.LoanApplicationId,
                                       LoanApplicationRefNo = c.ApplicationRefNumber,
                                       ActualCollateralValue = a.Amount,
                                        //customerName = e.Name
                                        //collateralValue = b.CollateralValue,
                                        CollateralValue = (decimal)d.CollateralPercentage / 100 * c.ApprovedAmount,
                                       CollateralTypeName = b.credit_collateraltype.Name,
                                       CollateralCode = b.CollateralCode
                                   }).ToList();

            var collateralManagementsViewModel = new List<CollateralManagementObj>();
            var ids = loanCollaterals.Select(x => x.LoanApplicationId).Distinct().ToList();

            foreach (var id in ids)
            {
                var allCollateralSelectedById = loanCollaterals.Where(x => x.LoanApplicationId == id).ToList();
                var sum = allCollateralSelectedById.Sum(x => x.ActualCollateralValue);
                var loanApplicaton =  GetLoanApplication(id.Value).Result;

                collateralManagementsViewModel.Add(new CollateralManagementObj
                {
                    LoanApplicationId = id,
                    ExpectedCollateralValue = allCollateralSelectedById[0].CollateralValue,
                    TotalCollateralValue = sum,
                    CustomerId = loanApplicaton.CustomerId,
                    LoanApplicationRefNo = loanApplicaton.ApplicationRefNumber,
                    CustomerName = loanApplicaton.CustomerName,
                });
            }

            return collateralManagementsViewModel;
        }
            catch (Exception ex){
                throw new Exception(ex.Message);
        }
    }

        public async Task<LoanApplicationObj> GetLoanApplication(int loanApplicationId)
        {
            try
            {
                var CurrencyList = await _serverRequest.GetCurrencyAsync();
                //var compResponse = await _serverRequest.GetAllCompanyAsync();
                var staffResponse = await _serverRequest.GetAllStaffAsync();

                var _FrequencyType = await _dataContext.credit_frequencytype.Where(d => d.Deleted == false).ToListAsync();
                var _Product = await _dataContext.credit_product.Where(x => x.Deleted == false).ToListAsync();
                var _CreditProduct = await _dataContext.credit_product.Where(d => d.Deleted == false).ToListAsync();
                var _CreditRating = await _dataContext.credit_creditratingpd.Where(d => d.Deleted == false).ToListAsync();


                var loanCust = _dataContext.credit_loancustomer.Where(d => d.Deleted == false).ToList();  
                var loanApplicationObj = _dataContext.credit_loanapplication.Where(s => loanCust.Select(d => d.CustomerId).Contains(s.CustomerId)).ToList(); 

                var a = loanApplicationObj.FirstOrDefault(a => a.LoanApplicationId == loanApplicationId);
                var loanApp = new LoanApplicationObj()
                {
                    ApplicationDate = a.ApplicationDate,
                    ApprovedAmount = a.ApprovedAmount,
                    ApprovedProductId = a.ApprovedProductId,
                    ApprovedRate = a.ApprovedRate,
                    ApprovedTenor = a.ApprovedTenor,
                    CurrencyId = a.CurrencyId,
                    CustomerId = a.CustomerId,
                    EffectiveDate = a.EffectiveDate,
                    FirstPrincipalDate = a.FirstPrincipalDate,
                    FirstInterestDate = a.FirstInterestDate,
                    ExchangeRate = a.ExchangeRate,
                    HasDoneChecklist = a.HasDoneChecklist,
                    MaturityDate = a.MaturityDate,
                    ProposedAmount = a.ProposedAmount,
                    ProposedProductId = a.ProposedProductId,
                    ProposedRate = a.ProposedRate,
                    ProposedTenor = a.ProposedTenor,
                    ProductFrequency = _FrequencyType.FirstOrDefault(x => x.FrequencyTypeId == _Product.FirstOrDefault(x => x.ProductId == a.ApprovedProductId).FrequencyTypeId)?.Mode,
                    ProductTenor = _CreditProduct.FirstOrDefault(x => x.ProductId == a.ApprovedProductId)?.Period,
                    LoanApplicationId = a.LoanApplicationId,
                    CurrencyName = CurrencyList.commonLookups.FirstOrDefault(d => d.LookupId == a.CurrencyId)?.LookupName ?? "",
                    ApprovedProductName = _CreditProduct.FirstOrDefault(x => x.ProductId == a.ApprovedProductId)?.ProductName,
                    ProposedProductName = _CreditProduct.FirstOrDefault(x => x.ProductId == a.ProposedProductId)?.ProductName,
                    LoanApplicationStatusId = a.LoanApplicationStatusId,
                    CompanyId = a.CompanyId,
                    //CompanyName = compResponse.companyStructures.FirstOrDefault(x => x.companyStructureId == a.CompanyId)?.name ?? string.Empty,
                    ApplicationRefNumber = a.ApplicationRefNumber,
                    RiskBasedDescription = _CreditRating.FirstOrDefault(x => x.ProductId == a.ApprovedProductId
                    && x.MaxRangeScore >= a.Score && a.Score >= x.MinRangeScore && x.Deleted == false)?.Description == null ? "Flat Based"
                                          : _dataContext.credit_creditratingpd.FirstOrDefault(x => x.ProductId == a.ApprovedProductId
                                          && x.MaxRangeScore >= a.Score && a.Score >= x.MinRangeScore && x.Deleted == false)?.Description,
                    Purpose = a.Purpose,
                    CreditScore = a.Score,
                    ProbabilityOfDefault = a.PD,
                    ProductWeightedScore = _CreditProduct.FirstOrDefault(x => x.ProductId == a.ApprovedProductId)?.WeightedMaxScore,
                    PaymentAccount = a.PaymentAccount,
                    RepaymentAccount = a.RepaymentAccount
                };
                if(loanApp != null)
                {                  
                    var staffExist = staffResponse.staff.FirstOrDefault(x => x.staffId == loanCust.FirstOrDefault(d => a.CustomerId == d.CustomerId).RelationshipManagerId);
                    if (staffExist != null)
                    {
                        loanApp.RelationOfficer1st = staffResponse.staff.FirstOrDefault(x => x.staffId == loanCust.FirstOrDefault(d => a.CustomerId == d.CustomerId).RelationshipManagerId).firstName ?? string.Empty;
                        loanApp.RelationOfficer2nd = staffResponse.staff.FirstOrDefault(x => x.staffId == loanCust.FirstOrDefault(d => a.CustomerId == d.CustomerId).RelationshipManagerId).lastName ?? string.Empty;
                    }
                    else
                    {
                        loanApp.RelationOfficer1st = string.Empty;
                        loanApp.RelationOfficer2nd = string.Empty;
                    }
                }
                return loanApp ?? new LoanApplicationObj();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, new Exception("Error has Occurred"));
            }

        }

        public IEnumerable<LoanApplicationObj> GetLoanApplicationByCustomer(int customerId)
        {
            var loanApplication = (from a in _dataContext.credit_loanapplication
                                   where a.Deleted == false && a.CustomerId == customerId
                                   select new LoanApplicationObj
                                   {
                                       ApplicationDate = a.ApplicationDate,
                                       ApprovedAmount = a.ApprovedAmount,
                                       ApprovedProductId = a.ApprovedProductId,
                                       ApprovedRate = a.ApprovedRate,
                                       ApprovedTenor = a.ApprovedTenor,
                                       CurrencyId = a.CurrencyId,
                                       CustomerId = a.CustomerId,
                                       EffectiveDate = a.EffectiveDate,
                                       FirstPrincipalDate = a.FirstPrincipalDate,
                                       FirstInterestDate = a.FirstInterestDate,
                                       ExchangeRate = a.ExchangeRate,
                                       HasDoneChecklist = a.HasDoneChecklist,
                                       MaturityDate = a.MaturityDate,
                                       ProposedAmount = a.ProposedAmount,
                                       ProposedProductId = a.ProposedProductId,
                                       ProposedRate = a.ProposedRate,
                                       ProposedTenor = a.ProposedTenor,
                                       LoanApplicationId = a.LoanApplicationId,
                                       //currencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                       CustomerName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).FirstName,
                                       ApprovedProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ApprovedProductId).ProductName,
                                       ProposedProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProposedProductId).ProductName,
                                       LoanApplicationStatusId = a.LoanApplicationStatusId,
                                       CompanyId = a.CompanyId,
                                       ApplicationRefNumber = a.ApplicationRefNumber,
                                       Purpose = a.Purpose
                                   }).ToList();

            return loanApplication;
        }

        public IEnumerable<LoanApplicationObj> GetLoanApplicationByRefNumber(string applicationRefNumber)
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false && a.ApplicationRefNumber == applicationRefNumber
                                       && a.LoanApplicationStatusId == (int)ApprovalStatus.Approved
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           EffectiveDate = a.EffectiveDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           CompanyId = (int)a.CompanyId,
                                           //CompanyName = _dataContext.cor_company.Where(x => x.CompanyId == a.CompanyId).FirstOrDefault().Name,
                                           LoanApplicationStatusId = (int)a.LoanApplicationStatusId,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanApplicationObj GetLoanApplicationIdByPID(int cid, int pid)
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       join b in _dataContext.credit_loancustomer on a.CustomerId equals b.CustomerId
                                       where a.Deleted == false && a.ProposedProductId == pid && cid == b.CustomerTypeId
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId
                                       }).FirstOrDefault();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanApplicationObj> GetLoanApplicationList()
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false
                                       && a.LoanApplicationStatusId == (int)LoanApplicationStatusEnum.ApplicationInProgress
                                       || a.LoanApplicationStatusId == (int)LoanApplicationStatusEnum.ApplicationCompleted
                                       || a.LoanApplicationStatusId == (int)LoanApplicationStatusEnum.ChecklistInProgress
                                       || a.LoanApplicationStatusId == (int)LoanApplicationStatusEnum.ChecklistCompleted
                                       //|| a.LoanApplicationStatusId == (int)LoanApplicationStatusEnum.CAMInProgress
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //currencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           LoanApplicationStatusId = a.LoanApplicationStatusId,
                                           CompanyId = a.CompanyId,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanRecommendationLogObjs> GetLoanRecommendationLog(int loanApplicationId)
        {
            var log = (from a in _dataContext.credit_loanapplicationrecommendationlog
                       where a.LoanApplicationId == loanApplicationId
                       orderby a.CreatedOn descending
                       select new LoanRecommendationLogObjs
                       {
                           LoanApplicationId = a.LoanApplicationId,
                           Amount = a.ApprovedAmount,
                           ProductId = a.ApprovedProductId,
                           ProductName = _dataContext.credit_product.FirstOrDefault(x=>x.ProductId == a.ApprovedProductId).ProductName,
                           Rate = a.ApprovedRate,
                           Tenor = a.ApprovedTenor,
                           CreatedBy = a.CreatedBy
                           //createdBy = (from g in _dataContext.cor_useraccount
                                        //join b in _dataContext.cor_staff on g.StaffId equals b.StaffId
                                        //where g.UserName.ToLower() == a.CreatedBy
                                        //select b.FirstName + " " + b.LastName).FirstOrDefault(),
                       }).ToList();
            return log;
        }

        public bool GetOfferletterDecisionStatus(int loanApplicationId)
        {
            var loanExist = _dataContext.credit_offerletter.Where(x => x.LoanApplicationId == loanApplicationId).FirstOrDefault();
            if (loanExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public credit_offerletter GetOfferletterDecision(int loanApplicationId)
        {
            var loanExist = _dataContext.credit_offerletter.Where(x => x.LoanApplicationId == loanApplicationId).FirstOrDefault();
            if (loanExist != null)
            {
                return loanExist;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<LoanApplicationObj> GetRunningLoanApplicationByCustomer(int customerId)
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false 
                                       //&& a.GenerateOfferLetter 
                                       && a.CustomerId == customerId
                                       //&& a.MaturityDate > DateTime.Now
                                       && a.ApprovalStatusId == 2
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //currencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           LoanApplicationStatusId = a.LoanApplicationStatusId,
                                           CompanyId = a.CompanyId,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           Purpose = a.Purpose
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanApplicationObj GetWebsiteLoanApplicationById(int id)
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication_website
                                       join b in _dataContext.credit_loancustomer on a.CustomerId equals b.CustomerId
                                       where a.Deleted == false && a.WebsiteLoanApplicationId == id
                                       && a.LoanApplicationStatusId == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           WebsiteLoanApplicationId = a.WebsiteLoanApplicationId,
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = b.FirstName + " " + b.LastName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           LoanApplicationStatusId = a.LoanApplicationStatusId,
                                           CompanyId = a.CompanyId,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           DateCreated = a.CreatedOn,
                                           Purpose = a.Purpose,
                                       }).FirstOrDefault();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanApplicationObj> GetWebsiteLoanApplicationList()
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication_website
                                       join b in _dataContext.credit_loancustomer on a.CustomerId equals b.CustomerId
                                       where a.Deleted == false
                                       && a.LoanApplicationStatusId == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           WebsiteLoanApplicationId = a.WebsiteLoanApplicationId,
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           FirstPrincipalDate = a.FirstPrincipalDate,
                                           FirstInterestDate = a.FirstInterestDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = b.FirstName + " " + b.LastName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           LoanApplicationStatusId = a.LoanApplicationStatusId,
                                           CompanyId = a.CompanyId,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           DateCreated = a.CreatedOn,
                                           Purpose = a.Purpose,
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> OfferLetterDecision(LoanApplicationObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.OfferLetterId > 0)
                {
                    var uploadOfferLetterExist = _dataContext.credit_offerletter.Find(entity.OfferLetterId);
                    if (uploadOfferLetterExist != null)
                    {
                        uploadOfferLetterExist.LoanApplicationId = entity.LoanApplicationId;
                        uploadOfferLetterExist.ReportStatus = entity.ReportStatus;
                        uploadOfferLetterExist.Active = true;
                        uploadOfferLetterExist.Deleted = false;
                        uploadOfferLetterExist.UpdatedBy = entity.CreatedBy;
                        uploadOfferLetterExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var uploadOfferLetter = new credit_offerletter
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        ReportStatus = entity.ReportStatus,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_offerletter.Add(uploadOfferLetter);
                }
                var loanapp = _dataContext.credit_loanapplication.Where(x => x.LoanApplicationId == entity.LoanApplicationId).FirstOrDefault();
                if (entity.ReportStatus.ToLower() == "accept")
                {

                }
                else
                {
                    loanapp.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
                }

                var response = await _dataContext.SaveChangesAsync() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoanApplicationRespObj> SubmitLoanForCreditAppraisal(int applicationId)
        {
            try
           {
                var user = await _serverRequest.UserDataAsync();

                var loanApp = _dataContext.credit_loanapplication.Find(applicationId);
                var customer = _dataContext.credit_loancustomer.Find(loanApp.CustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                decimal actualCollateralValue = 0;
                if (customer.CustomerTypeId == (int)CustomerType.Individual)
                {
                    var loanScorecard = _dataContext.credit_individualapplicationscorecard.Where(x => x.LoanApplicationId == applicationId).ToList();
                    if (loanScorecard.Count == 0) 
                        return new LoanApplicationRespObj
                        {
                            ResponseId = 2,
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please fill Score Card to continue" } }
                        };
                }
                else
                {
                    var loanScorecard = _dataContext.credit_corporateapplicationscorecard.Where(x => x.LoanApplicationId == applicationId && x.Deleted == false).ToList();
                    if (loanScorecard.Count == 0) return new LoanApplicationRespObj
                    {
                        ResponseId = 2,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please fill Score Card to continue" } }
                    };
                }

                //var loanCreditBureau = _dataContext.credit_loancreditbureau.Where(x => x.LoanApplicationId == applicationId && x.Deleted == false).ToList();
                //if (loanCreditBureau.Count == 0) return new LoanApplicationRespObj
                //{
                //    ResponseId = 3,
                //    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please fill Credit Bureau to continue" } }
                //};

                var allowableCollaterals = await _allowableCollateralRepository.GetAllowableCollateralByProductIdAsync(loanApp.ProposedProductId);
                //var requireCollateral = _dataContext.credit_product.Where(x => x.ProductId == loanApp.ProposedProductId).FirstOrDefault().CollateralRequired;

                //Check for allowable collateral
                if (allowableCollaterals.Count() > 0)
                {
                    var collateral = _dataContext.credit_collateralcustomerconsumption.Where(x => x.LoanApplicationId == applicationId && x.Deleted == false).ToList();

                    if (collateral.Count == 0)
                    {
                        return new LoanApplicationRespObj
                        {
                            ResponseId = 4,
                            Status = new APIResponseStatus { IsSuccessful = false }
                        };
                    }
                    else
                    {
                        actualCollateralValue = collateral.Sum(x => x.Amount);
                    }

                    decimal collateralPer = (decimal)_dataContext.credit_product.Where(x => x.ProductId == loanApp.ProposedProductId && x.Deleted == false).FirstOrDefault().CollateralPercentage;

                    if ((collateralPer / 100 * loanApp.ApprovedAmount) > actualCollateralValue)
                    {
                        throw new Exception("Collateral Value is less than expected Collateral ", new Exception());
                    }
                }


                

                using(var _trans = _dataContext.Database.BeginTransaction())
                { 
                    if (!PassFeeIncomeEntry(loanApp.LoanApplicationId).Status.IsSuccessful)
                    {
                        return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = { FriendlyMessage = "Financial Entry not passed" } } };
                    }
                    await _serverRequest.SendMail(new MailObj
                    {
                        fromAddresses = new List<FromAddress> { },
                        toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                        subject = "Successful Loan Application",
                        content = $"Hi {customer.FirstName}, <br> Your application is successful and awaiting approval.",
                        sendIt = true,
                    });

                    await _serverRequest.SendMail(new MailObj
                    {
                        fromAddresses = new List<FromAddress> { },
                        toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = user.FirstName}
                            },
                        subject = "Successful Loan Application",
                        content = $"Hi {staff.firstName}, <br> Appraisal is pending for Loan application with reference : "+ loanApp.ApplicationRefNumber + "",
                        sendIt = true,
                    });

                    var targetIds = new List<int>();
                    targetIds.Add(loanApp.LoanApplicationId);
                    var request = new GoForApprovalRequest
                    {
                        StaffId = user.StaffId,
                        CompanyId = 1,
                        StatusId = (int)ApprovalStatus.Pending,
                        TargetId = targetIds,
                        Comment = "Loan Application Approval",
                        OperationId = (int)OperationsEnum.LoanApplicationApproval,
                        DeferredExecution = true, // false by default will call the internal SaveChanges() 
                        ExternalInitialization = true,
                        EmailNotification = true,
                    };

                    var result = await _serverRequest.GotForApprovalAsync(request);

                    try
                    {
                        if (!result.IsSuccessStatusCode)
                        {
                            new ApprovalRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }
                        var stringData = await result.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                        if (res.ApprovalProcessStarted)
                        {
                            loanApp.HasDoneChecklist = true;
                            loanApp.LoanApplicationStatusId = (int)LoanApplicationStatusEnum.ChecklistCompleted;
                            loanApp.WorkflowToken = res.Status.CustomToken;                           
                            await _dataContext.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new LoanApplicationRespObj
                            {
                                ResponseId = 1,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = res.Status.IsSuccessful,
                                    Message = res.Status.Message
                                }
                            };
                        }

                        if (res.EnableWorkflow || !res.HasWorkflowAccess)
                        {
                            await _trans.RollbackAsync();
                            return new LoanApplicationRespObj
                            { 
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = false,
                                    Message = res.Status.Message
                                }
                            };
                        }
                        if (!res.EnableWorkflow)
                        {
                            loanApp.HasDoneChecklist = true;
                            loanApp.LoanApplicationStatusId = (int)LoanApplicationStatusEnum.ChecklistCompleted;
                            await _dataContext.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new LoanApplicationRespObj
                            {
                                LoanApplicationId = loanApp.LoanApplicationId,
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = true,
                                    Message = new APIResponseMessage
                                    {
                                        FriendlyMessage = "Successful"
                                    }
                                }
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        _trans.Rollback();
                        throw ex;
                    }
                    finally { _trans.Dispose(); }
                }

                return new LoanApplicationRespObj
                {
                    ResponseId = 1,
                    Status = new APIResponseStatus { IsSuccessful = false }
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private LoanApplicationRespObj PassFeeIncomeEntry(int loanAppId)
        {
            var loanApp = _dataContext.credit_loanapplication.Find(loanAppId);
            var customer = _dataContext.credit_loancustomer.Find(loanApp.CustomerId);

            var pd_rate = _dataContext.credit_creditratingpd.Where(x => x.ProductId == loanApp.ProposedProductId && x.Deleted == false).ToList();
            if (pd_rate != null)
            {
                foreach (var item in pd_rate)
                {
                    if (item.MaxRangeScore >= loanApp.Score && loanApp.Score >= item.MinRangeScore)
                    {
                        loanApp.ProposedRate = (double)item.InterestRate;
                        loanApp.ApprovedRate = (double)item.InterestRate;
                    }
                }
            }


            deposit_accountsetup operatingAccount;
            if (customer != null)
            {
                var depositAccount = _dataContext.deposit_accountopening.Where(x => x.AccountNumber == customer.CASAAccountNumber).FirstOrDefault();
                if (depositAccount != null)
                {
                    operatingAccount = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == depositAccount.AccountTypeId).FirstOrDefault();
                }
                else
                {
                    return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Loan Customer doesn't have an operating account" } } };
                }
            }
            else
            {
                return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Loan Customer doesn't exist" } } };
            }
            var productFee = _dataContext.credit_productfee.Where(x => x.ProductId == loanApp.ApprovedProductId && x.Deleted == false).ToList();
            var casa = _dataContext.credit_casa.Where(x => x.AccountNumber == customer.CASAAccountNumber).FirstOrDefault();
            if (casa == null)
            {
                return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer doesn't have CASA Account" } } };
            }
            foreach (var curr in productFee)
            {
                var fee = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == curr.FeeId && x.IsIntegral == false && x.Deleted == false);
                decimal productamount = 0;
                if (curr.ProductFeeType == 2)
                {
                    productamount = (loanApp.ApprovedAmount * Convert.ToDecimal(curr.ProductAmount)) / 100;
                }
                else
                {
                    productamount = Convert.ToDecimal(curr.ProductAmount);
                }
                if (productamount > casa.AvailableBalance)
                {
                    return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer Account Not funded" } } };
                }
                if (fee != null)
                {
                    var entry = new TransactionObj
                    {
                        IsApproved = false,
                        CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loanApp.CustomerId).FirstOrDefault().CASAAccountNumber,
                        CompanyId = loanApp.CompanyId ?? 0,
                        Amount = productamount,
                        CurrencyId = loanApp.CurrencyId,
                        Description = "Payment of Non Integral Fee",
                        DebitGL = operatingAccount.GLMapping ?? 1,
                        CreditGL = fee.TotalFeeGL.Value,
                        ReferenceNo = loanApp.ApplicationRefNumber,
                        OperationId = (int)OperationsEnum.LoanApplicationApproval,
                        JournalType = "System",
                        RateType = 1,//Buying Rate
                    };

                    var res = _serverRequest.PassEntryToFinance(entry).Result;
                    if (!res.Status.IsSuccessful)
                    {
                        //return new LoanApplicationRespObj { ResponseId = 1, Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res.Status.Message.FriendlyMessage } } };
                    }
                    //Update Product Fee Status
                    List<credit_productfeestatus> statuses = new List<credit_productfeestatus>();
                    var status = new credit_productfeestatus();
                    status.LoanApplicationId = loanApp.LoanApplicationId;
                    status.ProductFeeId = curr.ProductFeeId;
                    status.Status = true;
                    statuses.Add(status);
                    _dataContext.credit_productfeestatus.AddRange(statuses);
                    _dataContext.SaveChanges();
                }
                   
            }

            var resp = new LoanApplicationRespObj
            {
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage { FriendlyMessage = "Transaction Successful" }
                }
            };
            return resp;
        }

        public async Task<LoanApplicationRespObj> UpdateLoanApplication(LoanApplicationObj entity)
        {
            try
            {
                if (entity == null)
                    return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid request payload" } } };

                var customer = _dataContext.credit_loancustomer.Find(entity.CustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();               
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);               

                if (entity.WebsiteLoanApplicationId > 0)
                {
                    var oldApp = _dataContext.credit_loanapplication_website.Where(x => x.WebsiteLoanApplicationId == entity.WebsiteLoanApplicationId).FirstOrDefault();
                    oldApp.LoanApplicationStatusId = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                if(entity.PaymentMode == 2)//Funding to other banks
                {
                    var bankDetail = _dataContext.credit_customerbankdetails.FirstOrDefault(x => x.CustomerId == entity.CustomerId && x.Deleted == false);
                    if (bankDetail == null)
                    {
                        return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Bank Details not updated" } } };
                    }
                }

                if(entity.RepaymentMode == 2)//Card Payment
                {
                    var cardDetail = _dataContext.credit_customercarddetails.FirstOrDefault(x => x.CustomerId == entity.CustomerId);
                    if(cardDetail == null)
                    {
                        return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Card Payment Details not found" } } };
                    }
                    var maturityDate = entity.EffectiveDate.AddDays(entity.ProposedTenor);
                    var maturityYear = maturityDate.Year.ToString();
                    var expiryMonth = Convert.ToInt32(cardDetail.ExpiryMonth);
                    maturityYear = maturityYear.Substring(2, 2);
                    if(maturityYear == cardDetail.ExpiryYear)
                    {
                        if (expiryMonth < maturityDate.Month)
                        {
                            return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Card Payment Expiry Date is before the loan maturity date" } } };
                        }
                    }
                   
                }else if (entity.RepaymentMode == 1)//Direct Debit
                {
                    var bankDetail = _dataContext.credit_customerbankdetails.FirstOrDefault(x => x.CustomerId == entity.CustomerId && x.Deleted == false);
                    if (bankDetail == null)
                    {
                        return new LoanApplicationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Bank Details not found" } } };
                    }
                }

                credit_loanapplication loanApplication = new credit_loanapplication();
                if (entity.LoanApplicationId > 0)
                {
                    loanApplication = _dataContext.credit_loanapplication.Find(entity.LoanApplicationId);
                    if (loanApplication != null)
                    {
                        loanApplication.ApprovedAmount = entity.ProposedAmount;
                        loanApplication.ApprovedProductId = entity.ProposedProductId;
                        loanApplication.ApprovedRate = entity.ProposedRate;
                        loanApplication.ApprovedTenor = entity.ProposedTenor;
                        loanApplication.CurrencyId = entity.CurrencyId;
                        loanApplication.CustomerId = entity.CustomerId;
                        loanApplication.CompanyId = entity.CompanyId;
                        loanApplication.EffectiveDate = entity.EffectiveDate;
                        loanApplication.FirstInterestDate = entity.FirstInterestDate;
                        loanApplication.FirstPrincipalDate = entity.FirstPrincipalDate;
                        loanApplication.ExchangeRate = entity.ExchangeRate;
                        loanApplication.HasDoneChecklist = entity.HasDoneChecklist;
                        loanApplication.MaturityDate = entity.EffectiveDate.AddDays(entity.ProposedTenor);
                        loanApplication.ProposedAmount = entity.ProposedAmount;
                        loanApplication.ProposedProductId = entity.ProposedProductId;
                        loanApplication.ProposedRate = entity.ProposedRate;
                        loanApplication.ProposedTenor = entity.ProposedTenor;
                        loanApplication.Purpose = entity.Purpose;
                        loanApplication.PaymentMode = entity.PaymentMode;
                        loanApplication.RepaymentMode = entity.RepaymentMode;
                        loanApplication.RepaymentAccount = entity.RepaymentAccount;
                        loanApplication.PaymentAccount = entity.PaymentAccount;
                        loanApplication.Active = true;
                        loanApplication.Deleted = false;
                        loanApplication.UpdatedBy = entity.CreatedBy;
                        loanApplication.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var refNo = GenerateLoanReferenceNumber();

                    loanApplication = new credit_loanapplication
                    {

                        ApprovedAmount = entity.ProposedAmount,
                        ApprovedProductId = entity.ProposedProductId,
                        ApprovedRate = entity.ProposedRate,
                        ApprovedTenor = entity.ProposedTenor,
                        CurrencyId = entity.CurrencyId,
                        CustomerId = entity.CustomerId,
                        CompanyId = entity.CompanyId,
                        EffectiveDate = entity.EffectiveDate,
                        FirstInterestDate = entity.FirstInterestDate,
                        FirstPrincipalDate = entity.FirstPrincipalDate,
                        ExchangeRate = entity.ExchangeRate,
                        HasDoneChecklist = entity.HasDoneChecklist,
                        MaturityDate = entity.EffectiveDate.AddDays(entity.ProposedTenor),
                        ProposedAmount = entity.ProposedAmount,
                        ProposedProductId = entity.ProposedProductId,
                        ProposedRate = entity.ProposedRate,
                        ProposedTenor = entity.ProposedTenor,
                        LoanApplicationStatusId = (int)LoanApplicationStatusEnum.ApplicationInProgress,
                        ApplicationRefNumber = refNo,
                        ApplicationDate = DateTime.Now.Date,
                        Purpose = entity.Purpose,
                        PaymentMode = entity.PaymentMode,
                        RepaymentMode = entity.RepaymentMode,
                        PaymentAccount = entity.PaymentAccount,
                        RepaymentAccount = entity.RepaymentAccount,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now
                    };
                    _dataContext.credit_loanapplication.Add(loanApplication);
                }

                using (var trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var output = _dataContext.SaveChanges() > 0;

                        if (output)
                        {
                            trans.Commit();
                            await _serverRequest.SendMail(new MailObj
                            {
                                fromAddresses = new List<FromAddress> { },
                                toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                                subject = "Application Successfully Submitted",
                                content = $"Hi {customer.FirstName}, <br> Your application was received and it's currently undergoing eligibility check.",
                                sendIt = true,
                            });

                            await _serverRequest.SendMail(new MailObj
                            {
                                fromAddresses = new List<FromAddress> { },
                                toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                                subject = "Eligibility Check Pending",
                                content = $"Hi {staff.firstName}, <br> Eligibility check is pending for the Loan Application with the reference: " + loanApplication.ApplicationRefNumber + "",
                                sendIt = true,
                            });
                        }
                        return new LoanApplicationRespObj
                        {
                            LoanApplicationId = loanApplication.LoanApplicationId,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved Successfully" } }
                        };
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateLoanApplicationByCustomer(LoanApplicationObj entity)
        {
            try
            {
                if (entity == null) return null;
                credit_loanapplication_website loanApplication = null;

                if (entity.LoanApplicationId > 0)
                {
                    loanApplication = _dataContext.credit_loanapplication_website.Find(entity.LoanApplicationId);
                    if (loanApplication != null)
                    {
                        loanApplication.ApprovedAmount = entity.ProposedAmount;
                        loanApplication.ApprovedProductId = entity.ProposedProductId;
                        loanApplication.ApprovedRate = entity.ProposedRate;
                        loanApplication.ApprovedTenor = entity.ProposedTenor;
                        loanApplication.CurrencyId = entity.CurrencyId;
                        loanApplication.CustomerId = entity.CustomerId;
                        loanApplication.CompanyId = entity.CompanyId;
                        loanApplication.EffectiveDate = entity.EffectiveDate;
                        loanApplication.FirstInterestDate = entity.FirstInterestDate;
                        loanApplication.FirstPrincipalDate = entity.FirstPrincipalDate;
                        loanApplication.ExchangeRate = entity.ExchangeRate;
                        loanApplication.HasDoneChecklist = entity.HasDoneChecklist;
                        loanApplication.MaturityDate = entity.EffectiveDate.AddDays(entity.ProposedTenor);
                        loanApplication.ProposedAmount = entity.ProposedAmount;
                        loanApplication.ProposedProductId = entity.ProposedProductId;
                        loanApplication.ProposedRate = entity.ProposedRate;
                        loanApplication.ProposedTenor = entity.ProposedTenor;
                        loanApplication.Purpose = entity.Purpose;
                        loanApplication.Active = true;
                        loanApplication.Deleted = false;
                        loanApplication.UpdatedBy = entity.CreatedBy;
                        loanApplication.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var refNo = GenerateLoanReferenceNumber();

                    loanApplication = new credit_loanapplication_website
                    {

                        ApprovedAmount = entity.ProposedAmount,
                        ApprovedProductId = entity.ProposedProductId,
                        ApprovedRate = entity.ProposedRate,
                        ApprovedTenor = entity.ProposedTenor,
                        CurrencyId = entity.CurrencyId,
                        CustomerId = entity.CustomerId,
                        CompanyId = entity.CompanyId,
                        EffectiveDate = entity.EffectiveDate,
                        FirstInterestDate = entity.FirstInterestDate,
                        FirstPrincipalDate = entity.FirstPrincipalDate,
                        ExchangeRate = entity.ExchangeRate,
                        HasDoneChecklist = entity.HasDoneChecklist,
                        MaturityDate = entity.EffectiveDate.AddDays(entity.ProposedTenor),
                        ProposedAmount = entity.ProposedAmount,
                        ProposedProductId = entity.ProposedProductId,
                        ProposedRate = entity.ProposedRate,
                        ProposedTenor = entity.ProposedTenor,
                        LoanApplicationStatusId = (int)LoanApplicationStatusEnum.ApplicationInProgress,
                        ApplicationRefNumber = refNo,
                        ApplicationDate = DateTime.Now.Date,
                        Purpose = entity.Purpose,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now
                    };
                    _dataContext.credit_loanapplication_website.Add(loanApplication);
                }
                var response = _dataContext.SaveChanges() > 0;
                return loanApplication.ApplicationRefNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ApprovalRecommendationObj UpdateLoanApplicationRecommendation(ApprovalRecommendationObj entity, string user)
        {
            try
            {
                if (entity == null) return null;

                if (entity.LoanApplicationId > 0)
                {
                    var loanApplicationExist = _dataContext.credit_loanapplication.Find(entity.LoanApplicationId);
                    if (loanApplicationExist != null)
                    {

                        loanApplicationExist.ApprovedAmount = entity.ApprovedAmount;
                        loanApplicationExist.ApprovedProductId = entity.ApprovedProductId;
                        loanApplicationExist.ApprovedRate = entity.ApprovedRate;
                        loanApplicationExist.ApprovedTenor = entity.ApprovedTenor;
                        loanApplicationExist.UpdatedBy = user;
                        loanApplicationExist.UpdatedOn = DateTime.Now;

                        var log = new credit_loanapplicationrecommendationlog
                        {
                            LoanApplicationId = entity.LoanApplicationId,
                            ApprovedAmount = entity.ApprovedAmount,
                            ApprovedProductId = entity.ApprovedProductId,
                            ApprovedRate = entity.ApprovedRate,
                            ApprovedTenor = entity.ApprovedTenor,
                            CreatedBy = user,
                            CreatedOn = DateTime.Now
                        };
                        _dataContext.credit_loanapplicationrecommendationlog.Add(log);
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    return new ApprovalRecommendationObj
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        ApprovedAmount = entity.ApprovedAmount,
                        ApprovedProductId = entity.ApprovedProductId,
                        ApprovedRate = entity.ApprovedRate,
                        ApprovedTenor = entity.ApprovedTenor,
                    };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UploadOfferLetter(LoanApplicationObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.OfferLetterId > 0)
                {
                    var uploadOfferLetterExist = _dataContext.credit_offerletter.Find(entity.OfferLetterId);
                    if (uploadOfferLetterExist != null)
                    {
                        uploadOfferLetterExist.LoanApplicationId = entity.LoanApplicationId;
                        uploadOfferLetterExist.ReportStatus = entity.ReportStatus;
                        uploadOfferLetterExist.SupportDocument = entity.SupportDocument;
                        uploadOfferLetterExist.Active = true;
                        uploadOfferLetterExist.Deleted = false;
                        uploadOfferLetterExist.UpdatedBy = entity.CreatedBy;
                        uploadOfferLetterExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var uploadOfferLetter = new credit_offerletter
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        ReportStatus = entity.ReportStatus,
                        SupportDocument = entity.SupportDocument,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_offerletter.Add(uploadOfferLetter);
                }
                var loanapp = _dataContext.credit_loanapplication.Where(x => x.LoanApplicationId == entity.LoanApplicationId).FirstOrDefault();
                if (entity.ReportStatus.ToLower() == "accept")
                {

                }
                else
                {
                    loanapp.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
                }
                var response = await _dataContext.SaveChangesAsync() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<credit_loanapplication> GetLoanapplicationOfferLeterAsync(string applicationRefNumber)
        {
            var result = await _dataContext.credit_loanapplication.
                FirstOrDefaultAsync(d => d.Deleted == false && 
                d.ApplicationRefNumber.ToLower().Trim() == applicationRefNumber.ToLower().Trim());
            return result;
        }
        private string GenerateLoanReferenceNumber()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            double unixTime = (int)unixTicks.TotalSeconds;
            return unixTime.ToString();
        }
    }
}
