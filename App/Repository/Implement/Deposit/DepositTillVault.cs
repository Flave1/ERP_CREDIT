using Banking.Data;
using Banking.Repository.Interface.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement.Deposit
{
    public class DepositTillVault : IDepositTillVault
    {
        private readonly DataContext _dataContext;

        public bool AddUpdateDepositTillVault(deposit_tillvaultform entity)
        {
            if (entity.TillVaultId > 0)
            {
                var item = _dataContext.deposit_tillvaultform.Find(entity.TillVaultId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                _dataContext.deposit_tillvaultform.Add(entity);
            return _dataContext.SaveChanges() > 0;
        }

        public bool AddUpdateTillVaultSetup(deposit_tillvaultsetup entity)
        {
            if (entity.TillVaultSetupId > 0)
            {
                var item = _dataContext.deposit_tillvaultsetup.Find(entity.TillVaultSetupId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                _dataContext.deposit_tillvaultsetup.Add(entity);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteDepositTillVault(int CustomerId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTillVaultSetup(int CustomerId)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateExportTillVault()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_tillvaultform> GetAllDepositTillVault()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_tillvaultsetup> GetAllTillVaultSetup()
        {
            throw new NotImplementedException();
        }

        public deposit_tillvaultform GetDepositTillVault(int id)
        {
            throw new NotImplementedException();
        }

        public deposit_tillvaultsetup GetTillVaultSetup(int id)
        {
            throw new NotImplementedException();
        }

        public bool UploadTillVault(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }
    }
}
