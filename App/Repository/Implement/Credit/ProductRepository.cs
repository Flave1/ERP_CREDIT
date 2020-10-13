using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Repository.Implement.Credit
{
    public class ProductRepository : IProduct
    {
        private readonly DataContext _dataContext;
        private readonly Dictionary<int, string> fees;
        private readonly IIdentityServerRequest _serverRequest;
        public ProductRepository(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            fees = new Dictionary<int, string>()
            {
                { 1, "Fixed" },
                { 2, "Percentage" }
            };
        }
        //Product
        public ProductObj AddUpdateProduct(ProductObj entity)
        {
            try
            {
                //if (entity == null) return false;
                if (entity == null) return null;
                var tenor = GetTenorByPeriodAndFrequency((int)entity.Period, (int)entity.FrequencyTypeId);
                credit_product product = null;
                if (entity.ProductId > 0)
                {
                    product = _dataContext.credit_product.Find(entity.ProductId);
                    if (product != null)
                    {
                        product.ProductCode = entity.ProductCode;
                        product.ProductName = entity.ProductName;
                        product.PaymentType = entity.PaymentType;
                        product.InterestType = entity.InterestType;
                        product.Rate = entity.Rate;
                        product.LowRiskDefinition = entity.LowRiskDefinition;
                        product.Period = entity.Period;
                        product.CleanUpCircle = entity.CleanUpCircle;
                        product.LateTerminationCharge = entity.LateTerminationCharge;
                        product.EarlyTerminationCharge = entity.EarlyTerminationCharge;
                        product.WeightedMaxScore = entity.WeightedMaxScore;
                        product.ProductLimit = entity.ProductLimit;
                        product.Default = entity.Defaultvalue;
                        product.DefaultRange = entity.DefaultRange;
                        product.Significant2 = entity.Significant2;
                        product.Significant3 = entity.Significant3;
                        product.ProductTypeId = entity.ProductTypeId;
                        product.PrincipalGL = entity.PrincipalGL;
                        product.InterestIncomeExpenseGL = entity.InterestIncomeExpenseGL;
                        product.InterestReceivablePayableGL = entity.InterestReceivablePayableGL;
                        product.FrequencyTypeId = entity.FrequencyTypeId;
                        product.TenorInDays = tenor;
                        product.ScheduleTypeId = entity.ScheduleTypeId;
                        product.CollateralPercentage = entity.CollateralPercentage;
                        product.FeeIncomeGL = entity.FeeIncomeGL;
                        product.Active = true;
                        product.Deleted = false;
                        product.UpdatedBy = entity.CreatedBy;
                        product.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    product = new credit_product
                    {
                        ProductCode = entity.ProductCode,
                        ProductName = entity.ProductName,
                        Rate = entity.Rate,
                        PaymentType = entity.PaymentType,
                        Period = entity.Period,
                        CleanUpCircle = entity.CleanUpCircle,
                        WeightedMaxScore = entity.WeightedMaxScore,
                        ProductLimit = entity.ProductLimit,
                        InterestType = entity.InterestType,
                        LateTerminationCharge = entity.LateTerminationCharge,
                        EarlyTerminationCharge = entity.EarlyTerminationCharge,
                        LowRiskDefinition = entity.LowRiskDefinition,
                        Default = entity.Defaultvalue,
                        DefaultRange = entity.DefaultRange,
                        Significant2 = entity.Significant2,
                        Significant3 = entity.Significant3,
                        ProductTypeId = entity.ProductTypeId,
                        PrincipalGL = entity.PrincipalGL,
                        InterestIncomeExpenseGL = entity.InterestIncomeExpenseGL,
                        InterestReceivablePayableGL = entity.InterestReceivablePayableGL,
                        FrequencyTypeId = entity.FrequencyTypeId,
                        ScheduleTypeId = entity.ScheduleTypeId,
                        CollateralPercentage = entity.CollateralPercentage,
                        TenorInDays = tenor,
                        FeeIncomeGL = entity.FeeIncomeGL,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_product.Add(product);
                }

                using (var trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var output = _dataContext.SaveChanges() > 0;

                        if (output)
                        {
                            trans.Commit();
                        }
                        return new ProductObj
                        {
                            ProductId = product.ProductId,
                            ProductCode = entity.ProductCode,
                            ProductName = entity.ProductName,
                            Rate = entity.Rate,
                            PaymentType = entity.PaymentType,
                            Period = entity.Period,
                            CleanUpCircle = entity.CleanUpCircle,
                            LateTerminationCharge = entity.LateTerminationCharge,
                            EarlyTerminationCharge = entity.EarlyTerminationCharge,
                            LowRiskDefinition = entity.LowRiskDefinition,
                            Defaultvalue = entity.Defaultvalue,
                            DefaultRange = entity.DefaultRange,
                            Significant2 = entity.Significant2,
                            Significant3 = entity.Significant3,
                            ProductTypeId = entity.ProductTypeId,
                            PrincipalGL = entity.PrincipalGL,
                            InterestIncomeExpenseGL = entity.InterestIncomeExpenseGL,
                            InterestReceivablePayableGL = entity.InterestReceivablePayableGL,
                            FrequencyTypeId = entity.FrequencyTypeId,
                            ScheduleTypeId = entity.ScheduleTypeId,
                            CollateralPercentage = entity.CollateralPercentage,
                            WeightedMaxScore = entity.WeightedMaxScore,
                            ProductLimit = entity.ProductLimit,
                            Active = true,
                            Deleted = false,
                            CreatedBy = entity.CreatedBy,
                            CreatedOn = DateTime.Now,
                            UpdatedBy = entity.CreatedBy,
                            UpdatedOn = DateTime.Now,
                        };

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteProduct(int Id)
        {
            var itemToDelete = await _dataContext.credit_product.FindAsync(Id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public byte[] GenerateExportProduct()
        {            
            DataTable dt = new DataTable();
            dt.Columns.Add("Product Code");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Product Type");
            dt.Columns.Add("Frequency Type");
            dt.Columns.Add("Period");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Product Limit");
            dt.Columns.Add("Collateral Percentage");
            dt.Columns.Add("Low Risk Definition");
            dt.Columns.Add("Significant2");
            dt.Columns.Add("Significant3");
            dt.Columns.Add("Default Definition");
            dt.Columns.Add("Late Termination Charge");
            dt.Columns.Add("Early Termination Charge");
            dt.Columns.Add("Weighted Score");
            dt.Columns.Add("Interest Repayment");
            dt.Columns.Add("Schedule Method");
            dt.Columns.Add("Product Principal GL");
            dt.Columns.Add("Interest Income GL");
            dt.Columns.Add("Interest Receivable GL");
            dt.Columns.Add("Charge/Fee GL");

            var subglList = _serverRequest.GetAllSubGlAsync().Result;
            var products =  (from a in _dataContext.credit_product where a.Deleted == false
                                         select new ProductObj
                                               {
                                                   CleanUpCircle = a.CleanUpCircle,
                                                   LateTerminationCharge = a.LateTerminationCharge,
                                                   EarlyTerminationCharge = a.EarlyTerminationCharge,
                                                   PaymentType = a.PaymentType,
                                                   Period = a.Period,
                                                   ProductCode = a.ProductCode,
                                                   ProductName = a.ProductName,
                                                   WeightedMaxScore = a.WeightedMaxScore,
                                                   ProductLimit = a.ProductLimit,
                                                   InterestType = a.InterestType,
                                                   LowRiskDefinition = a.LowRiskDefinition,
                                                   Rate = a.Rate,
                                                   PaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.PaymentType).RepaymentTypeName,
                                                   ProductId = a.ProductId,
                                                   Defaultvalue = a.Default,
                                                   DefaultRange = a.DefaultRange,
                                                   Significant2 = a.Significant2,
                                                   Significant3 = a.Significant3,
                                                   ProductTypeId = a.ProductTypeId,
                                                   ProductTypeName = _dataContext.credit_producttype.FirstOrDefault(x => x.ProductTypeId == a.ProductTypeId).ProductTypeName,
                                                   PrincipalGL = a.PrincipalGL,
                                                   InterestIncomeExpenseGL = a.InterestIncomeExpenseGL,
                                                   InterestReceivablePayableGL = a.InterestReceivablePayableGL,
                                                   FrequencyTypeId = a.FrequencyTypeId,
                                                   FrequencyTypeName = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.FrequencyTypeId).Mode,
                                                   ScheduleTypeId = a.ScheduleTypeId,
                                                   ScheduleTypeName = _dataContext.credit_loanscheduletype.FirstOrDefault(x=>x.LoanScheduleTypeId == a.ScheduleTypeId).LoanScheduleTypeName,
                                                   CollateralPercentage = a.CollateralPercentage,
                                                   Tenor = a.TenorInDays,
                                                   FeeIncomeGL = a.FeeIncomeGL,
                                               }).ToList();
            foreach (var item in products)
            {
                item.ProductPrincipalGL = subglList.subGls.FirstOrDefault(x => x.SubGLId == item.PrincipalGL)?.SubGLCode;
                item.InterestReceivableGL = subglList.subGls.FirstOrDefault(x => x.SubGLId == item.InterestReceivablePayableGL)?.SubGLCode;
                item.InterestIncomeGL = subglList.subGls.FirstOrDefault(x => x.SubGLId == item.InterestIncomeExpenseGL)?.SubGLCode;
                item.ChargeFeeGL = subglList.subGls.FirstOrDefault(x => x.SubGLId == item.FeeIncomeGL)?.SubGLCode;
            }

            foreach (var kk in products)
            {
                var row = dt.NewRow();
                row["Product Code"] = kk.ProductCode;
                row["Product Name"] = kk.ProductName;
                row["Product Type"] = kk.ProductTypeName;
                row["Frequency Type"] = kk.FrequencyTypeName;
                row["Period"] = kk.Period;
                row["Rate"] = kk.Rate;
                row["Product Limit"] = kk.ProductLimit;
                row["Collateral Percentage"] = kk.CollateralPercentage;
                row["Low Risk Definition"] = kk.LowRiskDefinition;
                row["Significant2"] = kk.Significant2;
                row["Significant3"] = kk.Significant3;
                row["Default Definition"] = kk.Defaultvalue;
                row["Late Termination Charge"] = kk.LateTerminationCharge;
                row["Early Termination Charge"] = kk.EarlyTerminationCharge;
                row["Weighted Score"] = kk.WeightedMaxScore;
                row["Interest Repayment"] = kk.PaymentTypeName;
                row["Schedule Method"] = kk.ScheduleTypeName;
                row["Product Principal GL"] = kk.ProductPrincipalGL;
                row["Interest Income GL"] = kk.InterestIncomeGL;
                row["Interest Receivable GL"] = kk.InterestReceivableGL;
                row["Charge/Fee GL"] = kk.ChargeFeeGL;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (products != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Fees");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        public async Task<IEnumerable<ProductObj>> GetProductsAsync()
        {
            try
            {
                var product = await (from a in _dataContext.credit_product
                       where a.Deleted == false
                       select

                       new ProductObj
                       {
                           CleanUpCircle = a.CleanUpCircle,
                           LateTerminationCharge = a.LateTerminationCharge,
                           EarlyTerminationCharge = a.EarlyTerminationCharge,
                           PaymentType = a.PaymentType,
                           Period = a.Period,
                           ProductCode = a.ProductCode,
                           ProductName = a.ProductName,
                           WeightedMaxScore = a.WeightedMaxScore,
                           ProductLimit = a.ProductLimit,
                           InterestType = a.InterestType,
                           LowRiskDefinition = a.LowRiskDefinition,
                           Rate = a.Rate,
                           PaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.PaymentType).RepaymentTypeName,
                           ProductId = a.ProductId,
                           Defaultvalue = a.Default,
                           DefaultRange = a.DefaultRange,
                           Significant2 = a.Significant2,
                           Significant3 = a.Significant3,
                           ProductTypeId = a.ProductTypeId,
                           ProductTypeName = _dataContext.credit_producttype.FirstOrDefault(x => x.ProductTypeId == a.ProductTypeId).ProductTypeName,
                           PrincipalGL = a.PrincipalGL,
                           InterestIncomeExpenseGL = a.InterestIncomeExpenseGL,
                           InterestReceivablePayableGL = a.InterestReceivablePayableGL,
                           FrequencyTypeId = a.FrequencyTypeId,
                           FrequencyTypeName = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.FrequencyTypeId).Mode,
                           ScheduleTypeId = a.ScheduleTypeId,
                           CollateralPercentage = a.CollateralPercentage,
                           Tenor = a.TenorInDays,
                           FeeIncomeGL = a.FeeIncomeGL,
                           Productfee = _dataContext.credit_productfee.Where(x => x.ProductId == a.ProductId).Select(x => new ProductFeeObj()
                           {
                               ProductAmount = x.ProductAmount,
                               ProductFeeId = x.ProductFeeId,
                               ProductFeeName = x.credit_fee.FeeName,
                               ProductFeeType = x.ProductFeeType,
                               ProductId = x.ProductId,
                               ProductPaymentType = x.ProductPaymentType,
                               ProductPaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == x.ProductPaymentType).RepaymentTypeName,

                           }).ToList(),
                       }).ToListAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public IEnumerable<ProductObj> GetAllProduct()
        {
            var product = GetProductsAsync().Result;
            //var product = from a in GetProducts() select a;

            return product;
        }
        public ProductObj GetProduct(int productId)
        {
            var product = GetProductsAsync().Result.FirstOrDefault(x=>x.ProductId == productId);

            return product;
        }
        public ProductRegRespObj UploadProduct(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return new ProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                };

                List<ProductObj> uploadedRecord = new List<ProductObj>();
                var subglList =  _serverRequest.GetAllSubGlAsync().Result;
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
                                var item = new ProductObj
                                {
                                    ProductCode = workSheet.Cells[i, 1].Value != null ? (workSheet.Cells[i, 1].Value.ToString()) : string.Empty,
                                    ProductName = workSheet.Cells[i, 2].Value != null ? (workSheet.Cells[i, 2].Value.ToString()) : string.Empty,
                                    ProductTypeName = workSheet.Cells[i, 3].Value != null ? (workSheet.Cells[i, 3].Value.ToString()) : string.Empty,
                                    FrequencyTypeName = workSheet.Cells[i, 4].Value != null ? (workSheet.Cells[i, 4].Value.ToString()) : string.Empty,
                                    Period = workSheet.Cells[i, 5].Value != null ? int.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
                                    Rate = workSheet.Cells[i, 6].Value != null ? double.Parse(workSheet.Cells[i, 6].Value.ToString()) : 0,
                                    ProductLimit = workSheet.Cells[i, 7].Value != null ? decimal.Parse(workSheet.Cells[i, 7].Value.ToString()) : 0,
                                    CollateralPercentage = workSheet.Cells[i, 8].Value != null ? decimal.Parse(workSheet.Cells[i, 8].Value.ToString()) : 0,
                                    LowRiskDefinition = workSheet.Cells[i, 9].Value != null ? double.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
                                    Significant2 = workSheet.Cells[i, 10].Value != null ? decimal.Parse(workSheet.Cells[i, 10].Value.ToString()) : 0,
                                    Significant3 = workSheet.Cells[i, 11].Value != null ? decimal.Parse(workSheet.Cells[i, 11].Value.ToString()) : 0,
                                    Defaultvalue = workSheet.Cells[i, 12].Value != null ? int.Parse(workSheet.Cells[i, 12].Value.ToString()) : 0,
                                    LateTerminationCharge = workSheet.Cells[i, 13].Value != null ? double.Parse(workSheet.Cells[i, 13].Value.ToString()) : 0,
                                    EarlyTerminationCharge = workSheet.Cells[i, 14].Value != null ? double.Parse(workSheet.Cells[i, 14].Value.ToString()) : 0,
                                    WeightedMaxScore = workSheet.Cells[i, 15].Value != null ? decimal.Parse(workSheet.Cells[i, 15].Value.ToString()) : 0,
                                    PaymentTypeName = workSheet.Cells[i, 16].Value != null ? (workSheet.Cells[i, 16].Value.ToString()) : string.Empty,
                                    ScheduleTypeName = workSheet.Cells[i, 17].Value != null ? (workSheet.Cells[i, 17].Value.ToString()) : string.Empty,
                                    ProductPrincipalGL = workSheet.Cells[i, 18].Value != null ? (workSheet.Cells[i, 18].Value.ToString()) : string.Empty,
                                    InterestIncomeGL = workSheet.Cells[i, 19].Value != null ? (workSheet.Cells[i, 19].Value.ToString()) : string.Empty,
                                    InterestReceivableGL = workSheet.Cells[i, 20].Value != null ? (workSheet.Cells[i, 20].Value.ToString()) : string.Empty,
                                    ChargeFeeGL = workSheet.Cells[i, 21].Value != null ? (workSheet.Cells[i, 21].Value.ToString()) : string.Empty,
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        if (entity.ProductName == "" || entity.PaymentTypeName == "" || entity.ProductTypeName == "" || entity.FrequencyTypeName == "" || entity.ScheduleTypeName == "")
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include all fields" } }
                            };
                        }
                        var productId = 0;
                        entity.InterestType = 1;
                        var product = _dataContext.credit_product.Where(x => x.ProductName.ToLower().Trim() == entity.ProductName.ToLower().Trim()).FirstOrDefault();
                        if (product != null)
                        {
                            productId = product.ProductId;
                        }
                        var productType = _dataContext.credit_producttype.Where(x => x.ProductTypeName == entity.ProductTypeName).FirstOrDefault();
                        if (productType == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid product type name" } }
                            };
                        }
                        var frequencyType = _dataContext.credit_frequencytype.Where(x => x.Mode.ToLower().Trim() == entity.FrequencyTypeName.ToLower().Trim()).FirstOrDefault();
                        if (frequencyType == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid frequency type name" } }
                            };
                        }
                        var interestRepayment = _dataContext.credit_repaymenttype.Where(x => x.RepaymentTypeName.ToLower().Trim() == entity.PaymentTypeName.ToLower().Trim()).FirstOrDefault();
                        if (interestRepayment == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid interestRepayment type name" } }
                            };
                        }
                        var ScheduleType = _dataContext.credit_loanscheduletype.Where(x => x.LoanScheduleTypeName.ToLower().Trim() == entity.ScheduleTypeName.ToLower().Trim()).FirstOrDefault();
                        if (ScheduleType == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid ScheduleType name" } }
                            };
                        }

                        var productPrincipalGL = subglList.subGls.Where(x => x.SubGLCode.ToLower().Trim() == entity.ProductPrincipalGL.ToLower().Trim()).FirstOrDefault();
                        if (productPrincipalGL == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid Product Principal GL code" } }
                            };
                        }
                        var interestIncomeGL = subglList.subGls.Where(x => x.SubGLCode.ToLower().Trim() == entity.InterestIncomeGL.ToLower().Trim()).FirstOrDefault();
                        if (interestIncomeGL == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid Interest Income GL code" } }
                            };
                        }
                        var interestReceivableGL = subglList.subGls.Where(x => x.SubGLCode.ToLower().Trim() == entity.InterestReceivableGL.ToLower().Trim()).FirstOrDefault();
                        if (interestReceivableGL == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid Interest Receivable GL code" } }
                            };
                        }
                        var chargeFeeGL = subglList.subGls.Where(x => x.SubGLCode.ToLower().Trim() == entity.ChargeFeeGL.ToLower().Trim()).FirstOrDefault();
                        if (chargeFeeGL == null)
                        {
                            return new ProductRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Please include a valid Charge/Fee GL code" } }
                            };
                        }

                        var accountTypeExist = _dataContext.credit_product.FirstOrDefault(x=>x.ProductCode == product.ProductCode && x.Deleted == false);
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.ProductCode = entity.ProductCode;
                            accountTypeExist.ProductTypeId = productType.ProductTypeId;
                            accountTypeExist.ProductName = product.ProductName;
                            accountTypeExist.Rate = entity.Rate;
                            accountTypeExist.Period = entity.Period;
                            accountTypeExist.ProductLimit = entity.ProductLimit;              
                            accountTypeExist.LateTerminationCharge = entity.LateTerminationCharge;
                            accountTypeExist.EarlyTerminationCharge = entity.EarlyTerminationCharge;
                            accountTypeExist.LowRiskDefinition = entity.LowRiskDefinition;
                            accountTypeExist.Default = entity.Defaultvalue;
                            accountTypeExist.Significant2 = entity.Significant2;
                            accountTypeExist.Significant3 = entity.Significant3;
                            accountTypeExist.ProductTypeId = productType.ProductTypeId;
                            accountTypeExist.FrequencyTypeId = frequencyType.FrequencyTypeId;
                            accountTypeExist.CollateralPercentage = entity.CollateralPercentage;
                            accountTypeExist.WeightedMaxScore = entity.WeightedMaxScore;
                            accountTypeExist.InterestType = entity.InterestType;
                            accountTypeExist.ScheduleTypeId = ScheduleType.LoanScheduleTypeId;
                            accountTypeExist.PrincipalGL = productPrincipalGL.SubGLId;
                            accountTypeExist.InterestIncomeExpenseGL = interestIncomeGL.SubGLId;
                            accountTypeExist.InterestReceivablePayableGL = interestReceivableGL.SubGLId;
                            accountTypeExist.FeeIncomeGL = chargeFeeGL.SubGLId;
                            accountTypeExist.PaymentType = interestRepayment.RepaymentTypeId;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = createdBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new credit_product
                            {
                                ProductId = productId,
                                ProductCode = entity.ProductCode,
                                ProductName = entity.ProductName,
                                Rate = entity.Rate,
                                Period = entity.Period,
                                LateTerminationCharge = entity.LateTerminationCharge,
                                EarlyTerminationCharge = entity.EarlyTerminationCharge,
                                LowRiskDefinition = entity.LowRiskDefinition,
                                Default = entity.Defaultvalue,
                                Significant2 = entity.Significant2,
                                Significant3 = entity.Significant3,
                                ProductTypeId = productType.ProductTypeId,
                                FrequencyTypeId = frequencyType.FrequencyTypeId,
                                CollateralPercentage = entity.CollateralPercentage,
                                WeightedMaxScore = entity.WeightedMaxScore,
                                InterestType = interestRepayment.RepaymentTypeId,
                                ScheduleTypeId = ScheduleType.LoanScheduleTypeId,
                                PrincipalGL = productPrincipalGL.SubGLId,
                                InterestIncomeExpenseGL = interestIncomeGL.SubGLId,
                                InterestReceivablePayableGL = interestReceivableGL.SubGLId,
                                FeeIncomeGL = chargeFeeGL.SubGLId,
                                PaymentType = interestRepayment.RepaymentTypeId,
                                ProductLimit = entity.ProductLimit,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.credit_product.Add(accountType);
                        }
                    }
                }
                var isDone = _dataContext.SaveChanges() > 0;
                return new ProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //ProductType

        public bool AddUpdateProductType(ProductTypeObj entity)
        {
            try
            {

                if (entity == null) return false;

                if (entity.ProductTypeId > 0)
                {
                    var productTypeExist = _dataContext.credit_producttype.FirstOrDefault(x=>x.ProductName.ToLower().Trim() ==entity.ProductTypeName.ToLower().Trim());
                    if (productTypeExist != null)
                    {
                        productTypeExist.ProductTypeName = entity.ProductTypeName;
                        productTypeExist.Active = true;
                        productTypeExist.Deleted = false;
                        productTypeExist.CreatedBy = entity.CreatedBy;
                        productTypeExist.CreatedOn = DateTime.Now;
                        productTypeExist.UpdatedBy = entity.CreatedBy;
                        productTypeExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var productType = new credit_producttype
                    {
                        ProductTypeName = entity.ProductTypeName,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.credit_producttype.Add(productType);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Record not created");
            }
        }

        public async Task<bool> DeleteProductTypeAsync(int Id)
        {
            var itemToDelete = await _dataContext.credit_producttype.FindAsync(Id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public byte[] GenerateExportProductType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductTypeName");
            var structures = (from a in _dataContext.credit_producttype
                              where a.Deleted == false
                              select new credit_producttype
                              {
                                  ProductTypeName = a.ProductTypeName,
                              }).ToList();
            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["ProductTypeName"] = kk.ProductTypeName;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
        public async Task<IEnumerable<credit_producttype>> GetAllProductTypeAsync()
        {
            return await _dataContext.credit_producttype.Where(x => x.Deleted == false).ToListAsync();
        }
        public async Task<credit_producttype> GetProductTypeByIdAsync(int id)
        {
            return await _dataContext.credit_producttype.FirstOrDefaultAsync(x => x.Deleted == false && x.ProductTypeId == id);
        }
        public async Task<bool> UploadProductTypeAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<ProductTypeObj> uploadedRecord = new List<ProductTypeObj>();
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
                                var item = new ProductTypeObj
                                {
                                    ProductTypeName = workSheet.Cells[i, 1].Value.ToString()
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
                        var category = _dataContext.credit_producttype.Where(x => x.ProductTypeName == item.ProductTypeName && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.ProductTypeName = item.ProductTypeName;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new credit_producttype
                            {
                                ProductTypeName = item.ProductTypeName,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            //structures.Add(structure);
                            _dataContext.credit_producttype.Add(structure);
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



        //ProductFee
        public byte[] GenerateExportProductFee(int productId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Fee Name");
            dt.Columns.Add("Payment Type");
            dt.Columns.Add("Amount/Percentage");
            dt.Columns.Add("Fee Type");
            var productFee = GetProductFeeByProduct(productId);
            foreach (var kk in productFee)
            {
                var row = dt.NewRow();
                row["Fee Name"] = kk.ProductFeeName;
                row["Payment Type"] = kk.ProductPaymentTypeName;
                row["Amount/Percentage"] = kk.ProductAmount;
                if (kk.ProductFeeType == 1)
                {
                    row["Fee Type"] = "Fixed";
                }
                else
                {
                    row["Fee Type"] = "Percentage";
                }

                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (productFee != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProductFee");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        public async Task<bool> DeleteProductFeeAsync(int productFeeId)
        {
            var itemToDelete = await _dataContext.credit_productfee.FindAsync(productFeeId);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public IEnumerable<ProductFeeObj> GetAllProductFee()
        {
            try
            {
                var productFee = (from a in _dataContext.credit_productfee
                                  where a.Deleted == false
                                  select new ProductFeeObj
                                  {
                                      ProductFeeName = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == a.FeeId && x.Deleted == false).FeeName,
                                      FeeId = a.FeeId,
                                      ProductFeeId = a.ProductFeeId,
                                      ProductAmount = a.ProductAmount,
                                      ProductPaymentType = a.ProductPaymentType,
                                      ProductFeeType = a.ProductFeeType,
                                      ProductPaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.ProductPaymentType && c.Deleted == false).RepaymentTypeName,
                                      ProductId = a.ProductId,
                                  }).ToList();

                return productFee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ProductFeeObj GetProductFee(int productFeeId)
        {
            var productFee = (from a in _dataContext.credit_productfee
                              where a.Deleted == false && a.ProductFeeId == productFeeId
                              select new ProductFeeObj
                              {
                                  ProductFeeName = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == a.FeeId && x.Deleted == false).FeeName,
                                  FeeId = a.FeeId,
                                  ProductFeeId = a.ProductFeeId,
                                  ProductAmount = a.ProductAmount,
                                  ProductPaymentType = a.ProductPaymentType,
                                  ProductFeeType = a.ProductFeeType,
                                  ProductPaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.ProductPaymentType && c.Deleted == false).RepaymentTypeName,
                                  ProductId = a.ProductId,
                              }).FirstOrDefault();

            return productFee;
        }
        public List<ProductFeeObj> GetProductFeeByLoanApplicationId(int Id)
        {
            try
            {
                var repaymentTypelist = _dataContext.credit_repaymenttype.Where(c => c.Deleted == false).ToList();
                var feelist = _dataContext.credit_fee.Where(c => c.Deleted == false).ToList();
                var productFee = (from a in _dataContext.credit_productfee
                                  join b in _dataContext.credit_loanapplication on a.ProductId equals b.ApprovedProductId
                                  where a.Deleted == false && b.LoanApplicationId == Id && b.Deleted == false
                                  select new ProductFeeObj
                                  {
                                      ProductFeeName = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == a.FeeId && x.Deleted == false).FeeName,
                                      ProductFeeId = a.ProductFeeId,
                                      FeeId = a.FeeId,
                                      Status = _dataContext.credit_productfeestatus.Where(x => x.ProductFeeId == a.ProductFeeId && x.LoanApplicationId == b.LoanApplicationId).FirstOrDefault() == null ? false : _dataContext.credit_productfeestatus.Where(x => x.ProductFeeId == a.ProductFeeId && x.LoanApplicationId == b.LoanApplicationId).FirstOrDefault().Status,
                                      IsIntegral = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == a.FeeId).IsIntegral,
                                      ProductAmount = a.ProductAmount,
                                      ProductPaymentType = a.ProductPaymentType,
                                      ProductFeeType = a.ProductFeeType,
                                      ProductPaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.ProductPaymentType && c.Deleted == false).RepaymentTypeName,
                                      ProductId = a.ProductId,
                                  }).ToList();

                productFee.ForEach(x =>
                {
                    x.FeeTypeName = fees.FirstOrDefault(y => y.Key == x.ProductFeeType).Value;
                });

                return productFee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<ProductFeeObj> GetProductFeeByProduct(int productId)
        {
            try
            {
                var productFee = (from a in _dataContext.credit_productfee
                                  where a.Deleted == false && a.ProductId == productId
                                  select new ProductFeeObj
                                  {
                                      ProductFeeName = _dataContext.credit_fee.FirstOrDefault(x => x.FeeId == a.FeeId && x.Deleted == false).FeeName,
                                      ProductFeeId = a.ProductFeeId,
                                      FeeId = a.FeeId,
                                      IsIntegral = _dataContext.credit_fee.Where(x => x.FeeId == a.FeeId).FirstOrDefault().IsIntegral,
                                      ProductAmount = a.ProductAmount,
                                      ProductPaymentType = a.ProductPaymentType,
                                      ProductFeeType = a.ProductFeeType,
                                      ProductPaymentTypeName = _dataContext.credit_repaymenttype.FirstOrDefault(c => c.RepaymentTypeId == a.ProductPaymentType).RepaymentTypeName,
                                      ProductId = a.ProductId,
                                  }).ToList();

                productFee.ForEach(x =>
                {
                    x.FeeTypeName = fees.FirstOrDefault(y => y.Key == x.ProductFeeType).Value;
                });

                return productFee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateProductFee(credit_productfee model)
        {
            try
            {
                if (model.ProductFeeId > 0)
                {
                    var itemToUpdate = await _dataContext.credit_productfee.FindAsync(model.ProductFeeId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                     await _dataContext.credit_productfee.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProductFeeRegRespObj> UploadProductFee(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return new ProductFeeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                };
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<ProductFeeObj> uploadedRecord = new List<ProductFeeObj>();
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
                                var item = new ProductFeeObj
                                {
                                    ProductName = workSheet.Cells[i, 1].Value.ToString(),
                                    ProductFeeName = workSheet.Cells[i, 2].Value.ToString(),
                                    ProductAmount = double.Parse(workSheet.Cells[i, 3].Value.ToString()),
                                    ProductFeeTypeName = workSheet.Cells[i, 4].Value.ToString(),
                                    ProductPaymentTypeName = workSheet.Cells[i, 5].Value.ToString()
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
                        var product = _dataContext.credit_product.Where(x => x.ProductCode == item.ProductName && x.Deleted == false).FirstOrDefault();
                        if (product == null)
                            return new ProductFeeRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Upload a correct product name" } }
                            };
                        item.ProductId = product.ProductId;

                        var PaymentType = _dataContext.credit_repaymenttype.Where(x => x.RepaymentTypeName == item.ProductFeeTypeName && x.Deleted == false).FirstOrDefault();
                        if (PaymentType == null)
                            return new ProductFeeRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Upload a correct Repayment Type Name" } }
                            };
                        item.ProductPaymentType = PaymentType.RepaymentTypeId;

                        item.FeeId = _dataContext.credit_fee.Where(x => x.FeeName == item.ProductFeeName && x.Deleted == false).FirstOrDefault().FeeId;
                        if (item.ProductPaymentTypeName.ToLower() == "percentage")
                        {
                            item.ProductFeeType = 2;
                        }
                        else if (item.ProductPaymentTypeName.ToLower() == "fixed")
                        {
                            item.ProductFeeType = 1;
                        }
                        var category = _dataContext.credit_productfee.Where(x => x.ProductAmount == item.ProductAmount && x.ProductId == item.ProductId && x.FeeId == item.FeeId && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.ProductFeeType = item.ProductFeeType;
                            category.FeeId = item.FeeId;
                            category.ProductPaymentType = item.ProductPaymentType;
                            category.ProductAmount = item.ProductAmount;
                            category.ProductId = item.ProductId;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new credit_productfee
                            {
                                ProductAmount = item.ProductAmount,
                                FeeId = item.FeeId,
                                ProductFeeType = item.ProductFeeType,
                                ProductPaymentType = item.ProductPaymentType,
                                ProductId = item.ProductId,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.credit_productfee.Add(structure);
                        }
                    }
                }
                var isDone = _dataContext.SaveChanges() > 0;
                return new ProductFeeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int GetTenorByPeriodAndFrequency(int period, int frequencyId)
        {
            int daysInYear = (int)365.2425;
            int tenor = 0;
            if (frequencyId == (int)FrequencyTypeEnum.Yearly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * daysInYear)));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.TwiceYearly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 2))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.Quarterly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 4))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.SixTimesYearly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 6))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.Monthly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 12))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.TwiceMonthly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 24))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.Weekly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 52))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.Daily)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 365))));
                return tenor;
            }
            else if (frequencyId == (int)FrequencyTypeEnum.ThriceYearly)
            {
                tenor = Convert.ToInt32(Decimal.Round((period * (daysInYear / 3))));
                return tenor;
            }
            return tenor;
        }


    }
}
