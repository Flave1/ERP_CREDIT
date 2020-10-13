using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface ICategoryService
    {
        Task<bool> AddUpdateCategoryAsync(deposit_category model);

        Task<bool> DeleteCategoryAsync(int id);

        Task<IEnumerable<deposit_category>> GetAllCategoryAsync();

        Task<deposit_category> GetCategoryByIdAsync(int id);

        Task<bool> UploadcategoryAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportCategory();
    }
}
