using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanCustomerObj : GeneralEntity
    {
        public LoanCustomerObj()
        {
            Director = new List<LoanCustomerDirectorObj>();
            Bank = new List<LoanCustomerBankDetailsObj>();
            Identity = new List<LoanCustomerIdentityDetailsObj>();
            Nextofkin = new List<LoanCustomerNextOfKinObj>();
            Shareholder = new List<LoanCustomerDirectorShareHolderObj>();
            Document = new List<LoanCustomerDocumentObj>();
        }
        public int CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int? TitleId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int? GenderId { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public byte[] Signature { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string PostalAddress { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }
        public int? EmploymentType { get; set; }
        public string Employment { get; set; }
        public bool? PoliticallyExposed { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string RegistrationNo { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string Industry { get; set; }
        public string IncorporationCountry { get; set; }
        public decimal? AnnualTurnover { get; set; }
        public decimal? ShareholderFund { get; set; }
        public int ApprovalStatusId { get; set; }
        public int? RelationshipOfficerId { get; set; }
        public int? MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; }
        public string RelationshipManager { get; set; }
        public string RelationshipManagerEmail { get; set; }
        public decimal? InFlow { get; set; }
        public decimal? OutFlow { get; set; }
        public decimal Balance { get; set; }
        public string CASAAccountNumber { get; set; }
        public decimal? ProfileStatus { get; set; }

        public List<LoanCustomerDirectorObj> Director { get; set; }
        public List<LoanCustomerBankDetailsObj> Bank { get; set; }
        public List<LoanCustomerIdentityDetailsObj> Identity { get; set; }
        public List<LoanCustomerNextOfKinObj> Nextofkin { get; set; }
        public List<LoanCustomerDirectorShareHolderObj> Shareholder { get; set; }
        public List<LoanCustomerDocumentObj> Document { get; set; }
    }

    public class LoanCustomerLiteObj
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string CustomerTypeName { get; set; }
        public string AccountNumber { get; set; }
    }

    public class LoanCustomerDirectorObj : GeneralEntity
    {
        public int CustomerDirectorId { get; set; }
        public int DirectorTypeId { get; set; }
        public string DirectorType { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int CustomerId { get; set; }
        public string PhoneNo { get; set; }
        public decimal? PercentageShare { get; set; }
        public DateTime? Dob { get; set; }
        public Byte[] Signature { get; set; }
        public bool PoliticallyPosition { get; set; }
        public bool RelativePoliticallyPosition { get; set; }
    }

    public class LoanCustomerIdentityDetailsObj : GeneralEntity
    {
        public int CustomerIdentityDetailsId { get; set; }
        public string Number { get; set; }
        public int IdentificationId { get; set; }
        public string Identification { get; set; }
        public string Issuer { get; set; }
        public int CustomerId { get; set; }
    }


    public class LoanCustomerBankDetailsObj : GeneralEntity
    {
        public int CustomerBankDetailsId { get; set; }
        public string Bvn { get; set; }
        public string Account { get; set; }
        public string Bank { get; set; }
        public int CustomerId { get; set; }
    }

    public class LoanCustomerCardDetailsObj : GeneralEntity
    {
        public int CustomerCardDetailsId { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CurrencyCode { get; set; }
        public string IssuingBank { get; set; }
    }


    public class LoanCustomerNextOfKinObj : GeneralEntity
    {
        public int CustomerNextOfKinId { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int CustomerId { get; set; }
        public string PhoneNo { get; set; }
    }


    public class LoanCustomerDirectorShareHolderObj : GeneralEntity
    {
        public int DirectorShareHolderId { get; set; }
        public string CompanyName { get; set; }
        public double PercentageHolder { get; set; }
        public int CustomerId { get; set; }
    }

    public class LoanCustomerDocumentObj: GeneralEntity
    {
        public int CustomerDocumentId { get; set; }

        public int CustomerId { get; set; }

        public int DocumentTypeId { get; set; }

        public string PhysicalLocation { get; set; }

        public string DocumentName { get; set; }

        public string DocumentExtension { get; set; }

        public byte[] DocumentFile { get; set; }
    }

    public class StartLoanApplicationCustomerObj
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string CustomerTypeName { get; set; }
        public decimal? TotalExposure { get; set; }
        public decimal? CurrentExposure { get; set; }
    }

    public class DocumentTypeObj : GeneralEntity
    {
        public int DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public int UserAccountId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Guid { get; set; }
        public int ResetPasswordId { get; set; }

        public string Email { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? ResetPassword { get; set; }
    }

    public class LoanCustomerRespObj
    {
        public IEnumerable<LoanCustomerObj> Customers { get; set; }
        public IEnumerable<LoanCustomerLiteObj> CustomerLites { get; set; }
        public IEnumerable<LoanCustomerObj> StartCustomers { get; set; }
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerDirectorRespObj
    {
        public IEnumerable<LoanCustomerDirectorObj> CustomerDirectors { get; set; }
        public int CustomerDirectorId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerDocumentRespObj
    {
        public IEnumerable<LoanCustomerDocumentObj> CustomerDocuments { get; set; }
        public int DocumentTypeId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerIdentityRespObj
    {
        public IEnumerable<LoanCustomerIdentityDetailsObj> CustomerIdentity { get; set; }
        public int CustomerIdentityDetailsId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerBankDetailRespObj
    {
        public IEnumerable<LoanCustomerBankDetailsObj> CustomerBankDetails { get; set; }
        public LoanCustomerCardDetailsObj CustomerCardDetails { get; set; }
        public int CustomerBankDetailsId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerNextOfKinRespObj
    {
        public IEnumerable<LoanCustomerNextOfKinObj> CustomerNextOfKin { get; set; }
        public int CustomerNextOfKinId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerDirectorShareHolderRespObj
    {
        public IEnumerable<LoanCustomerDirectorShareHolderObj> DirectorShareHolder { get; set; }
        public int DirectorShareHolderId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class StartLoanCustomerRespObj
    {
        public IEnumerable<StartLoanApplicationCustomerObj> Customers { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoanCustomerSearchObj
    {
        public int CustomerId { get; set; }
        public int DocumentTypeId { get; set; }
        public int CustomerDirectorId { get; set; }
        public int CustomerIdentityDetailsId { get; set; }
        public int CustomerBankDetailsId { get; set; }
        public int CustomerNextOfKinId { get; set; }
        public int DirectorShareHolderId { get; set; }
        public string AccountNumber { get; set; }
    }

    public class DeleteCustomerCommand
    {
        public List<int> Ids { get; set; }
    }

    public class GLTransactionObj
    {
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
    }

    public class GLTransactionRespObj
    {
        public IEnumerable<GLTransactionObj> CustomerTransactions { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

