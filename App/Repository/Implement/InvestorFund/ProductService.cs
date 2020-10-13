using Banking.Contracts.Response.InvestorFund;
using Banking.Data;
using Banking.DomainObjects.InvestorFund;
using Banking.Repository.Interface.InvestorFund;
using GOSLibraries.GOS_API_Response;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement.InvestorFund
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;
        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Product
        public bool AddUpdateProduct(inf_product model)
        {
            try
            {
                if (model.ProductId > 0)
                {
                    var itemToUpdate = _dataContext.inf_product.FindAsync(model.ProductId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    _dataContext.inf_product.Add(model);
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProduct(int id)
        {
            var itemToDelete = _dataContext.inf_product.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportProduct()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Product Code");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Product Type");
            dt.Columns.Add("Product Limit");
            dt.Columns.Add("Interest Repayment Type");
            dt.Columns.Add("Schedule Method");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Maximum Period");
            dt.Columns.Add("Interest Rate Annual");
            dt.Columns.Add("Liquidation Charge");
            var statementType = (from a in _dataContext.inf_product
                                 where a.Deleted == false
                                 select new InfProductObj
                                 {
                                     ProductCode = a.ProductCode,
                                     ProductName = a.ProductName,
                                     ProductTypeId = a.ProductTypeId,
                                     ProductLimit = a.ProductLimit,
                                     InterestRepaymentTypeId = a.InterestRepaymentTypeId,
                                     ScheduleMethodId = a.ScheduleMethodId,
                                     FrequencyId = a.FrequencyId,
                                     MaximumPeriod = a.MaximumPeriod,
                                     InterestRateAnnual = a.InterestRateAnnual,
                                     EarlyTerminationCharge = a.EarlyTerminationCharge
                                 }).ToList();
            foreach (var kk in statementType)
            {
                var frequency = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == kk.FrequencyId).FirstOrDefault();
                var producttype = _dataContext.inf_producttype.Where(x => x.ProductTypeId == kk.ProductTypeId).FirstOrDefault();
                var interestRepaymentType = _dataContext.credit_repaymenttype.Where(x => x.RepaymentTypeId == kk.InterestRepaymentTypeId).FirstOrDefault();
                var scheduleMethod = _dataContext.credit_loanscheduletype.Where(x => x.LoanScheduleTypeId == kk.ScheduleMethodId).FirstOrDefault();
                var row = dt.NewRow();
                row["Product Code"] = kk.ProductCode;
                row["Product Name"] = kk.ProductName;
                row["Product Type"] = producttype.Name;
                row["Product Limit"] = kk.ProductLimit;
                row["Interest Repayment Type"] = interestRepaymentType.RepaymentTypeName;
                row["Schedule Method"] = scheduleMethod.LoanScheduleTypeName;
                row["Frequency"] = frequency.Mode;
                row["Maximum Period"] = kk.MaximumPeriod;
                row["Interest Rate Annual"] = kk.InterestRateAnnual;
                row["Liquidation Charge"] = kk.EarlyTerminationCharge;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Product");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public List<InfProductObj> GetAllProduct()
        {
            try
            {
                var product = (from a in _dataContext.inf_product
                               where a.Deleted == false
                               select

                               new InfProductObj
                               {
                                   ProductId = a.ProductId,
                                   ProductCode = a.ProductCode,
                                   ProductName = a.ProductName,
                                   Rate = a.Rate,
                                   ProductTypeId = a.ProductTypeId,
                                   ProductLimit = a.ProductLimit,
                                   ProductLimitId = a.ProductLimitId,
                                   InterestRateMax = a.InterestRateMax,
                                   InterestRepaymentTypeId = a.InterestRepaymentTypeId,
                                   ScheduleMethodId = a.ScheduleMethodId,
                                   FrequencyId = a.FrequencyId,
                                   FrequencyName = _dataContext.credit_frequencytype.FirstOrDefault(x=>x.FrequencyTypeId == a.FrequencyId).Mode,
                                   MaximumPeriod = a.MaximumPeriod,
                                   InterestRateAnnual = a.InterestRateAnnual,
                                   InterestRateFrequency = a.InterestRateFrequency,
                                   ProductPrincipalGl = a.ProductPrincipalGl,
                                   ReceiverPrincipalGl = a.ReceiverPrincipalGl,
                                   InterstExpenseGl = a.InterstExpenseGl,
                                   InterestPayableGl = a.InterestPayableGl,
                                   EarlyTerminationCharge = a.EarlyTerminationCharge,
                               }).ToList();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InfProductObj GetProduct(int Id)
        {
            try
            {
                var product = (from a in _dataContext.inf_product
                               where a.Deleted == false && a.ProductId == Id
                               select

                              new InfProductObj
                              {
                                  ProductId = a.ProductId,
                                  ProductCode = a.ProductCode,
                                  ProductName = a.ProductName,
                                  Rate = a.Rate,
                                  ProductTypeId = a.ProductTypeId,
                                  ProductLimit = a.ProductLimit,
                                  ProductLimitId = a.ProductLimitId,
                                  InterestRateMax = a.InterestRateMax,
                                  InterestRepaymentTypeId = a.InterestRepaymentTypeId,
                                  ScheduleMethodId = a.ScheduleMethodId,
                                  FrequencyId = a.FrequencyId,
                                  MaximumPeriod = a.MaximumPeriod,
                                  InterestRateAnnual = a.InterestRateAnnual,
                                  InterestRateFrequency = a.InterestRateFrequency,
                                  ProductPrincipalGl = a.ProductPrincipalGl,
                                  ReceiverPrincipalGl = a.ReceiverPrincipalGl,
                                  InterstExpenseGl = a.InterstExpenseGl,
                                  InterestPayableGl = a.InterestPayableGl,
                                  EarlyTerminationCharge = a.EarlyTerminationCharge,
                              }).FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public InfProductRegRespObj UploadProduct(List<byte[]> record, string createdBy)
        {
            try
            {
                List<InfProductObj> uploadedRecord = new List<InfProductObj>();
                if (record == null) return new InfProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                };
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                                var item = new InfProductObj
                                {
                                    ProductCode = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    ProductName = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                                    ProductTypeName = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                                    ProductLimit = workSheet.Cells[i, 4].Value != null ? int.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0,
                                    InterestRepaymentTypeName = workSheet.Cells[i, 5].Value != null ? workSheet.Cells[i, 5].Value.ToString() : null,
                                    ScheduleMethodName = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : null,
                                    FrequencyName = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
                                    MaximumPeriod = workSheet.Cells[i, 8].Value != null ? decimal.Parse(workSheet.Cells[i, 8].Value.ToString()) : 0,
                                    InterestRateAnnual = workSheet.Cells[i, 9].Value != null ? decimal.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
                                    EarlyTerminationCharge = workSheet.Cells[i, 10].Value != null ? decimal.Parse(workSheet.Cells[i, 10].Value.ToString()) : 0,
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
                        if (item.ProductName == "" || item.InterestRepaymentTypeName == "" || item.ProductTypeName == "" || item.FrequencyName == "" || item.ScheduleMethodName == "")
                        {
                            return new InfProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include all fields" } }
                            };
                        }
                        var productId = 0;
                        var productobj = _dataContext.inf_product.Where(x => x.ProductName.ToLower().Trim() == item.ProductName.ToLower().Trim()).FirstOrDefault();
                        if (productobj != null)
                        {
                            productId = productobj.ProductId;
                        }
                        var productType = _dataContext.inf_producttype.Where(x => x.Name.ToLower().Trim() == item.ProductTypeName.ToLower().Trim()).FirstOrDefault();
                        if (productType == null)
                        {
                            return new InfProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid product type name" } }
                            };
                        }
                        var frequencyType = _dataContext.credit_frequencytype.Where(x => x.Mode.ToLower().Trim() == item.FrequencyName.ToLower().Trim()).FirstOrDefault();
                        if (frequencyType == null)
                        {
                            return new InfProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid frequency type name" } }
                            };
                        }
                        var interestRepayment = _dataContext.credit_repaymenttype.Where(x => x.RepaymentTypeName.ToLower().Trim() == item.InterestRepaymentTypeName.ToLower().Trim()).FirstOrDefault();
                        if (interestRepayment == null)
                        {
                            return new InfProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid interestRepayment type name" } }
                            };
                        }
                        var ScheduleType = _dataContext.credit_loanscheduletype.Where(x => x.LoanScheduleTypeName.ToLower().Trim() == item.ScheduleMethodName.ToLower().Trim()).FirstOrDefault();
                        if (ScheduleType == null)
                        {
                            return new InfProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid ScheduleType name" } }
                            };
                        }
                        var productexist = _dataContext.inf_product.Find(productId);
                        if (productexist != null)
                        {
                            productexist.ProductCode = item.ProductCode;
                            productexist.ProductName = item.ProductName;
                            productexist.ProductTypeId = productType.ProductTypeId;
                            productexist.ProductLimit = item.ProductLimit;
                            productexist.InterestRepaymentTypeId = interestRepayment.RepaymentTypeId;
                            productexist.ScheduleMethodId = ScheduleType.LoanScheduleTypeId;
                            productexist.FrequencyId = frequencyType.FrequencyTypeId;
                            productexist.MaximumPeriod = item.MaximumPeriod;
                            productexist.InterestRateAnnual = item.InterestRateAnnual;
                            productexist.ProductPrincipalGl = item.ProductPrincipalGl;
                            productexist.ReceiverPrincipalGl = item.ReceiverPrincipalGl;
                            productexist.InterstExpenseGl = item.InterstExpenseGl;
                            productexist.InterestPayableGl = item.InterestPayableGl;
                            productexist.EarlyTerminationCharge = item.EarlyTerminationCharge;
                            productexist.Active = true;
                            productexist.Deleted = false;
                            productexist.Updatedby = createdBy;
                            productexist.Updatedon = DateTime.Now;
                        }
                        else
                        {
                            var product = new inf_product
                            {
                                ProductCode = item.ProductCode,
                                ProductName = item.ProductName,
                                ProductTypeId = productType.ProductTypeId,
                                ProductLimit = item.ProductLimit,
                                InterestRepaymentTypeId = interestRepayment.RepaymentTypeId,
                                ScheduleMethodId = ScheduleType.LoanScheduleTypeId,
                                FrequencyId = frequencyType.FrequencyTypeId,
                                MaximumPeriod = item.MaximumPeriod,
                                InterestRateAnnual = item.InterestRateAnnual,
                                ProductPrincipalGl = item.ProductPrincipalGl,
                                ReceiverPrincipalGl = item.ReceiverPrincipalGl,
                                InterstExpenseGl = item.InterstExpenseGl,
                                InterestPayableGl = item.InterestPayableGl,
                                EarlyTerminationCharge = item.EarlyTerminationCharge,
                                Active = true,
                                Deleted = false,
                                Createdby = createdBy,
                                Createdon = DateTime.Now,
                            };
                            _dataContext.inf_product.Add(product);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return new InfProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ProductType
        public bool AddUpdateProductType(inf_producttype model)
        {
            try
            {
                if (model.ProductTypeId > 0)
                {
                    var itemToUpdate = _dataContext.inf_producttype.Find(model.ProductTypeId);
                     //_dataContext.inf_producttype.Add(itemToUpdate);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    _dataContext.inf_producttype.Add(model);
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProductType(int id)
        {
            var itemToDelete = _dataContext.inf_producttype.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportProductType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var statementType = (from a in _dataContext.inf_producttype
                                 where a.Deleted == false
                                 select new InfProductTypeObj
                                 {
                                     Name = a.Name,
                                     Description = a.Description
                                 }).ToList();

            foreach (var kk in statementType)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProductType");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public List<InfProductTypeObj> GetAllProductType()
        {
            try
            {
                var producttype = (from a in _dataContext.inf_producttype
                               where a.Deleted == false
                               select

                               new InfProductTypeObj
                               {
                                   ProductTypeId = a.ProductTypeId,
                                   Name = a.Name,
                                   Description = a.Description,
                               }).ToList();

                return producttype;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InfProductTypeObj GetProductType(int Id)
        {
            try
            {
                var product = (from a in _dataContext.inf_producttype
                               where a.Deleted == false && a.ProductTypeId == Id
                               select

                              new InfProductTypeObj
                              {
                                  ProductTypeId = a.ProductTypeId,
                                  Name = a.Name,
                                  Description = a.Description,
                              }).FirstOrDefault();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public InfProductTypeRegRespObj UploadProductType(List<byte[]> record, string createdBy)
        {
            try
            {
                List<inf_producttype> uploadedRecord = new List<inf_producttype>();
                if (record == null) return new InfProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                };
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                                var item = new inf_producttype
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
                        var producttypeexist = _dataContext.inf_producttype.FirstOrDefault(x=>x.Name.ToLower().Trim() == item.Name.ToLower().Trim());
                        if (producttypeexist != null)
                        {
                            producttypeexist.Name = item.Name;
                            producttypeexist.Description = item.Description;
                            producttypeexist.Active = true;
                            producttypeexist.Deleted = false;
                            producttypeexist.Updatedby = item.Updatedby;
                            producttypeexist.Updatedon = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new inf_producttype
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                Createdby = createdBy,
                                Createdon = DateTime.Now,
                            };
                            _dataContext.inf_producttype.Add(accountType);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return new InfProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}




