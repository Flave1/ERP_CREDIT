using Banking.Data;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using Finance.Contracts.Response.Reports;
using GOSLibraries.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Repository.Implement.Credit
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerRequest _serverRequest;
        public ReportService(DataContext context, IConfiguration configuration, IIdentityServerRequest serverRequest)
        {
            _context = context;
            _configuration = configuration;
            _serverRequest = serverRequest;
        }

        public List<LoanPaymentSchedulePeriodicObj> GetOfferLeterPeriodicSchedule(string applicationRefNumber)
        {
            var LoanApplicationId = (from a in _context.credit_loanapplication where a.ApplicationRefNumber == applicationRefNumber select a.LoanApplicationId).FirstOrDefault();
            var data = (from a in _context.tmp_loanapplicationscheduleperiodic
                        where a.LoanApplicationId == LoanApplicationId
                        select new LoanPaymentSchedulePeriodicObj
                        {
                            LoanId = a.LoanApplicationId,
                            PaymentNumber = a.PaymentNumber,
                            PaymentDate = a.PaymentDate.Date,
                            StartPrincipalAmount = (double)a.StartPrincipalAmount,
                            PeriodPaymentAmount = (double)a.PeriodPaymentAmount,
                            PeriodInterestAmount = (double)a.PeriodInterestAmount,
                            PeriodPrincipalAmount = (double)a.PeriodPrincipalAmount,
                            EndPrincipalAmount = (double)a.EndPrincipalAmount,
                            InterestRate = a.InterestRate,

                            AmortisedStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                            AmortisedPeriodPaymentAmount = (double)a.AmortisedPeriodPaymentAmount,
                            AmortisedPeriodInterestAmount = (double)a.AmortisedPeriodInterestAmount,
                            AmortisedPeriodPrincipalAmount = (double)a.AmortisedPeriodPrincipalAmount,
                            AmortisedEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                            EffectiveInterestRate = a.EffectiveInterestRate,
                        }).OrderBy(x => x.PaymentNumber).ToList();
            return data;
        }
        public List<ProductFeeObj> GetLoanApplicationFee(string applicationRefNumber)
        {
            try
            {
                var fees = (from a in _context.credit_loanapplication
                            join b in _context.credit_product on a.ApprovedProductId equals b.ProductId
                            join c in _context.credit_productfee on a.ApprovedProductId equals c.ProductId
                            join d in _context.credit_fee on c.FeeId equals d.FeeId
                            where a.ApplicationRefNumber == applicationRefNumber && a.Deleted == false &&
                                     a.ApprovalStatusId == (int)ApprovalStatus.Approved && a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter
                            select new ProductFeeObj()
                            {
                                FeeName = d.FeeName,
                                RateValue = (decimal)c.ProductAmount,
                                ProductName = b.ProductName
                            }).ToList();

                if (fees != null)
                {
                    return fees;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new List<ProductFeeObj>();
        }
        public OfferLetterDetailObj GenerateOfferLetterLMS(string loanRefNumber)
        {
            var _Companies = _serverRequest.GetAllCompanyAsync().Result;
            var _Currencies = _serverRequest.GetCurrencyAsync().Result;
            var offerLetterDetails = (from a in _context.credit_loanreviewapplication
                                      join c in _context.credit_loan on a.LoanId equals c.LoanId
                                      join b in _context.credit_loancustomer on a.CustomerId equals b.CustomerId into cc
                                      from b in cc.DefaultIfEmpty()
                                      where c.LoanRefNumber == loanRefNumber && a.Deleted == false &&
                                      a.ApprovalStatusId == (int)ApprovalStatus.Approved
                                      select new OfferLetterDetailObj
                                      {
                                          ProductName = _context.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                          LoanApplicationId = loanRefNumber,
                                          CustomerName = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).CustomerTypeId != 2 ? b.FirstName + " " + b.LastName : b.FirstName,
                                          Tenor = a.ApprovedTenor,
                                          InterestRate = a.ApprovedRate,
                                          LoanAmount = a.ApprovedAmount,
                                          ExchangeRate = c.ExchangeRate,
                                          CustomerAddress = b.Address ?? string.Empty,
                                          CustomerEmailAddress = b.Email,
                                          CustomerPhoneNumber = b.PhoneNo,
                                          ApplicationDate = a.CreatedOn ?? DateTime.Now,
                                          RepaymentSchedule = "Not applicable",
                                          RepaymentTerms = "Not applicable",
                                          Purpose = "",
                                          CompanyId = c.CompanyId ?? 0,
                                      }).FirstOrDefault();

                offerLetterDetails.CompanyName = _Companies.companyStructures.FirstOrDefault(x => x.companyStructureId == offerLetterDetails.CompanyId)?.name;
                offerLetterDetails.CurrencyName = _Currencies.commonLookups.FirstOrDefault(x => x.LookupId == offerLetterDetails.CurrencyId)?.LookupName;
                return offerLetterDetails;
        }
        public List<LoanPaymentSchedulePeriodicObj> GetOfferLeterPeriodicScheduleLMS(string loanRefNumber)
        {
            var LoanApplicationId = (from a in _context.credit_loan where a.LoanRefNumber == loanRefNumber select a.LoanId).FirstOrDefault();
            var data = (from a in _context.credit_loanscheduleperiodic
                        where a.LoanId == LoanApplicationId && a.Deleted == false
                        select new LoanPaymentSchedulePeriodicObj
                        {
                            LoanId = a.LoanId,
                            PaymentNumber = a.PaymentNumber,
                            PaymentDate = a.PaymentDate.Date,
                            StartPrincipalAmount = (double)a.StartPrincipalAmount,
                            PeriodPaymentAmount = (double)a.PeriodPaymentAmount,
                            PeriodInterestAmount = (double)a.PeriodInterestAmount,
                            PeriodPrincipalAmount = (double)a.PeriodPrincipalAmount,
                            EndPrincipalAmount = (double)a.EndPrincipalAmount,
                            InterestRate = a.InterestRate,

                            AmortisedStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                            AmortisedPeriodPaymentAmount = (double)a.AmortisedPeriodPaymentAmount,
                            AmortisedPeriodInterestAmount = (double)a.AmortisedPeriodInterestAmount,
                            AmortisedPeriodPrincipalAmount = (double)a.AmortisedPeriodPrincipalAmount,
                            AmortisedEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                            EffectiveInterestRate = a.EffectiveInterestRate,
                        }).OrderBy(x=>x.PaymentNumber).ToList();
            return data;
        }


        ///Financial Statement
        public List<FSReportObj> GetFSReport(DateTime? date1, DateTime? date2)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var fsreport = new List<FSReportObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("fs_report_summary", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@d1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@d2",
                    Value = date2,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var fs = new FSReportObj();

                    if (reader["GlMappingId"] != DBNull.Value)
                        fs.GlMappingId = int.Parse(reader["GlMappingId"].ToString());

                    if (reader["Caption"] != DBNull.Value)
                        fs.Caption = (reader["Caption"].ToString());

                    if (reader["SubCaption"] != DBNull.Value)
                        fs.SubCaption = (reader["SubCaption"].ToString());

                    if (reader["CompanyId"] != DBNull.Value)
                        fs.CompanyId = int.Parse(reader["CompanyId"].ToString());

                    if (reader["CompanyName"] != DBNull.Value)
                        fs.CompanyName = (reader["CompanyName"].ToString());

                    if (reader["SubPosition"] != DBNull.Value)
                        fs.SubPosition = int.Parse(reader["SubPosition"].ToString());

                    if (reader["GlCode"] != DBNull.Value)
                        fs.GlCode = (reader["GlCode"].ToString());

                    if (reader["ParentCaption"] != DBNull.Value)
                        fs.ParentCaption = (reader["ParentCaption"].ToString());

                    if (reader["Position"] != DBNull.Value)
                        fs.Position = int.Parse(reader["Position"].ToString());

                    if (reader["AccountType"] != DBNull.Value)
                        fs.AccountType = (reader["AccountType"].ToString());
                    if (reader["AccountTypeId"] != DBNull.Value)
                        fs.AccountTypeId = int.Parse(reader["AccountTypeId"].ToString());
                    if (reader["StatementType"] != DBNull.Value)
                        fs.StatementType = (reader["StatementType"].ToString());
                    if (reader["StatementTypeId"] != DBNull.Value)
                        fs.StatementTypeId = int.Parse(reader["StatementTypeId"].ToString());
                    if (reader["SubGlCode"] != DBNull.Value)
                        fs.SubGlCode = (reader["SubGlCode"].ToString());
                    if (reader["SubGlName"] != DBNull.Value)
                        fs.SubGlName = (reader["SubGlName"].ToString());
                    if (reader["GlId"] != DBNull.Value)
                        fs.GlId = int.Parse(reader["GlId"].ToString());
                    if (reader["CB"] != DBNull.Value)
                        fs.CB = decimal.Parse(reader["CB"].ToString());
                    if (reader["SubGlCode"] != DBNull.Value)
                        fs.SubGlCode1 = (reader["SubGlCode"].ToString());
                    if (reader["RunDate"] != DBNull.Value)
                        fs.RunDate = DateTime.Parse(reader["RunDate"].ToString()).Date;
                    if (reader["PreRunDate"] != DBNull.Value)
                        fs.PreRunDate = DateTime.Parse(reader["PreRunDate"].ToString()).Date;
                    fsreport.Add(fs);
                }
                con.Close();
            }
            return fsreport;
        }
        public List<FSReportObj> GetPLReport(DateTime? date1, DateTime? date2)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var fsreport = new List<FSReportObj>();
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("pl_report_summary", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@d1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@d2",
                    Value = date2,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var fs = new FSReportObj();

                    if (reader["GlMappingId"] != DBNull.Value)
                        fs.GlMappingId = int.Parse(reader["GlMappingId"].ToString());

                    if (reader["Caption"] != DBNull.Value)
                        fs.Caption = (reader["Caption"].ToString());

                    if (reader["SubCaption"] != DBNull.Value)
                        fs.SubCaption = (reader["SubCaption"].ToString());

                    if (reader["CompanyId"] != DBNull.Value)
                        fs.CompanyId = int.Parse(reader["CompanyId"].ToString());

                    if (reader["CompanyName"] != DBNull.Value)
                        fs.CompanyName = (reader["CompanyName"].ToString());

                    if (reader["SubPosition"] != DBNull.Value)
                        fs.SubPosition = int.Parse(reader["SubPosition"].ToString());

                    if (reader["GlCode"] != DBNull.Value)
                        fs.GlCode = (reader["GlCode"].ToString());

                    if (reader["ParentCaption"] != DBNull.Value)
                        fs.ParentCaption = (reader["ParentCaption"].ToString());

                    if (reader["Position"] != DBNull.Value)
                        fs.Position = int.Parse(reader["Position"].ToString());

                    if (reader["AccountType"] != DBNull.Value)
                        fs.AccountType = (reader["AccountType"].ToString());
                    if (reader["AccountTypeId"] != DBNull.Value)
                        fs.AccountTypeId = int.Parse(reader["AccountTypeId"].ToString());
                    if (reader["StatementType"] != DBNull.Value)
                        fs.StatementType = (reader["StatementType"].ToString());
                    if (reader["StatementTypeId"] != DBNull.Value)
                        fs.StatementTypeId = int.Parse(reader["StatementTypeId"].ToString());
                    if (reader["SubGlCode"] != DBNull.Value)
                        fs.SubGlCode = (reader["SubGlCode"].ToString());
                    if (reader["SubGlName"] != DBNull.Value)
                        fs.SubGlName = (reader["SubGlName"].ToString());
                    if (reader["GlId"] != DBNull.Value)
                        fs.GlId = int.Parse(reader["GlId"].ToString());
                    if (reader["CB"] != DBNull.Value)
                        fs.CB = decimal.Parse(reader["CB"].ToString());
                    if (reader["SubGlCode"] != DBNull.Value)
                        fs.SubGlCode1 = (reader["SubGlCode"].ToString());
                    if (reader["RunDate"] != DBNull.Value)
                        fs.RunDate = DateTime.Parse(reader["RunDate"].ToString()).Date;
                    if (reader["PreRunDate"] != DBNull.Value)
                        fs.PreRunDate = DateTime.Parse(reader["PreRunDate"].ToString()).Date;
                    fsreport.Add(fs);
                }
                con.Close();
            }
            return fsreport;
        }




        ///Investment Reports
        public List<CorporateInvestorCustomerObj> GetInvestmentCustomerCorporate(DateTime? date1, DateTime? date2, int ct)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            var corporateInvestorCustmer = new List<CorporateInvestorCustomerObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("investment_customer_get_all", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@customerType",
                    Value = ct,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new CorporateInvestorCustomerObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["CustomerID"] != DBNull.Value)
                        cc.CustomerID = int.Parse(reader["CustomerID"].ToString());

                    if (reader["DateOfIncorporation"] != DBNull.Value)
                        cc.DateOfIncorporation = DateTime.Parse(reader["DateOfIncorporation"].ToString());

                    if (reader["CompanyName"] != DBNull.Value)
                        cc.CompanyName = (reader["CompanyName"].ToString());

                    if (reader["Industry"] != DBNull.Value)
                        cc.Industry = (reader["Industry"].ToString());

                    if (reader["Size"] != DBNull.Value)
                        cc.Size = (reader["Size"].ToString());

                    if (reader["PhoneNumber"] != DBNull.Value)
                        cc.PhoneNumber = (reader["PhoneNumber"].ToString());

                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());

                    if (reader["PoliticallyExposed"] != DBNull.Value)
                        cc.PoliticallyExposed = (reader["PoliticallyExposed"].ToString());

                    if (reader["CurrentBalance"] != DBNull.Value)
                        cc.CurrentBalance = (reader["CurrentBalance"].ToString());

                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());

                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());
                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());
                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = (reader["Sum"].ToString());
                    corporateInvestorCustmer.Add(cc);
                }
                con.Close();
            }
            foreach (var item in corporateInvestorCustmer)
            {
                item.RelationshipOfficer = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
            }
            return corporateInvestorCustmer;
        }
        public List<IndividualInvestorCustomerObj> GetInvestmentCustomerIndividual(DateTime? date1, DateTime? date2, int ct)
        {
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            var genderLists = _serverRequest.GetGenderAsync().Result;
            var maritalLists = _serverRequest.GetMaritalStatusAsync().Result;
            var employmentTypeLists = _serverRequest.GetEmploymentTypeAsync().Result;
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var investmentCustomerIndividual = new List<IndividualInvestorCustomerObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("investment_customer_get_all", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@customerType",
                    Value = ct,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new IndividualInvestorCustomerObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["CustomerID"] != DBNull.Value)
                        cc.CustomerID = int.Parse(reader["CustomerID"].ToString());

                    if (reader["CustomerName"] != DBNull.Value)
                        cc.CustomerName = (reader["CustomerName"].ToString());

                    if (reader["DateofBirth"] != DBNull.Value)
                        cc.DateofBirth = DateTime.Parse(reader["DateofBirth"].ToString());

                    if (reader["GenderId"] != DBNull.Value)
                        cc.GenderId = int.Parse(reader["GenderId"].ToString());

                    if (reader["MaritalStatusId"] != DBNull.Value)
                        cc.MaritalStatusId = int.Parse(reader["MaritalStatusId"].ToString());

                    if (reader["PhoneNumber"] != DBNull.Value)
                        cc.PhoneNumber = (reader["PhoneNumber"].ToString());

                    if (reader["EmailAddress"] != DBNull.Value)
                        cc.EmailAddress = (reader["EmailAddress"].ToString());

                    if (reader["CustomerAddress"] != DBNull.Value)
                        cc.CustomerAddress = (reader["CustomerAddress"].ToString());

                    if (reader["NextOfKin"] != DBNull.Value)
                        cc.NextOfKin = (reader["NextOfKin"].ToString());

                    if (reader["CurrentBalance"] != DBNull.Value)
                        cc.CurrentBalance = (reader["CurrentBalance"].ToString());

                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());

                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());

                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());
                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());
                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = (reader["Sum"].ToString());
                    investmentCustomerIndividual.Add(cc);
                }
                con.Close();
            }
            foreach (var item in investmentCustomerIndividual)
            {
                item.AccountOfficer = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
                item.MaritalStatus = maritalLists.commonLookups.FirstOrDefault(x => x.LookupId == item.MaritalStatusId)?.LookupName;
                item.Gender = genderLists.commonLookups.FirstOrDefault(x => x.LookupId == item.GenderId)?.LookupName;
            }
            return investmentCustomerIndividual;
        }
        public List<InvestmentObj> GetInvestment(DateTime? date1, DateTime? date2)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            var investment = new List<InvestmentObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("investment_get_all", con);               
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new InvestmentObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["InvestmentID"] != DBNull.Value)
                        cc.InvestmentID = (reader["InvestmentID"].ToString());

                    if (reader["ProductName"] != DBNull.Value)
                        cc.ProductName = (reader["ProductName"].ToString());

                    if (reader["LinkedAccountNumber"] != DBNull.Value)
                        cc.LinkedAccountNumber = (reader["LinkedAccountNumber"].ToString());

                    if (reader["CustomerName"] != DBNull.Value)
                        cc.CustomerName = (reader["CustomerName"].ToString());

                    if (reader["Industry"] != DBNull.Value)
                        cc.Industry = (reader["Industry"].ToString());

                    if (reader["InvestmentDate"] != DBNull.Value)
                        cc.InvestmentDate = DateTime.Parse(reader["InvestmentDate"].ToString());

                    if (reader["MaturityDate"] != DBNull.Value)
                        cc.MaturityDate = DateTime.Parse(reader["MaturityDate"].ToString());

                    if (reader["InvestmentAmount"] != DBNull.Value)
                        cc.InvestmentAmount = (reader["InvestmentAmount"].ToString());

                    if (reader["ApprovedRate"] != DBNull.Value)
                        cc.ApprovedRate = decimal.Parse(reader["ApprovedRate"].ToString());

                    if (reader["ApprovedTenor"] != DBNull.Value)
                        cc.ApprovedTenor = decimal.Parse(reader["ApprovedTenor"].ToString());

                    if (reader["NoOfDaysToMaturity"] != DBNull.Value)
                        cc.NoOfDaysToMaturity = int.Parse(reader["NoOfDaysToMaturity"].ToString());

                    if (reader["InvestmentStatus"] != DBNull.Value)
                        cc.InvestmentStatus = (reader["InvestmentStatus"].ToString());

                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());

                    if (reader["TotalInterest"] != DBNull.Value)
                        cc.TotalInterest = decimal.Parse(reader["TotalInterest"].ToString());

                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());

                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());
                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());
                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = (reader["Sum"].ToString());
                    investment.Add(cc);
                }
                con.Close();
            }
            foreach(var item in investment)
            {
                item.AccountOfficer = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
                item.InvestmentStatus = item.InvestmentStatus == "0" ? "Pending": item.InvestmentStatus == "1" ? "Running" : item.InvestmentStatus == "2" ? "Matured" : item.InvestmentStatus == "3" ? "Liquidated" : null;
            }

            return investment;
        }
        public List<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleInvestmentCertificate(string RefNumber)
        {
            var InvestorFundId = (from a in _context.inf_investorfund where a.RefNumber == RefNumber select a.InvestorFundId).FirstOrDefault();
            var data = (from a in _context.inf_investdailyschedule
                        join b in _context.inf_investorfund on a.InvestorFundId equals b.InvestorFundId
                        where a.InvestorFundId == InvestorFundId && a.EndPeriod == true
                        select new LoanPaymentSchedulePeriodicObj
                        {
                            LoanId = a.InvestorFundId ?? 0,
                            PaymentNumber = a.PeriodId ?? 0,
                            PaymentDate = a.PeriodDate.Value.Date,
                            StartPrincipalAmount = (double)a.OB,
                            PeriodPaymentAmount = (double)a.Repayment,
                            PeriodInterestAmount = (double)a.InterestAmount,
                            PeriodPrincipalAmount = (double)a.InterestAmount,
                            EndPrincipalAmount = (double)a.CB,
                            InterestRate = (double)b.ApprovedRate,

                            AmortisedStartPrincipalAmount = (double)a.InterestAmount,
                            AmortisedPeriodPaymentAmount = (double)a.InterestAmount,
                            AmortisedPeriodInterestAmount = (double)a.InterestAmount,
                            AmortisedPeriodPrincipalAmount = (double)a.InterestAmount,
                            AmortisedEndPrincipalAmount = (double)a.InterestAmount,
                            EffectiveInterestRate = (double)b.ApprovedRate,

                        }).ToList();
            return data;
        }
        public OfferLetterDetailObj GenerateInvestmentCertificate(string RefNumber)
        {
            var _Companies = _serverRequest.GetAllCompanyAsync().Result;
            var _Currencies = _serverRequest.GetCurrencyAsync().Result;
            var offerLetterDetails = (from a in _context.inf_investorfund
                                      join b in _context.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                      where a.RefNumber == RefNumber && a.Deleted == false &&
                                      a.ApprovalStatus == 2
                                      select new OfferLetterDetailObj
                                      {
                                          ProductName = _context.inf_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                          CustomerName = b.FirstName + " " + b.LastName,
                                          Tenor = (int)a.ApprovedTenor,
                                          InterestRate = (double)a.ApprovedRate,
                                          LoanAmount = a.ApprovedAmount ?? 0,
                                          ExchangeRate = 0,
                                          CustomerAddress = b.Address ?? string.Empty,
                                          CustomerEmailAddress = b.Email,
                                          CustomerPhoneNumber = b.PhoneNo,
                                          ApplicationDate = a.CreatedOn ?? DateTime.Now,
                                          LoanApplicationId = a.RefNumber,
                                          RepaymentSchedule = "Not applicable",
                                          RepaymentTerms = "Not applicable",
                                          Purpose = a.InvestmentPurpose
                                      }).FirstOrDefault();

            offerLetterDetails.CompanyName = _Companies.companyStructures.FirstOrDefault(x => x.companyStructureId == offerLetterDetails.CompanyId)?.name;
            offerLetterDetails.CurrencyName = _Currencies.commonLookups.FirstOrDefault(x => x.LookupId == offerLetterDetails.CurrencyId)?.LookupName;
            return offerLetterDetails;
        }

        ///Credit Reports
        public List<CorporateCustomerObj> GetCreditCustomerCorporate(DateTime? date1, DateTime? date2, int ct)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            var corporateCustmer = new List<CorporateCustomerObj>();
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            SqlConnection connection = new SqlConnection(connectionString);
            using (var con = connection)
            {
                var cmd = new SqlCommand("credit_customer_get_all", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@customerType",
                    Value = ct,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new CorporateCustomerObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["CustomerId"] != DBNull.Value)
                        cc.CustomerId = int.Parse(reader["CustomerId"].ToString());

                    if (reader["CompanyName"] != DBNull.Value)
                        cc.CompanyName = (reader["CompanyName"].ToString());

                    if (reader["DateOfIncorporation"] != DBNull.Value)
                        cc.DateOfIncorporation = DateTime.Parse(reader["DateOfIncorporation"].ToString());

                    if (reader["RegistrationNumber"] != DBNull.Value)
                        cc.RegistrationNumber = (reader["RegistrationNumber"].ToString());

                    if (reader["PhoneNumber"] != DBNull.Value)
                        cc.PhoneNumber = (reader["PhoneNumber"].ToString());

                    if (reader["Email"] != DBNull.Value)
                        cc.Email = (reader["Email"].ToString());

                    if (reader["Address"] != DBNull.Value)
                        cc.Address = (reader["Address"].ToString());

                    if (reader["CompanyDirector"] != DBNull.Value)
                        cc.CompanyDirector = (reader["CompanyDirector"].ToString());

                    if (reader["PoliticallyExposed"] != DBNull.Value)
                        cc.PoliticallyExposed = (reader["PoliticallyExposed"].ToString());
                    if (reader["Industry"] != DBNull.Value)
                        cc.Industry = (reader["Industry"].ToString());
                    if (reader["IncorporationCountry"] != DBNull.Value)
                        cc.IncorporationCountry = (reader["IncorporationCountry"].ToString());
                    if (reader["AnnualTurnover"] != DBNull.Value)
                        cc.AnnualTurnover = decimal.Parse(reader["AnnualTurnover"].ToString());
                    if (reader["CurrentExposure"] != DBNull.Value)
                        cc.CurrentExposure = decimal.Parse(reader["CurrentExposure"].ToString());
                    if (reader["ExposureLimit"] != DBNull.Value)
                        cc.ExposureLimit = decimal.Parse(reader["ExposureLimit"].ToString());
                    if (reader["ShareholderFund"] != DBNull.Value)
                        cc.ShareholderFund = decimal.Parse(reader["ShareholderFund"].ToString());
                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());
                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());
                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = (reader["Sum"].ToString());
                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());
                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());
                    if (reader["CreatedOn"] != DBNull.Value)
                        cc.CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString());
                    corporateCustmer.Add(cc);
                }
                con.Close();
            }
            foreach (var item in corporateCustmer)
            {
                item.RelationshipManager = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
            }

            return corporateCustmer;
        }
        public List<IndividualCustomerObj> GetCreditCustomerIndividual(DateTime? date1, DateTime? date2, int ct)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            var genderLists = _serverRequest.GetGenderAsync().Result;
            var maritalLists = _serverRequest.GetMaritalStatusAsync().Result;
            var employmentTypeLists = _serverRequest.GetEmploymentTypeAsync().Result;
            var individualCustomer = new List<IndividualCustomerObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("credit_customer_get_all", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@customerType",
                    Value = ct,
                });

                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new IndividualCustomerObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["CustomerId"] != DBNull.Value)
                        cc.CustomerId = int.Parse(reader["CustomerId"].ToString());

                    if (reader["CustomerName"] != DBNull.Value)
                        cc.CustomerName = (reader["CustomerName"].ToString());

                    if (reader["DateOfBirth"] != DBNull.Value)
                        cc.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());

                    if (reader["GenderId"] != DBNull.Value)
                        cc.GenderId = int.Parse(reader["GenderId"].ToString());

                    if (reader["MaritalStatusId"] != DBNull.Value)
                        cc.MaritalStatusId = int.Parse(reader["MaritalStatusId"].ToString());

                    if (reader["PhoneNo"] != DBNull.Value)
                        cc.PhoneNo = (reader["PhoneNo"].ToString());

                    if (reader["Email"] != DBNull.Value)
                        cc.Email = (reader["Email"].ToString());

                    if (reader["Address"] != DBNull.Value)
                        cc.Address = (reader["Address"].ToString());

                    if (reader["NextOfKin"] != DBNull.Value)
                        cc.NextOfKin = (reader["NextOfKin"].ToString());
                    if (reader["EmploymentType"] != DBNull.Value)
                        cc.EmploymentType = int.Parse(reader["EmploymentType"].ToString());
                    if (reader["Employer"] != DBNull.Value)
                        cc.Employer = (reader["Employer"].ToString());
                    if (reader["CurrentExposure"] != DBNull.Value)
                        cc.CurrentExposure = decimal.Parse(reader["CurrentExposure"].ToString());
                    if (reader["ExposureLimit"] != DBNull.Value)
                        cc.ExposureLimit = decimal.Parse(reader["ExposureLimit"].ToString());
                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());
                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());
                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = (reader["Sum"].ToString());
                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());
                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());
                    if (reader["CreatedOn"] != DBNull.Value)
                        cc.CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString());
                    individualCustomer.Add(cc);
                }
                con.Close();
            }
            foreach(var item in individualCustomer)
            {
                item.RelationshipManager = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
                item.MaritalStatus = maritalLists.commonLookups.FirstOrDefault(x => x.LookupId == item.MaritalStatusId)?.LookupName;
                item.Gender = genderLists.commonLookups.FirstOrDefault(x => x.LookupId == item.GenderId)?.LookupName;
                item.EmploymentStatus = employmentTypeLists.commonLookups.FirstOrDefault(x => x.LookupId == item.EmploymentType)?.LookupName;
            }
            return individualCustomer;
        }
        public List<LoansObj> GetLoan(DateTime? date1, DateTime? date2)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var loan = new List<LoansObj>();
            var staffLists = _serverRequest.GetAllStaffAsync().Result;
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("loan_get_all", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date1",
                    Value = date1,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@date2",
                    Value = date2,
                });


                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cc = new LoansObj();

                    if (reader["SN"] != DBNull.Value)
                        cc.SN = int.Parse(reader["SN"].ToString());

                    if (reader["LoanRefNumber"] != DBNull.Value)
                        cc.LoanRefNumber = (reader["LoanRefNumber"].ToString());

                    if (reader["ProductName"] != DBNull.Value)
                        cc.ProductName = (reader["ProductName"].ToString());

                    if (reader["LinkedAccountNumber"] != DBNull.Value)
                        cc.LinkedAccountNumber = (reader["LinkedAccountNumber"].ToString());

                    if (reader["CustomerName"] != DBNull.Value)
                        cc.CustomerName = (reader["CustomerName"].ToString());

                    if (reader["Industry"] != DBNull.Value)
                        cc.Industry = (reader["Industry"].ToString());

                    if (reader["DisbursementDate"] != DBNull.Value)
                        cc.DisbursementDate = DateTime.Parse(reader["DisbursementDate"].ToString());

                    if (reader["MaturityDate"] != DBNull.Value)
                        cc.MaturityDate = DateTime.Parse(reader["MaturityDate"].ToString());

                    if (reader["ApplicationAmount"] != DBNull.Value)
                        cc.ApplicationAmount = decimal.Parse(reader["ApplicationAmount"].ToString());

                    if (reader["DisbursedAmount"] != DBNull.Value)
                        cc.DisbursedAmount = decimal.Parse(reader["DisbursedAmount"].ToString());

                    if (reader["ApplicationAmount"] != DBNull.Value)
                        cc.ApplicationAmount = decimal.Parse(reader["ApplicationAmount"].ToString());

                    if (reader["Tenor"] != DBNull.Value)
                        cc.Tenor = decimal.Parse(reader["Tenor"].ToString());

                    if (reader["InterestRate"] != DBNull.Value)
                        cc.InterestRate = decimal.Parse(reader["InterestRate"].ToString());

                    if (reader["TotalInterest"] != DBNull.Value)
                        cc.TotalInterest = decimal.Parse(reader["TotalInterest"].ToString());

                    if (reader["NoOfDaysInOverdue"] != DBNull.Value)
                        cc.NoOfDaysInOverdue = int.Parse(reader["NoOfDaysInOverdue"].ToString());

                    if (reader["ProvisioningRequirement"] != DBNull.Value)
                        cc.ProvisioningRequirement = int.Parse(reader["ProvisioningRequirement"].ToString());

                    if (reader["Description"] != DBNull.Value)
                        cc.Description = (reader["Description"].ToString());


                    if (reader["RepaymentDate"] != DBNull.Value)
                        cc.RepaymentDate = DateTime.Parse(reader["RepaymentDate"].ToString());

                    if (reader["RepaymentAmount"] != DBNull.Value)
                        cc.RepaymentAmount = decimal.Parse(reader["RepaymentAmount"].ToString());

                    if (reader["PAR"] != DBNull.Value)
                        cc.PAR = decimal.Parse(reader["PAR"].ToString());

                    if (reader["OutstandingPrincipal"] != DBNull.Value)
                        cc.OutstandingPrincipal = decimal.Parse(reader["OutstandingPrincipal"].ToString());

                    if (reader["OutstandingInterest"] != DBNull.Value)
                        cc.OutstandingInterest = decimal.Parse(reader["OutstandingInterest"].ToString());

                    if (reader["TotalOutstanding"] != DBNull.Value)
                        cc.TotalOutstanding = decimal.Parse(reader["TotalOutstanding"].ToString());

                    if (reader["PastDuePrincipal"] != DBNull.Value)
                        cc.PastDuePrincipal = decimal.Parse(reader["PastDuePrincipal"].ToString());

                    if (reader["PastDueInterest"] != DBNull.Value)
                        cc.PastDueInterest = decimal.Parse(reader["PastDueInterest"].ToString());

                    if (reader["RelationshipManagerId"] != DBNull.Value)
                        cc.RelationshipManagerId = int.Parse(reader["RelationshipManagerId"].ToString());

                    if (reader["Total"] != DBNull.Value)
                        cc.Total = (reader["Total"].ToString());

                    if (reader["Sum"] != DBNull.Value)
                        cc.Sum = decimal.Parse(reader["Sum"].ToString());
                    if (reader["From"] != DBNull.Value)
                        cc.From = DateTime.Parse(reader["From"].ToString());
                    if (reader["To"] != DBNull.Value)
                        cc.To = DateTime.Parse(reader["To"].ToString());

                    loan.Add(cc);
                }
                con.Close();
            }
            foreach(var item in loan)
            {
                item.AccountOfficer = staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.firstName + " " + staffLists.staff.FirstOrDefault(x => x.staffId == item.RelationshipManagerId)?.lastName;
            }
            return loan;
        }
    }
}
