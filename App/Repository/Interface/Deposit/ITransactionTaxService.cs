using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface ITransactionTaxService
    {
        Task<bool> AddUpdateTransactionTaxAsync(deposit_transactiontax model);
        Task<bool> DeleteTransactionTaxAsync(int id);
        Task<IEnumerable<deposit_transactiontax>> GetAllTransactionTaxAsync();
        Task<deposit_transactiontax> GetTransactionTaxByIdAsync(int id);
        Task<bool> UploadTransactionTaxAsync(byte[] record, string createdBy);
        byte[] GenerateExportTransactionTax();
    }
}
