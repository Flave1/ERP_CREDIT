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
    public class TransferService : ITransferService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        public TransferService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region TransferSetup
        public async Task<bool> AddUpdateTransferSetupAsync(deposit_transfersetup model)
        {
            try
            {

                if (model.TransferSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transfersetup.FindAsync(model.TransferSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transfersetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransferSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transfersetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transfersetup>> GetAllTransferSetupAsync()
        {
            return await _dataContext.deposit_transfersetup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transfersetup> GetTransferSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_transfersetup.FindAsync(id);
        }

        public async Task<bool> UploadTransferSetupAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_transfersetup> uploadedRecord = new List<deposit_transfersetup>();
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
                                var item = new deposit_transfersetup
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
                        var Transferexist = _dataContext.deposit_transfersetup.Where(x => x.TransferSetupId == item.TransferSetupId && x.Deleted == false).FirstOrDefault();
                        if (Transferexist != null)
                        {
                            Transferexist.Product = item.Product;
                            //Transferexist.can = item.CanApply;
                            Transferexist.Active = true;
                            Transferexist.Deleted = false;
                            Transferexist.UpdatedBy = item.UpdatedBy;
                            Transferexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Transfer = new deposit_transfersetup
                            {
                                Product = item.Product,
                                //Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_transfersetup.AddAsync(Transfer);
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
        public byte[] GenerateExportTransferSetup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Transfer = (from a in _dataContext.deposit_transfersetup
                                 where a.Deleted == false
                                 select new deposit_transfersetup
                                 {
                                     Product = a.Product,
                                     //TransferId = a.TransferId,
                                     //Description = a.Description
                                 }).ToList();
            foreach (var kk in Transfer)
            {
                var row = dt.NewRow();
                row["Can Apply"] = kk.Product;
                //row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Transfer != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Transfer");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region TransferForm
        public async Task<bool> AddUpdateTransferAsync(deposit_transferform model)
        {
            try
            {

                if (model.TransferFormId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transferform.FindAsync(model.TransferFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transferform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransferAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transferform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transferform>> GetAllTransferAsync()
        {
            return await _dataContext.deposit_transferform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transferform> GetTransferByIdAsync(int id)
        {
            return await _dataContext.deposit_transferform.FindAsync(id);
        }

        /*public async Task<bool> UploadTransferAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_transferform> uploadedRecord = new List<deposit_transferform>();
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
                                var item = new deposit_Transfer
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
                        var Transferexist = _dataContext.deposit_transferform.Where(x => x.TransferId == item.TransferId && x.Deleted == false).FirstOrDefault();
                        if (Transferexist != null)
                        {
                            Transferexist.Name = item.Name;
                            Transferexist.Description = item.Description;
                            Transferexist.Active = true;
                            Transferexist.Deleted = false;
                            Transferexist.UpdatedBy = item.UpdatedBy;
                            Transferexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Transfer = new deposit_Transfer
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_transferform.AddAsync(Transfer);
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
        public byte[] GenerateExportTransfer()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Transfer = (from a in _dataContext.deposit_Transfer
                                 where a.Deleted == false
                                 select new deposit_Transfer
                                 {
                                     Name = a.Name,
                                     TransferId = a.TransferId,
                                     Description = a.Description
                                 }).ToList();
            foreach (var kk in Transfer)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Transfer != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Transfer");
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
