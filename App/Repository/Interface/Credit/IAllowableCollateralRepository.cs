using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface IAllowableCollateralRepository
    {
        Task DeleteListAllowableCollateralByProductIdAsync(int productId);

        Task AddAllowableCollateral(IList<AllowableCollateralObj> model);

        Task<IList<AllowableCollateralObj>> GetAllowableCollateralByProductIdAsync(int productId);

        Task<IList<AllowableCollateralObj>> GetAllAllowableCollateralAsync();
    }
}
