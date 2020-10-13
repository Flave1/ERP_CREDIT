using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Implement.Credit.Collateral;
using Banking.Repository.Interface.Credit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;

namespace Banking.Repository.Implement.Credit
{
    public class CollateralCustomerConsumptionRepository : ICollateralCustomerConsumptionRepository
    {
        private readonly DataContext _dataContext;
        private readonly ICollateralRepository _collateralRepository;

        public CollateralCustomerConsumptionRepository(DataContext dataContext, ICollateralRepository collateralRepository)
        {
            _dataContext = dataContext;
            _collateralRepository = collateralRepository;
        }
        public async Task<bool> AddOrUpdateCollateralCustomerConsumptionAsync(CollateralCustomerConsumptionObj model)
        {
            var creditCollateralCustomerConsumptionDb = await _dataContext.credit_collateralcustomerconsumption.FirstOrDefaultAsync(x => x.CollateralCustomerConsumptionId == model.CollateralCustomerConsumptionId);

            if (creditCollateralCustomerConsumptionDb != null)
            {

                creditCollateralCustomerConsumptionDb.CollateralCustomerId = model.CollateralCustomerId;
                creditCollateralCustomerConsumptionDb.CollateralCustomerConsumptionId = model.CollateralCustomerConsumptionId;
                creditCollateralCustomerConsumptionDb.LoanApplicationId = model.LoanApplicationId;
                creditCollateralCustomerConsumptionDb.Amount = model.ActualCollateralValue;
                creditCollateralCustomerConsumptionDb.Active = true;
                creditCollateralCustomerConsumptionDb.Deleted = false;
                creditCollateralCustomerConsumptionDb.UpdatedBy = model.CreatedBy;
                creditCollateralCustomerConsumptionDb.UpdatedOn = DateTime.Now;
            }
            else
            {
                var item = new credit_collateralcustomerconsumption
                {
                    CollateralCustomerId = model.CollateralCustomerId,
                    CollateralCustomerConsumptionId = model.CollateralCustomerConsumptionId,
                    CustomerId = model.CustomerId,
                    LoanApplicationId = model.LoanApplicationId,
                    Amount = model.ActualCollateralValue,
                    Active = true,
                    Deleted = false,
                    CreatedBy = model.CreatedBy,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = model.CreatedBy,
                    UpdatedOn = DateTime.Now,
                };
               await _dataContext.credit_collateralcustomerconsumption.AddAsync(item);
            } 
           return  await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task DeleteListCollateralCustomerConsumptionByCollateralCustomerConsumptionIdAsync(int CollateralCustomerConsumptionId)
        {
            var creditCollateralCustomerConsumptionDb = _dataContext.credit_collateralcustomerconsumption.FirstOrDefault(x => x.CollateralCustomerConsumptionId == CollateralCustomerConsumptionId && x.Deleted == false);

            if (creditCollateralCustomerConsumptionDb != null)
            {
                creditCollateralCustomerConsumptionDb.Deleted = true;
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IList<CollateralCustomerConsumptionObj>> GetAllCollateralCustomerConsumptionAsync()
        {
            var collateralCustomerConsumptions = await _dataContext.credit_collateralcustomerconsumption.Where(x => x.Deleted != true).ToListAsync();
            var collateralCustomerConsumptionsViewModel = new List<CollateralCustomerConsumptionObj>();

            foreach (var consumption in collateralCustomerConsumptions)
            {
                collateralCustomerConsumptionsViewModel.Add(new CollateralCustomerConsumptionObj
                {
                    Amount = consumption.Amount,
                    CollateralCustomerConsumptionId = consumption.CollateralCustomerConsumptionId,
                    CollateralCustomerId = consumption.CollateralCustomerId,
                });
            }

            return collateralCustomerConsumptionsViewModel;
        }

        public async Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByCollateralCustomerIdAsync(int collateralCustomerId)
        {
            var collateralCustomerConsumptions = await _dataContext.credit_collateralcustomerconsumption.Where(x => x.Deleted != true && x.CollateralCustomerId == collateralCustomerId).ToListAsync();
            var collateralCustomerConsumptionsViewModel = new List<CollateralCustomerConsumptionObj>();

            foreach (var consumption in collateralCustomerConsumptions)
            {
                collateralCustomerConsumptionsViewModel.Add(new CollateralCustomerConsumptionObj
                {
                    Amount = consumption.Amount,
                    CollateralCustomerConsumptionId = consumption.CollateralCustomerConsumptionId,
                    CollateralCustomerId = consumption.CollateralCustomerId,
                    CustomerId = consumption.CustomerId
                });
            }

            return collateralCustomerConsumptionsViewModel;
        }

        public async Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByCustomerIdAsync(int customerId)
        {
            var collateralCustomerConsumptions = await _dataContext.credit_collateralcustomerconsumption.Where(x => x.Deleted != true && x.CustomerId == customerId).ToListAsync();
            var collateralCustomerConsumptionsViewModel = new List<CollateralCustomerConsumptionObj>();

            foreach (var consumption in collateralCustomerConsumptions)
            {
                collateralCustomerConsumptionsViewModel.Add(new CollateralCustomerConsumptionObj
                {
                    Amount = consumption.Amount,
                    CollateralCustomerConsumptionId = consumption.CollateralCustomerConsumptionId,
                    CollateralCustomerId = consumption.CollateralCustomerId,
                    CustomerId = consumption.CustomerId
                });
            }

            return collateralCustomerConsumptionsViewModel;
        }

        public async Task<CollateralCustomerConsumptionObj> GetCollateralCustomerConsumptionById(int collateralCustomerConsumptionById)
        {
            var creditCollateralCustomerConsumptionDb = await _dataContext.credit_collateralcustomerconsumption.FirstOrDefaultAsync(x => x.CollateralCustomerConsumptionId == collateralCustomerConsumptionById);

            return new CollateralCustomerConsumptionObj
            {
                Amount = creditCollateralCustomerConsumptionDb.Amount,
                CollateralCustomerConsumptionId = creditCollateralCustomerConsumptionDb.CollateralCustomerConsumptionId,
                CollateralCustomerId = creditCollateralCustomerConsumptionDb.CollateralCustomerId,
            };
        }

        public async Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByLoanApplicationIdAsync(int loanApplicationId)
        {
            var collateralCustomerConsumptions = await _dataContext.credit_collateralcustomerconsumption.Where(x => x.Deleted != true && x.LoanApplicationId == loanApplicationId).ToListAsync();
            var collateralCustomerConsumptionsViewModel = new List<CollateralCustomerConsumptionObj>();

            foreach (var consumption in collateralCustomerConsumptions)
            {
                collateralCustomerConsumptionsViewModel.Add(new CollateralCustomerConsumptionObj
                {
                    Amount = consumption.Amount,
                    CollateralCustomerConsumptionId = consumption.CollateralCustomerConsumptionId,
                    CollateralCustomerId = consumption.CollateralCustomerId,
                    CustomerId = consumption.CustomerId
                });
            }

            return collateralCustomerConsumptionsViewModel;
        }
    }
}
