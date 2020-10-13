using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.Response.Finance;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Helpers;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.DailyAccrualObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanOperationsRepository : ILoanOperationsRepository
    {
        private readonly DataContext _context;
        //private IFinanceTransactionRepository financeTransaction;
        private IIdentityServerRequest _serverRequest;
        private ILoanScheduleRepository _schedule;
        private readonly IIdentityService _identityService;
        private readonly ICreditAppraisalRepository _customerTransaction;

        public LoanOperationsRepository(DataContext context, ILoanScheduleRepository schedule, IIdentityService identityService, IIdentityServerRequest serverRequest, ICreditAppraisalRepository customerTransaction)
        {
            _schedule = schedule;
            _context = context;
            _identityService = identityService;
            _serverRequest = serverRequest;
            _customerTransaction = customerTransaction;
        }

        public ApprovalRegRespObj AddOperationReview(LoanReviewOperationObj model)
        {
            ApprovalRegRespObj response = new ApprovalRegRespObj();
            var user = _serverRequest.UserDataAsync();            
            model.StaffId = user.Result.StaffId;

            List<credit_loanreviewoperationirregularinput> irregularSchedules = new List<credit_loanreviewoperationirregularinput>();
            if (model.ReviewIrregularSchedule.Count > 0)
            {
                foreach (var item in model.ReviewIrregularSchedule)
                {
                    var irregularPlan = new credit_loanreviewoperationirregularinput
                    {
                        PaymentAmount = item.PaymentAmount,
                        PaymentDate = item.PaymentDate,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = DateTime.Now
                    };
                    irregularSchedules.Add(irregularPlan);
                }
            }


            var data = new credit_loan_review_operation
            {
                LoanId = model.LoanId,
                ProductTypeId = model.ProductTypeId,
                OperationTypeId = model.OperationTypeId,
                EffectiveDate = model.ProposedEffectiveDate,
                ReviewDatials = model.ReviewDetails,
                InterestRate = model.InterateRate == null ? 0 : (double)model.InterateRate,
                Prepayment = model.Prepayment,
                PrincipalFrequencyTypeId = model.PrincipalFrequencyTypeId,
                InterestFrequencyTypeId = model.InterestFrequencyTypeId,
                PrincipalFirstPaymentDate = model.PrincipalFirstPaymentDate,
                InterestFirstPaymentDate = model.InterestFirstPaymentDate,
                MaturityDate = model.MaturityDate,
                Tenor = (int)(model.MaturityDate - model.ProposedEffectiveDate).Value.TotalDays,
                CasaAccountId = model.CASA_AccountId,
                FeeCharges = model.Fee_Charges,
                ApprovalStatusId = (int)ApprovalStatus.Pending,
                ISManagementRate = model.IsManagementRate,
                OperationCompleted = false,
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.Now,
                //credit_loanreviewoperationirregularinput = irregularSchedules
            };


            var operationPerformed = _context.credit_loanreviewapplication.Where(x => x.LoanReviewApplicationId == model.LmsApplicationDetailId).FirstOrDefault();
            if (operationPerformed != null)
            {
                operationPerformed.OperationPerformed = true;
            }

                try
                {
                    _context.credit_loan_review_operation.Add(data);
                    var ttt = DisburseAppraisedLoan(model.LoanId, model.StaffId, model.CreatedBy, model);
                    _context.SaveChanges();
                    if (ttt.loanPayment != null && ttt.AnyIdentifier > 0)
                    {
                        return ttt;
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }

        public bool DateChange(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId)
        {
            throw new NotImplementedException();
        }

        public bool FrequencyChange(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LoanReviewOperationApprovalObj> GetApprovedLoanOperationReview()
        {
            var dataLoan = (from ln in _context.credit_loan
                            join op in _context.credit_loan_review_operation on ln.LoanId equals op.LoanId
                            join c in _context.credit_loancustomer on ln.CustomerId equals c.CustomerId
                            where op.ApprovalStatusId == (int)ApprovalStatus.Approved && op.OperationCompleted == false
                            orderby op.OperationTypeId descending
                            select new LoanReviewOperationApprovalObj
                            {
                                LoanId = ln.LoanId,
                                CustomerId = ln.CustomerId,
                                ProductId = ln.ProductId,
                                CasaAccountId = (int)ln.CasaAccountId,
                                BranchId = (int)ln.BranchId,
                                LoanReferenceNumber = ln.LoanRefNumber,
                                PrincipalFrequencyTypeId = ln.PrincipalFrequencyTypeId != null ? (int)ln.PrincipalFrequencyTypeId : 0,
                                InterestFrequencyTypeId = ln.InterestFrequencyTypeId != null ? (int)ln.InterestFrequencyTypeId : 0,
                                EffectiveDate = ln.EffectiveDate,
                                MaturityDate = ln.MaturityDate,
                                BookingDate = ln.BookingDate,
                                PrincipalAmount = (decimal)ln.OutstandingPrincipal,
                                ApprovalStatusId = op.ApprovalStatusId,
                                //ApprovalStatusName = _context.cor_approvalstatus.FirstOrDefault(f => f.ApprovalStatusId == op.ApprovalStatusId).ApprovalStatusName,
                                ApprovedBy = ln.ApprovedBy,
                                ApproverComment = ln.ApprovedComments,
                                DateApproved = ln.ApprovedDate,
                                ScheduleTypeId = (int)ln.ScheduleTypeId,
                                IsDisbursed = ln.IsDisbursed,
                                DisbursedBy = (int)ln.DisbursedBy,
                                DisburserComment = ln.DisbursedComments,
                                DisburseDate = ln.DisbursedDate,
                                OperationId = ln.LoanOperationId,
                                EquityContribution = (decimal)ln.EquityContribution,
                                FirstPrincipalPaymentDate = ln.FirstInterestPaymentDate,
                                FirstInterestPaymentDate = ln.FirstPrincipalPaymentDate,
                                OutstandingPrincipal = (decimal)ln.OutstandingPrincipal,
                                ProductAccountNumber = _context.credit_casa.Where(x => x.CasaAccountId == ln.CasaAccountId).FirstOrDefault().AccountNumber,
                                ProductAccountName = _context.credit_product.Where(x => x.ProductId == ln.ProductId).FirstOrDefault().ProductName,
                                CustomerName = c.CustomerTypeId != 2 ? c.FirstName + " " + c.LastName + " " + c.MiddleName : c.FirstName,
                                CurrencyId = ln.CurrencyId,
                                InterestRate = ln.InterestRate,
                                //BranchName = _context.cor_branch.Where(x => x.BranchId == ln.BranchId).FirstOrDefault().BranchName,
                                //Comment = "",
                                LoanReviewOperationsId = op.LoanReviewOperationId,
                                OperationTypeId = op.OperationTypeId,
                                //OperationTypeName = _context.cor_operation.FirstOrDefault(d => d.OperationTypeId == op.OperationTypeId).OperationName,
                                NewEffectiveDate = op.EffectiveDate,
                                ReviewDetails = op.ReviewDatials,
                                Prepayment = op.Prepayment,
                                NewPrincipalFrequencyTypeId = op.PrincipalFrequencyTypeId,
                                NewInterestFrequencyTypeId = op.InterestFrequencyTypeId,
                                NewPrincipalFirstPaymentDate = op.PrincipalFirstPaymentDate,
                                NewInterestFirstPaymentDate = op.InterestFirstPaymentDate,
                                NewTenor = op.Tenor,
                                CASA_AccountId = op.CasaAccountId,
                                Fee_Charges = op.FeeCharges,
                            }).ToList();
            var data = dataLoan;
            return data;
        }

        public IEnumerable<LoanObj> GetApprovedLoanReview()
        {
            try
            {
                //var applicationDate = _context.cor_applicationdate.FirstOrDefault().CurrentDate;
                var applicationDate = DateTime.Now;
                var now = DateTime.Now.Date;

                var allFilteredLoan = (from a in _context.credit_loan
                                       join b in _context.credit_loanreviewapplication on a.LoanId equals b.LoanId
                                       join c in _context.credit_loancustomer on a.CustomerId equals c.CustomerId
                                       //join e in _context.cor_operation on b.OperationId equals e.OperationId
                                       where a.IsDisbursed == true && 
                                       b.ApprovalStatusId == (int)ApprovalStatus.Approved
                                      //&& e.OperationTypeId == (int)OperationTypeEnum.LoanManagement
                                      && b.OperationPerformed == false

                                       select new LoanObj
                                       {
                                           LoanId = a.LoanId,
                                           CustomerId = a.CustomerId,
                                           CustomerName = c.CustomerTypeId != 2 ? c.FirstName + " " + c.LastName + " " + c.MiddleName : c.FirstName,
                                           ProductId = a.ProductId,
                                           CompanyId = (int)a.CompanyId,
                                           CasaAccountId = (int)a.CasaAccountId,
                                           BranchId = (int)a.BranchId,
                                           //BranchName = _context.cor_branch.Where(x => x.BranchId == a.BranchId).FirstOrDefault().BranchName,
                                           LoanReferenceNumber = a.LoanRefNumber,
                                           ApplicationReferenceNumber = _context.credit_loanapplication.Where(x => x.LoanApplicationId == a.LoanApplicationId).FirstOrDefault().ApplicationRefNumber ?? "N/A",
                                           PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId != null ? a.PrincipalFrequencyTypeId : 0,
                                           InterestFrequencyTypeId = a.InterestFrequencyTypeId != null ? a.InterestFrequencyTypeId : 0,
                                           ProductTypeId = (int)_context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductTypeId,
                                           ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           FeeAmount = _context.credit_productfee.Where(x => x.ProductId == a.ProductId).Sum(b => b.ProductAmount),
                                           InterestRate = a.InterestRate,
                                           EffectiveDate = a.EffectiveDate,
                                           MaturityDate = a.MaturityDate,
                                           BookingDate = a.BookingDate,
                                           PrincipalAmount = a.PrincipalAmount,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           ApprovedBy = a.ApprovedBy,
                                           ApproverComment = a.ApprovedComments,
                                           DateApproved = a.ApprovedDate,
                                           LoanStatusId = (int)a.LoanStatusId,
                                           ScheduleTypeId = (int)a.ScheduleTypeId,
                                           IsDisbursed = a.IsDisbursed,
                                           DisbursedBy = a.DisbursedBy,
                                           DisburserComment = a.DisbursedComments,
                                           DisburseDate = a.DisbursedDate,
                                           OperationId = a.LoanOperationId,
                                           OperationName = b.Prepayment == 0 ? "Loan Review Application" : "Prepayment",
                                           CasaAccountNumber = _context.credit_casa.Where(x => x.CasaAccountId == a.CasaAccountId).FirstOrDefault().AccountNumber,
                                           ProductAccountName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           EquityContribution = (decimal)a.EquityContribution,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           OutstandingPrincipal = (decimal)a.OutstandingPrincipal,
                                           OutstandingInterest = (decimal)a.OutstandingInterest,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = b.CreatedOn,
                                           ExchangeRate = a.ExchangeRate,
                                           CurrencyId = a.CurrencyId,
                                           //Currency = _context.cor_currency.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().CurrencyName,
                                           LoanReviewOperationTypeId = b.OperationId,
                                           ReviewDetails = b.ReviewDetails,
                                           InterestOnPastDueInterest = (decimal)a.InterestOnPastDueInterest,
                                           InterestOnPastDuePrincipal = (decimal)a.InterestOnPastDuePrincipal,
                                           PastDueInterest = (decimal)a.PastDueInterest,
                                           PastDuePrincipal = (decimal)a.PastDuePrincipal,
                                           AccrualBasis = a.AccrualBasis,
                                           SystemCurrentDate = applicationDate,
                                           LmsApplicationDetailId = b.LoanReviewApplicationId,
                                       }).ToList();

                return allFilteredLoan.OrderByDescending(x => x.CreatedOn);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<LoanObj> GetApprovedLoanReviewRemedial()
        {
            try
            {
                //var applicationDate = _context.cor_applicationdate.FirstOrDefault().CurrentDate;
                var applicationDate = DateTime.Now;
                var allFilteredLoan = (from a in _context.credit_loan
                                       join b in _context.credit_loanreviewapplication on a.LoanId equals b.LoanId
                                       join c in _context.credit_loancustomer on a.CustomerId equals c.CustomerId
                                       join d in _context.credit_loanscheduledaily on a.LoanId equals d.LoanId
                                       //join e in _context.cor_operation on b.OperationId equals e.OperationId
                                       where a.IsDisbursed == true && b.ApprovalStatusId == (int)ApprovalStatus.Approved
                                      //&& e.OperationTypeId == (int)OperationTypeEnum.Remedial
                                      && b.OperationPerformed == false
                                      && d.Date == applicationDate.Date
                                       select new LoanObj
                                       {
                                           LoanId = a.LoanId,
                                           CustomerId = a.CustomerId,
                                           CustomerName = c.CustomerTypeId != 2 ? c.FirstName + " " + c.LastName + " " + c.MiddleName : c.FirstName,
                                           ProductId = a.ProductId,
                                           CompanyId = (int)a.CompanyId,
                                           CasaAccountId = (int)a.CasaAccountId,
                                           BranchId = (int)a.BranchId,
                                           //BranchName = _context.cor_branch.Where(x => x.BranchId == a.BranchId).FirstOrDefault().BranchName,
                                           LoanReferenceNumber = a.LoanRefNumber,
                                           ApplicationReferenceNumber = _context.credit_loanapplication.Where(x => x.LoanApplicationId == a.LoanApplicationId).FirstOrDefault().ApplicationRefNumber ?? "N/A",
                                           PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId != null ? a.PrincipalFrequencyTypeId : 0,
                                           InterestFrequencyTypeId = a.InterestFrequencyTypeId != null ? a.InterestFrequencyTypeId : 0,
                                           ProductTypeId = (int)_context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductTypeId,
                                           ProductName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           InterestRate = a.InterestRate,
                                           EffectiveDate = a.EffectiveDate,
                                           MaturityDate = a.MaturityDate,
                                           BookingDate = a.BookingDate,
                                           PrincipalAmount = a.PrincipalAmount,
                                           ApprovalStatusId = a.ApprovalStatusId,
                                           ApprovedBy = a.ApprovedBy,
                                           ApproverComment = a.ApprovedComments,
                                           DateApproved = a.ApprovedDate,
                                           LoanStatusId = (int)a.LoanStatusId,
                                           ScheduleTypeId = (int)a.ScheduleTypeId,
                                           IsDisbursed = a.IsDisbursed,
                                           DisbursedBy = a.DisbursedBy,
                                           DisburserComment = a.DisbursedComments,
                                           DisburseDate = a.DisbursedDate,
                                           OperationId = a.LoanOperationId,
                                           //OperationName = e.OperationName,
                                           CasaAccountNumber = _context.credit_casa.Where(x => x.CasaAccountId == a.CasaAccountId).FirstOrDefault().AccountNumber,
                                           ProductAccountName = _context.credit_product.Where(x => x.ProductId == a.ProductId).FirstOrDefault().ProductName,
                                           EquityContribution = (decimal)a.EquityContribution,
                                           FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                                           FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                                           OutstandingPrincipal = (decimal)a.OutstandingPrincipal,
                                           OutstandingInterest = (decimal)a.OutstandingInterest,
                                           CreatedBy = a.CreatedBy,
                                           ExchangeRate = a.ExchangeRate,
                                           CurrencyId = a.CurrencyId,
                                           //Currency = _context.cor_currency.Where(x => x.CurrencyId == a.CurrencyId).FirstOrDefault().CurrencyName,
                                           LoanReviewOperationTypeId = b.OperationId,
                                           ReviewDetails = b.ReviewDetails,
                                           InterestOnPastDueInterest = (decimal)a.InterestOnPastDueInterest,
                                           InterestOnPastDuePrincipal = (decimal)a.InterestOnPastDuePrincipal,
                                           PastDueInterest = (decimal)a.PastDueInterest,
                                           PastDuePrincipal = (decimal)a.PastDuePrincipal,
                                           AccrualedAmount = d.AccruedInterest,
                                           SystemCurrentDate = applicationDate,
                                           LmsApplicationDetailId = b.LoanReviewApplicationId,
                                       }).ToList();

                return allFilteredLoan;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<LoanPaymentSchedulePeriodicObj> GetLoanScheduleByLoanId(int loanId)
        {
            var loanSchedule = (from sch in _context.credit_loanscheduleperiodic
                                where sch.LoanId == loanId
                                select new LoanPaymentSchedulePeriodicObj
                                {
                                    LoanId = sch.LoanId,
                                    PaymentNumber = sch.PaymentNumber,
                                    PaymentDate = sch.PaymentDate,
                                    StartPrincipalAmount = (double)sch.StartPrincipalAmount,
                                    PeriodPaymentAmount = (double)sch.PeriodPaymentAmount,
                                    PeriodInterestAmount = (double)sch.PeriodInterestAmount,
                                    PeriodPrincipalAmount = (double)sch.PeriodPrincipalAmount,
                                    EndPrincipalAmount = (double)sch.EndPrincipalAmount,
                                    InterestRate = sch.InterestRate,
                                    AmortisedStartPrincipalAmount = (double)sch.AmortisedStartPrincipalAmount,
                                    AmortisedPeriodPaymentAmount = (double)sch.AmortisedPeriodPaymentAmount,
                                    AmortisedPeriodInterestAmount = (double)sch.AmortisedPeriodInterestAmount,
                                    AmortisedPeriodPrincipalAmount = (double)sch.AmortisedPeriodPrincipalAmount,
                                    AmortisedEndPrincipalAmount = (double)sch.AmortisedEndPrincipalAmount,
                                    EffectiveInterestRate = sch.EffectiveInterestRate
                                }).ToList();
            return loanSchedule;
        }

        public async Task<IEnumerable<LoanOperationTypeObj>> GetOperationType()
        {
            var operation = await _serverRequest.GetAllOperationAsync();
            return (from data in operation.Operations
                    where data.OperationTypeId == (int)OperationTypeEnum.LoanManagement && data.OperationTypeId != 27
                    select new LoanOperationTypeObj()
                    {
                        OperationTypeId = data.OperationId,
                        OperationTypeName = data.OperationName
                    });
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LoanOperationTypeObj>> GetOperationTypeRestructure()
        {
            var operation = await _serverRequest.GetAllOperationAsync();
            return (from data in operation.Operations
                    where data.OperationTypeId == (int)OperationTypeEnum.LoanManagement && data.OperationId == 27
                    select new LoanOperationTypeObj()
                    {
                        OperationTypeId = data.OperationId,
                        OperationTypeName = data.OperationName
                    });
        }

        public IEnumerable<LoanOperationTypeObj> GetRemedialOperationType()
        {
            //return (from data in _context.cor_operation
            //        where data.OperationTypeId == (int)OperationTypeEnum.Remedial
            //        select new LoanOperationTypeObj()
            //        {
            //            OperationTypeId = data.OperationId,
            //            OperationTypeName = data.OperationName
            //        });
            throw new NotImplementedException();
        }

        public LoanObj GetRunningLoans(int companyId, string refNo)
        {
            //var applicationDate = _context.cor_applicationdate.FirstOrDefault().CurrentDate;
            var applicationDate = DateTime.Now;
            var data = _context.credit_loan.FirstOrDefault(x => x.LoanRefNumber == refNo);
            DateTime maturityDate = data.MaturityDate;
            DateTime effectiveDate = data.EffectiveDate;
            decimal outStandingBalance = (decimal)data.OutstandingPrincipal;
            TimeSpan difference = maturityDate - applicationDate;
            int days = (int)difference.TotalDays;
            decimal accruedInterest = 0;
            var dailyAccruedInterest = _context.credit_loanscheduledaily.Where(x => x.LoanId == data.LoanId && x.Date == applicationDate).FirstOrDefault();
            if (dailyAccruedInterest != null)
            {
                accruedInterest = dailyAccruedInterest.AccruedInterest;
            }
            accruedInterest = decimal.Round(accruedInterest, 2, MidpointRounding.AwayFromZero);
            DateTime nextPaymentDate = _context.credit_loanscheduleperiodic.FirstOrDefault(x => x.LoanId == data.LoanId && x.PaymentDate > applicationDate).PaymentDate;
            outStandingBalance = decimal.Round(outStandingBalance, 2, MidpointRounding.AwayFromZero);

            decimal pastDue = decimal.Round(((decimal)data.PastDueInterest), 2, MidpointRounding.AwayFromZero);
            decimal totalamount = (accruedInterest + outStandingBalance + pastDue);

            var cust = _context.credit_loancustomer.Where(x => x.CustomerId == data.CustomerId).FirstOrDefault();
            var runningLoan = (from l in _context.credit_loan
                               where l.LoanRefNumber == refNo
                               select new LoanObj()
                               {
                                   LoanId = l.LoanId,
                                   //CompanyName = _context.cor_company.Where(x => x.CompanyId == l.CompanyId).FirstOrDefault().Name,
                                   CompanyId = (int)l.CompanyId,
                                   CustomerName = cust.FirstName + " " + cust.MiddleName + " " + cust.LastName,
                                   CustomerId = l.CustomerId,
                                   ApprovedAmount = l.PrincipalAmount,
                                   ProductTypeId = _context.credit_product.Where(x => x.ProductId == l.ProductId).FirstOrDefault().ProductTypeId,
                                   BranchId = (int)l.BranchId,
                                   //BranchName = _context.cor_branch.Where(x => x.CompanyId == l.CompanyId).FirstOrDefault().BranchName,
                                   InterestRate = l.InterestRate,
                                   OutstandingInterest = (decimal)l.OutstandingInterest,
                                   OutstandingPrincipal = (decimal)l.OutstandingPrincipal,
                                   PrincipalAmount = l.PrincipalAmount,
                                   //Currency = _context.cor_currency.Where(x => x.CurrencyId == l.CurrencyId).FirstOrDefault().CurrencyName,
                                   LoanReferenceNumber = l.LoanRefNumber,
                                   EffectiveDate = applicationDate,
                                   PreviousEffectiveDate = l.EffectiveDate,
                                   EquityContribution = 0,
                                   MaintainTenor = true,
                                   MaturityDate = l.MaturityDate,
                                   ScheduleTypeId = (int)l.ScheduleTypeId,
                                   ScheduleTypeCategoryId = _context.credit_loanscheduletype.Where(x => x.LoanScheduleTypeId == l.ScheduleTypeId).FirstOrDefault().LoanScheduleCategoryId,
                                   Teno = days,
                                   Newtenor = 0,
                                   AccrualedAmount = accruedInterest,
                                   TotalAmount = totalamount,
                                   FirstPrincipalPaymentDate = nextPaymentDate,
                                   FirstInterestPaymentDate = nextPaymentDate,
                                   PrincipalFrequencyTypeId = l.PrincipalFrequencyTypeId,
                                   InterestFrequencyTypeId = l.InterestFrequencyTypeId,
                                   PastDueTotal = pastDue,
                                   SystemCurrentDate = applicationDate
                               }).FirstOrDefault();

            return runningLoan;
        }

        public int GoForApproval(ApprovalObj entity)
        {
            //using (var trans = _context.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        var user = _context.cor_useraccount.Where(x => x.UserName.ToLower().Trim() == entity.createdBy.ToLower().Trim()).FirstOrDefault();

            //        workflow.StaffId = user.StaffId;
            //        workflow.CompanyId = 1;
            //        workflow.StatusId = (int)ApprovalStatus.Processing;
            //        workflow.TargetId = entity.targetId.ToString();
            //        workflow.Comment = entity.comment;
            //        workflow.OperationId = entity.operationId;
            //        //workFlow.DeferredExecution = false;

            //        workflow.LogActivity();

            //        var reviewRecord = (from s in _context.credit_loan_review_operation
            //                            where s.LoanReviewOperationId == Convert.ToInt32(entity.targetId) && s.OperationTypeId == entity.operationId
            //                           && s.ApprovalStatusId != (int)ApprovalStatus.Approved
            //                            && s.OperationCompleted == false
            //                            select s).FirstOrDefault();

            //        if (entity.approvalStatusId == (short)ApprovalStatus.Disapproved)
            //        {
            //            reviewRecord.ApprovalStatusId = (short)ApprovalStatus.Disapproved;
            //            _context.SaveChanges();
            //            trans.Commit();
            //            return 2;
            //        }
            //        bool output = false;
            //        if (workflow.NewState == (int)ApprovalState.Ended)
            //        {
            //            var result = LoanRephasementProcess((short)reviewRecord.LoanReviewOperationId, reviewRecord.LoanId, entity.staffId);

            //            if (result == true)
            //            {
            //                reviewRecord.ApprovalStatusId = (int)ApprovalStatus.Approved;
            //                output = _context.SaveChanges() > 0;
            //            }

            //            if (output)
            //            {
            //                trans.Commit();
            //            }
            //            return 1;
            //        }
            //        else
            //        {
            //            var records = _context.credit_loan_review_operation.Find(entity.targetId);
            //            if (records != null)
            //            {
            //                records.ApprovalStatusId = (int)ApprovalStatus.Processing;
            //            }
            //            output = _context.SaveChanges() > 0;
            //            //if (output)
            //            //{
            //            trans.Commit();
            //            //}
            //        }
            //        return 0;
            //    }

            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        throw new Exception(ex.Message);
            //    }
            //}
            throw new NotImplementedException();
        }

        public bool InterestRateReview(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId)
        {
            bool output = false;
            //var systemDate = _context.cor_applicationdate.FirstOrDefault().CurrentDate;
            var systemDate = DateTime.Now;

            if (LoanExist(loanId) > 0)
            {
                DeleteLoanExist(loanId);
                ArchiveLoan(loanId, loanInput.OperationId);/////loanId change this to OperationId
                ArchivePeriodicSchedule(loanId);
                ArchiveDailySchedule(loanId);


                //----------generate and save periodic loan schedule -----------------------------------
                List<LoanPaymentSchedulePeriodicObj> periodicScheduleTemp = _schedule.GeneratePeriodicLoanSchedule(loanInput);

                List<credit_temp_loanscheduleperiodic> tblPeriodicScheduleTemp = new List<credit_temp_loanscheduleperiodic>();
                foreach (var item in periodicScheduleTemp)
                {
                    credit_temp_loanscheduleperiodic scheduleTemp = new credit_temp_loanscheduleperiodic();

                    scheduleTemp.LoanId = loanId;
                    scheduleTemp.PaymentNumber = item.PaymentNumber;
                    scheduleTemp.PaymentDate = item.PaymentDate;
                    scheduleTemp.StartPrincipalAmount = (decimal)item.StartPrincipalAmount;
                    scheduleTemp.PeriodPaymentAmount = (decimal)item.PeriodPaymentAmount;
                    scheduleTemp.PeriodInterestAmount = (decimal)item.PeriodInterestAmount;
                    scheduleTemp.PeriodPrincipalAmount = (decimal)item.PeriodPrincipalAmount;
                    scheduleTemp.EndPrincipalAmount = (decimal)item.EndPrincipalAmount;
                    scheduleTemp.InterestRate = item.InterestRate;
                    scheduleTemp.AmortisedStartPrincipalAmount = (decimal)item.AmortisedStartPrincipalAmount;
                    scheduleTemp.AmortisedPeriodPaymentAmount = (decimal)item.AmortisedPeriodPaymentAmount;
                    scheduleTemp.AmortisedPeriodInterestAmount = (decimal)item.AmortisedPeriodInterestAmount;
                    scheduleTemp.AmortisedPeriodPrincipalAmount = (decimal)item.AmortisedPeriodPrincipalAmount;
                    scheduleTemp.AmortisedEndPrincipalAmount = (decimal)item.AmortisedEndPrincipalAmount;
                    scheduleTemp.EffectiveInterestRate = item.EffectiveInterestRate;
                    scheduleTemp.CreatedBy = item.CreatedBy;
                    scheduleTemp.CreatedOn = systemDate;

                    tblPeriodicScheduleTemp.Add(scheduleTemp);
                }
                //-------------------------------------------------------------------------------------


                //----------generate and save daily loan schedule -----------------------------------
                List<LoanPaymentScheduleDailyObj> dailyScheduleTemp = _schedule.GenerateDailyLoanSchedule(loanInput);

                List<credit_temp_loanscheduledaily> tblDailyScheduleTemp = new List<credit_temp_loanscheduledaily>();

                foreach (var item in dailyScheduleTemp)
                {
                    credit_temp_loanscheduledaily scheduleTemp = new credit_temp_loanscheduledaily();

                    scheduleTemp.LoanId = loanId;
                    scheduleTemp.PaymentNumber = item.PaymentNumber;
                    scheduleTemp.Date = item.Date;
                    scheduleTemp.PaymentDate = item.PaymentDate;
                    scheduleTemp.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                    scheduleTemp.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                    scheduleTemp.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                    scheduleTemp.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                    scheduleTemp.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                    scheduleTemp.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                    scheduleTemp.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                    scheduleTemp.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                    scheduleTemp.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                    scheduleTemp.InterestRate = item.NorminalInterestRate;
                    scheduleTemp.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                    scheduleTemp.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                    scheduleTemp.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                    scheduleTemp.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                    scheduleTemp.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                    scheduleTemp.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                    scheduleTemp.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                    scheduleTemp.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                    scheduleTemp.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                    scheduleTemp.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                    scheduleTemp.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                    scheduleTemp.EffectiveInterestRate = item.EffectiveInterestRate;
                    scheduleTemp.CreatedBy = item.CreatedBy;
                    scheduleTemp.CreatedOn = systemDate;

                    tblDailyScheduleTemp.Add(scheduleTemp);
                }
                //----------------------------------------------------------------


                //------------adding records to the database--------------------------

                //if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                //{ this._context.tbl_Loan_Schedule_Irregular_Input.AddRange(tblIrregularSchedule); }////change to Temp table


                this._context.credit_temp_loanscheduleperiodic.AddRange(tblPeriodicScheduleTemp);////change to Temp table

                this._context.credit_temp_loanscheduledaily.AddRange(tblDailyScheduleTemp); ////change to Temp table
                _context.SaveChanges();
                //----------update loan details -----------------------------------
                var loan = this._context.credit_loan.FirstOrDefault(x => x.LoanId == loanId);
                loan.MaturityDate = periodicScheduleTemp.Max(x => x.PaymentDate);
                //-------------------------------------------------

                MergePeriodicSchedule(loanId, applicationDate);

                _context.SaveChanges();
                //-------------------------------------------------------
            }

            output = true;

            return output;
        }

        public bool LoanPrepayment(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId)
        {
            bool output = false;
            double penalAmount = 0;
            try

            {
                //var systemDate = _context.cor_applicationdate.FirstOrDefault().CurrentDate;
                var systemDate = DateTime.Now;
                var reviewData = _context.credit_loan_review_operation.Where(x => x.LoanId == loanId && x.OperationTypeId == loanInput.OperationId && x.OperationCompleted == false).FirstOrDefault();
                var product = _context.credit_product.FirstOrDefault(x => x.ProductId == loanInput.ProductId);
                //var penalCharge = _context.TBL_CHARGE_FEE.FirstOrDefault(x => x.OPERATIONID == (int)OperationsEnum.Prepayment);
                var loan = this._context.credit_loan.Where(x => x.LoanId == loanInput.LoanId).FirstOrDefault();
                decimal principalOutStandingBalance = (decimal)loan.OutstandingPrincipal;
                decimal accruedInterest = 0;
                var accrued = (from a in _context.credit_loanscheduledaily
                               join b in _context.credit_loan on a.LoanId equals b.LoanId
                               where a.LoanId == loanId && a.Date == applicationDate
                               select a).FirstOrDefault();

                if (accrued != null)
                {
                    accruedInterest = accrued.AccruedInterest;
                }
                else
                {
                    throw new Exception("Application Date not found in Payment Schedule");
                }
                accruedInterest = decimal.Round(accruedInterest, 2, MidpointRounding.AwayFromZero);
                principalOutStandingBalance = decimal.Round(principalOutStandingBalance, 2, MidpointRounding.AwayFromZero);
                decimal pastDue = decimal.Round((decimal)(loan.PastDueInterest + loan.InterestOnPastDueInterest + loan.InterestOnPastDuePrincipal), 2, MidpointRounding.AwayFromZero);
                //decimal pastDue = decimal.Round((decimal)(loan.PastDueInterest), 2, MidpointRounding.AwayFromZero);
                decimal totalamount = (principalOutStandingBalance + pastDue + accruedInterest);
                penalAmount = 0;


                decimal partPayment = (decimal)loanInput.PayAmount - (pastDue + accruedInterest);

                int value = product.InterestReceivablePayableGL.Value;

                //finacle.GetGlAccountCode(product.INTERESTRECEIVABLEPAYABLEGL.Value, item.currencyId, item.branchId)

                //List<FinanceTransactionViewModel> inputTransactions = new List<FinanceTransactionViewModel>();
                if (loanInput.PayAmount >= (double)(totalamount + (decimal)penalAmount))
                {

                    //inputTransactions.Add(financeTransaction.PostBuildLoanPrepaymentPosting(loanInput, accruedInterest, product.InterestReceivablePayableGL.Value, "Accrued Interest", (int)OperationsEnum.InterestLoanRepayment));

                    //inputTransactions.Add(financeTransaction.PostBuildLoanPrepaymentPosting(loanInput, principalOutStandingBalance, product.PrincipalGL.Value, "Principal Amount", (int)OperationsEnum.PrincipalLoanRepayment));


                    updateloanTableStatus(loanInput.LoanId);
                }
                else
                {

                    //inputTransactions.Add(financeTransaction.PostBuildLoanPrepaymentPosting(loanInput, accruedInterest, product.InterestReceivablePayableGL.Value, "Accrued Interest", (int)OperationsEnum.InterestLoanRepayment));

                    //inputTransactions.Add(financeTransaction.PostBuildLoanPrepaymentPosting(loanInput, partPayment, product.PrincipalGL.Value, "Principal Amount", (int)OperationsEnum.PrincipalLoanRepayment));


                    if (LoanExist(loanId) > 0)
                    {
                        var archiveBatchCode = GeneralHelpers.GenerateRandomDigitCode(7);
                        DeleteLoanExist(loanId);
                        ArchiveLoan(loanId, loanInput.OperationId);/////loanId change this to OperationId
                        ArchivePeriodicSchedule(loanId);
                        ArchiveDailySchedule(loanId);

                        loanInput.PrincipalAmount = ((double)totalamount - (loanInput.PayAmount + penalAmount));

                        //----------generate and save periodic loan schedule -----------------------------------

                        List<LoanPaymentSchedulePeriodicObj> periodicScheduleTemp = _schedule.GeneratePeriodicLoanSchedule(loanInput);

                        List<credit_temp_loanscheduleperiodic> tblPeriodicScheduleTemp = new List<credit_temp_loanscheduleperiodic>();
                        foreach (var item in periodicScheduleTemp)
                        {
                            credit_temp_loanscheduleperiodic scheduleTemp = new credit_temp_loanscheduleperiodic();

                            scheduleTemp.LoanId = loanId;
                            scheduleTemp.PaymentNumber = item.PaymentNumber;
                            scheduleTemp.PaymentDate = item.PaymentDate;
                            scheduleTemp.StartPrincipalAmount = (decimal)item.StartPrincipalAmount;
                            scheduleTemp.PeriodPaymentAmount = (decimal)item.PeriodPaymentAmount;
                            scheduleTemp.PeriodInterestAmount = (decimal)item.PeriodInterestAmount;
                            scheduleTemp.PeriodPrincipalAmount = (decimal)item.PeriodPrincipalAmount;
                            scheduleTemp.EndPrincipalAmount = (decimal)item.EndPrincipalAmount;
                            scheduleTemp.InterestRate = item.InterestRate;
                            scheduleTemp.AmortisedStartPrincipalAmount = (decimal)item.AmortisedStartPrincipalAmount;
                            scheduleTemp.AmortisedPeriodPaymentAmount = (decimal)item.AmortisedPeriodPaymentAmount;
                            scheduleTemp.AmortisedPeriodInterestAmount = (decimal)item.AmortisedPeriodInterestAmount;
                            scheduleTemp.AmortisedPeriodPrincipalAmount = (decimal)item.AmortisedPeriodPrincipalAmount;
                            scheduleTemp.AmortisedEndPrincipalAmount = (decimal)item.AmortisedEndPrincipalAmount;
                            scheduleTemp.EffectiveInterestRate = item.EffectiveInterestRate;
                            scheduleTemp.CreatedBy = item.CreatedBy;
                            scheduleTemp.CreatedOn = systemDate;

                            tblPeriodicScheduleTemp.Add(scheduleTemp);
                        }
                        //-------------------------------------------------------------------------------------


                        //----------generate and save daily loan schedule -----------------------------------
                        List<LoanPaymentScheduleDailyObj> dailyScheduleTemp = _schedule.GenerateDailyLoanSchedule(loanInput);

                        List<credit_temp_loanscheduledaily> tblDailyScheduleTemp = new List<credit_temp_loanscheduledaily>();

                        foreach (var item in dailyScheduleTemp)
                        {
                            credit_temp_loanscheduledaily scheduleTemp = new credit_temp_loanscheduledaily();

                            scheduleTemp.LoanId = loanId;
                            scheduleTemp.PaymentNumber = item.PaymentNumber;
                            scheduleTemp.Date = item.Date;
                            scheduleTemp.PaymentDate = item.PaymentDate;
                            scheduleTemp.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                            scheduleTemp.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                            scheduleTemp.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                            scheduleTemp.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                            scheduleTemp.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                            scheduleTemp.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                            scheduleTemp.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                            scheduleTemp.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                            scheduleTemp.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                            scheduleTemp.InterestRate = item.NorminalInterestRate;
                            scheduleTemp.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                            scheduleTemp.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                            scheduleTemp.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                            scheduleTemp.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                            scheduleTemp.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                            scheduleTemp.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                            scheduleTemp.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                            scheduleTemp.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                            scheduleTemp.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                            scheduleTemp.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                            scheduleTemp.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                            scheduleTemp.EffectiveInterestRate = item.EffectiveInterestRate;
                            scheduleTemp.CreatedBy = item.CreatedBy;
                            scheduleTemp.CreatedOn = systemDate;

                            tblDailyScheduleTemp.Add(scheduleTemp);
                        }
                        //----------------------------------------------------------------

                        //------------adding records to the database--------------------------
                        this._context.credit_temp_loanscheduleperiodic.AddRange(tblPeriodicScheduleTemp);////change to Temp table

                        this._context.credit_temp_loanscheduledaily.AddRange(tblDailyScheduleTemp); ////change to Temp table
                                                                                                    //context.SaveChanges();

                        MergePeriodicSchedule(loanId, applicationDate);
                        MergeDailySchedule(loanId, applicationDate);

                        var outstInterest = from d in _context.credit_temp_loanscheduleperiodic
                                            where d.LoanId == loanId
                                            let sumPrincipalAmount = _context.credit_temp_loanscheduleperiodic.Where(a => a.LoanId == loanId).Sum(a => a.PeriodInterestAmount)
                                            select sumPrincipalAmount;
                        var outstandingInterest = outstInterest.FirstOrDefault();
                        //----------update loan details -----------------------------------
                        loan.MaturityDate = periodicScheduleTemp.Max(x => x.PaymentDate);
                        loan.OutstandingPrincipal = loan.OutstandingPrincipal - (decimal)loanInput.PayAmount;
                        loan.OutstandingInterest = outstandingInterest;
                        loan.EffectiveDate = reviewData.EffectiveDate;
                        //-------------------------------------------------

                        //-------------------------------------------------------
                    }

                }
                var result = _context.SaveChanges() > 0;

                if (result)
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return output;
        }

        public IEnumerable<DailyInterestAccrualObj> ProcessDailyLoansInterestAccrual(DateTime applicationDate)
        {
            var transactionCode = GeneralHelpers.GenerateRandomDigitCode(10);

            var data = (from a in _context.credit_loanscheduledaily
                        join b in _context.credit_loan on a.LoanId equals b.LoanId
                        join c in _context.credit_loanscheduleperiodic on b.LoanId equals c.LoanId
                        where a.Date.Date == applicationDate.Date && b.LoanStatusId == (short)LoanStatusEnum.Active
                        && a.PaymentDate.Date == c.PaymentDate.Date && c.Deleted == false && a.Deleted == false

                        select new DailyInterestAccrualObj()
                        {
                            ReferenceNumber = b.LoanRefNumber,
                            ProductId = b.ProductId,
                            CompanyId = (int)b.CompanyId,
                            CurrencyId = b.CurrencyId,
                            ExchangeRate = b.ExchangeRate,
                            InterestRate = a.InterestRate,
                            Date = applicationDate,
                            DailyAccuralAmount = (double)a.DailyInterestAmount,
                            MainAmount = c.PeriodInterestAmount,
                            CategoryId = (short)DailyAccrualCategory.TermLoan,
                            TransactionTypeId = (byte)LoanTransactionTypeEnum.Interest,
                            BaseReferenceNumber = b.LoanRefNumber,
                        }).ToList();

            List<credit_daily_accural> transAccrual = new List<credit_daily_accural>();


            foreach (var item in data)
            {
                credit_daily_accural dailyAccrual = new credit_daily_accural();

                dailyAccrual.ReferenceNumber = item.ReferenceNumber;
                dailyAccrual.ProductId = item.ProductId;
                dailyAccrual.ExchangeRate = item.ExchangeRate;
                dailyAccrual.CurrencyId = item.CurrencyId;
                dailyAccrual.InterestRate = item.InterestRate;
                dailyAccrual.Date = item.Date;
                dailyAccrual.DailyAccuralAmount = (decimal)Math.Abs(item.DailyAccuralAmount);
                dailyAccrual.Amount = item.MainAmount;
                dailyAccrual.CategoryId = item.CategoryId;
                dailyAccrual.CompanyId = item.CompanyId;
                dailyAccrual.TransactionTypeId = item.TransactionTypeId;


                transAccrual.Add(dailyAccrual);

            }
            this._context.credit_daily_accural.AddRange(transAccrual);
            _context.SaveChanges();


            var model = (from a in _context.credit_daily_accural
                         where a.Date.Date == applicationDate.Date && a.CategoryId == (short)DailyAccrualCategory.TermLoan
                         group a by new { a.ProductId, a.CompanyId, a.CurrencyId, a.ReferenceNumber } into groupedQ
                         select new DailyInterestAccrualObj()
                         {
                             ProductId = groupedQ.Key.ProductId,
                             CompanyId = groupedQ.Key.CompanyId,
                             CurrencyId = groupedQ.Key.CurrencyId,
                             DailyAccuralAmount = (double)groupedQ.Sum(i => i.DailyAccuralAmount),
                             ReferenceNumber = groupedQ.Key.ReferenceNumber,
                         }).ToList();

                     
            var transList = new List<TransactionObj>();
            foreach (var item in model)
            {
                item.Date = applicationDate;
                var product = _context.credit_product.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (product == null)
                {
                    //throw new Exception($"This product with productCode {model.productcode} no longer exist");
                }
                if (product.InterestReceivablePayableGL == null)
                {
                    //throw new Exception($"This product with product name {product.ProductName} doesn't have Interest Receivable GL");
                }
                if (product.InterestIncomeExpenseGL == null)
                {
                    //throw new Exception($"This product with product name {product.ProductName} doesn't have Interest Income Expense GL");
                }
                var entry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = string.Empty,
                    CompanyId = item.CompanyId,
                    Amount = Convert.ToDecimal(item.DailyAccuralAmount),
                    CurrencyId = 0,
                    Description = "Loan Daily Interest Accrual Posting",
                    DebitGL = product.InterestReceivablePayableGL.Value,
                    CreditGL = product.InterestIncomeExpenseGL.Value,
                    ReferenceNo = item.ReferenceNumber,
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

        public IEnumerable<DailyInterestAccrualObj> ProcessDailyPastDueInterestAccrual(DateTime applicationDate)
        {
            var transactionCode = GeneralHelpers.GenerateRandomDigitCode(10);

            var data = (from a in _context.credit_loanscheduledaily
                        join b in _context.credit_loan on a.LoanId equals b.LoanId
                        join c in _context.credit_loanscheduleperiodic on b.LoanId equals c.LoanId
                        where a.Date.Date == applicationDate.Date && b.LoanStatusId == (short)LoanStatusEnum.Active
                        && a.PaymentDate.Date == c.PaymentDate.Date && c.Deleted == false && a.Deleted == false

                        select new DailyInterestAccrualObj()
                        {
                            ReferenceNumber = b.LoanRefNumber,
                            ProductId = b.ProductId,
                            CompanyId = (int)b.CompanyId,
                            CurrencyId = b.CurrencyId,
                            ExchangeRate = b.ExchangeRate,
                            InterestRate = a.InterestRate,
                            Date = applicationDate,
                            DailyAccuralAmount = (double)a.InterestRate,
                            MainAmount = (decimal)b.PastDueInterest,
                            CategoryId = (short)DailyAccrualCategory.PastDueInterest,
                            TransactionTypeId = (byte)LoanTransactionTypeEnum.Interest,
                        }).ToList();

            List<credit_daily_accural> transAccrual = new List<credit_daily_accural>();


            foreach (var item in data)
            {
                credit_daily_accural dailyAccrual = new credit_daily_accural();

                dailyAccrual.ReferenceNumber = item.ReferenceNumber;
                dailyAccrual.ProductId = item.ProductId;
                dailyAccrual.ExchangeRate = item.ExchangeRate;
                dailyAccrual.CurrencyId = item.CurrencyId;
                dailyAccrual.InterestRate = item.InterestRate;
                dailyAccrual.Date = item.Date;
                dailyAccrual.DailyAccuralAmount = (decimal)Math.Abs(item.DailyAccuralAmount);
                dailyAccrual.Amount = item.MainAmount;
                dailyAccrual.CategoryId = item.CategoryId;
                dailyAccrual.CompanyId = item.CompanyId;
                dailyAccrual.TransactionTypeId = item.TransactionTypeId;

                transAccrual.Add(dailyAccrual);

            }
            this._context.credit_daily_accural.AddRange(transAccrual);
            _context.SaveChanges();


            var model = (from a in _context.credit_daily_accural
                         where a.Date.Date == applicationDate.Date && a.CategoryId == (short)DailyAccrualCategory.PastDueInterest
                         group a by new { a.ProductId, a.CompanyId, a.CurrencyId, a.ReferenceNumber } into groupedQ
                         select new DailyInterestAccrualObj()
                         {
                             ProductId = groupedQ.Key.ProductId,
                             CompanyId = groupedQ.Key.CompanyId,
                             CurrencyId = groupedQ.Key.CurrencyId,
                             DailyAccuralAmount = (double)groupedQ.Sum(i => i.DailyAccuralAmount),
                             ReferenceNumber = groupedQ.Key.ReferenceNumber,
                         }).ToList();
            var transList = new List<TransactionObj>();

            foreach (var item in model)
            {
                item.Date = applicationDate;
                var product = _context.credit_product.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (product == null)
                {

                }
                if (product.InterestReceivablePayableGL == null)
                {

                }
                if (product.InterestIncomeExpenseGL == null)
                {

                }
                var entry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = string.Empty,
                    CompanyId = item.CompanyId,
                    Amount = Convert.ToDecimal(item.DailyAccuralAmount),
                    CurrencyId = item.CurrencyId,
                    Description = "Loan Daily Interest Accrual Posting",
                    DebitGL = product.InterestReceivablePayableGL.Value,
                    CreditGL = product.InterestIncomeExpenseGL.Value,
                    ReferenceNo = item.ReferenceNumber,
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

        public IEnumerable<DailyInterestAccrualObj> ProcessDailyPastDuePrincipalAccrual(DateTime applicationDate)
        {

            var data = (from a in _context.credit_loanscheduledaily
                        join b in _context.credit_loan on a.LoanId equals b.LoanId
                        join c in _context.credit_loanscheduleperiodic on b.LoanId equals c.LoanId
                        where a.Date.Date == applicationDate.Date && b.LoanStatusId == (short)LoanStatusEnum.Active
                        && a.PaymentDate.Date == c.PaymentDate.Date && c.Deleted == false && a.Deleted == false

                        select new DailyInterestAccrualObj()
                        {
                            ReferenceNumber = b.LoanRefNumber,
                            ProductId = b.ProductId,
                            CompanyId = (int)b.CompanyId,
                            CurrencyId = b.CurrencyId,
                            ExchangeRate = b.ExchangeRate,
                            InterestRate = a.InterestRate,
                            Date = applicationDate,
                            DailyAccuralAmount = (double)a.InterestRate,
                            MainAmount = (decimal)b.PastDuePrincipal,
                            CategoryId = (short)DailyAccrualCategory.PastDuePrincipal,
                            TransactionTypeId = (byte)LoanTransactionTypeEnum.Interest,

                        }).ToList();

            List<credit_daily_accural> transAccrual = new List<credit_daily_accural>();


            foreach (var item in data)
            {
                credit_daily_accural dailyAccrual = new credit_daily_accural();

                dailyAccrual.ReferenceNumber = item.ReferenceNumber;
                dailyAccrual.ProductId = item.ProductId;
                dailyAccrual.ExchangeRate = item.ExchangeRate;
                dailyAccrual.CurrencyId = item.CurrencyId;
                dailyAccrual.InterestRate = item.InterestRate;
                dailyAccrual.Date = item.Date;
                dailyAccrual.DailyAccuralAmount = (decimal)Math.Abs(item.DailyAccuralAmount);
                dailyAccrual.Amount = item.MainAmount;
                dailyAccrual.CategoryId = item.CategoryId;
                dailyAccrual.CompanyId = item.CompanyId;
                dailyAccrual.TransactionTypeId = item.TransactionTypeId;
                transAccrual.Add(dailyAccrual);

            }
            this._context.credit_daily_accural.AddRange(transAccrual);
            _context.SaveChanges();


            var model = (from a in _context.credit_daily_accural
                         where a.Date.Date == applicationDate.Date && a.CategoryId == (short)DailyAccrualCategory.PastDuePrincipal
                         group a by new { a.ProductId, a.CompanyId, a.CurrencyId, a.ReferenceNumber } into groupedQ
                         select new DailyInterestAccrualObj()
                         {
                             ProductId = groupedQ.Key.ProductId,
                             CompanyId = groupedQ.Key.CompanyId,
                             CurrencyId = groupedQ.Key.CurrencyId,
                             DailyAccuralAmount = (double)groupedQ.Sum(i => i.DailyAccuralAmount),
                             ReferenceNumber = groupedQ.Key.ReferenceNumber,
                         }).ToList();


            var transList = new List<TransactionObj>();

            foreach (var item in model)
            {
                item.Date = applicationDate;
                var product = _context.credit_product.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (product == null)
                {

                }
                if (product.InterestReceivablePayableGL == null)
                {

                }
                if (product.InterestIncomeExpenseGL == null)
                {

                }
                var entry = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = string.Empty,
                    CompanyId = item.CompanyId,
                    Amount = Convert.ToDecimal(item.DailyAccuralAmount),
                    CurrencyId = item.CurrencyId,
                    Description = "Loan Daily Interest Accrual Posting",
                    DebitGL = product.InterestReceivablePayableGL.Value,
                    CreditGL = product.InterestIncomeExpenseGL.Value,
                    ReferenceNo = item.ReferenceNumber,
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

        public IEnumerable<LoanRepaymentObj> ProcessLoanRepaymentPostingPastDue(DateTime applicationDate)
        {
            try
            {
               var companyList = _serverRequest.GetAllCompanyAsync().Result;
                bool result = false;
                var model = (from a in _context.credit_loanscheduleperiodic
                             join b in _context.credit_loan on a.LoanId equals b.LoanId
                             where a.PaymentDate.Date == applicationDate.Date && b.LoanStatusId == (short)LoanStatusEnum.Active
                            && a.PeriodPrincipalAmount != 0 && a.PeriodInterestAmount != 0 && b.IsDisbursed == true && a.Deleted == false
                             select new LoanRepaymentObj()
                             {
                                 ProductId = b.ProductId,
                                 CompanyId = (int)b.CompanyId,
                                 CurrencyId = b.CurrencyId,
                                 ExchangeRate = b.ExchangeRate,
                                 PeriodInterestAmount = a.PeriodInterestAmount,
                                 PeriodPrincipalAmount = a.PeriodPrincipalAmount,
                                 InterestRate = a.InterestRate,
                                 PaymentDate = applicationDate,
                                 LoanId = a.LoanId,
                                 TotalAmount = a.PeriodInterestAmount + a.PeriodPrincipalAmount,
                                 CasaAccountNumber = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == b.CustomerId).CASAAccountNumber,
                                 LoanRefNo = b.LoanRefNumber,
                                 ClosingBalance = (decimal)(b.OutstandingInterest + b.OutstandingPrincipal + b.PastDueInterest + b.PastDuePrincipal),
                             }).ToList();

                List<credit_loan_past_due> transPastDue = new List<credit_loan_past_due>();
                List<credit_loan_repayment> transRepayment = new List<credit_loan_repayment>();
                List<credit_casa_lien> transLien = new List<credit_casa_lien>();
                var transList = new List<TransactionObj>();

                foreach (var item in model)
                {
                    decimal casabalance = 0;
                    var PastDueCode = GeneralHelpers.GenerateRandomDigitCode(10);
                    var product = _context.credit_product.FirstOrDefault(x => x.ProductId == item.ProductId);
                    if (product.InterestReceivablePayableGL == null)
                    {
                        //throw new Exception($"This product with product name {product.ProductName} doesn't have Interest Receivable GL");
                    }
                    if (product.PrincipalGL == null)
                    {
                        //throw new Exception($"This product with product name {product.ProductName} doesn't have Product Principal GL");
                    }

                    var casa = this._context.credit_casa.FirstOrDefault(x => x.AccountNumber == item.CasaAccountNumber);
                    casabalance = casa.AvailableBalance;

                    decimal principalAmountNotCollected = 0;
                    decimal partialPrincipalAmountCollected = 0;
                    decimal partialInterestAmountCollected = 0;
                    decimal interestAmountNotCollected = 0;

                    var customer = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == item.LoanId);
                    var acctNumber = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == customer.CustomerId).CASAAccountNumber;
                    var depositCustomer = _context.deposit_accountopening.Where(x => x.AccountNumber == acctNumber).FirstOrDefault();
                    var operatingAccount = _context.deposit_accountsetup.Where(x => x.DepositAccountId == depositCustomer.AccountTypeId).FirstOrDefault();

                    var entry1 = new TransactionObj
                    {
                        IsApproved = false,
                        CasaAccountNumber = customer.CASAAccountNumber,
                        CompanyId = item.CompanyId,
                        Amount = Convert.ToDecimal(item.PeriodPrincipalAmount),
                        CurrencyId = item.CurrencyId,
                        Description = "Principal Repayment",
                        DebitGL = operatingAccount.GLMapping ?? 0,
                        CreditGL = product.PrincipalGL.Value,
                        ReferenceNo = item.LoanRefNo,
                        OperationId = (int)OperationsEnum.DailyInterestAccural,
                        JournalType = "System",
                        RateType = 2,//Buying Rate
                    };
                    //transList.Add(entry1);
                    var entry2 = new TransactionObj
                    {
                        IsApproved = false,
                        CasaAccountNumber = customer.CASAAccountNumber,
                        CompanyId = item.CompanyId,
                        Amount = Convert.ToDecimal(item.PeriodInterestAmount),
                        CurrencyId = item.CurrencyId,
                        Description = "Interest Repayment",
                        DebitGL = operatingAccount.GLMapping ?? 0,
                        CreditGL = product.InterestIncomeExpenseGL.Value,
                        ReferenceNo = item.LoanRefNo,
                        OperationId = (int)OperationsEnum.InterestLoanRepayment,
                        JournalType = "System",
                        RateType = 2,//Buying Rate
                    };
                    //transList.Add(entry2);

                    CustomerTransactionObj customerEntry = new CustomerTransactionObj
                    {
                        CasaAccountNumber = customer.CASAAccountNumber,
                        Description = "Loan Repayment",
                        TransactionDate = DateTime.Now,
                        ValueDate = applicationDate,
                        TransactionType = "Debit",
                        CreditAmount = 0,
                        DebitAmount = Convert.ToDecimal(item.PeriodInterestAmount + item.PeriodPrincipalAmount),
                        Beneficiary = companyList.companyStructures.FirstOrDefault(x=>x.companyStructureId == item.CompanyId)?.name,
                        ReferenceNo = item.LoanRefNo,
                    };

                    _customerTransaction.CustomerTransactionEoD(customerEntry);

                    if (casabalance >= item.TotalAmount)
                    {
                        var res1 = _serverRequest.PassEntryToFinance(entry1).Result;
                        var res2 = _serverRequest.PassEntryToFinance(entry2).Result;

                        updateloanTablePrincipal(item.LoanId, item.PeriodPrincipalAmount);
                        updateloanTableInterest(item.LoanId, item.PeriodInterestAmount);

                        credit_loan_repayment repayment = new credit_loan_repayment();
                        repayment.LoanId = item.LoanId;
                        repayment.Date = item.PaymentDate;
                        repayment.InterestAmount = item.PeriodInterestAmount;
                        repayment.PrincipalAmount = item.PeriodPrincipalAmount;
                        repayment.ClosingBalance = item.ClosingBalance - (item.PeriodInterestAmount + item.PeriodPrincipalAmount);

                        transRepayment.Add(repayment);

                    }
                    else if (casabalance > item.PeriodInterestAmount && casabalance < item.TotalAmount)
                    {
                        partialPrincipalAmountCollected = casabalance - item.PeriodInterestAmount; ;
                        principalAmountNotCollected = item.PeriodPrincipalAmount - partialPrincipalAmountCollected;
                        credit_loan_past_due pastDue = new credit_loan_past_due();

                        pastDue.LoanId = item.LoanId;
                        pastDue.PastDueCode = PastDueCode;
                        pastDue.CreditAmount = 0;
                        pastDue.Description = "Past Due Entries on Principal as a result of Account not funded";
                        pastDue.DebitAmount = Math.Abs(principalAmountNotCollected);
                        pastDue.Date = item.PaymentDate;
                        pastDue.DateWithDefault = item.PaymentDate.AddDays((double)product.Default);
                        pastDue.TransactionTypeId = (byte)LoanTransactionTypeEnum.Principal;
                        pastDue.Parent_PastDueCode = item.LoanRefNo;
                        pastDue.ProductTypeId = (int)product.ProductTypeId;

                        transPastDue.Add(pastDue);

                        updateloanTablePastDuePrincipal(pastDue.LoanId, pastDue.DebitAmount);

                        var res1 = _serverRequest.PassEntryToFinance(entry1).Result;
                        var res2 = _serverRequest.PassEntryToFinance(entry2).Result;

                        updateloanTableInterest(item.LoanId, item.PeriodInterestAmount);
                        updateloanTablePrincipal(item.LoanId, partialPrincipalAmountCollected);

                        credit_loan_repayment repayment = new credit_loan_repayment();
                        repayment.LoanId = item.LoanId;
                        repayment.Date = item.PaymentDate;
                        repayment.InterestAmount = item.PeriodInterestAmount;
                        repayment.PrincipalAmount = partialPrincipalAmountCollected;
                        repayment.ClosingBalance = item.ClosingBalance - (item.PeriodInterestAmount + partialPrincipalAmountCollected);

                        transRepayment.Add(repayment);


                        credit_casa_lien lien = new credit_casa_lien();

                        lien.ProductAccountNumber = casa.AccountNumber;
                        lien.SourceReferenceNumber = pastDue.Parent_PastDueCode;
                        lien.LienReferenceNumber = pastDue.Parent_PastDueCode;
                        lien.LienDebitAmount = pastDue.DebitAmount;
                        lien.CompanyId = item.CompanyId;
                        lien.LienTypeId = (short)LienTypeEnum.PrincipalRepayment;
                        lien.Description = "lien placed due to Account not funded at Anniversary Date";

                        transLien.Add(lien);

                    }
                    else if (casabalance < item.PeriodInterestAmount && casabalance > 0)
                    {
                        partialInterestAmountCollected = casabalance;
                        interestAmountNotCollected = item.PeriodInterestAmount - partialInterestAmountCollected;
                        credit_loan_past_due pastDueInterest = new credit_loan_past_due();

                        pastDueInterest.LoanId = item.LoanId;
                        pastDueInterest.PastDueCode = PastDueCode;
                        pastDueInterest.CreditAmount = 0;
                        pastDueInterest.Description = "Past Due Entries on Interest as a result of Account not funded";
                        pastDueInterest.DebitAmount = Math.Abs(interestAmountNotCollected);
                        pastDueInterest.Date = item.PaymentDate;
                        pastDueInterest.TransactionTypeId = (byte)LoanTransactionTypeEnum.Interest;
                        pastDueInterest.Parent_PastDueCode = item.LoanRefNo;
                        pastDueInterest.ProductTypeId = (int)product.ProductTypeId;

                        transPastDue.Add(pastDueInterest);


                        updateloanTablePastDueInterest(pastDueInterest.LoanId, pastDueInterest.DebitAmount);

                        credit_loan_past_due pastDuePrincipal = new credit_loan_past_due();

                        pastDuePrincipal.LoanId = item.LoanId;
                        pastDuePrincipal.PastDueCode = PastDueCode;
                        pastDuePrincipal.CreditAmount = 0;
                        pastDuePrincipal.Description = "Past Due Entries on Principal as a result of Account not funded";
                        pastDuePrincipal.DebitAmount = item.PeriodPrincipalAmount;
                        pastDuePrincipal.Date = item.PaymentDate;
                        pastDuePrincipal.TransactionTypeId = (byte)LoanTransactionTypeEnum.Principal;
                        pastDuePrincipal.Parent_PastDueCode = item.LoanRefNo;
                        pastDuePrincipal.ProductTypeId = (int)product.ProductTypeId;

                        transPastDue.Add(pastDuePrincipal);


                        updateloanTablePastDuePrincipal(pastDuePrincipal.LoanId, pastDuePrincipal.DebitAmount);

                        var res1 = _serverRequest.PassEntryToFinance(entry1).Result;
                        var res2 = _serverRequest.PassEntryToFinance(entry2).Result;

                        updateloanTableInterest(item.LoanId, partialInterestAmountCollected);

                        credit_loan_repayment repayment = new credit_loan_repayment();
                        repayment.LoanId = item.LoanId;
                        repayment.Date = item.PaymentDate;
                        repayment.InterestAmount = partialInterestAmountCollected;
                        repayment.PrincipalAmount = 0;
                        repayment.ClosingBalance = item.ClosingBalance - (partialInterestAmountCollected + 0);

                        transRepayment.Add(repayment);

                        credit_casa_lien lien = new credit_casa_lien();

                        lien.ProductAccountNumber = casa.AccountNumber;
                        lien.SourceReferenceNumber = item.LoanRefNo;
                        lien.LienReferenceNumber = pastDueInterest.Parent_PastDueCode;
                        lien.LienDebitAmount = pastDueInterest.DebitAmount;
                        lien.CompanyId = item.CompanyId;
                        lien.LienTypeId = (short)LienTypeEnum.InterestRepayment;
                        lien.Description = "lien placed due to Account not funded at Anniversary Date";

                        transLien.Add(lien);

                        credit_casa_lien lienPrincipal = new credit_casa_lien();

                        lienPrincipal.ProductAccountNumber = casa.AccountNumber;
                        lienPrincipal.SourceReferenceNumber = item.LoanRefNo;
                        lienPrincipal.LienReferenceNumber = pastDuePrincipal.Parent_PastDueCode;
                        lienPrincipal.LienDebitAmount = pastDuePrincipal.DebitAmount;
                        lienPrincipal.CompanyId = item.CompanyId;
                        lienPrincipal.LienTypeId = (short)LienTypeEnum.PrincipalRepayment;
                        lienPrincipal.Description = "lien placed due to Account not funded at Anniversary Date";

                        transLien.Add(lienPrincipal);
                    }
                    else if (casabalance <= 0)
                    {
                        credit_loan_past_due pastDueInterest = new credit_loan_past_due();

                        pastDueInterest.LoanId = item.LoanId;
                        pastDueInterest.PastDueCode = PastDueCode;
                        pastDueInterest.CreditAmount = 0;
                        pastDueInterest.Description = "Past Due Entries on Interest as a result of Account not funded";
                        pastDueInterest.DebitAmount = Math.Abs(item.PeriodInterestAmount);
                        pastDueInterest.Date = item.PaymentDate;
                        pastDueInterest.TransactionTypeId = (byte)LoanTransactionTypeEnum.Interest;
                        pastDueInterest.Parent_PastDueCode = item.LoanRefNo;
                        pastDueInterest.ProductTypeId = (int)product.ProductTypeId;

                        transPastDue.Add(pastDueInterest);

                        updateloanTablePastDueInterest(pastDueInterest.LoanId, pastDueInterest.DebitAmount);

                        credit_loan_past_due pastDuePrincipal = new credit_loan_past_due();

                        pastDuePrincipal.LoanId = item.LoanId;
                        pastDuePrincipal.PastDueCode = PastDueCode;
                        pastDuePrincipal.CreditAmount = 0;
                        pastDuePrincipal.Description = "Past Due Entries on Principal as a result of Account not funded";
                        pastDuePrincipal.DebitAmount = Math.Abs(item.PeriodPrincipalAmount);
                        pastDuePrincipal.Date = item.PaymentDate;
                        pastDuePrincipal.TransactionTypeId = (byte)LoanTransactionTypeEnum.Principal;
                        pastDuePrincipal.Parent_PastDueCode = item.LoanRefNo;
                        pastDuePrincipal.ProductTypeId = (int)product.ProductTypeId;

                        transPastDue.Add(pastDuePrincipal);
                        updateloanTablePastDuePrincipal(pastDuePrincipal.LoanId, pastDuePrincipal.DebitAmount);

                        var res1 = _serverRequest.PassEntryToFinance(entry1).Result;
                        var res2 = _serverRequest.PassEntryToFinance(entry2).Result;

                        credit_loan_repayment repayment = new credit_loan_repayment();
                        repayment.LoanId = item.LoanId;
                        repayment.Date = item.PaymentDate;
                        repayment.InterestAmount = 0;
                        repayment.PrincipalAmount = 0;
                        repayment.ClosingBalance = item.ClosingBalance;

                        transRepayment.Add(repayment);

                        credit_casa_lien lien = new credit_casa_lien();

                        lien.ProductAccountNumber = casa.AccountNumber;
                        lien.SourceReferenceNumber = item.LoanRefNo;
                        lien.LienDebitAmount = pastDueInterest.DebitAmount;
                        lien.LienReferenceNumber = pastDueInterest.Parent_PastDueCode;
                        lien.CompanyId = item.CompanyId;
                        lien.LienTypeId = (short)LienTypeEnum.InterestRepayment;
                        lien.Description = "lien placed due to Account not funded at Anniversary Date";

                        transLien.Add(lien);


                        credit_casa_lien lienPrincipal = new credit_casa_lien();

                        lienPrincipal.ProductAccountNumber = casa.AccountNumber;
                        lienPrincipal.SourceReferenceNumber = item.LoanRefNo;
                        lienPrincipal.LienReferenceNumber = pastDuePrincipal.Parent_PastDueCode;
                        lienPrincipal.LienDebitAmount = pastDuePrincipal.DebitAmount;
                        lienPrincipal.CompanyId = item.CompanyId;
                        lienPrincipal.LienTypeId = (short)LienTypeEnum.PrincipalRepayment;
                        lienPrincipal.Description = "lien placed due to Account not funded at Anniversary Date";

                        transLien.Add(lienPrincipal);

                    }
                }

                this._context.credit_loan_past_due.AddRange(transPastDue);
                this._context.credit_loan_repayment.AddRange(transRepayment);
                this._context.credit_casa_lien.AddRange(transLien);
                var results = _context.SaveChanges() > 1;
                if (results != false)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                if (result)
                {
                    return model;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool TenorExtension(int loanId, LoanPaymentRestructureScheduleInputObj loanInput, DateTime applicationDate, int staffId)
        {
            throw new NotImplementedException();
        }

        public void updateHistoricalLoanBalance()
        {
            List<credit_individualapplicationscorecard_history> histories = new List<credit_individualapplicationscorecard_history>();
            var history = _context.credit_individualapplicationscorecard_history.Where(x => x.Deleted == false).ToList();
            foreach (var item in history)
            {
                var loan = _context.credit_loan.Where(x => x.LoanRefNumber == item.LoanReferenceNumber && x.Deleted == false).FirstOrDefault();
                var historicalLoan = _context.credit_individualapplicationscorecard_history.Where(x => x.LoanReferenceNumber == item.LoanReferenceNumber).FirstOrDefault();
                if (historicalLoan != null && loan != null)
                {
                    historicalLoan.OutstandingBalance = loan.OutstandingPrincipal + loan.OutstandingInterest;
                }

            }
            _context.SaveChanges();
        }


        //////PRIVATE METHODS
        ///
        private ApprovalRegRespObj DisburseAppraisedLoan(int loanId, int staffId, string createdBy, LoanReviewOperationObj model)
        {
            ApprovalRegRespObj response = new ApprovalRegRespObj();
            var loan = _context.credit_loan_temp.Where(x => x.LoanId == loanId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            var loanApplication = _context.credit_loanapplication.Find(loan.LoanApplicationId);
            var loanrev = _context.credit_loanreviewapplication.Where(x => x.LoanId == loanId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            model.OperationId = loanrev.OperationId;
            model.OperationTypeId = loanrev.OperationId;
            model.Prepayment = loanrev.Prepayment ?? 0;
            DeleteExistingSchedule(loanId);
            var Original_loan = _context.credit_loan.Where(x => x.LoanId == loanId).FirstOrDefault();

            deposit_accountsetup operatingAccount = new deposit_accountsetup();

            var accountNuber = _context.credit_loancustomer.FirstOrDefault(x => x.CustomerId == Original_loan.CustomerId);
            var depositCustomer = _context.deposit_accountopening.Where(x => x.AccountNumber == accountNuber.CASAAccountNumber).FirstOrDefault();
            if (depositCustomer == null)
            {
                operatingAccount = _context.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == 3);
            }
            else
            {
                operatingAccount = _context.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == depositCustomer.AccountTypeId);
            }

            LoanPaymentScheduleInputObj input = new LoanPaymentScheduleInputObj
            {
                ScheduleMethodId = (short)loan.ScheduleTypeId,
                PrincipalAmount = (double)loan.PrincipalAmount,
                EffectiveDate = model.ProposedEffectiveDate,
                InterestRate = loan.InterestRate,
                PrincipalFrequency = loan.PrincipalFrequencyTypeId,
                InterestFrequency = loan.InterestFrequencyTypeId,
                PrincipalFirstpaymentDate = (DateTime)model.PrincipalFirstPaymentDate,
                InterestFirstpaymentDate = (DateTime)model.InterestFirstPaymentDate,
                MaturityDate = (DateTime)model.MaturityDate,
                AccurialBasis = (int)loan.AccrualBasis,
                IntegralFeeAmount = 0,
                FirstDayType = (int)loan.FirstDayType,
                CreatedBy = createdBy,
                IrregularPaymentSchedule = null
            };

            if ((int)OperationsEnum.Prepayment != model.OperationTypeId)
            {
                decimal new_entry;
                var OOP = Original_loan.OutstandingOldPrincipal ?? 0;
                var OP = Original_loan.OutstandingPrincipal ?? 0;
                if (OOP > OP)
                {
                    new_entry = (OOP - OP);
                }
                else
                {
                    new_entry = (OP - OOP);
                }

                model.OperationId = 16;
                model.CurrencyId = Original_loan.CurrencyId;
                model.ProductId = Original_loan.ProductId;
                model.CompanyId = Original_loan.CompanyId ?? 0;
                model.LoanReferenceNumber = Original_loan.LoanRefNumber;
                model.PrincipalAmountIR = Original_loan.OutstandingOldInterest ?? 0;
                model.InterestAmountIR = Original_loan.OutstandingOldInterest ?? 0;
                model.PrincipalAmount = new_entry;
                model.InterestIncome = new_entry;

                if (OOP > OP)
                {
                    //Interest Income Posting
                    //financeTransaction.BuildLoanRestructuringPostingIC(model);
                    var LoanDisbursementEntryIC = new TransactionObj
                    {
                        IsApproved = false,
                        CasaAccountNumber = _context.credit_loancustomer.Where(x => x.CustomerId == Original_loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                        CompanyId = model.CompanyId,
                        Amount = model.InterestIncome,
                        CurrencyId = model.CurrencyId,
                        Description = "Loan Restructuring",
                        DebitGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).InterestIncomeExpenseGL.Value,
                        CreditGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).PrincipalGL.Value,
                        ReferenceNo = model.LoanReferenceNumber,
                        OperationId = 16,
                        JournalType = "System",
                        RateType = 1,//Buying Rate
                    };
                    var res1 = _serverRequest.PassEntryToFinance(LoanDisbursementEntryIC).Result;
                    if (res1.Status.IsSuccessful == false)
                    {
                        //return new ApprovalRegRespObj
                        //{
                        //    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res1.Status.Message.FriendlyMessage } }
                        //};
                    }
                }
                else
                {
                    //Interest Income Posting(Inverse)
                    //financeTransaction.BuildLoanRestructuringPostingIC_Inverse(model);
                    var LoanDisbursementEntryIC_Inverse = new TransactionObj
                    {
                        IsApproved = false,
                        CasaAccountNumber = _context.credit_loancustomer.Where(x => x.CustomerId == Original_loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                        CompanyId = model.CompanyId,
                        Amount = model.PrincipalAmount,
                        CurrencyId = model.CurrencyId,
                        Description = "Loan Restructuring",
                        DebitGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).PrincipalGL.Value,
                        CreditGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).InterestIncomeExpenseGL.Value,
                        ReferenceNo = model.LoanReferenceNumber,
                        OperationId = 16,
                        JournalType = "System",
                        RateType = 1,//Buying Rate
                    };
                    var res2 = _serverRequest.PassEntryToFinance(LoanDisbursementEntryIC_Inverse).Result;
                    if (res2.Status.IsSuccessful == false)
                    {
                        //return new ApprovalRegRespObj
                        //{
                        //    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res2.Status.Message.FriendlyMessage } }
                        //};
                    }
                }

                //Interest Receiveable Posting
                //financeTransaction.BuildLoanRestructuringPostingIR(model);
                var LoanDisbursementEntryIR = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _context.credit_loancustomer.Where(x => x.CustomerId == Original_loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = model.CompanyId,
                    Amount = model.PrincipalAmount,
                    CurrencyId = model.CurrencyId,
                    Description = "Loan Restructuring",
                    DebitGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).PrincipalGL.Value,
                    CreditGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).InterestReceivablePayableGL.Value,
                    ReferenceNo = model.LoanReferenceNumber,
                    OperationId = 16,
                    JournalType = "System",
                    RateType = 1,//Buying Rate
                };
                var res3 = _serverRequest.PassEntryToFinance(LoanDisbursementEntryIR).Result;
                if (res3.Status.IsSuccessful == false)
                {
                    //return new ApprovalRegRespObj
                    //{
                    //    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res3.Status.Message.FriendlyMessage } }
                    //};
                }
            }
            else
            {
                //financeTransaction.PostBuildLoanPrepaymentPosting(model, model.prepayment, "Loan Prepayment", model.operationId);
                var LoanDisbursementEntryPrepayment = new TransactionObj
                {
                    IsApproved = false,
                    CasaAccountNumber = _context.credit_loancustomer.Where(x => x.CustomerId == Original_loan.CustomerId).FirstOrDefault().CASAAccountNumber,
                    CompanyId = model.CompanyId,
                    Amount = model.PrincipalAmount,
                    CurrencyId = model.CurrencyId,
                    Description = "Loan Restructuring",
                    DebitGL = operatingAccount.GLMapping ??0,
                    CreditGL = _context.credit_product.FirstOrDefault(x => x.ProductId == Original_loan.ProductId).PrincipalGL.Value,
                    ReferenceNo = model.LoanReferenceNumber,
                    OperationId = 16,
                    JournalType = "System",
                    RateType = 1,//Buying Rate
                };
                var res3 = _serverRequest.PassEntryToFinance(LoanDisbursementEntryPrepayment).Result;
                if (res3.Status.IsSuccessful == false)
                {
                    //return new ApprovalRegRespObj
                    //{
                    //    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res3.Status.Message.FriendlyMessage } }
                    //};
                }
            }

            response.AnyIdentifier = loanId;
            response.loanPayment = input;
            //_schedule.AddLoanSchedule(loanId, input);


            loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
            loan.ApprovedBy = staffId;
            loan.ApprovedDate = DateTime.Now;

            Original_loan.DisbursedBy = staffId;
            Original_loan.DisbursedDate = DateTime.Now;
            loan.IsDisbursed = true;
            loan.LoanStatusId = (int)LoanStatusEnum.Active;
            loanApplication.LoanApplicationStatusId = (int)ApplicationStatus.Disbursed;
            Original_loan.OutstandingInterest = 0;
            _context.SaveChanges();
            return response;
        }

        private void DeleteExistingSchedule(int loanId)
        {
            var schedule = _context.credit_loanscheduleperiodic.Where(x => x.LoanId == loanId && x.Deleted == false).ToList();
            foreach (var curr in schedule)
            {
                //curr.Deleted = true;
                _context.credit_loanscheduleperiodic.Remove(curr);
            }
            var response = _context.SaveChanges();
        }

        private int LoanExist(int loanId)
        {
            var loanRef = (from a in _context.credit_loan
                           where a.LoanId == loanId
                           select a);
            int loanRefResults = loanRef.Count();

            return loanRefResults;
        }

        private bool DeleteLoanExist(int loanId)
        {

            /// change to every table here to Archive 
            bool output = false;


            var removeLoan_Archive = (from p in _context.credit_loan_archive /// change to credit_loan_Archive 
                                      where p.LoanId == loanId
                                      select p);
            var removeLoan_Schedule_Periodic_Archive = (from p in _context.credit_loanscheduleperiodicarchive
                                                        where p.LoanId == loanId
                                                        select p);
            var removeLoan_Schedule_Daily_Archive = (from p in _context.credit_loanscheduledailyarchive
                                                     where p.LoanId == loanId
                                                     select p);
            var removeLoan_Schedule_Daily_Temp = (from p in _context.credit_temp_loanscheduledaily
                                                  where p.LoanId == loanId
                                                  select p);
            var removeLoan_Schedule_Periodic_Temp = (from p in _context.credit_temp_loanscheduleperiodic
                                                     where p.LoanId == loanId
                                                     select p);

            if (removeLoan_Archive != null)
            {
                _context.credit_loan_archive.RemoveRange(removeLoan_Archive);
            }
            if (removeLoan_Schedule_Periodic_Archive != null)
            {
                _context.credit_loanscheduleperiodicarchive.RemoveRange(removeLoan_Schedule_Periodic_Archive);

            }
            if (removeLoan_Schedule_Daily_Archive != null)
            {
                _context.credit_loanscheduledailyarchive.RemoveRange(removeLoan_Schedule_Daily_Archive);

            }
            if (removeLoan_Schedule_Daily_Temp != null)
            {
                _context.credit_temp_loanscheduledaily.RemoveRange(removeLoan_Schedule_Daily_Temp);

            }
            if (removeLoan_Schedule_Periodic_Temp != null)
            {
                _context.credit_temp_loanscheduleperiodic.RemoveRange(removeLoan_Schedule_Periodic_Temp);

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {              
                throw ex;
            }
            //_context.SaveChanges();
            output = true;

            return output;
        }

        private IEnumerable<LoanObj> ArchiveLoan(int loanId, int operationId)
        {
            var model = (from a in _context.credit_loan
                         where a.LoanId == loanId && a.LoanStatusId == (short)LoanStatusEnum.Active
                         select new LoanObj()
                         {
                             LoanId = a.LoanId,
                             LoanRefNumber = a.LoanRefNumber,
                             CustomerRiskRatingId = a.CustomerRiskRatingId,
                             CustomerId = a.CustomerId,
                             ProductId = a.ProductId,
                             CompanyId = (int)a.CompanyId,
                             CurrencyId = a.CurrencyId,
                             InterestRate = a.InterestRate,
                             PrincipalFrequencyTypeId = a.PrincipalFrequencyTypeId,
                             InterestFrequencyTypeId = a.InterestFrequencyTypeId,
                             EffectiveDate = a.EffectiveDate,
                             MaturityDate = a.MaturityDate,
                             BookingDate = a.BookingDate,
                             PrincipalAmount = a.PrincipalAmount,
                             ApprovalStatusId = a.ApprovalStatusId,
                             ApprovedBy = a.ApprovedBy,
                             ApprovedComments = a.ApprovedComments,
                             ApprovedDate = a.ApprovedDate,
                             LoanStatusId = (int)a.LoanStatusId,
                             ScheduleTypeId = (int)a.ScheduleTypeId,
                             IsDisbursed = a.IsDisbursed,
                             DisbursedBy = a.DisbursedBy,
                             DisbursedComments = a.DisbursedComments,
                             DisbursedDate = a.DisbursedDate,
                             EquityContribution = (decimal)a.EquityContribution,
                             FirstPrincipalPaymentDate = a.FirstPrincipalPaymentDate,
                             FirstInterestPaymentDate = a.FirstInterestPaymentDate,
                             OutstandingPrincipal = (decimal)a.OutstandingPrincipal,
                             OutstandingInterest = (decimal)a.OutstandingInterest,
                             NplDate = a.NPLDate,
                             CreatedBy = a.CreatedBy,
                             BranchId = (int)a.BranchId,
                             CasaAccountId = (int)a.CasaAccountId,
                             PastDueInterest = (decimal)a.PastDueInterest,
                             PastDuePrincipal = (decimal)a.PastDuePrincipal,
                             InterestOnPastDueInterest = (decimal)a.InterestOnPastDueInterest,
                             InterestOnPastDuePrincipal = (decimal)a.InterestOnPastDuePrincipal

                         }).ToList();

            List<credit_loan_archive> loanArchive = new List<credit_loan_archive>();



            foreach (var item in model)
            {

                credit_loan_archive addLoanArchive = new credit_loan_archive();
                addLoanArchive.ChangeEffectiveDate = DateTime.Today;
                addLoanArchive.ChangeReason = "Rephasement";
                addLoanArchive.LoanId = item.LoanId;
                addLoanArchive.CustomerRiskRatingId = item.CustomerRiskRatingId;
                addLoanArchive.CustomerId = item.CustomerId;
                addLoanArchive.ProductId = item.ProductId;
                addLoanArchive.CompanyId = item.CompanyId;
                addLoanArchive.CurrencyId = (int)item.CurrencyId;
                addLoanArchive.ExchangeRate = item.ExchangeRate;
                addLoanArchive.LoanRefNumber = item.LoanRefNumber;
                addLoanArchive.PrincipalFrequencyTypeId = item.PrincipalFrequencyTypeId;
                addLoanArchive.InterestFrequencyTypeId = item.InterestFrequencyTypeId;
                addLoanArchive.InterestRate = (double)item.InterestRate;
                addLoanArchive.EffectiveDate = item.EffectiveDate;
                addLoanArchive.MaturityDate = item.MaturityDate;
                addLoanArchive.BookingDate = item.BookingDate;
                addLoanArchive.PrincipalAmount = item.PrincipalAmount;
                addLoanArchive.ApprovalStatusId = item.ApprovalStatusId;
                addLoanArchive.ApprovedBy = (int)item.ApprovedBy;
                addLoanArchive.ApprovedComments = item.ApprovedComments;
                addLoanArchive.ApprovedDate = item.ApprovedDate;
                addLoanArchive.LoanStatusId = item.LoanStatusId;
                addLoanArchive.CreatedBy = item.CreatedBy;
                addLoanArchive.CreatedOn = item.CreatedOn;
                addLoanArchive.ScheduleTypeId = item.ScheduleTypeId;
                addLoanArchive.IsDisbursed = item.IsDisbursed;
                addLoanArchive.DisbursedBy = item.DisbursedBy;
                addLoanArchive.DisbursedComments = item.DisbursedComments;
                addLoanArchive.DisbursedDate = item.DisbursedDate;
                addLoanArchive.OperationId = operationId;
                addLoanArchive.EquityContribution = item.EquityContribution;
                addLoanArchive.FirstPrincipalPaymentDate = item.FirstPrincipalPaymentDate;
                addLoanArchive.FirstInterestPaymentDate = item.FirstInterestPaymentDate;
                addLoanArchive.OutstandingPrincipal = item.OutstandingPrincipal;
                addLoanArchive.OutstandingInterest = item.OutstandingInterest;
                addLoanArchive.PrincipalFrequencyTypeId = item.PrincipalFrequencyTypeId;
                addLoanArchive.NPLDate = item.NplDate;
                addLoanArchive.PastDueInterest = item.PastDueInterest;
                addLoanArchive.PastDuePrincipal = item.PastDuePrincipal;
                addLoanArchive.InterestOnPastDueInterest = item.InterestOnPastDueInterest;
                addLoanArchive.InterestOnPastDuePrincipal = item.InterestOnPastDuePrincipal;
                addLoanArchive.BranchId = item.BranchId;
                addLoanArchive.CasaAccountId = item.CasaAccountId;

                loanArchive.Add(addLoanArchive);
            }
            //tbl_Loan
            this._context.credit_loan_archive.AddRange(loanArchive);

            _context.SaveChanges();
            return model;
        }

        private IEnumerable<LoanPaymentSchedulePeriodicObj> ArchivePeriodicSchedule(int loanId)
        {
            var batchCode = GeneralHelpers.GenerateRandomDigitCode(5);
            var model = (from a in _context.credit_loanscheduleperiodic
                         where a.LoanId == loanId
                         select new LoanPaymentSchedulePeriodicObj()
                         {
                             LoanId = a.LoanId,
                             PaymentNumber = a.PaymentNumber,
                             PaymentDate = a.PaymentDate,
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
                             CreatedBy = a.CreatedBy,
                             CreatedOn = a.CreatedOn,

                         }).ToList();

            List<credit_loanscheduleperiodicarchive> loanSchedulePeriodicArchive = new List<credit_loanscheduleperiodicarchive>();



            foreach (var item in model)
            {

                credit_loanscheduleperiodicarchive addLoanSchedulePeriodicArchive = new credit_loanscheduleperiodicarchive();
                addLoanSchedulePeriodicArchive.LoanId = item.LoanId;
                addLoanSchedulePeriodicArchive.PaymentNumber = item.PaymentNumber;
                addLoanSchedulePeriodicArchive.PaymentDate = item.PaymentDate;
                addLoanSchedulePeriodicArchive.StartPrincipalAmount = (decimal)item.StartPrincipalAmount;
                addLoanSchedulePeriodicArchive.PeriodPaymentAmount = (decimal)item.PeriodPaymentAmount;
                addLoanSchedulePeriodicArchive.PeriodInterestAmount = (decimal)item.PeriodInterestAmount;
                addLoanSchedulePeriodicArchive.PeriodPrincipalAmount = (decimal)item.PeriodPrincipalAmount;
                addLoanSchedulePeriodicArchive.EndPrincipalAmount = (decimal)item.EndPrincipalAmount;

                addLoanSchedulePeriodicArchive.InterestRate = item.InterestRate;
                addLoanSchedulePeriodicArchive.AmortisedStartPrincipalAmount = (decimal)item.AmortisedStartPrincipalAmount;
                addLoanSchedulePeriodicArchive.AmortisedPeriodPaymentAmount = (decimal)item.AmortisedPeriodPaymentAmount;
                addLoanSchedulePeriodicArchive.AmortisedPeriodInterestAmount = (decimal)item.AmortisedPeriodInterestAmount;
                addLoanSchedulePeriodicArchive.AmortisedPeriodPrincipalAmount = (decimal)item.AmortisedPeriodPrincipalAmount;
                addLoanSchedulePeriodicArchive.AmortisedEndPrincipalAmount = (decimal)item.AmortisedEndPrincipalAmount;
                addLoanSchedulePeriodicArchive.EffectiveInterestRate = item.EffectiveInterestRate;
                addLoanSchedulePeriodicArchive.CreatedBy = item.CreatedBy;
                addLoanSchedulePeriodicArchive.CreatedOn = item.CreatedOn;
                //addLoanSchedulePeriodicArchive.ArchiveDate = generalSetup.GetApplicationDate();
                addLoanSchedulePeriodicArchive.ArchiveBatchCode = batchCode;

                loanSchedulePeriodicArchive.Add(addLoanSchedulePeriodicArchive);

            }

            this._context.credit_loanscheduleperiodicarchive.AddRange(loanSchedulePeriodicArchive);

            _context.SaveChanges();
            return model;
        }

        private IEnumerable<LoanPaymentScheduleDailyObj> ArchiveDailySchedule(int loanId)
        {
            var batchCode = GeneralHelpers.GenerateRandomDigitCode(5);
            var model = (from a in _context.credit_loanscheduledaily
                         where a.LoanId == loanId
                         select new LoanPaymentScheduleDailyObj()
                         {
                             LoanId = a.LoanId,
                             PaymentNumber = a.PaymentNumber,
                             Date = a.Date,
                             PaymentDate = a.PaymentDate,
                             OpeningBalance = (double)a.OpeningBalance,
                             StartPrincipalAmount = (double)a.StartPrincipalAmount,
                             DailyPaymentAmount = (double)a.DailyPaymentAmount,
                             DailyInterestAmount = (double)a.DailyInterestAmount,
                             DailyPrincipalAmount = (double)a.DailyPrincipalAmount,
                             ClosingBalance = (double)a.ClosingBalance,
                             EndPrincipalAmount = (double)a.EndPrincipalAmount,
                             AmortisedCost = (double)a.AmortisedCost,
                             AccruedInterest = (double)a.AccruedInterest,
                             NorminalInterestRate = a.InterestRate,

                             AmOpeningBalance = (double)a.AmortisedOpeningBalance,
                             AmStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                             AmDailyPaymentAmount = (double)a.AmortisedDailyPaymentAmount,
                             AmDailyInterestAmount = (double)a.AmortisedDailyInterestAmount,
                             AmDailyPrincipalAmount = (double)a.AmortisedDailyPrincipalAmount,
                             AmClosingBalance = (double)a.AmortisedClosingBalance,
                             AmEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                             AmAccruedInterest = (double)a.AmortisedAccruedInterest,
                             AmAmortisedCost = (double)a.Amortised_AmortisedCost,
                             UnEarnedFee = (double)a.UnearnedFee,
                             EarnedFee = (double)a.EarnedFee,
                             EffectiveInterestRate = a.EffectiveInterestRate,
                             CreatedBy = a.CreatedBy,
                             CreatedOn = a.CreatedOn,

                         }).ToList();

            List<credit_loanscheduledailyarchive> loanScheduleDailyArchive = new List<credit_loanscheduledailyarchive>();



            foreach (var item in model)
            {
                credit_loanscheduledailyarchive addLoanScheduleDailyArchive = new credit_loanscheduledailyarchive();


                addLoanScheduleDailyArchive.LoanId = loanId;
                addLoanScheduleDailyArchive.PaymentNumber = item.PaymentNumber;
                addLoanScheduleDailyArchive.Date = item.Date;
                addLoanScheduleDailyArchive.PaymentDate = item.PaymentDate;
                addLoanScheduleDailyArchive.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                addLoanScheduleDailyArchive.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                addLoanScheduleDailyArchive.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                addLoanScheduleDailyArchive.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                addLoanScheduleDailyArchive.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                addLoanScheduleDailyArchive.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                addLoanScheduleDailyArchive.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                addLoanScheduleDailyArchive.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                addLoanScheduleDailyArchive.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                addLoanScheduleDailyArchive.InterestRate = item.NorminalInterestRate;

                addLoanScheduleDailyArchive.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                addLoanScheduleDailyArchive.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                addLoanScheduleDailyArchive.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                addLoanScheduleDailyArchive.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                addLoanScheduleDailyArchive.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                addLoanScheduleDailyArchive.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                addLoanScheduleDailyArchive.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                addLoanScheduleDailyArchive.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                addLoanScheduleDailyArchive.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                addLoanScheduleDailyArchive.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                addLoanScheduleDailyArchive.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                addLoanScheduleDailyArchive.EffectiveInterestRate = item.EffectiveInterestRate;
                addLoanScheduleDailyArchive.CreatedBy = item.CreatedBy;
                addLoanScheduleDailyArchive.CreatedOn = item.CreatedOn;
                //addLoanScheduleDailyArchive.ArchiveDate = generalSetup.GetApplicationDate();
                addLoanScheduleDailyArchive.ArchiveBatchCode = batchCode;

                loanScheduleDailyArchive.Add(addLoanScheduleDailyArchive);

            }

            this._context.credit_loanscheduledailyarchive.AddRange(loanScheduleDailyArchive);

            _context.SaveChanges();
            return model;
        }

        private IEnumerable<LoanPaymentSchedulePeriodicObj> MergePeriodicSchedule(int loanId, DateTime applicationDate)
        {
            int no = 0;//number.Count() - 1;
            var DbF = Microsoft.EntityFrameworkCore.EF.Functions;

            var model = (from a in _context.credit_loanscheduleperiodicarchive
                         where a.LoanId == loanId && a.PaymentDate < applicationDate.Date
                         orderby a.PaymentNumber ascending
                         select new LoanPaymentSchedulePeriodicObj()
                         {
                             LoanId = a.LoanId,
                             PaymentNumber = a.PaymentNumber,
                             PaymentDate = a.PaymentDate,
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
                             CreatedBy = a.CreatedBy,
                             CreatedOn = a.CreatedOn,
                         }).Concat
                         (from a in _context.credit_temp_loanscheduleperiodic
                          where a.LoanId == loanId && a.PaymentDate >= applicationDate.Date
                          && a.PaymentNumber != 0
                          orderby a.PaymentNumber ascending
                          select new LoanPaymentSchedulePeriodicObj()
                          {
                              LoanId = a.LoanId,
                              PaymentNumber = a.PaymentNumber,
                              PaymentDate = a.PaymentDate,
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
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn,
                          }).ToList();

            List<credit_loanscheduleperiodic> loanSchedulePeriodic = new List<credit_loanscheduleperiodic>();



            foreach (var item in model)
            {
                credit_loanscheduleperiodic addLoanSchedulePeriodic = new credit_loanscheduleperiodic();


                addLoanSchedulePeriodic.LoanId = item.LoanId;
                addLoanSchedulePeriodic.PaymentNumber = ++no - 1;//item.paymentNumber;
                addLoanSchedulePeriodic.PaymentDate = item.PaymentDate;
                addLoanSchedulePeriodic.StartPrincipalAmount = (decimal)item.StartPrincipalAmount;
                addLoanSchedulePeriodic.PeriodPaymentAmount = (decimal)item.PeriodPaymentAmount;
                addLoanSchedulePeriodic.PeriodInterestAmount = (decimal)item.PeriodInterestAmount;
                addLoanSchedulePeriodic.PeriodPrincipalAmount = (decimal)item.PeriodPrincipalAmount;
                addLoanSchedulePeriodic.EndPrincipalAmount = (decimal)item.EndPrincipalAmount;

                addLoanSchedulePeriodic.InterestRate = item.InterestRate;
                addLoanSchedulePeriodic.AmortisedStartPrincipalAmount = (decimal)item.AmortisedStartPrincipalAmount;
                addLoanSchedulePeriodic.AmortisedPeriodPaymentAmount = (decimal)item.AmortisedPeriodPaymentAmount;
                addLoanSchedulePeriodic.AmortisedPeriodInterestAmount = (decimal)item.AmortisedPeriodInterestAmount;
                addLoanSchedulePeriodic.AmortisedPeriodPrincipalAmount = (decimal)item.AmortisedPeriodPrincipalAmount;
                addLoanSchedulePeriodic.AmortisedEndPrincipalAmount = (decimal)item.AmortisedEndPrincipalAmount;
                addLoanSchedulePeriodic.EffectiveInterestRate = item.EffectiveInterestRate;
                addLoanSchedulePeriodic.CreatedBy = item.CreatedBy;
                addLoanSchedulePeriodic.CreatedOn = item.CreatedOn;



                loanSchedulePeriodic.Add(addLoanSchedulePeriodic);

            }


            var itemToRemove = (from p in _context.credit_loanscheduleperiodic
                                where p.LoanId == loanId
                                select p);

            if (itemToRemove != null)
            {
                _context.credit_loanscheduleperiodic.RemoveRange(itemToRemove);
                _context.SaveChanges();
            }

            this._context.credit_loanscheduleperiodic.AddRange(loanSchedulePeriodic);

            _context.SaveChanges();
            return model;
        }

        private IEnumerable<LoanPaymentScheduleDailyObj> MergeDailySchedule(int loanId, DateTime applicationDate)
        {
            int no = 0;//number.Count() - 1;

            var model = (from a in _context.credit_loanscheduledailyarchive
                         where a.LoanId == loanId && a.Date < applicationDate.Date
                         orderby a.PaymentNumber ascending
                         select new LoanPaymentScheduleDailyObj()
                         {
                             LoanId = a.LoanId,
                             PaymentNumber = a.PaymentNumber,
                             Date = a.Date,
                             PaymentDate = a.PaymentDate,
                             OpeningBalance = (double)a.OpeningBalance,
                             StartPrincipalAmount = (double)a.StartPrincipalAmount,
                             DailyPaymentAmount = (double)a.DailyPaymentAmount,
                             DailyInterestAmount = (double)a.DailyInterestAmount,
                             DailyPrincipalAmount = (double)a.DailyPrincipalAmount,
                             ClosingBalance = (double)a.ClosingBalance,
                             EndPrincipalAmount = (double)a.EndPrincipalAmount,
                             AmortisedCost = (double)a.AmortisedCost,
                             AccruedInterest = (double)a.AccruedInterest,
                             NorminalInterestRate = a.InterestRate,

                             AmOpeningBalance = (double)a.AmortisedOpeningBalance,
                             AmStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                             AmDailyPaymentAmount = (double)a.AmortisedDailyPaymentAmount,
                             AmDailyInterestAmount = (double)a.AmortisedDailyInterestAmount,
                             AmDailyPrincipalAmount = (double)a.AmortisedDailyPrincipalAmount,
                             AmClosingBalance = (double)a.AmortisedClosingBalance,
                             AmEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                             AmAccruedInterest = (double)a.AmortisedAccruedInterest,
                             AmAmortisedCost = (double)a.Amortised_AmortisedCost,
                             UnEarnedFee = (double)a.UnearnedFee,
                             EarnedFee = (double)a.EarnedFee,
                             EffectiveInterestRate = a.EffectiveInterestRate,
                             CreatedBy = a.CreatedBy,
                             CreatedOn = a.CreatedOn,
                         }).Concat
                         (from a in _context.credit_temp_loanscheduledaily
                          where a.LoanId == loanId && a.Date >= applicationDate.Date
                          && a.PaymentNumber != 0
                          orderby a.PaymentNumber ascending
                          select new LoanPaymentScheduleDailyObj()
                          {
                              LoanId = a.LoanId,
                              PaymentNumber = a.PaymentNumber,
                              Date = a.Date,
                              PaymentDate = a.PaymentDate,
                              OpeningBalance = (double)a.OpeningBalance,
                              StartPrincipalAmount = (double)a.StartPrincipalAmount,
                              DailyPaymentAmount = (double)a.DailyPaymentAmount,
                              DailyInterestAmount = (double)a.DailyInterestAmount,
                              DailyPrincipalAmount = (double)a.DailyPrincipalAmount,
                              ClosingBalance = (double)a.ClosingBalance,
                              EndPrincipalAmount = (double)a.EndPrincipalAmount,
                              AmortisedCost = (double)a.AmortisedCost,
                              AccruedInterest = (double)a.AccruedInterest,
                              NorminalInterestRate = a.InterestRate,

                              AmOpeningBalance = (double)a.AmortisedOpeningBalance,
                              AmStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                              AmDailyPaymentAmount = (double)a.AmortisedDailyPaymentAmount,
                              AmDailyInterestAmount = (double)a.AmortisedDailyInterestAmount,
                              AmDailyPrincipalAmount = (double)a.AmortisedDailyPrincipalAmount,
                              AmClosingBalance = (double)a.AmortisedClosingBalance,
                              AmEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                              AmAccruedInterest = (double)a.AmortisedAccruedInterest,
                              AmAmortisedCost = (double)a.Amortised_AmortisedCost,
                              UnEarnedFee = (double)a.UnearnedFee,
                              EarnedFee = (double)a.EarnedFee,
                              EffectiveInterestRate = a.EffectiveInterestRate,
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn,
                          }).ToList();

            // List<tbl_Loan_Schedule_Periodic> loanSchedulePeriodic = new List<tbl_Loan_Schedule_Periodic>();
            List<credit_loanscheduledaily> loanScheduleDaily = new List<credit_loanscheduledaily>();


            foreach (var item in model)
            {
                credit_loanscheduledaily addLoanScheduleDaily = new credit_loanscheduledaily();

                addLoanScheduleDaily.LoanId = loanId;
                addLoanScheduleDaily.PaymentNumber = ++no - 1;
                addLoanScheduleDaily.Date = item.Date;
                addLoanScheduleDaily.PaymentDate = item.PaymentDate;
                addLoanScheduleDaily.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                addLoanScheduleDaily.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                addLoanScheduleDaily.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                addLoanScheduleDaily.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                addLoanScheduleDaily.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                addLoanScheduleDaily.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                addLoanScheduleDaily.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                addLoanScheduleDaily.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                addLoanScheduleDaily.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                addLoanScheduleDaily.InterestRate = item.NorminalInterestRate;
                addLoanScheduleDaily.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                addLoanScheduleDaily.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                addLoanScheduleDaily.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                addLoanScheduleDaily.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                addLoanScheduleDaily.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                addLoanScheduleDaily.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                addLoanScheduleDaily.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                addLoanScheduleDaily.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                addLoanScheduleDaily.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                addLoanScheduleDaily.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                addLoanScheduleDaily.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                addLoanScheduleDaily.EffectiveInterestRate = item.EffectiveInterestRate;
                addLoanScheduleDaily.CreatedBy = item.CreatedBy;
                addLoanScheduleDaily.CreatedOn = item.CreatedOn;

                loanScheduleDaily.Add(addLoanScheduleDaily);

            }


            var itemToRemove = (from p in _context.credit_loanscheduledaily
                                where p.LoanId == loanId
                                select p);


            if (itemToRemove != null)
            {
                _context.credit_loanscheduledaily.RemoveRange(itemToRemove);
                _context.SaveChanges();
            }

            this._context.credit_loanscheduledaily.AddRange(loanScheduleDaily);

            _context.SaveChanges();
            return model;
        }

        private void updateloanTableStatus(int loanId)
        {
            credit_loan result = (from p in _context.credit_loan
                                  where p.LoanId == loanId
                                  select p).SingleOrDefault();

            result.LoanStatusId = (short)LoanStatusEnum.Completed;



            _context.SaveChanges();
        }

        private void updateloanTablePrincipal(int loanId, decimal amoumt)
        {
            credit_loan result = (from p in _context.credit_loan
                                  where p.LoanId == loanId
                                  select p).SingleOrDefault();

            result.OutstandingPrincipal = result.OutstandingPrincipal - amoumt;
            result.OutstandingAmortisedPrincipal = result.OutstandingAmortisedPrincipal - amoumt;


            _context.SaveChanges();
        }

        private void updateloanTableInterest(int loanId, decimal amoumt)
        {
            credit_loan result = (from p in _context.credit_loan
                                  where p.LoanId == loanId
                                  select p).SingleOrDefault();

            result.OutstandingInterest = result.OutstandingInterest - amoumt;


            _context.SaveChanges();
        }

        private void updateloanTablePastDuePrincipal(int loanId, decimal amoumt)
        {
            credit_loan result = (from p in _context.credit_loan
                                  where p.LoanId == loanId
                                  select p).SingleOrDefault();

            result.PastDuePrincipal = result.PastDuePrincipal + amoumt;

            _context.SaveChanges();
        }

        private void updateloanTablePastDueInterest(int loanId, decimal amoumt)
        {
            credit_loan result = (from p in _context.credit_loan
                                  where p.LoanId == loanId
                                  select p).SingleOrDefault();

            result.PastDueInterest = result.PastDueInterest + amoumt;

            _context.SaveChanges();
        }

    }
}
