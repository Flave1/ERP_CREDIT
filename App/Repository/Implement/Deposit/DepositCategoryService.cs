using Banking.AuthHandler.Interface;
using Banking.Data;
using Banking.Repository.Interface.Deposit;
using GODP.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement.Deposit
{
    public class DepositCategoryService : IDepositCategoryService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;

        public DepositCategoryService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region DepositCategory
        public async Task<bool> AddUpdateCategoryAsync(deposit_category model)
        {
            try
            {

                if (model.CategoryId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_category.FindAsync(model.CategoryId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_category.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_category.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_category>> GetAllCategoryAsync()
        {
            return await _dataContext.deposit_category.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_category> GetCategoryByIdAsync(int id)
        {
            return await _dataContext.deposit_category.FindAsync(id);
        }

        public async Task<bool> UploadCategoryAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<deposit_category> uploadedRecord = new List<deposit_category>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new deposit_category
                        {
                            Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            Description = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.deposit_category.Where(x => x.Name == item.Name && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.Name = item.Name;
                            category.Description = item.Description;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new deposit_category
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_category.AddAsync(structure);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] GenerateExportCategory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var category = (from a in _dataContext.deposit_category
                            where a.Deleted == false
                            select new deposit_category
                            {
                                CategoryId = a.CategoryId,
                                Name = a.Name,
                                Description = a.Description,
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Cashier Teller");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion
    }
}
