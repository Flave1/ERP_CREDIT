using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ICollateralCustomerConsumptionRepository
    {
        Task DeleteListCollateralCustomerConsumptionByCollateralCustomerConsumptionIdAsync(int CollateralCustomerConsumptionId);

        Task<bool> AddOrUpdateCollateralCustomerConsumptionAsync(CollateralCustomerConsumptionObj model);

        Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByLoanApplicationIdAsync(int loanApplicationId);

        Task<IList<CollateralCustomerConsumptionObj>> GetAllCollateralCustomerConsumptionAsync();

        Task<CollateralCustomerConsumptionObj> GetCollateralCustomerConsumptionById(int collateralCustomerConsumptionById);

        Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByCustomerIdAsync(int customerId);

        Task<IList<CollateralCustomerConsumptionObj>> GetCollateralCustomerConsumptionByCollateralCustomerIdAsync(int collateralCustomerId);
    }
}
