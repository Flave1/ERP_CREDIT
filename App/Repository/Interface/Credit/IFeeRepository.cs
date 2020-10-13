using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Credit
{
    public interface IFeeRepository
    {
        Task<bool> AddUpdateFeeAsync(credit_fee model);
        Task<IEnumerable<credit_fee>> GetAllFeeAsync();
        Task<IEnumerable<credit_fee>> GetAllIntegralFeeAsync();
        Task<credit_fee> GetFeeByIdAsync(int id);
        Task<bool> DeleteFeeAsync(int id);
        Task<bool> UploadFeeAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportFees();

        Task<IEnumerable<credit_repaymenttype>> GetAllRepaymentTypeAsync();
    }
}
