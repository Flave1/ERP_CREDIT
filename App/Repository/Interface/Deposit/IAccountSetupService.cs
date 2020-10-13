using Banking.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IAccountSetupService
    {
        Task<bool> AddUpdateAccountSetupAsync(deposit_accountsetup model);
        Task<bool> DeleteAccountSetupAsync(int id);
        Task<IEnumerable<DepositAccountObj>> GetAllAccountSetupAsync();
        Task<DepositAccountObj> GetAccountSetupByIdAsync(int id);
        Task<bool> UploadAccountSetupAsync(byte[] record, string createdBy);
        byte[] GenerateExportAccountSetup();


        bool AddUpdateDepositForm(DepositformObj entity);
        IEnumerable<DepositformObj> GetAllDepositForm();
        DepositformObj GetDepositForm(int id);
        bool DeleteDepositionForm(int id);
    }
}
