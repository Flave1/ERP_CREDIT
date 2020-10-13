using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Helpers;
using Banking.Repository.Interface.Credit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Implement.Credit
{
    public class IFRSRepository : IIFRSRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public IFRSRepository(DataContext context, IConfiguration configuration)
        {
            _dataContext = context;
            _configuration = configuration;
        }
        public bool AddUpdateIFRSSetupData(IFRSSetupDataObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.SetUpId > 0)
                {
                    var setupExist = _dataContext.ifrs_setup_data.Find(entity.SetUpId);
                    if (setupExist != null)
                    {
                        setupExist.CCF = entity.Ccf;
                        setupExist.Classification_Type = entity.Classification_Type;
                        setupExist.Deteroriation_Level = entity.Deteroriation_Level;
                        setupExist.GroupBased = entity.GroupBased;
                        setupExist.Historical_PD_Year_Count = entity.Historical_PD_Year_Count;
                        setupExist.Ltpdapproach = entity.Ltpdapproach;
                        setupExist.PDBasis = entity.PdBasis;
                        setupExist.RunDate = entity.RunDate;
                        setupExist.Threshold = entity.Threshold;
                        setupExist.Active = true;
                        setupExist.Deleted = false;
                        setupExist.CreatedBy = entity.CreatedBy;
                        setupExist.CreatedOn = DateTime.Now;
                        setupExist.UpdatedBy = entity.CreatedBy;
                        setupExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var approvalLevelStaff = new ifrs_setup_data
                    {
                        CCF = entity.Ccf,
                        Classification_Type = entity.Classification_Type,
                        Deteroriation_Level = entity.Deteroriation_Level,
                        GroupBased = entity.GroupBased,
                        Historical_PD_Year_Count = entity.Historical_PD_Year_Count,
                        Ltpdapproach = entity.Ltpdapproach,
                        PDBasis = entity.PdBasis,
                        RunDate = entity.RunDate,
                        Threshold = entity.Threshold,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.ifrs_setup_data.Add(approvalLevelStaff);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateMacroEconomicVariable(MacroEconomicVariableObj entity)
        {
            try
            {
                if (entity == null) return false;

                ifrs_macroeconomic_variables_year variable = null;

                    variable = _dataContext.ifrs_macroeconomic_variables_year.FirstOrDefault(x=>x.Year == entity.Year);
                    if (variable != null)
                    {
                        variable.Unemployement = entity.Unemployement;
                        variable.erosion = entity.Erosion;
                        variable.ForegnEx = entity.ForegnEx;
                        variable.GDP = entity.Gdp;
                        variable.Inflation = entity.Inflation;
                        variable.otherfactor = entity.Otherfactor;
                        variable.Others = entity.Others;
                        variable.Active = true;
                        variable.Deleted = false;
                        variable.CreatedBy = entity.CreatedBy;
                        variable.CreatedOn = DateTime.Now;
                        variable.UpdatedBy = entity.CreatedBy;
                        variable.UpdatedOn = DateTime.Now;
                }
                else
                {
                    variable = new ifrs_macroeconomic_variables_year
                    {
                        Year = entity.Year,
                        Unemployement = entity.Unemployement,
                        erosion = entity.Erosion,
                        ForegnEx = entity.ForegnEx,
                        GDP = entity.Gdp,
                        Inflation = entity.Inflation,
                        otherfactor = entity.Otherfactor,
                        Others = entity.Others,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _dataContext.ifrs_macroeconomic_variables_year.Add(variable);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteIfrsScenarioSetup(int id)
        {
            try
            {
                var accountType = _dataContext.ifrs_scenario_setup.Find(id);
                if (accountType != null)
                {
                    accountType.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteIFRSSetupData(int setupId)
        {
            try
            {
                var setup = _dataContext.ifrs_setup_data.Find(setupId);
                if (setup != null)
                {
                    setup.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLGDHistory(int historicalLGDId)
        {
            try
            {
                var variable = _dataContext.credit_lgd_historyinformation.Find(historicalLGDId);
                if (variable != null)
                {
                    var details = _dataContext.credit_lgd_historyinformationdetails.Where(x => x.LoanReferenceNumber == variable.LoanReferenceNumber).ToList();
                    if (details.Count > 0)
                    {
                        _dataContext.credit_lgd_historyinformationdetails.RemoveRange(details);
                    }
                    _dataContext.credit_lgd_historyinformation.Remove(variable);
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteMacroEconomicVariable(int macroEconomicVariableId)
        {
            try
            {
                var variable = _dataContext.ifrs_macroeconomic_variables_year.Find(macroEconomicVariableId);
                if (variable != null)
                {
                    variable.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteScoreCardHistory(int applicationScoreCardId)
        {
            try
            {
                var variable = _dataContext.credit_individualapplicationscorecard_history.Find(applicationScoreCardId);
                if (variable != null)
                {
                    var details = _dataContext.credit_individualapplicationscorecarddetails_history.Where(x => x.LoanReferenceNumber == variable.LoanReferenceNumber).ToList();
                    if (details.Count > 0)
                    {
                        _dataContext.credit_individualapplicationscorecarddetails_history.RemoveRange(details);
                    }
                    _dataContext.credit_individualapplicationscorecard_history.Remove(variable);
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] GenerateExportLGDHistory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LoanAmount");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("LoanReferenceNumber");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("EffectiveDate");
            dt.Columns.Add("MaturityDate");
            dt.Columns.Add("CR");
            dt.Columns.Add("EIR");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Field1");
            dt.Columns.Add("Field2");
            dt.Columns.Add("Field3");
            dt.Columns.Add("Field4");
            dt.Columns.Add("Field5");
            dt.Columns.Add("Field6");
            dt.Columns.Add("Field7");
            dt.Columns.Add("Field8");
            dt.Columns.Add("Field9");
            dt.Columns.Add("Field10");
            dt.Columns.Add("Field11");
            dt.Columns.Add("Field12");
            dt.Columns.Add("Field13");
            dt.Columns.Add("Field14");
            dt.Columns.Add("Field15");
            dt.Columns.Add("Field16");
            dt.Columns.Add("Field17");
            dt.Columns.Add("Field18");
            dt.Columns.Add("Field19");
            dt.Columns.Add("Field20");
            dt.Columns.Add("Field21");
            dt.Columns.Add("Field22");
            dt.Columns.Add("Field23");
            dt.Columns.Add("Field24");
            dt.Columns.Add("Field25");
            dt.Columns.Add("Field26");
            dt.Columns.Add("Field27");
            dt.Columns.Add("Field28");
            dt.Columns.Add("Field29");
            dt.Columns.Add("Field30");
            dt.Columns.Add("Field31");
            dt.Columns.Add("Field32");
            dt.Columns.Add("Field33");
            dt.Columns.Add("Field34");
            dt.Columns.Add("Field35");
            dt.Columns.Add("Field36");
            dt.Columns.Add("Field37");
            dt.Columns.Add("Field38");
            dt.Columns.Add("Field39");
            dt.Columns.Add("Field40");
            dt.Columns.Add("Field41");
            dt.Columns.Add("Field42");
            dt.Columns.Add("Field43");
            dt.Columns.Add("Field44");
            dt.Columns.Add("Field45");
            dt.Columns.Add("Field46");
            dt.Columns.Add("Field47");
            dt.Columns.Add("Field48");
            dt.Columns.Add("Field49");
            dt.Columns.Add("Field50");
            dt.Columns.Add("Field51");
            dt.Columns.Add("Field52");
            dt.Columns.Add("Field53");
            dt.Columns.Add("Field54");
            dt.Columns.Add("Field55");
            dt.Columns.Add("Field56");
            dt.Columns.Add("Field57");
            dt.Columns.Add("Field58");
            dt.Columns.Add("Field59");
            dt.Columns.Add("Field60");
            var variable = (from a in _dataContext.credit_lgd_historyinformation
                            where a.Deleted == false
                            select new LGDHistoryObj
                            {
                                HistoricalLGDId = a.HistoricalLGDId,
                                LoanAmount = a.LoanAmount,
                                CustomerName = a.CustomerName,
                                LoanReferenceNumber = a.LoanReferenceNumber,
                                ProductCode = a.ProductCode,
                                ProductName = a.ProductName,
                                EffectiveDate = a.EffectiveDate,
                                MaturityDate = a.MaturityDate,
                                Cr = a.CR,
                                Eir = a.EIR,
                                Frequency = a.Frequency,
                                Field1 = a.Field1,
                                Field2 = a.Field2,
                                Field3 = a.Field3,
                                Field4 = a.Field4,
                                Field5 = a.Field5,
                                Field6 = a.Field6,
                                Field7 = a.Field7,
                                Field8 = a.Field8,
                                Field9 = a.Field9,
                                Field10 = a.Field10,
                                Field11 = a.Field11,
                                Field12 = a.Field12,
                                Field13 = a.Field13,
                                Field14 = a.Field14,
                                Field15 = a.Field15,
                                Field16 = a.Field16,
                                Field17 = a.Field17,
                                Field18 = a.Field18,
                                Field19 = a.Field19,
                                Field20 = a.Field20,
                                Field21 = a.Field21,
                                Field22 = a.Field22,
                                Field23 = a.Field23,
                                Field24 = a.Field24,
                                Field25 = a.Field25,
                                Field26 = a.Field26,
                                Field27 = a.Field27,
                                Field28 = a.Field28,
                                Field29 = a.Field29,
                                Field30 = a.Field30,
                                Field31 = a.Field31,
                                Field32 = a.Field32,
                                Field33 = a.Field33,
                                Field34 = a.Field34,
                                Field35 = a.Field35,
                                Field36 = a.Field36,
                                Field37 = a.Field37,
                                Field38 = a.Field38,
                                Field39 = a.Field39,
                                Field40 = a.Field40,
                                Field41 = a.Field41,
                                Field42 = a.Field42,
                                Field43 = a.Field43,
                                Field44 = a.Field44,
                                Field45 = a.Field45,
                                Field46 = a.Field46,
                                Field47 = a.Field47,
                                Field48 = a.Field48,
                                Field49 = a.Field49,
                                Field50 = a.Field50,
                                Field51 = a.Field51,
                                Field52 = a.Field52,
                                Field53 = a.Field53,
                                Field54 = a.Field54,
                                Field55 = a.Field55,
                                Field56 = a.Field56,
                                Field57 = a.Field57,
                                Field58 = a.Field58,
                                Field59 = a.Field59,
                                Field60 = a.Field60
                            }).ToList();

            foreach (var kk in variable)
            {
                var row = dt.NewRow();

                row["LoanAmount"] = kk.LoanAmount;
                row["CustomerName"] = kk.CustomerName;
                row["LoanReferenceNumber"] = kk.LoanReferenceNumber;
                row["ProductCode"] = kk.ProductCode;
                row["ProductName"] = kk.ProductName;
                row["EffectiveDate"] = kk.EffectiveDate;
                row["MaturityDate"] = kk.MaturityDate;
                row["CR"] = kk.Cr;
                row["EIR"] = kk.Eir;
                row["Frequency"] = kk.Frequency;
                row["Field1"] = kk.Field1;
                row["Field2"] = kk.Field2;
                row["Field3"] = kk.Field3;
                row["Field4"] = kk.Field4;
                row["Field5"] = kk.Field5;
                row["Field6"] = kk.Field6;
                row["Field7"] = kk.Field7;
                row["Field8"] = kk.Field8;
                row["Field9"] = kk.Field9;
                row["Field10"] = kk.Field10;
                row["Field11"] = kk.Field11;
                row["Field12"] = kk.Field12;
                row["Field13"] = kk.Field13;
                row["Field14"] = kk.Field14;
                row["Field15"] = kk.Field15;
                row["Field16"] = kk.Field16;
                row["Field17"] = kk.Field17;
                row["Field18"] = kk.Field18;
                row["Field19"] = kk.Field19;
                row["Field20"] = kk.Field20;
                row["Field21"] = kk.Field21;
                row["Field22"] = kk.Field22;
                row["Field23"] = kk.Field23;
                row["Field24"] = kk.Field24;
                row["Field25"] = kk.Field25;
                row["Field26"] = kk.Field26;
                row["Field27"] = kk.Field27;
                row["Field28"] = kk.Field28;
                row["Field29"] = kk.Field29;
                row["Field30"] = kk.Field30;
                row["Field31"] = kk.Field31;
                row["Field32"] = kk.Field32;
                row["Field33"] = kk.Field33;
                row["Field34"] = kk.Field34;
                row["Field35"] = kk.Field35;
                row["Field36"] = kk.Field36;
                row["Field37"] = kk.Field37;
                row["Field38"] = kk.Field38;
                row["Field39"] = kk.Field39;
                row["Field40"] = kk.Field40;
                row["Field41"] = kk.Field41;
                row["Field42"] = kk.Field42;
                row["Field43"] = kk.Field43;
                row["Field44"] = kk.Field44;
                row["Field45"] = kk.Field45;
                row["Field46"] = kk.Field46;
                row["Field47"] = kk.Field47;
                row["Field48"] = kk.Field48;
                row["Field49"] = kk.Field49;
                row["Field50"] = kk.Field50;
                row["Field51"] = kk.Field51;
                row["Field52"] = kk.Field52;
                row["Field53"] = kk.Field53;
                row["Field54"] = kk.Field54;
                row["Field55"] = kk.Field55;
                row["Field56"] = kk.Field56;
                row["Field57"] = kk.Field57;
                row["Field58"] = kk.Field58;
                row["Field59"] = kk.Field59;
                row["Field60"] = kk.Field60;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (variable != null)
            {

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("LGDHistory");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportMacroEconomicViriable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Year");
            dt.Columns.Add("Unemployement");
            dt.Columns.Add("GDP");
            dt.Columns.Add("Inflation");
            dt.Columns.Add("Others");

            var variable = (from a in _dataContext.ifrs_macroeconomic_variables_year
                            where a.Deleted == false
                            select new MacroEconomicVariableObj
                            {
                                MacroEconomicVariableId = a.MacroEconomicVariableId,
                                Year = a.Year,
                                Unemployement = a.Unemployement,
                                Erosion = a.erosion,
                                ForegnEx = a.ForegnEx,
                                Gdp = a.GDP,
                                Inflation = a.Inflation,
                                Otherfactor = a.otherfactor,
                                Others = a.Others,
                            }).ToList();

            foreach (var kk in variable)
            {
                var row = dt.NewRow();
                row["Year"] = kk.Year;
                row["Unemployement"] = kk.Unemployement;
                row["GDP"] = kk.Gdp;
                row["Inflation"] = kk.Inflation;
                row["Others"] = kk.Others;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (variable != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("MacroEconomicViriable");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportScoreCardHistory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LoanAmount");
            dt.Columns.Add("CustomerType");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("LoanReferenceNumber");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("EffectiveDate");
            dt.Columns.Add("MaturityDate");
            dt.Columns.Add("Field1");
            dt.Columns.Add("Field2");
            dt.Columns.Add("Field3");
            dt.Columns.Add("Field4");
            dt.Columns.Add("Field5");
            dt.Columns.Add("Field6");
            dt.Columns.Add("Field7");
            dt.Columns.Add("Field8");
            dt.Columns.Add("Field9");
            dt.Columns.Add("Field10");
            dt.Columns.Add("Field11");
            dt.Columns.Add("Field12");
            dt.Columns.Add("Field13");
            dt.Columns.Add("Field14");
            dt.Columns.Add("Field15");
            dt.Columns.Add("Field16");
            dt.Columns.Add("Field17");
            dt.Columns.Add("Field18");
            dt.Columns.Add("Field19");
            dt.Columns.Add("Field20");
            dt.Columns.Add("Field21");
            dt.Columns.Add("Field22");
            dt.Columns.Add("Field23");
            dt.Columns.Add("Field24");
            dt.Columns.Add("Field25");
            dt.Columns.Add("Field26");
            dt.Columns.Add("Field27");
            dt.Columns.Add("Field28");
            dt.Columns.Add("Field29");
            dt.Columns.Add("Field30");

            var variable = (from a in _dataContext.credit_individualapplicationscorecard_history
                            where a.Deleted == false
                            select new ScoreCardHistoryObj
                            {
                                ApplicationScoreCardId = a.ApplicationScoreCardId,
                                CustomerTypeId = a.CustomerTypeId,
                                LoanAmount = a.LoanAmount,
                                CustomerName = a.CustomerName,
                                LoanReferenceNumber = a.LoanReferenceNumber,
                                ProductCode = a.ProductCode,
                                ProductName = a.ProductName,
                                EffectiveDate = a.EffectiveDate,
                                MaturityDate = a.MaturityDate,
                                Field1 = a.Field1,
                                Field2 = a.Field2,
                                Field3 = a.Field3,
                                Field4 = a.Field4,
                                Field5 = a.Field5,
                                Field6 = a.Field6,
                                Field7 = a.Field7,
                                Field8 = a.Field8,
                                Field9 = a.Field9,
                                Field10 = a.Field10,
                                Field11 = a.Field11,
                                Field12 = a.Field12,
                                Field13 = a.Field13,
                                Field14 = a.Field14,
                                Field15 = a.Field15,
                                Field16 = a.Field16,
                                Field17 = a.Field17,
                                Field18 = a.Field18,
                                Field19 = a.Field19,
                                Field20 = a.Field20,
                                Field21 = a.Field21,
                                Field22 = a.Field22,
                                Field23 = a.Field23,
                                Field24 = a.Field24,
                                Field25 = a.Field25,
                                Field26 = a.Field26,
                                Field27 = a.Field27,
                                Field28 = a.Field28,
                                Field29 = a.Field29,
                                Field30 = a.Field30
                            }).ToList();

            foreach (var kk in variable)
            {
                var row = dt.NewRow();
                var customerTypeName = kk.CustomerTypeId == 1 ? "Individual" : "Corporate";
                row["LoanAmount"] = kk.LoanAmount;
                row["CustomerName"] = kk.CustomerName;
                row["CustomerType"] = customerTypeName;
                row["LoanReferenceNumber"] = kk.LoanReferenceNumber;
                row["ProductCode"] = kk.ProductCode;
                row["ProductName"] = kk.ProductName;
                row["EffectiveDate"] = kk.EffectiveDate;
                row["MaturityDate"] = kk.MaturityDate;
                row["Field1"] = kk.Field1;
                row["Field2"] = kk.Field2;
                row["Field3"] = kk.Field3;
                row["Field4"] = kk.Field4;
                row["Field5"] = kk.Field5;
                row["Field6"] = kk.Field6;
                row["Field7"] = kk.Field7;
                row["Field8"] = kk.Field8;
                row["Field9"] = kk.Field9;
                row["Field10"] = kk.Field10;
                row["Field11"] = kk.Field11;
                row["Field12"] = kk.Field12;
                row["Field13"] = kk.Field13;
                row["Field14"] = kk.Field14;
                row["Field15"] = kk.Field15;
                row["Field16"] = kk.Field16;
                row["Field17"] = kk.Field17;
                row["Field18"] = kk.Field18;
                row["Field19"] = kk.Field19;
                row["Field20"] = kk.Field20;
                row["Field21"] = kk.Field21;
                row["Field22"] = kk.Field22;
                row["Field23"] = kk.Field23;
                row["Field24"] = kk.Field24;
                row["Field25"] = kk.Field25;
                row["Field26"] = kk.Field26;
                row["Field27"] = kk.Field27;
                row["Field28"] = kk.Field28;
                row["Field29"] = kk.Field29;
                row["Field30"] = kk.Field30;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (variable != null)
            {

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("HistoricalPD");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public byte[] GenerateExportScoreCardHistoryByProduct(int productId, int customerTypeId)
        {
            var creditRiskCategory = (from a in _dataContext.credit_creditriskattribute
                                      join b in _dataContext.credit_creditriskscorecard
                                      on a.CreditRiskAttributeId equals b.CreditRiskAttributeId
                                      into app
                                      from b in app.DefaultIfEmpty()
                                      join c in _dataContext.credit_weightedriskscore on b.CreditRiskAttributeId
                                      equals c.CreditRiskAttributeId
                                      where a.Deleted == false && b.Deleted == false && c.CustomerTypeId == customerTypeId && c.ProductId == productId
                                      orderby a.CreditRiskAttributeId
                                      select new
                                      {
                                          attribute = a.CreditRiskAttribute

                                      }).ToList();

            var result = creditRiskCategory.GroupBy(x => x.attribute).Select(p => p.FirstOrDefault());

            DataTable dt = new DataTable();
            dt.Columns.Add("LoanAmount");
            dt.Columns.Add("CustomerType");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("LoanReferenceNumber");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("EffectiveDate");
            dt.Columns.Add("MaturityDate");
            foreach (var row in result)
            {
                dt.Columns.Add(row.attribute);
            }


            var variable = (from a in _dataContext.credit_individualapplicationscorecard_history
                            where a.Deleted == false
                            select new ScoreCardHistoryObj
                            {
                                ApplicationScoreCardId = a.ApplicationScoreCardId,
                                LoanAmount = a.LoanAmount,
                                CustomerName = a.CustomerName,
                                CustomerTypeId = a.CustomerTypeId,
                                LoanReferenceNumber = a.LoanReferenceNumber,
                                ProductCode = a.ProductCode,
                                ProductName = a.ProductName,
                                EffectiveDate = a.EffectiveDate,
                                MaturityDate = a.MaturityDate,
                                Field1 = a.Field1,
                                Field2 = a.Field2,
                                Field3 = a.Field3,
                                Field4 = a.Field4,
                                Field5 = a.Field5,
                                Field6 = a.Field6,
                                Field7 = a.Field7,
                                Field8 = a.Field8,
                                Field9 = a.Field9,
                                Field10 = a.Field10,
                                Field11 = a.Field11,
                                Field12 = a.Field12,
                                Field13 = a.Field13,
                                Field14 = a.Field14,
                                Field15 = a.Field15,
                                Field16 = a.Field16,
                                Field17 = a.Field17,
                                Field18 = a.Field18,
                                Field19 = a.Field19,
                                Field20 = a.Field20,
                                Field21 = a.Field21,
                                Field22 = a.Field22,
                                Field23 = a.Field23,
                                Field24 = a.Field24,
                                Field25 = a.Field25,
                                Field26 = a.Field26,
                                Field27 = a.Field27,
                                Field28 = a.Field28,
                                Field29 = a.Field29,
                                Field30 = a.Field30
                            }).ToList();

            foreach (var kk in variable)
            {
                var row = dt.NewRow();
                int rowCount = 0;
                row["LoanAmount"] = kk.LoanAmount;
                row["CustomerType"] = kk.CustomerTypeId;
                row["CustomerName"] = kk.CustomerName;
                row["LoanReferenceNumber"] = kk.LoanReferenceNumber;
                row["ProductCode"] = kk.ProductCode;
                row["ProductName"] = kk.ProductName;
                row["EffectiveDate"] = kk.EffectiveDate;
                row["MaturityDate"] = kk.MaturityDate;
                foreach (var value in creditRiskCategory)
                {
                    rowCount = rowCount + 1;

                    if (rowCount == 1)
                        row[value.attribute] = kk.Field1;
                    else if (rowCount == 2)
                        row[value.attribute] = kk.Field2;
                    else if (rowCount == 3)
                        row[value.attribute] = kk.Field3;
                    else if (rowCount == 4)
                        row[value.attribute] = kk.Field4;
                    else if (rowCount == 5)
                        row[value.attribute] = kk.Field5;
                    else if (rowCount == 6)
                        row[value.attribute] = kk.Field6;
                    else if (rowCount == 7)
                        row[value.attribute] = kk.Field7;
                    else if (rowCount == 8)
                        row[value.attribute] = kk.Field8;
                    else if (rowCount == 9)
                        row[value.attribute] = kk.Field9;
                    else if (rowCount == 10)
                        row[value.attribute] = kk.Field10;
                    else if (rowCount == 11)
                        row[value.attribute] = kk.Field11;
                    else if (rowCount == 12)
                        row[value.attribute] = kk.Field12;
                    else if (rowCount == 13)
                        row[value.attribute] = kk.Field13;
                    else if (rowCount == 14)
                        row[value.attribute] = kk.Field14;
                    else if (rowCount == 15)
                        row[value.attribute] = kk.Field15;
                    else if (rowCount == 16)
                        row[value.attribute] = kk.Field16;
                    else if (rowCount == 17)
                        row[value.attribute] = kk.Field17;
                    else if (rowCount == 18)
                        row[value.attribute] = kk.Field18;
                    else if (rowCount == 19)
                        row[value.attribute] = kk.Field19;
                    else if (rowCount == 20)
                        row[value.attribute] = kk.Field20;
                    else if (rowCount == 21)
                        row[value.attribute] = kk.Field21;
                    else if (rowCount == 22)
                        row[value.attribute] = kk.Field22;
                    else if (rowCount == 23)
                        row[value.attribute] = kk.Field23;
                    else if (rowCount == 24)
                        row[value.attribute] = kk.Field24;
                    else if (rowCount == 25)
                        row[value.attribute] = kk.Field25;
                    else if (rowCount == 26)
                        row[value.attribute] = kk.Field26;
                    else if (rowCount == 27)
                        row[value.attribute] = kk.Field27;
                    else if (rowCount == 28)
                        row[value.attribute] = kk.Field28;
                    else if (rowCount == 29)
                        row[value.attribute] = kk.Field29;
                    else if (rowCount == 30)
                        row[value.attribute] = kk.Field30;


                }

                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (variable != null)
            {

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("HistoricalPD");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public IEnumerable<IfrsScenarioSetupObj> GetAllIfrsScenarioSetup()
        {
            try
            {
                var accountType = (from a in _dataContext.ifrs_scenario_setup
                                   where a.Deleted == false
                                   select new IfrsScenarioSetupObj
                                   {
                                       ScenarioId = a.ScenarioId,
                                       Scenario = a.Scenario,
                                       Likelihood = a.Likelihood,
                                       Rate = a.Rate
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<IFRSSetupDataObj> GetAllIFRSSetupData()
        {
            try
            {
                var setup = (from a in _dataContext.ifrs_setup_data
                             where a.Deleted == false
                             select new IFRSSetupDataObj
                             {
                                 SetUpId = a.SetUpId,
                                 Ccf = a.CCF,
                                 Classification_Type = a.Classification_Type,
                                 Deteroriation_Level = a.Deteroriation_Level,
                                 GroupBased = a.GroupBased,
                                 Historical_PD_Year_Count = a.Historical_PD_Year_Count,
                                 Ltpdapproach = a.Ltpdapproach,
                                 PdBasis = a.PDBasis,
                                 RunDate = a.RunDate,
                                 Threshold = a.Threshold,
                             }).ToList();

                return setup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LGDHistoryObj> GetAllLoanLGDHistory()
        {
            try
            {
                var variable = (from a in _dataContext.credit_lgd_historyinformation
                                where a.Deleted == false
                                select new LGDHistoryObj
                                {
                                    HistoricalLGDId = a.HistoricalLGDId,
                                    LoanAmount = a.LoanAmount,
                                    CustomerName = a.CustomerName,
                                    LoanReferenceNumber = a.LoanReferenceNumber,
                                    ProductName = a.ProductName,
                                    ProductCode = a.ProductCode,
                                    EffectiveDate = a.EffectiveDate,
                                    MaturityDate = a.MaturityDate,
                                    Cr = a.CR,
                                    Eir = a.EIR,
                                    Frequency = a.Frequency,
                                    Field1 = a.Field1,
                                    Field2 = a.Field2,
                                    Field3 = a.Field3,
                                    Field4 = a.Field4,
                                    Field5 = a.Field5,
                                    Field6 = a.Field6,
                                    Field7 = a.Field7,
                                    Field8 = a.Field8,
                                    Field9 = a.Field9,
                                    Field10 = a.Field10,
                                    Field11 = a.Field11,
                                    Field12 = a.Field12,
                                    Field13 = a.Field13,
                                    Field14 = a.Field14,
                                    Field15 = a.Field15,
                                    Field16 = a.Field16,
                                    Field17 = a.Field17,
                                    Field18 = a.Field18,
                                    Field19 = a.Field19,
                                    Field20 = a.Field20,
                                    Field21 = a.Field21,
                                    Field22 = a.Field22,
                                    Field23 = a.Field23,
                                    Field24 = a.Field24,
                                    Field25 = a.Field25,
                                    Field26 = a.Field26,
                                    Field27 = a.Field27,
                                    Field28 = a.Field28,
                                    Field29 = a.Field29,
                                    Field30 = a.Field30,
                                    Field31 = a.Field31,
                                    Field32 = a.Field32,
                                    Field33 = a.Field33,
                                    Field34 = a.Field34,
                                    Field35 = a.Field35,
                                    Field36 = a.Field36,
                                    Field37 = a.Field37,
                                    Field38 = a.Field38,
                                    Field39 = a.Field39,
                                    Field40 = a.Field40,
                                    Field41 = a.Field41,
                                    Field42 = a.Field42,
                                    Field43 = a.Field43,
                                    Field44 = a.Field44,
                                    Field45 = a.Field45,
                                    Field46 = a.Field46,
                                    Field47 = a.Field47,
                                    Field48 = a.Field48,
                                    Field49 = a.Field49,
                                    Field50 = a.Field50,
                                    Field51 = a.Field51,
                                    Field52 = a.Field52,
                                    Field53 = a.Field53,
                                    Field54 = a.Field54,
                                    Field55 = a.Field55,
                                    Field56 = a.Field56,
                                    Field57 = a.Field57,
                                    Field58 = a.Field58,
                                    Field59 = a.Field59,
                                    Field60 = a.Field60
                                }).ToList();

                return variable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<MacroEconomicVariableObj> GetAllMacroEconomicVariable()
        {
            try
            {
                var variable = (from a in _dataContext.ifrs_macroeconomic_variables_year
                                where a.Deleted == false
                                select new MacroEconomicVariableObj
                                {
                                    MacroEconomicVariableId = a.MacroEconomicVariableId,
                                    Year = a.Year,
                                    Unemployement = a.Unemployement,
                                    Erosion = a.erosion,
                                    ForegnEx = a.ForegnEx,
                                    Gdp = a.GDP,
                                    Inflation = a.Inflation,
                                    Otherfactor = a.otherfactor,
                                    Others = a.Others,
                                }).ToList();

                return variable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ScoreCardHistoryObj> GetAllScoreCardHistory()
        {

            try
            {
                var variable = (from a in _dataContext.credit_individualapplicationscorecard_history
                                where a.Deleted == false
                                select new ScoreCardHistoryObj
                                {
                                    ApplicationScoreCardId = a.ApplicationScoreCardId,
                                    LoanAmount = a.LoanAmount,
                                    CustomerName = a.CustomerName,
                                    LoanReferenceNumber = a.LoanReferenceNumber,
                                    ProductCode = a.ProductCode,
                                    ProductName = a.ProductName,
                                    EffectiveDate = a.EffectiveDate,
                                    MaturityDate = a.MaturityDate,
                                    Field1 = a.Field1,
                                    Field2 = a.Field2,
                                    Field3 = a.Field3,
                                    Field4 = a.Field4,
                                    Field5 = a.Field5,
                                    Field6 = a.Field6,
                                    Field7 = a.Field7,
                                    Field8 = a.Field8,
                                    Field9 = a.Field9,
                                    Field10 = a.Field10,
                                    Field11 = a.Field11,
                                    Field12 = a.Field12,
                                    Field13 = a.Field13,
                                    Field14 = a.Field14,
                                    Field15 = a.Field15,
                                    Field16 = a.Field16,
                                    Field17 = a.Field17,
                                    Field18 = a.Field18,
                                    Field19 = a.Field19,
                                    Field20 = a.Field20,
                                    Field21 = a.Field21,
                                    Field22 = a.Field22,
                                    Field23 = a.Field23,
                                    Field24 = a.Field24,
                                    Field25 = a.Field25,
                                    Field26 = a.Field26,
                                    Field27 = a.Field27,
                                    Field28 = a.Field28,
                                    Field29 = a.Field29,
                                    Field30 = a.Field30
                                }).ToList();

                return variable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IfrsScenarioSetupObj GetIfrsScenarioSetup(int id)
        {
            try
            {
                var accountType = (from a in _dataContext.ifrs_scenario_setup
                                   where a.Deleted == false && a.ScenarioId == id
                                   select new IfrsScenarioSetupObj
                                   {
                                       ScenarioId = a.ScenarioId,
                                       Scenario = a.Scenario,
                                       Likelihood = a.Likelihood,
                                       Rate = a.Rate
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IFRSSetupDataObj GetIFRSSetupData(int setupId)
        {
            try
            {
                var setup = (from a in _dataContext.ifrs_setup_data
                             where a.SetUpId == setupId && a.Deleted == false
                             select new IFRSSetupDataObj
                             {
                                 SetUpId = a.SetUpId,
                                 Ccf = a.CCF,
                                 Classification_Type = a.Classification_Type,
                                 Deteroriation_Level = a.Deteroriation_Level,
                                 GroupBased = a.GroupBased,
                                 Historical_PD_Year_Count = a.Historical_PD_Year_Count,
                                 Ltpdapproach = a.Ltpdapproach,
                                 PdBasis = a.PDBasis,
                                 RunDate = a.RunDate,
                                 Threshold = a.Threshold,
                             }).FirstOrDefault();

                return setup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MacroEconomicVariableObj GetMacroEconomicVariable(int macroEconomicVariableId)
        {
            try
            {
                var variable = (from a in _dataContext.ifrs_macroeconomic_variables_year
                                where a.MacroEconomicVariableId == macroEconomicVariableId && a.Deleted == false
                                select new MacroEconomicVariableObj
                                {
                                    MacroEconomicVariableId = a.MacroEconomicVariableId,
                                    Year = a.Year,
                                    Unemployement = a.Unemployement,
                                    Erosion = a.erosion,
                                    ForegnEx = a.ForegnEx,
                                    Gdp = a.GDP,
                                    Inflation = a.Inflation,
                                    Otherfactor = a.otherfactor,
                                    Others = a.Others,
                                }).FirstOrDefault();

                return variable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateIfrsScenarioSetup(IfrsScenarioSetupObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.ScenarioId > 0)
                {
                    var accountTypeExist = _dataContext.ifrs_scenario_setup.Find(entity.ScenarioId);
                    if (accountTypeExist != null)
                    {
                        accountTypeExist.ScenarioId = entity.ScenarioId;
                        accountTypeExist.Scenario = entity.Scenario;
                        accountTypeExist.Likelihood = entity.Likelihood;
                        accountTypeExist.Rate = entity.Rate;
                        accountTypeExist.Active = true;
                        accountTypeExist.Deleted = false;
                        accountTypeExist.UpdatedBy = entity.CreatedBy;
                        accountTypeExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var accountType = new ifrs_scenario_setup
                    {
                        ScenarioId = entity.ScenarioId,
                        Scenario = entity.Scenario,
                        Likelihood = entity.Likelihood,
                        Rate = entity.Rate,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.ifrs_scenario_setup.Add(accountType);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateScoreCardHistoryByLoanDisbursement(int loanId, string createdBy)
        {
            try { 
            List<credit_individualapplicationscorecard_history> histories = new List<credit_individualapplicationscorecard_history>();
            var now = DateTime.Now.Date;
            var loan = _dataContext.credit_loan.Where(x => x.LoanId == loanId && x.Deleted == false).FirstOrDefault();
            var loanInterest = _dataContext.credit_loanscheduledaily.Where(x => x.LoanId == loanId && x.Date.Date == now && x.Deleted == false).FirstOrDefault();
            if (loan == null) return false;
            var customerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loan.CustomerId).FirstOrDefault();
            var product = _dataContext.credit_product.Where(x => x.ProductId == loan.ProductId && x.Deleted == false).FirstOrDefault();
            if (customerName.CustomerTypeId == 1)
            {
                var item = _dataContext.credit_individualapplicationscorecard.
                    Where(x => x.LoanApplicationId == loan.LoanApplicationId).FirstOrDefault();
                if (item == null) throw new Exception("Loan Application does not have score card", new Exception());
                credit_individualapplicationscorecard_history history = null;
                history = _dataContext.credit_individualapplicationscorecard_history.Where(x => x.LoanReferenceNumber == loan.LoanRefNumber).FirstOrDefault();
                if (history != null)
                {
                    history.LoanAmount = loan.PrincipalAmount;
                    history.CustomerName = customerName.FirstName + " " + customerName.LastName;
                        history.OutstandingBalance = loan.PrincipalAmount; //+ loanInterest.AccruedInterest;
                    history.CustomerTypeId = customerName.CustomerTypeId;
                    history.LoanReferenceNumber = loan.LoanRefNumber;
                    history.ProductCode = product.ProductCode;
                    history.ProductName = product.ProductName;
                    history.EffectiveDate = loan.EffectiveDate;
                    history.MaturityDate = loan.MaturityDate;
                    history.Field1 = item.Field1 == 1000000 ? null : item.Field1;
                    history.Field2 = item.Field2 == 1000000 ? null : item.Field2;
                    history.Field3 = item.Field3 == 1000000 ? null : item.Field3;
                    history.Field4 = item.Field4 == 1000000 ? null : item.Field4;
                    history.Field5 = item.Field5 == 1000000 ? null : item.Field5;
                    history.Field6 = item.Field6 == 1000000 ? null : item.Field6;
                    history.Field7 = item.Field7 == 1000000 ? null : item.Field7;
                    history.Field8 = item.Field8 == 1000000 ? null : item.Field8;
                    history.Field9 = item.Field9 == 1000000 ? null : item.Field9;
                    history.Field10 = item.Field10 == 1000000 ? null : item.Field10;
                    history.Field11 = item.Field11 == 1000000 ? null : item.Field11;
                    history.Field12 = item.Field12 == 1000000 ? null : item.Field12;
                    history.Field13 = item.Field13 == 1000000 ? null : item.Field13;
                    history.Field14 = item.Field14 == 1000000 ? null : item.Field14;
                    history.Field15 = item.Field15 == 1000000 ? null : item.Field15;
                    history.Field16 = item.Field16 == 1000000 ? null : item.Field16;
                    history.Field17 = item.Field17 == 1000000 ? null : item.Field17;
                    history.Field18 = item.Field18 == 1000000 ? null : item.Field18;
                    history.Field19 = item.Field19 == 1000000 ? null : item.Field19;
                    history.Field20 = item.Field20 == 1000000 ? null : item.Field20;
                    history.Field21 = item.Field21 == 1000000 ? null : item.Field21;
                    history.Field22 = item.Field22 == 1000000 ? null : item.Field22;
                    history.Field23 = item.Field23 == 1000000 ? null : item.Field23;
                    history.Field24 = item.Field24 == 1000000 ? null : item.Field24;
                    history.Field25 = item.Field25 == 1000000 ? null : item.Field25;
                    history.Field26 = item.Field26 == 1000000 ? null : item.Field26;
                    history.Field27 = item.Field27 == 1000000 ? null : item.Field27;
                    history.Field28 = item.Field28 == 1000000 ? null : item.Field28;
                    history.Field29 = item.Field29 == 1000000 ? null : item.Field29;
                    history.Field30 = item.Field30 == 1000000 ? null : item.Field30;
                    history.Active = true;
                    history.Deleted = false;
                    history.UpdatedBy = createdBy;
                    history.UpdatedOn = DateTime.Now;
                }
                else
                {
                        history = new credit_individualapplicationscorecard_history
                        {
                            LoanAmount = loan.PrincipalAmount,
                            OutstandingBalance = loan.PrincipalAmount,// + loanInterest.AccruedInterest,
                        CustomerName = customerName.FirstName + " " + customerName.LastName,
                        CustomerTypeId = customerName.CustomerTypeId,
                        LoanReferenceNumber = loan.LoanRefNumber,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        EffectiveDate = loan.EffectiveDate,
                        MaturityDate = loan.MaturityDate,
                        Field1 = item.Field1 == 1000000 ? null : item.Field1,
                        Field2 = item.Field2 == 1000000 ? null : item.Field2,
                        Field3 = item.Field3 == 1000000 ? null : item.Field3,
                        Field4 = item.Field4 == 1000000 ? null : item.Field4,
                        Field5 = item.Field5 == 1000000 ? null : item.Field5,
                        Field6 = item.Field6 == 1000000 ? null : item.Field6,
                        Field7 = item.Field7 == 1000000 ? null : item.Field7,
                        Field8 = item.Field8 == 1000000 ? null : item.Field8,
                        Field9 = item.Field9 == 1000000 ? null : item.Field9,
                        Field10 = item.Field10 == 1000000 ? null : item.Field10,
                        Field11 = item.Field11 == 1000000 ? null : item.Field11,
                        Field12 = item.Field12 == 1000000 ? null : item.Field12,
                        Field13 = item.Field13 == 1000000 ? null : item.Field13,
                        Field14 = item.Field14 == 1000000 ? null : item.Field14,
                        Field15 = item.Field15 == 1000000 ? null : item.Field15,
                        Field16 = item.Field16 == 1000000 ? null : item.Field16,
                        Field17 = item.Field17 == 1000000 ? null : item.Field17,
                        Field18 = item.Field18 == 1000000 ? null : item.Field18,
                        Field19 = item.Field19 == 1000000 ? null : item.Field19,
                        Field20 = item.Field20 == 1000000 ? null : item.Field20,
                        Field21 = item.Field21 == 1000000 ? null : item.Field21,
                        Field22 = item.Field22 == 1000000 ? null : item.Field22,
                        Field23 = item.Field23 == 1000000 ? null : item.Field23,
                        Field24 = item.Field24 == 1000000 ? null : item.Field24,
                        Field25 = item.Field25 == 1000000 ? null : item.Field25,
                        Field26 = item.Field26 == 1000000 ? null : item.Field26,
                        Field27 = item.Field27 == 1000000 ? null : item.Field27,
                        Field28 = item.Field28 == 1000000 ? null : item.Field28,
                        Field29 = item.Field29 == 1000000 ? null : item.Field29,
                        Field30 = item.Field30 == 1000000 ? null : item.Field30,
                        Active = true,
                        Deleted = false,
                        CreatedBy = createdBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_individualapplicationscorecard_history.Add(history);
                    histories.Add(history);
                    AddScoreCardHistoryDetail(histories);
                }
            }
            else
            {
                var item = _dataContext.credit_corporateapplicationscorecard.
                   Where(x => x.LoanApplicationId == loan.LoanApplicationId).FirstOrDefault();
                if (item == null) throw new Exception("Loan Application does not have score card");
                credit_individualapplicationscorecard_history history = null;
                history = _dataContext.credit_individualapplicationscorecard_history.Where(x => x.LoanReferenceNumber == loan.LoanRefNumber).FirstOrDefault();
                if (history != null)
                {
                    history.LoanAmount = loan.PrincipalAmount;
                    history.CustomerName = customerName.FirstName + " " + customerName.LastName;
                    history.OutstandingBalance = loan.PrincipalAmount + loanInterest.AccruedInterest;
                    history.CustomerTypeId = customerName.CustomerTypeId;
                    history.LoanReferenceNumber = loan.LoanRefNumber;
                    history.ProductCode = product.ProductCode;
                    history.ProductName = product.ProductName;
                    history.EffectiveDate = loan.EffectiveDate;
                    history.MaturityDate = loan.MaturityDate;
                    history.Field1 = item.Field1 == 1000000 ? null : item.Field1;
                    history.Field2 = item.Field2 == 1000000 ? null : item.Field2;
                    history.Field3 = item.Field3 == 1000000 ? null : item.Field3;
                    history.Field4 = item.Field4 == 1000000 ? null : item.Field4;
                    history.Field5 = item.Field5 == 1000000 ? null : item.Field5;
                    history.Field6 = item.Field6 == 1000000 ? null : item.Field6;
                    history.Field7 = item.Field7 == 1000000 ? null : item.Field7;
                    history.Field8 = item.Field8 == 1000000 ? null : item.Field8;
                    history.Field9 = item.Field9 == 1000000 ? null : item.Field9;
                    history.Field10 = item.Field10 == 1000000 ? null : item.Field10;
                    history.Field11 = item.Field11 == 1000000 ? null : item.Field11;
                    history.Field12 = item.Field12 == 1000000 ? null : item.Field12;
                    history.Field13 = item.Field13 == 1000000 ? null : item.Field13;
                    history.Field14 = item.Field14 == 1000000 ? null : item.Field14;
                    history.Field15 = item.Field15 == 1000000 ? null : item.Field15;
                    history.Field16 = item.Field16 == 1000000 ? null : item.Field16;
                    history.Field17 = item.Field17 == 1000000 ? null : item.Field17;
                    history.Field18 = item.Field18 == 1000000 ? null : item.Field18;
                    history.Field19 = item.Field19 == 1000000 ? null : item.Field19;
                    history.Field20 = item.Field20 == 1000000 ? null : item.Field20;
                    history.Field21 = item.Field21 == 1000000 ? null : item.Field21;
                    history.Field22 = item.Field22 == 1000000 ? null : item.Field22;
                    history.Field23 = item.Field23 == 1000000 ? null : item.Field23;
                    history.Field24 = item.Field24 == 1000000 ? null : item.Field24;
                    history.Field25 = item.Field25 == 1000000 ? null : item.Field25;
                    history.Field26 = item.Field26 == 1000000 ? null : item.Field26;
                    history.Field27 = item.Field27 == 1000000 ? null : item.Field27;
                    history.Field28 = item.Field28 == 1000000 ? null : item.Field28;
                    history.Field29 = item.Field29 == 1000000 ? null : item.Field29;
                    history.Field30 = item.Field30 == 1000000 ? null : item.Field30;
                    history.Active = true;
                    history.Deleted = false;
                    history.UpdatedBy = createdBy;
                    history.UpdatedOn = DateTime.Now;
                }
                else
                {
                    history = new credit_individualapplicationscorecard_history
                    {
                        LoanAmount = loan.PrincipalAmount,
                        OutstandingBalance = loan.PrincipalAmount + loanInterest.AccruedInterest,
                        CustomerName = customerName.FirstName + " " + customerName.LastName,
                        CustomerTypeId = customerName.CustomerTypeId,
                        LoanReferenceNumber = loan.LoanRefNumber,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        EffectiveDate = loan.EffectiveDate,
                        MaturityDate = loan.MaturityDate,
                        Field1 = item.Field1 == 1000000 ? null : item.Field1,
                        Field2 = item.Field2 == 1000000 ? null : item.Field2,
                        Field3 = item.Field3 == 1000000 ? null : item.Field3,
                        Field4 = item.Field4 == 1000000 ? null : item.Field4,
                        Field5 = item.Field5 == 1000000 ? null : item.Field5,
                        Field6 = item.Field6 == 1000000 ? null : item.Field6,
                        Field7 = item.Field7 == 1000000 ? null : item.Field7,
                        Field8 = item.Field8 == 1000000 ? null : item.Field8,
                        Field9 = item.Field9 == 1000000 ? null : item.Field9,
                        Field10 = item.Field10 == 1000000 ? null : item.Field10,
                        Field11 = item.Field11 == 1000000 ? null : item.Field11,
                        Field12 = item.Field12 == 1000000 ? null : item.Field12,
                        Field13 = item.Field13 == 1000000 ? null : item.Field13,
                        Field14 = item.Field14 == 1000000 ? null : item.Field14,
                        Field15 = item.Field15 == 1000000 ? null : item.Field15,
                        Field16 = item.Field16 == 1000000 ? null : item.Field16,
                        Field17 = item.Field17 == 1000000 ? null : item.Field17,
                        Field18 = item.Field18 == 1000000 ? null : item.Field18,
                        Field19 = item.Field19 == 1000000 ? null : item.Field19,
                        Field20 = item.Field20 == 1000000 ? null : item.Field20,
                        Field21 = item.Field21 == 1000000 ? null : item.Field21,
                        Field22 = item.Field22 == 1000000 ? null : item.Field22,
                        Field23 = item.Field23 == 1000000 ? null : item.Field23,
                        Field24 = item.Field24 == 1000000 ? null : item.Field24,
                        Field25 = item.Field25 == 1000000 ? null : item.Field25,
                        Field26 = item.Field26 == 1000000 ? null : item.Field26,
                        Field27 = item.Field27 == 1000000 ? null : item.Field27,
                        Field28 = item.Field28 == 1000000 ? null : item.Field28,
                        Field29 = item.Field29 == 1000000 ? null : item.Field29,
                        Field30 = item.Field30 == 1000000 ? null : item.Field30,
                        Active = true,
                        Deleted = false,
                        CreatedBy = createdBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_individualapplicationscorecard_history.Add(history);
                    histories.Add(history);
                    AddScoreCardHistoryDetail(histories);
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

        public bool UploadLGDHistory(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                bool response = false;
                List<credit_lgd_historyinformation> uploadedRecord = new List<credit_lgd_historyinformation>();
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
                                var item = new credit_lgd_historyinformation
                                {
                                    LoanAmount = workSheet.Cells[i, 1].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 1].Value.ToString()),
                                    CustomerName = workSheet.Cells[i, 2].Value == null ? string.Empty : workSheet.Cells[i, 2].Value.ToString(),
                                    LoanReferenceNumber = workSheet.Cells[i, 3].Value == null ? string.Empty : workSheet.Cells[i, 3].Value.ToString(),
                                    ProductCode = workSheet.Cells[i, 4].Value == null ? string.Empty : workSheet.Cells[i, 4].Value.ToString(),
                                    ProductName = workSheet.Cells[i, 5].Value == null ? string.Empty : workSheet.Cells[i, 5].Value.ToString(),
                                    EffectiveDate = workSheet.Cells[i, 6].Value == null ? DateTime.Now : DateTime.Parse(workSheet.Cells[i, 6].Value.ToString()),
                                    MaturityDate = workSheet.Cells[i, 7].Value == null ? DateTime.Now : DateTime.Parse(workSheet.Cells[i, 7].Value.ToString()),
                                    CR = workSheet.Cells[i, 8].Value == null ? 1000000000000 : double.Parse(workSheet.Cells[i, 8].Value.ToString()),
                                    EIR = workSheet.Cells[i, 9].Value == null ? 1000000000000 : double.Parse(workSheet.Cells[i, 9].Value.ToString()),
                                    Frequency = workSheet.Cells[i, 10].Value == null ? string.Empty : workSheet.Cells[i, 10].Value.ToString(),
                                    Field1 = workSheet.Cells[i, 11].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 11].Value.ToString()),
                                    Field2 = workSheet.Cells[i, 12].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 12].Value.ToString()),
                                    Field3 = workSheet.Cells[i, 13].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 13].Value.ToString()),
                                    Field4 = workSheet.Cells[i, 14].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 14].Value.ToString()),
                                    Field5 = workSheet.Cells[i, 15].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 15].Value.ToString()),
                                    Field6 = workSheet.Cells[i, 16].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 16].Value.ToString()),
                                    Field7 = workSheet.Cells[i, 17].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 17].Value.ToString()),
                                    Field8 = workSheet.Cells[i, 18].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 18].Value.ToString()),
                                    Field9 = workSheet.Cells[i, 19].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 19].Value.ToString()),
                                    Field10 = workSheet.Cells[i, 20].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 20].Value.ToString()),
                                    Field11 = workSheet.Cells[i, 21].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 21].Value.ToString()),
                                    Field12 = workSheet.Cells[i, 22].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 22].Value.ToString()),
                                    Field13 = workSheet.Cells[i, 23].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 23].Value.ToString()),
                                    Field14 = workSheet.Cells[i, 24].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 24].Value.ToString()),
                                    Field15 = workSheet.Cells[i, 25].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 25].Value.ToString()),
                                    Field16 = workSheet.Cells[i, 26].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 26].Value.ToString()),
                                    Field17 = workSheet.Cells[i, 27].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 27].Value.ToString()),
                                    Field18 = workSheet.Cells[i, 28].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 28].Value.ToString()),
                                    Field19 = workSheet.Cells[i, 29].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 29].Value.ToString()),
                                    Field20 = workSheet.Cells[i, 30].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 30].Value.ToString()),
                                    Field21 = workSheet.Cells[i, 31].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 31].Value.ToString()),
                                    Field22 = workSheet.Cells[i, 32].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 32].Value.ToString()),
                                    Field23 = workSheet.Cells[i, 33].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 33].Value.ToString()),
                                    Field24 = workSheet.Cells[i, 34].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 34].Value.ToString()),
                                    Field25 = workSheet.Cells[i, 35].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 35].Value.ToString()),
                                    Field26 = workSheet.Cells[i, 36].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 36].Value.ToString()),
                                    Field27 = workSheet.Cells[i, 37].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 37].Value.ToString()),
                                    Field28 = workSheet.Cells[i, 38].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 38].Value.ToString()),
                                    Field29 = workSheet.Cells[i, 39].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 39].Value.ToString()),
                                    Field30 = workSheet.Cells[i, 40].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 40].Value.ToString()),
                                    Field31 = workSheet.Cells[i, 41].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 41].Value.ToString()),
                                    Field32 = workSheet.Cells[i, 42].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 42].Value.ToString()),
                                    Field33 = workSheet.Cells[i, 43].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 43].Value.ToString()),
                                    Field34 = workSheet.Cells[i, 44].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 44].Value.ToString()),
                                    Field35 = workSheet.Cells[i, 45].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 45].Value.ToString()),
                                    Field36 = workSheet.Cells[i, 46].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 46].Value.ToString()),
                                    Field37 = workSheet.Cells[i, 47].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 47].Value.ToString()),
                                    Field38 = workSheet.Cells[i, 48].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 48].Value.ToString()),
                                    Field39 = workSheet.Cells[i, 49].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 49].Value.ToString()),
                                    Field40 = workSheet.Cells[i, 50].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 50].Value.ToString()),
                                    Field41 = workSheet.Cells[i, 51].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 51].Value.ToString()),
                                    Field42 = workSheet.Cells[i, 52].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 52].Value.ToString()),
                                    Field43 = workSheet.Cells[i, 53].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 53].Value.ToString()),
                                    Field44 = workSheet.Cells[i, 54].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 54].Value.ToString()),
                                    Field45 = workSheet.Cells[i, 55].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 55].Value.ToString()),
                                    Field46 = workSheet.Cells[i, 56].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 56].Value.ToString()),
                                    Field47 = workSheet.Cells[i, 57].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 57].Value.ToString()),
                                    Field48 = workSheet.Cells[i, 58].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 58].Value.ToString()),
                                    Field49 = workSheet.Cells[i, 59].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 59].Value.ToString()),
                                    Field50 = workSheet.Cells[i, 60].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 60].Value.ToString()),
                                    Field51 = workSheet.Cells[i, 61].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 61].Value.ToString()),
                                    Field52 = workSheet.Cells[i, 62].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 62].Value.ToString()),
                                    Field53 = workSheet.Cells[i, 63].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 63].Value.ToString()),
                                    Field54 = workSheet.Cells[i, 64].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 64].Value.ToString()),
                                    Field55 = workSheet.Cells[i, 65].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 65].Value.ToString()),
                                    Field56 = workSheet.Cells[i, 66].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 66].Value.ToString()),
                                    Field57 = workSheet.Cells[i, 67].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 67].Value.ToString()),
                                    Field58 = workSheet.Cells[i, 68].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 68].Value.ToString()),
                                    Field59 = workSheet.Cells[i, 69].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 69].Value.ToString()),
                                    Field60 = workSheet.Cells[i, 70].Value == null ? 1000000000000 : decimal.Parse(workSheet.Cells[i, 70].Value.ToString()),
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                List<credit_lgd_historyinformation> histories = new List<credit_lgd_historyinformation>();
                credit_lgd_historyinformation history = null;
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        history = _dataContext.credit_lgd_historyinformation.Where(x => x.LoanReferenceNumber == item.LoanReferenceNumber).FirstOrDefault();
                        if (history != null)
                        {
                            history.LoanAmount = item.LoanAmount;
                            history.CustomerName = item.CustomerName;
                            history.LoanReferenceNumber = item.LoanReferenceNumber;
                            history.ProductCode = item.ProductCode;
                            history.ProductName = item.ProductName;
                            history.EffectiveDate = item.EffectiveDate;
                            history.MaturityDate = item.MaturityDate;
                            history.CR = item.CR;
                            history.EIR = item.EIR;
                            history.Frequency = item.Frequency;
                            history.Field1 = item.Field1 == 1000000000000 ? null : item.Field1;
                            history.Field2 = item.Field2 == 1000000000000 ? null : item.Field2;
                            history.Field3 = item.Field3 == 1000000000000 ? null : item.Field3;
                            history.Field4 = item.Field4 == 1000000000000 ? null : item.Field4;
                            history.Field5 = item.Field5 == 1000000000000 ? null : item.Field5;
                            history.Field6 = item.Field6 == 1000000000000 ? null : item.Field6;
                            history.Field7 = item.Field7 == 1000000000000 ? null : item.Field7;
                            history.Field8 = item.Field8 == 1000000000000 ? null : item.Field8;
                            history.Field9 = item.Field9 == 1000000000000 ? null : item.Field9;
                            history.Field10 = item.Field10 == 1000000000000 ? null : item.Field10;
                            history.Field11 = item.Field11 == 1000000000000 ? null : item.Field11;
                            history.Field12 = item.Field12 == 1000000000000 ? null : item.Field12;
                            history.Field13 = item.Field13 == 1000000000000 ? null : item.Field13;
                            history.Field14 = item.Field14 == 1000000000000 ? null : item.Field14;
                            history.Field15 = item.Field15 == 1000000000000 ? null : item.Field15;
                            history.Field16 = item.Field16 == 1000000000000 ? null : item.Field16;
                            history.Field17 = item.Field17 == 1000000000000 ? null : item.Field17;
                            history.Field18 = item.Field18 == 1000000000000 ? null : item.Field18;
                            history.Field19 = item.Field19 == 1000000000000 ? null : item.Field19;
                            history.Field20 = item.Field20 == 1000000000000 ? null : item.Field20;
                            history.Field21 = item.Field21 == 1000000000000 ? null : item.Field21;
                            history.Field22 = item.Field22 == 1000000000000 ? null : item.Field22;
                            history.Field23 = item.Field23 == 1000000000000 ? null : item.Field23;
                            history.Field24 = item.Field24 == 1000000000000 ? null : item.Field24;
                            history.Field25 = item.Field25 == 1000000000000 ? null : item.Field25;
                            history.Field26 = item.Field26 == 1000000000000 ? null : item.Field26;
                            history.Field27 = item.Field27 == 1000000000000 ? null : item.Field27;
                            history.Field28 = item.Field28 == 1000000000000 ? null : item.Field28;
                            history.Field29 = item.Field29 == 1000000000000 ? null : item.Field29;
                            history.Field30 = item.Field30 == 1000000000000 ? null : item.Field30;
                            history.Field31 = item.Field31 == 1000000000000 ? null : item.Field31;
                            history.Field32 = item.Field32 == 1000000000000 ? null : item.Field32;
                            history.Field33 = item.Field33 == 1000000000000 ? null : item.Field33;
                            history.Field34 = item.Field34 == 1000000000000 ? null : item.Field34;
                            history.Field35 = item.Field35 == 1000000000000 ? null : item.Field35;
                            history.Field36 = item.Field36 == 1000000000000 ? null : item.Field36;
                            history.Field37 = item.Field37 == 1000000000000 ? null : item.Field37;
                            history.Field38 = item.Field38 == 1000000000000 ? null : item.Field38;
                            history.Field39 = item.Field39 == 1000000000000 ? null : item.Field39;
                            history.Field40 = item.Field40 == 1000000000000 ? null : item.Field40;
                            history.Field41 = item.Field41 == 1000000000000 ? null : item.Field41;
                            history.Field42 = item.Field42 == 1000000000000 ? null : item.Field42;
                            history.Field43 = item.Field43 == 1000000000000 ? null : item.Field43;
                            history.Field44 = item.Field44 == 1000000000000 ? null : item.Field44;
                            history.Field45 = item.Field45 == 1000000000000 ? null : item.Field45;
                            history.Field46 = item.Field46 == 1000000000000 ? null : item.Field46;
                            history.Field47 = item.Field47 == 1000000000000 ? null : item.Field47;
                            history.Field48 = item.Field48 == 1000000000000 ? null : item.Field48;
                            history.Field49 = item.Field49 == 1000000000000 ? null : item.Field49;
                            history.Field50 = item.Field50 == 1000000000000 ? null : item.Field50;
                            history.Field51 = item.Field51 == 1000000000000 ? null : item.Field51;
                            history.Field52 = item.Field52 == 1000000000000 ? null : item.Field52;
                            history.Field53 = item.Field53 == 1000000000000 ? null : item.Field53;
                            history.Field54 = item.Field54 == 1000000000000 ? null : item.Field54;
                            history.Field55 = item.Field55 == 1000000000000 ? null : item.Field55;
                            history.Field56 = item.Field56 == 1000000000000 ? null : item.Field56;
                            history.Field57 = item.Field57 == 1000000000000 ? null : item.Field57;
                            history.Field58 = item.Field58 == 1000000000000 ? null : item.Field58;
                            history.Field59 = item.Field59 == 1000000000000 ? null : item.Field59;
                            history.Field60 = item.Field60 == 1000000000000 ? null : item.Field60;
                            history.Active = true;
                            history.Deleted = false;
                            history.CreatedBy = createdBy;
                            history.CreatedOn = DateTime.Now;
                            history.UpdatedBy = createdBy;
                            history.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            history = new credit_lgd_historyinformation
                            {
                                LoanAmount = item.LoanAmount,
                                CustomerName = item.CustomerName,
                                LoanReferenceNumber = item.LoanReferenceNumber,
                                ProductCode = item.ProductCode,
                                ProductName = item.ProductName,
                                EffectiveDate = item.EffectiveDate,
                                MaturityDate = item.MaturityDate,
                                CR = item.CR,
                                EIR = item.EIR,
                                Frequency = item.Frequency,
                                Field1 = item.Field1 == 1000000000000 ? null : item.Field1,
                                Field2 = item.Field2 == 1000000000000 ? null : item.Field2,
                                Field3 = item.Field3 == 1000000000000 ? null : item.Field3,
                                Field4 = item.Field4 == 1000000000000 ? null : item.Field4,
                                Field5 = item.Field5 == 1000000000000 ? null : item.Field5,
                                Field6 = item.Field6 == 1000000000000 ? null : item.Field6,
                                Field7 = item.Field7 == 1000000000000 ? null : item.Field7,
                                Field8 = item.Field8 == 1000000000000 ? null : item.Field8,
                                Field9 = item.Field9 == 1000000000000 ? null : item.Field9,
                                Field10 = item.Field10 == 1000000000000 ? null : item.Field10,
                                Field11 = item.Field11 == 1000000000000 ? null : item.Field11,
                                Field12 = item.Field12 == 1000000000000 ? null : item.Field12,
                                Field13 = item.Field13 == 1000000000000 ? null : item.Field13,
                                Field14 = item.Field14 == 1000000000000 ? null : item.Field14,
                                Field15 = item.Field15 == 1000000000000 ? null : item.Field15,
                                Field16 = item.Field16 == 1000000000000 ? null : item.Field16,
                                Field17 = item.Field17 == 1000000000000 ? null : item.Field17,
                                Field18 = item.Field18 == 1000000000000 ? null : item.Field18,
                                Field19 = item.Field19 == 1000000000000 ? null : item.Field19,
                                Field20 = item.Field20 == 1000000000000 ? null : item.Field20,
                                Field21 = item.Field21 == 1000000000000 ? null : item.Field21,
                                Field22 = item.Field22 == 1000000000000 ? null : item.Field22,
                                Field23 = item.Field23 == 1000000000000 ? null : item.Field23,
                                Field24 = item.Field24 == 1000000000000 ? null : item.Field24,
                                Field25 = item.Field25 == 1000000000000 ? null : item.Field25,
                                Field26 = item.Field26 == 1000000000000 ? null : item.Field26,
                                Field27 = item.Field27 == 1000000000000 ? null : item.Field27,
                                Field28 = item.Field28 == 1000000000000 ? null : item.Field28,
                                Field29 = item.Field29 == 1000000000000 ? null : item.Field29,
                                Field30 = item.Field30 == 1000000000000 ? null : item.Field30,
                                Field31 = item.Field31 == 1000000000000 ? null : item.Field31,
                                Field32 = item.Field32 == 1000000000000 ? null : item.Field32,
                                Field33 = item.Field33 == 1000000000000 ? null : item.Field33,
                                Field34 = item.Field34 == 1000000000000 ? null : item.Field34,
                                Field35 = item.Field35 == 1000000000000 ? null : item.Field35,
                                Field36 = item.Field36 == 1000000000000 ? null : item.Field36,
                                Field37 = item.Field37 == 1000000000000 ? null : item.Field37,
                                Field38 = item.Field38 == 1000000000000 ? null : item.Field38,
                                Field39 = item.Field39 == 1000000000000 ? null : item.Field39,
                                Field40 = item.Field40 == 1000000000000 ? null : item.Field40,
                                Field41 = item.Field41 == 1000000000000 ? null : item.Field41,
                                Field42 = item.Field42 == 1000000000000 ? null : item.Field42,
                                Field43 = item.Field43 == 1000000000000 ? null : item.Field43,
                                Field44 = item.Field44 == 1000000000000 ? null : item.Field44,
                                Field45 = item.Field45 == 1000000000000 ? null : item.Field45,
                                Field46 = item.Field46 == 1000000000000 ? null : item.Field46,
                                Field47 = item.Field47 == 1000000000000 ? null : item.Field47,
                                Field48 = item.Field48 == 1000000000000 ? null : item.Field48,
                                Field49 = item.Field49 == 1000000000000 ? null : item.Field49,
                                Field50 = item.Field50 == 1000000000000 ? null : item.Field50,
                                Field51 = item.Field51 == 1000000000000 ? null : item.Field51,
                                Field52 = item.Field52 == 1000000000000 ? null : item.Field52,
                                Field53 = item.Field53 == 1000000000000 ? null : item.Field53,
                                Field54 = item.Field54 == 1000000000000 ? null : item.Field54,
                                Field55 = item.Field55 == 1000000000000 ? null : item.Field55,
                                Field56 = item.Field56 == 1000000000000 ? null : item.Field56,
                                Field57 = item.Field57 == 1000000000000 ? null : item.Field57,
                                Field58 = item.Field58 == 1000000000000 ? null : item.Field58,
                                Field59 = item.Field59 == 1000000000000 ? null : item.Field59,
                                Field60 = item.Field60 == 1000000000000 ? null : item.Field60,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = createdBy,
                                UpdatedOn = DateTime.Now,
                            };
                            histories.Add(history);
                        }
                        _dataContext.credit_lgd_historyinformation.AddRange(histories);
                    }

                    response = _dataContext.SaveChanges() > 0;

                    AddLGDHistoryInformationDetail(uploadedRecord);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadMacroEconomicViriable(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                bool response = false;
                List<MacroEconomicVariableObj> uploadedRecord = new List<MacroEconomicVariableObj>();
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
                                var item = new MacroEconomicVariableObj
                                {
                                    Year = int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                                    Unemployement = double.Parse(workSheet.Cells[i, 2].Value.ToString()),
                                    Gdp = double.Parse(workSheet.Cells[i, 3].Value.ToString()),
                                    Inflation = double.Parse(workSheet.Cells[i, 4].Value.ToString()),
                                    Others = double.Parse(workSheet.Cells[i, 5].Value.ToString())
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }
                }
                List<ifrs_macroeconomic_variables_year> variables = new List<ifrs_macroeconomic_variables_year>();
                ifrs_macroeconomic_variables_year variable = null;
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        variable = _dataContext.ifrs_macroeconomic_variables_year.Where(x => x.Year == item.Year).FirstOrDefault();

                        if (variable != null)
                        {
                            variable.Unemployement = item.Unemployement;
                            variable.erosion = item.Erosion;
                            variable.ForegnEx = item.ForegnEx;
                            variable.GDP = item.Gdp;
                            variable.Inflation = item.Inflation;
                            variable.otherfactor = item.Otherfactor;
                            variable.Others = item.Others;
                            variable.Active = true;
                            variable.Deleted = false;
                            variable.CreatedBy = createdBy;
                            variable.CreatedOn = DateTime.Now;
                            variable.UpdatedBy = createdBy;
                            variable.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            variable = new ifrs_macroeconomic_variables_year
                            {
                                Year = item.Year,
                                Unemployement = item.Unemployement,
                                erosion = item.Erosion,
                                ForegnEx = item.ForegnEx,
                                GDP = item.Gdp,
                                Inflation = item.Inflation,
                                otherfactor = item.Otherfactor,
                                Others = item.Others,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = createdBy,
                                UpdatedOn = DateTime.Now,
                            };
                            variables.Add(variable);
                        }
                    }
                    _dataContext.ifrs_macroeconomic_variables_year.AddRange(variables);

                    response = _dataContext.SaveChanges() > 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UploadScoreCardHistory(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                bool response = false;
                List<credit_individualapplicationscorecard_history> uploadedRecord = new List<credit_individualapplicationscorecard_history>();
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
                                var item = new credit_individualapplicationscorecard_history
                                {
                                    LoanAmount = workSheet.Cells[i, 1].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 1].Value.ToString()),
                                    CustomerTypeId = workSheet.Cells[i, 2].Value == null ? 1000000 : workSheet.Cells[i, 2].Value.ToString().ToLower() == "individual" ? 1 : 2,
                                    CustomerName = workSheet.Cells[i, 3].Value == null ? string.Empty : workSheet.Cells[i, 3].Value.ToString(),
                                    LoanReferenceNumber = workSheet.Cells[i, 4].Value == null ? string.Empty : workSheet.Cells[i, 4].Value.ToString(),
                                    ProductCode = workSheet.Cells[i, 5].Value == null ? string.Empty : workSheet.Cells[i, 5].Value.ToString(),
                                    ProductName = workSheet.Cells[i, 6].Value == null ? string.Empty : workSheet.Cells[i, 6].Value.ToString(),
                                    EffectiveDate = workSheet.Cells[i, 7].Value == null ? DateTime.Now : DateTime.Parse(workSheet.Cells[i, 7].Value.ToString()),
                                    MaturityDate = workSheet.Cells[i, 8].Value == null ? DateTime.Now : DateTime.Parse(workSheet.Cells[i, 8].Value.ToString()),
                                    Field1 = workSheet.Cells[i, 9].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 9].Value.ToString()),
                                    Field2 = workSheet.Cells[i, 10].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 10].Value.ToString()),
                                    Field3 = workSheet.Cells[i, 11].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 11].Value.ToString()),
                                    Field4 = workSheet.Cells[i, 12].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 12].Value.ToString()),
                                    Field5 = workSheet.Cells[i, 13].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 13].Value.ToString()),
                                    Field6 = workSheet.Cells[i, 14].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 14].Value.ToString()),
                                    Field7 = workSheet.Cells[i, 15].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 15].Value.ToString()),
                                    Field8 = workSheet.Cells[i, 16].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 16].Value.ToString()),
                                    Field9 = workSheet.Cells[i, 17].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 17].Value.ToString()),
                                    Field10 = workSheet.Cells[i, 18].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 18].Value.ToString()),
                                    Field11 = workSheet.Cells[i, 19].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 19].Value.ToString()),
                                    Field12 = workSheet.Cells[i, 20].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 20].Value.ToString()),
                                    Field13 = workSheet.Cells[i, 21].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 21].Value.ToString()),
                                    Field14 = workSheet.Cells[i, 22].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 22].Value.ToString()),
                                    Field15 = workSheet.Cells[i, 23].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 23].Value.ToString()),
                                    Field16 = workSheet.Cells[i, 24].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 24].Value.ToString()),
                                    Field17 = workSheet.Cells[i, 25].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 25].Value.ToString()),
                                    Field18 = workSheet.Cells[i, 26].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 26].Value.ToString()),
                                    Field19 = workSheet.Cells[i, 27].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 27].Value.ToString()),
                                    Field20 = workSheet.Cells[i, 28].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 28].Value.ToString()),
                                    Field21 = workSheet.Cells[i, 29].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 29].Value.ToString()),
                                    Field22 = workSheet.Cells[i, 30].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 30].Value.ToString()),
                                    Field23 = workSheet.Cells[i, 31].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 31].Value.ToString()),
                                    Field24 = workSheet.Cells[i, 32].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 32].Value.ToString()),
                                    Field25 = workSheet.Cells[i, 33].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 33].Value.ToString()),
                                    Field26 = workSheet.Cells[i, 34].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 34].Value.ToString()),
                                    Field27 = workSheet.Cells[i, 35].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 35].Value.ToString()),
                                    Field28 = workSheet.Cells[i, 36].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 36].Value.ToString()),
                                    Field29 = workSheet.Cells[i, 37].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 37].Value.ToString()),
                                    Field30 = workSheet.Cells[i, 38].Value == null ? 1000000 : decimal.Parse(workSheet.Cells[i, 38].Value.ToString()),
                                };
                                if(item.ProductCode != "")
                                {
                                    uploadedRecord.Add(item);
                                }                              
                            }
                        }
                    }
                }
                List<credit_individualapplicationscorecard_history> histories = new List<credit_individualapplicationscorecard_history>();
                credit_individualapplicationscorecard_history history = null;
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var productObj = _dataContext.credit_product.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                        if (productObj == null)
                        {
                            throw new Exception("Product with product code: " + item.ProductCode + " not found");
                        }
                        history = _dataContext.credit_individualapplicationscorecard_history.Where(x => x.LoanReferenceNumber == item.LoanReferenceNumber).FirstOrDefault();

                        if (history != null)
                        {
                            history.LoanAmount = item.LoanAmount;
                            history.OutstandingBalance = 0;
                            history.CustomerTypeId = item.CustomerTypeId;
                            history.CustomerName = item.CustomerName;
                            history.LoanReferenceNumber = item.LoanReferenceNumber;
                            history.ProductCode = item.ProductCode;
                            history.ProductName = productObj.ProductName;
                            history.EffectiveDate = item.EffectiveDate;
                            history.MaturityDate = item.MaturityDate;
                            history.Field1 = item.Field1 == 1000000 ? null : item.Field1;
                            history.Field2 = item.Field2 == 1000000 ? null : item.Field2;
                            history.Field3 = item.Field3 == 1000000 ? null : item.Field3;
                            history.Field4 = item.Field4 == 1000000 ? null : item.Field4;
                            history.Field5 = item.Field5 == 1000000 ? null : item.Field5;
                            history.Field6 = item.Field6 == 1000000 ? null : item.Field6;
                            history.Field7 = item.Field7 == 1000000 ? null : item.Field7;
                            history.Field8 = item.Field8 == 1000000 ? null : item.Field8;
                            history.Field9 = item.Field9 == 1000000 ? null : item.Field9;
                            history.Field10 = item.Field10 == 1000000 ? null : item.Field10;
                            history.Field11 = item.Field11 == 1000000 ? null : item.Field11;
                            history.Field12 = item.Field12 == 1000000 ? null : item.Field12;
                            history.Field13 = item.Field13 == 1000000 ? null : item.Field13;
                            history.Field14 = item.Field14 == 1000000 ? null : item.Field14;
                            history.Field15 = item.Field15 == 1000000 ? null : item.Field15;
                            history.Field16 = item.Field16 == 1000000 ? null : item.Field16;
                            history.Field17 = item.Field17 == 1000000 ? null : item.Field17;
                            history.Field18 = item.Field18 == 1000000 ? null : item.Field18;
                            history.Field19 = item.Field19 == 1000000 ? null : item.Field19;
                            history.Field20 = item.Field20 == 1000000 ? null : item.Field20;
                            history.Field21 = item.Field21 == 1000000 ? null : item.Field21;
                            history.Field22 = item.Field22 == 1000000 ? null : item.Field22;
                            history.Field23 = item.Field23 == 1000000 ? null : item.Field23;
                            history.Field24 = item.Field24 == 1000000 ? null : item.Field24;
                            history.Field25 = item.Field25 == 1000000 ? null : item.Field25;
                            history.Field26 = item.Field26 == 1000000 ? null : item.Field26;
                            history.Field27 = item.Field27 == 1000000 ? null : item.Field27;
                            history.Field28 = item.Field28 == 1000000 ? null : item.Field28;
                            history.Field29 = item.Field29 == 1000000 ? null : item.Field29;
                            history.Field30 = item.Field30 == 1000000 ? null : item.Field30;
                            history.Active = true;
                            history.Deleted = false;
                            history.UpdatedBy = createdBy;
                            history.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            history = new credit_individualapplicationscorecard_history
                            {
                                LoanAmount = item.LoanAmount,
                                OutstandingBalance = 0,
                                CustomerTypeId = item.CustomerTypeId,
                                CustomerName = item.CustomerName,
                                LoanReferenceNumber = item.LoanReferenceNumber,
                                ProductCode = item.ProductCode,
                                ProductName = productObj.ProductName,
                                EffectiveDate = item.EffectiveDate,
                                MaturityDate = item.MaturityDate,
                                Field1 = item.Field1 == 1000000 ? null : item.Field1,
                                Field2 = item.Field2 == 1000000 ? null : item.Field2,
                                Field3 = item.Field3 == 1000000 ? null : item.Field3,
                                Field4 = item.Field4 == 1000000 ? null : item.Field4,
                                Field5 = item.Field5 == 1000000 ? null : item.Field5,
                                Field6 = item.Field6 == 1000000 ? null : item.Field6,
                                Field7 = item.Field7 == 1000000 ? null : item.Field7,
                                Field8 = item.Field8 == 1000000 ? null : item.Field8,
                                Field9 = item.Field9 == 1000000 ? null : item.Field9,
                                Field10 = item.Field10 == 1000000 ? null : item.Field10,
                                Field11 = item.Field11 == 1000000 ? null : item.Field11,
                                Field12 = item.Field12 == 1000000 ? null : item.Field12,
                                Field13 = item.Field13 == 1000000 ? null : item.Field13,
                                Field14 = item.Field14 == 1000000 ? null : item.Field14,
                                Field15 = item.Field15 == 1000000 ? null : item.Field15,
                                Field16 = item.Field16 == 1000000 ? null : item.Field16,
                                Field17 = item.Field17 == 1000000 ? null : item.Field17,
                                Field18 = item.Field18 == 1000000 ? null : item.Field18,
                                Field19 = item.Field19 == 1000000 ? null : item.Field19,
                                Field20 = item.Field20 == 1000000 ? null : item.Field20,
                                Field21 = item.Field21 == 1000000 ? null : item.Field21,
                                Field22 = item.Field22 == 1000000 ? null : item.Field22,
                                Field23 = item.Field23 == 1000000 ? null : item.Field23,
                                Field24 = item.Field24 == 1000000 ? null : item.Field24,
                                Field25 = item.Field25 == 1000000 ? null : item.Field25,
                                Field26 = item.Field26 == 1000000 ? null : item.Field26,
                                Field27 = item.Field27 == 1000000 ? null : item.Field27,
                                Field28 = item.Field28 == 1000000 ? null : item.Field28,
                                Field29 = item.Field29 == 1000000 ? null : item.Field29,
                                Field30 = item.Field30 == 1000000 ? null : item.Field30,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            histories.Add(history);
                        }
                    }
                    _dataContext.credit_individualapplicationscorecard_history.AddRange(histories);
                    using (var trans = _dataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            response = _dataContext.SaveChanges() > 0;
                            AddScoreCardHistoryDetail(uploadedRecord);
                            AddIndividualApplicationScoreCard(uploadedRecord, createdBy);

                            if (response)
                            {
                                trans.Commit();
                            }
                            return response;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidateYearExist(int year)
        {
            try
            {
                return _dataContext.ifrs_macroeconomic_variables_year.Where(x => x.Year == year).Any();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //////PRIVATE METHODS
        private void AddScoreCardHistoryDetail(List<credit_individualapplicationscorecard_history> inputTable)
        {
            List<credit_individualapplicationscorecarddetails_history> outputTable = new List<credit_individualapplicationscorecarddetails_history>();

            foreach (var item in inputTable)
            {
                var history = _dataContext.credit_individualapplicationscorecarddetails_history.Where(x => x.LoanReferenceNumber == item.LoanReferenceNumber).ToList();
                if (history.Count > 0)
                {
                    _dataContext.credit_individualapplicationscorecarddetails_history.RemoveRange(history);
                }
                var newRows = new List<credit_individualapplicationscorecarddetails_history>();
                int nextFieldCount = 1;

                if (item.Field1 != null && item.Field1 != 1000000)
                {
                    var newRow = new credit_individualapplicationscorecarddetails_history();
                    newRow.LoanAmount = item.LoanAmount;
                    newRow.CustomerName = item.CustomerName;
                    newRow.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow.ProductCode = item.ProductCode;
                    newRow.ProductName = item.ProductName;
                    newRow.EffectiveDate = item.EffectiveDate;
                    newRow.MaturityDate = item.MaturityDate;
                    newRow.Active = true;
                    newRow.Deleted = false;
                    newRow.CreatedBy = item.CreatedBy;
                    newRow.CreatedOn = DateTime.Now;
                    newRow.UpdatedBy = item.CreatedBy;
                    newRow.UpdatedOn = DateTime.Now;
                    newRow.AttributeField = "Field" + nextFieldCount;
                    newRow.Score = (decimal)item.Field1;
                    newRows.Add(newRow);
                    nextFieldCount++;
                }
                if (item.Field2 != null && item.Field2 != 1000000)
                {
                    var newRow2 = new credit_individualapplicationscorecarddetails_history();
                    newRow2.LoanAmount = item.LoanAmount;
                    newRow2.CustomerName = item.CustomerName;
                    newRow2.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow2.ProductCode = item.ProductCode;
                    newRow2.ProductName = item.ProductName;
                    newRow2.EffectiveDate = item.EffectiveDate;
                    newRow2.MaturityDate = item.MaturityDate;
                    newRow2.Active = true;
                    newRow2.Deleted = false;
                    newRow2.CreatedBy = item.CreatedBy;
                    newRow2.CreatedOn = DateTime.Now;
                    newRow2.UpdatedBy = item.CreatedBy;
                    newRow2.UpdatedOn = DateTime.Now;
                    newRow2.AttributeField = "Field" + nextFieldCount;
                    newRow2.Score = (decimal)item.Field2;
                    newRows.Add(newRow2);
                    nextFieldCount++;
                }
                if (item.Field3 != null && item.Field3 != 1000000)
                {
                    var newRow3 = new credit_individualapplicationscorecarddetails_history();
                    newRow3.LoanAmount = item.LoanAmount;
                    newRow3.CustomerName = item.CustomerName;
                    newRow3.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow3.ProductCode = item.ProductCode;
                    newRow3.ProductName = item.ProductName;
                    newRow3.EffectiveDate = item.EffectiveDate;
                    newRow3.MaturityDate = item.MaturityDate;
                    newRow3.Active = true;
                    newRow3.Deleted = false;
                    newRow3.CreatedBy = item.CreatedBy;
                    newRow3.CreatedOn = DateTime.Now;
                    newRow3.UpdatedBy = item.CreatedBy;
                    newRow3.UpdatedOn = DateTime.Now;
                    newRow3.AttributeField = "Field" + nextFieldCount;
                    newRow3.Score = (decimal)item.Field3;
                    newRows.Add(newRow3);
                    nextFieldCount++;
                }
                if (item.Field4 != null && item.Field4 != 1000000)
                {
                    var newRow4 = new credit_individualapplicationscorecarddetails_history();
                    newRow4.LoanAmount = item.LoanAmount;
                    newRow4.CustomerName = item.CustomerName;
                    newRow4.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow4.ProductCode = item.ProductCode;
                    newRow4.ProductName = item.ProductName;
                    newRow4.EffectiveDate = item.EffectiveDate;
                    newRow4.MaturityDate = item.MaturityDate;
                    newRow4.Active = true;
                    newRow4.Deleted = false;
                    newRow4.CreatedBy = item.CreatedBy;
                    newRow4.CreatedOn = DateTime.Now;
                    newRow4.UpdatedBy = item.CreatedBy;
                    newRow4.UpdatedOn = DateTime.Now;
                    newRow4.AttributeField = "Field" + nextFieldCount;
                    newRow4.Score = (decimal)item.Field4;
                    newRows.Add(newRow4);
                    nextFieldCount++;
                }
                if (item.Field5 != null && item.Field5 != 1000000)
                {
                    var newRow5 = new credit_individualapplicationscorecarddetails_history();
                    newRow5.LoanAmount = item.LoanAmount;
                    newRow5.CustomerName = item.CustomerName;
                    newRow5.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow5.ProductCode = item.ProductCode;
                    newRow5.ProductName = item.ProductName;
                    newRow5.EffectiveDate = item.EffectiveDate;
                    newRow5.MaturityDate = item.MaturityDate;
                    newRow5.Active = true;
                    newRow5.Deleted = false;
                    newRow5.CreatedBy = item.CreatedBy;
                    newRow5.CreatedOn = DateTime.Now;
                    newRow5.UpdatedBy = item.CreatedBy;
                    newRow5.UpdatedOn = DateTime.Now;
                    newRow5.AttributeField = "Field" + nextFieldCount;
                    newRow5.Score = (decimal)item.Field5;
                    newRows.Add(newRow5);
                    nextFieldCount++;
                }
                if (item.Field6 != null && item.Field6 != 1000000)
                {
                    var newRow6 = new credit_individualapplicationscorecarddetails_history();
                    newRow6.LoanAmount = item.LoanAmount;
                    newRow6.CustomerName = item.CustomerName;
                    newRow6.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow6.ProductCode = item.ProductCode;
                    newRow6.ProductName = item.ProductName;
                    newRow6.EffectiveDate = item.EffectiveDate;
                    newRow6.MaturityDate = item.MaturityDate;
                    newRow6.Active = true;
                    newRow6.Deleted = false;
                    newRow6.CreatedBy = item.CreatedBy;
                    newRow6.CreatedOn = DateTime.Now;
                    newRow6.UpdatedBy = item.CreatedBy;
                    newRow6.UpdatedOn = DateTime.Now;
                    newRow6.AttributeField = "Field" + nextFieldCount;
                    newRow6.Score = (decimal)item.Field6;
                    newRows.Add(newRow6);
                    nextFieldCount++;
                }
                if (item.Field7 != null && item.Field7 != 1000000)
                {
                    var newRow7 = new credit_individualapplicationscorecarddetails_history();
                    newRow7.LoanAmount = item.LoanAmount;
                    newRow7.CustomerName = item.CustomerName;
                    newRow7.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow7.ProductCode = item.ProductCode;
                    newRow7.ProductName = item.ProductName;
                    newRow7.EffectiveDate = item.EffectiveDate;
                    newRow7.MaturityDate = item.MaturityDate;
                    newRow7.Active = true;
                    newRow7.Deleted = false;
                    newRow7.CreatedBy = item.CreatedBy;
                    newRow7.CreatedOn = DateTime.Now;
                    newRow7.UpdatedBy = item.CreatedBy;
                    newRow7.UpdatedOn = DateTime.Now;
                    newRow7.AttributeField = "Field" + nextFieldCount;
                    newRow7.Score = (decimal)item.Field7;
                    newRows.Add(newRow7);
                    nextFieldCount++;
                }
                if (item.Field8 != null && item.Field8 != 1000000)
                {
                    var newRow8 = new credit_individualapplicationscorecarddetails_history();
                    newRow8.LoanAmount = item.LoanAmount;
                    newRow8.CustomerName = item.CustomerName;
                    newRow8.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow8.ProductCode = item.ProductCode;
                    newRow8.ProductName = item.ProductName;
                    newRow8.EffectiveDate = item.EffectiveDate;
                    newRow8.MaturityDate = item.MaturityDate;
                    newRow8.Active = true;
                    newRow8.Deleted = false;
                    newRow8.CreatedBy = item.CreatedBy;
                    newRow8.CreatedOn = DateTime.Now;
                    newRow8.UpdatedBy = item.CreatedBy;
                    newRow8.UpdatedOn = DateTime.Now;
                    newRow8.AttributeField = "Field" + nextFieldCount;
                    newRow8.Score = (decimal)item.Field8;
                    newRows.Add(newRow8);
                    nextFieldCount++;
                }
                if (item.Field9 != null && item.Field9 != 1000000)
                {
                    var newRow9 = new credit_individualapplicationscorecarddetails_history();
                    newRow9.LoanAmount = item.LoanAmount;
                    newRow9.CustomerName = item.CustomerName;
                    newRow9.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow9.ProductCode = item.ProductCode;
                    newRow9.ProductName = item.ProductName;
                    newRow9.EffectiveDate = item.EffectiveDate;
                    newRow9.MaturityDate = item.MaturityDate;
                    newRow9.Active = true;
                    newRow9.Deleted = false;
                    newRow9.CreatedBy = item.CreatedBy;
                    newRow9.CreatedOn = DateTime.Now;
                    newRow9.UpdatedBy = item.CreatedBy;
                    newRow9.UpdatedOn = DateTime.Now;
                    newRow9.AttributeField = "Field" + nextFieldCount;
                    newRow9.Score = (decimal)item.Field9;
                    newRows.Add(newRow9);
                    nextFieldCount++;
                }
                if (item.Field10 != null && item.Field10 != 1000000)
                {
                    var newRow10 = new credit_individualapplicationscorecarddetails_history();
                    newRow10.LoanAmount = item.LoanAmount;
                    newRow10.CustomerName = item.CustomerName;
                    newRow10.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow10.ProductCode = item.ProductCode;
                    newRow10.ProductName = item.ProductName;
                    newRow10.EffectiveDate = item.EffectiveDate;
                    newRow10.MaturityDate = item.MaturityDate;
                    newRow10.Active = true;
                    newRow10.Deleted = false;
                    newRow10.CreatedBy = item.CreatedBy;
                    newRow10.CreatedOn = DateTime.Now;
                    newRow10.UpdatedBy = item.CreatedBy;
                    newRow10.UpdatedOn = DateTime.Now;
                    newRow10.AttributeField = "Field" + nextFieldCount;
                    newRow10.Score = (decimal)item.Field10;
                    newRows.Add(newRow10);
                    nextFieldCount++;
                }
                if (item.Field11 != null && item.Field11 != 1000000)
                {
                    var newRow11 = new credit_individualapplicationscorecarddetails_history();
                    newRow11.LoanAmount = item.LoanAmount;
                    newRow11.CustomerName = item.CustomerName;
                    newRow11.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow11.ProductCode = item.ProductCode;
                    newRow11.ProductName = item.ProductName;
                    newRow11.EffectiveDate = item.EffectiveDate;
                    newRow11.MaturityDate = item.MaturityDate;
                    newRow11.Active = true;
                    newRow11.Deleted = false;
                    newRow11.CreatedBy = item.CreatedBy;
                    newRow11.CreatedOn = DateTime.Now;
                    newRow11.UpdatedBy = item.CreatedBy;
                    newRow11.UpdatedOn = DateTime.Now;
                    newRow11.AttributeField = "Field" + nextFieldCount;
                    newRow11.Score = (decimal)item.Field11;
                    newRows.Add(newRow11);
                    nextFieldCount++;
                }
                if (item.Field12 != null && item.Field12 != 1000000)
                {
                    var newRow12 = new credit_individualapplicationscorecarddetails_history();
                    newRow12.LoanAmount = item.LoanAmount;
                    newRow12.CustomerName = item.CustomerName;
                    newRow12.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow12.ProductCode = item.ProductCode;
                    newRow12.ProductName = item.ProductName;
                    newRow12.EffectiveDate = item.EffectiveDate;
                    newRow12.MaturityDate = item.MaturityDate;
                    newRow12.Active = true;
                    newRow12.Deleted = false;
                    newRow12.CreatedBy = item.CreatedBy;
                    newRow12.CreatedOn = DateTime.Now;
                    newRow12.UpdatedBy = item.CreatedBy;
                    newRow12.UpdatedOn = DateTime.Now;
                    newRow12.AttributeField = "Field" + nextFieldCount;
                    newRow12.Score = (decimal)item.Field12;
                    newRows.Add(newRow12);
                    nextFieldCount++;
                }
                if (item.Field13 != null && item.Field13 != 1000000)
                {
                    var newRow13 = new credit_individualapplicationscorecarddetails_history();
                    newRow13.LoanAmount = item.LoanAmount;
                    newRow13.CustomerName = item.CustomerName;
                    newRow13.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow13.ProductCode = item.ProductCode;
                    newRow13.ProductName = item.ProductName;
                    newRow13.EffectiveDate = item.EffectiveDate;
                    newRow13.MaturityDate = item.MaturityDate;
                    newRow13.Active = true;
                    newRow13.Deleted = false;
                    newRow13.CreatedBy = item.CreatedBy;
                    newRow13.CreatedOn = DateTime.Now;
                    newRow13.UpdatedBy = item.CreatedBy;
                    newRow13.UpdatedOn = DateTime.Now;
                    newRow13.AttributeField = "Field" + nextFieldCount;
                    newRow13.Score = (decimal)item.Field13;
                    newRows.Add(newRow13);
                    nextFieldCount++;
                }
                if (item.Field14 != null && item.Field14 != 1000000)
                {
                    var newRow14 = new credit_individualapplicationscorecarddetails_history();
                    newRow14.LoanAmount = item.LoanAmount;
                    newRow14.CustomerName = item.CustomerName;
                    newRow14.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow14.ProductCode = item.ProductCode;
                    newRow14.ProductName = item.ProductName;
                    newRow14.EffectiveDate = item.EffectiveDate;
                    newRow14.MaturityDate = item.MaturityDate;
                    newRow14.Active = true;
                    newRow14.Deleted = false;
                    newRow14.CreatedBy = item.CreatedBy;
                    newRow14.CreatedOn = DateTime.Now;
                    newRow14.UpdatedBy = item.CreatedBy;
                    newRow14.UpdatedOn = DateTime.Now;
                    newRow14.AttributeField = "Field" + nextFieldCount;
                    newRow14.Score = (decimal)item.Field14;
                    newRows.Add(newRow14);
                    nextFieldCount++;
                }
                if (item.Field15 != null && item.Field15 != 1000000)
                {
                    var newRow15 = new credit_individualapplicationscorecarddetails_history();
                    newRow15.LoanAmount = item.LoanAmount;
                    newRow15.CustomerName = item.CustomerName;
                    newRow15.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow15.ProductCode = item.ProductCode;
                    newRow15.ProductName = item.ProductName;
                    newRow15.EffectiveDate = item.EffectiveDate;
                    newRow15.MaturityDate = item.MaturityDate;
                    newRow15.Active = true;
                    newRow15.Deleted = false;
                    newRow15.CreatedBy = item.CreatedBy;
                    newRow15.CreatedOn = DateTime.Now;
                    newRow15.UpdatedBy = item.CreatedBy;
                    newRow15.UpdatedOn = DateTime.Now;
                    newRow15.AttributeField = "Field" + nextFieldCount;
                    newRow15.Score = (decimal)item.Field15;
                    newRows.Add(newRow15);
                    nextFieldCount++;
                }
                if (item.Field16 != null && item.Field16 != 1000000)
                {
                    var newRow16 = new credit_individualapplicationscorecarddetails_history();
                    newRow16.LoanAmount = item.LoanAmount;
                    newRow16.CustomerName = item.CustomerName;
                    newRow16.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow16.ProductCode = item.ProductCode;
                    newRow16.ProductName = item.ProductName;
                    newRow16.EffectiveDate = item.EffectiveDate;
                    newRow16.MaturityDate = item.MaturityDate;
                    newRow16.Active = true;
                    newRow16.Deleted = false;
                    newRow16.CreatedBy = item.CreatedBy;
                    newRow16.CreatedOn = DateTime.Now;
                    newRow16.UpdatedBy = item.CreatedBy;
                    newRow16.UpdatedOn = DateTime.Now;
                    newRow16.AttributeField = "Field" + nextFieldCount;
                    newRow16.Score = (decimal)item.Field16;
                    newRows.Add(newRow16);
                    nextFieldCount++;
                }
                if (item.Field17 != null && item.Field17 != 1000000)
                {
                    var newRow17 = new credit_individualapplicationscorecarddetails_history();
                    newRow17.LoanAmount = item.LoanAmount;
                    newRow17.CustomerName = item.CustomerName;
                    newRow17.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow17.ProductCode = item.ProductCode;
                    newRow17.ProductName = item.ProductName;
                    newRow17.EffectiveDate = item.EffectiveDate;
                    newRow17.MaturityDate = item.MaturityDate;
                    newRow17.Active = true;
                    newRow17.Deleted = false;
                    newRow17.CreatedBy = item.CreatedBy;
                    newRow17.CreatedOn = DateTime.Now;
                    newRow17.UpdatedBy = item.CreatedBy;
                    newRow17.UpdatedOn = DateTime.Now;
                    newRow17.AttributeField = "Field" + nextFieldCount;
                    newRow17.Score = (decimal)item.Field17;
                    newRows.Add(newRow17);
                    nextFieldCount++;
                }
                if (item.Field18 != null && item.Field18 != 1000000)
                {
                    var newRow18 = new credit_individualapplicationscorecarddetails_history();
                    newRow18.LoanAmount = item.LoanAmount;
                    newRow18.CustomerName = item.CustomerName;
                    newRow18.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow18.ProductCode = item.ProductCode;
                    newRow18.ProductName = item.ProductName;
                    newRow18.EffectiveDate = item.EffectiveDate;
                    newRow18.MaturityDate = item.MaturityDate;
                    newRow18.Active = true;
                    newRow18.Deleted = false;
                    newRow18.CreatedBy = item.CreatedBy;
                    newRow18.CreatedOn = DateTime.Now;
                    newRow18.UpdatedBy = item.CreatedBy;
                    newRow18.UpdatedOn = DateTime.Now;
                    newRow18.AttributeField = "Field" + nextFieldCount;
                    newRow18.Score = (decimal)item.Field18;
                    newRows.Add(newRow18);
                    nextFieldCount++;
                }
                if (item.Field19 != null && item.Field19 != 1000000)
                {
                    var newRow19 = new credit_individualapplicationscorecarddetails_history();
                    newRow19.LoanAmount = item.LoanAmount;
                    newRow19.CustomerName = item.CustomerName;
                    newRow19.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow19.ProductCode = item.ProductCode;
                    newRow19.ProductName = item.ProductName;
                    newRow19.EffectiveDate = item.EffectiveDate;
                    newRow19.MaturityDate = item.MaturityDate;
                    newRow19.Active = true;
                    newRow19.Deleted = false;
                    newRow19.CreatedBy = item.CreatedBy;
                    newRow19.CreatedOn = DateTime.Now;
                    newRow19.UpdatedBy = item.CreatedBy;
                    newRow19.UpdatedOn = DateTime.Now;
                    newRow19.AttributeField = "Field" + nextFieldCount;
                    newRow19.Score = (decimal)item.Field19;
                    newRows.Add(newRow19);
                    nextFieldCount++;
                }
                if (item.Field20 != null && item.Field20 != 1000000)
                {
                    var newRow20 = new credit_individualapplicationscorecarddetails_history();
                    newRow20.LoanAmount = item.LoanAmount;
                    newRow20.CustomerName = item.CustomerName;
                    newRow20.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow20.ProductCode = item.ProductCode;
                    newRow20.ProductName = item.ProductName;
                    newRow20.EffectiveDate = item.EffectiveDate;
                    newRow20.MaturityDate = item.MaturityDate;
                    newRow20.Active = true;
                    newRow20.Deleted = false;
                    newRow20.CreatedBy = item.CreatedBy;
                    newRow20.CreatedOn = DateTime.Now;
                    newRow20.UpdatedBy = item.CreatedBy;
                    newRow20.UpdatedOn = DateTime.Now;
                    newRow20.AttributeField = "Field" + nextFieldCount;
                    newRow20.Score = (decimal)item.Field20;
                    newRows.Add(newRow20);
                    nextFieldCount++;
                }
                if (item.Field21 != null && item.Field21 != 1000000)
                {
                    var newRow21 = new credit_individualapplicationscorecarddetails_history();
                    newRow21.LoanAmount = item.LoanAmount;
                    newRow21.CustomerName = item.CustomerName;
                    newRow21.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow21.ProductCode = item.ProductCode;
                    newRow21.ProductName = item.ProductName;
                    newRow21.EffectiveDate = item.EffectiveDate;
                    newRow21.MaturityDate = item.MaturityDate;
                    newRow21.Active = true;
                    newRow21.Deleted = false;
                    newRow21.CreatedBy = item.CreatedBy;
                    newRow21.CreatedOn = DateTime.Now;
                    newRow21.UpdatedBy = item.CreatedBy;
                    newRow21.UpdatedOn = DateTime.Now;
                    newRow21.AttributeField = "Field" + nextFieldCount;
                    newRow21.Score = (decimal)item.Field21;
                    newRows.Add(newRow21);
                    nextFieldCount++;
                }
                if (item.Field22 != null && item.Field22 != 1000000)
                {
                    var newRow22 = new credit_individualapplicationscorecarddetails_history();
                    newRow22.LoanAmount = item.LoanAmount;
                    newRow22.CustomerName = item.CustomerName;
                    newRow22.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow22.ProductCode = item.ProductCode;
                    newRow22.ProductName = item.ProductName;
                    newRow22.EffectiveDate = item.EffectiveDate;
                    newRow22.MaturityDate = item.MaturityDate;
                    newRow22.Active = true;
                    newRow22.Deleted = false;
                    newRow22.CreatedBy = item.CreatedBy;
                    newRow22.CreatedOn = DateTime.Now;
                    newRow22.UpdatedBy = item.CreatedBy;
                    newRow22.UpdatedOn = DateTime.Now;
                    newRow22.AttributeField = "Field" + nextFieldCount;
                    newRow22.Score = (decimal)item.Field22;
                    newRows.Add(newRow22);
                    nextFieldCount++;
                }
                if (item.Field23 != null && item.Field23 != 1000000)
                {
                    var newRow23 = new credit_individualapplicationscorecarddetails_history();
                    newRow23.LoanAmount = item.LoanAmount;
                    newRow23.CustomerName = item.CustomerName;
                    newRow23.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow23.ProductCode = item.ProductCode;
                    newRow23.ProductName = item.ProductName;
                    newRow23.EffectiveDate = item.EffectiveDate;
                    newRow23.MaturityDate = item.MaturityDate;
                    newRow23.Active = true;
                    newRow23.Deleted = false;
                    newRow23.CreatedBy = item.CreatedBy;
                    newRow23.CreatedOn = DateTime.Now;
                    newRow23.UpdatedBy = item.CreatedBy;
                    newRow23.UpdatedOn = DateTime.Now;
                    newRow23.AttributeField = "Field" + nextFieldCount;
                    newRow23.Score = (decimal)item.Field23;
                    newRows.Add(newRow23);
                    nextFieldCount++;
                }
                if (item.Field24 != null && item.Field24 != 1000000)
                {
                    var newRow24 = new credit_individualapplicationscorecarddetails_history();
                    newRow24.LoanAmount = item.LoanAmount;
                    newRow24.CustomerName = item.CustomerName;
                    newRow24.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow24.ProductCode = item.ProductCode;
                    newRow24.ProductName = item.ProductName;
                    newRow24.EffectiveDate = item.EffectiveDate;
                    newRow24.MaturityDate = item.MaturityDate;
                    newRow24.Active = true;
                    newRow24.Deleted = false;
                    newRow24.CreatedBy = item.CreatedBy;
                    newRow24.CreatedOn = DateTime.Now;
                    newRow24.UpdatedBy = item.CreatedBy;
                    newRow24.UpdatedOn = DateTime.Now;
                    newRow24.AttributeField = "Field" + nextFieldCount;
                    newRow24.Score = (decimal)item.Field24;
                    newRows.Add(newRow24);
                    nextFieldCount++;
                }
                if (item.Field25 != null && item.Field25 != 1000000)
                {
                    var newRow25 = new credit_individualapplicationscorecarddetails_history();
                    newRow25.LoanAmount = item.LoanAmount;
                    newRow25.CustomerName = item.CustomerName;
                    newRow25.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow25.ProductCode = item.ProductCode;
                    newRow25.ProductName = item.ProductName;
                    newRow25.EffectiveDate = item.EffectiveDate;
                    newRow25.MaturityDate = item.MaturityDate;
                    newRow25.Active = true;
                    newRow25.Deleted = false;
                    newRow25.CreatedBy = item.CreatedBy;
                    newRow25.CreatedOn = DateTime.Now;
                    newRow25.UpdatedBy = item.CreatedBy;
                    newRow25.UpdatedOn = DateTime.Now;
                    newRow25.AttributeField = "Field" + nextFieldCount;
                    newRow25.Score = (decimal)item.Field25;
                    newRows.Add(newRow25);
                    nextFieldCount++;
                }
                if (item.Field26 != null && item.Field26 != 1000000)
                {
                    var newRow26 = new credit_individualapplicationscorecarddetails_history();
                    newRow26.LoanAmount = item.LoanAmount;
                    newRow26.CustomerName = item.CustomerName;
                    newRow26.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow26.ProductCode = item.ProductCode;
                    newRow26.ProductName = item.ProductName;
                    newRow26.EffectiveDate = item.EffectiveDate;
                    newRow26.MaturityDate = item.MaturityDate;
                    newRow26.Active = true;
                    newRow26.Deleted = false;
                    newRow26.CreatedBy = item.CreatedBy;
                    newRow26.CreatedOn = DateTime.Now;
                    newRow26.UpdatedBy = item.CreatedBy;
                    newRow26.UpdatedOn = DateTime.Now;
                    newRow26.AttributeField = "Field" + nextFieldCount;
                    newRow26.Score = (decimal)item.Field26;
                    newRows.Add(newRow26);
                    nextFieldCount++;
                }
                if (item.Field27 != null && item.Field27 != 1000000)
                {
                    var newRow27 = new credit_individualapplicationscorecarddetails_history();
                    newRow27.LoanAmount = item.LoanAmount;
                    newRow27.CustomerName = item.CustomerName;
                    newRow27.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow27.ProductCode = item.ProductCode;
                    newRow27.ProductName = item.ProductName;
                    newRow27.EffectiveDate = item.EffectiveDate;
                    newRow27.MaturityDate = item.MaturityDate;
                    newRow27.Active = true;
                    newRow27.Deleted = false;
                    newRow27.CreatedBy = item.CreatedBy;
                    newRow27.CreatedOn = DateTime.Now;
                    newRow27.UpdatedBy = item.CreatedBy;
                    newRow27.UpdatedOn = DateTime.Now;
                    newRow27.AttributeField = "Field" + nextFieldCount;
                    newRow27.Score = (decimal)item.Field27;
                    newRows.Add(newRow27);
                    nextFieldCount++;
                }
                if (item.Field28 != null && item.Field28 != 1000000)
                {
                    var newRow28 = new credit_individualapplicationscorecarddetails_history();
                    newRow28.LoanAmount = item.LoanAmount;
                    newRow28.CustomerName = item.CustomerName;
                    newRow28.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow28.ProductCode = item.ProductCode;
                    newRow28.ProductName = item.ProductName;
                    newRow28.EffectiveDate = item.EffectiveDate;
                    newRow28.MaturityDate = item.MaturityDate;
                    newRow28.Active = true;
                    newRow28.Deleted = false;
                    newRow28.CreatedBy = item.CreatedBy;
                    newRow28.CreatedOn = DateTime.Now;
                    newRow28.UpdatedBy = item.CreatedBy;
                    newRow28.UpdatedOn = DateTime.Now;
                    newRow28.AttributeField = "Field" + nextFieldCount;
                    newRow28.Score = (decimal)item.Field28;
                    newRows.Add(newRow28);
                    nextFieldCount++;
                }
                if (item.Field29 != null && item.Field29 != 1000000)
                {
                    var newRow29 = new credit_individualapplicationscorecarddetails_history();
                    newRow29.LoanAmount = item.LoanAmount;
                    newRow29.CustomerName = item.CustomerName;
                    newRow29.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow29.ProductCode = item.ProductCode;
                    newRow29.ProductName = item.ProductName;
                    newRow29.EffectiveDate = item.EffectiveDate;
                    newRow29.MaturityDate = item.MaturityDate;
                    newRow29.Active = true;
                    newRow29.Deleted = false;
                    newRow29.CreatedBy = item.CreatedBy;
                    newRow29.CreatedOn = DateTime.Now;
                    newRow29.UpdatedBy = item.CreatedBy;
                    newRow29.UpdatedOn = DateTime.Now;
                    newRow29.AttributeField = "Field" + nextFieldCount;
                    newRow29.Score = (decimal)item.Field29;
                    newRows.Add(newRow29);
                    nextFieldCount++;
                }
                if (item.Field30 != null && item.Field30 != 1000000)
                {
                    var newRow30 = new credit_individualapplicationscorecarddetails_history();
                    newRow30.LoanAmount = item.LoanAmount;
                    newRow30.CustomerName = item.CustomerName;
                    newRow30.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow30.ProductCode = item.ProductCode;
                    newRow30.ProductName = item.ProductName;
                    newRow30.EffectiveDate = item.EffectiveDate;
                    newRow30.MaturityDate = item.MaturityDate;
                    newRow30.Active = true;
                    newRow30.Deleted = false;
                    newRow30.CreatedBy = item.CreatedBy;
                    newRow30.CreatedOn = DateTime.Now;
                    newRow30.UpdatedBy = item.CreatedBy;
                    newRow30.UpdatedOn = DateTime.Now;
                    newRow30.AttributeField = "Field" + nextFieldCount;
                    newRow30.Score = (decimal)item.Field30;
                    newRows.Add(newRow30);
                    nextFieldCount++;
                }

                outputTable.AddRange(newRows);
            }

            _dataContext.credit_individualapplicationscorecarddetails_history.AddRange(outputTable);

            _dataContext.SaveChanges();

        }

        private void AddLGDHistoryInformationDetail(List<credit_lgd_historyinformation> inputTable)
        {
            List<credit_lgd_historyinformationdetails> outputTable = new List<credit_lgd_historyinformationdetails>();

            foreach (var item in inputTable)
            {
                var history = _dataContext.credit_lgd_historyinformationdetails.Where(x => x.LoanReferenceNumber == item.LoanReferenceNumber).ToList();
                if (history.Count > 0)
                {
                    _dataContext.credit_lgd_historyinformationdetails.RemoveRange(history);
                }
                var newRows = new List<credit_lgd_historyinformationdetails>();
                int nextFieldCount = 1;

                if (item.Field1 != null && item.Field1 != 1000000000000)
                {
                    var newRow = new credit_lgd_historyinformationdetails();
                    newRow.LoanAmount = item.LoanAmount;
                    newRow.CustomerName = item.CustomerName;
                    newRow.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow.ProductCode = item.ProductCode;
                    newRow.ProductName = item.ProductName;
                    newRow.EffectiveDate = item.EffectiveDate;
                    newRow.MaturityDate = item.MaturityDate;
                    newRow.Active = true;
                    newRow.Deleted = false;
                    newRow.CreatedBy = item.CreatedBy;
                    newRow.CreatedOn = DateTime.Now;
                    newRow.UpdatedBy = item.CreatedBy;
                    newRow.UpdatedOn = DateTime.Now;
                    newRow.AttributeField = "Field" + nextFieldCount;
                    newRow.Amount = (decimal)item.Field1;
                    newRows.Add(newRow);
                    nextFieldCount++;
                }
                if (item.Field2 != null && item.Field2 != 1000000000000)
                {
                    var newRow2 = new credit_lgd_historyinformationdetails();
                    newRow2.LoanAmount = item.LoanAmount;
                    newRow2.CustomerName = item.CustomerName;
                    newRow2.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow2.ProductCode = item.ProductCode;
                    newRow2.ProductName = item.ProductName;
                    newRow2.EffectiveDate = item.EffectiveDate;
                    newRow2.MaturityDate = item.MaturityDate;
                    newRow2.Active = true;
                    newRow2.Deleted = false;
                    newRow2.CreatedBy = item.CreatedBy;
                    newRow2.CreatedOn = DateTime.Now;
                    newRow2.UpdatedBy = item.CreatedBy;
                    newRow2.UpdatedOn = DateTime.Now;
                    newRow2.AttributeField = "Field" + nextFieldCount;
                    newRow2.Amount = (decimal)item.Field2;
                    newRows.Add(newRow2);
                    nextFieldCount++;
                }
                if (item.Field3 != null && item.Field3 != 1000000000000)
                {
                    var newRow3 = new credit_lgd_historyinformationdetails();
                    newRow3.LoanAmount = item.LoanAmount;
                    newRow3.CustomerName = item.CustomerName;
                    newRow3.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow3.ProductCode = item.ProductCode;
                    newRow3.ProductName = item.ProductName;
                    newRow3.EffectiveDate = item.EffectiveDate;
                    newRow3.MaturityDate = item.MaturityDate;
                    newRow3.Active = true;
                    newRow3.Deleted = false;
                    newRow3.CreatedBy = item.CreatedBy;
                    newRow3.CreatedOn = DateTime.Now;
                    newRow3.UpdatedBy = item.CreatedBy;
                    newRow3.UpdatedOn = DateTime.Now;
                    newRow3.AttributeField = "Field" + nextFieldCount;
                    newRow3.Amount = (decimal)item.Field3;
                    newRows.Add(newRow3);
                    nextFieldCount++;
                }
                if (item.Field4 != null && item.Field4 != 1000000000000)
                {
                    var newRow4 = new credit_lgd_historyinformationdetails();
                    newRow4.LoanAmount = item.LoanAmount;
                    newRow4.CustomerName = item.CustomerName;
                    newRow4.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow4.ProductCode = item.ProductCode;
                    newRow4.ProductName = item.ProductName;
                    newRow4.EffectiveDate = item.EffectiveDate;
                    newRow4.MaturityDate = item.MaturityDate;
                    newRow4.Active = true;
                    newRow4.Deleted = false;
                    newRow4.CreatedBy = item.CreatedBy;
                    newRow4.CreatedOn = DateTime.Now;
                    newRow4.UpdatedBy = item.CreatedBy;
                    newRow4.UpdatedOn = DateTime.Now;
                    newRow4.AttributeField = "Field" + nextFieldCount;
                    newRow4.Amount = (decimal)item.Field4;
                    newRows.Add(newRow4);
                    nextFieldCount++;
                }
                if (item.Field5 != null && item.Field5 != 1000000000000)
                {
                    var newRow5 = new credit_lgd_historyinformationdetails();
                    newRow5.LoanAmount = item.LoanAmount;
                    newRow5.CustomerName = item.CustomerName;
                    newRow5.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow5.ProductCode = item.ProductCode;
                    newRow5.ProductName = item.ProductName;
                    newRow5.EffectiveDate = item.EffectiveDate;
                    newRow5.MaturityDate = item.MaturityDate;
                    newRow5.Active = true;
                    newRow5.Deleted = false;
                    newRow5.CreatedBy = item.CreatedBy;
                    newRow5.CreatedOn = DateTime.Now;
                    newRow5.UpdatedBy = item.CreatedBy;
                    newRow5.UpdatedOn = DateTime.Now;
                    newRow5.AttributeField = "Field" + nextFieldCount;
                    newRow5.Amount = (decimal)item.Field5;
                    newRows.Add(newRow5);
                    nextFieldCount++;
                }
                if (item.Field6 != null && item.Field6 != 1000000000000)
                {
                    var newRow6 = new credit_lgd_historyinformationdetails();
                    newRow6.LoanAmount = item.LoanAmount;
                    newRow6.CustomerName = item.CustomerName;
                    newRow6.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow6.ProductCode = item.ProductCode;
                    newRow6.ProductName = item.ProductName;
                    newRow6.EffectiveDate = item.EffectiveDate;
                    newRow6.MaturityDate = item.MaturityDate;
                    newRow6.Active = true;
                    newRow6.Deleted = false;
                    newRow6.CreatedBy = item.CreatedBy;
                    newRow6.CreatedOn = DateTime.Now;
                    newRow6.UpdatedBy = item.CreatedBy;
                    newRow6.UpdatedOn = DateTime.Now;
                    newRow6.AttributeField = "Field" + nextFieldCount;
                    newRow6.Amount = (decimal)item.Field6;
                    newRows.Add(newRow6);
                    nextFieldCount++;
                }
                if (item.Field7 != null && item.Field7 != 1000000000000)
                {
                    var newRow7 = new credit_lgd_historyinformationdetails();
                    newRow7.LoanAmount = item.LoanAmount;
                    newRow7.CustomerName = item.CustomerName;
                    newRow7.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow7.ProductCode = item.ProductCode;
                    newRow7.ProductName = item.ProductName;
                    newRow7.EffectiveDate = item.EffectiveDate;
                    newRow7.MaturityDate = item.MaturityDate;
                    newRow7.Active = true;
                    newRow7.Deleted = false;
                    newRow7.CreatedBy = item.CreatedBy;
                    newRow7.CreatedOn = DateTime.Now;
                    newRow7.UpdatedBy = item.CreatedBy;
                    newRow7.UpdatedOn = DateTime.Now;
                    newRow7.AttributeField = "Field" + nextFieldCount;
                    newRow7.Amount = (decimal)item.Field7;
                    newRows.Add(newRow7);
                    nextFieldCount++;
                }
                if (item.Field8 != null && item.Field8 != 1000000000000)
                {
                    var newRow8 = new credit_lgd_historyinformationdetails();
                    newRow8.LoanAmount = item.LoanAmount;
                    newRow8.CustomerName = item.CustomerName;
                    newRow8.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow8.ProductCode = item.ProductCode;
                    newRow8.ProductName = item.ProductName;
                    newRow8.EffectiveDate = item.EffectiveDate;
                    newRow8.MaturityDate = item.MaturityDate;
                    newRow8.Active = true;
                    newRow8.Deleted = false;
                    newRow8.CreatedBy = item.CreatedBy;
                    newRow8.CreatedOn = DateTime.Now;
                    newRow8.UpdatedBy = item.CreatedBy;
                    newRow8.UpdatedOn = DateTime.Now;
                    newRow8.AttributeField = "Field" + nextFieldCount;
                    newRow8.Amount = (decimal)item.Field8;
                    newRows.Add(newRow8);
                    nextFieldCount++;
                }
                if (item.Field9 != null && item.Field9 != 1000000000000)
                {
                    var newRow9 = new credit_lgd_historyinformationdetails();
                    newRow9.LoanAmount = item.LoanAmount;
                    newRow9.CustomerName = item.CustomerName;
                    newRow9.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow9.ProductCode = item.ProductCode;
                    newRow9.ProductName = item.ProductName;
                    newRow9.EffectiveDate = item.EffectiveDate;
                    newRow9.MaturityDate = item.MaturityDate;
                    newRow9.Active = true;
                    newRow9.Deleted = false;
                    newRow9.CreatedBy = item.CreatedBy;
                    newRow9.CreatedOn = DateTime.Now;
                    newRow9.UpdatedBy = item.CreatedBy;
                    newRow9.UpdatedOn = DateTime.Now;
                    newRow9.AttributeField = "Field" + nextFieldCount;
                    newRow9.Amount = (decimal)item.Field9;
                    newRows.Add(newRow9);
                    nextFieldCount++;
                }
                if (item.Field10 != null && item.Field10 != 1000000000000)
                {
                    var newRow10 = new credit_lgd_historyinformationdetails();
                    newRow10.LoanAmount = item.LoanAmount;
                    newRow10.CustomerName = item.CustomerName;
                    newRow10.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow10.ProductCode = item.ProductCode;
                    newRow10.ProductName = item.ProductName;
                    newRow10.EffectiveDate = item.EffectiveDate;
                    newRow10.MaturityDate = item.MaturityDate;
                    newRow10.Active = true;
                    newRow10.Deleted = false;
                    newRow10.CreatedBy = item.CreatedBy;
                    newRow10.CreatedOn = DateTime.Now;
                    newRow10.UpdatedBy = item.CreatedBy;
                    newRow10.UpdatedOn = DateTime.Now;
                    newRow10.AttributeField = "Field" + nextFieldCount;
                    newRow10.Amount = (decimal)item.Field10;
                    newRows.Add(newRow10);
                    nextFieldCount++;
                }
                if (item.Field11 != null && item.Field11 != 1000000000000)
                {
                    var newRow11 = new credit_lgd_historyinformationdetails();
                    newRow11.LoanAmount = item.LoanAmount;
                    newRow11.CustomerName = item.CustomerName;
                    newRow11.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow11.ProductCode = item.ProductCode;
                    newRow11.ProductName = item.ProductName;
                    newRow11.EffectiveDate = item.EffectiveDate;
                    newRow11.MaturityDate = item.MaturityDate;
                    newRow11.Active = true;
                    newRow11.Deleted = false;
                    newRow11.CreatedBy = item.CreatedBy;
                    newRow11.CreatedOn = DateTime.Now;
                    newRow11.UpdatedBy = item.CreatedBy;
                    newRow11.UpdatedOn = DateTime.Now;
                    newRow11.AttributeField = "Field" + nextFieldCount;
                    newRow11.Amount = (decimal)item.Field11;
                    newRows.Add(newRow11);
                    nextFieldCount++;
                }
                if (item.Field12 != null && item.Field12 != 1000000000000)
                {
                    var newRow12 = new credit_lgd_historyinformationdetails();
                    newRow12.LoanAmount = item.LoanAmount;
                    newRow12.CustomerName = item.CustomerName;
                    newRow12.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow12.ProductCode = item.ProductCode;
                    newRow12.ProductName = item.ProductName;
                    newRow12.EffectiveDate = item.EffectiveDate;
                    newRow12.MaturityDate = item.MaturityDate;
                    newRow12.Active = true;
                    newRow12.Deleted = false;
                    newRow12.CreatedBy = item.CreatedBy;
                    newRow12.CreatedOn = DateTime.Now;
                    newRow12.UpdatedBy = item.CreatedBy;
                    newRow12.UpdatedOn = DateTime.Now;
                    newRow12.AttributeField = "Field" + nextFieldCount;
                    newRow12.Amount = (decimal)item.Field12;
                    newRows.Add(newRow12);
                    nextFieldCount++;
                }
                if (item.Field13 != null && item.Field13 != 1000000000000)
                {
                    var newRow13 = new credit_lgd_historyinformationdetails();
                    newRow13.LoanAmount = item.LoanAmount;
                    newRow13.CustomerName = item.CustomerName;
                    newRow13.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow13.ProductCode = item.ProductCode;
                    newRow13.ProductName = item.ProductName;
                    newRow13.EffectiveDate = item.EffectiveDate;
                    newRow13.MaturityDate = item.MaturityDate;
                    newRow13.Active = true;
                    newRow13.Deleted = false;
                    newRow13.CreatedBy = item.CreatedBy;
                    newRow13.CreatedOn = DateTime.Now;
                    newRow13.UpdatedBy = item.CreatedBy;
                    newRow13.UpdatedOn = DateTime.Now;
                    newRow13.AttributeField = "Field" + nextFieldCount;
                    newRow13.Amount = (decimal)item.Field13;
                    newRows.Add(newRow13);
                    nextFieldCount++;
                }
                if (item.Field14 != null && item.Field14 != 1000000000000)
                {
                    var newRow14 = new credit_lgd_historyinformationdetails();
                    newRow14.LoanAmount = item.LoanAmount;
                    newRow14.CustomerName = item.CustomerName;
                    newRow14.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow14.ProductCode = item.ProductCode;
                    newRow14.ProductName = item.ProductName;
                    newRow14.EffectiveDate = item.EffectiveDate;
                    newRow14.MaturityDate = item.MaturityDate;
                    newRow14.Active = true;
                    newRow14.Deleted = false;
                    newRow14.CreatedBy = item.CreatedBy;
                    newRow14.CreatedOn = DateTime.Now;
                    newRow14.UpdatedBy = item.CreatedBy;
                    newRow14.UpdatedOn = DateTime.Now;
                    newRow14.AttributeField = "Field" + nextFieldCount;
                    newRow14.Amount = (decimal)item.Field14;
                    newRows.Add(newRow14);
                    nextFieldCount++;
                }
                if (item.Field15 != null && item.Field15 != 1000000000000)
                {
                    var newRow15 = new credit_lgd_historyinformationdetails();
                    newRow15.LoanAmount = item.LoanAmount;
                    newRow15.CustomerName = item.CustomerName;
                    newRow15.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow15.ProductCode = item.ProductCode;
                    newRow15.ProductName = item.ProductName;
                    newRow15.EffectiveDate = item.EffectiveDate;
                    newRow15.MaturityDate = item.MaturityDate;
                    newRow15.Active = true;
                    newRow15.Deleted = false;
                    newRow15.CreatedBy = item.CreatedBy;
                    newRow15.CreatedOn = DateTime.Now;
                    newRow15.UpdatedBy = item.CreatedBy;
                    newRow15.UpdatedOn = DateTime.Now;
                    newRow15.AttributeField = "Field" + nextFieldCount;
                    newRow15.Amount = (decimal)item.Field15;
                    newRows.Add(newRow15);
                    nextFieldCount++;
                }
                if (item.Field16 != null && item.Field16 != 1000000000000)
                {
                    var newRow16 = new credit_lgd_historyinformationdetails();
                    newRow16.LoanAmount = item.LoanAmount;
                    newRow16.CustomerName = item.CustomerName;
                    newRow16.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow16.ProductCode = item.ProductCode;
                    newRow16.ProductName = item.ProductName;
                    newRow16.EffectiveDate = item.EffectiveDate;
                    newRow16.MaturityDate = item.MaturityDate;
                    newRow16.Active = true;
                    newRow16.Deleted = false;
                    newRow16.CreatedBy = item.CreatedBy;
                    newRow16.CreatedOn = DateTime.Now;
                    newRow16.UpdatedBy = item.CreatedBy;
                    newRow16.UpdatedOn = DateTime.Now;
                    newRow16.AttributeField = "Field" + nextFieldCount;
                    newRow16.Amount = (decimal)item.Field16;
                    newRows.Add(newRow16);
                    nextFieldCount++;
                }
                if (item.Field17 != null && item.Field17 != 1000000000000)
                {
                    var newRow17 = new credit_lgd_historyinformationdetails();
                    newRow17.LoanAmount = item.LoanAmount;
                    newRow17.CustomerName = item.CustomerName;
                    newRow17.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow17.ProductCode = item.ProductCode;
                    newRow17.ProductName = item.ProductName;
                    newRow17.EffectiveDate = item.EffectiveDate;
                    newRow17.MaturityDate = item.MaturityDate;
                    newRow17.Active = true;
                    newRow17.Deleted = false;
                    newRow17.CreatedBy = item.CreatedBy;
                    newRow17.CreatedOn = DateTime.Now;
                    newRow17.UpdatedBy = item.CreatedBy;
                    newRow17.UpdatedOn = DateTime.Now;
                    newRow17.AttributeField = "Field" + nextFieldCount;
                    newRow17.Amount = (decimal)item.Field17;
                    newRows.Add(newRow17);
                    nextFieldCount++;
                }
                if (item.Field18 != null && item.Field18 != 1000000000000)
                {
                    var newRow18 = new credit_lgd_historyinformationdetails();
                    newRow18.LoanAmount = item.LoanAmount;
                    newRow18.CustomerName = item.CustomerName;
                    newRow18.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow18.ProductCode = item.ProductCode;
                    newRow18.ProductName = item.ProductName;
                    newRow18.EffectiveDate = item.EffectiveDate;
                    newRow18.MaturityDate = item.MaturityDate;
                    newRow18.Active = true;
                    newRow18.Deleted = false;
                    newRow18.CreatedBy = item.CreatedBy;
                    newRow18.CreatedOn = DateTime.Now;
                    newRow18.UpdatedBy = item.CreatedBy;
                    newRow18.UpdatedOn = DateTime.Now;
                    newRow18.AttributeField = "Field" + nextFieldCount;
                    newRow18.Amount = (decimal)item.Field18;
                    newRows.Add(newRow18);
                    nextFieldCount++;
                }
                if (item.Field19 != null && item.Field19 != 1000000000000)
                {
                    var newRow19 = new credit_lgd_historyinformationdetails();
                    newRow19.LoanAmount = item.LoanAmount;
                    newRow19.CustomerName = item.CustomerName;
                    newRow19.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow19.ProductCode = item.ProductCode;
                    newRow19.ProductName = item.ProductName;
                    newRow19.EffectiveDate = item.EffectiveDate;
                    newRow19.MaturityDate = item.MaturityDate;
                    newRow19.Active = true;
                    newRow19.Deleted = false;
                    newRow19.CreatedBy = item.CreatedBy;
                    newRow19.CreatedOn = DateTime.Now;
                    newRow19.UpdatedBy = item.CreatedBy;
                    newRow19.UpdatedOn = DateTime.Now;
                    newRow19.AttributeField = "Field" + nextFieldCount;
                    newRow19.Amount = (decimal)item.Field19;
                    newRows.Add(newRow19);
                    nextFieldCount++;
                }
                if (item.Field20 != null && item.Field20 != 1000000000000)
                {
                    var newRow20 = new credit_lgd_historyinformationdetails();
                    newRow20.LoanAmount = item.LoanAmount;
                    newRow20.CustomerName = item.CustomerName;
                    newRow20.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow20.ProductCode = item.ProductCode;
                    newRow20.ProductName = item.ProductName;
                    newRow20.EffectiveDate = item.EffectiveDate;
                    newRow20.MaturityDate = item.MaturityDate;
                    newRow20.Active = true;
                    newRow20.Deleted = false;
                    newRow20.CreatedBy = item.CreatedBy;
                    newRow20.CreatedOn = DateTime.Now;
                    newRow20.UpdatedBy = item.CreatedBy;
                    newRow20.UpdatedOn = DateTime.Now;
                    newRow20.AttributeField = "Field" + nextFieldCount;
                    newRow20.Amount = (decimal)item.Field20;
                    newRows.Add(newRow20);
                    nextFieldCount++;
                }
                if (item.Field21 != null && item.Field21 != 1000000000000)
                {
                    var newRow21 = new credit_lgd_historyinformationdetails();
                    newRow21.LoanAmount = item.LoanAmount;
                    newRow21.CustomerName = item.CustomerName;
                    newRow21.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow21.ProductCode = item.ProductCode;
                    newRow21.ProductName = item.ProductName;
                    newRow21.EffectiveDate = item.EffectiveDate;
                    newRow21.MaturityDate = item.MaturityDate;
                    newRow21.Active = true;
                    newRow21.Deleted = false;
                    newRow21.CreatedBy = item.CreatedBy;
                    newRow21.CreatedOn = DateTime.Now;
                    newRow21.UpdatedBy = item.CreatedBy;
                    newRow21.UpdatedOn = DateTime.Now;
                    newRow21.AttributeField = "Field" + nextFieldCount;
                    newRow21.Amount = (decimal)item.Field21;
                    newRows.Add(newRow21);
                    nextFieldCount++;
                }
                if (item.Field22 != null && item.Field22 != 1000000000000)
                {
                    var newRow22 = new credit_lgd_historyinformationdetails();
                    newRow22.LoanAmount = item.LoanAmount;
                    newRow22.CustomerName = item.CustomerName;
                    newRow22.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow22.ProductCode = item.ProductCode;
                    newRow22.ProductName = item.ProductName;
                    newRow22.EffectiveDate = item.EffectiveDate;
                    newRow22.MaturityDate = item.MaturityDate;
                    newRow22.Active = true;
                    newRow22.Deleted = false;
                    newRow22.CreatedBy = item.CreatedBy;
                    newRow22.CreatedOn = DateTime.Now;
                    newRow22.UpdatedBy = item.CreatedBy;
                    newRow22.UpdatedOn = DateTime.Now;
                    newRow22.AttributeField = "Field" + nextFieldCount;
                    newRow22.Amount = (decimal)item.Field22;
                    newRows.Add(newRow22);
                    nextFieldCount++;
                }
                if (item.Field23 != null && item.Field23 != 1000000000000)
                {
                    var newRow23 = new credit_lgd_historyinformationdetails();
                    newRow23.LoanAmount = item.LoanAmount;
                    newRow23.CustomerName = item.CustomerName;
                    newRow23.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow23.ProductCode = item.ProductCode;
                    newRow23.ProductName = item.ProductName;
                    newRow23.EffectiveDate = item.EffectiveDate;
                    newRow23.MaturityDate = item.MaturityDate;
                    newRow23.Active = true;
                    newRow23.Deleted = false;
                    newRow23.CreatedBy = item.CreatedBy;
                    newRow23.CreatedOn = DateTime.Now;
                    newRow23.UpdatedBy = item.CreatedBy;
                    newRow23.UpdatedOn = DateTime.Now;
                    newRow23.AttributeField = "Field" + nextFieldCount;
                    newRow23.Amount = (decimal)item.Field23;
                    newRows.Add(newRow23);
                    nextFieldCount++;
                }
                if (item.Field24 != null && item.Field24 != 1000000000000)
                {
                    var newRow24 = new credit_lgd_historyinformationdetails();
                    newRow24.LoanAmount = item.LoanAmount;
                    newRow24.CustomerName = item.CustomerName;
                    newRow24.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow24.ProductCode = item.ProductCode;
                    newRow24.ProductName = item.ProductName;
                    newRow24.EffectiveDate = item.EffectiveDate;
                    newRow24.MaturityDate = item.MaturityDate;
                    newRow24.Active = true;
                    newRow24.Deleted = false;
                    newRow24.CreatedBy = item.CreatedBy;
                    newRow24.CreatedOn = DateTime.Now;
                    newRow24.UpdatedBy = item.CreatedBy;
                    newRow24.UpdatedOn = DateTime.Now;
                    newRow24.AttributeField = "Field" + nextFieldCount;
                    newRow24.Amount = (decimal)item.Field24;
                    newRows.Add(newRow24);
                    nextFieldCount++;
                }
                if (item.Field25 != null && item.Field25 != 1000000000000)
                {
                    var newRow25 = new credit_lgd_historyinformationdetails();
                    newRow25.LoanAmount = item.LoanAmount;
                    newRow25.CustomerName = item.CustomerName;
                    newRow25.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow25.ProductCode = item.ProductCode;
                    newRow25.ProductName = item.ProductName;
                    newRow25.EffectiveDate = item.EffectiveDate;
                    newRow25.MaturityDate = item.MaturityDate;
                    newRow25.Active = true;
                    newRow25.Deleted = false;
                    newRow25.CreatedBy = item.CreatedBy;
                    newRow25.CreatedOn = DateTime.Now;
                    newRow25.UpdatedBy = item.CreatedBy;
                    newRow25.UpdatedOn = DateTime.Now;
                    newRow25.AttributeField = "Field" + nextFieldCount;
                    newRow25.Amount = (decimal)item.Field25;
                    newRows.Add(newRow25);
                    nextFieldCount++;
                }
                if (item.Field26 != null && item.Field26 != 1000000000000)
                {
                    var newRow26 = new credit_lgd_historyinformationdetails();
                    newRow26.LoanAmount = item.LoanAmount;
                    newRow26.CustomerName = item.CustomerName;
                    newRow26.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow26.ProductCode = item.ProductCode;
                    newRow26.ProductName = item.ProductName;
                    newRow26.EffectiveDate = item.EffectiveDate;
                    newRow26.MaturityDate = item.MaturityDate;
                    newRow26.Active = true;
                    newRow26.Deleted = false;
                    newRow26.CreatedBy = item.CreatedBy;
                    newRow26.CreatedOn = DateTime.Now;
                    newRow26.UpdatedBy = item.CreatedBy;
                    newRow26.UpdatedOn = DateTime.Now;
                    newRow26.AttributeField = "Field" + nextFieldCount;
                    newRow26.Amount = (decimal)item.Field26;
                    newRows.Add(newRow26);
                    nextFieldCount++;
                }
                if (item.Field27 != null && item.Field27 != 1000000000000)
                {
                    var newRow27 = new credit_lgd_historyinformationdetails();
                    newRow27.LoanAmount = item.LoanAmount;
                    newRow27.CustomerName = item.CustomerName;
                    newRow27.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow27.ProductCode = item.ProductCode;
                    newRow27.ProductName = item.ProductName;
                    newRow27.EffectiveDate = item.EffectiveDate;
                    newRow27.MaturityDate = item.MaturityDate;
                    newRow27.Active = true;
                    newRow27.Deleted = false;
                    newRow27.CreatedBy = item.CreatedBy;
                    newRow27.CreatedOn = DateTime.Now;
                    newRow27.UpdatedBy = item.CreatedBy;
                    newRow27.UpdatedOn = DateTime.Now;
                    newRow27.AttributeField = "Field" + nextFieldCount;
                    newRow27.Amount = (decimal)item.Field27;
                    newRows.Add(newRow27);
                    nextFieldCount++;
                }
                if (item.Field28 != null && item.Field28 != 1000000000000)
                {
                    var newRow28 = new credit_lgd_historyinformationdetails();
                    newRow28.LoanAmount = item.LoanAmount;
                    newRow28.CustomerName = item.CustomerName;
                    newRow28.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow28.ProductCode = item.ProductCode;
                    newRow28.ProductName = item.ProductName;
                    newRow28.EffectiveDate = item.EffectiveDate;
                    newRow28.MaturityDate = item.MaturityDate;
                    newRow28.Active = true;
                    newRow28.Deleted = false;
                    newRow28.CreatedBy = item.CreatedBy;
                    newRow28.CreatedOn = DateTime.Now;
                    newRow28.UpdatedBy = item.CreatedBy;
                    newRow28.UpdatedOn = DateTime.Now;
                    newRow28.AttributeField = "Field" + nextFieldCount;
                    newRow28.Amount = (decimal)item.Field28;
                    newRows.Add(newRow28);
                    nextFieldCount++;
                }
                if (item.Field29 != null && item.Field29 != 1000000000000)
                {
                    var newRow29 = new credit_lgd_historyinformationdetails();
                    newRow29.LoanAmount = item.LoanAmount;
                    newRow29.CustomerName = item.CustomerName;
                    newRow29.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow29.ProductCode = item.ProductCode;
                    newRow29.ProductName = item.ProductName;
                    newRow29.EffectiveDate = item.EffectiveDate;
                    newRow29.MaturityDate = item.MaturityDate;
                    newRow29.Active = true;
                    newRow29.Deleted = false;
                    newRow29.CreatedBy = item.CreatedBy;
                    newRow29.CreatedOn = DateTime.Now;
                    newRow29.UpdatedBy = item.CreatedBy;
                    newRow29.UpdatedOn = DateTime.Now;
                    newRow29.AttributeField = "Field" + nextFieldCount;
                    newRow29.Amount = (decimal)item.Field29;
                    newRows.Add(newRow29);
                    nextFieldCount++;
                }
                if (item.Field30 != null && item.Field30 != 1000000000000)
                {
                    var newRow30 = new credit_lgd_historyinformationdetails();
                    newRow30.LoanAmount = item.LoanAmount;
                    newRow30.CustomerName = item.CustomerName;
                    newRow30.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow30.ProductCode = item.ProductCode;
                    newRow30.ProductName = item.ProductName;
                    newRow30.EffectiveDate = item.EffectiveDate;
                    newRow30.MaturityDate = item.MaturityDate;
                    newRow30.Active = true;
                    newRow30.Deleted = false;
                    newRow30.CreatedBy = item.CreatedBy;
                    newRow30.CreatedOn = DateTime.Now;
                    newRow30.UpdatedBy = item.CreatedBy;
                    newRow30.UpdatedOn = DateTime.Now;
                    newRow30.AttributeField = "Field" + nextFieldCount;
                    newRow30.Amount = (decimal)item.Field30;
                    newRows.Add(newRow30);
                    nextFieldCount++;
                }
                if (item.Field31 != null && item.Field31 != 1000000000000)
                {
                    var newRow31 = new credit_lgd_historyinformationdetails();
                    newRow31.LoanAmount = item.LoanAmount;
                    newRow31.CustomerName = item.CustomerName;
                    newRow31.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow31.ProductCode = item.ProductCode;
                    newRow31.ProductName = item.ProductName;
                    newRow31.EffectiveDate = item.EffectiveDate;
                    newRow31.MaturityDate = item.MaturityDate;
                    newRow31.Active = true;
                    newRow31.Deleted = false;
                    newRow31.CreatedBy = item.CreatedBy;
                    newRow31.CreatedOn = DateTime.Now;
                    newRow31.UpdatedBy = item.CreatedBy;
                    newRow31.UpdatedOn = DateTime.Now;
                    newRow31.AttributeField = "Field" + nextFieldCount;
                    newRow31.Amount = (decimal)item.Field31;
                    newRows.Add(newRow31);
                    nextFieldCount++;
                }
                if (item.Field32 != null && item.Field32 != 1000000000000)
                {
                    var newRow32 = new credit_lgd_historyinformationdetails();
                    newRow32.LoanAmount = item.LoanAmount;
                    newRow32.CustomerName = item.CustomerName;
                    newRow32.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow32.ProductCode = item.ProductCode;
                    newRow32.ProductName = item.ProductName;
                    newRow32.EffectiveDate = item.EffectiveDate;
                    newRow32.MaturityDate = item.MaturityDate;
                    newRow32.Active = true;
                    newRow32.Deleted = false;
                    newRow32.CreatedBy = item.CreatedBy;
                    newRow32.CreatedOn = DateTime.Now;
                    newRow32.UpdatedBy = item.CreatedBy;
                    newRow32.UpdatedOn = DateTime.Now;
                    newRow32.AttributeField = "Field" + nextFieldCount;
                    newRow32.Amount = (decimal)item.Field32;
                    newRows.Add(newRow32);
                    nextFieldCount++;
                }
                if (item.Field33 != null && item.Field33 != 1000000000000)
                {
                    var newRow33 = new credit_lgd_historyinformationdetails();
                    newRow33.LoanAmount = item.LoanAmount;
                    newRow33.CustomerName = item.CustomerName;
                    newRow33.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow33.ProductCode = item.ProductCode;
                    newRow33.ProductName = item.ProductName;
                    newRow33.EffectiveDate = item.EffectiveDate;
                    newRow33.MaturityDate = item.MaturityDate;
                    newRow33.Active = true;
                    newRow33.Deleted = false;
                    newRow33.CreatedBy = item.CreatedBy;
                    newRow33.CreatedOn = DateTime.Now;
                    newRow33.UpdatedBy = item.CreatedBy;
                    newRow33.UpdatedOn = DateTime.Now;
                    newRow33.AttributeField = "Field" + nextFieldCount;
                    newRow33.Amount = (decimal)item.Field33;
                    newRows.Add(newRow33);
                    nextFieldCount++;
                }
                if (item.Field34 != null && item.Field34 != 1000000000000)
                {
                    var newRow34 = new credit_lgd_historyinformationdetails();
                    newRow34.LoanAmount = item.LoanAmount;
                    newRow34.CustomerName = item.CustomerName;
                    newRow34.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow34.ProductCode = item.ProductCode;
                    newRow34.ProductName = item.ProductName;
                    newRow34.EffectiveDate = item.EffectiveDate;
                    newRow34.MaturityDate = item.MaturityDate;
                    newRow34.Active = true;
                    newRow34.Deleted = false;
                    newRow34.CreatedBy = item.CreatedBy;
                    newRow34.CreatedOn = DateTime.Now;
                    newRow34.UpdatedBy = item.CreatedBy;
                    newRow34.UpdatedOn = DateTime.Now;
                    newRow34.AttributeField = "Field" + nextFieldCount;
                    newRow34.Amount = (decimal)item.Field34;
                    newRows.Add(newRow34);
                    nextFieldCount++;
                }
                if (item.Field35 != null && item.Field35 != 1000000000000)
                {
                    var newRow35 = new credit_lgd_historyinformationdetails();
                    newRow35.LoanAmount = item.LoanAmount;
                    newRow35.CustomerName = item.CustomerName;
                    newRow35.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow35.ProductCode = item.ProductCode;
                    newRow35.ProductName = item.ProductName;
                    newRow35.EffectiveDate = item.EffectiveDate;
                    newRow35.MaturityDate = item.MaturityDate;
                    newRow35.Active = true;
                    newRow35.Deleted = false;
                    newRow35.CreatedBy = item.CreatedBy;
                    newRow35.CreatedOn = DateTime.Now;
                    newRow35.UpdatedBy = item.CreatedBy;
                    newRow35.UpdatedOn = DateTime.Now;
                    newRow35.AttributeField = "Field" + nextFieldCount;
                    newRow35.Amount = (decimal)item.Field35;
                    newRows.Add(newRow35);
                    nextFieldCount++;
                }
                if (item.Field36 != null && item.Field36 != 1000000000000)
                {
                    var newRow36 = new credit_lgd_historyinformationdetails();
                    newRow36.LoanAmount = item.LoanAmount;
                    newRow36.CustomerName = item.CustomerName;
                    newRow36.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow36.ProductCode = item.ProductCode;
                    newRow36.ProductName = item.ProductName;
                    newRow36.EffectiveDate = item.EffectiveDate;
                    newRow36.MaturityDate = item.MaturityDate;
                    newRow36.Active = true;
                    newRow36.Deleted = false;
                    newRow36.CreatedBy = item.CreatedBy;
                    newRow36.CreatedOn = DateTime.Now;
                    newRow36.UpdatedBy = item.CreatedBy;
                    newRow36.UpdatedOn = DateTime.Now;
                    newRow36.AttributeField = "Field" + nextFieldCount;
                    newRow36.Amount = (decimal)item.Field36;
                    newRows.Add(newRow36);
                    nextFieldCount++;
                }
                if (item.Field37 != null && item.Field37 != 1000000000000)
                {
                    var newRow37 = new credit_lgd_historyinformationdetails();
                    newRow37.LoanAmount = item.LoanAmount;
                    newRow37.CustomerName = item.CustomerName;
                    newRow37.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow37.ProductCode = item.ProductCode;
                    newRow37.ProductName = item.ProductName;
                    newRow37.EffectiveDate = item.EffectiveDate;
                    newRow37.MaturityDate = item.MaturityDate;
                    newRow37.Active = true;
                    newRow37.Deleted = false;
                    newRow37.CreatedBy = item.CreatedBy;
                    newRow37.CreatedOn = DateTime.Now;
                    newRow37.UpdatedBy = item.CreatedBy;
                    newRow37.UpdatedOn = DateTime.Now;
                    newRow37.AttributeField = "Field" + nextFieldCount;
                    newRow37.Amount = (decimal)item.Field37;
                    newRows.Add(newRow37);
                    nextFieldCount++;
                }
                if (item.Field38 != null && item.Field38 != 1000000000000)
                {
                    var newRow38 = new credit_lgd_historyinformationdetails();
                    newRow38.LoanAmount = item.LoanAmount;
                    newRow38.CustomerName = item.CustomerName;
                    newRow38.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow38.ProductCode = item.ProductCode;
                    newRow38.ProductName = item.ProductName;
                    newRow38.EffectiveDate = item.EffectiveDate;
                    newRow38.MaturityDate = item.MaturityDate;
                    newRow38.Active = true;
                    newRow38.Deleted = false;
                    newRow38.CreatedBy = item.CreatedBy;
                    newRow38.CreatedOn = DateTime.Now;
                    newRow38.UpdatedBy = item.CreatedBy;
                    newRow38.UpdatedOn = DateTime.Now;
                    newRow38.AttributeField = "Field" + nextFieldCount;
                    newRow38.Amount = (decimal)item.Field38;
                    newRows.Add(newRow38);
                    nextFieldCount++;
                }
                if (item.Field39 != null && item.Field39 != 1000000000000)
                {
                    var newRow39 = new credit_lgd_historyinformationdetails();
                    newRow39.LoanAmount = item.LoanAmount;
                    newRow39.CustomerName = item.CustomerName;
                    newRow39.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow39.ProductCode = item.ProductCode;
                    newRow39.ProductName = item.ProductName;
                    newRow39.EffectiveDate = item.EffectiveDate;
                    newRow39.MaturityDate = item.MaturityDate;
                    newRow39.Active = true;
                    newRow39.Deleted = false;
                    newRow39.CreatedBy = item.CreatedBy;
                    newRow39.CreatedOn = DateTime.Now;
                    newRow39.UpdatedBy = item.CreatedBy;
                    newRow39.UpdatedOn = DateTime.Now;
                    newRow39.AttributeField = "Field" + nextFieldCount;
                    newRow39.Amount = (decimal)item.Field39;
                    newRows.Add(newRow39);
                    nextFieldCount++;
                }
                if (item.Field40 != null && item.Field40 != 1000000000000)
                {
                    var newRow40 = new credit_lgd_historyinformationdetails();
                    newRow40.LoanAmount = item.LoanAmount;
                    newRow40.CustomerName = item.CustomerName;
                    newRow40.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow40.ProductCode = item.ProductCode;
                    newRow40.ProductName = item.ProductName;
                    newRow40.EffectiveDate = item.EffectiveDate;
                    newRow40.MaturityDate = item.MaturityDate;
                    newRow40.Active = true;
                    newRow40.Deleted = false;
                    newRow40.CreatedBy = item.CreatedBy;
                    newRow40.CreatedOn = DateTime.Now;
                    newRow40.UpdatedBy = item.CreatedBy;
                    newRow40.UpdatedOn = DateTime.Now;
                    newRow40.AttributeField = "Field" + nextFieldCount;
                    newRow40.Amount = (decimal)item.Field40;
                    newRows.Add(newRow40);
                    nextFieldCount++;
                }
                if (item.Field41 != null && item.Field41 != 1000000000000)
                {
                    var newRow41 = new credit_lgd_historyinformationdetails();
                    newRow41.LoanAmount = item.LoanAmount;
                    newRow41.CustomerName = item.CustomerName;
                    newRow41.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow41.ProductCode = item.ProductCode;
                    newRow41.ProductName = item.ProductName;
                    newRow41.EffectiveDate = item.EffectiveDate;
                    newRow41.MaturityDate = item.MaturityDate;
                    newRow41.Active = true;
                    newRow41.Deleted = false;
                    newRow41.CreatedBy = item.CreatedBy;
                    newRow41.CreatedOn = DateTime.Now;
                    newRow41.UpdatedBy = item.CreatedBy;
                    newRow41.UpdatedOn = DateTime.Now;
                    newRow41.AttributeField = "Field" + nextFieldCount;
                    newRow41.Amount = (decimal)item.Field41;
                    newRows.Add(newRow41);
                    nextFieldCount++;
                }
                if (item.Field42 != null && item.Field42 != 1000000000000)
                {
                    var newRow42 = new credit_lgd_historyinformationdetails();
                    newRow42.LoanAmount = item.LoanAmount;
                    newRow42.CustomerName = item.CustomerName;
                    newRow42.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow42.ProductCode = item.ProductCode;
                    newRow42.ProductName = item.ProductName;
                    newRow42.EffectiveDate = item.EffectiveDate;
                    newRow42.MaturityDate = item.MaturityDate;
                    newRow42.Active = true;
                    newRow42.Deleted = false;
                    newRow42.CreatedBy = item.CreatedBy;
                    newRow42.CreatedOn = DateTime.Now;
                    newRow42.UpdatedBy = item.CreatedBy;
                    newRow42.UpdatedOn = DateTime.Now;
                    newRow42.AttributeField = "Field" + nextFieldCount;
                    newRow42.Amount = (decimal)item.Field42;
                    newRows.Add(newRow42);
                    nextFieldCount++;
                }
                if (item.Field43 != null && item.Field43 != 1000000000000)
                {
                    var newRow43 = new credit_lgd_historyinformationdetails();
                    newRow43.LoanAmount = item.LoanAmount;
                    newRow43.CustomerName = item.CustomerName;
                    newRow43.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow43.ProductCode = item.ProductCode;
                    newRow43.ProductName = item.ProductName;
                    newRow43.EffectiveDate = item.EffectiveDate;
                    newRow43.MaturityDate = item.MaturityDate;
                    newRow43.Active = true;
                    newRow43.Deleted = false;
                    newRow43.CreatedBy = item.CreatedBy;
                    newRow43.CreatedOn = DateTime.Now;
                    newRow43.UpdatedBy = item.CreatedBy;
                    newRow43.UpdatedOn = DateTime.Now;
                    newRow43.AttributeField = "Field" + nextFieldCount;
                    newRow43.Amount = (decimal)item.Field43;
                    newRows.Add(newRow43);
                    nextFieldCount++;
                }
                if (item.Field44 != null && item.Field44 != 1000000000000)
                {
                    var newRow44 = new credit_lgd_historyinformationdetails();
                    newRow44.LoanAmount = item.LoanAmount;
                    newRow44.CustomerName = item.CustomerName;
                    newRow44.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow44.ProductCode = item.ProductCode;
                    newRow44.ProductName = item.ProductName;
                    newRow44.EffectiveDate = item.EffectiveDate;
                    newRow44.MaturityDate = item.MaturityDate;
                    newRow44.Active = true;
                    newRow44.Deleted = false;
                    newRow44.CreatedBy = item.CreatedBy;
                    newRow44.CreatedOn = DateTime.Now;
                    newRow44.UpdatedBy = item.CreatedBy;
                    newRow44.UpdatedOn = DateTime.Now;
                    newRow44.AttributeField = "Field" + nextFieldCount;
                    newRow44.Amount = (decimal)item.Field44;
                    newRows.Add(newRow44);
                    nextFieldCount++;
                }
                if (item.Field45 != null && item.Field45 != 1000000000000)
                {
                    var newRow45 = new credit_lgd_historyinformationdetails();
                    newRow45.LoanAmount = item.LoanAmount;
                    newRow45.CustomerName = item.CustomerName;
                    newRow45.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow45.ProductCode = item.ProductCode;
                    newRow45.ProductName = item.ProductName;
                    newRow45.EffectiveDate = item.EffectiveDate;
                    newRow45.MaturityDate = item.MaturityDate;
                    newRow45.Active = true;
                    newRow45.Deleted = false;
                    newRow45.CreatedBy = item.CreatedBy;
                    newRow45.CreatedOn = DateTime.Now;
                    newRow45.UpdatedBy = item.CreatedBy;
                    newRow45.UpdatedOn = DateTime.Now;
                    newRow45.AttributeField = "Field" + nextFieldCount;
                    newRow45.Amount = (decimal)item.Field45;
                    newRows.Add(newRow45);
                    nextFieldCount++;
                }
                if (item.Field46 != null && item.Field46 != 1000000000000)
                {
                    var newRow46 = new credit_lgd_historyinformationdetails();
                    newRow46.LoanAmount = item.LoanAmount;
                    newRow46.CustomerName = item.CustomerName;
                    newRow46.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow46.ProductCode = item.ProductCode;
                    newRow46.ProductName = item.ProductName;
                    newRow46.EffectiveDate = item.EffectiveDate;
                    newRow46.MaturityDate = item.MaturityDate;
                    newRow46.Active = true;
                    newRow46.Deleted = false;
                    newRow46.CreatedBy = item.CreatedBy;
                    newRow46.CreatedOn = DateTime.Now;
                    newRow46.UpdatedBy = item.CreatedBy;
                    newRow46.UpdatedOn = DateTime.Now;
                    newRow46.AttributeField = "Field" + nextFieldCount;
                    newRow46.Amount = (decimal)item.Field46;
                    newRows.Add(newRow46);
                    nextFieldCount++;
                }
                if (item.Field47 != null && item.Field47 != 1000000000000)
                {
                    var newRow47 = new credit_lgd_historyinformationdetails();
                    newRow47.LoanAmount = item.LoanAmount;
                    newRow47.CustomerName = item.CustomerName;
                    newRow47.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow47.ProductCode = item.ProductCode;
                    newRow47.ProductName = item.ProductName;
                    newRow47.EffectiveDate = item.EffectiveDate;
                    newRow47.MaturityDate = item.MaturityDate;
                    newRow47.Active = true;
                    newRow47.Deleted = false;
                    newRow47.CreatedBy = item.CreatedBy;
                    newRow47.CreatedOn = DateTime.Now;
                    newRow47.UpdatedBy = item.CreatedBy;
                    newRow47.UpdatedOn = DateTime.Now;
                    newRow47.AttributeField = "Field" + nextFieldCount;
                    newRow47.Amount = (decimal)item.Field47;
                    newRows.Add(newRow47);
                    nextFieldCount++;
                }
                if (item.Field48 != null && item.Field48 != 1000000000000)
                {
                    var newRow48 = new credit_lgd_historyinformationdetails();
                    newRow48.LoanAmount = item.LoanAmount;
                    newRow48.CustomerName = item.CustomerName;
                    newRow48.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow48.ProductCode = item.ProductCode;
                    newRow48.ProductName = item.ProductName;
                    newRow48.EffectiveDate = item.EffectiveDate;
                    newRow48.MaturityDate = item.MaturityDate;
                    newRow48.Active = true;
                    newRow48.Deleted = false;
                    newRow48.CreatedBy = item.CreatedBy;
                    newRow48.CreatedOn = DateTime.Now;
                    newRow48.UpdatedBy = item.CreatedBy;
                    newRow48.UpdatedOn = DateTime.Now;
                    newRow48.AttributeField = "Field" + nextFieldCount;
                    newRow48.Amount = (decimal)item.Field48;
                    newRows.Add(newRow48);
                    nextFieldCount++;
                }
                if (item.Field49 != null && item.Field49 != 1000000000000)
                {
                    var newRow49 = new credit_lgd_historyinformationdetails();
                    newRow49.LoanAmount = item.LoanAmount;
                    newRow49.CustomerName = item.CustomerName;
                    newRow49.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow49.ProductCode = item.ProductCode;
                    newRow49.ProductName = item.ProductName;
                    newRow49.EffectiveDate = item.EffectiveDate;
                    newRow49.MaturityDate = item.MaturityDate;
                    newRow49.Active = true;
                    newRow49.Deleted = false;
                    newRow49.CreatedBy = item.CreatedBy;
                    newRow49.CreatedOn = DateTime.Now;
                    newRow49.UpdatedBy = item.CreatedBy;
                    newRow49.UpdatedOn = DateTime.Now;
                    newRow49.AttributeField = "Field" + nextFieldCount;
                    newRow49.Amount = (decimal)item.Field49;
                    newRows.Add(newRow49);
                    nextFieldCount++;
                }
                if (item.Field50 != null && item.Field50 != 1000000000000)
                {
                    var newRow50 = new credit_lgd_historyinformationdetails();
                    newRow50.LoanAmount = item.LoanAmount;
                    newRow50.CustomerName = item.CustomerName;
                    newRow50.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow50.ProductCode = item.ProductCode;
                    newRow50.ProductName = item.ProductName;
                    newRow50.EffectiveDate = item.EffectiveDate;
                    newRow50.MaturityDate = item.MaturityDate;
                    newRow50.Active = true;
                    newRow50.Deleted = false;
                    newRow50.CreatedBy = item.CreatedBy;
                    newRow50.CreatedOn = DateTime.Now;
                    newRow50.UpdatedBy = item.CreatedBy;
                    newRow50.UpdatedOn = DateTime.Now;
                    newRow50.AttributeField = "Field" + nextFieldCount;
                    newRow50.Amount = (decimal)item.Field50;
                    newRows.Add(newRow50);
                    nextFieldCount++;
                }
                if (item.Field51 != null && item.Field51 != 1000000000000)
                {
                    var newRow51 = new credit_lgd_historyinformationdetails();
                    newRow51.LoanAmount = item.LoanAmount;
                    newRow51.CustomerName = item.CustomerName;
                    newRow51.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow51.ProductCode = item.ProductCode;
                    newRow51.ProductName = item.ProductName;
                    newRow51.EffectiveDate = item.EffectiveDate;
                    newRow51.MaturityDate = item.MaturityDate;
                    newRow51.Active = true;
                    newRow51.Deleted = false;
                    newRow51.CreatedBy = item.CreatedBy;
                    newRow51.CreatedOn = DateTime.Now;
                    newRow51.UpdatedBy = item.CreatedBy;
                    newRow51.UpdatedOn = DateTime.Now;
                    newRow51.AttributeField = "Field" + nextFieldCount;
                    newRow51.Amount = (decimal)item.Field51;
                    newRows.Add(newRow51);
                    nextFieldCount++;
                }
                if (item.Field52 != null && item.Field52 != 1000000000000)
                {
                    var newRow52 = new credit_lgd_historyinformationdetails();
                    newRow52.LoanAmount = item.LoanAmount;
                    newRow52.CustomerName = item.CustomerName;
                    newRow52.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow52.ProductCode = item.ProductCode;
                    newRow52.ProductName = item.ProductName;
                    newRow52.EffectiveDate = item.EffectiveDate;
                    newRow52.MaturityDate = item.MaturityDate;
                    newRow52.Active = true;
                    newRow52.Deleted = false;
                    newRow52.CreatedBy = item.CreatedBy;
                    newRow52.CreatedOn = DateTime.Now;
                    newRow52.UpdatedBy = item.CreatedBy;
                    newRow52.UpdatedOn = DateTime.Now;
                    newRow52.AttributeField = "Field" + nextFieldCount;
                    newRow52.Amount = (decimal)item.Field52;
                    newRows.Add(newRow52);
                    nextFieldCount++;
                }
                if (item.Field53 != null && item.Field53 != 1000000000000)
                {
                    var newRow53 = new credit_lgd_historyinformationdetails();
                    newRow53.LoanAmount = item.LoanAmount;
                    newRow53.CustomerName = item.CustomerName;
                    newRow53.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow53.ProductCode = item.ProductCode;
                    newRow53.ProductName = item.ProductName;
                    newRow53.EffectiveDate = item.EffectiveDate;
                    newRow53.MaturityDate = item.MaturityDate;
                    newRow53.Active = true;
                    newRow53.Deleted = false;
                    newRow53.CreatedBy = item.CreatedBy;
                    newRow53.CreatedOn = DateTime.Now;
                    newRow53.UpdatedBy = item.CreatedBy;
                    newRow53.UpdatedOn = DateTime.Now;
                    newRow53.AttributeField = "Field" + nextFieldCount;
                    newRow53.Amount = (decimal)item.Field53;
                    newRows.Add(newRow53);
                    nextFieldCount++;
                }
                if (item.Field54 != null && item.Field54 != 1000000000000)
                {
                    var newRow54 = new credit_lgd_historyinformationdetails();
                    newRow54.LoanAmount = item.LoanAmount;
                    newRow54.CustomerName = item.CustomerName;
                    newRow54.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow54.ProductCode = item.ProductCode;
                    newRow54.ProductName = item.ProductName;
                    newRow54.EffectiveDate = item.EffectiveDate;
                    newRow54.MaturityDate = item.MaturityDate;
                    newRow54.Active = true;
                    newRow54.Deleted = false;
                    newRow54.CreatedBy = item.CreatedBy;
                    newRow54.CreatedOn = DateTime.Now;
                    newRow54.UpdatedBy = item.CreatedBy;
                    newRow54.UpdatedOn = DateTime.Now;
                    newRow54.AttributeField = "Field" + nextFieldCount;
                    newRow54.Amount = (decimal)item.Field54;
                    newRows.Add(newRow54);
                    nextFieldCount++;
                }
                if (item.Field55 != null && item.Field55 != 1000000000000)
                {
                    var newRow55 = new credit_lgd_historyinformationdetails();
                    newRow55.LoanAmount = item.LoanAmount;
                    newRow55.CustomerName = item.CustomerName;
                    newRow55.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow55.ProductCode = item.ProductCode;
                    newRow55.ProductName = item.ProductName;
                    newRow55.EffectiveDate = item.EffectiveDate;
                    newRow55.MaturityDate = item.MaturityDate;
                    newRow55.Active = true;
                    newRow55.Deleted = false;
                    newRow55.CreatedBy = item.CreatedBy;
                    newRow55.CreatedOn = DateTime.Now;
                    newRow55.UpdatedBy = item.CreatedBy;
                    newRow55.UpdatedOn = DateTime.Now;
                    newRow55.AttributeField = "Field" + nextFieldCount;
                    newRow55.Amount = (decimal)item.Field55;
                    newRows.Add(newRow55);
                    nextFieldCount++;
                }
                if (item.Field56 != null && item.Field56 != 1000000000000)
                {
                    var newRow56 = new credit_lgd_historyinformationdetails();
                    newRow56.LoanAmount = item.LoanAmount;
                    newRow56.CustomerName = item.CustomerName;
                    newRow56.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow56.ProductCode = item.ProductCode;
                    newRow56.ProductName = item.ProductName;
                    newRow56.EffectiveDate = item.EffectiveDate;
                    newRow56.MaturityDate = item.MaturityDate;
                    newRow56.Active = true;
                    newRow56.Deleted = false;
                    newRow56.CreatedBy = item.CreatedBy;
                    newRow56.CreatedOn = DateTime.Now;
                    newRow56.UpdatedBy = item.CreatedBy;
                    newRow56.UpdatedOn = DateTime.Now;
                    newRow56.AttributeField = "Field" + nextFieldCount;
                    newRow56.Amount = (decimal)item.Field56;
                    newRows.Add(newRow56);
                    nextFieldCount++;
                }
                if (item.Field57 != null && item.Field57 != 1000000000000)
                {
                    var newRow57 = new credit_lgd_historyinformationdetails();
                    newRow57.LoanAmount = item.LoanAmount;
                    newRow57.CustomerName = item.CustomerName;
                    newRow57.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow57.ProductCode = item.ProductCode;
                    newRow57.ProductName = item.ProductName;
                    newRow57.EffectiveDate = item.EffectiveDate;
                    newRow57.MaturityDate = item.MaturityDate;
                    newRow57.Active = true;
                    newRow57.Deleted = false;
                    newRow57.CreatedBy = item.CreatedBy;
                    newRow57.CreatedOn = DateTime.Now;
                    newRow57.UpdatedBy = item.CreatedBy;
                    newRow57.UpdatedOn = DateTime.Now;
                    newRow57.AttributeField = "Field" + nextFieldCount;
                    newRow57.Amount = (decimal)item.Field57;
                    newRows.Add(newRow57);
                    nextFieldCount++;
                }
                if (item.Field58 != null && item.Field58 != 1000000000000)
                {
                    var newRow58 = new credit_lgd_historyinformationdetails();
                    newRow58.LoanAmount = item.LoanAmount;
                    newRow58.CustomerName = item.CustomerName;
                    newRow58.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow58.ProductCode = item.ProductCode;
                    newRow58.ProductName = item.ProductName;
                    newRow58.EffectiveDate = item.EffectiveDate;
                    newRow58.MaturityDate = item.MaturityDate;
                    newRow58.Active = true;
                    newRow58.Deleted = false;
                    newRow58.CreatedBy = item.CreatedBy;
                    newRow58.CreatedOn = DateTime.Now;
                    newRow58.UpdatedBy = item.CreatedBy;
                    newRow58.UpdatedOn = DateTime.Now;
                    newRow58.AttributeField = "Field" + nextFieldCount;
                    newRow58.Amount = (decimal)item.Field58;
                    newRows.Add(newRow58);
                    nextFieldCount++;
                }
                if (item.Field59 != null && item.Field59 != 1000000000000)
                {
                    var newRow59 = new credit_lgd_historyinformationdetails();
                    newRow59.LoanAmount = item.LoanAmount;
                    newRow59.CustomerName = item.CustomerName;
                    newRow59.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow59.ProductCode = item.ProductCode;
                    newRow59.ProductName = item.ProductName;
                    newRow59.EffectiveDate = item.EffectiveDate;
                    newRow59.MaturityDate = item.MaturityDate;
                    newRow59.Active = true;
                    newRow59.Deleted = false;
                    newRow59.CreatedBy = item.CreatedBy;
                    newRow59.CreatedOn = DateTime.Now;
                    newRow59.UpdatedBy = item.CreatedBy;
                    newRow59.UpdatedOn = DateTime.Now;
                    newRow59.AttributeField = "Field" + nextFieldCount;
                    newRow59.Amount = (decimal)item.Field59;
                    newRows.Add(newRow59);
                    nextFieldCount++;
                }
                if (item.Field60 != null && item.Field60 != 1000000000000)
                {
                    var newRow60 = new credit_lgd_historyinformationdetails();
                    newRow60.LoanAmount = item.LoanAmount;
                    newRow60.CustomerName = item.CustomerName;
                    newRow60.LoanReferenceNumber = item.LoanReferenceNumber;
                    newRow60.ProductCode = item.ProductCode;
                    newRow60.ProductName = item.ProductName;
                    newRow60.EffectiveDate = item.EffectiveDate;
                    newRow60.MaturityDate = item.MaturityDate;
                    newRow60.Active = true;
                    newRow60.Deleted = false;
                    newRow60.CreatedBy = item.CreatedBy;
                    newRow60.CreatedOn = DateTime.Now;
                    newRow60.UpdatedBy = item.CreatedBy;
                    newRow60.UpdatedOn = DateTime.Now;
                    newRow60.AttributeField = "Field" + nextFieldCount;
                    newRow60.Amount = (decimal)item.Field60;
                    newRows.Add(newRow60);
                    nextFieldCount++;
                }
                outputTable.AddRange(newRows);
            }

            _dataContext.credit_lgd_historyinformationdetails.AddRange(outputTable);

            _dataContext.SaveChanges();

        }

        private void AddIndividualApplicationScoreCard(List<credit_individualapplicationscorecard_history> inputTable, string createdBy)
        {
            try
            {
                var response = false;
                if (inputTable.Count > 0)
                {

                    foreach (var item in inputTable)
                    {
                        var loanId = Convert.ToInt32(GeneralHelpers.GenerateRandomDigitCode(5));
                        var CustomerId = Convert.ToInt32(GeneralHelpers.GenerateRandomDigitCode(5));
                        var product = _dataContext.credit_product.Where(x => x.ProductCode == item.ProductCode).FirstOrDefault();
                        if (product == null)
                        {
                            throw new Exception("Product doesn't exist for product code");
                        }
                        var individualscorecard = _dataContext.credit_individualapplicationscorecard.Where(x => x.LoanRefNo == item.LoanReferenceNumber).FirstOrDefault();

                        if (individualscorecard != null)
                        {
                            individualscorecard.ProductId = product.ProductId;
                            individualscorecard.CustomerId = CustomerId;
                            individualscorecard.Field1 = item.Field1 == 1000000 ? null : item.Field1;
                            individualscorecard.Field2 = item.Field2 == 1000000 ? null : item.Field2;
                            individualscorecard.Field3 = item.Field3 == 1000000 ? null : item.Field3;
                            individualscorecard.Field4 = item.Field4 == 1000000 ? null : item.Field4;
                            individualscorecard.Field5 = item.Field5 == 1000000 ? null : item.Field5;
                            individualscorecard.Field6 = item.Field6 == 1000000 ? null : item.Field6;
                            individualscorecard.Field7 = item.Field7 == 1000000 ? null : item.Field7;
                            individualscorecard.Field8 = item.Field8 == 1000000 ? null : item.Field8;
                            individualscorecard.Field9 = item.Field9 == 1000000 ? null : item.Field9;
                            individualscorecard.Field10 = item.Field10 == 1000000 ? null : item.Field10;
                            individualscorecard.Field11 = item.Field11 == 1000000 ? null : item.Field11;
                            individualscorecard.Field12 = item.Field12 == 1000000 ? null : item.Field12;
                            individualscorecard.Field13 = item.Field13 == 1000000 ? null : item.Field13;
                            individualscorecard.Field14 = item.Field14 == 1000000 ? null : item.Field14;
                            individualscorecard.Field15 = item.Field15 == 1000000 ? null : item.Field15;
                            individualscorecard.Field16 = item.Field16 == 1000000 ? null : item.Field16;
                            individualscorecard.Field17 = item.Field17 == 1000000 ? null : item.Field17;
                            individualscorecard.Field18 = item.Field18 == 1000000 ? null : item.Field18;
                            individualscorecard.Field19 = item.Field19 == 1000000 ? null : item.Field19;
                            individualscorecard.Field20 = item.Field20 == 1000000 ? null : item.Field20;
                            individualscorecard.Field21 = item.Field21 == 1000000 ? null : item.Field21;
                            individualscorecard.Field22 = item.Field22 == 1000000 ? null : item.Field22;
                            individualscorecard.Field23 = item.Field23 == 1000000 ? null : item.Field23;
                            individualscorecard.Field24 = item.Field24 == 1000000 ? null : item.Field24;
                            individualscorecard.Field25 = item.Field25 == 1000000 ? null : item.Field25;
                            individualscorecard.Field26 = item.Field26 == 1000000 ? null : item.Field26;
                            individualscorecard.Field27 = item.Field27 == 1000000 ? null : item.Field27;
                            individualscorecard.Field28 = item.Field28 == 1000000 ? null : item.Field28;
                            individualscorecard.Field29 = item.Field29 == 1000000 ? null : item.Field29;
                            individualscorecard.Field30 = item.Field30 == 1000000 ? null : item.Field30;
                            individualscorecard.Active = true;
                            individualscorecard.Deleted = false;
                            individualscorecard.UpdatedBy = createdBy;
                            individualscorecard.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            individualscorecard = new credit_individualapplicationscorecard
                            {
                                LoanApplicationId = loanId,
                                LoanRefNo = item.LoanReferenceNumber,
                                CustomerId = CustomerId,
                                ProductId = product.ProductId,
                                Field1 = item.Field1 == 1000000 ? null : item.Field1,
                                Field2 = item.Field2 == 1000000 ? null : item.Field2,
                                Field3 = item.Field3 == 1000000 ? null : item.Field3,
                                Field4 = item.Field4 == 1000000 ? null : item.Field4,
                                Field5 = item.Field5 == 1000000 ? null : item.Field5,
                                Field6 = item.Field6 == 1000000 ? null : item.Field6,
                                Field7 = item.Field7 == 1000000 ? null : item.Field7,
                                Field8 = item.Field8 == 1000000 ? null : item.Field8,
                                Field9 = item.Field9 == 1000000 ? null : item.Field9,
                                Field10 = item.Field10 == 1000000 ? null : item.Field10,
                                Field11 = item.Field11 == 1000000 ? null : item.Field11,
                                Field12 = item.Field12 == 1000000 ? null : item.Field12,
                                Field13 = item.Field13 == 1000000 ? null : item.Field13,
                                Field14 = item.Field14 == 1000000 ? null : item.Field14,
                                Field15 = item.Field15 == 1000000 ? null : item.Field15,
                                Field16 = item.Field16 == 1000000 ? null : item.Field16,
                                Field17 = item.Field17 == 1000000 ? null : item.Field17,
                                Field18 = item.Field18 == 1000000 ? null : item.Field18,
                                Field19 = item.Field19 == 1000000 ? null : item.Field19,
                                Field20 = item.Field20 == 1000000 ? null : item.Field20,
                                Field21 = item.Field21 == 1000000 ? null : item.Field21,
                                Field22 = item.Field22 == 1000000 ? null : item.Field22,
                                Field23 = item.Field23 == 1000000 ? null : item.Field23,
                                Field24 = item.Field24 == 1000000 ? null : item.Field24,
                                Field25 = item.Field25 == 1000000 ? null : item.Field25,
                                Field26 = item.Field26 == 1000000 ? null : item.Field26,
                                Field27 = item.Field27 == 1000000 ? null : item.Field27,
                                Field28 = item.Field28 == 1000000 ? null : item.Field28,
                                Field29 = item.Field29 == 1000000 ? null : item.Field29,
                                Field30 = item.Field30 == 1000000 ? null : item.Field30,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.credit_individualapplicationscorecard.Add(individualscorecard);
                        }
                    }
                    response = _dataContext.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        ///////IMPAIRMENT GOES HERE

        public bool RunImpairment()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                SqlConnection connection = new SqlConnection(connectionString);
                using (var con = connection)
                {
                    var cmd = new SqlCommand("spp_CalculateImpairment", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.CommandTimeout = 0;

                    con.Open();

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public IEnumerable<ImpairmentObj> GetImpairmentFromSP()
        {
            var oldRecords = (from f in _dataContext.credit_impairment_final select f).ToList();
            if (oldRecords != null)
            {
                _dataContext.credit_impairment_final.RemoveRange(oldRecords);
                _dataContext.SaveChanges();
            }
            var impairment = new List<ImpairmentObj>();
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            using (var con = connection)
            {
                var cmd = new SqlCommand("spp_get_impairment", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new ImpairmentObj();

                    if (reader["ProductCode"] != DBNull.Value)
                        cc.ProductCode = (reader["ProductCode"].ToString());

                    if (reader["ProductName"] != DBNull.Value)
                        cc.ProductName = (reader["ProductName"].ToString());

                    if (reader["PDType"] != DBNull.Value)
                        cc.ECLType = (reader["PDType"].ToString());

                    if (reader["Stage"] != DBNull.Value)
                        cc.Stage = (reader["Stage"].ToString());

                    if (reader["PD"] != DBNull.Value)
                        cc.PD = decimal.Parse(reader["PD"].ToString());

                    if (reader["LGD"] != DBNull.Value)
                        cc.LGD = decimal.Parse(reader["LGD"].ToString());

                    if (reader["EAD"] != DBNull.Value)
                        cc.EAD = decimal.Parse(reader["EAD"].ToString());

                    impairment.Add(cc);
                }
                con.Close();
            }

            foreach (var item in impairment)
            {
                credit_impairment_final impairmentDTO = new credit_impairment_final();
                decimal sum = 0, sum2 = 0;
                int count = 0;
                var scenario = (from f in _dataContext.ifrs_scenario_setup
                                where f.Deleted == false
                                select new
                                {
                                    f.ScenarioId,
                                    f.Likelihood,
                                    f.Scenario,
                                    f.Rate
                                }).ToList();
                foreach (var k in scenario)
                {
                    count = count + 1;

                    if (count == scenario.Count())
                    {
                        impairmentDTO.ImpairmentFinalId = 0;
                        impairmentDTO.ProductName = item.ProductName;
                        impairmentDTO.ProductCode = item.ProductCode;
                        impairmentDTO.ECLType = item.ECLType;
                        impairmentDTO.Stage = item.Stage;
                        impairmentDTO.Scenario = k.Scenario;
                        impairmentDTO.Likelihood = k.Likelihood / 100;
                        impairmentDTO.Rate = 1 + (k.Rate / 100);
                        impairmentDTO.PD = (impairmentDTO.Rate ?? 0) * item.PD;
                        impairmentDTO.LGD = (impairmentDTO.Rate ?? 0) * item.LGD;
                        sum = (impairmentDTO.Likelihood * impairmentDTO.PD * impairmentDTO.LGD) ?? 0;
                        sum2 = sum2 + sum;
                        impairmentDTO.EAD = item.EAD;
                        impairmentDTO.ECL = sum2 * item.EAD;
                        _dataContext.credit_impairment_final.Add(impairmentDTO);
                        _dataContext.SaveChanges();
                    }
                    else
                    {
                        impairmentDTO.ImpairmentFinalId = 0;
                        impairmentDTO.ProductName = item.ProductName;
                        impairmentDTO.ProductCode = item.ProductCode;
                        impairmentDTO.ECLType = item.ECLType;
                        impairmentDTO.Stage = item.Stage;
                        impairmentDTO.Scenario = k.Scenario;
                        impairmentDTO.Likelihood = k.Likelihood / 100;
                        impairmentDTO.Rate = 1 + (k.Rate / 100);
                        impairmentDTO.PD = (impairmentDTO.Rate ?? 0) * item.PD;
                        impairmentDTO.LGD = (impairmentDTO.Rate ?? 0) * item.LGD;
                        sum = (impairmentDTO.Likelihood * impairmentDTO.PD * impairmentDTO.LGD) ?? 0;
                        sum2 = sum2 + sum;
                        _dataContext.credit_impairment_final.Add(impairmentDTO);
                        _dataContext.SaveChanges();
                    }
                }
            }

        var fullImpairment = (from a in _dataContext.credit_impairment_final
                          select new ImpairmentObj
                          {
                              ImpairmentFinalId = a.ImpairmentFinalId,
                              ProductName = a.ProductName,
                              ProductCode = a.ProductCode,
                              ECLType = a.ECLType,
                              Stage = a.Stage,
                              Scenario = a.Scenario,
                              Likelihood = a.Likelihood,
                              Rate = a.Rate,
                              PD = a.LGD,
                              EAD = a.EAD,
                              ECL = a.ECL,
                              Date = a.Date,
                          }).ToList();
            return fullImpairment;
        }

        public byte[] GenerateExportImpairment()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Product Code");
            dt.Columns.Add("ECL Type");
            dt.Columns.Add("Stage");
            dt.Columns.Add("Scenario");
            dt.Columns.Add("Likelihood");
            dt.Columns.Add("Rate");
            dt.Columns.Add("PD");
            dt.Columns.Add("LGD");
            dt.Columns.Add("EAD");
            dt.Columns.Add("Impairment");

            var structures = (from f in _dataContext.credit_impairment_final select f).ToList();


            foreach (var kk in structures)
            {
                var row = dt.NewRow();
                row["Product Name"] = kk.ProductName;
                row["Product Code"] = kk.ProductCode;
                row["ECL Type"] = kk.ECLType;
                row["Stage"] = kk.Stage;
                row["Scenario"] = kk.Scenario;
                row["Likelihood"] = kk.Likelihood;
                row["Rate"] = kk.Rate;
                row["PD"] = kk.PD;
                row["LGD"] = kk.LGD;
                row["EAD"] = kk.EAD;
                row["Impairment"] = kk.ECL;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (structures != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyStructureDifinition");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}
