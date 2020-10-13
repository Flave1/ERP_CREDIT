using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IBusinessCategoryService
    {
        Task<bool> AddUpdateBusinessCategoryAsync(deposit_businesscategory model);
        Task<IEnumerable<deposit_businesscategory>> GetAllBusinessCategoryAsync();
        Task<deposit_businesscategory> GetBusinessCategoryByIdAsync(int id);
        Task<bool> DeleteBusinessCategoryAsync(int id);
        Task<bool> UploadBusinessCategoryAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportBusinessCategory();
    }
}
