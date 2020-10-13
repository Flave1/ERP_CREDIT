using Banking.Contracts.Response.Deposit;
using AutoMapper;
using GODP.Entities.Models;
using Banking.DomainObjects.Credit;
using static Banking.Contracts.Response.Credit.FeeObjs;
using static Banking.Contracts.Response.Credit.CreditClassificationObjs;
using static Banking.Contracts.Response.Credit.LoanStagingObjs;
using static Banking.Contracts.Response.Credit.ProductObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;

namespace Banking.MapProfiles
{
    public class DomainToRequestProfiles : Profile
    {
        public DomainToRequestProfiles()
        {
            CreateMap<deposit_accountype, AccountTypeObj>();
            CreateMap<deposit_category, CategoryObj>();
            CreateMap<deposit_cashiertellersetup, CashierTellerSetupObj>();
            CreateMap<deposit_cashiertellerform, CashierTellerFormObj>();
            CreateMap<deposit_transactioncharge, TransactionChargeObj>();
            CreateMap<deposit_transactiontax, TransactionTaxObj>();
            CreateMap<deposit_customeridentification, CustomerIdentificationObj>();
            CreateMap<deposit_customernextofkin, CustomerNextOfKinObj>();
            CreateMap<deposit_customercontactpersons, CustomerContactPersonsObj>();
            CreateMap<deposit_customerkyc, deposit_customerkyc>();
            CreateMap<deposit_customerdirectors, CustomerDirectorsObj>();
            CreateMap<deposit_customersignatory, CustomerSignatoryObj>();
            CreateMap<deposit_customersignature, CustomerSignatureObj>();
            CreateMap<deposit_customerkycdocumentupload, KyCustomerDocUploadObj>();
            CreateMap<deposit_businesscategory, BusinessCategoryObj>();


            CreateMap<credit_fee, FeeObj>();
            CreateMap<credit_repaymenttype, RepaymentTypeObj>();
            CreateMap<credit_creditclassification, CreditClassificationObj>();
            CreateMap<credit_loanstaging, LoanStagingObj>();
            CreateMap<credit_loan, credit_loan_obj>();
            CreateMap<credit_producttype, ProductTypeObj>();
            
            
        }
    }
}
