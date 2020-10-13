using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;

namespace Banking.Repository.Implement.Credit
{
    public class AllowableCollateralRepository : IAllowableCollateralRepository
    {
        private readonly DataContext _dataContext;
        public AllowableCollateralRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task AddAllowableCollateral(IList<AllowableCollateralObj> model)
        {
            var allowable_Collateraltypes = new List<cor_allowable_collateraltype>();

            foreach (var item in model)
            {
                allowable_Collateraltypes.Add(new cor_allowable_collateraltype
                {
                    ProductId = item.ProductId,
                    CollateralTypeId = item.CollateralTypeId
                });
            }

            _dataContext.cor_allowable_collateraltype.AddRange(allowable_Collateraltypes);

            return _dataContext.SaveChangesAsync();
        }

        public async Task DeleteListAllowableCollateralByProductIdAsync(int productId)
        {
            var allowable_collaterals = await _dataContext.cor_allowable_collateraltype.Where(x => x.ProductId == productId).ToListAsync();

            _dataContext.cor_allowable_collateraltype.RemoveRange(allowable_collaterals);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IList<AllowableCollateralObj>> GetAllAllowableCollateralAsync()
        {
            var allowable_collaterals = await _dataContext.cor_allowable_collateraltype.ToListAsync();
            var allowableCollateralViewModel = new List<AllowableCollateralObj>();

            foreach (var allowable_collateral in allowable_collaterals)
            {
                allowableCollateralViewModel.Add(new AllowableCollateralObj
                {
                    AllowableCollateralId = allowable_collateral.AllowableCollateralId,
                    ProductId = allowable_collateral.ProductId,
                    CollateralTypeId = allowable_collateral.CollateralTypeId
                });
            }

            return allowableCollateralViewModel;
        }

        public async Task<IList<AllowableCollateralObj>> GetAllowableCollateralByProductIdAsync(int productId)
        {
            var allowable_collaterals = await _dataContext.cor_allowable_collateraltype.Where(x => x.ProductId == productId).ToListAsync();

            var allowableCollateralViewModel = new List<AllowableCollateralObj>();

            foreach (var allowable_collateral in allowable_collaterals)
            {
                allowableCollateralViewModel.Add(new AllowableCollateralObj
                {
                    AllowableCollateralId = allowable_collateral.AllowableCollateralId,
                    ProductId = allowable_collateral.ProductId,
                    CollateralTypeId = allowable_collateral.CollateralTypeId
                });
            }

            return allowableCollateralViewModel;
        }
    }
}
