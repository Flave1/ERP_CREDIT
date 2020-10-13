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
    public class CashierTellerService : ICashierTellerService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;


        public CashierTellerService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region CashierTellerSetup
        public async Task<bool> AddUpdateCashierTellerSetupAsync(deposit_cashiertellersetup model)
        {
            try
            {

                if (model.DepositCashierTellerSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_cashiertellersetup.FindAsync(model.DepositCashierTellerSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_cashiertellersetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCashierTellerSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_cashiertellersetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_cashiertellersetup>> GetAllCashierTellerSetupAsync()
        {
            return await _dataContext.deposit_cashiertellersetup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_cashiertellersetup> GetCashierTellerSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_cashiertellersetup.FindAsync(id);
        }

        public async Task<bool> UploadCashierTellerSetupAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<deposit_cashiertellersetup> uploadedRecord = new List<deposit_cashiertellersetup>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new deposit_cashiertellersetup
                        {
                            Structure = workSheet.Cells[i, 1].Value != null ? int.Parse(workSheet.Cells[i, 1].Value.ToString()) : 0,
                            ProductId = workSheet.Cells[i, 2].Value != null ? int.Parse(workSheet.Cells[i, 2].Value.ToString()) : 0,
                            PresetChart = workSheet.Cells[i, 3].Value != null ? bool.Parse(workSheet.Cells[i, 3].Value.ToString()) : false,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.deposit_cashiertellersetup.Where(x => x.PresetChart == item.PresetChart && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.Structure = item.Structure;
                            category.ProductId = item.ProductId;
                            category.PresetChart = item.PresetChart;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new deposit_cashiertellersetup
                            {
                                Structure = item.Structure,
                                ProductId = item.ProductId,
                                PresetChart = item.PresetChart,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_cashiertellersetup.AddAsync(structure);
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

        public byte[] GenerateExportCashierTellerSetup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Company Name");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Preset Chart");
            var category = (from a in _dataContext.deposit_cashiertellersetup
                            where a.Deleted == false
                            select new deposit_cashiertellersetup
                            {
                                Structure = a.Structure,
                                ProductId = a.ProductId,
                                PresetChart = a.PresetChart
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Company Name"] = kk.Structure;
                row["Product Name"] = kk.ProductId;
                row["Preset Chart"] = kk.PresetChart;
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

        #region CashierTellerForm
        public async Task<bool> AddUpdateCashierTellerFormAsync(deposit_cashiertellerform model)
        {
            try
            {

                if (model.DepositCashierTellerId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_cashiertellerform.FindAsync(model.DepositCashierTellerId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_cashiertellerform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCashierTellerFormAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_cashiertellerform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_cashiertellerform>> GetAllCashierTellerFormAsync()
        {
            return await _dataContext.deposit_cashiertellerform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_cashiertellerform> GetCashierTellerFormByIdAsync(int id)
        {
            return await _dataContext.deposit_cashiertellerform.FindAsync(id);
        }

        #endregion
    }
}

