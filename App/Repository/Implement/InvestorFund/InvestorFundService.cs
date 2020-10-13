using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Finance;
using Banking.Contracts.Response.FlutterWave;
using Banking.Contracts.Response.InvestorFund;
using Banking.Contracts.Response.Mail;
using Banking.Data;
using Banking.DomainObjects.InvestorFund;
using Banking.Helpers;
using Banking.Repository.Interface.InvestorFund;
using Banking.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Repository.Implement.InvestorFund
{
    public class InvestorFundService : IInvestorFundService

    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IFlutterWaveRequest _flutter;
        private readonly IConfiguration _configuration;
        private string GenerateLoanReferenceNumber()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            double unixTime = (int)unixTicks.TotalSeconds;
            return unixTime.ToString();
        }
        public InvestorFundService(DataContext dataContext, IIdentityService identityService, IConfiguration configuration, IIdentityServerRequest serverRequest, IFlutterWaveRequest flutter)
        {
            _dataContext = dataContext;
            _identityService = identityService;
            _serverRequest = serverRequest;
            _flutter = flutter;
            _configuration = configuration;
        }

        #region InvestorFund

        public async Task<InvestorFundRespObj> UpdateInvestorFund(InvestorFundObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                if (entity == null) return null;
                if (entity.WebsiteInvestorFundId > 0)
                {
                    var oldApp = _dataContext.inf_investorfund_website.FirstOrDefault(x => x.WebsiteInvestorFundId == entity.WebsiteInvestorFundId);
                    oldApp.ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                var customer = _dataContext.credit_loancustomer.Find(entity.InvestorFundCustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new InvestorFundRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                var mdata = entity.S_date ?? DateTime.Now;
                entity.MaturityDate = mdata.AddDays((int)entity.ProposedTenor);
                inf_investorfund investmentExist = null;
                if (entity.InvestorFundId > 0)
                {
                    investmentExist = _dataContext.inf_investorfund.Where(x => x.InvestorFundId == entity.InvestorFundId).FirstOrDefault();
                    if (investmentExist != null)
                    {
                        investmentExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        investmentExist.ApprovalStatus = entity.ApprovalStatus;
                        investmentExist.RefNumber = entity.RefNumber;
                        investmentExist.ProductId = entity.ProductId;
                        investmentExist.ProposedTenor = entity.ProposedTenor;
                        investmentExist.ProposedRate = entity.ProposedRate;
                        investmentExist.ApprovedTenor = entity.ProposedTenor;
                        investmentExist.ApprovedProductId = entity.ProductId;
                        investmentExist.ApprovedRate = entity.ProposedRate;
                        investmentExist.ApprovedAmount = entity.ProposedAmount;
                        investmentExist.MaturityDate = entity.MaturityDate;
                        investmentExist.FrequencyId = entity.FrequencyId;
                        investmentExist.Period = entity.Period;
                        investmentExist.ProposedAmount = entity.ProposedAmount;
                        investmentExist.CurrencyId = entity.CurrencyId;
                        investmentExist.EffectiveDate = entity.EffectiveDate;
                        investmentExist.InvestmentPurpose = entity.InvestmentPurpose;
                        investmentExist.EnableRollOver = entity.EnableRollOver;
                        investmentExist.InstrumentId = entity.InstrumentId;
                        investmentExist.InstrumentNumer = entity.InstrumentNumer;
                        investmentExist.InstrumentDate = entity.InstrumentDate;
                        investmentExist.CompanyId = entity.CompanyId;
                        investmentExist.Active = true;
                        investmentExist.Deleted = false;
                        investmentExist.UpdatedBy = entity.CreatedBy;
                        investmentExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var refNo = GenerateLoanReferenceNumber();

                    investmentExist = new inf_investorfund
                    {
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ApprovalStatus = entity.ApprovalStatus,
                        InvestmentStatus = 0,
                        RefNumber = refNo,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        ApprovedRate = entity.ProposedRate,
                        ApprovedAmount = entity.ProposedAmount,
                        ApprovedTenor = entity.ProposedTenor,
                        ApprovedProductId = entity.ProductId,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.S_date,
                        MaturityDate = entity.MaturityDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        EnableRollOver = entity.EnableRollOver,
                        InstrumentId = entity.InstrumentId,
                        InstrumentNumer = entity.InstrumentNumer,
                        InstrumentDate = entity.InstrumentDate,
                        CompanyId = entity.CompanyId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.inf_investorfund.Add(investmentExist);
                }
                try
                {
                    using (var _trans = _dataContext.Database.BeginTransaction())
                    {
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                            subject = "Successful Investment Application",
                            content = $"Hi {customer.FirstName}, <br> Your application is successful and awaiting approval.",
                            sendIt = true,
                        });

                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                            subject = "Successful Investment Application",
                            content = $"Hi {staff.firstName}, <br> Appraisal is pending for Investment application with reference : " + investmentExist.RefNumber + "",
                            sendIt = true,
                        });
                        var output = _dataContext.SaveChanges() > 0;
                        var targetIds = new List<int>();
                        targetIds.Add(investmentExist.InvestorFundId);
                        var request = new GoForApprovalRequest
                        {
                            StaffId = user.StaffId,
                            CompanyId = 1,
                            StatusId = (int)ApprovalStatus.Pending,
                            TargetId = targetIds,
                            Comment = "Investment Application",
                            OperationId = (int)OperationsEnum.InvestorFundApproval,
                            DeferredExecution = true, // false by default will call the internal SaveChanges() 
                            ExternalInitialization = true,
                            EmailNotification = true,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(request);
                        try
                        {
                            if (!result.IsSuccessStatusCode)
                            {
                                new ApprovalRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                    }
                                };
                            }
                            var stringData = await result.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                            if (res.ApprovalProcessStarted)
                            {
                                investmentExist.WorkflowToken = res.Status.CustomToken;
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                            if (res.EnableWorkflow || !res.HasWorkflowAccess)
                            {
                                await _trans.RollbackAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = false,
                                        Message = res.Status.Message
                                    }
                                };
                            }
                            if (!res.EnableWorkflow)
                            {
                                var response = InvestmentApproval(investmentExist.InvestorFundId, 2);
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = true,
                                        Message = new APIResponseMessage
                                        {
                                            FriendlyMessage = "Successful"
                                        }
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            _trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = " Not Successful"
                            }
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }      

        public async Task<InvestorFundRespObj> UpdateRollOverInvestorFund(InvestorFundObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                if (entity == null) return null;
                if (entity.InvestorFundIdWebsiteRolloverId > 0)
                {
                    var oldApp = _dataContext.inf_investorfund_rollover_website.FirstOrDefault(x => x.InvestorFundIdWebsiteRolloverId == entity.InvestorFundIdWebsiteRolloverId);
                    oldApp.ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                var mdata = entity.EffectiveDate ?? DateTime.Now;
                entity.MaturityDate = mdata.AddDays((int)entity.ProposedTenor);
                inf_investorfund investmentExist = null;
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundCustomerId);
                if (entity.InvestorFundId > 0)
                {
                    investmentExist = _dataContext.inf_investorfund.FirstOrDefault(x => x.InvestorFundId == entity.InvestorFundId);
                    if (investmentExist != null)
                    {
                        investmentExist.ProposedTenor = entity.ProposedTenor;
                        investmentExist.ApprovedTenor = entity.ProposedTenor;
                        investmentExist.ApprovedAmount = entity.ProposedAmount;                       
                        investmentExist.ProposedAmount = entity.ProposedAmount;
                        investmentExist.MaturityDate = entity.MaturityDate;
                        investmentExist.EffectiveDate = entity.EffectiveDate;
                        investmentExist.ApprovalStatus = 1;
                        investmentExist.InvestmentStatus = 0;
                        investmentExist.CreatedOn = DateTime.Now;

                        var topUpList = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == entity.InvestorFundId).ToList();
                        if (topUpList.Count() > 0)
                        {
                            _dataContext.inf_investdailyschedule_topup.RemoveRange(topUpList);
                        }
                    }
                }

                try
                {
                    using (var _trans = _dataContext.Database.BeginTransaction())
                    {
                        var output = _dataContext.SaveChanges() > 0;
                        var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                            subject = "Successful RollOver Application",
                            content = $"Hi {customer.FirstName}, <br> Your rollover application is successful and awaiting approval.",
                            sendIt = true,
                        });
                        var targetIds = new List<int>();
                        targetIds.Add(investmentExist.InvestorFundId);
                        var request = new GoForApprovalRequest
                        {
                            StaffId = user.StaffId,
                            CompanyId = 1,
                            StatusId = (int)ApprovalStatus.Pending,
                            TargetId = targetIds,
                            Comment = "RollOver Investment Application",
                            OperationId = (int)OperationsEnum.InvestorFundApproval,
                            DeferredExecution = true, // false by default will call the internal SaveChanges() 
                            ExternalInitialization = true,
                            EmailNotification = true,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(request);
                        try
                        {
                            if (!result.IsSuccessStatusCode)
                            {
                                new ApprovalRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                    }
                                };
                            }
                            var stringData = await result.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                            if (res.ApprovalProcessStarted)
                            {
                                investmentExist.WorkflowToken = res.Status.CustomToken;
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                            if (res.EnableWorkflow || !res.HasWorkflowAccess)
                            {
                                await _trans.RollbackAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = false,
                                        Message = res.Status.Message
                                    }
                                };
                            }
                            if (!res.EnableWorkflow)
                            {
                                var response = InvestmentApproval(investmentExist.InvestorFundId, 2);
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new InvestorFundRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = true,
                                        Message = new APIResponseMessage
                                        {
                                            FriendlyMessage = "Successful"
                                        }
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            _trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = " Not Successful"
                            }
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InvestorFundRespObj> UpdateTopUpInvestorFund(InvestorFundObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                if (entity == null) return null;
                if (entity.InvestorFundIdWebsiteTopupId > 0)
                {
                    var oldApp = _dataContext.inf_investorfund_topup_website.FirstOrDefault(x => x.InvestorFundIdWebsiteTopupId == entity.InvestorFundIdWebsiteTopupId);
                    oldApp.ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                inf_investorfund investmentExist = null;
                var refNo = GenerateLoanReferenceNumber();
                if (entity.InvestorFundId > 0)
                {
                    investmentExist = _dataContext.inf_investorfund.FirstOrDefault(x => x.InvestorFundId == entity.InvestorFundId);
                    if (investmentExist != null)
                    {
                        var now = DateTime.Now.Date;
                        var remainingTenor = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == entity.InvestorFundId && x.PeriodDate.Value.Date > now.Date).ToList();
                        investmentExist.ApprovedAmount = investmentExist.ApprovedAmount + entity.ProposedAmount;
                        _dataContext.SaveChanges();

                        GenerateInvestmentDailyTopUpSchedule(investmentExist.InvestorFundId, entity.ProposedAmount??0, refNo, remainingTenor.Count());
                    }
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Topup Successful"
                            }
                        }
                    };
                }
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unsuccessful"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult<InvestorFundRespObj>> GetInvestmentForAppraisalAsync()
        {
            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" }
                        }
                    };
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return new InvestorFundRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    };

                }
                var investment = await GetInvestmentAwaitingApprovalAsync(res.workflowTasks.Select(x => x.TargetId).ToList(), res.workflowTasks.Select(d => d.WorkflowToken).ToList());


                return new InvestorFundRespObj
                {
                    InvestmentLists = investment.Select(a => new InvestmentListObj
                    {
                        ApplicationDate = (DateTime)a.CreatedOn,
                        InvestorFundCustomerId = a.InvestorFundCustomerId,
                        ProposedAmount = a.ProposedAmount,
                        InvestorFundId = a.InvestorFundId,
                        InvestorName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).LastName,
                        ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                        ProductId = a.ProductId,
                        ApprovalStatus = a.ApprovalStatus,
                        RefNumber = a.RefNumber,
                        workflowToken = a.WorkflowToken
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = investment.Count() < 1 ? "No Investment awaiting approvals" : null
                        }
                    }
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private async Task<IEnumerable<inf_investorfund>> GetInvestmentAwaitingApprovalAsync(List<int> InvestfundIds, List<string> tokens)
        {
            var item = await _dataContext.inf_investorfund
                .Where(s => InvestfundIds.Contains(s.InvestorFundId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public bool DeleteInvestorFund(int id)
        {
            var itemToDelete = _dataContext.inf_investorfund.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportInvestorFund()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Customer");
            dt.Columns.Add("Product");
            dt.Columns.Add("Proposed Tenor");
            dt.Columns.Add("Proposed Rate");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Period");
            dt.Columns.Add("Proposed Amount");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Effective Date");
            dt.Columns.Add("Investment Purpose");
            dt.Columns.Add("Enable Roll Over");
            dt.Columns.Add("Instrument Id");
            dt.Columns.Add("Instrument Number");
            dt.Columns.Add("Instrument Date");
            //dt.Columns.Add("Companny");
            var statementType = (from a in _dataContext.inf_investorfund
                                 where a.Deleted == false
                                 select new InvestorFundObj
                                 {
                                     ProductId = a.ProductId ?? 0,
                                     InvestorFundCustomerId = a.InvestorFundCustomerId,
                                     ProposedTenor = a.ProposedTenor,
                                     ProposedRate = a.ProposedRate,
                                     FrequencyId = a.FrequencyId,
                                     Period = a.Period,
                                     ProposedAmount = a.ProposedAmount,
                                     CurrencyId = a.CurrencyId,
                                     EffectiveDate = a.EffectiveDate,
                                     InvestmentPurpose = a.InvestmentPurpose,
                                     EnableRollOver = a.EnableRollOver,
                                     InstrumentId = a.InstrumentId,
                                     InstrumentNumer = a.InstrumentNumer,
                                     InstrumentDate = a.InstrumentDate,
                                     //CompanyId = a.CompanyId,
                                 }).ToList();

            foreach (var kk in statementType)
            {
                var row = dt.NewRow();
                row["Product"] = kk.ProductId;
                row["Customer"] = kk.InvestorFundCustomerId;
                row["Proposed Tenor"] = kk.ProposedTenor;
                row["Proposed Rate"] = kk.ProposedRate;
                row["Frequency"] = kk.FrequencyId;
                row["Period"] = kk.Period;
                row["Proposed Amount"] = kk.ProposedAmount;
                row["Currency"] = kk.CurrencyId;
                row["Effective Date"] = kk.EffectiveDate;
                row["Investment Purpose"] = kk.InvestmentPurpose;
                row["Enable Roll Over"] = kk.EnableRollOver;
                row["Instrument Id"] = kk.InstrumentId;
                row["Instrument Number"] = kk.InstrumentNumer;
                row["Instrument Date"] = kk.InstrumentDate;
                //row["Company"] = kk.CompanyId;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Investor Fund");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<InvestorFundObj> GetInvestorFundAsync(int investorFundId)
        {
            try
            {
                var CurrencyList = await _serverRequest.GetCurrencyAsync();
                var staffResponse = await _serverRequest.GetAllStaffAsync();

                var _FrequencyType = await _dataContext.credit_frequencytype.Where(d => d.Deleted == false).ToListAsync();
                var _Product = await _dataContext.inf_product.Where(x => x.Deleted == false).ToListAsync();
                var _InvestDailySchdl = await _dataContext.inf_investdailyschedule.ToListAsync();


                var loanCust = _dataContext.credit_loancustomer.Where(d => d.Deleted == false).ToList();
                var investorFundObj = _dataContext.inf_investorfund.Where(s => loanCust.Select(d => d.CustomerId).Contains(s.InvestorFundCustomerId)).ToList();
                var productList = _dataContext.inf_product.Where(x => x.Deleted == false).ToList();
                var now = DateTime.Now;
              
                var a = investorFundObj.FirstOrDefault(a => a.InvestorFundId == investorFundId);
                var staffId = loanCust.FirstOrDefault(d => a.InvestorFundCustomerId == d.CustomerId)?.RelationshipManagerId;
                var lnApp = new InvestorFundObj()
                {
                    InvestorFundId = a.InvestorFundId,
                    InvestmentStatus = a.InvestmentStatus,
                    ProductId = a.ProductId ?? 0,
                    AccountNumber = loanCust.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).CASAAccountNumber,
                    ProductName = _Product.FirstOrDefault(x => x.ProductId == a.ProductId)?.ProductName,
                    TerminationCharge = _Product.FirstOrDefault(x => x.ProductId == a.ProductId)?.EarlyTerminationCharge,
                    InvestorFundCustomerId = a.InvestorFundCustomerId,
                    ProposedTenor = a.ProposedTenor,
                    ProposedRate = a.ProposedRate,
                    ApprovedRate = a.ApprovedRate,
                    ApprovedAmount = a.ApprovedAmount,
                    ApprovedTenor = a.ApprovedTenor,
                    ApprovedProductId = a.ProductId,
                    ApprovedProductName = _Product.FirstOrDefault(x => x.ProductId == a.ProductId)?.ProductName,
                    FrequencyId = a.FrequencyId,
                    Period = a.Period,
                    ProposedAmount = a.ProposedAmount,
                    CurrencyId = a.CurrencyId,
                    EffectiveDate = a.EffectiveDate,
                    MaturityDate = a.MaturityDate,
                    InvestmentPurpose = a.InvestmentPurpose,
                    FrequencyName = _FrequencyType.FirstOrDefault(x => x.FrequencyTypeId == _Product.FirstOrDefault(x => x.ProductId == a.ApprovedProductId)?.FrequencyId)?.Mode,
                    CurrencyName = CurrencyList.commonLookups.FirstOrDefault(d => d.LookupId == a.CurrencyId)?.LookupName ?? "",
                    EnableRollOver = a.EnableRollOver,
                    InstrumentId = a.InstrumentId,
                    InstrumentNumer = a.InstrumentNumer,
                    InstrumentDate = a.InstrumentDate,
                    CompanyId = a.CompanyId ?? 0,
                    RefNumber = a.RefNumber,
                    ApprovalStatus = a.ApprovalStatus,
                    InvestorName = loanCust.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).FirstName + " " + loanCust.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).LastName,
                    CustomerTypeId = loanCust.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).CustomerTypeId,
                    CustomerTypeName = loanCust.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).CustomerTypeId == 2 ? "Corporate" : "Individual",
                    Payout = _InvestDailySchdl.LastOrDefault(x => x.InvestorFundId == a.InvestorFundId)?.Repayment,
                    InterestEarned = _InvestDailySchdl.FirstOrDefault(x => x.InvestorFundId == a.InvestorFundId && x.PeriodDate.Value.Date == now.Date)?.CB,
                    //ExchangeRate = CurrencyList.commonLookups.FirstOrDefault(d => d.LookupId == a.CurrencyId)?.SellingRate,
                };
                if(staffId != null)
                {
                    lnApp.RelationshipManager = staffResponse.staff.FirstOrDefault(x => x.staffId == staffId)?.firstName + "  " + staffResponse.staff.FirstOrDefault(x => x.staffId == staffId)?.lastName;
                }
                var topUpList = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == a.InvestorFundId && x.PeriodDate.Value.Date == now.Date)
                        .Select(x => new { x.ReferenceNo, x.InvestorFundId, x.OB, x.Repayment, x.InterestAmount }).GroupBy(y => new { y.ReferenceNo, y.OB, y.Repayment, y.InterestAmount }, (key, group) => new
                        {
                            ReferenceNo = key.ReferenceNo,
                            InterestEarned = key.OB + key.InterestAmount,
                            Payout = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == a.InvestorFundId && x.ReferenceNo == key.ReferenceNo).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                        }).ToList();
                decimal? interestEarned = 0, payout = 0;
                if (topUpList.Count() > 0)
                {
                    foreach (var i in topUpList)
                    {
                        interestEarned = interestEarned + i.InterestEarned;
                        payout = payout + i.Payout;
                    }
                }
                lnApp.Payout = lnApp.Payout + payout;
                lnApp.InterestEarned = lnApp.InterestEarned + interestEarned;
                return lnApp;
            }
            catch (Exception ex)
            {
                throw new Exception("Error has Occurred", new Exception());
            }

        }

        public async Task<InvestorFundRegRespObj> UpdateInvestorFundByCustomer(InvestorFundObj entity)
        {
            try
            {
                if (entity == null) return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid resquest payload" } }
                };
                var webUrl = _configuration.GetValue<string>("FrontEndUrl:webUrl");
                var currencyList = _serverRequest.GetCurrencyAsync().Result;
                var customer = _dataContext.credit_loancustomer.Find(entity.InvestorFundCustomerId);
                var staffList = await _serverRequest.GetAllStaffAsync();               

                inf_investorfund_website collectionExist = new inf_investorfund_website();
                if (entity.InvestorFundId > 0)
                {
                    collectionExist = _dataContext.inf_investorfund_website.FirstOrDefault(x => x.WebsiteInvestorFundId == entity.WebsiteInvestorFundId);
                    if (collectionExist != null)
                    {
                        collectionExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        collectionExist.RefNumber = entity.RefNumber;
                        collectionExist.ProductId = entity.ProductId;
                        collectionExist.ProposedTenor = entity.ProposedTenor;
                        collectionExist.ProposedRate = entity.ProposedRate;
                        collectionExist.ApprovedTenor = entity.ProposedTenor;
                        collectionExist.ApprovedProductId = entity.ProductId;
                        collectionExist.ApprovedRate = entity.ProposedRate;
                        collectionExist.ApprovedAmount = entity.ProposedAmount;
                        collectionExist.FrequencyId = entity.FrequencyId;
                        collectionExist.Period = entity.Period;
                        collectionExist.ProposedAmount = entity.ProposedAmount;
                        collectionExist.CurrencyId = entity.CurrencyId;
                        collectionExist.EffectiveDate = entity.EffectiveDate;
                        collectionExist.InvestmentPurpose = entity.InvestmentPurpose;
                        collectionExist.EnableRollOver = entity.EnableRollOver;
                        collectionExist.InstrumentId = entity.InstrumentId;
                        collectionExist.InstrumentNumer = entity.InstrumentNumer;
                        collectionExist.InstrumentDate = entity.InstrumentDate;
                        collectionExist.CompanyId = entity.CompanyId;
                        collectionExist.Active = true;
                        collectionExist.Deleted = false;
                        collectionExist.Updatedby = "Customer";
                        collectionExist.Updatedon = DateTime.Now;
                    }
                }
                else
                {
                    var refNo = GenerateLoanReferenceNumber();

                    collectionExist = new inf_investorfund_website
                    {
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress,
                        InvestmentStatus = 0,
                        RefNumber = refNo,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        ApprovedRate = entity.ProposedRate,
                        ApprovedAmount = entity.ProposedAmount,
                        ApprovedTenor = entity.ProposedTenor,
                        ApprovedProductId = entity.ProductId,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.EffectiveDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        EnableRollOver = entity.EnableRollOver,
                        InstrumentId = entity.InstrumentId,
                        InstrumentNumer = entity.InstrumentNumer,
                        InstrumentDate = entity.InstrumentDate,
                        CompanyId = entity.CompanyId,
                        Active = true,
                        Deleted = false,
                        Createdby = "Customer",
                        Createdon = DateTime.Now,
                    };
                    _dataContext.inf_investorfund_website.Add(collectionExist);
                }
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    if (customer.RelationshipManagerId != null)
                    {
                        var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                            subject = "New Investment Application",
                            content = $"Hi {staff.firstName}, <br> A new investment request has been made by <br/> Investor : " + customer.FirstName + " " + customer.LastName + "",
                            sendIt = true,
                        });
                    }

                    var flutter = _serverRequest.GetFlutterWaveKeys().Result;
                    if (flutter.keys.useFlutterWave)
                    {
                        var currencyCode = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == entity.CurrencyId).Code;
                        var investor = _dataContext.credit_loancustomer.Find(entity.InvestorFundCustomerId);                     
                        var paymentObj = new PaymentObj
                        {
                            tx_ref = collectionExist.RefNumber,
                            currency = currencyCode,
                            amount = Convert.ToString(collectionExist.ApprovedAmount),
                            redirect_url = webUrl + "/#/dashboard",
                            payment_options = "card, mobilemoney, ussd, banktransfer",
                            customer = new Customer
                            {
                                name = investor.FirstName + " " + investor.LastName,
                                email = investor.Email,
                                phonenumber = investor.PhoneNo
                            },
                            customizations = new Customizations
                            {
                                title = "Investment Funding",
                                description = "Investment funding",
                                logo = "https://dl.dropboxusercontent.com/s/eu501mhy21003hh/gos2.png"
                            }
                        };
                        var res = _flutter.makePayment(paymentObj).Result;
                        if (res.status.ToLower() == "success")
                        {
                            return new InvestorFundRegRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } },
                                Link = res.data.link
                            };
                        }
                    }
                    return new InvestorFundRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } },
                        Link = webUrl + "/#/investment/all-investments"
                    };
                }
                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InvestorFundRegRespObj> UpdateInvestorFundRollOverByCustomer(InvestFundRollOverObj entity)
        {
            try
            {
                if (entity == null) return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid resquest payload" } }
                };
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                var staffList = await _serverRequest.GetAllStaffAsync();
                inf_investorfund_rollover_website itemExist = new inf_investorfund_rollover_website();
                if (entity.InvestorFundId > 0)
                {
                    itemExist = _dataContext.inf_investorfund_rollover_website.FirstOrDefault(x => x.InvestorFundIdWebsiteRolloverId == entity.InvestorFundIdWebsiteRolloverId);
                    if (itemExist != null)
                    {
                        itemExist.InvestorFundId = entity.InvestorFundId;
                        itemExist.ApprovedTenor = entity.ApprovedTenor;
                        itemExist.RollOverAmount = entity.RollOverAmount;
                        itemExist.EffectiveDate = entity.EffectiveDate;
                        itemExist.Active = true;
                        itemExist.Deleted = false;
                        itemExist.UpdatedBy = "Customer";
                        itemExist.UpdatedOn = DateTime.Now;
                    }
                    else
                    {
                        itemExist = new inf_investorfund_rollover_website
                        {
                            InvestorFundId = entity.InvestorFundId,
                            ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress,
                            RollOverAmount = entity.RollOverAmount,
                            ApprovedTenor = entity.ApprovedTenor,
                            EffectiveDate = entity.EffectiveDate,
                            Active = true,
                            Deleted = false,
                            CreatedBy = "Customer",
                            CreatedOn = DateTime.Now,
                        };
                        _dataContext.inf_investorfund_rollover_website.Add(itemExist);
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    if (customer.RelationshipManagerId != null)
                    {
                        var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                            subject = "New RollOver Request",
                            content = $"Hi {staff.firstName}, <br> A new rollover request has been made by investor : " + customer.FirstName + " " + customer.LastName + "",
                            sendIt = true,
                        });
                    }
                   
                }

                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Not Successful" } }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InvestorFundRegRespObj> UpdateInvestorFundTopUpByCustomer(InvestFundTopUpObj entity)
        {
            try
            {
                if (entity == null) return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid resquest payload" } }
                };
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);

                var staffList = await _serverRequest.GetAllStaffAsync();
                
                inf_investorfund_topup_website itemExist = new inf_investorfund_topup_website();
                if (entity.InvestorFundId > 0)
                {
                    itemExist = _dataContext.inf_investorfund_topup_website.FirstOrDefault(x => x.InvestorFundIdWebsiteTopupId == entity.InvestorFundIdWebsiteTopupId);
                    if (itemExist != null)
                    {
                        itemExist.InvestorFundId = entity.InvestorFundId;
                        itemExist.TopUpAmount = entity.TopUpAmount;
                        itemExist.Active = true;
                        itemExist.Deleted = false;
                        itemExist.UpdatedBy = "Customer";
                        itemExist.UpdatedOn = DateTime.Now;
                    }
                    else
                    {
                        itemExist = new inf_investorfund_topup_website
                        {
                            InvestorFundId = entity.InvestorFundId,
                            ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress,
                            TopUpAmount = entity.TopUpAmount,
                            Active = true,
                            Deleted = false,
                            CreatedBy = "Customer",
                            CreatedOn = DateTime.Now,
                        };
                        _dataContext.inf_investorfund_topup_website.Add(itemExist);
                    }
                }
               
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    if (customer.RelationshipManagerId != null)
                    {
                        var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                            subject = "New TopUp Request",
                            content = $"Hi {staff.firstName}, <br> A new top up request has been made by <br/> Investor : " + customer.FirstName + " " + customer.LastName + "",
                            sendIt = true,
                        });
                    }

                }

                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Not Successful" } }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InvestmentApprovalRecommendationObj UpdateInvestmentRecommendation(InvestmentApprovalRecommendationObj entity, string user)
        {
            try
            {

                if (entity == null) return null;

                if (entity.InvestorFundId > 0)
                {

                    var ApplicationExist = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                    if (ApplicationExist != null)
                    {
                        ApplicationExist.ApprovedAmount = entity.ApprovedAmount;
                        ApplicationExist.ApprovedProductId = entity.ApprovedProductId;
                        ApplicationExist.ApprovedRate = entity.ApprovedRate;
                        ApplicationExist.ApprovedTenor = entity.ApprovedTenor;
                        //ApplicationExist.UpdatedBy = user.;
                        ApplicationExist.UpdatedOn = DateTime.Now;

                        var log = new inf_investmentrecommendationlog
                        {
                            InvestorFundId = entity.InvestorFundId,
                            ApprovedAmount = entity.ApprovedAmount,
                            ApprovedProductId = entity.ApprovedProductId,
                            ApprovedRate = entity.ApprovedRate,
                            ApprovedTenor = entity.ApprovedTenor,
                            CreatedBy = user,
                            CreatedOn = entity.InvestorFundId > 0 ? DateTime.Today : DateTime.Today,
                        };
                        _dataContext.inf_investmentrecommendationlog.Add(log);
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    return new InvestmentApprovalRecommendationObj
                    {
                        InvestorFundId = entity.InvestorFundId,
                        ApprovedAmount = entity.ApprovedAmount,
                        ApprovedProductId = entity.ApprovedProductId,
                        ApprovedRate = entity.ApprovedRate,
                        ApprovedTenor = entity.ApprovedTenor,

                    };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestmentRecommendationLogObj> GetInvestmentRecommendationLog(int InvestorFundId)
        {
            var user = _serverRequest.UserDataAsync().Result;
            var log = (from a in _dataContext.inf_investmentrecommendationlog
                       where a.InvestorFundId == InvestorFundId
                       orderby a.CreatedOn descending
                       select new InvestmentRecommendationLogObj
                       {
                           InvestorFundId = a.InvestorFundId,
                           Amount = a.ApprovedAmount,
                           ProductId = a.ApprovedProductId,
                           ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                           Rate = a.ApprovedRate,
                           Tenor = a.ApprovedTenor,
                           CreatedBy = user.UserName,
                       }).ToList();
            return log;
        }

        public IEnumerable<InvestorFundObj> GetAllInvestorFundWebsiteList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund_website
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   where a.Deleted == false && a.ApprovalStatus == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                   select new InvestorFundObj
                                   {
                                       WebsiteInvestorFundId = a.WebsiteInvestorFundId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId ?? 0,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       CompanyId = a.CompanyId ?? 0,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestFundTopUpObj> GetAllInvestorFundTopUpWebsiteList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.inf_investorfund_topup_website on a.InvestorFundId equals b.InvestorFundId
                                   join c in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals c.CustomerId
                                   where b.Deleted == false && b.ApprovalStatus == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                   select new InvestFundTopUpObj
                                   {
                                       RequestDate = b.CreatedOn,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestorFundId = a.InvestorFundId,
                                       InvestorFundIdWebsiteTopupId = b.InvestorFundIdWebsiteTopupId,
                                       TopUpAmount = b.TopUpAmount,
                                       InvestorName = c.FirstName + " " + c.LastName,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = b.ApprovalStatus,
                                   }).ToList();
                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InvestorFundObj GetAllInvestorFundTopUpWebsiteById(int Id)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join c in _dataContext.inf_investorfund_topup_website on a.InvestorFundId equals c.InvestorFundId
                                   where a.Deleted == false && c.InvestorFundIdWebsiteTopupId == Id
                                   select new InvestorFundObj
                                   {
                                       InvestorFundIdWebsiteTopupId = c.InvestorFundIdWebsiteTopupId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ApprovedAmount,
                                       TopUpAmount = c.TopUpAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       CompanyId = a.CompanyId ?? 0,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestFundRollOverObj> GetAllInvestorFundRollOverWebsiteList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.inf_investorfund_rollover_website on a.InvestorFundId equals b.InvestorFundId
                                   join c in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals c.CustomerId
                                   where b.Deleted == false && b.ApprovalStatus == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                   select new InvestFundRollOverObj
                                   {
                                       InvestorFundId = a.InvestorFundId,
                                       InvestorFundIdWebsiteRolloverId = b.InvestorFundIdWebsiteRolloverId,
                                       RollOverAmount = b.RollOverAmount,
                                       InvestorName = c.FirstName + " " + c.LastName,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = b.ApprovalStatus,
                                       EffectiveDate = b.EffectiveDate,
                                       RequestDate = b.CreatedOn
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InvestorFundObj GetAllInvestorFundRollOverWebsiteById(int Id)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join c in _dataContext.inf_investorfund_rollover_website on a.InvestorFundId equals c.InvestorFundId
                                   where a.Deleted == false && c.InvestorFundIdWebsiteRolloverId == Id
                                   select new InvestorFundObj
                                   {
                                       InvestorFundIdWebsiteRolloverId = c.InvestorFundIdWebsiteRolloverId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = c.RollOverAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = c.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       CompanyId = a.CompanyId ?? 0,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       InvestorFundId = a.InvestorFundId
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public InvestorFundObj GetAllInvestorFundWebsiteById(int Id)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund_website
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   where a.Deleted == false && a.WebsiteInvestorFundId == Id
                                   select new InvestorFundObj
                                   {
                                       WebsiteInvestorFundId = a.WebsiteInvestorFundId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId ?? 0,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       CompanyId = a.CompanyId ?? 0,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorRunningFacilitiesObj> GetAllInvestorRunningFacilities(int customerId)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   where a.Deleted == false && a.InvestorFundCustomerId == customerId && a.ApprovalStatus == 2
                                   select new InvestorRunningFacilitiesObj
                                   {
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       CustomerNameId = a.CustomerNameId,
                                       CustomerName = b.FirstName + " " + b.LastName,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       ApprovedTenor = a.ProposedTenor,
                                       ApprovedRate = a.ProposedRate,
                                       EffectiveDate = a.EffectiveDate,
                                       MaturityDate = a.MaturityDate,
                                       ApprovedAmount = a.ProposedAmount,
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorRunningFacilitiesObj> GetAllInvestmentByCustomerId(int customerId)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   where a.Deleted == false && a.InvestorFundCustomerId == customerId //&& a.ApprovalStatus == 2
                                   select new InvestorRunningFacilitiesObj
                                   {
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       CustomerNameId = a.CustomerNameId,
                                       CustomerName = b.FirstName + " " + b.LastName,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       ApprovedTenor = a.ProposedTenor,
                                       ApprovedRate = a.ProposedRate,
                                       EffectiveDate = a.EffectiveDate,
                                       MaturityDate = a.MaturityDate,
                                       ApprovedAmount = a.ProposedAmount,
                                       ApprovalStatus = a.ApprovalStatus == 2 ? "Approved" : "Not Approved",
                                   }).ToList();
                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestmentListObj> GetAllInvestmentList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.inf_customer on a.InvestorFundCustomerId equals b.InvestorFundCustormerId
                                   where a.Deleted == false
                                   select new InvestmentListObj
                                   {
                                       InvestorFundId = a.InvestorFundId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ApplicationDate = a.EffectiveDate,
                                       RefNumber = a.RefNumber,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       ProposedAmount = a.ProposedAmount,
                                       ApprovalStatus = a.ApprovalStatus
                                   }).ToList().OrderByDescending(x => x.ApplicationDate);

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorListObj> GetAllInvestorList()
        {
            try
            {
                var customerList = _dataContext.credit_loancustomer.Where(x => x.Deleted == false).ToList();
                var invetsorFund = _dataContext.inf_investorfund.Where(d => d.Deleted == false).ToList();
                var InvestList = customerList.Select(a => new InvestorListObj
                {
                    InvestorFundCustomerId = a.CustomerId,
                    CustomerTypeId = a.CustomerTypeId,
                    Email = a.Email,
                    Phone = a.PhoneNo,
                    CustomerTypeName = a.CustomerTypeId == 2 ? "Corporate" : "Individual",
                    CustomerName = a.FirstName + " " + a.LastName,
                    CurrentBalance = invetsorFund.Where(h => h.InvestorFundCustomerId == a.CustomerId)?.Sum(f => f?.ProposedAmount),
                    ExpectedInterest = invetsorFund.Where(h => h.InvestorFundCustomerId == a.CustomerId)?.Sum(f => f?.ProposedRate),
                }).ToList();
                return InvestList.GroupBy(k => k.InvestorFundCustomerId).Select(l => l.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorFundObj> GetAllInvestmentCertificates()
        {
            try
            {
                var loanApplication = (from a in _dataContext.inf_investorfund
                                       join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                       where a.Deleted == false && a.ApprovalStatus == 2
                                       //&& a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter
                                       orderby a.CreatedOn descending
                                       select new InvestorFundObj
                                       {
                                           InvestorFundId = a.InvestorFundId,
                                           ProductId = a.ProductId ?? 0,
                                           ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                           InvestorFundCustomerId = a.InvestorFundCustomerId,
                                           ProposedTenor = a.ProposedTenor,
                                           ProposedRate = a.ProposedRate,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ApprovedProductId = a.ProductId,
                                           ApprovedProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           FrequencyId = a.FrequencyId,
                                           Period = a.Period,
                                           ProposedAmount = a.ProposedAmount,
                                           CurrencyId = a.CurrencyId,
                                           EffectiveDate = a.EffectiveDate,
                                           InvestmentPurpose = a.InvestmentPurpose,
                                           EnableRollOver = a.EnableRollOver,
                                           InstrumentId = a.InstrumentId,
                                           InstrumentNumer = a.InstrumentNumer,
                                           InstrumentDate = a.InstrumentDate,
                                           RefNumber = a.RefNumber,
                                           ApprovalStatus = a.ApprovalStatus,
                                           InvestorName = b.FirstName + " " + b.LastName,
                                           CustomerTypeId = b.CustomerTypeId,
                                           CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorFundObj> GetAllRunningInvestmenByCustomer(int customerId)
        {
            try
            {
                var now = DateTime.Now.Date;
                var Application = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join e in _dataContext.inf_investdailyschedule on a.InvestorFundId equals e.InvestorFundId
                                   where a.Deleted == false 
                                   && a.ApprovalStatus == 2 
                                   && e.PeriodDate.Value.Date == now.Date
                                   && a.InvestorFundCustomerId == customerId
                                   orderby a.CreatedOn descending
                                   select new InvestorFundObj
                                   {
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       TerminationCharge = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       ApprovedRate = a.ApprovedRate,
                                       ApprovedAmount = a.ApprovedAmount,
                                       ApprovedTenor = a.ApprovedTenor,
                                       ApprovedProductId = a.ApprovedProductId,
                                       ApprovedProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       RefNumber = a.RefNumber, 
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestmentStatus = a.InvestmentStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       AccountNumber = b.CASAAccountNumber,
                                       Payout = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == a.InvestorFundId).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                                       InterestEarned = e.OB + e.InterestAmount,
                                   }).ToList();
                return Application;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorFundObj> GetAllRunningInvestment()
        {
            try
            {
                var now = DateTime.Now.Date;
                var Application = (from a in _dataContext.inf_investorfund
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join c in _dataContext.inf_product on a.ApprovedProductId equals c.ProductId
                                   join e in _dataContext.inf_investdailyschedule on a.InvestorFundId equals e.InvestorFundId
                                   where a.Deleted == false && a.ApprovalStatus == 2 && e.PeriodDate.Value.Date == now.Date
                                   orderby a.CreatedOn descending
                                   select new InvestorFundObj
                                   {
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId ?? 0,
                                       ProductName = c.ProductName,
                                       TerminationCharge = c.EarlyTerminationCharge,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       ApprovedRate = a.ApprovedRate,
                                       ApprovedAmount = a.ApprovedAmount,
                                       ApprovedTenor = a.ApprovedTenor,
                                       ApprovedProductId = a.ApprovedProductId,
                                       ApprovedProductName = c.ProductName,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       MaturityDate = a.MaturityDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       EnableRollOver = a.EnableRollOver,
                                       InstrumentId = a.InstrumentId,
                                       InstrumentNumer = a.InstrumentNumer,
                                       InstrumentDate = a.InstrumentDate,
                                       RefNumber = a.RefNumber,
                                       ApprovalStatus = a.ApprovalStatus,
                                       InvestmentStatus = a.MaturityDate.Value.Date <= now.Date ? 2 : a.InvestmentStatus,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       AccountNumber = b.CASAAccountNumber,
                                       Payout = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == a.InvestorFundId).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                                       InterestEarned = e.OB + e.InterestAmount,
                                   }).ToList();
                foreach(var item in Application)
                {
                    var topUpList = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == item.InvestorFundId && x.PeriodDate.Value.Date == now.Date)
                        .Select(x => new { x.ReferenceNo, x.InvestorFundId, x.OB, x.Repayment, x.InterestAmount }).GroupBy(y => new { y.ReferenceNo, y.OB, y.Repayment, y.InterestAmount }, (key, group) => new
                            {
                                ReferenceNo = key.ReferenceNo,
                                InterestEarned = key.OB + key.InterestAmount,
                                Payout = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == item.InvestorFundId && x.ReferenceNo == key.ReferenceNo).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                            }).ToList();
                        decimal? interestEarned = 0, payout = 0;
                        if (topUpList.Count() > 0)
                        {                          
                            foreach(var i in topUpList)
                            {
                                interestEarned = interestEarned + i.InterestEarned;
                                payout = payout + i.Payout;
                            }
                        }
                        item.Payout = item.Payout + payout;
                        item.InterestEarned = item.InterestEarned + interestEarned;
                }

                return Application;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<InvestorFundObj> GetAllRunningInvestmentByStatus(int Id)
        {
            try
            {
                var now = DateTime.Now.Date;
                var Application = (from a in _dataContext.inf_investorfund
                                       join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                       join c in _dataContext.inf_product on a.ApprovedProductId equals c.ProductId
                                       join e in _dataContext.inf_investdailyschedule on a.InvestorFundId equals e.InvestorFundId
                                       where a.Deleted == false && a.ApprovalStatus == 2 && a.InvestmentStatus == Id && e.PeriodDate.Value.Date == now.Date
                                       orderby a.CreatedOn descending
                                       select new InvestorFundObj
                                       {
                                           InvestorFundId = a.InvestorFundId,
                                           ProductId = a.ProductId ?? 0,
                                           ProductName = c.ProductName,
                                           TerminationCharge = c.EarlyTerminationCharge,
                                           InvestorFundCustomerId = a.InvestorFundCustomerId,
                                           ProposedTenor = a.ProposedTenor,
                                           ProposedRate = a.ProposedRate,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedProductName = c.ProductName,
                                           FrequencyId = a.FrequencyId,
                                           Period = a.Period,
                                           ProposedAmount = a.ProposedAmount,
                                           CurrencyId = a.CurrencyId,
                                           EffectiveDate = a.EffectiveDate,
                                           InvestmentPurpose = a.InvestmentPurpose,
                                           EnableRollOver = a.EnableRollOver,
                                           InstrumentId = a.InstrumentId,
                                           InstrumentNumer = a.InstrumentNumer,
                                           InstrumentDate = a.InstrumentDate,
                                           RefNumber = a.RefNumber,
                                           ApprovalStatus = a.ApprovalStatus,
                                           InvestmentStatus = a.MaturityDate.Value.Date <= now.Date ? 2 : a.InvestmentStatus,
                                           InvestorName = b.FirstName + " " + b.LastName,
                                           CustomerTypeId = b.CustomerTypeId,
                                           CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                           AccountNumber = b.CASAAccountNumber,
                                           Payout = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == a.InvestorFundId).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                                           InterestEarned = e.OB + e.InterestAmount,
                                       }).OrderByDescending(x => x.EffectiveDate).ToList();
                foreach (var item in Application)
                {
                    var topUpList = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == item.InvestorFundId && x.PeriodDate.Value.Date == now.Date)
                        .Select(x => new { x.ReferenceNo, x.InvestorFundId, x.OB, x.Repayment, x.InterestAmount }).GroupBy(y => new { y.ReferenceNo, y.OB, y.Repayment, y.InterestAmount }, (key, group) => new
                        {
                            ReferenceNo = key.ReferenceNo,
                            InterestEarned = key.OB + key.InterestAmount,
                            Payout = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == item.InvestorFundId && x.ReferenceNo == key.ReferenceNo).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                        }).ToList();
                    decimal? interestEarned = 0, payout = 0;
                    if (topUpList.Count() > 0)
                    {
                        foreach (var i in topUpList)
                        {
                            interestEarned = interestEarned + i.InterestEarned;
                            payout = payout + i.Payout;
                        }
                    }
                    item.Payout = item.Payout + payout;
                    item.InterestEarned = item.InterestEarned + interestEarned;
                }
                return Application;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> InvestmentApproval(int targetId, int approvalStatusId)
        {
            bool response = false;
            var investmentApplication = _dataContext.inf_investorfund.Find(targetId);
            int tenor = (int)investmentApplication.ApprovedTenor;
            var effectiveDate = investmentApplication.EffectiveDate ?? DateTime.Now;
            var maturityDate = effectiveDate.AddDays(tenor);
            investmentApplication.MaturityDate = maturityDate;
            investmentApplication.ApprovalStatus = approvalStatusId;
            investmentApplication.InvestmentStatus = (int)InvestmentStatus.Running;
            var customer = _dataContext.credit_loancustomer.Find(investmentApplication.InvestorFundCustomerId);
            response = _dataContext.SaveChanges() > 0;
            if (response)
            {
                var ttt = AddInvestmentSchedule(targetId);
                await _serverRequest.SendMail(new MailObj
                {
                    fromAddresses = new List<FromAddress> { },
                    toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                    subject = "Investment Successfully Approved",
                    content = $"Hi {customer.FirstName}, <br> Your investment application has been finally approved. <br/> Investment Amount : {investmentApplication.ApprovedAmount}",
                    sendIt = true,
                });
            }
            return response;
        }

        public bool AddInvestmentSchedule(int InvestorFundId)
        {
            var InvestmentApp = _dataContext.inf_investorfund.Find(InvestorFundId);
            int tenor = (int)InvestmentApp.ApprovedTenor;
            var effectiveDate = InvestmentApp.EffectiveDate ?? DateTime.Now;
            var product = _dataContext.inf_product.Find(InvestmentApp.ApprovedProductId);
            var freq = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == product.FrequencyId).FirstOrDefault().Days;
            var maturityDate = effectiveDate.AddDays(tenor);

            var paymentDate = effectiveDate.AddDays(1);

            //_financeTransaction.BuildInvestmentPaymentPosting(input);
            var investmentEntry = new TransactionObj
            {
                IsApproved = false,
                CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == InvestmentApp.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                CompanyId = InvestmentApp.CompanyId,
                Amount = InvestmentApp.ApprovedAmount ?? 0,
                CurrencyId = InvestmentApp.CurrencyId ?? 0,
                Description = "Investment Principal Payment Posting",
                DebitGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == InvestmentApp.ProductId).ProductPrincipalGl.Value,
                CreditGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == InvestmentApp.ProductId).ReceiverPrincipalGl.Value,
                ReferenceNo = InvestmentApp.RefNumber,
                OperationId = 16,
                JournalType = "System",
                RateType = 2,//Buying Rate
            };

            var res1 = _serverRequest.PassEntryToFinance(investmentEntry).Result;
            if (res1.Status.IsSuccessful == false)
            {
                //throw new Exception(res1.Status.Message.FriendlyMessage, new Exception());
            }

            var previousSchedule = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == InvestmentApp.InvestorFundId).ToList();
            if (previousSchedule.Count() > 0)
            {
                _dataContext.inf_investdailyschedule.RemoveRange(previousSchedule);
            }

            GenerateInvestmentDailySchedule(InvestorFundId);

            GenerateInvestmentPeriodicSchedule(InvestorFundId);

            return true;

        }

        public IEnumerable<DailyInterestAccrualObj> ProcessMaturedInvestmentPosting(DateTime applicationDate)
        {
            var data = (from a in _dataContext.inf_investdailyschedule
                        join b in _dataContext.inf_investorfund on a.InvestorFundId equals b.InvestorFundId
                        where b.MaturityDate.Value.Date == applicationDate.Date && b.ApprovalStatus == 2

                        select new DailyInterestAccrualObj()
                        {
                            referenceNumber = b.RefNumber,
                            productId = b.ProductId ?? 0,
                            companyId = b.CompanyId ?? 0,
                            currencyId = b.CurrencyId ?? 0,
                            //exchangeRate = GetExchangeRate(valueDate, b.CurrencyId ?? 0).sellingRate,
                            interestRate = (double)b.ApprovedRate,
                            date = applicationDate,
                            dailyAccuralAmount = (double)a.Repayment - (double)b.ApprovedAmount,
                            mainAmount = (decimal)a.Repayment - (decimal)b.ApprovedAmount,
                            categoryId = (short)DailyAccrualCategory.TermLoan,
                            transactionTypeId = (byte)LoanTransactionTypeEnum.Interest,
                        }).ToList();

            var transList = new List<TransactionObj>();
            foreach (var item in data)
            {
                item.date = applicationDate;
                var product = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == item.productId);
                if (product == null)
                {

                }
                if (product.InterestPayableGl == null)
                {

                }
                if (product.ProductPrincipalGl == null)
                {

                }
                var entry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = string.Empty,
                    CompanyId = item.CompanyId,
                    Amount = Convert.ToDecimal(item.mainAmount),
                    CurrencyId = item.currencyId,
                    Description = "Investment Maturity Posting",
                    DebitGL = product.InterestPayableGl.Value,
                    CreditGL = product.ProductPrincipalGl.Value,
                    ReferenceNo = item.referenceNumber,
                    OperationId = (int)OperationsEnum.DailyInterestAccural,
                    JournalType = "System",
                    RateType = 2,//Buying Rate
                };
                //transList.Add(entry);
                var res1 = _serverRequest.PassEntryToFinance(entry).Result;
            }
            //var res1 = _serverRequest.PassEntryToFinance(transList).Result;
            _dataContext.SaveChanges();
            return data;
        }

        public void ProcessRollOverPosting(DateTime applicationDate)
        {
            var data =  _dataContext.inf_investorfund.Where(x=>x.MaturityDate.Value.Date == applicationDate.Date && x.ApprovalStatus == 2).ToList();

            foreach (var item in data)
            {
                var InvestmentApp = _dataContext.inf_investorfund.FirstOrDefault(x => x.InvestorFundId == item.InvestorFundId);
                if (InvestmentApp.EnableRollOver ?? false)
                {
                    var effectiveDate = DateTime.Now;
                    var tenor = Convert.ToDouble(InvestmentApp.ApprovedTenor);
                    var maturityDate = effectiveDate.AddDays(tenor);
                    var expectedPayout = _dataContext.inf_investdailyschedule.LastOrDefault(x => x.InvestorFundId == InvestmentApp.InvestorFundId)?.Repayment;
                    InvestmentApp.EffectiveDate = effectiveDate;
                    InvestmentApp.MaturityDate = maturityDate;
                    InvestmentApp.ApprovedAmount = expectedPayout;
                    _dataContext.SaveChanges();

                    var previousSchedule = _dataContext.inf_investdailyschedule.Where(x => x.InvestorFundId == InvestmentApp.InvestorFundId).ToList();
                    if(previousSchedule.Count() > 0)
                    {
                        _dataContext.inf_investdailyschedule.RemoveRange(previousSchedule);
                    }                   
                    GenerateInvestmentDailySchedule(InvestmentApp.InvestorFundId);
                }
            }
        }

        public decimal GetCurrentBalance(DateTime date, int id)
        {
            try
            {
                var accountType = _dataContext.inf_investdailyschedule.FirstOrDefault(x => x.PeriodDate.Value.Date == date.Date && x.InvestorFundId == id);
                if (accountType == null)
                {
                    throw new Exception("The selected date doesn't have a current balance");
                }
                decimal? bal = accountType.CB;
                var topUpList = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == id && x.PeriodDate.Value.Date == date.Date)
                            .Select(x => new { x.ReferenceNo, x.InvestorFundId, x.OB, x.Repayment, x.InterestAmount }).GroupBy(y => new { y.ReferenceNo, y.OB, y.Repayment, y.InterestAmount }, (key, group) => new
                            {
                                ReferenceNo = key.ReferenceNo,
                                InterestEarned = key.OB + key.InterestAmount,
                                Payout = _dataContext.inf_investdailyschedule_topup.Where(x => x.InvestorFundId == id && x.ReferenceNo == key.ReferenceNo).OrderByDescending(l => l.Period).FirstOrDefault().Repayment,
                            }).ToList();
                decimal? interestEarned = 0, payout = 0;
                if (topUpList.Count() > 0)
                {
                    foreach (var i in topUpList)
                    {
                        interestEarned = interestEarned + i.InterestEarned;
                        payout = payout + i.Payout;
                    }
                }

                bal = bal + interestEarned;


                return bal ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GenerateInvestmentDailySchedule(int InvestorFundId)
        {
            var InvestmentApp = _dataContext.inf_investorfund.Find(InvestorFundId);
            var effectiveDate = InvestmentApp.EffectiveDate ?? DateTime.Now;
            var product = _dataContext.inf_product.Find(InvestmentApp.ApprovedProductId);
            var freq = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == product.FrequencyId).FirstOrDefault();
            int dailyPeriod = (int)InvestmentApp.ApprovedTenor;
            decimal dailyInterest = (InvestmentApp.ApprovedRate / freq.Days ?? 0) / 100;
            decimal dailyCB = InvestmentApp.ApprovedAmount ?? 0;
            int i = 1;
            int count = 0;

            for (int k = 0; k <= dailyPeriod; k++)
            {
                inf_investdailyschedule dailyschedule = new inf_investdailyschedule();
                if (count == freq.Days)
                {
                    i++;
                    count = 0;
                    dailyschedule.EndPeriod = true;
                }
                if (k == 0)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = 0;
                    dailyschedule.Repayment = 0;
                    dailyschedule.CB = dailyCB;
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    dailyschedule.EndPeriod = true;
                    _dataContext.inf_investdailyschedule.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k == 1 && k == dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.CB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyCB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investdailyschedule.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k > 0 && k < dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = 0;
                    dailyschedule.CB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyCB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investdailyschedule.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k == dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i + 1;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.CB = 0;
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyschedule.EndPeriod = true;
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investdailyschedule.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                count++;
            }
        }

        private void GenerateInvestmentPeriodicSchedule(int InvestorFundId)
        {
            var InvestmentApp = _dataContext.inf_investorfund.Find(InvestorFundId);
            int period = int.Parse(InvestmentApp.Period);
            var effectiveDate = InvestmentApp.EffectiveDate ?? DateTime.Now;
            var product = _dataContext.inf_product.Find(InvestmentApp.ApprovedProductId);
            int freq = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == product.FrequencyId).FirstOrDefault().Days ?? 0;
            decimal CB = InvestmentApp.ApprovedAmount ?? 0;

            for (int i = 0; i <= period; i++)
            {
                inf_investmentperiodicschedule schedule = new inf_investmentperiodicschedule();
                if (i == 0)
                {
                    schedule.Period = i;
                    schedule.OB = CB;
                    schedule.InterestAmount = 0;
                    schedule.Repayment = 0;
                    schedule.CB = CB;
                    schedule.PeriodDate = effectiveDate.AddDays(freq);
                    schedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investmentperiodicschedule.Add(schedule);
                    _dataContext.SaveChanges();
                }
                else if (i == 1 && i == period)
                {
                    schedule.Period = i;
                    schedule.OB = CB;
                    schedule.InterestAmount = (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.Repayment = CB + (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.CB = CB + (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.PeriodDate = effectiveDate.AddDays(freq);
                    CB = CB + (CB * (InvestmentApp.ApprovedRate / 100)) ?? 0;
                    schedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investmentperiodicschedule.Add(schedule);
                    _dataContext.SaveChanges();
                }
                else if (i > 0 && i < period)
                {
                    schedule.Period = i;
                    schedule.OB = CB;
                    schedule.InterestAmount = (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.Repayment = 0;
                    schedule.CB = CB + (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.PeriodDate = effectiveDate.AddDays(freq);
                    CB = CB + (CB * (InvestmentApp.ApprovedRate / 100)) ?? 0;
                    schedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investmentperiodicschedule.Add(schedule);
                    _dataContext.SaveChanges();
                }
                else if (i == period)
                {
                    schedule.Period = i;
                    schedule.OB = CB;
                    schedule.InterestAmount = (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.Repayment = CB + (CB * (InvestmentApp.ApprovedRate / 100));
                    schedule.CB = 0;
                    schedule.PeriodDate = effectiveDate.AddDays(freq);
                    schedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    _dataContext.inf_investmentperiodicschedule.Add(schedule);
                    _dataContext.SaveChanges();
                }
                freq++;
            }
        }

        private void GenerateInvestmentDailyTopUpSchedule(int InvestorFundId, decimal topUpAmount, string Ref, int tenor)
        {
            var InvestmentApp = _dataContext.inf_investorfund.Find(InvestorFundId);
            var effectiveDate = DateTime.Now;
            var product = _dataContext.inf_product.Find(InvestmentApp.ApprovedProductId);
            var freq = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == product.FrequencyId).FirstOrDefault();
            int dailyPeriod = tenor;
            decimal dailyInterest = (InvestmentApp.ApprovedRate / freq.Days ?? 0) / 100;
            decimal dailyCB = topUpAmount;
            int i = 1;
            int count = 0;

            for (int k = 0; k <= dailyPeriod; k++)
            {
                inf_investdailyschedule_topup dailyschedule = new inf_investdailyschedule_topup();
                if (count == freq.Days)
                {
                    i++;
                    count = 0;
                    dailyschedule.EndPeriod = true;
                }
                if (k == 0)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = 0;
                    dailyschedule.Repayment = 0;
                    dailyschedule.CB = dailyCB;
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    dailyschedule.ReferenceNo = Ref;
                    dailyschedule.EndPeriod = true;
                    _dataContext.inf_investdailyschedule_topup.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k == 1 && k == dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.CB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyCB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    dailyschedule.ReferenceNo = Ref;
                    _dataContext.inf_investdailyschedule_topup.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k > 0 && k < dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = 0;
                    dailyschedule.CB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyCB = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    dailyschedule.ReferenceNo = Ref;
                    _dataContext.inf_investdailyschedule_topup.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                else if (k == dailyPeriod)
                {
                    dailyschedule.Period = k;
                    dailyschedule.PeriodId = i + 1;
                    dailyschedule.OB = dailyCB;
                    dailyschedule.InterestAmount = (dailyCB * dailyInterest);
                    dailyschedule.Repayment = dailyCB + (dailyCB * dailyInterest);
                    dailyschedule.CB = 0;
                    dailyschedule.PeriodDate = effectiveDate.AddDays(k);
                    dailyschedule.EndPeriod = true;
                    dailyschedule.InvestorFundId = InvestmentApp.InvestorFundId;
                    dailyschedule.ReferenceNo = Ref;
                    _dataContext.inf_investdailyschedule_topup.Add(dailyschedule);
                    _dataContext.SaveChanges();
                }
                count++;
            }
        }

        #endregion

        #region Collection
        public async Task<CollectionRespObj> AddUpdateCollection(CollectionObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                if (entity == null) return null;
                if (entity.WebsiteCollectionOperationId > 0)
                {
                    var oldApp = _dataContext.inf_collection_website.Where(x => x.WebsiteCollectionOperationId == entity.WebsiteCollectionOperationId).FirstOrDefault();
                    oldApp.ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new CollectionRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Investor doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);

                var mdata = entity.S_date ?? DateTime.Now;
                entity.MaturityDate = mdata.AddDays((int)entity.ProposedTenor);
                inf_collection collectionExist = null;
                if (entity.CollectionId > 0)
                {
                    collectionExist = _dataContext.inf_collection.Find(entity.CollectionId);
                    if (collectionExist != null)
                    {
                        collectionExist.CollectionId = entity.CollectionId;
                        collectionExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        collectionExist.InvestorFundId = entity.InvestorFundId;
                        collectionExist.ProductId = entity.ProductId;
                        collectionExist.ProposedTenor = entity.ProposedTenor;
                        collectionExist.ProposedRate = entity.ProposedRate;
                        collectionExist.FrequencyId = entity.FrequencyId;
                        collectionExist.Period = entity.Period;
                        collectionExist.ProposedAmount = entity.ProposedAmount;
                        collectionExist.CurrencyId = entity.CurrencyId;
                        collectionExist.EffectiveDate = entity.EffectiveDate;
                        collectionExist.InvestmentPurpose = entity.InvestmentPurpose;
                        collectionExist.CollectionDate = entity.CollectionDate;
                        collectionExist.AmountPayable = entity.AmountPayable;
                        collectionExist.DrProductPrincipal = entity.DrProductPrincipal;
                        collectionExist.CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL;
                        collectionExist.Account = entity.Account;
                        collectionExist.PaymentAccount = entity.PaymentAccount;
                        collectionExist.Active = true;
                        collectionExist.Deleted = false;
                        collectionExist.UpdatedBy = user.UserName;
                        collectionExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    collectionExist = new inf_collection
                    {
                        CollectionId = entity.CollectionId,
                        InvestorFundId = entity.InvestorFundId,
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.EffectiveDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        CollectionDate = entity.CollectionDate,
                        AmountPayable = entity.AmountPayable,
                        DrProductPrincipal = entity.DrProductPrincipal,
                        CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL,
                        Account = entity.Account,
                        PaymentAccount = entity.PaymentAccount,
                        Active = true,
                        Deleted = false,
                        CreatedBy = user.UserName,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.inf_collection.Add(collectionExist);
                }
                try
                {
                    using (var _trans = _dataContext.Database.BeginTransaction())
                    {
                        var output = _dataContext.SaveChanges() > 0;
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                            subject = "Successful Collection Application",
                            content = $"Hi {customer.FirstName}, <br> Your collection application is successful and awaiting approval.",
                            sendIt = true,
                        });

                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = user.FirstName}
                            },
                            subject = "Successful Collection Application",
                            content = $"Hi {staff.firstName}, <br> Appraisal is pending for Collection application with Investment reference : " + investment.RefNumber + "",
                            sendIt = true,
                        });
                        var targetIds = new List<int>();
                        targetIds.Add(collectionExist.CollectionId);
                        var request = new GoForApprovalRequest
                        {
                            StaffId = user.StaffId,
                            CompanyId = 1,
                            StatusId = (int)ApprovalStatus.Pending,
                            TargetId = targetIds,
                            Comment = "Collection Application",
                            OperationId = (int)OperationsEnum.CollectionApproval,
                            DeferredExecution = true, // false by default will call the internal SaveChanges() 
                            ExternalInitialization = true,
                            EmailNotification = true,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(request);
                        try
                        {
                            if (!result.IsSuccessStatusCode)
                            {
                                new ApprovalRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                    }
                                };
                            }
                            var stringData = await result.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                            if (res.ApprovalProcessStarted)
                            {
                                collectionExist.WorkflowToken = res.Status.CustomToken;
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new CollectionRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                            if (res.EnableWorkflow || !res.HasWorkflowAccess)
                            {
                                await _trans.RollbackAsync();
                                return new CollectionRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = false,
                                        Message = res.Status.Message
                                    }
                                };
                            }
                            if (!res.EnableWorkflow)
                            {
                                var response = CollectionApproval(collectionExist.CollectionId, 2);
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new CollectionRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = true,
                                        Message = new APIResponseMessage
                                        {
                                            FriendlyMessage = "Successful"
                                        }
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            _trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    return new CollectionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = " Not Successful"
                            }
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCollection(int id)
        {
            var itemToDelete = _dataContext.inf_collection.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportCollection()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Product");
            dt.Columns.Add("Customer");
            dt.Columns.Add("Proposed Tenor");
            dt.Columns.Add("Proposed Rate");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Period");
            dt.Columns.Add("Proposed Amount");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Effective Date");
            dt.Columns.Add("Investment Purpose");
            dt.Columns.Add("Collection Date");
            dt.Columns.Add("Amount Payable");
            dt.Columns.Add("Dr Product Principal");
            dt.Columns.Add("Cr Receiver Principal GL");
            var statementType = (from a in _dataContext.inf_collection
                                 where a.Deleted == false
                                 select new CollectionObj
                                 {
                                     ProductId = a.ProductId,
                                     InvestorFundCustomerId = a.InvestorFundCustomerId,
                                     ProposedTenor = a.ProposedTenor,
                                     ProposedRate = a.ProposedRate,
                                     FrequencyId = a.FrequencyId,
                                     Period = a.Period,
                                     ProposedAmount = a.ProposedAmount,
                                     CurrencyId = a.CurrencyId,
                                     EffectiveDate = a.EffectiveDate,
                                     InvestmentPurpose = a.InvestmentPurpose,
                                     CollectionDate = a.CollectionDate,
                                     AmountPayable = a.AmountPayable,
                                     DrProductPrincipal = a.DrProductPrincipal,
                                     CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                 }).ToList();

            foreach (var kk in statementType)
            {
                var row = dt.NewRow();
                row["Product"] = kk.ProductId;
                row["Customer"] = kk.InvestorFundCustomerId;
                row["Proposed Tenor"] = kk.ProposedTenor;
                row["Proposed Rate"] = kk.ProposedRate;
                row["Frequency"] = kk.FrequencyId;
                row["Period"] = kk.Period;
                row["Proposed Amount"] = kk.ProposedAmount;
                row["Currency"] = kk.CurrencyId;
                row["Effective Date"] = kk.EffectiveDate;
                row["Investment Purpose"] = kk.InvestmentPurpose;
                row["Collection Date"] = kk.CollectionDate;
                row["Amount Payable"] = kk.AmountPayable;
                row["Dr Product Principal"] = kk.DrProductPrincipal;
                row["Cr Receiver Principal GL"] = kk.CrReceiverPrincipalGL;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CollectionOperation");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public List<CollectionObj> GetAllCollection()
        {
            try
            {
                var collection = (from a in _dataContext.inf_collection
                                   where a.Deleted == false
                                   select

                                   new CollectionObj
                                   {
                                       CollectionId = a.CollectionId,
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       /*RefNumber = b.RefNumber,
                                       RelationshipManager = _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().FirstName + " " + _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().LastName,
                                       CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().CurrencyName,
                                       ExchangeRate = _dataContext.cor_currencyrate.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().SellingRate,*/
                                       PaymentAccount = a.PaymentAccount,
                                       Account = a.Account,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       //FrequencyName = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == a.FrequencyId).FirstOrDefault().Mode,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       //MaturityDate = b.MaturityDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       CollectionDate = a.CollectionDate,
                                       AmountPayable = a.AmountPayable,
                                       DrProductPrincipal = a.DrProductPrincipal,
                                       CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                   }).ToList();

                return collection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CollectionObj GetCollection(int Id)
        {
            try
            {
                var collection = (from a in _dataContext.inf_collection
                               where a.Deleted == false && a.CollectionId == Id
                               select

                              new CollectionObj
                              {
                                  CollectionId = a.CollectionId,
                                  InvestorFundId = a.InvestorFundId,
                                  ProductId = a.ProductId,
                                  InvestorFundCustomerId = a.InvestorFundCustomerId,
                                  /*RefNumber = b.RefNumber,
                                  RelationshipManager = _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().FirstName + " " + _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().LastName,
                                  CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().CurrencyName,
                                  ExchangeRate = _dataContext.cor_currencyrate.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().SellingRate,*/
                                  PaymentAccount = a.PaymentAccount,
                                  Account = a.Account,
                                  ProposedTenor = a.ProposedTenor,
                                  ProposedRate = a.ProposedRate,
                                  FrequencyId = a.FrequencyId,
                                  //FrequencyName = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == a.FrequencyId).FirstOrDefault().Mode,
                                  Period = a.Period,
                                  ProposedAmount = a.ProposedAmount,
                                  CurrencyId = a.CurrencyId,
                                  EffectiveDate = a.EffectiveDate,
                                  //MaturityDate = b.MaturityDate,
                                  InvestmentPurpose = a.InvestmentPurpose,
                                  CollectionDate = a.CollectionDate,
                                  AmountPayable = a.AmountPayable,
                                  DrProductPrincipal = a.DrProductPrincipal,
                                  CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                              }).FirstOrDefault();

                return collection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CollectionRegRespObj> UpdateCollectionOperationByCustomer(CollectionObj entity)
        {
            try
            {
                if (entity == null) return new CollectionRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid request payload" } } };
                var customer = _dataContext.credit_loancustomer.Find(entity.InvestorFundCustomerId);
                var staffList = await _serverRequest.GetAllStaffAsync();
                var user = _serverRequest.UserDataAsync().Result;
                inf_collection_website collectionExist = null;
                if (entity.CollectionId > 0)
                {
                    collectionExist = _dataContext.inf_collection_website.Find(entity.WebsiteCollectionOperationId);
                    if (collectionExist != null)
                    {
                        collectionExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        collectionExist.InvestorFundId = entity.InvestorFundId;
                        collectionExist.ProductId = entity.ProductId;
                        collectionExist.ProposedTenor = entity.ProposedTenor;
                        collectionExist.ProposedRate = entity.ProposedRate;
                        collectionExist.FrequencyId = entity.FrequencyId;
                        collectionExist.Period = entity.Period;
                        collectionExist.ProposedAmount = entity.ProposedAmount;
                        collectionExist.CurrencyId = entity.CurrencyId;
                        collectionExist.EffectiveDate = entity.EffectiveDate;
                        collectionExist.InvestmentPurpose = entity.InvestmentPurpose;
                        collectionExist.CollectionDate = entity.CollectionDate;
                        collectionExist.AmountPayable = entity.AmountPayable;
                        collectionExist.DrProductPrincipal = entity.DrProductPrincipal;
                        collectionExist.CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL;
                        collectionExist.Account = entity.Account;
                        collectionExist.PaymentAccount = entity.PaymentAccount;
                        collectionExist.Active = true;
                        collectionExist.Deleted = false;
                        collectionExist.Updatedby = user.UserName;
                        collectionExist.Updatedon = entity.CollectionId > 0 ? DateTime.Today : DateTime.Today;
                    }
                }
                else
                {
                    collectionExist = new inf_collection_website
                    {
                        InvestorFundId = entity.InvestorFundId,
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.EffectiveDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        CollectionDate = entity.CollectionDate,
                        AmountPayable = entity.AmountPayable,
                        DrProductPrincipal = entity.DrProductPrincipal,
                        CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL,
                        Account = entity.Account,
                        PaymentAccount = entity.PaymentAccount,
                        ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress,
                        Active = true,
                        Deleted = false,
                        Createdby = user.UserName,
                        Createdon = DateTime.Today,
                    };
                    _dataContext.inf_collection_website.Add(collectionExist);
                }

                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    if (customer.RelationshipManagerId != null)
                    {
                        var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = staff.firstName}
                            },
                            subject = "New Collection Request",
                            content = $"Hi {staff.firstName}, <br> A new collection request has been made by <br/> Investor : " + customer.FirstName + " " + customer.LastName + "",
                            sendIt = true,
                        });
                    }
                    
                }

                return new CollectionRegRespObj { Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved successful" : "Record not saved" } } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CollectionApprovalRecommendationObj UpdateCollectionRecommendation(CollectionApprovalRecommendationObj entity, string user)
        {
            try
            {
                if (entity == null) return null;

                if (entity.InvInvestorFundId > 0)
                {
                    var ApplicationExist = _dataContext.inf_investorfund.Find(entity.InvInvestorFundId);
                    if (ApplicationExist != null)
                    {
                        ApplicationExist.ApprovedAmount = entity.ApprovedAmount;
                        ApplicationExist.ApprovedProductId = entity.ApprovedProductId;
                        ApplicationExist.ApprovedRate = entity.ApprovedRate;
                        ApplicationExist.ApprovedTenor = entity.ApprovedTenor;
                        ApplicationExist.UpdatedBy = user;
                        ApplicationExist.UpdatedOn = DateTime.Now;

                        var log = new inf_collectionrecommendationLog
                        {
                            InvInvestorFundId = entity.InvInvestorFundId,
                            ApprovedAmount = entity.ApprovedAmount,
                            ApprovedProductId = entity.ApprovedProductId,
                            ApprovedRate = entity.ApprovedRate,
                            ApprovedTenor = entity.ApprovedTenor,
                            CreatedBy = user,
                            CreatedOn = DateTime.Now
                        };
                        _dataContext.inf_collectionrecommendationLog.Add(log);
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    return new CollectionApprovalRecommendationObj
                    {
                        InvInvestorFundId = entity.InvInvestorFundId,
                        ApprovedAmount = entity.ApprovedAmount,
                        ApprovedProductId = entity.ApprovedProductId,
                        ApprovedRate = entity.ApprovedRate,
                        ApprovedTenor = entity.ApprovedTenor,

                    };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CollectionRecommendationLogObj> GetCollectionRecommendationLog(int InvInvestorFundId)
        {
            var user = _serverRequest.UserDataAsync().Result;
            var log = (from a in _dataContext.inf_collectionrecommendationLog
                       where a.InvInvestorFundId == InvInvestorFundId
                       orderby a.CreatedOn descending

                       select new CollectionRecommendationLogObj

                       {
                           InvInvestorFundId = a.InvInvestorFundId,
                           Amount = a.ApprovedAmount,
                           ProductId = a.ApprovedProductId,
                           ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                           Rate = a.ApprovedRate,
                           Tenor = a.ApprovedTenor,
                           CreatedBy = user.UserName
                       }).ToList();
            return log;
        }

        public async Task<ActionResult<CollectionRespObj>> GetCollectionForAppraisalAsync()
        {
            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new CollectionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" }
                        }
                    };
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return new CollectionRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new CollectionRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    };

                }
                var collection = await GetCollectionAwaitingApprovalAsync(res.workflowTasks.Select(x => x.TargetId).ToList(), res.workflowTasks.Select(d => d.WorkflowToken).ToList());


                return new CollectionRespObj
                {
                    InvestmentLists = collection.Select(a => new InvestmentListObj
                    {
                        ApplicationDate = (DateTime)a.CreatedOn,
                        InvestorFundCustomerId = a.InvestorFundCustomerId,
                        ProposedAmount = a.ProposedAmount,
                        InvestorFundId = a.InvestorFundId,
                        InvestorName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).LastName,
                        ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                        ApprovalStatus = a.ApprovalStatus,
                        RefNumber = _dataContext.inf_investorfund.FirstOrDefault(x=>x.InvestorFundId == a.InvestorFundId).RefNumber,
                        workflowToken = a.WorkflowToken,
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = collection.Count() < 1 ? "No Collection awaiting approvals" : null
                        }
                    }
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private async Task<IEnumerable<inf_collection>> GetCollectionAwaitingApprovalAsync(List<int> CollectionIds, List<string> tokens)
        {
            var item = await _dataContext.inf_collection
                .Where(s => CollectionIds.Contains(s.CollectionId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public async Task<bool> CollectionApproval(int targetId, int approvalStatusId)
        {
            bool response = false;
            var collectionApplication = _dataContext.inf_collection.Find(targetId);
            collectionApplication.ApprovalStatus = approvalStatusId;
            var investment = _dataContext.inf_investorfund.Find(collectionApplication.InvestorFundId);
            investment.InvestmentStatus = (int)InvestmentStatus.Matured;
            var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
            response = _dataContext.SaveChanges() > 0;

            LoanPaymentScheduleInputObj input = new LoanPaymentScheduleInputObj
            {
                PrincipalAmount = (double)collectionApplication.AmountPayable,
                ProductId = investment.ProductId ?? 0,
                CurrencyId = investment.CurrencyId ?? 0,
                CompanyId = investment.CompanyId ?? 0,
            };
            if (response)
            {
                //_financeTransaction.BuildCollectionPaymentPosting(input);
                var collectionEntry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = investment.CompanyId,
                    Amount = collectionApplication.AmountPayable ?? 0,
                    CurrencyId = investment.CurrencyId ?? 0,
                    Description = "Investment Collection Payment Posting",
                    DebitGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).ProductPrincipalGl.Value,
                    CreditGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).ReceiverPrincipalGl.Value,
                    ReferenceNo = investment.RefNumber,
                    OperationId = 16,
                    JournalType = "System",
                    RateType = 2,//Buying Rate
                };

                if (collectionApplication.PaymentAccount == "2")///Pay to other banks
                {
                    var flutter = _serverRequest.GetFlutterWaveKeys().Result;
                    if (flutter.keys.useFlutterWave)
                    {
                        var account = collectionApplication.Account.Split("-");
                        var currencyList = _serverRequest.GetCurrencyAsync().Result;
                        var currency = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == investment.CurrencyId)?.Code;
                        TransferObj payWithFlutterWave = new TransferObj
                        {
                            account_bank = account[0].Trim(),
                            account_number = account[2].Trim(),
                            amount = Convert.ToInt32(collectionApplication.AmountPayable),
                            narration = "Investment Collection",
                            currency = currency,
                            reference = investment.RefNumber.Trim()
                        };
                        var res = _flutter.createTransfer(payWithFlutterWave).Result;
                    }
                }
                var res1 = _serverRequest.PassEntryToFinance(collectionEntry).Result;
                if (res1.Status.IsSuccessful == false)
                {
                    //throw new Exception(res1.Status.Message.FriendlyMessage, new Exception());
                }
                await _serverRequest.SendMail(new MailObj
                {
                    fromAddresses = new List<FromAddress> { },
                    toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                    subject = "Collection Successfully Approved",
                    content = $"Hi {customer.FirstName}, <br> Your collection request has been finally approved. <br/> Amount Payable: {collectionApplication.AmountPayable}",
                    sendIt = true,
                });
            }
            return response;
        }

        public IEnumerable<CollectionObj> GetAllCollectionOperationWebsiteList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_collection_website
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join c in _dataContext.inf_investorfund on a.InvestorFundId equals c.InvestorFundId
                                   where a.Deleted == false && a.ApprovalStatus == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                   select new CollectionObj
                                   {
                                       WebsiteCollectionOperationId = a.WebsiteCollectionOperationId,
                                       ProductId = a.ProductId,
                                       InvestorFundId = a.InvestorFundId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       CollectionDate = a.CollectionDate,
                                       AmountPayable = a.AmountPayable,
                                       DrProductPrincipal = a.DrProductPrincipal,
                                       CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                       PaymentAccount = a.PaymentAccount,
                                       Account = a.Account,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       RefNumber = c.RefNumber,
                                       S_date = a.Createdon
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CollectionObj GetCollectionOperationWebsiteById(int id)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_collection_website
                                   where a.Deleted == false && a.WebsiteCollectionOperationId == id
                                   select new CollectionObj
                                   {
                                       CollectionId = a.WebsiteCollectionOperationId,
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       CollectionDate = a.CollectionDate,
                                       AmountPayable = a.AmountPayable,
                                       DrProductPrincipal = a.DrProductPrincipal,
                                       CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                       PaymentAccount = a.PaymentAccount,
                                       Account = a.Account,
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Liquidation
        public async Task<LiquidationRespObj> AddUpdateLiquidation(LiquidationObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                if (entity == null) return null;
                if (entity.WebsiteLiquidationOperationId > 0)
                {
                    var oldApp = _dataContext.inf_liquidation_website.Where(x => x.WebsiteLiquidationOperationId == entity.WebsiteLiquidationOperationId).FirstOrDefault();
                    oldApp.ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationCompleted;
                }
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new LiquidationRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Investor doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);

                var mdata = entity.S_date ?? DateTime.Now;
                entity.MaturityDate = mdata.AddDays((int)entity.ProposedTenor);
                inf_liquidation liquidationExist = null;
                if (entity.LiquidationId > 0)
                {
                    liquidationExist = _dataContext.inf_liquidation.Find(entity.LiquidationId);
                    if (liquidationExist != null)
                    {
                        liquidationExist.LiquidationId = entity.LiquidationId;
                        liquidationExist.InvestorFundId = entity.InvestorFundId;
                        liquidationExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        liquidationExist.ProductId = entity.ProductId;
                        liquidationExist.ProposedTenor = entity.ProposedTenor;
                        liquidationExist.ProposedRate = entity.ProposedRate;
                        liquidationExist.FrequencyId = entity.FrequencyId;
                        liquidationExist.Period = entity.Period;
                        liquidationExist.ProposedAmount = entity.ProposedAmount;
                        liquidationExist.CurrencyId = entity.CurrencyId;
                        liquidationExist.EffectiveDate = entity.EffectiveDate;
                        liquidationExist.InvestmentPurpose = entity.InvestmentPurpose;
                        liquidationExist.LiquidationDate = entity.LiquidationDate;
                        liquidationExist.EarlyTerminationCharge = entity.EarlyTerminationCharge;
                        liquidationExist.AmountPayable = entity.AmountPayable;
                        liquidationExist.DrProductPrincipal = entity.DrProductPrincipal;
                        liquidationExist.CrIntExpense = entity.CrIntExpense;
                        liquidationExist.DrIntPayable = entity.DrIntPayable;
                        liquidationExist.CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL;
                        liquidationExist.Account = entity.Account;
                        liquidationExist.PaymentAccount = entity.PaymentAccount;
                        liquidationExist.Active = true;
                        liquidationExist.Deleted = false;
                        liquidationExist.UpdatedBy = user.UserName;
                        liquidationExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    liquidationExist = new inf_liquidation
                    {
                        LiquidationId = entity.LiquidationId,
                        InvestorFundId = entity.InvestorFundId,
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.EffectiveDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        LiquidationDate = entity.LiquidationDate,
                        EarlyTerminationCharge = entity.EarlyTerminationCharge,
                        AmountPayable = entity.AmountPayable,
                        DrProductPrincipal = entity.DrProductPrincipal,
                        CrIntExpense = entity.CrIntExpense,
                        DrIntPayable = entity.DrIntPayable,
                        CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL,
                        Account = entity.Account,
                        PaymentAccount = entity.PaymentAccount,
                        Active = true,
                        Deleted = false,
                        CreatedBy = user.UserName,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.inf_liquidation.Add(liquidationExist);
                }
                try
                {
                    using (var _trans = _dataContext.Database.BeginTransaction())
                    {
                        var output = _dataContext.SaveChanges() > 0;
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                            subject = "Successful Liquidation Application",
                            content = $"Hi {customer.FirstName}, <br> Your liquidation application is successful and awaiting approval.",
                            sendIt = true,
                        });

                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = staff.email, name = user.FirstName}
                            },
                            subject = "Successful Liquidation Application",
                            content = $"Hi {staff.firstName}, <br> Appraisal is pending for liquidation application with Investment reference : " + investment.RefNumber + "",
                            sendIt = true,
                        });
                        var targetIds = new List<int>();
                        targetIds.Add(liquidationExist.LiquidationId);
                        var request = new GoForApprovalRequest
                        {
                            StaffId = user.StaffId,
                            CompanyId = 1,
                            StatusId = (int)ApprovalStatus.Pending,
                            TargetId = targetIds,
                            Comment = "Liquidation Approval",
                            OperationId = (int)OperationsEnum.LiquidationApproval,
                            DeferredExecution = true, // false by default will call the internal SaveChanges() 
                            ExternalInitialization = true,
                            EmailNotification = true,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(request);
                        try
                        {
                            if (!result.IsSuccessStatusCode)
                            {
                                new ApprovalRegRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                    }
                                };
                            }
                            var stringData = await result.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);
                            if (res.ApprovalProcessStarted)
                            {
                                liquidationExist.WorkflowToken = res.Status.CustomToken;
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new LiquidationRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = res.Status.IsSuccessful,
                                        Message = res.Status.Message
                                    }
                                };
                            }

                            if (res.EnableWorkflow || !res.HasWorkflowAccess)
                            {
                                await _trans.RollbackAsync();
                                return new LiquidationRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = false,
                                        Message = res.Status.Message
                                    }
                                };
                            }
                            if (!res.EnableWorkflow)
                            {
                                var response = LiquidationApproval(liquidationExist.LiquidationId, 2);
                                await _dataContext.SaveChangesAsync();
                                await _trans.CommitAsync();
                                return new LiquidationRespObj
                                {
                                    Status = new APIResponseStatus
                                    {
                                        IsSuccessful = true,
                                        Message = new APIResponseMessage
                                        {
                                            FriendlyMessage = "Successful"
                                        }
                                    }
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            _trans.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                    return new LiquidationRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = " Not Successful"
                            }
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLiquidation(int id)
        {
            var itemToDelete = _dataContext.inf_liquidation.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return _dataContext.SaveChanges() > 0;
        }

        public byte[] GenerateExportLiquidation()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Customer");
            dt.Columns.Add("Product");
            dt.Columns.Add("Proposed Tenor");
            dt.Columns.Add("Proposed Rate");
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Period");
            dt.Columns.Add("Proposed Amount");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Effective Date");
            dt.Columns.Add("Investment Purpose");
            dt.Columns.Add("Liquidation Date");
            dt.Columns.Add("Early Termination Charge");
            dt.Columns.Add("Amount Payable");
            dt.Columns.Add("Dr Product Principal");
            dt.Columns.Add("Cr Int Expense");
            dt.Columns.Add("Dr Int Payable");
            dt.Columns.Add("Cr Receiver Principal GL");
            var statementType = (from a in _dataContext.inf_liquidation
                                 where a.Deleted == false
                                 select new LiquidationObj
                                 {
                                     ProductId = a.ProductId,
                                     InvestorFundCustomerId = a.InvestorFundCustomerId,
                                     ProposedTenor = a.ProposedTenor,
                                     ProposedRate = a.ProposedRate,
                                     FrequencyId = a.FrequencyId,
                                     Period = a.Period,
                                     ProposedAmount = a.ProposedAmount,
                                     CurrencyId = a.CurrencyId,
                                     EffectiveDate = a.EffectiveDate,
                                     InvestmentPurpose = a.InvestmentPurpose,
                                     LiquidationDate = a.LiquidationDate,
                                     EarlyTerminationCharge = a.EarlyTerminationCharge,
                                     AmountPayable = a.AmountPayable,
                                     DrProductPrincipal = a.DrProductPrincipal,
                                     CrIntExpense = a.CrIntExpense,
                                     DrIntPayable = a.DrIntPayable,
                                     CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                 }).ToList();

            foreach (var kk in statementType)
            {
                var row = dt.NewRow();
                row["Product"] = kk.ProductId;
                row["Customer"] = kk.InvestorFundCustomerId;
                row["Proposed Tenor"] = kk.ProposedTenor;
                row["Proposed Rate"] = kk.ProposedRate;
                row["Frequency"] = kk.FrequencyId;
                row["Period"] = kk.Period;
                row["Proposed Amount"] = kk.ProposedAmount;
                row["Currency"] = kk.CurrencyId;
                row["Effective Date"] = kk.EffectiveDate;
                row["Investment Purpose"] = kk.InvestmentPurpose;
                row["Liquidation Date"] = kk.LiquidationDate;
                row["Early Termination Charge"] = kk.EarlyTerminationCharge;
                row["Amount Payable"] = kk.AmountPayable;
                row["Dr Product Principal"] = kk.DrProductPrincipal;
                row["Cr Int Expense"] = kk.CrIntExpense;
                row["Dr Int Payable"] = kk.DrIntPayable;
                row["Cr Receiver Principal GL"] = kk.CrReceiverPrincipalGL;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("LiquidationOperation");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public List<LiquidationObj> GetAllLiquidation()
        {
            try
            {
                var product = (from a in _dataContext.inf_liquidation
                               where a.Deleted == false
                               select

                               new LiquidationObj
                               {
                                   LiquidationId = a.LiquidationId,
                                   InvestorFundId = a.InvestorFundId,
                                   ProductId = a.ProductId,
                                   InvestorFundCustomerId = a.InvestorFundCustomerId,
                                   ProposedTenor = a.ProposedTenor,
                                   ProposedRate = a.ProposedRate,
                                   FrequencyId = a.FrequencyId,
                                   /*MaturityDate = b.MaturityDate,
                                   FrequencyName = _dataContext.credit_frequencytype.Where(x => x.FrequencyTypeId == a.FrequencyId).FirstOrDefault().Mode,
                                   RelationshipManager = _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().FirstName + " " + _dataContext.cor_staff.Where(x => x.StaffId == c.RelationshipManagerId).FirstOrDefault().LastName,*/
                                   Period = a.Period,
                                   ProposedAmount = a.ProposedAmount,
                                   PaymentAccount = a.PaymentAccount,
                                   Account = a.Account,
                                   /*RefNumber = b.RefNumber,
                                   CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().CurrencyName,
                                   ExchangeRate = _dataContext.cor_currencyrate.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().SellingRate,*/
                                   CurrencyId = a.CurrencyId,
                                   EffectiveDate = a.EffectiveDate,
                                   InvestmentPurpose = a.InvestmentPurpose,
                                   LiquidationDate = a.LiquidationDate,
                                   EarlyTerminationCharge = a.EarlyTerminationCharge,
                                   AmountPayable = a.AmountPayable,
                                   DrProductPrincipal = a.DrProductPrincipal,
                                   CrIntExpense = a.CrIntExpense,
                                   DrIntPayable = a.DrIntPayable,
                                   CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                               }).ToList();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LiquidationObj> GetLiquidation(int Id)
        {
            try
            {
                var CurrencyList = await _serverRequest.GetCurrencyAsync();
                var staffResponse = await _serverRequest.GetAllStaffAsync();
                var _FrequencyType = await _dataContext.credit_frequencytype.Where(d => d.Deleted == false).ToListAsync();
                var loanCust = _dataContext.credit_loancustomer.Where(d => d.Deleted == false).ToList();
                var investorFundList = _dataContext.inf_investorfund.Where(s => loanCust.Select(d => d.CustomerId).Contains(s.InvestorFundCustomerId)).ToList();
                var a = _dataContext.inf_liquidation.FirstOrDefault(x => x.LiquidationId == Id);
                var investFundObj = investorFundList.FirstOrDefault(x => x.InvestorFundId == a.InvestorFundId);
                var staffId = loanCust.FirstOrDefault(a => a.CustomerId == investFundObj.InvestorFundCustomerId)?.RelationshipManagerId;

                           var product = new LiquidationObj()
                              {
                                  LiquidationId = a.LiquidationId,
                                  InvestorFundId = a.InvestorFundId,
                                  ProductId = a.ProductId,
                                  InvestorFundCustomerId = a.InvestorFundCustomerId,
                                  ProposedTenor = a.ProposedTenor,
                                  ProposedRate = a.ProposedRate,
                                  FrequencyId = a.FrequencyId,
                                  MaturityDate = investFundObj.MaturityDate,
                                  FrequencyName = _FrequencyType.FirstOrDefault(x => x.FrequencyTypeId == a.FrequencyId).Mode,
                                  Period = a.Period,
                                  ProposedAmount = a.ProposedAmount,
                                  PaymentAccount = a.PaymentAccount,
                                  Account = a.Account,
                                  RefNumber = investFundObj.RefNumber,
                                  CurrencyId = a.CurrencyId,
                                  EffectiveDate = a.EffectiveDate,
                                  InvestmentPurpose = a.InvestmentPurpose,
                                  LiquidationDate = a.LiquidationDate,
                                  EarlyTerminationCharge = a.EarlyTerminationCharge,
                                  AmountPayable = a.AmountPayable,
                              };
                if (staffId != null)
                {
                    product.RelationshipManager = staffResponse.staff.FirstOrDefault(x => x.staffId == staffId)?.firstName + "  " + staffResponse.staff.FirstOrDefault(x => x.staffId == staffId)?.lastName;
                }
                product.CurrencyName = CurrencyList.commonLookups.FirstOrDefault(x => x.LookupId == a.CurrencyId)?.LookupName;
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LiquidationRegRespObj> UpdateLiquidationOperationByCustomer(LiquidationObj entity)
        {
            try
            {
                if (entity == null) return new LiquidationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid request payload" } }
                };
                var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                if (customer.RelationshipManagerId == null)
                {
                    return new LiquidationRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer doesn't have Relationship Manager" } } };
                }
                var staffList = await _serverRequest.GetAllStaffAsync();
                var staff = staffList.staff.FirstOrDefault(x => x.staffId == customer.RelationshipManagerId);

                var user = _serverRequest.UserDataAsync().Result;

                inf_liquidation_website collectionExist = null;
                if (entity.LiquidationId > 0)
                {
                    collectionExist = _dataContext.inf_liquidation_website.Find(entity.LiquidationId);
                    if (collectionExist != null)
                    {
                        collectionExist.InvestorFundId = entity.InvestorFundId;
                        collectionExist.InvestorFundCustomerId = entity.InvestorFundCustomerId;
                        collectionExist.ProductId = entity.ProductId;
                        collectionExist.ProposedTenor = entity.ProposedTenor;
                        collectionExist.ProposedRate = entity.ProposedRate;
                        collectionExist.FrequencyId = entity.FrequencyId;
                        collectionExist.Period = entity.Period;
                        collectionExist.ProposedAmount = entity.ProposedAmount;
                        collectionExist.CurrencyId = entity.CurrencyId;
                        collectionExist.EffectiveDate = entity.EffectiveDate;
                        collectionExist.InvestmentPurpose = entity.InvestmentPurpose;
                        collectionExist.LiquidationDate = entity.LiquidationDate;
                        collectionExist.EarlyTerminationCharge = entity.EarlyTerminationCharge;
                        collectionExist.AmountPayable = entity.AmountPayable;
                        collectionExist.DrProductPrincipal = entity.DrProductPrincipal;
                        collectionExist.CrIntExpense = entity.CrIntExpense;
                        collectionExist.DrIntPayable = entity.DrIntPayable;
                        collectionExist.CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL;
                        collectionExist.Account = entity.Account;
                        collectionExist.PaymentAccount = entity.PaymentAccount;
                        collectionExist.Active = true;
                        collectionExist.Deleted = false;
                        collectionExist.Updatedby = user.UserName;
                        collectionExist.Updatedon = DateTime.Today;
                    }
                }
                else
                {
                    collectionExist = new inf_liquidation_website
                    {
                        WebsiteLiquidationOperationId = entity.WebsiteLiquidationOperationId,
                        InvestorFundId = entity.InvestorFundId,
                        InvestorFundCustomerId = entity.InvestorFundCustomerId,
                        ProductId = entity.ProductId,
                        ProposedTenor = entity.ProposedTenor,
                        ProposedRate = entity.ProposedRate,
                        FrequencyId = entity.FrequencyId,
                        Period = entity.Period,
                        ProposedAmount = entity.ProposedAmount,
                        CurrencyId = entity.CurrencyId,
                        EffectiveDate = entity.EffectiveDate,
                        InvestmentPurpose = entity.InvestmentPurpose,
                        LiquidationDate = entity.LiquidationDate,
                        EarlyTerminationCharge = entity.EarlyTerminationCharge,
                        AmountPayable = entity.AmountPayable,
                        DrProductPrincipal = entity.DrProductPrincipal,
                        CrIntExpense = entity.CrIntExpense,
                        DrIntPayable = entity.DrIntPayable,
                        CrReceiverPrincipalGL = entity.CrReceiverPrincipalGL,
                        Account = entity.Account,
                        PaymentAccount = entity.PaymentAccount,
                        ApprovalStatus = (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress,
                        Active = true,
                        Deleted = false,
                        Createdby = user.UserName,
                        Createdon = DateTime.Today,
                    };
                    _dataContext.inf_liquidation_website.Add(collectionExist);
                }

                var response = _dataContext.SaveChanges() > 0;

                if (response)
                {
                    await _serverRequest.SendMail(new MailObj
                    {
                        fromAddresses = new List<FromAddress> { },
                        toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                        subject = "New Liquidation Request",
                        content = $"Hi {staff.firstName}, <br> A new liquidation application has been made by <br/> Investor : " + customer.FirstName + " " + customer.LastName + "",
                        sendIt = true,
                    });
                }

                return new LiquidationRegRespObj { Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Unsuccessful" } } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LiquidationApprovalRecommendationObj UpdateLiquidationRecommendation(LiquidationApprovalRecommendationObj entity, string users)
        {
            try
            {
                if (entity == null) return null;

                var user = _serverRequest.UserDataAsync().Result;
                if (entity.InvInvestorFundId > 0)
                {

                    var ApplicationExist = _dataContext.inf_investorfund.Find(entity.InvInvestorFundId);
                    if (ApplicationExist != null)
                    {

                        ApplicationExist.ApprovedAmount = entity.ApprovedAmount;
                        ApplicationExist.ApprovedProductId = entity.ApprovedProductId;
                        ApplicationExist.ApprovedRate = entity.ApprovedRate;
                        ApplicationExist.ApprovedTenor = entity.ApprovedTenor;
                        ApplicationExist.UpdatedBy = user.UserName;
                        ApplicationExist.UpdatedOn = DateTime.Today;

                        var log = new inf_liquidationrecommendationLog
                        {
                            InvInvestorFundId = entity.InvInvestorFundId,
                            ApprovedAmount = entity.ApprovedAmount,
                            ApprovedProductId = entity.ApprovedProductId,
                            ApprovedRate = entity.ApprovedRate,
                            ApprovedTenor = entity.ApprovedTenor,
                            CreatedBy = user.UserName,
                            CreatedOn = DateTime.Now
                        };
                        _dataContext.inf_liquidationrecommendationlog.Add(log);
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                if (response)
                {
                    return new LiquidationApprovalRecommendationObj
                    {
                        InvInvestorFundId = entity.InvInvestorFundId,
                        ApprovedAmount = entity.ApprovedAmount,
                        ApprovedProductId = entity.ApprovedProductId,
                        ApprovedRate = entity.ApprovedRate,
                        ApprovedTenor = entity.ApprovedTenor,

                    };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LiquidationRecommendationLogObj> GetLiquidationRecommendationLog(int InvestorFundId)
        {
            var user = _serverRequest.UserDataAsync().Result;
            var log = (from a in _dataContext.inf_liquidationrecommendationlog
                       where a.InvInvestorFundId == InvestorFundId
                       orderby a.CreatedOn descending
                       select new LiquidationRecommendationLogObj
                       {
                           InvInvestorFundId = a.InvInvestorFundId,
                           Amount = a.ApprovedAmount,
                           ProductId = a.ApprovedProductId,
                           ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                           Rate = a.ApprovedRate,
                           Tenor = a.ApprovedTenor,
                           CreatedBy = user.UserName,
                       }).ToList();
            return log;
        }

        public async Task<ActionResult<LiquidationRespObj>> GetLiquidationForAppraisalAsync()
        {
            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new LiquidationRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}" }
                        }
                    };
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return new LiquidationRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new LiquidationRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "No Pending Approval"
                            }
                        }
                    };

                }
                var liquidation = await GetLiquidationAwaitingApprovalAsync(res.workflowTasks.Select(x => x.TargetId).ToList(), res.workflowTasks.Select(d => d.WorkflowToken).ToList());


                return new LiquidationRespObj
                {
                    InvestmentLists = liquidation.Select(a => new InvestmentListObj
                    {
                        ApplicationDate = (DateTime)a.CreatedOn,
                        InvestorFundCustomerId = a.InvestorFundCustomerId,
                        ProposedAmount = a.ProposedAmount,
                        InvestorFundId = a.InvestorFundId,
                        InvestorName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.InvestorFundCustomerId).LastName,
                        ProductName = _dataContext.inf_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                        ApprovalStatus = a.ApprovalStatus,
                        RefNumber = _dataContext.inf_investorfund.FirstOrDefault(x=>x.InvestorFundId == a.InvestorFundId).RefNumber,
                        LiquidationId = a.LiquidationId,
                        workflowToken = a.WorkflowToken,
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = liquidation.Count() < 1 ? "No liquidation awaiting approvals" : null
                        }
                    }
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private async Task<IEnumerable<inf_liquidation>> GetLiquidationAwaitingApprovalAsync(List<int> LiquidationIds, List<string> tokens)
        {
            var item = await _dataContext.inf_liquidation
                .Where(s => LiquidationIds.Contains(s.LiquidationId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public async Task<bool> LiquidationApproval(int targetId, int approvalStatusId)
        {
            bool response = false;
            var liquidationApplication = _dataContext.inf_liquidation.Find(targetId);
            liquidationApplication.ApprovalStatus = approvalStatusId;
            var investment = _dataContext.inf_investorfund.Find(liquidationApplication.InvestorFundId);
            investment.InvestmentStatus = (int)InvestmentStatus.Liquidated;
            var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
            response = _dataContext.SaveChanges() > 0;
            if (response)
            {
                var liquidationEntry1 = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = investment.CompanyId,
                    DebitAmount = (decimal)investment.ApprovedAmount,
                    CreditAmount = (decimal)liquidationApplication.EarlyTerminationCharge,
                    Amount = (decimal)investment.ApprovedAmount,
                    CurrencyId = investment.CurrencyId ?? 0,
                    Description = "Investment Liquidation Payment Posting",
                    DebitGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).ProductPrincipalGl.Value,
                    CreditGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).InterstExpenseGl.Value,
                    ReferenceNo = investment.RefNumber,
                    OperationId = 16,
                    JournalType = "System",
                    RateType = 2,//Buying Rate
                };
                var liquidationEntry2 = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = investment.CompanyId,
                    DebitAmount = (decimal)liquidationApplication.AmountPayable - (decimal)investment.ApprovedAmount,
                    CreditAmount = (decimal)liquidationApplication.AmountPayable,
                    Amount = 0,
                    CurrencyId = investment.CurrencyId ?? 0,
                    Description = "Investment Liquidation Payment Posting",
                    DebitGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).InterestPayableGl.Value,
                    CreditGL = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == investment.ProductId).ReceiverPrincipalGl.Value,
                    ReferenceNo = investment.RefNumber,
                    OperationId = 16,
                    JournalType = "System",
                    RateType = 2,//Buying Rate
                };

                if (liquidationApplication.PaymentAccount == "2")///Pay to other banks
                {
                    var flutter = _serverRequest.GetFlutterWaveKeys().Result;
                    if (flutter.keys.useFlutterWave)
                    {
                        var account = liquidationApplication.Account.Split("-");
                        var currencyList = _serverRequest.GetCurrencyAsync().Result;
                        var currency = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == investment.CurrencyId)?.Code;
                        TransferObj payWithFlutterWave = new TransferObj
                        {
                            account_bank = account[0].Trim(),
                            account_number = account[2].Trim(),
                            amount = Convert.ToInt32(liquidationApplication.AmountPayable),
                            narration = "Investment Liquidation",
                            currency = currency,
                            reference = investment.RefNumber.Trim()
                        };
                        var res = _flutter.createTransfer(payWithFlutterWave).Result;
                    }
                }
                
                var res1 = _serverRequest.PassEntryToFinance(liquidationEntry1).Result;
                var res2 = _serverRequest.PassEntryToFinance(liquidationEntry2).Result;
                if (res1.Status.IsSuccessful == false)
                {
                    //throw new Exception(res1.Status.Message.FriendlyMessage, new Exception());
                }
                else if(res2.Status.IsSuccessful == false)
                {
                    //throw new Exception(res2.Status.Message.FriendlyMessage, new Exception());
                }
                await _serverRequest.SendMail(new MailObj
                {
                    fromAddresses = new List<FromAddress> { },
                    toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                    subject = "Liquidation Successfully Approved",
                    content = $"Hi {customer.FirstName}, <br> Your liquidation request has been finally approved. <br/> Amount Payable: {liquidationApplication.AmountPayable}",
                    sendIt = true,
                });
            }
            return response;
        }

        public IEnumerable<LiquidationObj> GetAllLiquidationOperationWebsiteList()
        {
            try
            {
                var accountType = (from a in _dataContext.inf_liquidation_website
                                   join b in _dataContext.credit_loancustomer on a.InvestorFundCustomerId equals b.CustomerId
                                   join c in _dataContext.inf_investorfund on a.InvestorFundId equals c.InvestorFundId
                                   where a.Deleted == false && a.ApprovalStatus == (int)WebsiteLoanApplicationStatusEnum.ApplicationInProgress
                                   select new LiquidationObj
                                   {
                                       WebsiteLiquidationOperationId = a.WebsiteLiquidationOperationId,
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       LiquidationDate = a.LiquidationDate,
                                       EarlyTerminationCharge = a.EarlyTerminationCharge,
                                       AmountPayable = a.AmountPayable,
                                       DrProductPrincipal = a.DrProductPrincipal,
                                       CrIntExpense = a.CrIntExpense,
                                       DrIntPayable = a.DrIntPayable,
                                       CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                       PaymentAccount = a.PaymentAccount,
                                       Account = a.Account,
                                       InvestorName = b.FirstName + " " + b.LastName,
                                       CustomerTypeId = b.CustomerTypeId,
                                       CustomerTypeName = b.CustomerTypeId == 2 ? "Corporate" : "Individual",
                                       RefNumber = c.RefNumber,
                                       S_date = a.Createdon
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<DailyInterestAccrualObj> ProcessDailyInvestmentInterestAccrual(DateTime applicationDate)
        {
            var data = (from x in _dataContext.inf_investdailyschedule
                        join b in _dataContext.inf_investorfund on x.InvestorFundId equals b.InvestorFundId
                        where x.PeriodDate.Value.Date == applicationDate.Date && b.ApprovalStatus == 2

                        select new DailyInterestAccrualObj()
                        {
                            referenceNumber = b.RefNumber,
                            productId = b.ProductId ?? 0,
                            companyId = b.CompanyId ?? 0,
                            currencyId = b.CurrencyId ?? 0,
                            interestRate = (double)b.ApprovedRate,
                            date = applicationDate,
                            dailyAccuralAmount = (double)x.InterestAmount,
                            mainAmount = x.InterestAmount ?? 0,
                            categoryId = (short)DailyAccrualCategory.TermLoan,
                            transactionTypeId = (byte)LoanTransactionTypeEnum.Interest,

                        }).ToList();

            List<inf_daily_accural> transAccrual = new List<inf_daily_accural>();


            foreach (var item in data)
            {
                inf_daily_accural dailyAccrual = new inf_daily_accural();

                dailyAccrual.ReferenceNumber = item.referenceNumber;
                dailyAccrual.ProductId = item.productId;
                dailyAccrual.ExchangeRate = item.exchangeRate;
                dailyAccrual.CurrencyId = item.currencyId;
                dailyAccrual.InterestRate = item.interestRate;
                dailyAccrual.Date = item.date;
                dailyAccrual.DailyAccuralAmount = (decimal)Math.Abs(item.dailyAccuralAmount);
                dailyAccrual.Amount = item.mainAmount;
                dailyAccrual.CategoryId = item.categoryId;
                dailyAccrual.CompanyId = item.companyId;
                dailyAccrual.TransactionTypeId = item.transactionTypeId;


                transAccrual.Add(dailyAccrual);

            }
            this._dataContext.inf_daily_accural.AddRange(transAccrual);
            _dataContext.SaveChanges();

            var model = (from a in _dataContext.inf_daily_accural
                         where a.Date.Date == applicationDate.Date && a.CategoryId == (short)DailyAccrualCategory.TermLoan
                         group a by new { a.ProductId, a.CompanyId, a.CurrencyId } into groupedQ
                         select new DailyInterestAccrualObj()
                         {
                             productId = groupedQ.Key.ProductId,
                             companyId = groupedQ.Key.CompanyId,
                             currencyId = groupedQ.Key.CurrencyId,
                             dailyAccuralAmount = (double)groupedQ.Sum(i => i.DailyAccuralAmount),
                         }).ToList();

            var transList = new List<TransactionObj>();
            foreach (var item in model)
            {
                item.date = applicationDate;
                var product = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == item.productId);
                if (product == null)
                {

                }
                if (product.InterstExpenseGl == null)
                {

                }
                if (product.InterestPayableGl == null)
                {

                }
                var entry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = string.Empty,
                    CompanyId = item.CompanyId,
                    Amount = Convert.ToDecimal(item.dailyAccuralAmount),
                    CurrencyId = item.currencyId,
                    Description = "Investment Daily Interest Accrual Posting",
                    DebitGL = product.InterstExpenseGl.Value,
                    CreditGL = product.InterestPayableGl.Value,
                    ReferenceNo = item.referenceNumber,
                    OperationId = (int)OperationsEnum.DailyInterestAccural,
                    JournalType = "System",
                    RateType = 2,//Buying Rate
                };
                //transList.Add(entry);
                var res1 = _serverRequest.PassEntryToFinance(entry).Result;
            }
            //var res1 = _serverRequest.PassEntryToFinance(transList).Result;
            return model;
        }

        public LiquidationObj GetLiquidationOperationWebsitebyId(int id)
        {
            try
            {
                var accountType = (from a in _dataContext.inf_liquidation_website
                                   where a.Deleted == false && a.WebsiteLiquidationOperationId == id
                                   select new LiquidationObj
                                   {
                                       WebsiteLiquidationOperationId = a.WebsiteLiquidationOperationId,
                                       InvestorFundId = a.InvestorFundId,
                                       ProductId = a.ProductId,
                                       InvestorFundCustomerId = a.InvestorFundCustomerId,
                                       ProposedTenor = a.ProposedTenor,
                                       ProposedRate = a.ProposedRate,
                                       FrequencyId = a.FrequencyId,
                                       Period = a.Period,
                                       ProposedAmount = a.ProposedAmount,
                                       CurrencyId = a.CurrencyId,
                                       EffectiveDate = a.EffectiveDate,
                                       InvestmentPurpose = a.InvestmentPurpose,
                                       LiquidationDate = a.LiquidationDate,
                                       EarlyTerminationCharge = a.EarlyTerminationCharge,
                                       AmountPayable = a.AmountPayable,
                                       DrProductPrincipal = a.DrProductPrincipal,
                                       CrIntExpense = a.CrIntExpense,
                                       DrIntPayable = a.DrIntPayable,
                                       CrReceiverPrincipalGL = a.CrReceiverPrincipalGL,
                                       PaymentAccount = a.PaymentAccount,
                                       Account = a.Account,
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}




