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
    public class TransactionTaxService : ITransactionTaxService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;

        public TransactionTaxService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        public async Task<bool> AddUpdateTransactionTaxAsync(deposit_transactiontax model)
        {
            try
            {

                if (model.TransactionTaxId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transactiontax.FindAsync(model.TransactionTaxId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transactiontax.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransactionTaxAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transactiontax.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transactiontax>> GetAllTransactionTaxAsync()
        {
            return await _dataContext.deposit_transactiontax.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transactiontax> GetTransactionTaxByIdAsync(int id)
        {
            return await _dataContext.deposit_transactiontax.FindAsync(id);
        }

        public async Task<bool> UploadTransactionTaxAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<deposit_transactiontax> uploadedRecord = new List<deposit_transactiontax>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new deposit_transactiontax
                        {
                            Name = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            FixedOrPercentage = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                            Amount_Percentage = workSheet.Cells[i, 3].Value != null ? decimal.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                            Description = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var tax = _dataContext.deposit_transactiontax.Where(x => x.Name == item.Name && x.Deleted == false).FirstOrDefault();
                        if (tax != null)
                        {
                            tax.Name = item.Name;
                            tax.FixedOrPercentage = item.FixedOrPercentage;
                            tax.Amount_Percentage = item.Amount_Percentage;
                            tax.Description = item.Description;
                            tax.Active = true;
                            tax.Deleted = false;
                            tax.UpdatedBy = createdBy;
                            tax.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new deposit_transactiontax
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
                            await _dataContext.deposit_transactiontax.AddAsync(structure);
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

        public byte[] GenerateExportTransactionTax()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Fixed or Percentage");
            dt.Columns.Add("Amount Percentage");
            dt.Columns.Add("Description");
            var tax = (from a in _dataContext.deposit_transactiontax
                       where a.Deleted == false
                            select new deposit_transactiontax
                            {
                                TransactionTaxId = a.TransactionTaxId,
                                Name = a.Name,
                                FixedOrPercentage = a.FixedOrPercentage,
                                Amount_Percentage = a.Amount_Percentage,
                                Description = a.Description,
                            }).ToList();
            foreach (var kk in tax)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Fixed or Percentage"] = kk.FixedOrPercentage;
                row["Amount Percentage"] = kk.Amount_Percentage;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (tax != null)
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
        
    }
}
