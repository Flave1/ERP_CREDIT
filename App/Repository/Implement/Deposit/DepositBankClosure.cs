using Banking.Data;
using Banking.DomainObjects;
using Banking.Repository.Interface.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement
{
    public class DepositBankClosure : IDepositBankClosure
    {
        private readonly DataContext _dataContext;

        public bool AddUpdateBankClosureSetup(deposit_bankclosuresetup entity)
        {
            if (entity.BankClosureSetupId > 0)
            {
                var item = _dataContext.deposit_bankclosuresetup.Find(entity.BankClosureSetupId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                _dataContext.deposit_bankclosuresetup.Add(entity);
            return _dataContext.SaveChanges() > 0;
        }

        public bool AddUpdateDepositBankClosure(deposit_bankclosure entity)
        {
            if (entity.BankClosureId > 0)
            {
                var item = _dataContext.deposit_bankclosure.Find(entity.BankClosureId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                _dataContext.deposit_bankclosure.Add(entity);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteBankClosureSetup(int CustomerId)
        {
            var item = _dataContext.deposit_bankclosuresetup.Find(CustomerId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(item);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteDepositBankClosure(int CustomerId)
        {
            var item = _dataContext.deposit_bankclosure.Find(CustomerId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(item);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteMultipleDepositBankClosure(List<int> deposit_bankclosureIds)
        {
            throw new NotImplementedException();
        }

       

        public deposit_bankclosure GetDepositBankClosure(int id)
        {
            throw new NotImplementedException();
        }

        public bool UploadBankClosure(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }


        public byte[] GenerateExportBankClosure()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_bankclosuresetup> GetAllBankClosureSetup()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_bankclosure> GetAllDepositBankClosure()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<deposit_bankclosure>> GetAllDepositBankClosureAsync()
        {
            throw new NotImplementedException();
        }

        public deposit_bankclosuresetup GetBankClosureSetup(int id)
        {
            throw new NotImplementedException();
        }
    }
}
