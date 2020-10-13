using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IDepositAccountypeService
    {
        Task<bool> AddUpdateAccountTypeAsync(deposit_accountype model);
        Task<IEnumerable<deposit_accountype>> GetAllAccountTypeAsync();
        Task<deposit_accountype> GetAccountTypeByIdAsync(int id);
        Task<bool> DeleteAccountTypeAsync(int id);
        Task<bool> UploadAccountTypeAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportAccountType();
    }
}
