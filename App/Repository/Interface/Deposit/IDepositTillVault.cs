using Banking.DomainObjects;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IDepositTillVault
    {
        #region DepositTillVaultSetup
        bool AddUpdateTillVaultSetup(deposit_tillvaultsetup entity);
        IEnumerable<deposit_tillvaultsetup> GetAllTillVaultSetup();
        deposit_tillvaultsetup GetTillVaultSetup(int id);
        byte[] GenerateExportTillVault();
        bool UploadTillVault(byte[] record, string createdBy);
        bool DeleteTillVaultSetup(int CustomerId);
        #endregion

        #region DepositTillVault
        bool AddUpdateDepositTillVault(deposit_tillvaultform entity);
        IEnumerable<deposit_tillvaultform> GetAllDepositTillVault();
        deposit_tillvaultform GetDepositTillVault(int id);
        bool DeleteDepositTillVault(int CustomerId);
        #endregion
    }
}
