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
    public class TransactionChargeService : ITransactionChargeService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;

        public TransactionChargeService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        public async Task<bool> AddUpdateTransactionChargeAsync(deposit_transactioncharge model)
        {
            try
            {

                if (model.TransactionChargeId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transactioncharge.FindAsync(model.TransactionChargeId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transactioncharge.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransactionChargeAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transactioncharge.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transactioncharge>> GetAllTransactionChargeAsync()
        {
            return await _dataContext.deposit_transactioncharge.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transactioncharge> GetTransactionChargeByIdAsync(int id)
        {
            return await _dataContext.deposit_transactioncharge.FindAsync(id);
        }

        public async Task<bool> UploadTransactionChargeAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<deposit_transactioncharge> uploadedRecord = new List<deposit_transactioncharge>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new deposit_transactioncharge
                        {
                            Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            FixedOrPercentage = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                            Amount_Percentage = workSheet.Cells[i, 3].Value != null ? decimal.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                            Description = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var charge = _dataContext.deposit_transactioncharge.Where(x => x.Name == item.Name && x.Deleted == false).FirstOrDefault();
                        if (charge != null)
                        {
                            charge.Name = item.Name;
                            charge.FixedOrPercentage = item.FixedOrPercentage;
                            charge.Amount_Percentage = item.Amount_Percentage;
                            charge.Description = item.Description;
                            charge.Active = true;
                            charge.Deleted = false;
                            charge.UpdatedBy = createdBy;
                            charge.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new deposit_transactioncharge
                            {
                                Name = item.Name,
                                FixedOrPercentage = item.FixedOrPercentage,
                                Amount_Percentage = item.Amount_Percentage,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_transactioncharge.AddAsync(structure);
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

        public byte[] GenerateExportTransactionCharge()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Fixed or Percentage");
            dt.Columns.Add("Amount Percentage");
            dt.Columns.Add("Description");
            var charge = (from a in _dataContext.deposit_transactioncharge
                            where a.Deleted == false
                            select new deposit_transactioncharge
                            {
                                TransactionChargeId = a.TransactionChargeId,
                                Name = a.Name,
                                FixedOrPercentage = a.FixedOrPercentage,
                                Amount_Percentage = a.Amount_Percentage,
                                Description = a.Description,
                            }).ToList();
            foreach (var kk in charge)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Fixed or Percentage"] = kk.FixedOrPercentage;
                row["Amount Percentage"] = kk.Amount_Percentage;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (charge != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Transaction Charge");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        
    }
}
