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
    public class DepositAccountypeService : IDepositAccountypeService
    {
        private readonly DataContext _dataContext;
        public DepositAccountypeService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateAccountTypeAsync(deposit_accountype model)
        {
            try
            {
               
                if (model.AccountTypeId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_accountype.FindAsync(model.AccountTypeId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_accountype.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAccountTypeAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_accountype.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<deposit_accountype> GetAccountTypeByIdAsync(int id)
        {
            return await _dataContext.deposit_accountype.FindAsync(id);
        }
        
        public async Task<IEnumerable<deposit_accountype>> GetAllAccountTypeAsync()
        {
            return await _dataContext.deposit_accountype.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadAccountTypeAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_accountype> uploadedRecord = new List<deposit_accountype>();
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
                                var item = new deposit_accountype
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
                        var accounttypeexist = _dataContext.deposit_accountype.Where(x => x.Name == item.Name && x.Deleted == false).FirstOrDefault();
                        if (accounttypeexist != null)
                        {
                            accounttypeexist.Name = item.Name;
                            accounttypeexist.Description = item.Description;
                            accounttypeexist.Active = true;
                            accounttypeexist.Deleted = false;
                            accounttypeexist.UpdatedBy = item.UpdatedBy;
                            accounttypeexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var accountype = new deposit_accountype
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_accountype.AddAsync(accountype);
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
                                                                                        
        public byte[] GenerateExportAccountType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var category = (from a in _dataContext.deposit_accountype
                            where a.Deleted == false
                            select new deposit_accountype
                            {
                                Name = a.Name,
                                AccountTypeId = a.AccountTypeId,
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
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Account Type");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}
