using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using GOSLibraries.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CreditBureauObjs;
using static Banking.Contracts.Response.Credit.CreditRiskAttributeObjs;
using static Banking.Contracts.Response.Credit.CreditRiskRatingObjs;
using static Banking.Contracts.Response.Credit.CreditRiskScoreCardObjs;
using static Banking.Contracts.Response.Credit.CreditWeightedRiskScoreCardObjs;
using Banking.Requests;

namespace Banking.Repository.Implement.Credit
{
    public class CreditRiskScoreCardRepository : ICreditRiskScoreCardRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerRequest _serverRequest;
        public CreditRiskScoreCardRepository(DataContext dataContext, IConfiguration configuration, IIdentityServerRequest serverRequest)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _serverRequest = serverRequest;
        }
        public int AddUpdateAppicationScoreCard(LoanApplicationScoreCardObj entity)
        {
            try
            {

                if (entity == null) return 0;

                var loanApp = _dataContext.credit_loanapplication.Find(entity.LoanApplicationId);
                var customer = _dataContext.credit_loancustomer.Find(loanApp.CustomerId);

                var data = (from a in _dataContext.credit_weightedriskscore
                            join b in _dataContext.credit_creditriskscorecard on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                            where a.CreditRiskAttributeId == b.CreditRiskAttributeId && a.ProductId == loanApp.ApprovedProductId && a.CustomerTypeId == (int)CustomerType.Individual && b.CustomerTypeId == a.CustomerTypeId
                            select a).ToList();


                if (customer.CustomerTypeId == (int)CustomerType.Individual)
                {

                    var scoreCardExist = _dataContext.credit_individualapplicationscorecard.
                        Where(x => x.LoanApplicationId == entity.LoanApplicationId).ToList();
                    if (scoreCardExist.Count > 0)
                    {
                        _dataContext.credit_individualapplicationscorecard.RemoveRange(scoreCardExist);
                        _dataContext.SaveChanges();
                    }

                    var scoreCard = new credit_individualapplicationscorecard
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        CustomerId = loanApp.CustomerId,
                        ProductId = loanApp.ProposedProductId,
                        Field1 = Convert.ToDecimal(entity.Field1),
                        Field2 = Convert.ToDecimal(entity.Field2),
                        Field3 = Convert.ToDecimal(entity.Field3),
                        Field4 = Convert.ToDecimal(entity.Field4),
                        Field5 = Convert.ToDecimal(entity.Field5),
                        Field6 = Convert.ToDecimal(entity.Field6),
                        Field7 = Convert.ToDecimal(entity.Field7),
                        Field8 = Convert.ToDecimal(entity.Field8),
                        Field9 = Convert.ToDecimal(entity.Field9),
                        Field10 = Convert.ToDecimal(entity.Field10),
                        Field11 = Convert.ToDecimal(entity.Field11),
                        Field12 = Convert.ToDecimal(entity.Field12),
                        Field13 = Convert.ToDecimal(entity.Field13),
                        Field14 = Convert.ToDecimal(entity.Field14),
                        Field15 = Convert.ToDecimal(entity.Field15),
                        Field16 = Convert.ToDecimal(entity.Field16),
                        Field17 = Convert.ToDecimal(entity.Field17),
                        Field18 = Convert.ToDecimal(entity.Field18),
                        Field19 = Convert.ToDecimal(entity.Field19),
                        Field20 = Convert.ToDecimal(entity.Field20),
                        Field21 = Convert.ToDecimal(entity.Field21),
                        Field22 = Convert.ToDecimal(entity.Field22),
                        Field23 = Convert.ToDecimal(entity.Field23),
                        Field24 = Convert.ToDecimal(entity.Field24),
                        Field25 = Convert.ToDecimal(entity.Field25),
                        Field26 = Convert.ToDecimal(entity.Field26),
                        Field27 = Convert.ToDecimal(entity.Field27),
                        Field28 = Convert.ToDecimal(entity.Field28),
                        Field29 = Convert.ToDecimal(entity.Field29),
                        Field30 = Convert.ToDecimal(entity.Field30),
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.credit_individualapplicationscorecard.Add(scoreCard);


                    if (entity.AttributeList.Count > 0)
                    {
                        var exist = _dataContext.credit_individualapplicationscorecarddetails
                            .Where(x => x.LoanApplicationId == entity.LoanApplicationId).ToList();
                        if (exist.Count > 0)
                        {
                            _dataContext.credit_individualapplicationscorecarddetails.RemoveRange(exist);
                        }

                        foreach (var item in entity.AttributeList.OrderBy(a => a.AttributeField))
                        {
                            var details = new credit_individualapplicationscorecarddetails
                            {
                                LoanApplicationId = entity.LoanApplicationId,
                                AttributeField = item.AttributeField,
                                Score = Convert.ToDecimal(item.Score),
                                CustomerId = loanApp.CustomerId,
                                ProductId = loanApp.ProposedProductId,
                                Active = true,
                                Deleted = false,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = entity.CreatedBy,
                                UpdatedOn = DateTime.Now,
                            };
                            _dataContext.credit_individualapplicationscorecarddetails.Add(details);
                        }
                    }

                    var response = _dataContext.SaveChanges() > 0;
                    int output = 0;
                    if (response)
                    {
                        CalculateIndividualScoreCard(loanApp.CustomerId, entity.LoanApplicationId);
                        output = 1; // _loanRepo.SubmitLoanForCreditAppraisal(entity.loanApplicationId, entity.createdBy);

                    }
                    return output;
                }
                else
                {

                    var scoreCardExist = _dataContext.credit_corporateapplicationscorecard.
                   Where(x => x.CustomerId == loanApp.CustomerId && x.LoanApplicationId == entity.LoanApplicationId).ToList();
                    if (scoreCardExist.Count > 0)
                    {
                        _dataContext.credit_corporateapplicationscorecard.RemoveRange(scoreCardExist);
                        _dataContext.SaveChanges();
                    }
                    var scoreCard = new credit_corporateapplicationscorecard
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        CustomerId = loanApp.CustomerId,
                        ProductId = loanApp.ProposedProductId,
                        //history.Field1 = item.Field1 == 1000000 ? null : item.Field1;
                        Field1 = Convert.ToDecimal(entity.Field1),
                        Field2 = Convert.ToDecimal(entity.Field2),
                        Field3 = Convert.ToDecimal(entity.Field3),
                        Field4 = Convert.ToDecimal(entity.Field4),
                        Field5 = Convert.ToDecimal(entity.Field5),
                        Field6 = Convert.ToDecimal(entity.Field6),
                        Field7 = Convert.ToDecimal(entity.Field7),
                        Field8 = Convert.ToDecimal(entity.Field8),
                        Field9 = Convert.ToDecimal(entity.Field9),
                        Field10 = Convert.ToDecimal(entity.Field10),
                        Field11 = Convert.ToDecimal(entity.Field11),
                        Field12 = Convert.ToDecimal(entity.Field12),
                        Field13 = Convert.ToDecimal(entity.Field13),
                        Field14 = Convert.ToDecimal(entity.Field14),
                        Field15 = Convert.ToDecimal(entity.Field15),
                        Field16 = Convert.ToDecimal(entity.Field16),
                        Field17 = Convert.ToDecimal(entity.Field17),
                        Field18 = Convert.ToDecimal(entity.Field18),
                        Field19 = Convert.ToDecimal(entity.Field19),
                        Field20 = Convert.ToDecimal(entity.Field20),
                        Field21 = Convert.ToDecimal(entity.Field21),
                        Field22 = Convert.ToDecimal(entity.Field22),
                        Field23 = Convert.ToDecimal(entity.Field23),
                        Field24 = Convert.ToDecimal(entity.Field24),
                        Field25 = Convert.ToDecimal(entity.Field25),
                        Field26 = Convert.ToDecimal(entity.Field26),
                        Field27 = Convert.ToDecimal(entity.Field27),
                        Field28 = Convert.ToDecimal(entity.Field28),
                        Field29 = Convert.ToDecimal(entity.Field29),
                        Field30 = Convert.ToDecimal(entity.Field30),
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.credit_corporateapplicationscorecard.Add(scoreCard);


                    if (entity.AttributeList.Count > 0)
                    {

                        var exist = _dataContext.credit_corporateapplicationscorecarddetails
                            .Where(x => x.LoanApplicationId == entity.LoanApplicationId).ToList();
                        if (exist.Count > 0)
                        {
                            _dataContext.credit_corporateapplicationscorecarddetails.RemoveRange(exist);

                            _dataContext.SaveChanges();
                        }

                        foreach (var item in entity.AttributeList.OrderBy(a => a.AttributeField))
                        {
                            var details = new credit_corporateapplicationscorecarddetails
                            {
                                LoanApplicationId = entity.LoanApplicationId,
                                AttributeField = item.AttributeField,
                                Score = Convert.ToDecimal(item.Score),
                                CustomerId = loanApp.CustomerId,
                                ProductId = loanApp.ProposedProductId,
                                Active = true,
                                Deleted = false,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = entity.CreatedBy,
                                UpdatedOn = DateTime.Now,
                            };
                            _dataContext.credit_corporateapplicationscorecarddetails.Add(details);
                        }
                    }

                    var response = _dataContext.SaveChanges() > 0;

                    int output = 0;
                    if (response)
                    {
                        CalculateCorperateScoreCard(loanApp.CustomerId, entity.LoanApplicationId);
                        output = 1; // _loanRepo.SubmitLoanForCreditAppraisal(entity.loanApplicationId, entity.createdBy);

                    }

                    return output;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditAttribute(CreditRiskAttibuteObj entity)
        {
            try
            {
                if (entity == null) return false;
                if (entity.SystemAttribute == "Others")
                {
                    entity.SystemAttribute = entity.CreditRiskAttribute;
                }
                else
                {
                    entity.SystemAttribute = entity.SystemAttribute;
                }

                if (entity.CreditRiskAttributeId > 0)
                {
                    var creditRiskAttributeExist = _dataContext.credit_creditriskattribute.Find(entity.CreditRiskAttributeId);
                    if (creditRiskAttributeExist != null)
                    {
                        creditRiskAttributeExist.CreditRiskAttributeId = entity.CreditRiskAttributeId;
                        creditRiskAttributeExist.CreditRiskCategoryId = entity.CreditRiskCategoryId;
                        creditRiskAttributeExist.CreditRiskAttribute = entity.CreditRiskAttribute;
                        creditRiskAttributeExist.FriendlyName = entity.SystemAttribute;
                        creditRiskAttributeExist.Active = true;
                        creditRiskAttributeExist.Deleted = false;
                        creditRiskAttributeExist.CreatedBy = entity.CreatedBy;
                        creditRiskAttributeExist.CreatedOn = DateTime.Now;
                        creditRiskAttributeExist.UpdatedBy = entity.CreatedBy;
                        creditRiskAttributeExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var attribute = (from a in _dataContext.credit_creditriskattribute
                                     orderby a.CreatedOn descending
                                     select a.AttributeField).FirstOrDefault();
                    string fieldName = "";
                    if (attribute != null)
                    {
                        int lastNumber = int.Parse(attribute.Substring(5));
                        fieldName = "Field" + (lastNumber + 1);

                    }
                    else
                    {
                        fieldName = "Field1";
                    }
                    var creditRiskAttribute = new credit_creditriskattribute
                    {
                        CreditRiskAttributeId = entity.CreditRiskAttributeId,
                        CreditRiskCategoryId = entity.CreditRiskCategoryId,
                        CreditRiskAttribute = entity.CreditRiskAttribute,
                        FriendlyName = entity.SystemAttribute,
                        AttributeField = fieldName,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.credit_creditriskattribute.Add(creditRiskAttribute);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditBureau(CreditBureauObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.CreditBureauId > 0)
                {
                    var bureauExist = _dataContext.credit_creditbureau.FirstOrDefault(x=>x.CreditBureauName.ToLower().Trim() == entity.CreditBureauName.ToLower().Trim());
                    if (bureauExist != null)
                    {
                        bureauExist.CorporateChargeAmount = entity.CorporateChargeAmount;
                        bureauExist.CreditBureauName = entity.CreditBureauName;
                        bureauExist.GLAccountId = entity.GLAccountId;
                        bureauExist.IndividualChargeAmount = entity.IndividualChargeAmount;
                        bureauExist.IsMandatory = entity.IsMandatory;
                        bureauExist.Active = true;
                        bureauExist.Deleted = false;
                        bureauExist.CreatedBy = entity.CreatedBy;
                        bureauExist.CreatedOn = DateTime.Now;
                        bureauExist.UpdatedBy = entity.CreatedBy;
                        bureauExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var creditBureau = new credit_creditbureau
                    {
                        CorporateChargeAmount = entity.CorporateChargeAmount,
                        CreditBureauName = entity.CreditBureauName,
                        GLAccountId = entity.GLAccountId,
                        IndividualChargeAmount = entity.IndividualChargeAmount,
                        IsMandatory = entity.IsMandatory,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.credit_creditbureau.Add(creditBureau);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditCategory(CreditRiskCategoryObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.CategoryId > 0)
                {
                    var categoryExist = _dataContext.credit_creditriskcategory.Find(entity.CategoryId);
                    if (categoryExist != null)
                    {
                        categoryExist.CreditRiskCategoryName = entity.CategoryName;
                        categoryExist.Description = entity.Description;
                        //categoryExist.UseInOrigination = entity.useInOrigination;
                        categoryExist.Active = true;
                        categoryExist.Deleted = false;
                        categoryExist.UpdatedBy = entity.CreatedBy;
                        categoryExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var category = new credit_creditriskcategory
                    {
                        Description = entity.Description,
                        CreditRiskCategoryName = entity.CategoryName,
                        //UseInOrigination = entity.useInOrigination,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_creditriskcategory.Add(category);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditRiskRating(CreditRiskRatingObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.CreditRiskRatingId > 0)
                {
                    var riskRatingExist = _dataContext.credit_creditrating.Find(entity.CreditRiskRatingId);
                    if (riskRatingExist != null)
                    {
                        riskRatingExist.Rate = entity.Rate;
                        riskRatingExist.MinRange = entity.MinRange;
                        riskRatingExist.MaxRange = entity.MaxRange;
                        riskRatingExist.AdvicedRange = entity.AdvicedRange;
                        riskRatingExist.RateDescription = entity.RateDescription;
                        riskRatingExist.Active = true;
                        riskRatingExist.Deleted = false;
                        riskRatingExist.UpdatedBy = entity.CreatedBy;
                        riskRatingExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var risk = new credit_creditrating
                    {
                        Rate = entity.Rate,
                        MinRange = entity.MinRange,
                        MaxRange = entity.MaxRange,
                        AdvicedRange = entity.AdvicedRange,
                        RateDescription = entity.RateDescription,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_creditrating.Add(risk);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditRiskRatingPD(CreditRiskRatingPDObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.CreditRiskRatingPDId > 0)
                {
                    var riskRatingExist = _dataContext.credit_creditratingpd.Find(entity.CreditRiskRatingPDId);
                    if (riskRatingExist != null)
                    {
                        riskRatingExist.PD = entity.Pd;
                        riskRatingExist.MinRangeScore = entity.MinRange;
                        riskRatingExist.MaxRangeScore = entity.MaxRange;
                        riskRatingExist.Description = entity.Description;
                        riskRatingExist.InterestRate = entity.InterestRate;
                        riskRatingExist.ProductId = entity.ProductId;
                        riskRatingExist.Active = true;
                        riskRatingExist.Deleted = false;
                        riskRatingExist.UpdatedBy = entity.CreatedBy;
                        riskRatingExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var risk = new credit_creditratingpd
                    {
                        PD = entity.Pd,
                        MinRangeScore = entity.MinRange,
                        MaxRangeScore = entity.MaxRange,
                        Description = entity.Description,
                        InterestRate = entity.InterestRate,
                        ProductId = entity.ProductId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_creditratingpd.Add(risk);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditRiskScoreCard(List<CreditRiskScoreCardObj> model)
        {
            try
            {
                bool response = false;
                if (model == null) return false;

                var creditRiskAttributeId = model[0].CreditRiskAttributeId;
                var creditRiskScoreCardExist = _dataContext.credit_creditriskscorecard.Where(x => x.CreditRiskAttributeId == creditRiskAttributeId).ToList();

                if (creditRiskScoreCardExist.Count > 0)
                {
                    _dataContext.credit_creditriskscorecard.RemoveRange(creditRiskScoreCardExist);
                    _dataContext.SaveChanges();
                }
                List<credit_creditriskscorecard> creditRiskScoreCards = new List<credit_creditriskscorecard>();
                foreach (var entity in model)
                {

                    var creditRiskScoreCard = new credit_creditriskscorecard
                    {
                        CreditRiskAttributeId = entity.CreditRiskAttributeId,
                        CustomerTypeId = entity.CustomerTypeId,
                        Value = entity.Value,
                        Score = entity.Score,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    creditRiskScoreCards.Add(creditRiskScoreCard);
                }
                _dataContext.credit_creditriskscorecard.AddRange(creditRiskScoreCards);
                response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateCreditWeightedRiskScore(List<CreditWeightedRiskScoreObj> model)
        {
            try
            {
                if (model.Count == 0) return false;
                var productId = model[0].ProductId;
                var customerTypeId = model[0].CustomerTypeId;
                var weightExist = _dataContext.credit_weightedriskscore.
                      Where(x => x.ProductId == productId && x.CustomerTypeId == customerTypeId).ToList();
                if (weightExist.Count > 0)
                {
                    _dataContext.credit_weightedriskscore.RemoveRange(weightExist);
                    _dataContext.SaveChanges();
                }
                List<credit_weightedriskscore> weighterisks = new List<credit_weightedriskscore>();
                int fieldCount = 1;
                foreach (var entity in model)
                {
                    var weighterisk = new credit_weightedriskscore
                    {
                        CreditRiskAttributeId = entity.CreditRiskAttributeId,
                        FeildName = "Field" + fieldCount,
                        ProductId = entity.ProductId,
                        CustomerTypeId = entity.CustomerTypeId,
                        WeightedScore = entity.WeightedScore,
                        ProductMaxWeight = entity.ProductMaxWeight,
                        UseAtOrigination = entity.UseAtOrigination,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    weighterisks.Add(weighterisk);
                    fieldCount = fieldCount + 1;
                }
                _dataContext.credit_weightedriskscore.AddRange(weighterisks);
                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddUpdateLoanCreditBureau(LoanCreditBureauObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.LoanCreditBureauId > 0)
                {
                    var loanCreditBureauExist = _dataContext.credit_loancreditbureau.Find(entity.LoanCreditBureauId);
                    if (loanCreditBureauExist != null)

                        if (loanCreditBureauExist != null)
                        {
                            loanCreditBureauExist.LoanApplicationId = entity.LoanApplicationId;
                            loanCreditBureauExist.CreditBureauId = entity.CreditBureauId;
                            loanCreditBureauExist.ChargeAmount = entity.ChargeAmount;
                            loanCreditBureauExist.ReportStatus = entity.ReportStatus;
                            loanCreditBureauExist.SupportDocument = entity.SupportDocument;
                            loanCreditBureauExist.Active = true;
                            loanCreditBureauExist.Deleted = false;
                            loanCreditBureauExist.UpdatedBy = entity.CreatedBy;
                            loanCreditBureauExist.UpdatedOn = DateTime.Now;
                        }
                }
                else
                {
                    var creditBureau = new credit_loancreditbureau
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        CreditBureauId = entity.CreditBureauId,
                        ChargeAmount = entity.ChargeAmount,
                        ReportStatus = entity.ReportStatus,
                        SupportDocument = entity.SupportDocument,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancreditbureau.Add(creditBureau);
                }

                var response = await _dataContext.SaveChangesAsync() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditBureau(int creditBureauId)
        {
            try
            {
                var creditRisk = _dataContext.credit_creditbureau.Find(creditBureauId);
                if (creditRisk != null)
                {
                    creditRisk.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditRiskAttribute(int creditRiskAttributeId)
        {
            try
            {
                var creditRiskAttribute = _dataContext.credit_creditriskattribute.Find(creditRiskAttributeId);
                if (creditRiskAttribute != null)
                {
                    creditRiskAttribute.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditRiskCategory(int creditRiskCategoryId)
        {
            try
            {
                var creditRiskCategory = _dataContext.credit_creditriskcategory.Find(creditRiskCategoryId);
                if (creditRiskCategory != null)
                {
                    creditRiskCategory.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditRiskRating(int creditRiskRatingId)
        {
            try
            {
                var creditRisk = _dataContext.credit_creditrating.Find(creditRiskRatingId);
                if (creditRisk != null)
                {
                    creditRisk.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditRiskRatingPD(int creditRiskRatingPDId)
        {
            try
            {
                var creditRisk = _dataContext.credit_creditratingpd.Find(creditRiskRatingPDId);
                if (creditRisk != null)
                {
                    creditRisk.Deleted = true;
                }
                _dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditRiskScoreCard(int creditRiskScoreCardId)
        {
            try
            {
                var creditRiskScoreCard = _dataContext.credit_creditriskscorecard.Find(creditRiskScoreCardId);
                if (creditRiskScoreCard != null)
                {
                    _dataContext.credit_creditriskscorecard.Remove(creditRiskScoreCard);
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditWeightedRiskScore(int weightedRiskScoreId)
        {
            try
            {
                var creditRisk = _dataContext.credit_weightedriskscore.Find(weightedRiskScoreId);
                if (creditRisk != null)
                {
                    creditRisk.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteListOfCreditBureau(params int[] ids)
        {
            var creditBureaus = await _dataContext.credit_creditbureau.Where(x => ids.Contains(x.CreditBureauId)).ToListAsync();

            foreach (var creditBureau in creditBureaus)
            {
                creditBureau.Deleted = true;
            }

            await _dataContext.SaveChangesAsync();
        }

        public bool DeleteLoanCreditBureau(int loanCreditBureauId)
        {
            try
            {
                var creditRisk = _dataContext.credit_loancreditbureau.Find(loanCreditBureauId);
                if (creditRisk != null)
                {
                    creditRisk.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteMultipleCreditRiskAttribute(int id)
        {
            var data = _dataContext.credit_creditriskattribute.Find(id);
            if (data != null)
            {
                data.Deleted = true;
            }

           return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportCreditBureau()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Credit Bureau");
            dt.Columns.Add("Corporate Charge Amount");
            dt.Columns.Add("Individual Charge Amount");
            dt.Columns.Add("Compulsory");

            var structures = (from a in _dataContext.credit_creditbureau
                              where a.Deleted == false
                              select new CreditBureauObj
                              {
                                  CreditBureauId = a.CreditBureauId,
                                  CreditBureauName = a.CreditBureauName,
                                  CorporateChargeAmount = a.CorporateChargeAmount,
                                  IndividualChargeAmount = a.IndividualChargeAmount,
                                  IsMandatory = a.IsMandatory
                              }).ToList();


            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["Credit Bureau"] = kk.CreditBureauName;
                row["Corporate Charge Amount"] = kk.CorporateChargeAmount;
                row["Individual Charge Amount"] = kk.IndividualChargeAmount;
                row["Compulsory"] = kk.IsMandatory;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CreditBureau");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportCreditRiskAttribute()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Attribute");
            dt.Columns.Add("Category");
            dt.Columns.Add("System Attribute");
            var creditRisk = (from a in _dataContext.credit_creditriskattribute
                              where a.Deleted == false
                              select new CreditRiskAttibuteObj
                              {
                                  CreditRiskAttributeId = a.CreditRiskAttributeId,
                                  CreditRiskCategoryId = a.CreditRiskCategoryId,
                                  CreditRiskCategoryName = _dataContext.credit_creditriskcategory.FirstOrDefault(x => x.CreditRiskCategoryId == a.CreditRiskCategoryId).CreditRiskCategoryName,
                                  CreditRiskAttribute = a.CreditRiskAttribute,
                                  AttributeField = a.AttributeField,
                                  SystemAttribute = a.FriendlyName,
                              }).ToList();

            foreach (var kk in creditRisk)
            {
                var row = dt.NewRow();
                row["Attribute"] = kk.CreditRiskAttribute;
                row["Category"] = kk.CreditRiskCategoryName;
                row["System Attribute"] = kk.SystemAttribute;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (creditRisk != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("creditRisk");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportCreditRiskRate()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Rating");
            dt.Columns.Add("Minimum Range");
            dt.Columns.Add("Maximum Range");
            dt.Columns.Add("Adviced Range");
            dt.Columns.Add("Rate Description");
            var creditRisk = (from a in _dataContext.credit_creditrating
                              where a.Deleted == false
                              select new CreditRiskRatingObj
                              {
                                  CreditRiskRatingId = a.CreditRiskRatingId,
                                  Rate = a.Rate,
                                  MinRange = a.MinRange,
                                  MaxRange = a.MaxRange,
                                  AdvicedRange = a.AdvicedRange,
                                  RateDescription = a.RateDescription,
                              }).ToList();

            foreach (var kk in creditRisk)
            {
                var row = dt.NewRow();
                row["Rating"] = kk.Rate;
                row["Minimum Range"] = kk.MinRange;
                row["Maximum Range"] = kk.MaxRange;
                row["Adviced Range"] = kk.AdvicedRange;
                row["Rate Description"] = kk.RateDescription;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (creditRisk != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("creditRisk");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportCreditRiskRatingPD()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PD");
            dt.Columns.Add("Min Range Score");
            dt.Columns.Add("Max Range Score");
            dt.Columns.Add("Description");
            dt.Columns.Add("Interest Rate");
            dt.Columns.Add("Product Name");
            var statementType = (from a in _dataContext.credit_creditratingpd
                                 where a.Deleted == false
                                 select new CreditRiskRatingPDObj
                                 {
                                     Pd = a.PD,
                                     MinRange = a.MaxRangeScore,
                                     MaxRange = a.MaxRangeScore,
                                     Description = a.Description,
                                     InterestRate = a.InterestRate,
                                     ProductId = a.ProductId
                                 }).ToList();

            foreach (var kk in statementType)
            {
                var row = dt.NewRow();
                kk.ProductName = _dataContext.credit_product.Where(x => x.ProductId == kk.ProductId).FirstOrDefault().ProductName;
                row["PD"] = kk.Pd;
                row["Min Range Score"] = kk.MinRange;
                row["Max Range Score"] = kk.MaxRange;
                row["Description"] = kk.Description;
                row["Interest Rate"] = kk.InterestRate;
                row["Product Name"] = kk.ProductName;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Credit Risk Rating PD");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public IEnumerable<CreditBureauObj> GetAllCreditBureau()
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditbureau
                                  where a.Deleted == false
                                  select new CreditBureauObj
                                  {
                                      CreditBureauId = a.CreditBureauId,
                                      CorporateChargeAmount = a.CorporateChargeAmount,
                                      CreditBureauName = a.CreditBureauName,
                                      GLAccountId = a.GLAccountId,
                                      IndividualChargeAmount = a.IndividualChargeAmount,
                                      IsMandatory = a.IsMandatory,

                                  }).ToList();

                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskAttibuteObj> GetAllCreditRiskAttribute()
        {
            try
            {
                var creditRiskScoreCard = (from a in _dataContext.credit_creditriskattribute
                                           where a.Deleted == false
                                           select new CreditRiskAttibuteObj
                                           {
                                               CreditRiskAttributeId = a.CreditRiskAttributeId,
                                               CreditRiskCategoryId = a.CreditRiskCategoryId,
                                               CreditRiskCategoryName = _dataContext.credit_creditriskcategory.FirstOrDefault(x => x.CreditRiskCategoryId == a.CreditRiskCategoryId).CreditRiskCategoryName,
                                               CreditRiskAttribute = a.CreditRiskAttribute,
                                               AttributeField = a.AttributeField,
                                               SystemAttribute = a.FriendlyName,
                                           }).ToList();

                return creditRiskScoreCard;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskCategoryObj> GetAllCreditRiskCategory()
        {
            try
            {
                var creditRiskCategory = (from a in _dataContext.credit_creditriskcategory
                                          where a.Deleted == false
                                          select new CreditRiskCategoryObj
                                          {
                                              CategoryId = a.CreditRiskCategoryId,
                                              CategoryName = a.CreditRiskCategoryName,
                                              Description = a.Description,
                                              //useInOrigination = a.UseInOrigination
                                          }).ToList();

                return creditRiskCategory;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskRatingObj> GetAllCreditRiskRating()
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditrating
                                  where a.Deleted == false
                                  select new CreditRiskRatingObj
                                  {
                                      CreditRiskRatingId = a.CreditRiskRatingId,
                                      Rate = a.Rate,
                                      MinRange = a.MinRange,
                                      MaxRange = a.MaxRange,
                                      AdvicedRange = a.AdvicedRange,
                                      RateDescription = a.RateDescription,
                                  }).ToList();

                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskRatingPDObj> GetAllCreditRiskRatingPD()
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditratingpd
                                  where a.Deleted == false
                                  select new CreditRiskRatingPDObj
                                  {
                                      CreditRiskRatingPDId = a.CreditRiskRatingPDId,
                                      Pd = a.PD,
                                      MinRange = a.MinRangeScore,
                                      MaxRange = a.MaxRangeScore,
                                      Description = a.Description,
                                      InterestRate = a.InterestRate,
                                      ProductId = a.ProductId,
                                      ProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProductId && x.Deleted == false).FirstOrDefault().ProductName
                                  }).ToList();

                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskScoreCardObj> GetAllCreditRiskScoreCard()
        {
            try
            {
                var creditRiskScoreCard = (from a in _dataContext.credit_creditriskscorecard
                                               //where a.Deleted == false
                                           orderby a.CreditRiskAttributeId
                                           select new CreditRiskScoreCardObj
                                           {
                                               CreditRiskAttributeId = a.CreditRiskAttributeId,
                                               CreditRiskAttributeName = _dataContext.credit_creditriskattribute.FirstOrDefault(x => x.CreditRiskAttributeId == a.CreditRiskAttributeId).CreditRiskAttribute,
                                               CustomerTypeId = a.CustomerTypeId,
                                               CustomerTypeName = a.CustomerTypeId == (int)CustomerType.Corporate ? "Corporate" : "Individual",
                                               Value = a.Value,
                                               Score = a.Score,
                                               CreditRiskScoreCardId = a.CreditRiskScoreCardId
                                           }).ToList();

                return creditRiskScoreCard;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditWeightedRiskScoreObj> GetAllCreditWeightedRiskScore()
        {
            try
            {
                //var creditRisk = (from b in _dataContext.credit_product
                //                  join a in _dataContext.credit_weightedriskscore on b.ProductId equals a.ProductId
                //                  into adds
                //                  from a in adds.DefaultIfEmpty()
                //                  where a.Deleted == false
                //                  orderby a.ProductId
                //                  select new WeightedScoreTreeNode
                //                  {
                //                      data = new CreditWeightedRiskScoreObj
                //                      {
                //                          WeightedRiskScoreId = a.WeightedRiskScoreId,
                //                          CreditRiskAttributeId = a.CreditRiskAttributeId,
                //                          AttributeName = a.credit_creditriskattribute.CreditRiskAttribute,
                //                          ProductId = a.ProductId,
                //                          ProductName = a.credit_product.ProductName,
                //                          WeightedScore = a.WeightedScore,
                //                          ProductMaxWeight = a.ProductMaxWeight,
                //                          UseAtOrigination = a.UseAtOrigination,

                //                      },
                //                      children = adds.Select(x => new CreditWeightedRiskScoreObj
                //                      {
                //                          WeightedRiskScoreId = a.WeightedRiskScoreId,
                //                          CreditRiskAttributeId = a.CreditRiskAttributeId,
                //                          AttributeName = a.credit_creditriskattribute.CreditRiskAttribute,
                //                          ProductId = a.ProductId,
                //                          ProductName = a.credit_product.ProductName,
                //                          WeightedScore = a.WeightedScore,
                //                          ProductMaxWeight = a.ProductMaxWeight,
                //                          UseAtOrigination = a.UseAtOrigination,
                //                      }).ToList()

                //                  }).ToList();

                //return creditRisk;
                try
                {
                    var creditRisk = (from a in _dataContext.credit_weightedriskscore
                                      where a.Deleted == false
                                      orderby a.ProductId
                                      select new CreditWeightedRiskScoreObj
                                      {
                                          WeightedRiskScoreId = a.WeightedRiskScoreId,
                                          CreditRiskAttributeId = a.CreditRiskAttributeId,
                                          AttributeName = a.credit_creditriskattribute.CreditRiskAttribute,
                                          ProductId = a.ProductId,
                                          CustomerTypeId = a.CustomerTypeId,
                                          ProductName = a.credit_product.ProductName,
                                          WeightedScore = a.WeightedScore,
                                          ProductMaxWeight = a.ProductMaxWeight,
                                          UseAtOrigination = a.UseAtOrigination,
                                      }).ToList();

                    return creditRisk;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCreditBureauObj> GetAllLoanCreditBureau()
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_loancreditbureau
                                  where a.Deleted == false
                                  select new LoanCreditBureauObj
                                  {
                                      LoanCreditBureauId = a.LoanCreditBureauId,
                                      LoanApplicationId = a.LoanApplicationId,
                                      LoanApplicationRefNo = a.credit_loanapplication.ApplicationRefNumber,
                                      CreditBureauId = a.CreditBureauId,
                                      CreditBureauName = a.credit_creditbureau.CreditBureauName,
                                      ChargeAmount = a.ChargeAmount,
                                      ReportStatus = a.ReportStatus,
                                      SupportDocument = a.SupportDocument,
                                  }).ToList();

                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }

        public IEnumerable<MappedCreditRiskAttibuteObj> GetAllMappedCreditRiskAttribute()
        {
            try
            {
                var creditRiskScoreCard = (from a in _dataContext.credit_creditriskattribute
                                           join b in _dataContext.credit_creditriskscorecard on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                           where a.Deleted == false
                                           select new MappedCreditRiskAttibuteObj
                                           {
                                               CreditRiskAttributeId = a.CreditRiskAttributeId,
                                               CreditRiskAttribute = a.CreditRiskAttribute,
                                               CustomerTypeId = b.CustomerTypeId,

                                           }).ToList();
                var result = creditRiskScoreCard.GroupBy(c => c.CreditRiskAttributeId).Select(v => v.FirstOrDefault()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<SystemAttributeObj> GetAllSystemCreditRiskAttribute()
        {
            try
            {
                var creditRiskScoreCard = (from a in _dataContext.credit_systemattribute
                                           where a.Deleted == false
                                           select new SystemAttributeObj
                                           {
                                               SystemAttributeId = a.SystemAttributeId,
                                               SystemAttributeName = a.SystemAttributeName,
                                           }).ToList();
                return creditRiskScoreCard;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ApplicationCreditRiskAttibuteObj> GetApplicationCreditRiskAttribute(int loanApplicatonId)
        {
            try
            {
                List<ApplicationCreditRiskAttibuteObj> resultList = new List<ApplicationCreditRiskAttibuteObj>();
                var loanApp = _dataContext.credit_loanapplication.Find(loanApplicatonId);
                var customer = _dataContext.credit_loancustomer.Find(loanApp.CustomerId);
                var maritalStatusList = _serverRequest.GetMaritalStatusAsync().Result;
                var genderList = _serverRequest.GetGenderAsync().Result;

                var today = DateTime.Today;
                DateTime birthdate = DateTime.Today;
                var age = 1;
                var maritalstatus = "";
                var gender = "";
                decimal? dd = 0;
                if (customer.CustomerTypeId == (int)CustomerType.Individual)
                {
                    birthdate = (DateTime)customer.DOB;
                    age = today.Year - birthdate.Year;
                    if (birthdate.Date > today.AddYears(-age)) age--;
                    maritalstatus = maritalStatusList.commonLookups.FirstOrDefault(x=>x.LookupId == customer.MaritalStatusId)?.LookupName;
                    gender = genderList.commonLookups.FirstOrDefault(x=>x.LookupId == customer.GenderId)?.LookupName;
                    dd = 0;
                }


                if (customer.CustomerTypeId == (int)CustomerType.Individual)
                {
                    var optionList = (from a in _dataContext.credit_creditriskattribute
                                      join b in _dataContext.credit_creditriskscorecard
                                     on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                      join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId equals c.CreditRiskAttributeId
                                      where a.Deleted == false && b.Deleted == false
                                             && c.CustomerTypeId == (int)CustomerType.Individual && c.UseAtOrigination == true
                                             && c.ProductId == loanApp.ProposedProductId && a.CreditRiskCategoryId == (int)CreditRiskCategoryEnum.ApplicationRisk
                                      select new OptionLists
                                      {
                                          CreditRiskAttributeId = a.CreditRiskAttributeId,
                                          Value = b.Value,
                                          Score = b.Score,
                                          MaxScore = c.WeightedScore
                                      }).AsQueryable();

                    var creditRiskCategory = (from z in _dataContext.credit_creditriskattribute
                                   join b in _dataContext.credit_creditriskscorecard
                                   on z.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                   join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId equals c.CreditRiskAttributeId
                                   where z.Deleted == false && b.Deleted == false
                                   && c.CustomerTypeId == (int)CustomerType.Individual && c.UseAtOrigination == true
                                   && c.ProductId == loanApp.ProposedProductId && z.CreditRiskCategoryId == (int)CreditRiskCategoryEnum.ApplicationRisk
                                   orderby z.CreditRiskAttributeId
                                   select new ApplicationCreditRiskAttibuteObj
                                   {
                                       FieldName = c.FeildName,
                                       FieldValue = 0,
                                       FieldId = z.CreditRiskAttributeId,
                                       ShowField = true,
                                       DisableField = false,
                                       LabelName = z.CreditRiskAttribute,
                                       LabelName1 = z.FriendlyName,
                                       Options = optionList.Where(x => x.CreditRiskAttributeId == z.CreditRiskAttributeId).ToList()
                                   }).ToList();


                    var result = creditRiskCategory.GroupBy(x => x.FieldName).Select(p => p.FirstOrDefault());


                    var recordExist = (from a in _dataContext.credit_individualapplicationscorecarddetails
                                       where a.LoanApplicationId == loanApplicatonId
                                       select a).ToList();




                    if (recordExist.Count > 0)
                    {

                        foreach (var item in result)
                        {
                            var scores = recordExist.Where(x => x.AttributeField == item.FieldName).FirstOrDefault();
                            if (scores != null)
                                item.FieldValue = Convert.ToInt32(scores.Score);
                            if (item.LabelName1.Trim().Contains("Age"))
                                item.DisableField = false;
                            if (item.LabelName1.ToLower().Trim().Contains("gender"))
                                item.DisableField = false;
                            if (item.LabelName1.ToLower().Trim().Contains("status"))
                                item.DisableField = false;
                        }
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            if (item.LabelName1.Trim().Contains("Age"))
                            {
                                foreach (var kk in item.Options)
                                {
                                    int range1, range2;
                                    string[] pair = kk.Value.Split('-');
                                    string first = pair[0].Trim();
                                    string second = pair[1].Trim();
                                    var isRange1Numeric = int.TryParse(first, out range1);
                                    var isRange2Numeric = int.TryParse(second, out range2);
                                    if (isRange1Numeric == isRange2Numeric)
                                    {

                                        if (first != null && second != null)
                                        {
                                            if (range1 <= age && range2 >= age)
                                                dd = kk.Score;
                                        }
                                    }
                                    else
                                    {
                                        if (age >= range1) dd = kk.Score;
                                    }

                                }
                                item.FieldValue = Convert.ToInt32(dd);
                                item.DisableField = false;
                            }
                            if (item.LabelName1.ToLower().Trim().Contains("gender"))
                            {
                                item.FieldValue = item.FieldValue = Convert.ToInt32(item.Options.Where(x => x.Value.ToLower() == gender.ToLower()).FirstOrDefault()?.Score);
                                item.DisableField = false;
                            }
                            if (item.LabelName1.ToLower().Trim().Contains("status"))
                            {
                                item.FieldValue = Convert.ToInt32(item.Options.Where(x => x.Value.ToLower() == maritalstatus.ToLower()).FirstOrDefault()?.Score);
                                item.DisableField = false;
                            }
                        }
                    }

                    var Behavioural = BehaviouralRisk(loanApp.ProposedProductId, (int)CustomerType.Individual);

                    resultList.AddRange(result);
                    if (Behavioural.Count() > 0)
                        resultList.AddRange(Behavioural);

                    var attribute = resultList.Count();

                    string fieldName = "";
                    if (attribute > 0)
                    {
                        fieldName = "Field" + (attribute + 1);

                    }


                    var option = new List<OptionLists>();
                    option.Add(new OptionLists { Value = "Yes", Score = 1 });
                    option.Add(new OptionLists { Value = "No", Score = 0 });
                    var defaulted = new ApplicationCreditRiskAttibuteObj
                    {
                        FieldName = fieldName,
                        FieldValue = 0,
                        ShowField = false,
                        LabelName = "Defaulted Customer",
                        Options = option
                    };


                    resultList.Add(defaulted);
                    foreach (var item in resultList)
                    {
                        if (item.FieldValue == null)
                        {
                            item.FieldValue = 0;
                        }
                    }
                    return resultList;
                }
                else
                {
                    var optionList = (from a in _dataContext.credit_creditriskattribute
                                      join b in _dataContext.credit_creditriskscorecard
                                     on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                      join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId equals c.CreditRiskAttributeId
                                      where a.Deleted == false && b.Deleted == false
                                             && c.CustomerTypeId == (int)CustomerType.Corporate && c.UseAtOrigination == true
                                             && c.ProductId == loanApp.ProposedProductId && a.CreditRiskCategoryId == (int)CreditRiskCategoryEnum.ApplicationRisk
                                      select new OptionLists
                                     {
                                         CreditRiskAttributeId = a.CreditRiskAttributeId,
                                         Value = b.Value,
                                         Score = b.Score,
                                         MaxScore = c.WeightedScore
                                     }).AsQueryable();

           var creditRiskCategory = (from a in _dataContext.credit_creditriskattribute
                                              join b in _dataContext.credit_creditriskscorecard
                                             on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                             //into app
                                             // from b in app.DefaultIfEmpty()
                                              join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId
                                              equals c.CreditRiskAttributeId
                                              where a.Deleted == false && b.Deleted == false
                                              && c.CustomerTypeId == (int)CustomerType.Corporate && c.UseAtOrigination == true
                                              && c.ProductId == loanApp.ProposedProductId && a.CreditRiskCategoryId == (int)CreditRiskCategoryEnum.ApplicationRisk
                                              orderby a.CreditRiskAttributeId
                                              select new ApplicationCreditRiskAttibuteObj
                                              {
                                                  FieldName = c.FeildName,
                                                  FieldValue = 0,
                                                  FieldId = a.CreditRiskAttributeId,
                                                  ShowField = true,
                                                  DisableField = false,
                                                  LabelName = a.CreditRiskAttribute,
                                                  LabelName1 = a.FriendlyName,
                                                  Options = optionList.Where(x=>x.CreditRiskAttributeId == a.CreditRiskAttributeId).ToList()
                                                  //Options = app.Select(x => new OptionLists
                                                  //{
                                                  //    Value = x.Value,
                                                  //    Score = x.Score,
                                                  //    MaxScore = c.WeightedScore
                                                  //}).ToList()
                                              }).ToList();
                    var result = creditRiskCategory.GroupBy(x => x.FieldName).Select(p => p.FirstOrDefault());

                    var recordExist = (from a in _dataContext.credit_corporateapplicationscorecarddetails
                                       where a.LoanApplicationId == loanApplicatonId
                                       select a).ToList();
                    if (recordExist.Count > 0)
                    {

                        foreach (var item in result)
                        {
                            var scores = recordExist.Where(x => x.AttributeField == item.FieldName).FirstOrDefault();
                            if (scores != null)
                                item.FieldValue = Convert.ToInt32(scores.Score);
                        }
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            if (item.LabelName.ToLower().Trim().Contains("date"))
                            {
                                foreach (var kk in item.Options)
                                {
                                    int range1, range2;
                                    string[] pair = kk.Value.Split('-');
                                    string first = pair[0].Trim();
                                    string second = pair[1].Trim();
                                    var isRange1Numeric = int.TryParse(first, out range1);
                                    var isRange2Numeric = int.TryParse(second, out range2);
                                    if (isRange1Numeric == isRange2Numeric)
                                    {

                                        if (first != null && second != null)
                                        {
                                            if (range1 <= age && range2 >= age)
                                                dd = kk.Score;
                                        }
                                    }
                                    else
                                    {
                                        if (age >= range1) dd = kk.Score;
                                    }

                                }
                                item.FieldValue = Convert.ToInt32(dd);
                            }
                        }

                    }

                    var Behavioural = BehaviouralRisk(loanApp.ProposedProductId, (int)CustomerType.Corporate);

                    resultList.AddRange(result);
                    if (Behavioural.Count() > 0)
                        resultList.AddRange(Behavioural);

                    var attribute = resultList.Count();

                    string fieldName = "";
                    if (attribute > 0)
                    {
                        fieldName = "Field" + (attribute + 1);

                    }


                    var option = new List<OptionLists>();
                    option.Add(new OptionLists { Value = "Yes", Score = 1 });
                    option.Add(new OptionLists { Value = "No", Score = 0 });
                    var defaulted = new ApplicationCreditRiskAttibuteObj
                    {
                        FieldName = fieldName,
                        FieldValue = 0,
                        ShowField = false,
                        LabelName = "Defaulted Customer",
                        Options = option
                    };

                    resultList.Add(defaulted);
                    foreach(var item in resultList)
                    {
                        if (item.FieldValue == null)
                        {
                            item.FieldValue = 0;
                        }
                    }
                    return resultList;
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditBureauObj GetCreditBureau(int creditBureauId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditbureau
                                  where a.Deleted == false && a.CreditBureauId == creditBureauId

                                  select new CreditBureauObj
                                  {
                                      CreditBureauId = a.CreditBureauId,
                                      CorporateChargeAmount = a.CorporateChargeAmount,
                                      CreditBureauName = a.CreditBureauName,
                                      GLAccountId = a.GLAccountId,
                                      IndividualChargeAmount = a.IndividualChargeAmount,
                                      IsMandatory = a.IsMandatory,
                                  }).FirstOrDefault();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditRiskAttibuteObj GetCreditRiskAttribute(int creditRiskAttributeId)
        {
            try
            {
                //var creditRiskScoreCards =  "",

                var creditRiskScoreCard = (from a in _dataContext.credit_creditriskattribute
                                           where a.Deleted == false && a.CreditRiskAttributeId == creditRiskAttributeId

                                           select new CreditRiskAttibuteObj
                                           {
                                               CreditRiskAttributeId = a.CreditRiskAttributeId,
                                               CreditRiskCategoryId = a.CreditRiskCategoryId,
                                               CreditRiskCategoryName = _dataContext.credit_creditriskcategory.FirstOrDefault(x => x.CreditRiskCategoryId == a.CreditRiskCategoryId).CreditRiskCategoryName,
                                               CreditRiskAttribute = a.CreditRiskAttribute,
                                               AttributeField = a.AttributeField,
                                               SystemAttribute = a.FriendlyName,
                                           }).FirstOrDefault();
                return creditRiskScoreCard;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditRiskCategoryObj GetCreditRiskCategory(int creditRiskCategoryId)
        {
            try
            {
                var creditRiskCategory = (from a in _dataContext.credit_creditriskcategory
                                          where a.Deleted == false && a.CreditRiskCategoryId == creditRiskCategoryId

                                          select new CreditRiskCategoryObj
                                          {
                                              CategoryId = a.CreditRiskCategoryId,
                                              CategoryName = a.CreditRiskCategoryName,
                                              Description = a.Description,
                                              //useInOrigination = a.UseInOrigination
                                          }).FirstOrDefault();
                return creditRiskCategory;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditRiskRatingObj GetCreditRiskRating(int creditRiskRatingId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditrating
                                  where a.Deleted == false && a.CreditRiskRatingId == creditRiskRatingId

                                  select new CreditRiskRatingObj
                                  {
                                      CreditRiskRatingId = a.CreditRiskRatingId,
                                      Rate = a.Rate,
                                      MinRange = a.MinRange,
                                      MaxRange = a.MaxRange,
                                      AdvicedRange = a.AdvicedRange,
                                      RateDescription = a.RateDescription,
                                  }).FirstOrDefault();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditRiskRatingDetailObj GetCreditRiskRatingDetail(int loanApplicationId)
        {
            try
            {
                string rate = "N/A";
                var creditRisk = (from a in _dataContext.credit_loanapplication
                                  where a.Deleted == false && a.LoanApplicationId == loanApplicationId
                                  select a
                                 ).FirstOrDefault();
                if (creditRisk != null)
                {
                    decimal pd = 0;
                    var riskRating = _dataContext.credit_creditrating.ToList();
                    if (riskRating.Count > 0)
                    {
                        pd = Convert.ToDecimal(creditRisk.PD);
                        var ratedPD = riskRating.Where(x => x.MinRange < pd && x.MaxRange > pd).FirstOrDefault();
                        if (ratedPD != null)
                        {
                            rate = ratedPD.Rate;
                        }
                    }
                }
                return new CreditRiskRatingDetailObj
                {
                    CreditScore = creditRisk.Score,
                    ProbabilityOfDefault = creditRisk.PD,
                    ProductWeighterScore = _dataContext.credit_product.Where(x=>x.ProductId == creditRisk.ApprovedProductId).FirstOrDefault().WeightedMaxScore,//creditRisk.credit_product.WeightedMaxScore,
                    CreditRating = rate
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditRiskRatingPDObj GetCreditRiskRatingPD(int creditRiskRatingPDId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_creditratingpd
                                  where a.Deleted == false && a.CreditRiskRatingPDId == creditRiskRatingPDId

                                  select new CreditRiskRatingPDObj
                                  {
                                      CreditRiskRatingPDId = a.CreditRiskRatingPDId,
                                      Pd = a.PD,
                                      MinRange = a.MinRangeScore,
                                      MaxRange = a.MaxRangeScore,
                                      Description = a.Description,
                                      InterestRate = a.InterestRate,
                                      ProductId = a.ProductId,
                                      ProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName
                                  }).FirstOrDefault();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskScoreCardObj> GetCreditRiskScoreCard(int creditRiskAttributeId)
        {
            try
            {
                var creditRiskScoreCard = (from a in _dataContext.credit_creditriskscorecard
                                               //where a.Deleted == false && a.CreditRiskAttributeId == creditRiskAttributeId
                                           where a.CreditRiskAttributeId == creditRiskAttributeId

                                           select new CreditRiskScoreCardObj
                                           {
                                               CreditRiskAttributeId = a.CreditRiskAttributeId,
                                               CreditRiskAttributeName = _dataContext.credit_creditriskattribute.FirstOrDefault(x => x.CreditRiskAttributeId == a.CreditRiskAttributeId).CreditRiskAttribute,
                                               CustomerTypeId = a.CustomerTypeId,
                                               CustomerTypeName = a.CustomerTypeId == (int)CustomerType.Corporate ? "Corporate" : "Individual",
                                               Value = a.Value,
                                               Score = a.Score,
                                               CreditRiskScoreCardId = a.CreditRiskScoreCardId
                                           }).ToList();
                return creditRiskScoreCard;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditWeightedRiskScoreObj> GetCreditWeightedRiskScore(int productId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_weightedriskscore
                                  where a.Deleted == false && a.ProductId == productId

                                  select new CreditWeightedRiskScoreObj
                                  {
                                      WeightedRiskScoreId = a.WeightedRiskScoreId,
                                      CreditRiskAttributeId = a.CreditRiskAttributeId,
                                      AttributeName = _dataContext.credit_creditriskattribute.FirstOrDefault(x=>x.CreditRiskAttributeId == a.CreditRiskAttributeId).CreditRiskAttribute,//a.credit_creditriskattribute.CreditRiskAttribute,
                                      ProductId = a.ProductId,
                                      CustomerTypeId = a.CustomerTypeId,
                                      CustomerTypeName = (int)CustomerType.Corporate == a.CustomerTypeId ? "Corporate" : "Individual",
                                      ProductName = _dataContext.credit_product.FirstOrDefault(x=>x.ProductId == a.ProductId).ProductName,//a.credit_product.ProductName,
                                      WeightedScore = a.WeightedScore,
                                      ProductMaxWeight = a.ProductMaxWeight,
                                      UseAtOrigination = a.UseAtOrigination,
                                  }).ToList();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditWeightedRiskScoreObj> GetCreditWeightedRiskScoreByCustomerType(int productId, int customerTypeId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_weightedriskscore
                                  where a.Deleted == false && a.ProductId == productId && a.CustomerTypeId == customerTypeId

                                  select new CreditWeightedRiskScoreObj
                                  {
                                      WeightedRiskScoreId = a.WeightedRiskScoreId,
                                      CreditRiskAttributeId = a.CreditRiskAttributeId,
                                      AttributeName = a.credit_creditriskattribute.CreditRiskAttribute,
                                      ProductId = a.ProductId,
                                      CustomerTypeId = a.CustomerTypeId,
                                      CustomerTypeName = (int)CustomerType.Corporate == a.CustomerTypeId ? "Corporate" : "Individual",
                                      ProductName = a.credit_product.ProductName,
                                      WeightedScore = a.WeightedScore,
                                      ProductMaxWeight = a.ProductMaxWeight,
                                      UseAtOrigination = a.UseAtOrigination,
                                  }).ToList();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditRiskScoreCardObj> GetDistinctAttribute()
        {
            try
            {
                var registry = (from a in _dataContext.credit_creditriskscorecard
                                where a.Deleted == false
                                select new CreditRiskScoreCardObj

                                {
                                    CreditRiskAttributeName = _dataContext.credit_creditriskattribute.
                                    Where(x => x.CreditRiskAttributeId == a.CreditRiskAttributeId)
                                    .FirstOrDefault().CreditRiskAttribute
                                }).Distinct().ToList();
                return registry;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<GroupedCreditRiskAttibuteObj> GetGroupedAttribute()
        {
            try
            {
                var list = (from a in _dataContext.credit_creditriskscorecard
                            where a.Deleted == false

                            select new CreditRiskScoreCardObj
                            {
                                CreditRiskAttributeId = a.CreditRiskAttributeId,
                                CreditRiskAttributeName = _dataContext.credit_creditriskattribute.FirstOrDefault(x => x.CreditRiskAttributeId == a.CreditRiskAttributeId).CreditRiskAttribute,
                                CustomerTypeId = a.CustomerTypeId,
                                CustomerTypeName = a.CustomerTypeId == (int)CustomerType.Corporate ? "Corporate" : "Individual",
                                Value = a.Value,
                                Score = a.Score,
                                CreditRiskScoreCardId = a.CreditRiskScoreCardId
                            }).ToList();


                var data = list.GroupBy(x => x.CreditRiskAttributeId).Select(p => p.FirstOrDefault());
                List<GroupedCreditRiskAttibuteObj> result = new List<GroupedCreditRiskAttibuteObj>();
                foreach (var item in data)
                {
                    var line = new GroupedCreditRiskAttibuteObj
                    {
                        data = new CreditRiskScoreCardObj
                        {
                            CreditRiskAttributeId = item.CreditRiskAttributeId,
                            CreditRiskAttributeName = item.CreditRiskAttributeName,
                            CustomerTypeId = item.CustomerTypeId,
                            CustomerTypeName = "",
                            Value = "",
                            Score = 0,
                            CreditRiskScoreCardId = item.CreditRiskScoreCardId
                        },
                        children = list.Where(x => x.CreditRiskAttributeId == item.CreditRiskAttributeId).Select(k => new ChildrenDataObj
                        {
                            data = new CreditRiskScoreCardObj
                            {
                                CreditRiskAttributeId = k.CreditRiskAttributeId,
                                CreditRiskAttributeName = k.CreditRiskAttributeName,
                                CustomerTypeId = k.CustomerTypeId,
                                CustomerTypeName = k.CustomerTypeName,
                                Value = k.Value,
                                Score = k.Score,
                                CreditRiskScoreCardId = k.CreditRiskScoreCardId
                            }
                        }).ToList()
                    };
                    result.Add(line);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<GroupedCreditRiskRatingPDObj> GetGroupedCreditRiskRatingPD()
        {
            try
            {
                var list = (from a in _dataContext.credit_creditratingpd
                            where a.Deleted == false

                            select new CreditRiskRatingPDObj
                            {
                                Pd = a.PD,
                                ProductId = a.ProductId ?? 0,
                                CreditRiskRatingPDId = a.CreditRiskRatingPDId,
                                MinRange = a.MinRangeScore,
                                MaxRange = a.MaxRangeScore,
                                Description = a.Description,
                                InterestRate = a.InterestRate,
                                ProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProductId && x.Deleted == false).FirstOrDefault().ProductName,
                            }).ToList();


                var data = list.GroupBy(x => x.ProductId).Select(p => p.FirstOrDefault());
                List<GroupedCreditRiskRatingPDObj> result = new List<GroupedCreditRiskRatingPDObj>();
                foreach (var item in data)
                {
                    var line = new GroupedCreditRiskRatingPDObj
                    {
                        data = new CreditRiskRatingPDObj
                        {
                            Pd = item.Pd,
                            CreditRiskRatingPDId = item.CreditRiskRatingPDId,
                            //minRange = 0,
                            //maxRange = 0,
                            Description = "",
                            //interestRate = 0,
                            ProductName = _dataContext.credit_product.Where(x => x.ProductId == item.ProductId && x.Deleted == false).FirstOrDefault().ProductName,
                        },
                        children = list.Where(x => x.ProductId == item.ProductId).Select(k => new ChildrenObj
                        {
                            data = new CreditRiskRatingPDObj
                            {
                                Pd = k.Pd,
                                CreditRiskRatingPDId = k.CreditRiskRatingPDId,
                                MinRange = k.MinRange,
                                MaxRange = k.MaxRange,
                                Description = k.Description,
                                InterestRate = k.InterestRate,
                                ProductName = _dataContext.credit_product.Where(x => x.ProductId == k.ProductId && x.Deleted == false).FirstOrDefault().ProductName,
                            }
                        }).ToList()
                    };
                    result.Add(line);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCreditBureauObj GetLoanApplicationCreditBureau(int loanApplicationId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_loancreditbureau
                                  where a.Deleted == false && a.LoanApplicationId == loanApplicationId

                                  select new LoanCreditBureauObj
                                  {
                                      LoanCreditBureauId = a.LoanCreditBureauId,
                                      LoanApplicationId = a.LoanApplicationId,
                                      LoanApplicationRefNo = a.credit_loanapplication.ApplicationRefNumber,
                                      CreditBureauId = a.CreditBureauId,
                                      CreditBureauName = a.credit_creditbureau.CreditBureauName,
                                      ChargeAmount = a.ChargeAmount,
                                      ReportStatus = a.ReportStatus,
                                      SupportDocument = a.SupportDocument,

                                  }).FirstOrDefault();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCreditBureauObj GetLoanCreditBureau(int loanCreditBureauId)
        {
            try
            {
                var creditRisk = (from a in _dataContext.credit_loancreditbureau
                                  where a.Deleted == false && a.LoanCreditBureauId == loanCreditBureauId

                                  select new LoanCreditBureauObj
                                  {
                                      LoanCreditBureauId = a.LoanCreditBureauId,
                                      LoanApplicationId = a.LoanApplicationId,
                                      LoanApplicationRefNo = a.credit_loanapplication.ApplicationRefNumber,
                                      CreditBureauId = a.CreditBureauId,
                                      CreditBureauName = a.credit_creditbureau.CreditBureauName,
                                      ChargeAmount = a.ChargeAmount,
                                      ReportStatus = a.ReportStatus,
                                      SupportDocument = a.SupportDocument,

                                  }).FirstOrDefault();
                return creditRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadCreditBureau(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<CreditBureauObj> uploadedRecord = new List<CreditBureauObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new CreditBureauObj
                                {
                                    CreditBureauName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    CorporateChargeAmount = workSheet.Cells[i, 2].Value != null ? decimal.Parse(workSheet.Cells[i, 2].Value.ToString()) : 0,
                                    IndividualChargeAmount = workSheet.Cells[i, 3].Value != null ? decimal.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                                    IsMandatory = workSheet.Cells[i, 4].Value != null ? bool.Parse(workSheet.Cells[i, 4].Value.ToString()) : false,
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                List<credit_creditbureau> creditBureaux = new List<credit_creditbureau>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var accountTypeExist = _dataContext.credit_creditbureau.Where(x => x.CreditBureauName.ToLower() == item.CreditBureauName.ToLower()).FirstOrDefault();
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.CreditBureauName = item.CreditBureauName;
                            accountTypeExist.CorporateChargeAmount = item.CorporateChargeAmount;
                            accountTypeExist.IndividualChargeAmount = item.IndividualChargeAmount;
                            accountTypeExist.IsMandatory = item.IsMandatory;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = item.UpdatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {

                            var accountType = new credit_creditbureau
                            {
                                CreditBureauId = item.CreditBureauId,
                                CreditBureauName = item.CreditBureauName,
                                CorporateChargeAmount = item.CorporateChargeAmount,
                                IndividualChargeAmount = item.IndividualChargeAmount,
                                IsMandatory = item.IsMandatory,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,

                            };
                            _dataContext.credit_creditbureau.Add(accountType);
                        }
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadCreditRiskAttribute(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<CreditRiskAttibuteObj> uploadedRecord = new List<CreditRiskAttibuteObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new CreditRiskAttibuteObj
                                {
                                    CreditRiskAttribute = workSheet.Cells[i, 1].Value.ToString(),
                                    CreditRiskCategoryName = workSheet.Cells[i, 2].Value.ToString(),
                                    SystemAttribute = workSheet.Cells[i, 3].Value.ToString()
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        if (item.SystemAttribute == "Others")
                        {
                            item.SystemAttribute = item.CreditRiskAttribute;
                        }
                        else
                        {
                            item.SystemAttribute = item.SystemAttribute;
                        }
                        item.CreditRiskCategoryId = _dataContext.credit_creditriskcategory.Where(x => x.CreditRiskCategoryName == item.CreditRiskCategoryName).FirstOrDefault().CreditRiskCategoryId;
                        var category = _dataContext.credit_creditriskattribute.Where(x => x.CreditRiskAttribute == item.CreditRiskAttribute && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.CreditRiskCategoryId = item.CreditRiskCategoryId;
                            category.CreditRiskAttribute = item.CreditRiskAttribute;
                            category.FriendlyName = item.SystemAttribute;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var attribute = (from a in _dataContext.credit_creditriskattribute
                                             orderby a.CreatedOn descending
                                             select a.AttributeField).FirstOrDefault();
                            string fieldName = "";
                            if (attribute != null)
                            {
                                int lastNumber = int.Parse(attribute.Substring(5));
                                fieldName = "Field" + (lastNumber + 1);

                            }
                            else
                            {
                                fieldName = "Field1";
                            }
                            var structure = new credit_creditriskattribute
                            {
                                CreditRiskAttributeId = item.CreditRiskAttributeId,
                                CreditRiskCategoryId = item.CreditRiskCategoryId,
                                CreditRiskAttribute = item.CreditRiskAttribute,
                                FriendlyName = item.SystemAttribute,
                                AttributeField = fieldName,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            //structures.Add(structure);
                            _dataContext.credit_creditriskattribute.Add(structure);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadCreditRiskRate(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<CreditRiskRatingObj> uploadedRecord = new List<CreditRiskRatingObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new CreditRiskRatingObj
                                {
                                    Rate = workSheet.Cells[i, 1].Value.ToString(),
                                    MinRange = decimal.Parse(workSheet.Cells[i, 2].Value.ToString()),
                                    MaxRange = decimal.Parse(workSheet.Cells[i, 3].Value.ToString()),
                                    AdvicedRange = decimal.Parse(workSheet.Cells[i, 4].Value.ToString()),
                                    RateDescription = workSheet.Cells[i, 5].Value.ToString()
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.credit_creditrating.Where(x => x.Rate == item.Rate && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.Rate = item.Rate;
                            category.MinRange = item.MinRange;
                            category.MaxRange = item.MaxRange;
                            category.AdvicedRange = item.AdvicedRange;
                            category.RateDescription = item.RateDescription;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new credit_creditrating
                            {
                                CreditRiskRatingId = item.CreditRiskRatingId,
                                Rate = item.Rate,
                                MinRange = item.MinRange,
                                MaxRange = item.MaxRange,
                                AdvicedRange = item.AdvicedRange,
                                RateDescription = item.RateDescription,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.credit_creditrating.Add(structure);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadCreditRiskRatingPD(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<CreditRiskRatingPDObj> uploadedRecord = new List<CreditRiskRatingPDObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new CreditRiskRatingPDObj
                                {
                                    Pd = decimal.Parse(workSheet.Cells[i, 1].Value.ToString()),
                                    MinRange = decimal.Parse(workSheet.Cells[i, 2].Value.ToString()),
                                    MaxRange = decimal.Parse(workSheet.Cells[i, 3].Value.ToString()),
                                    Description = workSheet.Cells[i, 4].Value.ToString(),
                                    InterestRate = decimal.Parse(workSheet.Cells[i, 5].Value.ToString()),
                                    ProductName = workSheet.Cells[i, 6].Value.ToString(),
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        if (entity.ProductName == "")
                        {
                            throw new Exception("Please include a valid Product Name");
                        }

                        var product = _dataContext.credit_product.Where(x => x.ProductName == entity.ProductName && x.Deleted == false).FirstOrDefault();
                        if (product == null)
                        {
                            throw new Exception("Please include a valid Product Name");
                        }


                        var riskRatingExist = _dataContext.credit_creditratingpd.Find(entity.ProductId);
                        if (riskRatingExist != null)
                        {
                            riskRatingExist.PD = entity.Pd;
                            riskRatingExist.MinRangeScore = entity.MinRange;
                            riskRatingExist.MaxRangeScore = entity.MaxRange;
                            riskRatingExist.Description = entity.Description;
                            riskRatingExist.InterestRate = entity.InterestRate;
                            riskRatingExist.ProductId = product.ProductId;
                            riskRatingExist.Active = true;
                            riskRatingExist.Deleted = false;
                            riskRatingExist.UpdatedBy = entity.CreatedBy;
                            riskRatingExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new credit_creditratingpd
                            {
                                PD = entity.Pd,
                                MinRangeScore = entity.MinRange,
                                MaxRangeScore = entity.MaxRange,
                                Description = entity.Description,
                                InterestRate = entity.InterestRate,
                                ProductId = product.ProductId,
                                Active = true,
                                Deleted = false,
                                UpdatedBy = entity.CreatedBy,
                                UpdatedOn = DateTime.Now,
                            };
                            _dataContext.credit_creditratingpd.Add(accountType);
                        }
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /////////////////PRIVATE METHODS
        private void CalculateIndividualScoreCard(int customerId, int loanApplicationId)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("spp_CalculateIndividualCustomerLoanScoreCard", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CustomerId",
                    Value = customerId,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanApplicationId",
                    Value = loanApplicationId
                });

                cmd.CommandTimeout = 0;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }

        }
        private void CalculateCorperateScoreCard(int customerId, int loanApplicationId)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("spp_CalculateCorperateCustomerLoanScoreCard", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CustomerId",
                    Value = customerId,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanApplicationId",
                    Value = loanApplicationId
                });

                cmd.CommandTimeout = 0;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        private IEnumerable<ApplicationCreditRiskAttibuteObj> BehaviouralRisk(int productId, int customerTypeId)
        {
            var istList = _dataContext.credit_creditriskscorecard.Where(x =>  _dataContext.credit_creditriskattribute.Select(x => x.CreditRiskAttributeId).Contains(x.CreditRiskAttributeId));

            var creditRiskCategory = (from a in _dataContext.credit_creditriskattribute
                                      join b in _dataContext.credit_creditriskscorecard
                                     on a.CreditRiskAttributeId equals b.CreditRiskAttributeId

                                      join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId
                                      equals c.CreditRiskAttributeId
                                      where a.Deleted == false && b.Deleted == false
                                      && c.CustomerTypeId == customerTypeId && c.UseAtOrigination == true
                                      && c.ProductId == productId && a.CreditRiskCategoryId == (int)CreditRiskCategoryEnum.BehaviouralRisk
                                      orderby a.CreditRiskAttributeId
                                      select new ApplicationCreditRiskAttibuteObj
                                      {
                                          FieldName = c.FeildName,
                                          FieldValue = Convert.ToInt32(istList.Where(x=>x.CreditRiskAttributeId == c.CreditRiskAttributeId).Max(x => x.Score)),
                                          FieldId = a.CreditRiskAttributeId,
                                          ShowField = false,
                                          LabelName = a.CreditRiskAttribute,
                                      }).ToList();
            var result = creditRiskCategory.GroupBy(x => x.FieldName).Select(p => p.FirstOrDefault());

            return result.ToList();
        }
    }
}
