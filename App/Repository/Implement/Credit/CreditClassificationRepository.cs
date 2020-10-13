using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CreditClassificationObjs;

namespace Banking.Repository.Implement.Credit
{
    public class CreditClassificationRepository : ICreditClassification
    {
        private readonly DataContext _dataContext;
        public CreditClassificationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateCreditClassificationAsync(credit_creditclassification model)
        {
            try
            {
                if (model.CreditClassificationId > 0)
                {
                    var itemToUpdate = await _dataContext.credit_creditclassification.FindAsync(model.CreditClassificationId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.credit_creditclassification.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCreditClassificationAsync(int id)
        {
            var itemToDelete = await _dataContext.credit_creditclassification.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<credit_creditclassification>> GetAllCreditClassificationAsync()
        {
            return await _dataContext.credit_creditclassification.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<credit_creditclassification> GetCreditClassificationByIdAsync(int id)
        {
            return await _dataContext.credit_creditclassification.Where(x => x.Deleted == false && x.CreditClassificationId == id).FirstOrDefaultAsync();
        }

        public Task<bool> UploadCreditClassificationAsync(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateExportCreditClassification()
        {
            throw new NotImplementedException();
        }



        //////////////OPERATING ACCOUNT SETUP
        ///
        public async Task<OperatingAccountObj> GetOperatingAccount()
        {
            var operatingAccount = await _dataContext.deposit_accountsetup.FirstOrDefaultAsync(x => x.DepositAccountId == 3);   //////Operating Account ID is 3

            return new OperatingAccountObj
            {
                OperatingAccountId = operatingAccount.DepositAccountId,
                OperatingAccountName = operatingAccount.AccountName,
                CasaGL = operatingAccount.GLMapping,
                CashAndBankGL = operatingAccount.BankGl,
                InUse = operatingAccount.InUse,
            };
        }

        public async Task<bool> AddOrUpdateOperatingAccountAsync(OperatingAccountObj entity)
        {
            if (entity.OperatingAccountId > 0)
            {
                var account = await _dataContext.deposit_accountsetup.FirstOrDefaultAsync(x => x.DepositAccountId == entity.OperatingAccountId);

                account.AccountName = entity.OperatingAccountName;
                account.GLMapping = entity.CashAndBankGL;
                account.BankGl = entity.CasaGL;
                account.InUse = entity.InUse;
                account.Active = true;
                account.Deleted = false;
                account.UpdatedBy = entity.UpdatedBy;
                account.UpdatedOn = DateTime.Now;
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
