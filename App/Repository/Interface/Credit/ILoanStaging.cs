using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanStagingObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanStaging
    {
        //Task<bool> AddUpdateLoanStagingAsync(credit_loanstaging model);
        //Task<IEnumerable<credit_loanstaging>> GetAllLoanStagingAsync();
        //Task<credit_loanstaging> GetLoanStagingByIdAsync(int id);
        Task<bool> DeleteLoanStagingAsync(int id);

        Task<LoanStagingObj> GetLoanStagingByIdAsync(int loanStagingId);

        Task<List<LoanStagingObj>> GetAllLoanStagingAsync();

        Task AddOrUpdateLoanStagingAsync(LoanStagingObj loanStagingViewModel);

    }
}
