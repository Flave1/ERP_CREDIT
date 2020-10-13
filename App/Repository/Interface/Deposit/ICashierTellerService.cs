using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface ICashierTellerService
    {
        #region CashierTellerSetup
        Task<bool> AddUpdateCashierTellerSetupAsync(deposit_cashiertellersetup model);
        Task<IEnumerable<deposit_cashiertellersetup>> GetAllCashierTellerSetupAsync();
        Task<deposit_cashiertellersetup> GetCashierTellerSetupByIdAsync(int id);
        Task<bool> DeleteCashierTellerSetupAsync(int id);
        Task<bool> UploadCashierTellerSetupAsync(byte[] record, string createdBy);
        byte[] GenerateExportCashierTellerSetup();
        #endregion

        #region CashierTellerForm 
        Task<bool> AddUpdateCashierTellerFormAsync(deposit_cashiertellerform model);
        Task<IEnumerable<deposit_cashiertellerform>> GetAllCashierTellerFormAsync();
        Task<deposit_cashiertellerform> GetCashierTellerFormByIdAsync(int id);
        Task<bool> DeleteCashierTellerFormAsync(int id);
        #endregion
    }
}
