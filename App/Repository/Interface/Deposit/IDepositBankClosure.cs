using Banking.DomainObjects;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IDepositBankClosure
    {
        #region DepositBankClosureSetup
        bool AddUpdateBankClosureSetup(deposit_bankclosuresetup entity);
        IEnumerable<deposit_bankclosuresetup> GetAllBankClosureSetup();
        deposit_bankclosuresetup GetBankClosureSetup(int id);
        byte[] GenerateExportBankClosure();
        bool UploadBankClosure(byte[] record, string createdBy);
        bool DeleteBankClosureSetup(int CustomerId);
        #endregion

        #region DepositBankClosure
        bool AddUpdateDepositBankClosure(deposit_bankclosure entity);
        Task<IEnumerable<deposit_bankclosure>> GetAllDepositBankClosureAsync();
        deposit_bankclosure GetDepositBankClosure(int id); 
        bool DeleteDepositBankClosure(int CustomerId);
        #endregion
    }
}
