using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.Response.Finance;
using Banking.Contracts.Response.FlutterWave;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Helpers;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanRepository : ILoanRepository
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private ILoanScheduleRepository _schedule;
        private readonly IIdentityService _identityService;
        private readonly IFlutterWaveRequest _flutter;
        private readonly IConfiguration _configuration;

        public LoanRepository(DataContext context, ILoanScheduleRepository schedule, IConfiguration configuration, IIdentityService identityService, IIdentityServerRequest serverRequest, IFlutterWaveRequest flutter)
        {
            _dataContext = context;
            _schedule = schedule;
            _identityService = identityService;
            _serverRequest = serverRequest;
            _flutter = flutter;
            _configuration = configuration;
        }
        public async Task<LoanRespObj> AddLoanBooking(LoanObj entity)
        {
            try
            {
                entity.OperationId = (int)OperationsEnum.LoanDisburment;
                if (entity == null) return null;
                var application = _dataContext.credit_loanapplication.Find(entity.LoanApplicationId);

                if (entity.MaturityDate <= entity.EffectiveDate)
                    throw new Exception("Loan terminal date should be more than effective date", new Exception());

                if (entity.EffectiveDate > entity.MaturityDate)
                    throw new Exception("The effective cannot be greater than maturity date", new Exception());

                if (entity.EffectiveDate >= DateTime.Now)
                    throw new Exception("You effective date cannot be back-dated.", new Exception());

                var loanRefNumber = GenerateLoanReferenceNumber(entity.ProductId);
                entity.LoanReferenceNumber = loanRefNumber;
                var user = _serverRequest.UserDataAsync().Result;

                credit_loan loan = null;

                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        loan = new credit_loan
                        {
                            CustomerId = entity.CustomerId,
                            ProductId = entity.ProductId,
                            LoanApplicationId = entity.LoanApplicationId,
                            PrincipalFrequencyTypeId = entity.PrincipalFrequencyTypeId,
                            InterestFrequencyTypeId = entity.InterestFrequencyTypeId,
                            ScheduleTypeId = entity.ScheduleTypeId,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            ApprovalStatusId = (int)ApprovalStatus.Approved,
                            ApprovedBy = user.StaffId,
                            ApprovedComments = entity.ApprovedComments,
                            ApprovedDate = entity.ApprovedDate,
                            BookingDate = DateTime.Now,
                            EffectiveDate = entity.EffectiveDate,
                            MaturityDate = entity.MaturityDate,
                            LoanStatusId = entity.LoanStatusId,
                            IntegralFeeAmount = (decimal)entity.FeeAmount,
                            IsDisbursed = false,
                            DisbursedBy = null,
                            DisbursedComments = "",
                            DisbursedDate = null,
                            CompanyId = entity.CompanyId,
                            LoanRefNumber = loanRefNumber,
                            PrincipalAmount = entity.PrincipalAmount,
                            EquityContribution = entity.EquityContribution,
                            FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate,
                            FirstInterestPaymentDate = entity.FirstInterestPaymentDate,
                            OutstandingPrincipal = entity.PrincipalAmount - entity.IntegralFeeAmount,
                            OutstandingInterest = entity.OutstandingInterest,
                            NPLDate = entity.NplDate,
                            CustomerRiskRatingId = entity.CustomerRiskRatingId,
                            LoanOperationId = (int)OperationsEnum.LoanBookingApproval,
                            StaffId = user.StaffId,
                            AccrualBasis = entity.AccrualBasis,
                            FirstDayType = entity.FirstDayType,
                            PastDueInterest = 0,
                            PastDuePrincipal = 0,
                            InterestRate = (double)entity.InterestRate,
                            InterestOnPastDueInterest = 0,
                            InterestOnPastDuePrincipal = 0,
                            CasaAccountId = entity.CasaAccountId,
                            BranchId = entity.BranchId,
                            Active = true,
                            Deleted = false,
                            CreatedBy = entity.CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        _dataContext.credit_loan.Add(loan);
                        application.LoanApplicationStatusId = (int)ApplicationStatus.LoanBooking;
                        await _dataContext.SaveChangesAsync();
                        var targetIds = new List<int>();
                        targetIds.Add(loan.LoanId);
                        /////////PUSHING TO WORKFLOW
                        var request = new GoForApprovalRequest
                        {
                            StaffId = user.StaffId,
                            CompanyId = 1,
                            StatusId = (int)ApprovalStatus.Pending,
                            TargetId = targetIds,
                            Comment = "Loan Booking Approval",
                            OperationId = (int)OperationsEnum.LoanBookingApproval,
                            DeferredExecution = true, // false by default will call the internal SaveChanges() 
                            ExternalInitialization = true,
                            EmailNotification = true,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(request);


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
                            application.LoanApplicationStatusId = (int)ApplicationStatus.LoanBooking;
                            loan.WorkflowToken = res.Status.CustomToken;
                            await _dataContext.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new LoanRespObj
                            {
                                LoanRefNumber = loan.LoanRefNumber,
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
                            return new LoanRespObj
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
                            await _trans.CommitAsync();
                            return new LoanRespObj
                            {
                                LoanRefNumber = loan.LoanRefNumber,
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
                    catch (SqlException ex)
                    {
                        _trans.Rollback();
                        throw;
                    }
                    finally { _trans.Dispose(); }
                }

                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Loan Booking not successful" } }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteCreditLoanComment(int id)
        {
            try
            {
                var comment = _dataContext.credit_loancomment.Find(id);
                if (comment != null)
                {
                    comment.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteCreditLoanDecision(int id)
        {
            try
            {
                var decision = _dataContext.credit_loandecision.Find(id);
                if (decision != null)
                {
                    decision.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditLoanCommentObj> GetAllCreditLoanComment(int loanId, int loanscheduleId)
        {
            try
            {
                var accountType = (from a in _dataContext.credit_loancomment
                                   where a.Deleted == false && a.LoanId == loanId && a.LoanScheduleId == loanscheduleId
                                   select new CreditLoanCommentObj
                                   {
                                       LoanCommentId = a.LoanCommentId,
                                       LoanScheduleId = a.LoanScheduleId,
                                       Date = a.Date,
                                       Comment = a.Comment,
                                       NextStep = a.NextStep,
                                       LoanId = a.LoanId
                                   }).ToList();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<credit_loan> GetAllCreditLoan()
        {
            try
            {
                var loan = _dataContext.credit_loan.Where(x => x.Deleted == false).ToList();

                return loan;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<CreditLoanDecisionObj> GetAllCreditLoanDecision(int loanId, int loanscheduleId)
        {
            try
            {
                var decision = (from a in _dataContext.credit_loandecision
                                where a.Deleted == false && a.LoanId == loanId && a.LoanScheduleId == loanscheduleId
                                select new CreditLoanDecisionObj
                                {
                                    LoanDecisionId = a.LoanDecisionId,
                                    Date = a.Date,
                                    Decision = a.Decision,
                                    LoanId = a.LoanId,
                                    LoanScheduleId = a.LoanScheduleId,
                                }).ToList();

                return decision;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<credit_offerletter> GetOfferLetterByLoanApplicationAsync(int applicationId)
        {
            return await _dataContext.credit_offerletter.FirstOrDefaultAsync(d => d.Deleted == false && d.LoanApplicationId == applicationId);
        }
        public IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetterReviewed()
        {
            try {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false && a.ApprovalStatusId == (int)ApprovalStatus.Approved
                                       && a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           CompanyId = (int)a.CompanyId,
                                           //CompanyName = _dataContext.cor_company.Where(x => x.CompanyId == a.CompanyId).FirstOrDefault().Name,
                                           LoanApplicationStatusId = (int)a.LoanApplicationStatusId,
                                           CreditScore = a.Score,
                                           ProbabilityOfDefault = a.PD
                                       }).ToList();
                if (loanApplication.Count > 0)
                {
                    foreach (var item in loanApplication)
                    {
                        item.IntegralFee = CalculateIntegralFee(item.ApprovedProductId, item.ApprovedAmount);
                    }
                }
                return loanApplication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LoanApplicationObj> GetAllLoanApplicationOfferLetterReviewedById(int id)
        {
            try
            {
                var loanApplication = (from a in _dataContext.credit_loanapplication
                                       where a.Deleted == false && a.ApprovalStatusId == (int)ApprovalStatus.Approved
                                       && a.LoanApplicationStatusId == (int)ApplicationStatus.OfferLetter && a.LoanApplicationId == id
                                       orderby a.ApplicationDate descending
                                       select new LoanApplicationObj
                                       {
                                           ApplicationDate = a.ApplicationDate,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedProductId = a.ApprovedProductId,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           CurrencyId = a.CurrencyId,
                                           CustomerId = a.CustomerId,
                                           EffectiveDate = a.EffectiveDate,
                                           ExchangeRate = a.ExchangeRate,
                                           HasDoneChecklist = a.HasDoneChecklist,
                                           MaturityDate = a.MaturityDate,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedProductId = a.ProposedProductId,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           LoanApplicationId = a.LoanApplicationId,
                                           //CurrencyName = _dataContext.cor_currency.Where(x => x.CurrencyId == a.CustomerId).FirstOrDefault().CurrencyName,
                                           CustomerName = _dataContext.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName,
                                           ApprovedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ApprovedProductId).FirstOrDefault().ProductName,
                                           ProposedProductName = _dataContext.credit_product.Where(x => x.ProductId == a.ProposedProductId).FirstOrDefault().ProductName,
                                           ApplicationRefNumber = a.ApplicationRefNumber,
                                           CompanyId = (int)a.CompanyId,
                                           //CompanyName = _dataContext.cor_company.Where(x => x.CompanyId == a.CompanyId).FirstOrDefault().Name,
                                           LoanApplicationStatusId = (int)a.LoanApplicationStatusId,
                                           CreditScore = a.Score,
                                           ProbabilityOfDefault = a.PD,
                                           //integralFee = CalculateIntegralFee(a.approvedProductId, approvedAmount)
                                       }).ToList();
                foreach (var item in loanApplication)
                {
                    item.IntegralFee = CalculateIntegralFee(item.ApprovedProductId, item.ApprovedAmount);
                }

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditLoanCommentObj GetCreditLoanComment(int loanId, int loanscheduleId, int commentId)
        {
            try
            {
                var accountType = (from a in _dataContext.credit_loancomment
                                   where a.Deleted == false && a.LoanId == loanId && a.LoanScheduleId == loanscheduleId && a.LoanCommentId == commentId
                                   select new CreditLoanCommentObj
                                   {
                                       LoanCommentId = a.LoanCommentId,
                                       LoanScheduleId = a.LoanScheduleId,
                                       Date = a.Date,
                                       Comment = a.Comment,
                                       NextStep = a.NextStep,
                                       LoanId = a.LoanId
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CreditLoanDecisionObj GetCreditLoanDecision(int loanId, int loanscheduleId)
        {
            try
            {
                var accountType = (from a in _dataContext.credit_loandecision
                                   where a.Deleted == false && a.LoanId == loanId && a.LoanScheduleId == loanscheduleId
                                   select new CreditLoanDecisionObj
                                   {
                                       LoanDecisionId = a.LoanDecisionId,
                                       Date = a.Date,
                                       Decision = a.Decision,
                                       LoanId = a.LoanId,
                                       LoanScheduleId = a.LoanScheduleId,
                                   }).FirstOrDefault();

                return accountType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoanRespObj> GetLoanBookingAwaitingApproval(string userName)
        {

            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new LoanRespObj
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
                    return new LoanRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new LoanRespObj
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
                var targetIds = res.workflowTasks.Select(x => x.TargetId).ToList();
                var tokens = res.workflowTasks.Select(d => d.WorkflowToken).ToList();

                var loanCustomerList = _dataContext.credit_loancustomer.Where(d => d.Deleted == false).ToList();
                var loanApplicationList = _dataContext.credit_loanapplication.Where(s => s.Deleted == false).ToList();
                var loanList = await GetLoansAwaitingApprovalAsync(targetIds, tokens);
                var CurrencyList = await _serverRequest.GetCurrencyAsync();
                var datas = (from a in loanList
                             join b in loanApplicationList on a.LoanApplicationId equals b.LoanApplicationId
                             orderby a.BookingDate descending
                             select new LoanListObj
                             {
                                 LoanId = a.LoanId,
                                 CustomerId = b.CustomerId,
                                 CustomerName = loanCustomerList.FirstOrDefault(x => x.CustomerId == b.CustomerId).FirstName + " " + loanCustomerList.FirstOrDefault(x => x.CustomerId == b.CustomerId).LastName,
                                 ProductId = a.ProductId,
                                 ProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                 LoanApplicationId = a.LoanApplicationId,
                                 PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId,
                                 PrincipalFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.PrincipalFrequencyTypeId).Mode,
                                 InterestFrequencyTypeId = a.InterestFrequencyTypeId,
                                 InterestFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.InterestFrequencyTypeId).Mode,
                                 ScheduleTypeId = a.ScheduleTypeId,
                                 ScheduleType = _dataContext.credit_loanscheduletype.FirstOrDefault(x => x.LoanScheduleTypeId == a.ScheduleTypeId).LoanScheduleTypeName,
                                 CurrencyId = b.CurrencyId,
                                 Currency = CurrencyList.commonLookups.FirstOrDefault(d => d.LookupId == a.CurrencyId)?.LookupName ?? string.Empty,
                                 ExchangeRate = a.ExchangeRate,
                                 BookingDate = a.BookingDate,
                                 EffectiveDate = a.EffectiveDate,
                                 MaturityDate = a.MaturityDate,
                                 LoanStatusId = a.LoanStatusId,
                                 LoanRefNumber = a.LoanRefNumber,
                                 PrincipalAmount = a.PrincipalAmount,
                                 FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                 FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                 OutstandingPrincipal = a.OutstandingPrincipal,
                                 OutstandingInterest = a.OutstandingInterest,
                                 CreditScore = b.Score,
                                 ProbabilityOfDefault = b.PD,
                                 WorkflowToken = a.WorkflowToken,
                             }).ToList();

                var loan = datas.GroupBy(d => d.LoanApplicationId)
                      .Select(g => g.OrderByDescending(b => b.BookingDate).FirstOrDefault());

                return new LoanRespObj
                {
                    ManageLoans = datas,

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = datas.Count() < 1 ? "No Loan Application awaiting approvals" : null
                        }
                    }
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<LoanListObj> GetLoanDetailInformation(int loanId)
        {
            var now = DateTime.Now.Date;
            var CurrencyList = await _serverRequest.GetCurrencyAsync();
            var data = (from a in _dataContext.credit_loan
                        join b in _dataContext.credit_loanapplication on a.LoanApplicationId equals b.LoanApplicationId
                        where a.LoanId == loanId
                        select new LoanListObj
                        {
                            LoanId = a.LoanId,
                            CustomerId = b.CustomerId,
                            CustomerName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).LastName,
                            ProductId = a.ProductId,
                            ProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                            LoanApplicationId = a.LoanApplicationId,
                            PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId,
                            PrincipalFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.PrincipalFrequencyTypeId).Mode,
                            InterestFrequencyTypeId = a.InterestFrequencyTypeId,
                            InterestFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.InterestFrequencyTypeId).Mode,
                            ScheduleTypeId = a.ScheduleTypeId,
                            ScheduleType = _dataContext.credit_loanscheduletype.FirstOrDefault(x => x.LoanScheduleTypeId == a.ScheduleTypeId).LoanScheduleTypeName,
                            CurrencyId = b.CurrencyId,
                            ExchangeRate = a.ExchangeRate,
                            BookingDate = a.BookingDate,
                            EffectiveDate = a.EffectiveDate,
                            MaturityDate = a.MaturityDate,
                            LoanStatusId = a.LoanStatusId,
                            LoanRefNumber = a.LoanRefNumber,
                            PrincipalAmount = a.PrincipalAmount,
                            FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                            FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                            OutstandingPrincipal = a.OutstandingAmortisedPrincipal,
                            OutstandingAmortisedPrincipal = a.OutstandingPrincipal,
                            OutstandingInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date.Date == now.Date && x.Deleted == false).AccruedInterest,
                            OutstandingAmortisedInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date.Date == now.Date && x.Deleted == false).AmortisedAccruedInterest,
                            CreditScore = b.Score,
                            ProbabilityOfDefault = b.PD,
                            OperatingAccountBal = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).CASAAccountNumber).AvailableBalance,
                        }).FirstOrDefault();
            data.Currency = CurrencyList.commonLookups.FirstOrDefault(d => d.LookupId == data.CurrencyId)?.LookupName;
            return data;
        }
        public IEnumerable<LoanListObj> GetManagedLoanInformation(int loanId)
        {
            var now = DateTime.Now.Date;
            var data = (from a in _dataContext.credit_loanscheduleperiodic
                        join d in _dataContext.credit_loan on a.LoanId equals d.LoanId
                        join b in _dataContext.credit_loancustomer on d.CustomerId equals b.CustomerId
                        join c in _dataContext.credit_casa on b.CASAAccountNumber equals c.AccountNumber
                        where a.LoanId == loanId && a.Deleted == false && d.Deleted == false
                        select new LoanListObj
                        {
                            LoanScheduleId = a.LoanSchedulePeriodicId,
                            LoanId = a.LoanId,
                            CustomerId = b.CustomerId,
                            CustomerName = b.FirstName + " " + b.LastName,
                            PaymentDate = a.PaymentDate,
                            Repayment = a.PeriodPaymentAmount,
                            RepaymentActual = a.ActualRepayment + a.ActualRepaymentInterest,
                            RepaymentPending = a.PaymentPending + a.PaymentPendingInterest,
                            OutstandingBalance = (a.PaymentDate.Date <= now) ? a.EndPrincipalAmount : 0,
                            LoanStatus = (a.PaymentDate.Date > now) ? "Not Due" :
                                         (a.PaymentDate.Date <= now && c.AvailableBalance < a.PeriodPaymentAmount) ? "Due But Pending" :
                                         (a.PaymentDate.Date <= now && c.AvailableBalance == a.PeriodPaymentAmount) ? "Due But Partially Paid" :
                                         (a.PaymentDate.Date <= now && c.AvailableBalance > a.PeriodPaymentAmount) ? "Fully Paid" : null,
                            OperatingAccountBal = (a.PaymentDate.Date <= now) ? c.AvailableBalance : 0,
                            LoanRefNumber = d.LoanRefNumber,
                            PrincipalAmount = d.PrincipalAmount,
                            OutstandingPrincipal = d.OutstandingPrincipal,
                            OutstandingInterest = _dataContext.credit_loanscheduledaily.Where(x => x.LoanId == a.LoanId && x.Date.Date == now && x.Deleted == false).FirstOrDefault().AccruedInterest,
                            Commented = _dataContext.credit_loancomment.Where(x => x.LoanScheduleId == a.LoanSchedulePeriodicId).FirstOrDefault() == null ? false : true,
                            Decided = _dataContext.credit_loandecision.Where(x => x.LoanScheduleId == a.LoanSchedulePeriodicId).FirstOrDefault() == null ? false : true,
                        }).OrderByDescending(x=>x.LoanScheduleId).ToList();
            return data;
        }

        public IEnumerable<LoanListObj> GetPastDueLoanInformation()
        {
            var now = DateTime.Now.Date;
            var data = (from a in _dataContext.credit_loan
                        join b in _dataContext.credit_loanapplication on a.LoanApplicationId equals b.LoanApplicationId
                        join c in _dataContext.credit_loan_past_due on a.LoanId equals c.LoanId
                        join d in _dataContext.credit_loancustomer on a.CustomerId equals d.CustomerId
                        select new LoanListObj
                        {
                            LoanId = a.LoanId,
                            CustomerId = b.CustomerId,
                            CustomerName = d.FirstName + " " + d.LastName,
                            ProductId = a.ProductId,
                            ProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                            LoanApplicationId = a.LoanApplicationId,
                            PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId,
                            PrincipalFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.PrincipalFrequencyTypeId).Mode,
                            InterestFrequencyTypeId = a.InterestFrequencyTypeId,
                            InterestFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.InterestFrequencyTypeId).Mode,
                            ScheduleTypeId = a.ScheduleTypeId,
                            ScheduleType = _dataContext.credit_loanscheduletype.FirstOrDefault(x => x.LoanScheduleTypeId == a.ScheduleTypeId).LoanScheduleTypeName,
                            CurrencyId = b.CurrencyId,
                            //Currency = b.cor_currency.CurrencyName,
                            ExchangeRate = a.ExchangeRate,
                            BookingDate = a.BookingDate,
                            EffectiveDate = a.EffectiveDate,
                            MaturityDate = a.MaturityDate,
                            LoanStatusId = a.LoanStatusId,
                            LoanRefNumber = a.LoanRefNumber,
                            PrincipalAmount = a.PrincipalAmount,
                            FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                            FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                            OutstandingPrincipal = a.OutstandingAmortisedPrincipal,
                            OutstandingAmortisedPrincipal = a.OutstandingPrincipal,
                            OutstandingInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date == now && x.Deleted == false).AccruedInterest,
                            OutstandingAmortisedInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date == now && x.Deleted == false).AmortisedAccruedInterest,
                            CreditScore = b.Score,
                            ProbabilityOfDefault = b.PD,
                            PaymentDate = c.Date,
                            PaymentDateDefault = c.DateWithDefault,
                            Description = c.Description,
                            PastDueAmount = c.DebitAmount,
                            PastDueId = c.PastDueId,
                            OperatingAccountBal = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == d.CASAAccountNumber).AvailableBalance,
                            DaysInOverdue = ((TimeSpan)(DateTime.Today - c.Date.Date)).Days, //DbFunctions.DiffDays(c.Date, DateTime.Today), 
                            LateRepaymentCharge = ((_dataContext.credit_product.Where(x => x.ProductId == a.ProductId && x.Deleted == false).FirstOrDefault().LateTerminationCharge ?? 0) / 365) * (double)c.DebitAmount,
                        }).ToList();
            return data;
        }

        public IEnumerable<LoanListObj> GetPaymentDueLoanInformation()
        {
            var now = DateTime.Now.Date;
            var data = (from a in _dataContext.credit_loan
                        join b in _dataContext.credit_loanapplication on a.LoanApplicationId equals b.LoanApplicationId
                        join c in _dataContext.credit_loanscheduleperiodic on a.LoanId equals c.LoanId
                        join d in _dataContext.credit_loancustomer on a.CustomerId equals d.CustomerId
                        where c.PaymentDate == now && c.PeriodPaymentAmount != 0 && c.Deleted == false
                        select new LoanListObj
                        {
                            LoanId = a.LoanId,
                            CustomerId = b.CustomerId,
                            CustomerName = d.FirstName + " " + d.LastName,
                            ProductId = a.ProductId,
                            ProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                            LoanApplicationId = a.LoanApplicationId,
                            PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId,
                            PrincipalFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.PrincipalFrequencyTypeId).Mode,
                            InterestFrequencyTypeId = a.InterestFrequencyTypeId,
                            InterestFrequencyType = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == a.InterestFrequencyTypeId).Mode,
                            ScheduleTypeId = a.ScheduleTypeId,
                            ScheduleType = _dataContext.credit_loanscheduletype.FirstOrDefault(x => x.LoanScheduleTypeId == a.ScheduleTypeId).LoanScheduleTypeName,
                            CurrencyId = b.CurrencyId,
                            ExchangeRate = a.ExchangeRate,
                            BookingDate = a.BookingDate,
                            EffectiveDate = a.EffectiveDate,
                            MaturityDate = a.MaturityDate,
                            LoanStatusId = a.LoanStatusId,
                            LoanRefNumber = a.LoanRefNumber,
                            PrincipalAmount = a.PrincipalAmount,
                            FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                            FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                            OutstandingPrincipal = a.OutstandingAmortisedPrincipal,
                            OutstandingAmortisedPrincipal = a.OutstandingPrincipal,
                            OutstandingInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date == now && x.Deleted == false).AccruedInterest,
                            OutstandingAmortisedInterest = _dataContext.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == a.LoanId && x.Date == now && x.Deleted == false).AmortisedAccruedInterest,
                            CreditScore = b.Score,
                            ProbabilityOfDefault = b.PD,
                            Repayment = c.PeriodPaymentAmount,
                            RepaymentPending = _dataContext.credit_loanscheduleperiodic.Where(x => x.PaymentDate.Date > now && x.LoanId == a.LoanId && x.Deleted == false).Sum(x => x.PeriodPaymentAmount),
                            OperatingAccountBal = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == d.CASAAccountNumber).AvailableBalance,
                        }).ToList();
            return data;
        }

        public LoanPaymentScheduleInputObj GetScheduleInput(int loanApplicationId)
        {

            var loanApp = _dataContext.credit_loanapplication.Find(loanApplicationId);
            var product = _dataContext.credit_product.Find(loanApp.ApprovedProductId);
            var maturityDate = loanApp.EffectiveDate.AddDays(loanApp.ApprovedTenor);

            var paymentDate = loanApp.EffectiveDate.AddDays(30);

            var input = new LoanPaymentScheduleInputObj
            {
                ScheduleMethodId = (short)product.ScheduleTypeId,
                PrincipalAmount = (double)loanApp.ApprovedAmount,
                EffectiveDate = loanApp.EffectiveDate,
                InterestRate = loanApp.ApprovedRate,
                PrincipalFrequency = product.FrequencyTypeId,
                InterestFrequency = product.FrequencyTypeId,
                PrincipalFirstpaymentDate = paymentDate,
                InterestFirstpaymentDate = paymentDate,
                MaturityDate = maturityDate,
                AccurialBasis = 2,
                IntegralFeeAmount = 0,
                FirstDayType = 0,
                CreatedBy = loanApp.CreatedBy,
                IrregularPaymentSchedule = null
            };

            return input;
        }

        public int GoForApproval(ApprovalObj entity)
        {
            throw new NotImplementedException();
            //using (var trans = _dataContext.Database.BeginTransaction())
            //{
            //    try
            //    {

            //var user = _dataContext.cor_useraccount.Where(x => x.UserName.ToLower().Trim() == entity.createdBy.ToLower().Trim()).FirstOrDefault();

            //workflow.StaffId = user.StaffId;
            //workflow.CompanyId = 1;
            //workflow.StatusId = ((short)entity.approvalStatusId == (short)ApprovalStatus.Approved) ? (short)ApprovalStatus.Processing : (short)entity.approvalStatusId;
            //workflow.TargetId = entity.targetId.ToString();
            //workflow.Comment = entity.comment;
            //workflow.OperationId = (int)OperationsEnum.LoanBookingApproval;
            //workflow.EmailNotification = true;
            //// workflow.ExternalInitialization = false;
            //// workflow.DeferredExecution = true;
            //workflow.LogActivity();


            //  _dataContext.SaveChanges();

            //if (entity.ApprovalStatusId == (short)ApprovalStatus.Disapproved)
            //{
            //    var loaninfo = _dataContext.credit_loan.Find(entity.TargetId);
            //    loaninfo.ApprovalStatusId = (short)ApprovalStatus.Disapproved;
            //    _dataContext.SaveChanges();
            //    trans.Commit();
            //    return 2;
            //}

            //if (workflow.NewState == (int)ApprovalState.Ended)
            //{
            //    var response = DisburseLoan(Convert.ToInt32(entity.targetId), user.StaffId, entity.createdBy, entity.comment);

            //    if (response)
            //    {
            //        trans.Commit();
            //    }
            //    return 1;
            //}
            //else
            //{
            //    trans.Commit();
            //}

            //return 0;
            //}
            //catch (Exception ex)
            //{
            //    //trans.Rollback();
            //    throw ex;
            //}
            //}
        }

        public bool UpdateCreditLoanComment(CreditLoanCommentObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.LoanCommentId > 0)
                {
                    var accountTypeExist = _dataContext.credit_loancomment.Find(entity.LoanCommentId);
                    if (accountTypeExist != null)
                    {
                        accountTypeExist.LoanCommentId = entity.LoanCommentId;
                        accountTypeExist.Date = entity.Date;
                        accountTypeExist.Comment = entity.Comment;
                        accountTypeExist.NextStep = entity.NextStep;
                        accountTypeExist.LoanScheduleId = entity.LoanScheduleId;
                        accountTypeExist.LoanId = entity.LoanId;
                        accountTypeExist.Active = true;
                        accountTypeExist.Deleted = false;
                        accountTypeExist.UpdatedBy = entity.CreatedBy;
                        accountTypeExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var accountType = new credit_loancomment
                    {
                        LoanCommentId = entity.LoanCommentId,
                        Date = entity.Date,
                        Comment = entity.Comment,
                        NextStep = entity.NextStep,
                        LoanId = entity.LoanId,
                        LoanScheduleId = entity.LoanScheduleId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancomment.Add(accountType);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateCreditLoanDecision(CreditLoanDecisionObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.LoanDecisionId > 0)
                {
                    var accountTypeExist = _dataContext.credit_loandecision.Find(entity.LoanDecisionId);
                    if (accountTypeExist != null)
                    {
                        accountTypeExist.LoanDecisionId = entity.LoanDecisionId;
                        accountTypeExist.Date = entity.Date;
                        accountTypeExist.Decision = entity.Decision;
                        accountTypeExist.LoanId = entity.LoanId;
                        accountTypeExist.LoanScheduleId = entity.LoanScheduleId;
                        accountTypeExist.Active = true;
                        accountTypeExist.Deleted = false;
                        accountTypeExist.UpdatedBy = entity.CreatedBy;
                        accountTypeExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var accountType = new credit_loandecision
                    {
                        LoanDecisionId = entity.LoanDecisionId,
                        Date = entity.Date,
                        Decision = entity.Decision,
                        LoanId = entity.LoanId,
                        LoanScheduleId = entity.LoanScheduleId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loandecision.Add(accountType);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        /////PRIVATE METHODS
        ///
        private string GenerateLoanReferenceNumber(int productId)
        {
            var productCode = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == productId).ProductCode;
            var count = ((_dataContext.credit_loan.Count(x => x.ProductId == productId)) + 1);
            var reference = $"{productCode.ToUpper()}-{GeneralHelpers.GenerateZeroString(5) + count.ToString().PadRight(5)}";

            return reference;
        }
        private decimal CalculateIntegralFee(int productId, decimal loanAmount)
        {
            decimal integralFee = 0;
            var productFee = (from a in _dataContext.credit_productfee
                              join b in _dataContext.credit_fee on a.FeeId equals b.FeeId
                              where a.Deleted == false && b.IsIntegral == true && b.Deleted == false && a.ProductId == productId
                              select a).ToList();
            if (productFee.Count > 0)
            {
                foreach (var item in productFee)
                {
                    if (item.ProductFeeType == 1)
                    {
                        integralFee = integralFee + (decimal)item.ProductAmount;
                    }
                    else
                    {
                        var perCentAmount = (decimal)item.ProductAmount / 100 * loanAmount;
                        integralFee = integralFee + perCentAmount;
                    }
                }
            }
            return integralFee;
        }
        public ApprovalRegRespObj DisburseLoan(int targetId, int staffId, string createdBy, string comment)
        {
            try
            {
                ApprovalRegRespObj response = new ApprovalRegRespObj();
                var compResponse = _serverRequest.GetAllCompanyAsync().Result;
                var loan = _dataContext.credit_loan.Find(targetId);
                var _customer = _dataContext.credit_loancustomer.Find(loan.CustomerId);
                var _loanapplication = _dataContext.credit_loanapplication.Find(loan.LoanApplicationId);               

                double feeamount = (double)CalculateIntegralFee(loan.ProductId, loan.PrincipalAmount);
                var IntegralFeeList = _dataContext.credit_productfee.Where(x => x.ProductId == loan.ProductId && x.Deleted == false).ToList();


                LoanPaymentScheduleInputObj input = new LoanPaymentScheduleInputObj
                {
                    ScheduleMethodId = (short)loan.ScheduleTypeId,
                    PrincipalAmount = (double)loan.PrincipalAmount,
                    EffectiveDate = loan.EffectiveDate,
                    InterestRate = _loanapplication.ApprovedRate,
                    PrincipalFrequency = loan.PrincipalFrequencyTypeId,
                    InterestFrequency = loan.InterestFrequencyTypeId,
                    PrincipalFirstpaymentDate = (DateTime)loan.FirstPrincipalPaymentDate,
                    InterestFirstpaymentDate = (DateTime)loan.FirstInterestPaymentDate,
                    MaturityDate = loan.MaturityDate,
                    AccurialBasis = (int)loan.AccrualBasis,
                    IntegralFeeAmount = feeamount,
                    FirstDayType = (int)loan.FirstDayType,
                    CreatedBy = createdBy,
                    IrregularPaymentSchedule = null
                };

                deposit_accountsetup operatingAccount = new deposit_accountsetup();

                var accountNuber = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == loan.CustomerId);
                var depositCustomer = _dataContext.deposit_accountopening.Where(x => x.AccountNumber == accountNuber.CASAAccountNumber).FirstOrDefault();
                if (depositCustomer == null)
                {
                    operatingAccount = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == 3);
                }
                else
                {
                    operatingAccount = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == depositCustomer.AccountTypeId);
                }

                var LoanDisbursementEntry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = loan.CompanyId ?? 0,
                    Amount = loan.PrincipalAmount,
                    CurrencyId = loan.CurrencyId,
                    Description = "Loan Disbursement",
                    DebitGL = operatingAccount.GLMapping ?? 1,
                    CreditGL = operatingAccount.BankGl ?? 0,
                    ReferenceNo = loan.LoanRefNumber,
                    OperationId = loan.LoanOperationId ?? 0,
                    JournalType = "System",
                    RateType = 1,//Buying Rate
                };

                var IntegralFeeEntry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = loan.CompanyId ?? 0,
                    Amount = (decimal)feeamount,
                    CurrencyId = loan.CurrencyId,
                    Description = "Payment of Integral Fee",
                    DebitGL = operatingAccount.GLMapping ?? 1,
                    CreditGL = operatingAccount.BankGl ?? 0,
                    ReferenceNo = loan.LoanRefNumber,
                    OperationId = loan.LoanOperationId ?? 0,
                    JournalType = "System",
                    RateType = 1,//Buying Rate
                };

                CustomerTransactionObj loanDisbursementCustomerEntry = new CustomerTransactionObj
                {
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    Description = "Loan Disbursement",
                    TransactionDate = DateTime.Now,
                    ValueDate = DateTime.Now,
                    TransactionType = "Credit",
                    CreditAmount = loan.PrincipalAmount,
                    DebitAmount = 0,
                    Beneficiary = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == loan.CustomerId).FirstName + " "+ _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == loan.CustomerId).LastName,
                    ReferenceNo = loan.LoanRefNumber,
                };

                CustomerTransactionObj IntegralFeeCustomerEntry = new CustomerTransactionObj
                {
                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    Description = "Payment of Integral Fee",
                    TransactionDate = DateTime.Now,
                    ValueDate = DateTime.Now,
                    TransactionType = "Debit",
                    CreditAmount = 0,
                    DebitAmount = (decimal)feeamount,
                    Beneficiary = compResponse.companyStructures.FirstOrDefault(x=>x.companyStructureId == loan.CompanyId)?.name,
                    ReferenceNo = loan.LoanRefNumber,
                };

                if(_loanapplication.PaymentMode == 2)///Pay to other banks
                {
                    var account = _loanapplication.PaymentAccount.Split("-");
                    var currencyList = _serverRequest.GetCurrencyAsync().Result;
                    var currency = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == loan.CurrencyId)?.Code;
                    //TransferObj payWithFlutterWave = new TransferObj
                    //{
                    //    account_bank = account[0],
                    //    account_number = account[2],
                    //    amount = Convert.ToInt32(loan.PrincipalAmount),
                    //    narration = "Loan Disbursement",
                    //    currency = currency,
                    //    reference = loan.LoanRefNumber
                    //};
                    BulkData flutterwaveObj = new BulkData
                    {
                        bank_code = account[0],
                        account_number = account[2].Trim(),
                        amount = Convert.ToInt32(loan.PrincipalAmount),
                        narration = "Loan Disbursement",
                        currency = currency,
                        reference = loan.LoanRefNumber.Trim()
                    };
                    var bulkList = new List<BulkData> { flutterwaveObj };

                    BulkTransferObj transferObj = new BulkTransferObj
                    {
                        title = "Loan Disbursement",
                        bulk_data = bulkList
                    };

                    response.FlutterObj = transferObj;
                }
                

                //Update Product Fee Status
                foreach (var curr in IntegralFeeList)
                {
                    List<credit_productfeestatus> statuses = new List<credit_productfeestatus>();
                    var status = new credit_productfeestatus();
                    status.LoanApplicationId = loan.LoanApplicationId;
                    status.ProductFeeId = curr.ProductFeeId;
                    status.Status = true;
                    statuses.Add(status);
                    _dataContext.credit_productfeestatus.AddRange(statuses);
                    _dataContext.SaveChanges();
                }


                var res = _serverRequest.PassEntryToFinance(LoanDisbursementEntry).Result;

                var res2 = _serverRequest.PassEntryToFinance(IntegralFeeEntry).Result;

                //if (res.Status.IsSuccessful == false)
                //{
                //    return new ApprovalRegRespObj
                //    {
                //        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res.Status.Message.FriendlyMessage } }
                //    };
                //}

                //if(res2.Status.IsSuccessful == false)
                //{
                //    return new ApprovalRegRespObj
                //    {
                //        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res2.Status.Message.FriendlyMessage } }
                //    };
                //}

                response.loanPayment = input;
                response.AnyIdentifier = targetId;
                response.DisbursementEntry = loanDisbursementCustomerEntry;
                response.IntegralFeeEntry = IntegralFeeCustomerEntry;

                //_ifrs.UpdateScoreCardHistoryByLoanDisbursement(loan.LoanId, createdBy);

                loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
                loan.ApprovedBy = staffId;
                loan.ApprovedComments = comment;
                loan.ApprovedDate = DateTime.Now;

                loan.DisbursedBy = staffId;
                loan.DisbursedComments = comment;
                loan.DisbursedDate = DateTime.Now;
                loan.IsDisbursed = true;
                loan.LoanStatusId = (int)LoanStatusEnum.Active;
                _loanapplication.LoanApplicationStatusId = (int)ApplicationStatus.Disbursed;
                _dataContext.SaveChanges();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
        private async Task<IEnumerable<credit_loan>> GetLoansAwaitingApprovalAsync(List<int> LoanIds, List<string> tokens)
        {
            var item = await _dataContext.credit_loan
                .Where(s => LoanIds.Contains(s.LoanId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        ///Cheque information
        ///
        public bool AddUpdateLoanCheque(loan_cheque_obj entity)
        {
            try
            {
                if (entity == null) return false;
                var oldrecords = _dataContext.credit_loan_cheque.FirstOrDefault(x=>x.LoanId == entity.LoanId);
                if (oldrecords != null)
                {
                    var oldList = _dataContext.credit_loan_cheque_list.Where(x => x.LoanId == entity.LoanId).ToList();
                    if(oldList.Count() > 0)
                    {
                        _dataContext.credit_loan_cheque_list.RemoveRange(oldList);
                    }
                    _dataContext.credit_loan_cheque.Remove(oldrecords);
                    _dataContext.SaveChanges();
                }
                credit_loan_cheque accountType = null;
                if (entity.LoanChequeId > 0)
                {
                    accountType = _dataContext.credit_loan_cheque.Find(entity.LoanChequeId);
                    if (accountType != null)
                    {
                        accountType.Start = entity.Start;
                        accountType.End = entity.End;
                        accountType.GeneralUpload = entity.GeneralUpload;
                        accountType.LoanId = entity.LoanId;
                        accountType.Active = true;
                        accountType.Deleted = false;
                        accountType.UpdatedBy = entity.CreatedBy;
                        accountType.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    accountType = new credit_loan_cheque
                    {
                        LoanChequeId = entity.LoanChequeId,
                        Start = entity.Start,
                        End = entity.End,
                        GeneralUpload = entity.GeneralUpload,
                        LoanId = entity.LoanId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loan_cheque.Add(accountType);
                }

                var response = _dataContext.SaveChanges() > 0;

                if (response)
                {
                    int start, end;
                    start = Convert.ToInt32(entity.Start); end = Convert.ToInt32(entity.End);
                    var zeroSpaces = entity.Start.Length;
                    int count = Convert.ToInt32(end - start);
                    var chequeNo = entity.Start;
                    for(int x=0; x <= count; x++)
                    {
                        var obj = new credit_loan_cheque_list
                        {
                            LoanChequeListId = 0,
                            LoanChequeId = accountType.LoanChequeId,
                            ChequeNo = chequeNo,
                            LoanId = entity.LoanId,
                            Status = 1,//Not Cleared
                            //SingleUpload = entity.SingleUpload,
                            Active = true,
                            Deleted = false,
                            CreatedBy = entity.CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        chequeNo = (Convert.ToInt32(chequeNo) + 1).ToString();
                        chequeNo = chequeNo.PadLeft(zeroSpaces, '0');
                        _dataContext.credit_loan_cheque_list.Add(obj);
                        _dataContext.SaveChanges();                    
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanChequeRespObj UploadSingleCheque(loan_cheque_obj entity)
        {
            if (entity == null)
            {
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid data" } }
                };
            }

            var accountType = _dataContext.credit_loan_cheque_list.Find(entity.LoanChequeListId);
            if (accountType != null)
            {
                accountType.SingleUpload = entity.SingleUpload;
                accountType.StatusName = entity.StatusName;
            }
           var isDone = _dataContext.SaveChanges() > 0;
            return new LoanChequeRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
            };
        }

        public LoanChequeRespObj UpdateChequeStatus(loan_cheque_obj entity)
        {
            if (entity == null)
            {
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid data" } }
                };
            }

            var accountType = _dataContext.credit_loan_cheque_list.Find(entity.LoanChequeListId);
            if (accountType != null)
            {
                accountType.Status = entity.Status;
            }
            var isDone = _dataContext.SaveChanges() > 0;
            return new LoanChequeRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
            };
        }

        public LoanChequeRespObj DownloadCheque(loan_cheque_obj entity)
        {
            if (entity == null)
            {
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Invalid data" } }
                };
            }

            var accountType = _dataContext.credit_loan_cheque_list.Find(entity.LoanChequeListId);
            var val2 = accountType.StatusName.Split(".")[1];
            if (accountType != null)
            {
                return new LoanChequeRespObj
                {
                    FileExtension = "."+ val2,
                    FileName = accountType.StatusName,
                    Export = accountType.SingleUpload,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            return new LoanChequeRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Document was not uploaded" } }
            };
        }


        public LoanChequeRespObj GetAllLoanChequeList(int loanId)
        {
            try
            {
                var chequeList = (from a in _dataContext.credit_loan_cheque_list
                                where a.Deleted == false && a.LoanId == loanId
                                 select new loan_cheque_obj
                                {
                                    LoanChequeListId = a.LoanChequeListId,
                                    LoanChequeId = a.LoanChequeId,
                                    SingleUpload = a.SingleUpload,
                                    Status = a.Status,
                                    StatusName = a.Status == 1 ? "Not Cleared" : a.Status == 2 ? "Cleared": a.Status == 3 ? "Presented" : null,
                                    ChequeNo = a.ChequeNo,
                                }).ToList();
                var chequeobj = _dataContext.credit_loan_cheque.FirstOrDefault(x => x.LoanId == loanId);
                if(chequeobj == null)
                {
                    return new LoanChequeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    };
                }
                return new LoanChequeRespObj
                {
                    Start = chequeobj.Start,
                    End = chequeobj.End,
                    LoanCheque = chequeList,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LoanRepaymentRespObj repaymentWithFlutterWave(repaymentObj model)
        {
            var customer = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == model.customerId);
            var loanSchedule = _dataContext.credit_loanscheduleperiodic.Find(model.loanScheduleId);
            var loan = _dataContext.credit_loan.FirstOrDefault(x => x.LoanId == loanSchedule.LoanId);
            var currencyList = _serverRequest.GetCurrencyAsync().Result;

            var currencyCode = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == loan.CurrencyId).Code;
            var webUrl = _configuration.GetValue<string>("FrontEndUrl:webUrl");
            var paymentObj = new PaymentObj
            {
                tx_ref = loan.LoanRefNumber,
                currency = currencyCode,
                amount = Convert.ToString(loanSchedule.PeriodPrincipalAmount),
                redirect_url = webUrl + "/#/dashboard",
                payment_options = "card, mobilemoney, ussd, banktransfer, barter, paga, credit",
                customer = new Customer
                {
                    name = customer.FirstName + " " + customer.LastName,
                    email = customer.Email,
                    phonenumber = customer.PhoneNo
                },
                customizations = new Customizations
                {
                    title = "Loan Repayment",
                    description = "Loan Repayment",
                    logo = "https://dl.dropboxusercontent.com/s/eu501mhy21003hh/gos2.png"
                }
            };
            var res = _flutter.makePayment(paymentObj).Result;
            if (res.status.ToLower() == "success")
            {
                return new LoanRepaymentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } },
                    Link = res.data.link
                };
            }
            return new LoanRepaymentRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } },
            };
        }

        public LoanRepaymentRespObj repaymentZeroWithFlutterWave(repaymentObj model)
        {
            var customer = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == model.customerId);
            var loan = _dataContext.credit_loan.FirstOrDefault(x => x.LoanId == model.loanId);
            var currencyList = _serverRequest.GetCurrencyAsync().Result;

            var currencyCode = currencyList.commonLookups.FirstOrDefault(x => x.LookupId == loan.CurrencyId).Code;
            var webUrl = _configuration.GetValue<string>("FrontEndUrl:webUrl");
            var paymentObj = new PaymentObj
            {
                tx_ref = loan.LoanRefNumber,
                currency = currencyCode,
                amount = Convert.ToString(0),
                redirect_url = webUrl + "/#/dashboard",
                payment_options = "card, mobilemoney, ussd, banktransfer, barter, paga, credit",
                customer = new Customer
                {
                    name = customer.FirstName + " " + customer.LastName,
                    email = customer.Email,
                    phonenumber = customer.PhoneNo
                },
                customizations = new Customizations
                {
                    title = "Loan Repayment",
                    description = "Loan Repayment",
                    logo = "https://dl.dropboxusercontent.com/s/eu501mhy21003hh/gos2.png"
                }
            };
            var res = _flutter.makePayment(paymentObj).Result;
            if (res.status.ToLower() == "success")
            {
                return new LoanRepaymentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } },
                    Link = res.data.link
                };
            }
            return new LoanRepaymentRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } },
            };
        }
    }
}
