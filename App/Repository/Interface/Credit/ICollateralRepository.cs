using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationCollateralObjs;

namespace Banking.Repository.Implement.Credit.Collateral
{
    public interface ICollateralRepository
    {
        #region Collateral Type Setup
        bool AddUpdateCollateralType(CollateralTypeObj model);
        List<CollateralTypeObj> GetAllCollateralType();
        CollateralTypeObj GetCollateralType(int collateralTypeId);
        bool DeleteCollateralType(int collateralTypeId);
        byte[] GenerateExportCollateralType();
        bool UploadCollateralType(byte[] record, string createdBy);
        #endregion

        #region Collateral Customer 

        Task<int> AddUpdateCustomerCollateral(credit_collateralcustomer model);
        Task<IList<CollateralTypeObj>> GetAllowableCollateralByLoanApplicationId(int loanApplicationId);
        List<CollateralCustomerObj> GetAllCollateralCustomer();
        CollateralCustomerObj GetCollateralCustomer(int collateralCustomerId);
        IEnumerable<CollateralCustomerObj> GetCollateralSingleCustomer(int customerId);
        bool DeleteCollateralCutomer(int collateralCustomerId);
        IEnumerable<CollateralCustomerObj> GetAllCustomerCollateralByLoanApplication(int loanApplicationId, bool includeNotAllowSharing);
        bool UploadCustomerCollateral(byte[] record, string createdBy);
        byte[] GenerateExportCustomerCollateral();       
        //bool DeleteCustomerCollateral(int collateralCustomerId);
        #endregion

        #region Loan Application Collateral
        bool AddUpdateLoanApplicationCollateral(LoanApplicationCollateralObj model);
        IEnumerable<LoanApplicationCollateralObj> GetAllLoanApplicationCollateral();
        IEnumerable<LoanApplicationCollateralObj> GetLoanApplicationCollateral(int loanApplicationId);
        bool DeleteLoanApplicationCollateral(int loanApplicationCollateralId);
        Task DeleteLoanApplicationCollateralDocumentAsync(int collateralCustomerId);
        #endregion

        IEnumerable<LoanApplicationCollateralObj> GetLoanApplicationCollateralForLoanApplicationId(int loanApplicationId);
        decimal GetLoanApplicationRequireAmount(int loanApplicationId);
    }
}
