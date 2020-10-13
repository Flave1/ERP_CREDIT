using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface ITransferService
    {
        #region Transferetup
        Task<bool> AddUpdateTransferSetupAsync(deposit_transfersetup model);

        Task<bool> DeleteTransferSetupAsync(int id);

        Task<IEnumerable<deposit_transfersetup>> GetAllTransferSetupAsync();

        Task<deposit_transfersetup> GetTransferSetupByIdAsync(int id);

        Task<bool> UploadTransferSetupAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportTransferSetup();
        #endregion
         

        #region TransferForm
        Task<bool> AddUpdateTransferAsync(deposit_transferform model);

        Task<bool> DeleteTransferAsync(int id);

        Task<IEnumerable<deposit_transferform>> GetAllTransferAsync();

        Task<deposit_transferform> GetTransferByIdAsync(int id);

        /*Task<bool> UploadTransferAsync(List<byte[]> record, string createdBy);
        public byte[] GenerateExportTransfer();*/
        #endregion
    }
}
