using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.CreditClassificationObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ICreditClassification
    {
        Task<bool> AddUpdateCreditClassificationAsync(credit_creditclassification model);
        Task<IEnumerable<credit_creditclassification>> GetAllCreditClassificationAsync();
        Task<credit_creditclassification> GetCreditClassificationByIdAsync(int id);
        Task<bool> DeleteCreditClassificationAsync(int id);
        Task<bool> UploadCreditClassificationAsync(byte[] record, string createdBy);
        byte[] GenerateExportCreditClassification();
        Task<OperatingAccountObj> GetOperatingAccount();
        Task<bool> AddOrUpdateOperatingAccountAsync(OperatingAccountObj entity);
    }
}
