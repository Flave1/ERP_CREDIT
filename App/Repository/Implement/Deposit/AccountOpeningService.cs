using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Deposit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Helpers;
using Banking.Repository.Interface.Deposit;
using Banking.Requests;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement.Deposit
{
    public class AccountOpeningService : IAccountOpeningService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public AccountOpeningService(DataContext dataContext, 
            IIdentityService identityService, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region CustomerDetails
        private IQueryable<DepositAccountOpeningObj> GetCustomerAccount(int CustomerId)
        {
            return from a in _dataContext.deposit_accountopening
                   where a.Deleted == false && a.CustomerId == CustomerId
                   select

                  new DepositAccountOpeningObj
                  {
                      CustomerId = a.CustomerId,
                      CustomerTypeId = a.CustomerTypeId,
                      AccountTypeId = a.AccountTypeId,
                      AccountCategoryId = a.AccountCategoryId,
                      AccountNumber = a.AccountNumber,
                      Title = a.Title,
                      Surname = a.Surname,
                      Firstname = a.Firstname,
                      Othername = a.Othername,
                      MaritalStatusId = a.MaritalStatusId,
                      RelationshipOfficerId = a.RelationshipOfficerId,
                      GenderId = a.GenderId,
                      BirthCountryId = a.BirthCountryId,
                      DOB = a.DOB,
                      MotherMaidenName = a.MotherMaidenName,
                      TaxIDNumber = a.TaxIDNumber,
                      BVN = a.BVN,
                      Nationality = a.Nationality,
                      ResidentPermitNumber = a.ResidentPermitNumber,
                      PermitIssueDate = a.PermitIssueDate,
                      PermitExpiryDate = a.PermitExpiryDate,
                      SocialSecurityNumber = a.SocialSecurityNumber,
                      StateOfOrigin = a.StateOfOrigin,
                      LocalGovernment = a.LocalGovernment,
                      ResidentOfCountry = a.ResidentOfCountry,
                      Address1 = a.Address1,
                      Address2 = a.Address2,
                      City = a.City,
                      StateId = a.StateId,
                      CountryId = a.CountryId,
                      Email = a.Email,
                      MailingAddress = a.MailingAddress,
                      MobileNumber = a.MobileNumber,
                      InternetBanking = a.InternetBanking,
                      EmailStatement = a.EmailStatement,
                      Card = a.Card,
                      SmsAlert = a.SmsAlert,
                      EmailAlert = a.EmailAlert,
                      Token = a.Token,
                      EmploymentType = a.EmploymentType,
                      EmployerName = a.EmployerName,
                      EmployerAddress = a.EmployerAddress,
                      EmployerState = a.EmployerState,
                      Occupation = a.Occupation,
                      BusinessName = a.BusinessName,
                      BusinessAddress = a.BusinessAddress,
                      BusinessState = a.BusinessState,
                      JobTitle = a.JobTitle,
                      Other = a.Other,
                      DeclarationDate = a.DeclarationDate,
                      DeclarationCompleted = a.DeclarationCompleted,
                      SoleSignatory = a.SoleSignatory,
                      MaxNoOfSignatory = a.MaxNoOfSignatory,
                      RegistrationNumber = a.RegistrationNumber,
                      Industry = a.Industry,
                      Jurisdiction = a.Jurisdiction,
                      Website = a.Website,
                      NatureOfBusiness = a.NatureOfBusiness,
                      AnnualRevenue = a.AnnualRevenue,
                      IsStockExchange = a.IsStockExchange,
                      Stock = a.Stock,
                      RegisteredAddress = a.RegisteredAddress,
                      ScumlNumber = a.ScumlNumber,

                      Identification = _dataContext.deposit_customeridentification.Where(x => x.CustomerId == a.CustomerId).Select(x => new CustomerIdentificationObj()
                      {
                          CustomerIdentityId = x.CustomerIdentityId,
                          CustomerId = x.CustomerId,
                          //Identification = _dataContext.cor_identification.FirstOrDefault(b => b.IdentificationId == x.MeansOfID).IdentificationName,
                          IDNumber = x.IDNumber,
                          DateIssued = x.DateIssued,
                          ExpiryDate = x.ExpiryDate

                      }).ToList(),

                      Kyc = _dataContext.deposit_customerkyc.Where(x => x.CustomerId == a.CustomerId).Select(x => new KyCustomerObj()
                      {
                          KycId = x.KycId,
                          CustomerId = x.CustomerId,
                          Financiallydisadvantaged = x.Financiallydisadvantaged,
                          Bankpolicydocuments = x.Bankpolicydocuments,
                          TieredKycrequirement = x.TieredKycrequirement,
                          RiskCategoryId = x.RiskCategoryId,
                          PoliticallyExposedPerson = x.PoliticallyExposedPerson,
                          Details = x.Details,
                          AddressVisited = x.AddressVisited,
                          CommentOnLocation = x.CommentOnLocation,
                          LocationColor = x.LocationColor,
                          LocationDescription = x.LocationDescription,
                          NameOfVisitingStaff = x.NameOfVisitingStaff,
                          DateOfVisitation = x.DateOfVisitation,
                          UtilityBillSubmitted = x.UtilityBillSubmitted,
                          AccountOpeningCompleted = x.AccountOpeningCompleted,
                          RecentPassportPhoto = x.RecentPassportPhoto,
                          ConfirmationName = x.ConfirmationName,
                          ConfirmationDate = x.ConfirmationDate,
                          DeferralFullName = x.DeferralFullName,
                          DeferralDate = x.DeferralDate,
                          DeferralApproved = x.DeferralApproved,
                      }).ToList(),

                      NextOfKin = _dataContext.deposit_customernextofkin.Where(x => x.CustomerId == a.CustomerId).Select(x => new CustomerNextOfKinObj()
                      {
                          NextOfKinId = x.NextOfKinId,
                          CustomerId = x.NextOfKinId,
                          Title = x.Title,
                          Surname = x.Surname,
                          FirstName = x.FirstName,
                          OtherName = x.OtherName,
                          DOB = x.DOB,
                          GenderId = x.GenderId,
                          Relationship = x.Relationship,
                          MobileNumber = x.MobileNumber,
                          Email = x.Email,
                          Address = x.Address,
                          City = x.City,
                          State = x.State,
                      }).ToList(),

                      Signatory = _dataContext.deposit_customersignatory.Where(x => x.CustomerId == a.CustomerId).Select(x => new CustomerSignatoryObj()
                      {
                          SignatoryId = x.SignatoryId,
                          CustomerId = x.CustomerId,
                          Surname = x.Surname,
                          Firstname = x.Firstname,
                          Othername = x.Othername,
                          ClassofSignatory = x.ClassofSignatory,
                          IdentificationType = x.IdentificationType,
                          IdentificationNumber = x.IdentificationNumber,
                          Telephone = x.Telephone,
                          SignatureUpload = x.SignatureUpload,
                          Date = x.Date,

                      }).ToList(),

                      Signature = _dataContext.deposit_customersignature.Where(x => x.CustomerId == a.CustomerId).Select(x => new CustomerSignatureObj()
                      {
                          SignatureId = x.SignatureId,
                          SignatoryId = x.SignatoryId,
                          CustomerId = x.CustomerId,
                          DocumentName = x.Name,
                          SignatureImg = x.SignatureImg,

                      }).ToList(),

                      Document = _dataContext.deposit_customerkycdocumentupload.Where(x => x.CustomerId == a.CustomerId).Select(x => new KyCustomerDocUploadObj()
                      {
                          DocumentId = x.DocumentId,
                          CustomerId = x.CustomerId,
                          KycId = x.KycId,
                          DocumentName = x.DocumentName,
                          DocumentUpload = x.DocumentUpload,
                          PhysicalLocation = x.PhysicalLocation,

                      }).ToList(),
                  };
        }
        public IEnumerable<CustomerDetailsObj> GetAllCustomerLite()
        {
            try
            {
                var Customer = (from a in _dataContext.deposit_accountopening
                                where a.Deleted == false
                                select

                               new CustomerDetailsObj
                               {
                                   customerId = a.CustomerId,
                                   accountNumber = a.AccountNumber,
                                   firstName = a.Firstname,
                                   lastName = a.Surname,
                                   email = a.Email,
                                   phoneNo = a.MobileNumber,
                                   customerTypeName = _dataContext.deposit_category.FirstOrDefault(c => c.CategoryId == a.CustomerTypeId).Name,
                               }).ToList();

                return Customer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CustomerDetailsObj> GetAllCustomerCasaList()
        {
            try
            {
                var Customer = (from a in _dataContext.deposit_accountopening
                                where a.Deleted == false
                                select

                               new CustomerDetailsObj
                               {
                                   customerId = a.CustomerId,
                                   accountNumber = a.AccountNumber,
                                   firstName = a.Firstname,
                                   lastName = a.Surname,
                                   email = a.Email,
                                   phoneNo = a.MobileNumber,
                                   customerTypeName = _dataContext.deposit_category.FirstOrDefault(c => c.CategoryId == a.CustomerTypeId).Name,
                                   availableBalance = _dataContext.credit_casa.FirstOrDefault(x=>x.AccountNumber == a.AccountNumber).AvailableBalance,
                               }).ToList();

                return Customer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DepositAccountOpeningObj GetCustomerDetails(int CustomerId)
        {
            try
            {
                var Customer = GetCustomerAccount(CustomerId).Where(a => a.CustomerId == CustomerId).FirstOrDefault();

                return Customer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> AddUpdateCustomerAsync(DepositAccountOpeningObj entity)
        {
            try
            {
                if (entity == null) return 0;
                int inserted_id = 0;
                deposit_accountopening CustomerExist = null;
                var accountNumber = GeneralHelpers.GenerateRandomDigitCode(10);

                if (entity.CustomerId > 0)
                {
                    CustomerExist = _dataContext.deposit_accountopening.Find(entity.CustomerId);
                    if (CustomerExist != null)
                    {
                        CustomerExist.AccountCategoryId = entity.AccountCategoryId;
                        CustomerExist.AccountTypeId = entity.AccountTypeId;
                        CustomerExist.CustomerTypeId = entity.CustomerTypeId;
                        CustomerExist.AccountNumber = entity.AccountNumber;
                        CustomerExist.Title = entity.Title;
                        CustomerExist.Surname = entity.Surname;
                        CustomerExist.Firstname = entity.Firstname;
                        CustomerExist.Othername = entity.Othername;
                        CustomerExist.MaritalStatusId = entity.MaritalStatusId;
                        CustomerExist.RelationshipOfficerId = entity.RelationshipOfficerId;
                        CustomerExist.GenderId = entity.GenderId;
                        CustomerExist.BirthCountryId = entity.BirthCountryId;
                        CustomerExist.DOB = entity.DOB;
                        CustomerExist.MotherMaidenName = entity.MotherMaidenName;
                        CustomerExist.TaxIDNumber = entity.TaxIDNumber;
                        CustomerExist.BVN = entity.BVN;
                        CustomerExist.Nationality = entity.Nationality;
                        CustomerExist.ResidentPermitNumber = entity.ResidentPermitNumber;
                        CustomerExist.PermitIssueDate = entity.PermitIssueDate;
                        CustomerExist.PermitExpiryDate = entity.PermitExpiryDate;
                        CustomerExist.SocialSecurityNumber = entity.SocialSecurityNumber;
                        CustomerExist.StateOfOrigin = entity.StateOfOrigin;
                        CustomerExist.LocalGovernment = entity.LocalGovernment;
                        CustomerExist.ResidentOfCountry = entity.ResidentOfCountry;
                        CustomerExist.Address1 = entity.Address1;
                        CustomerExist.Address2 = entity.Address2;
                        CustomerExist.City = entity.City;
                        CustomerExist.StateId = entity.StateId;
                        CustomerExist.CountryId = entity.CountryId;
                        CustomerExist.Email = entity.Email;
                        CustomerExist.MailingAddress = entity.MailingAddress;
                        CustomerExist.MobileNumber = entity.MobileNumber;
                        CustomerExist.InternetBanking = entity.InternetBanking;
                        CustomerExist.EmailStatement = entity.EmailStatement;
                        CustomerExist.Card = entity.Card;
                        CustomerExist.SmsAlert = entity.SmsAlert;
                        CustomerExist.EmailAlert = entity.EmailAlert;
                        CustomerExist.Token = entity.Token;
                        CustomerExist.EmploymentType = entity.EmploymentType;
                        CustomerExist.EmployerName = entity.EmployerName;
                        CustomerExist.EmployerAddress = entity.EmployerAddress;
                        CustomerExist.EmployerState = entity.EmployerState;
                        CustomerExist.Occupation = entity.Occupation;
                        CustomerExist.BusinessName = entity.BusinessName;
                        CustomerExist.BusinessAddress = entity.BusinessAddress;
                        CustomerExist.BusinessState = entity.BusinessState;
                        CustomerExist.JobTitle = entity.JobTitle;
                        CustomerExist.Other = entity.Other;
                        CustomerExist.DeclarationDate = entity.DeclarationDate;
                        CustomerExist.DeclarationCompleted = entity.DeclarationCompleted;
                        CustomerExist.SoleSignatory = entity.SoleSignatory;
                        CustomerExist.MaxNoOfSignatory = entity.MaxNoOfSignatory;
                        CustomerExist.RegistrationNumber = entity.RegistrationNumber;
                        CustomerExist.Industry = entity.Industry;
                        CustomerExist.Jurisdiction = entity.Jurisdiction;
                        CustomerExist.Website = entity.Website;
                        CustomerExist.NatureOfBusiness = entity.NatureOfBusiness;
                        CustomerExist.AnnualRevenue = entity.AnnualRevenue;
                        CustomerExist.IsStockExchange = entity.IsStockExchange;
                        CustomerExist.Stock = entity.Stock;
                        CustomerExist.RegisteredAddress = entity.RegisteredAddress;
                        CustomerExist.ScumlNumber = entity.ScumlNumber;
                        CustomerExist.UpdatedBy = entity.CreatedBy;
                        CustomerExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    CustomerExist = new deposit_accountopening
                    {
                        CustomerId = entity.CustomerId,
                        AccountCategoryId = entity.AccountCategoryId,
                        AccountTypeId = entity.AccountTypeId,
                        CustomerTypeId = entity.CustomerTypeId,
                        AccountNumber = accountNumber,
                        Title = entity.Title,
                        Surname = entity.Surname,
                        Firstname = entity.Firstname,
                        Othername = entity.Othername,
                        MaritalStatusId = entity.MaritalStatusId,
                        RelationshipOfficerId = entity.RelationshipOfficerId,
                        GenderId = entity.GenderId,
                        BirthCountryId = entity.BirthCountryId,
                        DOB = entity.DOB,
                        MotherMaidenName = entity.MotherMaidenName,
                        TaxIDNumber = entity.TaxIDNumber,
                        BVN = entity.BVN,
                        Nationality = entity.Nationality,
                        ResidentPermitNumber = entity.ResidentPermitNumber,
                        PermitIssueDate = entity.PermitIssueDate,
                        PermitExpiryDate = entity.PermitExpiryDate,
                        SocialSecurityNumber = entity.SocialSecurityNumber,
                        StateOfOrigin = entity.StateOfOrigin,
                        LocalGovernment = entity.LocalGovernment,
                        ResidentOfCountry = entity.ResidentOfCountry,
                        Address1 = entity.Address1,
                        Address2 = entity.Address2,
                        City = entity.City,
                        StateId = entity.StateId,
                        CountryId = entity.CountryId,
                        Email = entity.Email,
                        MailingAddress = entity.MailingAddress,
                        MobileNumber = entity.MobileNumber,
                        InternetBanking = entity.InternetBanking,
                        EmailStatement = entity.EmailStatement,
                        Card = entity.Card,
                        SmsAlert = entity.SmsAlert,
                        EmailAlert = entity.EmailAlert,
                        Token = entity.Token,
                        EmploymentType = entity.EmploymentType,
                        EmployerName = entity.EmployerName,
                        EmployerAddress = entity.EmployerAddress,
                        EmployerState = entity.EmployerState,
                        Occupation = entity.Occupation,
                        BusinessName = entity.BusinessName,
                        BusinessAddress = entity.BusinessAddress,
                        BusinessState = entity.BusinessState,
                        JobTitle = entity.JobTitle,
                        Other = entity.Other,
                        DeclarationDate = entity.DeclarationDate,
                        DeclarationCompleted = entity.DeclarationCompleted,
                        SoleSignatory = entity.SoleSignatory,
                        MaxNoOfSignatory = entity.MaxNoOfSignatory,
                        RegistrationNumber = entity.RegistrationNumber,
                        Industry = entity.Industry,
                        Jurisdiction = entity.Jurisdiction,
                        Website = entity.Website,
                        NatureOfBusiness = entity.NatureOfBusiness,
                        AnnualRevenue = entity.AnnualRevenue,
                        IsStockExchange = entity.IsStockExchange,
                        Stock = entity.Stock,
                        RegisteredAddress = entity.RegisteredAddress,
                        ScumlNumber = entity.ScumlNumber,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.deposit_accountopening.Add(CustomerExist);
                }
                using (var trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dataContext.SaveChanges();
                        inserted_id = CustomerExist.CustomerId;
                        updateCASA(entity, inserted_id, accountNumber);
                        trans.Commit();
                        return inserted_id;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_accountype.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        private void updateCASA(DepositAccountOpeningObj entity, int customerId, string accountNumber)
        {
            decimal bal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == entity.AccountTypeId).FirstOrDefault().InitialDeposit;
            decimal Ledgerbal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == entity.AccountTypeId).FirstOrDefault().InitialDeposit;
            decimal? irate = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == entity.AccountTypeId).FirstOrDefault().InterestRate;
            decimal lienBal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == entity.AccountTypeId).FirstOrDefault().InitialDeposit;

            var user = _serverRequest.UserDataAsync().Result;
            credit_casa casaAccount = null;
            casaAccount = _dataContext.credit_casa.Where(x => x.CustomerId == customerId).FirstOrDefault();
            if (casaAccount == null)
            {
                var customerAccount = new credit_casa
                {
                    AccountName =  $"{entity.Firstname}{entity.Surname}",//entity.Firstname + " " + entity.Surname,
                    AccountNumber = accountNumber,
                    AccountStatusId = (int)CASAAccountStatusEnum.Inactive,
                    //ActionBy = entity.actionBy,
                    ActionDate = DateTime.Now,
                    AprovalStatusId = (int)ApprovalStatus.Pending,
                    AvailableBalance = bal,
                    BranchId = 1,
                    CompanyId = 7,
                    CustomerId = customerId,
                    //CustomerSensitivityLevelId = entity.customerSensitivityLevelId,
                    EffectiveDate = DateTime.Now,
                    HasLien = true,
                    HasOverdraft = true,
                    InterestRate = irate,
                    IsCurrentAccount = false,
                    LedgerBalance = Ledgerbal,
                    LienAmount = lienBal,
                    //MISCode = "",
                    OperationId = (int)OperationsEnum.CasaAccountApproval,
                    //OverdraftAmount = 0,
                    //OverdraftExpiryDate = entity.overdraftExpiryDate,
                    //OverdraftInterestRate = 0,
                    //PostNoStatusId = entity.postNoStatusId,
                    ProductId = entity.AccountTypeId,
                    RelationshipManagerId = 0,
                    RelationshipOfficerId = 0,
                    //TEAMMISCode = "",
                    FromDeposit = true,
                    //Tenor = entity.tenor,
                    //TerminalDate = entity.terminalDate,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = user.UserName,
                    UpdatedOn = DateTime.Now,
                };
                _dataContext.credit_casa.Add(customerAccount);
                _dataContext.SaveChanges();
            }
            
        }

        #endregion

        #region DocumentUpload
        public async Task<bool> AddUpdateKYCustomerDocAsync(deposit_customerkycdocumentupload model)
        {
            try
            {

                if (model.DocumentId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customerkycdocumentupload.FindAsync(model.DocumentId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customerkycdocumentupload.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKYCustomerDocAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customerkycdocumentupload.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customerkycdocumentupload> GetKYCustomerDocByIdAsync(int id)
        {
            return await _dataContext.deposit_customerkycdocumentupload.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customerkycdocumentupload>> GetAllKYCustomerDocAsync()
        {
            return await _dataContext.deposit_customerkycdocumentupload.Where(d => d.Deleted == false).ToListAsync();
        }

        #endregion

        #region SignatureUpload
        public async Task<bool> AddUpdateSignatureAsync(deposit_customersignature model)
        {
            try
            {

                if (model.SignatureId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customersignature.FindAsync(model.SignatureId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customersignature.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteSignatureAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customersignature.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customersignature> GetSignatureByIdAsync(int id)
        {
            return await _dataContext.deposit_customersignature.FindAsync(id);
        }

        public async Task<deposit_customersignature> GetSignaturesByIdsAsync(int id, int sid)
        {
            return await _dataContext.deposit_customersignature.FindAsync(id, sid);
        }

        public async Task<IEnumerable<deposit_customersignature>> GetAllSignatureAsync()
        {
            return await _dataContext.deposit_customersignature.Where(d => d.Deleted == false).ToListAsync();
        }

        #endregion

        #region Signatory
        public async Task<bool> AddUpdateSignatoryAsync(deposit_customersignatory model)
        {
            try
            {

                if (model.SignatoryId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customersignatory.FindAsync(model.SignatoryId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customersignatory.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Signatory SignatureUpload
        public bool SignatoryUpload(byte[] image, CustomerSignatureObj entity)
        {
            try
            {
                if (entity == null) return false;
                var user = _serverRequest.UserDataAsync().Result;
                var Category = _dataContext.deposit_customersignature.Where(a => a.SignatoryId == entity.SignatoryId && a.CustomerId == entity.CustomerId).FirstOrDefault();
                if (Category != null)
                {
                    Category.SignatoryId = entity.SignatoryId;
                    Category.CustomerId = entity.CustomerId;
                    Category.Name = entity.DocumentName;
                    Category.SignatureImg = entity.SignatureImg;
                    Category.Active = true;
                    Category.Deleted = false;
                    Category.CreatedBy = user.UserName;
                    Category.CreatedOn = DateTime.Now;
                    Category.UpdatedBy = user.UserName;
                    Category.UpdatedOn = DateTime.Now;
                }
                else
                {
                    var kk = new deposit_customersignature
                    {
                        SignatureId = entity.SignatureId,
                        SignatoryId = entity.SignatoryId,
                        CustomerId = entity.CustomerId,
                        Name = entity.DocumentName,
                        SignatureImg = entity.SignatureImg,
                        Active = true,
                        Deleted = false,
                        CreatedBy = user.UserName,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = user.UserName,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.deposit_customersignature.Add(kk);
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteSignatoryAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customersignatory.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customersignatory> GetSignatoryByIdAsync(int id)
        {
            return await _dataContext.deposit_customersignatory.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customersignatory>> GetAllSignatoryAsync(int cid)
        {
            return await _dataContext.deposit_customersignatory.Where(d => d.Deleted == false).ToListAsync();
        }

        #endregion

        #region Directors
        public int AddUpdateDirector(CustomerDirectorsObj entity)
        {
            try
            {
                if (entity == null) return 0;
                int inserted_id = 0;
                var user = _serverRequest.UserDataAsync().Result;
                deposit_customerdirectors Category = null;
                if (entity.DirectorId > 0)
                {
                    Category = _dataContext.deposit_customerdirectors.Where(x => x.DirectorId == entity.DirectorId && entity.DirectorId == x.DirectorId).FirstOrDefault();
                    if (Category != null)
                    {
                        Category.CustomerId = entity.CustomerId;
                        Category.Surname = entity.Surname;
                        Category.Firstname = entity.Firstname;
                        Category.Othername = entity.Othername;
                        Category.TitleId = entity.TitleId;
                        Category.MaritalStatusId = entity.MaritalStatusId;
                        Category.IdentificationType = entity.IdentificationType;
                        Category.IdentificationNumber = entity.IdentificationNumber;
                        Category.Telephone = entity.Telephone;
                        Category.Date = entity.Date;
                        Category.DoB = entity.DoB;
                        Category.PlaceOfBirth = entity.PlaceOfBirth;
                        Category.MaidenName = entity.MaidenName;
                        Category.NextofKin = entity.NextofKin;
                        Category.BVN = entity.BVN;
                        Category.Email = entity.Email;
                        Category.Mobile = entity.Mobile;
                        Category.LGA = entity.LGA;
                        Category.StateOfOrigin = entity.StateOfOrigin; ;
                        Category.TaxIDNumber = entity.TaxIDNumber;
                        Category.MeansOfID = entity.MeansOfID;
                        Category.IDExpiryDate = entity.IDExpiryDate;
                        Category.IDIssueDate = entity.IDIssueDate;
                        Category.Occupation = entity.Occupation;
                        Category.JobTitle = entity.JobTitle;
                        Category.Position = entity.Position;
                        Category.Nationality = entity.Nationality;
                        Category.ResidentPermit = entity.ResidentPermit;
                        Category.PermitIssueDate = entity.PermitExpiryDate;
                        Category.PermitExpiryDate = entity.PermitExpiryDate;
                        Category.SocialSecurityNumber = entity.SocialSecurityNumber;
                        Category.Address1 = entity.Address1;
                        Category.City1 = entity.City1;
                        Category.State1 = entity.State1;
                        Category.Country1 = entity.Country1;
                        Category.Address2 = entity.Address2;
                        Category.City2 = entity.City2;
                        Category.State2 = entity.State2;
                        Category.Country2 = entity.Country2;
                        Category.UpdatedBy = user.UserName;
                        Category.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    Category = new deposit_customerdirectors
                    {
                        DirectorId = entity.DirectorId,
                        CustomerId = entity.CustomerId,
                        TitleId = entity.TitleId,
                        GenderId = entity.GenderId,
                        MaritalStatusId = entity.MaritalStatusId,
                        Surname = entity.Surname,
                        Firstname = entity.Firstname,
                        Othername = entity.Othername,
                        BVN = entity.BVN,
                        Email = entity.Email,
                        Mobile = entity.Mobile,
                        IdentificationType = entity.IdentificationType,
                        IdentificationNumber = entity.IdentificationNumber,
                        Telephone = entity.Telephone,
                        Date = entity.Date,
                        DoB = entity.DoB,
                        PlaceOfBirth = entity.PlaceOfBirth,
                        MaidenName = entity.MaidenName,
                        NextofKin = entity.NextofKin,
                        LGA = entity.LGA,
                        StateOfOrigin = entity.StateOfOrigin,
                        TaxIDNumber = entity.TaxIDNumber,
                        MeansOfID = entity.MeansOfID,
                        IDExpiryDate = entity.IDExpiryDate,
                        IDIssueDate = entity.IDIssueDate,
                        Occupation = entity.Occupation,
                        JobTitle = entity.JobTitle,
                        Position = entity.Position,
                        Nationality = entity.Nationality,
                        ResidentPermit = entity.ResidentPermit,
                        PermitIssueDate = entity.PermitExpiryDate,
                        PermitExpiryDate = entity.PermitExpiryDate,
                        SocialSecurityNumber = entity.SocialSecurityNumber,
                        Address1 = entity.Address1,
                        City1 = entity.City1,
                        State1 = entity.State1,
                        Country1 = entity.Country1,
                        Address2 = entity.Address2,
                        City2 = entity.City2,
                        State2 = entity.State2,
                        Country2 = entity.Country2,
                        Active = true,
                        Deleted = false,
                        CreatedBy = user.UserName,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = user.UserName,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.deposit_customerdirectors.Add(Category);
                    _dataContext.SaveChanges();
                    inserted_id = Category.DirectorId;
                    return inserted_id;
                }
                var response = _dataContext.SaveChanges() > 0;
                return Category.DirectorId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Director SignatureUpload
        public bool DirectorsignatureUpload(DirectorSignatureObj entity)
        {
            try
            {
                if (entity == null) return false;
                var user = _serverRequest.UserDataAsync().Result;
                var Category = _dataContext.deposit_customerdirectors.Where(a => a.DirectorId == entity.DirectorId && a.CustomerId == entity.CustomerId).FirstOrDefault();
                if (Category != null)
                {
                    Category.SignatureUpload = entity.SignatureImg;
                    Category.UpdatedBy = user.UserName;
                    Category.UpdatedOn = DateTime.Now;
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteDirectorsAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customerdirectors.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customerdirectors> GetDirectorByIdAsync(int id)
        {
            return await _dataContext.deposit_customerdirectors.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customerdirectors>> GetAllDirectorsAsync(int cid)
        {
            return await _dataContext.deposit_customerdirectors.Where(d => d.Deleted == false && d.CustomerId == cid).ToListAsync();
        }

        #endregion

        #region KYCustomer
        public async Task<bool> AddUpdateKYCustomerAsync(deposit_customerkyc model)
        {
            try
            {

                if (model.KycId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customerkyc.FindAsync(model.KycId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customerkyc.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteKYCustomerAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customerkyc.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customerkyc> GetKYCustomerByIdAsync(int id)
        {
            return await _dataContext.deposit_customerkyc.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customerkyc>> GetAllKYCustomerAsync(int cid)
        {
            return await _dataContext.deposit_customerkyc.Where(d => d.Deleted == false && d.CustomerId == cid).ToListAsync();
        }

        #endregion

        #region ContactPersons
        public async Task<bool> AddUpdateContactPersonsAsync(deposit_customercontactpersons model)
        {
            try
            {

                if (model.ContactPersonId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customercontactpersons.FindAsync(model.ContactPersonId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customercontactpersons.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteContactPersonsAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customercontactpersons.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customercontactpersons> GetContactPersonsByIdAsync(int id)
        {
            return await _dataContext.deposit_customercontactpersons.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customercontactpersons>> GetContactPersonsAsync(int cid)
        {
            return await _dataContext.deposit_customercontactpersons.Where(d => d.Deleted == false && d.CustomerId == cid).ToListAsync();
        }
        #endregion

        #region NextOfKin
        public async Task<bool> AddUpdateNextOfKinAsync(deposit_customernextofkin model)
        {
            try
            {

                if (model.NextOfKinId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customernextofkin.FindAsync(model.NextOfKinId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customernextofkin.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteNextOfKinAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customernextofkin.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customernextofkin> GetNextOfKinByIdAsync(int id)
        {
            return await _dataContext.deposit_customernextofkin.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customernextofkin>> GetAllNextOfKinAsync(int cid)
        {
            return await _dataContext.deposit_customernextofkin.Where(d => d.Deleted == false && d.CustomerId == cid).ToListAsync();
        }

        #endregion

        #region Identity Details
        public async Task<bool> AddUpdateIdentityDetailsAsync(deposit_customeridentification model)
        {
            try
            {

                if (model.CustomerIdentityId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_customeridentification.FindAsync(model.CustomerIdentityId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_customeridentification.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteIdentityDetailsAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_customeridentification.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_customeridentification> GetIdentityDetailsByIdAsync(int id)
        {
            return await _dataContext.deposit_customeridentification.FindAsync(id);
        }

        public async Task<IEnumerable<deposit_customeridentification>> GetAllIdentityDetailsAsync(int cid)
        {
            return await _dataContext.deposit_customeridentification.Where(d => d.Deleted == false && d.CustomerId == cid).ToListAsync();
        }

        #endregion

        #region DepositForm


        public object SignatoryUpload(byte[] image, string createdBy)
        {
            throw new NotImplementedException();
        }

        public bool DirectorsignatureUpload(byte[] image, DirectorSignatureObj entity)
        {
            throw new NotImplementedException();
        }

        public Task DirectorsignatureUpload(byte[] image, string createdBy)
        {
            throw new NotImplementedException();
        }

       
        #endregion
    }
}
