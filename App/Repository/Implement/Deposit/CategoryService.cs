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
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;


        public CategoryService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }
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

        public async Task<bool> UploadcategoryAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_category> uploadedRecord = new List<deposit_category>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new deposit_category
                                {
                                    Name = workSheet.Cells[i, 1].Value.ToString(),
                                    Description = workSheet.Cells[i, 2].Value.ToString()
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }

                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var categoryexist = _dataContext.deposit_category.Where(x => x.CategoryId == item.CategoryId && x.Deleted == false).FirstOrDefault();
                        if (categoryexist != null)
                        {
                            categoryexist.Name = item.Name;
                            categoryexist.Description = item.Description;
                            categoryexist.Active = true;
                            categoryexist.Deleted = false;
                            categoryexist.UpdatedBy = item.UpdatedBy;
                            categoryexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var category = new deposit_category
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_category.AddAsync(category);
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
                                Name = a.Name,
                                CategoryId = a.CategoryId,
                                Description = a.Description
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
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" category");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}
