using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    public class LoanManagementRepository : ILoanManagementRepository
    {
        private readonly DataContext _context;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public LoanManagementRepository(DataContext context, IIdentityService identityService, IIdentityServerRequest serverRequest)
        {
            _context = context;
            _identityService = identityService;
            _serverRequest = serverRequest;
        }

        public async Task<bool> AddTempLoanBooking(LoanObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                //var user = identity.UserName;
                entity.OperationId = (int)OperationsEnum.LoanDisburment;
                if (entity == null) return false;
                var application = _context.credit_loanapplication.Find(entity.LoanApplicationId);

                if (entity.MaturityDate <= entity.EffectiveDate)
                    throw new Exception("Error Ocurred", new Exception("Loan terminal date should be more than effective date"));

                if (entity.EffectiveDate > entity.MaturityDate)
                    throw new Exception("The effective cannot be greater than maturity date");

                if (entity.EffectiveDate >= DateTime.Now)
                    throw new Exception("You effective date cannot be back-dated.");


                var now = DateTime.Now.Date;
                var OriginalLoan = _context.credit_loan.Where(x => x.LoanId == entity.LoanId).FirstOrDefault();
                decimal OldBalance = 0;
                decimal OldInterest = 0;
                var accruedInterest = _context.credit_loanscheduledaily.Where(x => x.Date == now && x.LoanId == entity.LoanId && x.Deleted == false).FirstOrDefault();
                if (accruedInterest != null)
                {
                    OldBalance = OriginalLoan.OutstandingPrincipal ?? 0 + accruedInterest.AccruedInterest;
                    OldInterest = accruedInterest.AccruedInterest;
                }
                else
                {
                    OldBalance = OriginalLoan.OutstandingPrincipal ?? 0;
                }
               
                
                credit_loan_temp loan = null;

                loan = _context.credit_loan_temp.Where(x => x.TargetId == entity.TargetId).FirstOrDefault();
                if (loan != null)
                {
                    loan.LoanId = entity.LoanId;
                    loan.TargetId = entity.TargetId;
                    loan.CustomerId = entity.CustomerId;
                    loan.ProductId = entity.ProductId;
                    loan.LoanApplicationId = entity.LoanApplicationId;
                    loan.PrincipalFrequencyTypeId = entity.PrincipalFrequencyTypeId;
                    loan.InterestFrequencyTypeId = entity.InterestFrequencyTypeId;
                    loan.ScheduleTypeId = entity.ScheduleTypeId;
                    loan.CurrencyId = entity.CurrencyId;
                    loan.ExchangeRate = entity.ExchangeRate;
                    loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
                    loan.ApprovedBy = user.StaffId;
                    loan.ApprovedComments = entity.ApprovedComments;
                    loan.ApprovedDate = entity.ApprovedDate;
                    loan.BookingDate = DateTime.Now;
                    loan.EffectiveDate = entity.EffectiveDate;
                    loan.MaturityDate = entity.MaturityDate;
                    loan.LoanStatusId = entity.LoanStatusId;
                    loan.IsDisbursed = false;
                    loan.DisbursedBy = null;
                    loan.DisbursedComments = "";
                    loan.DisbursedDate = null;
                    loan.CompanyId = entity.CompanyId;
                    loan.PrincipalAmount = entity.PrincipalAmount;
                    loan.EquityContribution = entity.EquityContribution;
                    loan.FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate;
                    loan.FirstInterestPaymentDate = entity.FirstInterestPaymentDate;
                    loan.OutstandingPrincipal = entity.PrincipalAmount;
                    loan.OutstandingInterest = entity.OutstandingInterest;
                    loan.OldBalance = OldBalance;
                    loan.OldInterest = OldInterest;
                    loan.NPLDate = entity.NplDate;
                    loan.CustomerRiskRatingId = entity.CustomerRiskRatingId;
                    loan.LoanOperationId = (int)OperationsEnum.LoanBookingApproval;
                    loan.StaffId = user.StaffId;
                    loan.AccrualBasis = entity.AccrualBasis;
                    loan.FirstDayType = entity.FirstDayType;
                    loan.PastDueInterest = 0;
                    loan.PastDuePrincipal = 0;
                    loan.InterestRate = (double)entity.InterestRate;
                    loan.InterestOnPastDueInterest = 0;
                    loan.InterestOnPastDuePrincipal = 0;
                    loan.CasaAccountId = entity.CasaAccountId;
                    loan.BranchId = entity.BranchId;
                    loan.Active = true;
                    loan.Deleted = false;
                    loan.UpdatedBy = entity.CreatedBy;
                    loan.UpdatedOn = DateTime.Now;
                }
                else
                {
                    loan = new credit_loan_temp
                    {
                        LoanId = entity.LoanId,
                        TargetId = entity.TargetId,
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
                        IsDisbursed = false,
                        DisbursedBy = null,
                        DisbursedComments = "",
                        DisbursedDate = null,
                        CompanyId = entity.CompanyId,
                        PrincipalAmount = entity.PrincipalAmount,
                        EquityContribution = entity.EquityContribution,
                        FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate,
                        FirstInterestPaymentDate = entity.FirstInterestPaymentDate,
                        OutstandingPrincipal = entity.PrincipalAmount,
                        OutstandingInterest = entity.OutstandingInterest,
                        OldBalance = OldBalance,
                        OldInterest = OldInterest,
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
                    _context.credit_loan_temp.Add(loan);
                }
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LoanReviewListObjRespObj> AddUpdateLMSApplication(LoanReviewApplicationObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();

                if (entity == null)
                    return new LoanReviewListObjRespObj
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
                credit_loanreviewapplication loanReview = null;
                using (var _trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                if (entity.LoanReviewApplicationId > 0)
                {
                    loanReview = _context.credit_loanreviewapplication.Find(entity.LoanReviewApplicationId);
                    if (loanReview != null)
                    {
                        loanReview.OperationId = entity.OperationId;
                        loanReview.ReviewDetails = entity.ReviewDetails;
                        loanReview.ApprovedAmount = entity.ProposedAmount;
                        loanReview.ApprovedRate = entity.ProposedRate;
                        loanReview.ApprovedTenor = entity.ProposedTenor;
                        loanReview.Prepayment = entity.Prepayment;
                        loanReview.ProposedAmount = entity.ProposedAmount;
                        loanReview.ProposedRate = entity.ProposedRate;
                        loanReview.ProposedTenor = entity.ProposedTenor;
                        loanReview.PrincipalFrequencyTypeId = entity.PrincipalFrequency;
                        loanReview.InterestFrequencyTypeId = entity.InterestFrequency;
                        loanReview.FirstInterestPaymentDate = entity.FirstInterestPaymentDate;
                        loanReview.FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate;
                        loanReview.Active = true;
                        loanReview.Deleted = false;
                        loanReview.UpdatedBy = entity.CreatedBy;
                        loanReview.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    loanReview = new credit_loanreviewapplication
                    {
                        LoanId = entity.LoanId,
                        ProductId = entity.ProductId,
                        OperationId = entity.OperationId,
                        CustomerId = entity.CustomerId,
                        ReviewDetails = entity.ReviewDetails,
                        ApprovalStatusId = (int)ApprovalStatus.Pending,
                        GenerateOfferLetter = false,
                        OperationPerformed = false,
                        ApprovedAmount = entity.ProposedAmount,
                        Prepayment = entity.Prepayment,
                        ApprovedRate = entity.ProposedRate,
                        ApprovedTenor = entity.ProposedTenor,
                        ProposedAmount = entity.ProposedAmount,
                        ProposedRate = entity.ProposedRate,
                        ProposedTenor = entity.ProposedTenor,
                        InterestFrequencyTypeId = entity.InterestFrequency,
                        PrincipalFrequencyTypeId = entity.PrincipalFrequency,
                        FirstInterestPaymentDate = entity.FirstInterestPaymentDate,
                        FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _context.credit_loanreviewapplication.Add(loanReview);
                }
                await _context.SaveChangesAsync();
                var targetIds = new List<int>();
                targetIds.Add(loanReview.LoanReviewApplicationId);

                var request = new GoForApprovalRequest
                {
                    StaffId = user.StaffId,
                    CompanyId = 1,
                    StatusId = (int)ApprovalStatus.Pending,
                    TargetId = targetIds,
                    Comment = "Loan Review Application Approval",
                    OperationId = (int)OperationsEnum.LoanReviewApplication,
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
                            loanReview.WorkflowToken = res.Status.CustomToken;
                            await _context.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new LoanReviewListObjRespObj
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
                            return new LoanReviewListObjRespObj
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
                            await _context.SaveChangesAsync();
                            await _trans.CommitAsync();
                            return new LoanReviewListObjRespObj
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
                        return new LoanReviewListObjRespObj
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
                    catch (SqlException ex)
                    {
                        _trans.Rollback();
                        throw;
                    }
                    finally { _trans.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanReviewApplicationObj> GetAllLoanReviewApplication()
        {
            try
            {
                var loanApplication = (from a in _context.credit_loanreviewapplication
                                       join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                                       join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                                       where a.Deleted == false
                                       orderby a.CreatedOn descending
                                       select new LoanReviewApplicationObj
                                       {
                                           LoanReviewApplicationId = a.LoanReviewApplicationId,
                                           LoanId = a.LoanId,
                                           ProductId = a.ProductId,
                                           OperationId = a.OperationId,
                                           CustomerId = a.CustomerId,
                                           ReviewDetails = a.ReviewDetails,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           GenerateOfferLetter = a.GenerateOfferLetter,
                                           OperationPerformed = a.OperationPerformed,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).FirstName
                                           + " " + _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).LastName,
                                           ProductName = _context.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                           InterestFrequency = a.InterestFrequencyTypeId,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           PrincipalFrequencyName = e.Mode,
                                           InterestFrequencyName = f.Mode,

                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanReviewListObj> GetAllLoanReviewApplicationOfferLetter()
        {
            try
            {
                var loanReview = (from a in _context.credit_loanreviewapplication
                                  join b in _context.credit_loan on a.LoanId equals b.LoanId
                                  join c in _context.credit_loanapplication on b.LoanApplicationId equals c.LoanApplicationId
                                  where a.Deleted == false && a.ApprovalStatusId == (int)ApprovalStatus.Approved && a.GenerateOfferLetter == true
                                  && a.GenerateOfferLetter == true
                                  select new LoanReviewListObj
                                  {
                                      LoanReviewApplicationId = a.LoanReviewApplicationId,
                                      LoanId = a.LoanId,
                                      CustomerId = a.CustomerId,
                                      CustomerName = _context.credit_loancustomer.FirstOrDefault(x=>x.CustomerId == b.CustomerId).FirstName + " " + _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).LastName,
                                      ProductId = a.ProductId,
                                      ProductName = _context.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                      LoanApplicationId = b.LoanApplicationId,
                                      LoanRefNumber = b.LoanRefNumber,
                                      PrincipalAmount = b.PrincipalAmount,
                                      PropposedAmount = a.ApprovedAmount,
                                      PropposedInterestRate = a.ApprovedRate,
                                      PropposedTenor = a.ApprovedTenor,
                                      BookingDate = b.BookingDate,
                                      CreditScore = c.Score,
                                      ProbabilityOfDefault = c.PD,
                                      OperationType = a.Prepayment == 0 ? "Loan Review Application" : "Prepayment"
                                  }).ToList().OrderByDescending(x => x.LoanReviewApplicationId);
                return loanReview;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanReviewListObj> GetAllRunningLoan(string searchQuery, int? customerTypeId, int? productTypeId)
        {
            IEnumerable<LoanReviewListObj> data = null;
            var LoanReviewApplication = new List<LoanReviewListObj>();
            var now = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(searchQuery.Trim()))
            {
                var creditLoanOverDue = (from a in _context.credit_loan_repayment
                                         join b in _context.credit_loan on a.LoanId equals b.LoanId
                                         into nloans
                                         from c in nloans.DefaultIfEmpty()
                                         select new
                                         {
                                             a.Date,
                                             c.PastDuePrincipal,
                                             c.LoanId
                                         }).Union(from f in _context.credit_loan
                                                  where f.PastDuePrincipal == 0
                                                  let Date = f.CreatedOn ?? DateTime.Now
                                                  select new
                                                  {
                                                      Date,
                                                      f.PastDuePrincipal,
                                                      f.LoanId
                                                  }).ToList();

                var ids = creditLoanOverDue.Select(x => x.LoanId).Distinct();

                foreach (var id in ids)
                {
                    var earliestDate = creditLoanOverDue.Where(x => x.LoanId == id).OrderBy(y => y.Date).First();
                    var Range = ((TimeSpan)(DateTime.Today - earliestDate.Date)).Days;

                    data = (from a in _context.credit_loan
                            join b in _context.credit_loanapplication on a.LoanApplicationId equals b.LoanApplicationId
                            join c in _context.credit_loancustomer on b.CustomerId equals c.CustomerId
                            join d in _context.credit_product on b.ApprovedProductId equals d.ProductId
                            join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                            join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                            where a.IsDisbursed == true && a.LoanId == id
                            select new LoanReviewListObj
                            {
                                LoanId = a.LoanId,
                                CustomerId = b.CustomerId,
                                CustomerTypeId = c.CustomerTypeId,
                                CustomerName = c.FirstName + " " + c.LastName,
                                ProductId = a.ProductId,
                                ProductTypeId = d.ProductTypeId,
                                ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                LoanApplicationId = a.LoanApplicationId,
                                LoanRefNumber = a.LoanRefNumber,
                                Status = _context.credit_creditclassification.Where(x => x.UpperLimit >= Range && Range >= x.LowerLimit).FirstOrDefault().Description,
                                PrincipalAmount = a.PrincipalAmount,
                                PropposedAmount = a.OutstandingPrincipal + Convert.ToDecimal(_context.credit_loanscheduledaily.Where(x => x.LoanId == a.LoanId && x.Deleted == false && x.Date.Date == now.Date).FirstOrDefault().AccruedInterest),
                                EffectiveDate = a.EffectiveDate,
                                PropposedInterestRate = b.ApprovedRate,
                                PropposedTenor = b.ApprovedTenor,
                                BookingDate = a.BookingDate,
                                CreditScore = b.Score,
                                ProbabilityOfDefault = b.PD,
                                FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                InterestFrequency = a.InterestFrequencyTypeId,
                                PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                PrincipalFrequencyName = e.Mode,
                                InterestFrequencyName = f.Mode,

                            }).AsQueryable();
                    if (customerTypeId != null)
                    {
                        data = data.Where(d => d.CustomerTypeId == customerTypeId);
                    }
                    if (productTypeId != null)
                    {
                        data = data.Where(d => d.ProductTypeId == productTypeId);
                    }
                    if (searchQuery != null)
                    {
                        data = data.Where(d => d.CustomerName.ToLower().Contains(searchQuery.ToLower()));
                    }
                    var loan = data.GroupBy(d => d.LoanApplicationId)
                          .Select(g => g.OrderByDescending(b => b.BookingDate).FirstOrDefault());
                    LoanReviewApplication.AddRange(loan);
                }
            }
            return LoanReviewApplication;
        }

        public IEnumerable<LoanReviewListObj> GetAllRunningLoanByCustomerId(int customerId)
        {
            IEnumerable<LoanReviewListObj> data = null;
            var LoanReviewApplication = new List<LoanReviewListObj>();
            var now = DateTime.Now.Date;

            data = (from a in _context.credit_loan
                    join b in _context.credit_loanapplication on a.LoanApplicationId equals b.LoanApplicationId
                    join c in _context.credit_loancustomer on b.CustomerId equals c.CustomerId
                    join d in _context.credit_product on b.ApprovedProductId equals d.ProductId
                    join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                    join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                    where a.IsDisbursed == true && b.CustomerId == customerId
                    select new LoanReviewListObj
                    {
                        LoanId = a.LoanId,
                        CustomerId = b.CustomerId,
                        CustomerTypeId = c.CustomerTypeId,
                        CustomerName = b.credit_loancustomer.FirstName + " " + b.credit_loancustomer.LastName,
                        ProductId = a.ProductId,
                        ProductTypeId = d.ProductTypeId,
                        ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                        LoanApplicationId = a.LoanApplicationId,
                        LoanRefNumber = a.LoanRefNumber,
                        PrincipalAmount = a.PrincipalAmount,
                        PropposedAmount = a.OutstandingPrincipal + _context.credit_loanscheduledaily.Where(x => x.LoanId == a.LoanId && x.Deleted == false && x.Date == now).FirstOrDefault().AccruedInterest,
                        EffectiveDate = a.EffectiveDate,
                        PropposedInterestRate = b.ApprovedRate,
                        PropposedTenor = b.ApprovedTenor,
                        BookingDate = a.BookingDate,
                        CreditScore = b.Score,
                        ProbabilityOfDefault = b.PD,
                        FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                        FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                        InterestFrequency = a.InterestFrequencyTypeId,
                        PrincipalFrequency = a.PrincipalFrequencyTypeId,
                        PrincipalFrequencyName = e.Mode,
                        InterestFrequencyName = f.Mode,
                    }).AsQueryable();
            var loan = data.GroupBy(d => d.LoanApplicationId)
                  .Select(g => g.OrderByDescending(b => b.BookingDate).FirstOrDefault());
            LoanReviewApplication.AddRange(loan);
            return LoanReviewApplication;
        }

        public LoanReviewApplicationObj GetLoanReviewApplicationbyLoanId(int loanId)
        {
            try
            {
                var loanApplication = (from a in _context.credit_loanreviewapplication
                                       join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                                       join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                                       where a.Deleted == false && a.LoanId == loanId
                                       orderby a.CreatedOn descending
                                       select new LoanReviewApplicationObj
                                       {
                                           LoanReviewApplicationId = a.LoanReviewApplicationId,
                                           LoanId = a.LoanId,
                                           ProductId = a.ProductId,
                                           OperationId = a.OperationId,
                                           CustomerId = a.CustomerId,
                                           ReviewDetails = a.ReviewDetails,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           GenerateOfferLetter = a.GenerateOfferLetter,
                                           OperationPerformed = a.OperationPerformed,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ProposedAmount = a.ProposedAmount,
                                           Prepayment = a.Prepayment,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName
                                           + " " + _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().LastName,
                                           ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           InterestFrequency = a.InterestFrequencyTypeId,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           PrincipalFrequencyName = e.Mode,
                                           InterestFrequencyName = f.Mode,
                                       }).FirstOrDefault();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoanReviewListObjRespObj> GetLoanReviewAwaitingApproval()
        {
            try
            {
                var now = DateTime.Now.Date;
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new LoanReviewListObjRespObj
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
                    return new LoanReviewListObjRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new LoanReviewListObjRespObj
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

                var loanReviewList = await GetLoansReviewAwaitingApprovalAsync(targetIds, tokens);
                var datas = (from a in loanReviewList
                             join b in _context.credit_loan on a.LoanId equals b.LoanId
                             join c in _context.credit_loanapplication on b.LoanApplicationId equals c.LoanApplicationId
                             join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                             join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                             orderby a.CreatedOn descending
                             select new LoanReviewListObj
                             {
                                 LoanReviewApplicationId = a.LoanReviewApplicationId,
                                 LoanId = a.LoanId,
                                 CustomerId = a.CustomerId,
                                 CustomerName = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).FirstName + " " + _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).LastName,
                                 ProductId = a.ProductId,
                                 ProductName = _context.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                                 LoanApplicationId = b.LoanApplicationId,
                                 LoanRefNumber = b.LoanRefNumber,
                                 PrincipalAmount = b.PrincipalAmount,
                                 //OutstandingBalance = (b.OutstandingPrincipal + _context.credit_loanscheduledaily.FirstOrDefault(x => x.LoanId == b.LoanId && x.Deleted == false && x.Date == now).AccruedInterest) ?? 0,
                                 Prepayment = a.Prepayment,
                                 OperationId = a.OperationId,
                                 PropposedAmount = a.ProposedAmount,
                                 PropposedInterestRate = a.ProposedRate,
                                 PropposedTenor = a.ProposedTenor,
                                 BookingDate = b.BookingDate,
                                 CreditScore = c.Score,
                                 ProbabilityOfDefault = c.PD,
                                 FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                 FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                 InterestFrequency = a.InterestFrequencyTypeId,
                                 PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                 PrincipalFrequencyName = e.Mode,
                                 InterestFrequencyName = f.Mode,
                                 WorkflowToken = a.WorkflowToken,
                                 OperationType = a.Prepayment == 0 ? "Loan Review Application" : "Prepayment"
                             }).ToList();

                return new LoanReviewListObjRespObj
                {
                    LoanReviewList = datas,

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

        public IEnumerable<LoanReviewApplicationObj> GetLoanReviewApplicationList()
        {
            try
            {
                var loanApplication = (from a in _context.credit_loanreviewapplication
                                       join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                                       join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                                       where a.Deleted == false
                                       && a.OperationPerformed == false
                                       orderby a.CreatedOn descending
                                       select new LoanReviewApplicationObj
                                       {
                                           LoanReviewApplicationId = a.LoanReviewApplicationId,
                                           LoanId = a.LoanId,
                                           ProductId = a.ProductId,
                                           OperationId = a.OperationId,
                                           CustomerId = a.CustomerId,
                                           ReviewDetails = a.ReviewDetails,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           GenerateOfferLetter = a.GenerateOfferLetter,
                                           OperationPerformed = a.OperationPerformed,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ProposedAmount = a.ProposedAmount,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName
                                           + " " + _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().LastName,
                                           ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           InterestFrequency = a.InterestFrequencyTypeId,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           PrincipalFrequencyName = e.Mode,
                                           InterestFrequencyName = f.Mode,
                                       }).ToList();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanRecommendationLogObj> GetLoanReviewApplicationLog(int loanReviewApplicationId)
        {
            var log = (from a in _context.credit_loanreviewapplicationlog
                       join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                       join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                       where a.LoanReviewApplicationId == loanReviewApplicationId
                       orderby a.CreatedOn descending
                       select new LoanRecommendationLogObj
                       {
                           LoanApplicationId = a.LoanReviewApplicationId,
                           Amount = a.ApprovedAmount,
                           Rate = a.ApprovedRate,
                           Tenor = a.ApprovedTenor,
                           InterestFrequency = a.InterestFrequencyTypeId,
                           PrincipalFrequency = a.PrincipalFrequencyTypeId,
                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                           PrincipalFrequencyName = e.Mode,
                           InterestFrequencyName = f.Mode,
                           CreatedBy = a.CreatedBy
                           //CreatedBy = (from g in _context.cor_useraccount
                           //             join b in _context.cor_staff on g.StaffId equals b.StaffId
                           //             where g.UserName.ToLower() == a.CreatedBy
                           //             select b.FirstName + " " + b.LastName).FirstOrDefault(),
                       }).ToList();
            return log;
        }

        public LoanReviewApplicationObj GetSingleLoanReviewApplication(int loanReviewApplicationId)
        {
            try
            {
                var loanApplication = (from a in _context.credit_loanreviewapplication
                                       join e in _context.credit_frequencytype on a.PrincipalFrequencyTypeId equals e.FrequencyTypeId
                                       join f in _context.credit_frequencytype on a.InterestFrequencyTypeId equals f.FrequencyTypeId
                                       where a.Deleted == false && a.LoanReviewApplicationId == loanReviewApplicationId
                                       orderby a.CreatedOn descending
                                       select new LoanReviewApplicationObj
                                       {
                                           LoanReviewApplicationId = a.LoanReviewApplicationId,
                                           LoanId = a.LoanId,
                                           ProductId = a.ProductId,
                                           OperationId = a.OperationId,
                                           CustomerId = a.CustomerId,
                                           ReviewDetails = a.ReviewDetails,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           GenerateOfferLetter = a.GenerateOfferLetter,
                                           OperationPerformed = a.OperationPerformed,
                                           ApprovedAmount = a.ApprovedAmount,
                                           ApprovedRate = a.ApprovedRate,
                                           ApprovedTenor = a.ApprovedTenor,
                                           ProposedAmount = a.ProposedAmount,
                                           Prepayment = a.Prepayment,
                                           ProposedRate = a.ProposedRate,
                                           ProposedTenor = a.ProposedTenor,
                                           CustomerName = _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().FirstName
                                           + " " + _context.credit_loancustomer.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().LastName,
                                           ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           InterestFrequency = a.InterestFrequencyTypeId,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           PrincipalFrequency = a.PrincipalFrequencyTypeId,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           PrincipalFrequencyName = e.Mode,
                                           InterestFrequencyName = f.Mode,
                                       }).FirstOrDefault();

                return loanApplication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GoForApproval(LoanObj entity)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    {

                        if (entity.ApprovalStatusId == (short)ApprovalStatus.Disapproved)
                        {
                            var application = _context.credit_loanreviewapplication.Find(entity.TargetId);
                            application.ApprovalStatusId = (short)ApprovalStatus.Disapproved;
                            _context.SaveChanges();
                            trans.Commit();
                            return 2;
                        }

                        //if (workflow.NewState == (int)ApprovalState.Ended)
                        //{
                        //    var res = LoanReviewApplicationApproval(entity, (short)entity.ApprovalStatusId);
                        //    var response = DisburseAppraisedLoan(entity.LoanId, User.StaffId, entity.CreatedBy, entity.Comment);

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

                        return 0;
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public ApprovalRecommendationObj UpdateLoanReviewApplicationLog(ApprovalRecommendationObj entity, string user)
        {
            try
            {
                if (entity == null) return null;

                if (entity.LoanApplicationId > 0)
                {
                    var loanReviewExist = _context.credit_loanreviewapplication.Find(entity.LoanApplicationId);
                    if (loanReviewExist != null)
                    {

                        loanReviewExist.ApprovedAmount = entity.ApprovedAmount;
                        loanReviewExist.ApprovedRate = entity.ApprovedRate;
                        loanReviewExist.ApprovedTenor = entity.ApprovedTenor;
                        loanReviewExist.InterestFrequencyTypeId = entity.InterestFrequency;
                        loanReviewExist.PrincipalFrequencyTypeId = entity.PrincipalFrequency;
                        loanReviewExist.FirstInterestPaymentDate = entity.FirstInterestPaymentDate;
                        loanReviewExist.FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate;
                        loanReviewExist.UpdatedBy = user;
                        loanReviewExist.UpdatedOn = DateTime.Now;

                        var log = new credit_loanreviewapplicationlog
                        {
                            LoanReviewApplicationId = entity.LoanApplicationId,
                            LoanId = loanReviewExist.LoanId,
                            ApprovedAmount = entity.ApprovedAmount,
                            ApprovedRate = entity.ApprovedRate,
                            ApprovedTenor = entity.ApprovedTenor,
                            InterestFrequencyTypeId = entity.InterestFrequency,
                            PrincipalFrequencyTypeId = entity.PrincipalFrequency,
                            FirstInterestPaymentDate = entity.FirstInterestPaymentDate,
                            FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate,
                            CreatedBy = user,
                            CreatedOn = DateTime.Now
                        };
                        _context.credit_loanreviewapplicationlog.Add(log);
                    }
                }
                var response = _context.SaveChanges() > 0;
                if (response)
                {
                    return new ApprovalRecommendationObj
                    {
                        LoanApplicationId = entity.LoanApplicationId,
                        ApprovedAmount = entity.ApprovedAmount,
                        ApprovedProductId = entity.ApprovedProductId,
                        ApprovedRate = entity.ApprovedRate,
                        ApprovedTenor = entity.ApprovedTenor,
                        InterestFrequency = entity.InterestFrequency,
                        PrincipalFrequency = entity.PrincipalFrequency,
                        FirstInterestPaymentDate = entity.FirstInterestPaymentDate,
                        FirstPrincipalPaymentDate = entity.FirstPrincipalPaymentDate,

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

        public async Task<bool> UploadLoanReviewOfferLetter(LoanApplicationObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.OfferLetterId > 0)
                {
                    var uploadOfferLetterExist = _context.credit_loanreviewofferletter.Find(entity.OfferLetterId);
                    if (uploadOfferLetterExist != null)
                    {
                        uploadOfferLetterExist.ReportStatus = entity.ReportStatus;
                        uploadOfferLetterExist.SupportDocument = entity.SupportDocument;
                        uploadOfferLetterExist.Active = true;
                        uploadOfferLetterExist.Deleted = false;
                        uploadOfferLetterExist.CreatedBy = entity.CreatedBy;
                        uploadOfferLetterExist.CreatedOn = DateTime.Now;
                        uploadOfferLetterExist.UpdatedBy = entity.CreatedBy;
                        uploadOfferLetterExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var uploadOfferLetter = new credit_loanreviewofferletter
                    {
                        LoanReviewApplicationId = entity.LoanApplicationId,
                        ReportStatus = entity.ReportStatus,
                        SupportDocument = entity.SupportDocument,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = entity.CreatedBy,
                        UpdatedOn = DateTime.Now,
                    };
                    _context.credit_loanreviewofferletter.Add(uploadOfferLetter);
                }

                var response = await _context.SaveChangesAsync() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        ///////PRIVATE METHODS
        ///
        public ApprovalRegRespObj LoanReviewApplicationApproval(ApprovalObj entity, int staffId, string username)
        {
            var loanApplication = _context.credit_loanreviewapplication.Find(entity.TargetId);

            ApprovalRegRespObj response = new ApprovalRegRespObj();
            loanApplication.ApprovalStatusId = entity.ApprovalStatusId;
            loanApplication.GenerateOfferLetter = true;
            response.response = _context.SaveChanges() > 0;
            var ttt = DisburseAppraisedLoan(loanApplication.LoanId, staffId, username, entity.Comment);
            if (ttt.loanPayment != null && ttt.AnyIdentifier > 0)
            {
                return ttt;
            }
            return response;
        }


        private void DeleteExistingSchedule(int loanId)
        {
            var schedule = _context.credit_loanscheduleperiodic.Where(x => x.LoanId == loanId).ToList();
            var dailyschedule = _context.credit_loanscheduledaily.Where(x => x.LoanId == loanId).ToList();
            foreach (var curr in schedule)
            {
                curr.Deleted = true;
            }
            foreach (var curr in dailyschedule)
            {
                curr.Deleted = true;
            }
            _context.SaveChanges();
        }

        private ApprovalRegRespObj DisburseAppraisedLoan(int loanId, int staffId, string createdBy, string comment)
        {
            ApprovalRegRespObj response = new ApprovalRegRespObj();
            var loan = _context.credit_loan_temp.Where(x => x.LoanId == loanId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            var loanApplication = _context.credit_loanapplication.Find(loan.LoanApplicationId);
            DeleteExistingSchedule(loanId);

            LoanPaymentScheduleInputObj input = new LoanPaymentScheduleInputObj
            {
                ScheduleMethodId = (short)loan.ScheduleTypeId,
                PrincipalAmount = (double)loan.PrincipalAmount,
                EffectiveDate = loan.EffectiveDate,
                InterestRate = loan.InterestRate,
                PrincipalFrequency = loan.PrincipalFrequencyTypeId,
                InterestFrequency = loan.InterestFrequencyTypeId,
                PrincipalFirstpaymentDate = (DateTime)loan.FirstPrincipalPaymentDate,
                InterestFirstpaymentDate = (DateTime)loan.FirstInterestPaymentDate,
                MaturityDate = loan.MaturityDate,
                AccurialBasis = (int)loan.AccrualBasis,
                IntegralFeeAmount = 0,
                FirstDayType = (int)loan.FirstDayType,
                CreatedBy = createdBy,
                IrregularPaymentSchedule = null,
                OldBalance = (double)loan.OldBalance,
                OldInterest = (double)loan.OldInterest
            };

            response.AnyIdentifier = loanId;
            response.loanPayment = input;
            //var ttt = _schedule.AddReviewedLoanSchedule(loanId, input);

            loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
            loan.ApprovedBy = staffId;
            loan.ApprovedComments = comment;
            loan.ApprovedDate = DateTime.Now;

            loan.DisbursedBy = staffId;
            loan.DisbursedComments = comment;
            loan.DisbursedDate = DateTime.Now;
            loan.IsDisbursed = true;
            loan.LoanStatusId = (int)LoanStatusEnum.Active;
            loanApplication.LoanApplicationStatusId = (int)ApplicationStatus.Disbursed;
            loan.OutstandingInterest = 0;
            _context.SaveChanges();

            return response;
        }


        ////////////PRIVATE METHODS
        ///
        private async Task<IEnumerable<credit_loanreviewapplication>> GetLoansReviewAwaitingApprovalAsync(List<int> LoanReviewApplicationIds, List<string> tokens)
        {
            var item = await _context.credit_loanreviewapplication
                .Where(s => LoanReviewApplicationIds.Contains(s.LoanReviewApplicationId) && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }
    }
}
