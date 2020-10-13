using Banking.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerDetailsObj
    {
        public int customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string customerTypeName { get; set; }
        public string accountNumber { get; set; }
        public decimal availableBalance { get; set; }
    }

    public class AddUpdateAccountOpeningObj
    {
        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public int AccountTypeId { get; set; }

        public int? AccountCategoryId { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? Title { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Othername { get; set; }

        public int? MaritalStatusId { get; set; }

        public int? GenderId { get; set; }

        public int? BirthCountryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(50)]
        public string MotherMaidenName { get; set; }

        public int? RelationshipOfficerId { get; set; }

        [StringLength(500)]
        public string TaxIDNumber { get; set; }

        [StringLength(500)]
        public string BVN { get; set; }

        public int? Nationality { get; set; }

        [StringLength(50)]
        public string ResidentPermitNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitExpiryDate { get; set; }

        [StringLength(50)]
        public string SocialSecurityNumber { get; set; }

        public int? StateOfOrigin { get; set; }

        public int? LocalGovernment { get; set; }

        public bool? ResidentOfCountry { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string MailingAddress { get; set; }

        [StringLength(50)]
        public string MobileNumber { get; set; }

        public bool? InternetBanking { get; set; }

        public bool? EmailStatement { get; set; }

        public bool? Card { get; set; }

        public bool? SmsAlert { get; set; }

        public bool? EmailAlert { get; set; }

        public bool? Token { get; set; }

        public int? EmploymentType { get; set; }

        [StringLength(50)]
        public string EmployerName { get; set; }

        [StringLength(50)]
        public string EmployerAddress { get; set; }

        [StringLength(50)]
        public string EmployerState { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string BusinessName { get; set; }

        [StringLength(50)]
        public string BusinessAddress { get; set; }

        [StringLength(50)]
        public string BusinessState { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Other { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeclarationDate { get; set; }

        public bool? DeclarationCompleted { get; set; }

        public byte[] SignatureUpload { get; set; }

        public int? SoleSignatory { get; set; }

        public int? MaxNoOfSignatory { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string Industry { get; set; }

        [StringLength(50)]
        public string Jurisdiction { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(50)]
        public string NatureOfBusiness { get; set; }

        [StringLength(50)]
        public string AnnualRevenue { get; set; }

        public bool? IsStockExchange { get; set; }

        [StringLength(50)]
        public string Stock { get; set; }

        [StringLength(50)]
        public string RegisteredAddress { get; set; }

        [StringLength(50)]
        public string ScumlNumber { get; set; }
    }

    public class AccountOpeningRegRespObj
    {
        public int CustomerId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerDetailsRegRespObj
    {
        public int CustomerId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountOpeningRespObj
    {
        public int ResponseId { get; set; }
        public List<DepositAccountOpeningObj> AccountOpenings { get; set; }
        public IEnumerable<CustomerDetailsObj> CustomerLite { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class DepositAccountOpeningObj : GeneralEntity
    {

        public DepositAccountOpeningObj()
        {
            Identification = new List<CustomerIdentificationObj>();
            Kyc = new List<KyCustomerObj>();
            NextOfKin = new List<CustomerNextOfKinObj>();
            Signatory = new List<CustomerSignatoryObj>();
            Signature = new List<CustomerSignatureObj>();
            Document = new List<KyCustomerDocUploadObj>();
        }

        public int CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public int AccountTypeId { get; set; }
        public int? AccountCategoryId { get; set; }
        public int? Title { get; set; }
        public string AccountNumber { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? RelationshipOfficerId { get; set; }
        public int? GenderId { get; set; }
        public int? BirthCountryId { get; set; }
        public DateTime? DOB { get; set; }
        public string MotherMaidenName { get; set; }
        public string TaxIDNumber { get; set; }
        public string BVN { get; set; }
        public int? Nationality { get; set; }
        public string ResidentPermitNumber { get; set; }
        public DateTime? PermitIssueDate { get; set; }
        public DateTime? PermitExpiryDate { get; set; }
        public string SocialSecurityNumber { get; set; }
        public int? StateOfOrigin { get; set; }
        public int? LocalGovernment { get; set; }
        public bool? ResidentOfCountry { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string Email { get; set; }
        public string MailingAddress { get; set; }
        public string MobileNumber { get; set; }
        public bool? InternetBanking { get; set; }
        public bool? EmailStatement { get; set; }
        public bool? Card { get; set; }
        public bool? SmsAlert { get; set; }
        public bool? EmailAlert { get; set; }
        public bool? Token { get; set; }
        public int? EmploymentType { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public string EmployerState { get; set; }
        public string Occupation { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessState { get; set; }
        public string JobTitle { get; set; }
        public string Other { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public bool? DeclarationCompleted { get; set; }
        public int? SoleSignatory { get; set; }
        public int? MaxNoOfSignatory { get; set; }
        public string RegistrationNumber { get; set; }
        public string Industry { get; set; }
        public string Jurisdiction { get; set; }
        public string Website { get; set; }
        public string NatureOfBusiness { get; set; }
        public string AnnualRevenue { get; set; }
        public bool? IsStockExchange { get; set; }
        public string Stock { get; set; }
        public string RegisteredAddress { get; set; }
        public string ScumlNumber { get; set; }

        public List<CustomerIdentificationObj> Identification { get; set; }
        public List<KyCustomerObj> Kyc { get; set; }
        public List<CustomerNextOfKinObj> NextOfKin { get; set; }
        public List<CustomerSignatoryObj> Signatory { get; set; }
        public List<CustomerSignatureObj> Signature { get; set; }
        public List<KyCustomerDocUploadObj> Document { get; set; }
    }
}


