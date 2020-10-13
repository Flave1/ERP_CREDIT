using Banking.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Finance.Contracts.Response.Reports
{
    public class OfferLetterDetailObj : GeneralEntity
    {
        public string LoanApplicationId { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string CurrencyName { get; set; }
        public decimal LoanAmount { get; set; }
        public double ExchangeRate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public decimal BaseCurrencyLoanAmount
        {
            get { return this.LoanAmount * (decimal)this.ExchangeRate; }
        }

        public int Tenor { get; set; }
        public double InterestRate { get; set; }
        public string CustomerGroupName { get; set; }
        public string LoanTypeName { get; set; }
        public string RepaymentTerms { get; set; }
        public string RepaymentSchedule { get; set; }
        public string Purpose { get; set; }
        public short CurrencyId { get; set; }
        public byte[] Signature { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CorporateCustomerObj
    {
        public int SN { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public string RegistrationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyDirector { get; set; }
        public string PoliticallyExposed { get; set; }
        public string Industry { get; set; }
        public string IncorporationCountry { get; set; }
        public decimal? AnnualTurnover { get; set; }
        public decimal CurrentExposure { get; set; }
        public decimal ExposureLimit { get; set; }
        public decimal? ShareholderFund { get; set; }
        public string RelationshipManager { get; set; }
        public int RelationshipManagerId { get; set; }
        public string Group { get; set; }
        public string Total { get; set; }
        public string Sum { get; set; }
        public string CompanyName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime? CreatedOn { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class IndividualCustomerObj
    {
        public int SN { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public string MaritalStatus { get; set; }
        public int MaritalStatusId { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string NextOfKin { get; set; }
        public string EmploymentStatus { get; set; }
        public int EmploymentType { get; set; }
        public string Employer { get; set; }
        public decimal CurrentExposure { get; set; }
        public decimal ExposureLimit { get; set; }
        public string RelationshipManager { get; set; }
        public int RelationshipManagerId { get; set; }
        public string Group { get; set; }
        public string Total { get; set; }
        public string Sum { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime? CreatedOn { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LoansObj
    {
        public int SN { get; set; }
        public string LoanRefNumber { get; set; }
        public string ProductName { get; set; }
        public string LinkedAccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string Industry { get; set; }
        public DateTime DisbursementDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public decimal ApplicationAmount { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal Tenor { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalInterest { get; set; }
        public int NoOfDaysInOverdue { get; set; }
        public string Description { get; set; }
        public int ProvisioningRequirement { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal RepaymentAmount { get; set; }
        public decimal PAR { get; set; }
        public decimal OutstandingPrincipal { get; set; }
        public decimal OutstandingInterest { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal PastDuePrincipal { get; set; }
        public decimal PastDueInterest { get; set; }
        public string Total { get; set; }
        public string AccountOfficer { get; set; }
        public int RelationshipManagerId { get; set; }
        public decimal Sum { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomerColumnObj
    {
        public string name { get; set; }
    }

    public class LoanColumnObj
    {
        public string name { get; set; }
    }

    public class FSReportObj
    {
        public int GlMappingId { get; set; }
        public string Caption { get; set; }
        public string SubCaption { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int SubPosition { get; set; }
        public string GlCode { get; set; }
        public string ParentCaption { get; set; }
        public int Position { get; set; }
        public string AccountType { get; set; }
        public int AccountTypeId { get; set; }
        public string StatementType { get; set; }
        public int StatementTypeId { get; set; }
        public string SubGlCode { get; set; }
        public string SubGlName { get; set; }
        public int GlId { get; set; }
        public decimal CB { get; set; }
        public string SubGlCode1 { get; set; }
        public DateTime RunDate { get; set; }
        public DateTime PreRunDate { get; set; }
    }

    public class CorporateInvestorCustomerObj
    {
        public int SN { get; set; }
        public int CustomerID { get; set; }
        public DateTime DateOfIncorporation { get; set; }
        public string CompanyName { get; set; }
        public string Size { get; set; }
        public string PhoneNumber { get; set; }
        public string RelationshipOfficer { get; set; }
        public int RelationshipManagerId { get; set; }
        public string Industry { get; set; }
        public string PoliticallyExposed { get; set; }
        public string CurrentBalance { get; set; }
        public string Total { get; set; }
        public string Sum { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class InvestorCustomerObj : GeneralEntity
    {
        public int InvestorFundCustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public string CustomerTypeName { get; set; }

        public int? TitleId { get; set; }

        public string TitleName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }

        public int? GenderId { get; set; }

        public string GenderName { get; set; }

        public string MiddleName { get; set; }

        public int? MaritalStatusId { get; set; }

        public string MaritalStatusName { get; set; }

        public int? CompanyStructureId { get; set; }

        public string CompanyStructureName { get; set; }

        public string Industry { get; set; }

        public int? Size { get; set; }

        public DateTime? DateOfIncorporation { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public string Address { get; set; }

        public string PostalAddress { get; set; }

        public string AccountNumber { get; set; }

        public int? RelationshipOfficerId { get; set; }

        public string RelationshipOfficerName { get; set; }

        public bool? PoliticallyExposed { get; set; }
    }

    public class InvestorCustomerColumnObj
    {
        public string name { get; set; }
    }

    public class IndividualInvestorCustomerObj
    {
        public int SN { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public string MaritalStatus { get; set; }
        public int MaritalStatusId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CustomerAddress { get; set; }
        public string NextOfKin { get; set; }
        public string CurrentBalance { get; set; }
        public string AccountOfficer { get; set; }
        public int RelationshipManagerId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Total { get; set; }
        public string Sum { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }

    public class InvestmentObj
    {
        public int SN { get; set; }
        public string InvestmentID { get; set; }
        public string ProductName { get; set; }
        public string LinkedAccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string Industry { get; set; }
        public DateTime? InvestmentDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string InvestmentAmount { get; set; }
        public decimal ApprovedRate { get; set; }
        public decimal ApprovedTenor { get; set; }
        public int NoOfDaysToMaturity { get; set; }
        public string InvestmentStatus { get; set; }
        public string AccountOfficer { get; set; }
        public int RelationshipManagerId { get; set; }
        public decimal TotalInterest { get; set; }
        public string Total { get; set; }
        public string Sum { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }

    public class InvestmentColumnObj
    {
        public string name { get; set; }
    }

    public class ReportRespObj
    {
        public IEnumerable<LoanPaymentSchedulePeriodicObj> OfferLetterRepayments { get; set; }
        public OfferLetterDetailObj OfferLetterDetails { get; set; }
        public IEnumerable<ProductFeeObj> ProductFees { get; set; }
        public IEnumerable<CorporateCustomerObj> CorporateCustomers { get; set; }
        public IEnumerable<CorporateInvestorCustomerObj> CorporateInvestorCustomers { get; set; }
        public IEnumerable<IndividualInvestorCustomerObj> IndividualInvestorCustomers { get; set; }
        public IEnumerable<IndividualCustomerObj> IndividualCustomers { get; set; }
        public IEnumerable<LoansObj> Loans { get; set; }
        public IEnumerable<InvestmentObj> Investments { get; set; }
        public IEnumerable<FSReportObj> FSreports { get; set; }
        public IEnumerable<FSReportObj> PLreports { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ReportSearchObj
    {
        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
        public int customerTypeId { get; set; }
    }
}
