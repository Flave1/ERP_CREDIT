using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanApplicationCollateralObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanApplicationCollateralDocumentRespository
    {
        Task DeleteLoanApplicationCollateralDocumentAsync(int collateralCustomerId);

        Task<LoanApplicationCollateralDocumentObj> GetLoanApplicationCollateralDocumentByIdAsync(int CollateralCustomerId);

        Task<IList<LoanApplicationCollateralDocumentObj>> GetAllLoanApplicationCollateralDocumentAsync(int loanApplicationId);

        Task AddOrUpdateLoanApplicationCollateralDocumentAsync(LoanApplicationCollateralDocumentObj loanApplicationCollateralDocument);
    }
}
