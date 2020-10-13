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
    public class WithdrawalService : IWithdrawalService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        public WithdrawalService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region Withdrawaletup
        public async Task<bool> AddUpdateWithdrawalSetupAsync(deposit_withdrawalsetup model)
        {
            try
            {

                if (model.WithdrawalSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_withdrawalsetup.FindAsync(model.WithdrawalSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_withdrawalsetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteWithdrawalSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalsetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_withdrawalsetup>> GetAllWithdrawalSetupAsync()
        {
            return await _dataContext.deposit_withdrawalsetup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_withdrawalsetup> GetWithdrawalSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalsetup.FindAsync(id);
        }

        public async Task<bool> UploadWithdrawalSetupAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_withdrawalsetup> uploadedRecord = new List<deposit_withdrawalsetup>();
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
                                var item = new deposit_withdrawalsetup
                                {
                                    //can = workSheet.Cells[i, 1].Value.ToString(),
                                    //CanApply = workSheet.Cells[i, 2].Value.ToString()
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
                        var Withdrawalexist = _dataContext.deposit_withdrawalsetup.Where(x => x.WithdrawalSetupId == item.WithdrawalSetupId && x.Deleted == false).FirstOrDefault();
                        if (Withdrawalexist != null)
                        {
                            Withdrawalexist.Product = item.Product;
                            //Withdrawalexist.can = item.CanApply;
                            Withdrawalexist.Active = true;
                            Withdrawalexist.Deleted = false;
                            Withdrawalexist.UpdatedBy = item.UpdatedBy;
                            Withdrawalexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Withdrawal = new deposit_withdrawalsetup
                            {
                                Product = item.Product,
                                //Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_withdrawalsetup.AddAsync(Withdrawal);
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
        public byte[] GenerateExportWithdrawalSetup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Withdrawal = (from a in _dataContext.deposit_withdrawalsetup
                            where a.Deleted == false
                            select new deposit_withdrawalsetup
                            {
                                Product = a.Product,
                                //WithdrawalId = a.WithdrawalId,
                                //Description = a.Description
                            }).ToList();
            foreach (var kk in Withdrawal)
            {
                var row = dt.NewRow();
                row["Can Apply"] = kk.Product;
                //row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Withdrawal != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Withdrawal");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region WithdrawalForm
        public async Task<bool> AddUpdateWithdrawalAsync(deposit_withdrawalform model)
        {
            try
            {

                if (model.WithdrawalFormId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_withdrawalform.FindAsync(model.WithdrawalFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_withdrawalform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteWithdrawalAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_withdrawalform>> GetAllWithdrawalAsync()
        {
            return await _dataContext.deposit_withdrawalform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_withdrawalform> GetWithdrawalByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalform.FindAsync(id);
        }

        /*public async Task<bool> UploadWithdrawalAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_withdrawalform> uploadedRecord = new List<deposit_withdrawalform>();
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
                                var item = new deposit_Withdrawal
                                {
                                    can = workSheet.Cells[i, 1].Value.ToString(),
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
                        var Withdrawalexist = _dataContext.deposit_withdrawalform.Where(x => x.WithdrawalId == item.WithdrawalId && x.Deleted == false).FirstOrDefault();
                        if (Withdrawalexist != null)
                        {
                            Withdrawalexist.Name = item.Name;
                            Withdrawalexist.Description = item.Description;
                            Withdrawalexist.Active = true;
                            Withdrawalexist.Deleted = false;
                            Withdrawalexist.UpdatedBy = item.UpdatedBy;
                            Withdrawalexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Withdrawal = new deposit_Withdrawal
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_withdrawalform.AddAsync(Withdrawal);
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
        public byte[] GenerateExportWithdrawal()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Withdrawal = (from a in _dataContext.deposit_Withdrawal
                                 where a.Deleted == false
                                 select new deposit_Withdrawal
                                 {
                                     Name = a.Name,
                                     WithdrawalId = a.WithdrawalId,
                                     Description = a.Description
                                 }).ToList();
            foreach (var kk in Withdrawal)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Withdrawal != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Withdrawal");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }*/
        #endregion
    }
}
