using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanStagingObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanStagingRepository : ILoanStaging
    {
        private readonly DataContext _dataContext;
        public LoanStagingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //public async Task<bool> AddUpdateLoanStagingAsync(credit_loanstaging model)
        //{
        //    try
        //    {
        //        if (model.LoanStagingId > 0)
        //        {
        //            var itemToUpdate = await _dataContext.credit_loanstaging.FindAsync(model.LoanStagingId);
        //            _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
        //        }
        //        else
        //            await _dataContext.credit_loanstaging.AddAsync(model);
        //        return await _dataContext.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task AddOrUpdateLoanStagingAsync(LoanStagingObj loanStagingViewModel)
        {
            if (loanStagingViewModel.LoanStagingId > 0)
            {
                var loanstaging = await _dataContext.credit_loanstaging.FirstOrDefaultAsync(x => x.LoanStagingId == loanStagingViewModel.LoanStagingId);

                loanstaging.LoanStagingId = loanStagingViewModel.LoanStagingId;
                loanstaging.ProbationPeriod = loanStagingViewModel.ProbationPeriod;
                loanstaging.From = loanStagingViewModel.From;
                loanstaging.To = loanStagingViewModel.To;
                loanstaging.Active = true;
                loanstaging.Deleted = false;
                loanstaging.UpdatedBy = loanStagingViewModel.UpdatedBy;
                loanstaging.UpdatedOn = DateTime.Now;
            }
            else
            {
                var newLoanStaging = new credit_loanstaging
                {
                    LoanStagingId = loanStagingViewModel.LoanStagingId,
                    ProbationPeriod = loanStagingViewModel.ProbationPeriod,
                    From = loanStagingViewModel.From,
                    To = loanStagingViewModel.To,
                    Active = true,
                    Deleted = false,
                    CreatedBy = loanStagingViewModel.CreatedBy,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = loanStagingViewModel.CreatedBy,
                    UpdatedOn = DateTime.Now,
                };
                _dataContext.credit_loanstaging.Add(newLoanStaging);
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteLoanStagingAsync(int id)
        {
            var itemToDelete = await _dataContext.credit_loanstaging.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<LoanStagingObj>> GetAllLoanStagingAsync()
        {
            var creditLoanStagings = await _dataContext.credit_loanstaging.Where(x => x.Deleted != true).ToListAsync();
            var loanStagingViewModels = new List<LoanStagingObj>();

            foreach (var loanStagingVm in creditLoanStagings)
            {
                loanStagingViewModels.Add(new LoanStagingObj
                {
                    LoanStagingId = loanStagingVm.LoanStagingId,
                    ProbationPeriod = loanStagingVm.ProbationPeriod,
                    From = loanStagingVm.From,
                    To = loanStagingVm.To,
                });
            }

            return loanStagingViewModels;
        }

        public async Task<LoanStagingObj> GetLoanStagingByIdAsync(int loanStagingId)
        {
            var loanStaging = await _dataContext.credit_loanstaging.FirstOrDefaultAsync(x => x.LoanStagingId == loanStagingId);

            return new LoanStagingObj
            {
                LoanStagingId = loanStaging.LoanStagingId,
                ProbationPeriod = loanStaging.ProbationPeriod,
                From = loanStaging.From,
                To = loanStaging.To,
            };
        }


        //public async Task<IEnumerable<credit_loanstaging>> GetAllLoanStagingAsync()
        //{
        //    return await _dataContext.credit_loanstaging.Where(x => x.Deleted == false).ToListAsync();
        //}

        //public async Task<credit_loanstaging> GetLoanStagingByIdAsync(int id)
        //{
        //    return await _dataContext.credit_loanstaging.Where(x => x.Deleted == false && x.LoanStagingId == id).FirstOrDefaultAsync();
        //}
    }
}
