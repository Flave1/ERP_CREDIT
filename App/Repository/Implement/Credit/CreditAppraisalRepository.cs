using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.DomainObjects.Finance;
using Banking.Helpers;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Repository.Implement.Credit
{
    public class CreditAppraisalRepository : ICreditAppraisalRepository
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly ILoanScheduleRepository _schedule;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IHttpContextAccessor _accessor;
        private ApprovalRegRespObj response = new ApprovalRegRespObj();
        public CreditAppraisalRepository(DataContext context, IIdentityService identityService, ILoanScheduleRepository schedule, IIdentityServerRequest serverRequest, IHttpContextAccessor accessor)
        {
            _dataContext = context;
            _identityService = identityService;
            _schedule = schedule;
            _serverRequest = serverRequest;
            _accessor = accessor;
        }

        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationForAppraisalAsync()
        {
            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return new LoanApplicationRespObj
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
                    return new LoanApplicationRespObj
                    {
                        Status = res.Status
                    };
                }

                if (res.workflowTasks.Count() < 1)
                {
                    return new LoanApplicationRespObj
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
                var loanApplication = await GetLoanApplicationAwaitingApprovalAsync(res.workflowTasks.Select(x => x.TargetId).ToList(), res.workflowTasks.Select(d => d.WorkflowToken).ToList());


                return new LoanApplicationRespObj
                {
                    LoanApplications = loanApplication.Select(a => new LoanApplicationObj
                    {
                        ApplicationDate = (DateTime)a.CreatedOn,
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
                        //CurrencyName = a.cor_currency.CurrencyName,
                        CustomerName = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == a.CustomerId).LastName,
                        ApprovedProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ApprovedProductId).ProductName,
                        ProposedProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProposedProductId).ProductName,
                        LoanApplicationStatusId = a.LoanApplicationStatusId,
                        CompanyId = a.CompanyId,
                        ApplicationRefNumber = a.ApplicationRefNumber,
                        CreditScore = a.Score,
                        ProbabilityOfDefault = a.PD,
                        WorkflowToken = a.WorkflowToken
                    }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = loanApplication.Count() < 1 ? "No Loan Application awaiting approvals" : null
                        }
                    }
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        private async Task<IEnumerable<credit_loanapplication>> GetLoanApplicationAwaitingApprovalAsync(List<int> LoanApplicationIds, List<string> tokens)
        {
            var item = await _dataContext.credit_loanapplication
                .Where(s => LoanApplicationIds.Contains(s.LoanApplicationId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }

        public IEnumerable<ApprovalDetailsObj> GetApprovalTrail(int targetId, string workflowToken)
        {
            var staffResponse = _serverRequest.GetAllStaffAsync().Result;
            var trail = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(workflowToken) && x.TargetId == targetId);

            var data = trail.Select(x => new ApprovalDetailsObj
            {
                ApprovalDetailId = x.ApprovalDetailId,
                Comment = x.Comment,
                TargetId = x.TargetId,
                StaffId = x.StaffId,
                WorkflowToken = x.WorkflowToken,
                Date = x.Date,              
                ArrivalDate = x.ArrivalDate,
            }).OrderByDescending(x => x.ApprovalDetailId).ToList();
            foreach(var item in data)
            {
                item.FirstName = staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).firstName;
                item.LastName = staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).lastName;
            }
            return data;
        }

        public IEnumerable<PreviousStaff> GetApprovalTrailStaff(int targetId, string workflowToken)
        {
            var staffResponse = _serverRequest.GetAllStaffAsync().Result;
            var trail = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(workflowToken) && x.TargetId == targetId);

            var data = trail.Select(x => new PreviousStaff
            {
                StaffId = x.StaffId
            }).ToList();
            foreach (var item in data)
            {
                item.Name = staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).firstName + " " + staffResponse.staff.FirstOrDefault(d => d.staffId == item.StaffId).lastName;
            }
            return data.GroupBy(x => x.StaffId).Select(x=>x.First()).ToArray();
        }

        public UserPrivilegeObj GetUserPrivilege(int operationId, string userName)
        {
            //    var user = _dataContext.cor_useraccount.Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            //    var staff = _dataContext.cor_staff.Find(user.StaffId);

            //    var grants = (from a in _dataContext.cor_workflowlevel
            //                  join b in _dataContext.cor_workflowgroup on a.WorkflowGroupId equals b.WorkflowGroupId
            //                  join c in _dataContext.cor_workflowlevelstaff on a.WorkflowLevelId equals c.WorkflowLevelId
            //                  where a.Deleted == false && a.WorkflowGroupId == b.WorkflowGroupId && c.StaffId == staff.StaffId
            //                  select new UserPrivilegeViewModel
            //                  {
            //                      canEdit = (bool)a.CanModify,
            //                  }).FirstOrDefault();

            //    return grants;
            throw new NotImplementedException();
        }

        //////PRIVATE METHOD
        ///
        public ApprovalRegRespObj LoanApplicationApproval(int targetId, int approvalStatusId)
        {
            ApprovalRegRespObj response = new ApprovalRegRespObj();
            var loanApplication = _dataContext.credit_loanapplication.Find(targetId);
            loanApplication.ApprovalStatusId = approvalStatusId;
            loanApplication.LoanApplicationStatusId = (int)ApplicationStatus.OfferLetter;
            response.response = _dataContext.SaveChanges() > 0;
            var ttt = AddTempLoanApplicationSchedule(targetId);
            if (ttt.loanPayment != null && ttt.AnyIdentifier > 0)
            {
                return ttt;
            }
            return response;
        }

        private ApprovalRegRespObj AddTempLoanApplicationSchedule(int loanApplicationId)
        {
            try
            {
                var loanApp = _dataContext.credit_loanapplication.Find(loanApplicationId);
                var product = _dataContext.credit_product.Find(loanApp.ApprovedProductId);
                var maturityDate = loanApp.EffectiveDate.AddDays(loanApp.ApprovedTenor);

                //var paymentDate = loanApp.FirstPrincipalDate.Value.AddDays(30);
                var paymentDate = loanApp.FirstPrincipalDate;

                LoanPaymentScheduleInputObj input = new LoanPaymentScheduleInputObj
                {
                    ScheduleMethodId = (short)product.ScheduleTypeId,
                    PrincipalAmount = (double)loanApp.ApprovedAmount,
                    EffectiveDate = loanApp.EffectiveDate,
                    InterestRate = loanApp.ApprovedRate,
                    PrincipalFrequency = product.FrequencyTypeId,
                    InterestFrequency = product.FrequencyTypeId,
                    PrincipalFirstpaymentDate = paymentDate ?? DateTime.Now,
                    InterestFirstpaymentDate = paymentDate ?? DateTime.Now,
                    MaturityDate = maturityDate,
                    AccurialBasis = 2,
                    IntegralFeeAmount = 0,
                    FirstDayType = 0,
                    CreatedBy = loanApp.CreatedBy,
                    IrregularPaymentSchedule = null
                };

                return new ApprovalRegRespObj { AnyIdentifier = loanApplicationId, loanPayment = input };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region CUSTOMER_TRANSACTION
        public ApprovalRegRespObj CustomerTransaction(CustomerTransactionObj item)
        {
            try
            {               
                var update = UpdateCustomerBalances(item.DebitAmount, item.CreditAmount, item.CasaAccountNumber);
                if (!update)
                {
                    return new ApprovalRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Account Balance Transaction could not update" } } };
                }
                var bal = _dataContext.credit_casa.Where(x => x.AccountNumber == item.CasaAccountNumber).FirstOrDefault().AvailableBalance;
                fin_customertransaction customerTrans = new fin_customertransaction
                {
                    TransactionCode = "TRANS-" + GeneralHelpers.GenerateRandomDigitCode(10),
                    AccountNumber = item.CasaAccountNumber,
                    Description = item.Description,
                    TransactionDate = item.TransactionDate,
                    ValueDate = item.ValueDate,
                    TransactionType = item.TransactionType,
                    Amount = item.CreditAmount == 0 ? item.DebitAmount : item.CreditAmount,
                    CreditAmount = item.CreditAmount,
                    DebitAmount = item.DebitAmount,
                    AvailableBalance = bal,
                    Beneficiary = item.Beneficiary,
                    BatchNo = item.ReferenceNo
                };
                _dataContext.fin_customertransaction.Add(customerTrans);
                var response = _dataContext.SaveChanges() > 0;
                return new ApprovalRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } } };
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        private bool UpdateCustomerBalances(decimal debitAmount, decimal creditAmount, string accountNumber)
        {
            try
            {
                var account = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == accountNumber);
                if (account == null)
                    return false;
                
                decimal bal = 0;
                decimal balance = 0;

                if (debitAmount > 0)
                {
                    account.LedgerBalance = account.LedgerBalance - debitAmount;
                    account.AvailableBalance = account.AvailableBalance - debitAmount;
                    bal = account.AvailableBalance;
                }
                else
                {
                    account.LedgerBalance = account.LedgerBalance + creditAmount;
                    account.AvailableBalance = account.AvailableBalance + creditAmount;
                    bal = account.AvailableBalance;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApprovalRegRespObj CustomerTransactionEoD(CustomerTransactionObj item)
        {
            try
            {
                var update = UpdateCustomerBalanceForEoD(item.DebitAmount, item.CreditAmount, item.CasaAccountNumber, item.ReferenceNo, item.ValueDate.Value.Date);
                if (update == 0)
                {
                    return new ApprovalRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Account Balance Transaction could not update" } } };
                }
                var bal = _dataContext.credit_casa.Where(x => x.AccountNumber == item.CasaAccountNumber).FirstOrDefault().AvailableBalance;
                fin_customertransaction customerTrans = new fin_customertransaction
                {
                    TransactionCode = "TRANS-" + GeneralHelpers.GenerateRandomDigitCode(10),
                    AccountNumber = item.CasaAccountNumber,
                    Description = item.Description,
                    TransactionDate = item.TransactionDate,
                    ValueDate = item.ValueDate,
                    TransactionType = item.TransactionType,
                    Amount = item.CreditAmount == 0 ? item.DebitAmount : item.CreditAmount,
                    CreditAmount = item.CreditAmount,
                    DebitAmount = item.DebitAmount,
                    AvailableBalance = bal,
                    Beneficiary = item.Beneficiary,
                    BatchNo = item.ReferenceNo
                };
                _dataContext.fin_customertransaction.Add(customerTrans);
                var response = _dataContext.SaveChanges() > 0;
                return new ApprovalRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } } };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private decimal UpdateCustomerBalanceForEoD(decimal debitAmount, decimal creditAmount, string accountNumber, string loanRefNumber, DateTime valueDate)
        {
            var account = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == accountNumber);
            var loan = _dataContext.credit_loan.FirstOrDefault(x => x.LoanRefNumber == loanRefNumber);
            if (account == null)
            {
                throw new Exception("Account Balance does not exist on CASA");
            }
            decimal bal = 0;
            decimal balance = 0;

            if (debitAmount > 0)
            {
                account.LedgerBalance = account.LedgerBalance - debitAmount;
                account.AvailableBalance = account.AvailableBalance - debitAmount;
                bal = account.AvailableBalance;
                if (account.AvailableBalance < debitAmount)
                {
                    if (loan != null)
                    {
                        var loanSchedule = _dataContext.credit_loanscheduleperiodic.Where(x => x.LoanId == loan.LoanId && x.PaymentDate.Date == valueDate.Date).FirstOrDefault();
                        if (loanSchedule != null)
                        {
                            if (loanSchedule.PeriodPrincipalAmount == debitAmount)
                            {
                                loanSchedule.PaymentPending = debitAmount;
                            }
                            else if (loanSchedule.PeriodInterestAmount == debitAmount)
                            {
                                loanSchedule.PaymentPendingInterest = debitAmount;
                            }
                        }
                    }
                }
                else
                {
                    if (loan != null)
                    {
                        var loanSchedule = _dataContext.credit_loanscheduleperiodic.Where(x => x.LoanId == loan.LoanId && x.PaymentDate.Date == valueDate.Date).FirstOrDefault();
                        if (loanSchedule != null)
                        {
                            if (loanSchedule.PeriodPrincipalAmount == debitAmount)
                            {
                                loanSchedule.ActualRepayment = debitAmount;
                            }
                            else if (loanSchedule.PeriodInterestAmount == debitAmount)
                            {
                                loanSchedule.ActualRepaymentInterest = debitAmount;
                            }
                        }

                    }
                }
            }
            else
            {
                account.LedgerBalance = account.LedgerBalance + creditAmount;
                account.AvailableBalance = account.AvailableBalance + creditAmount;
                bal = account.AvailableBalance;
            }
            var result = _dataContext.SaveChanges() > 0;
            if (result)
            {
                return bal;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<CustomerTransactionObj> GetAllCustomerTransactionBySearch(string acctNumber, DateTime? date1, DateTime? date2)
        {
            IEnumerable<CustomerTransactionObj> data = null;
            if (!string.IsNullOrWhiteSpace(acctNumber.Trim()))
            {
                data = (from a in _dataContext.fin_customertransaction
                        join b in _dataContext.deposit_accountopening on a.AccountNumber equals b.AccountNumber
                        where a.AccountNumber.ToLower() == (acctNumber.ToLower())
                        select new CustomerTransactionObj
                        {
                            TransactionCode = a.TransactionCode,
                            CasaAccountNumber = a.AccountNumber,
                            FirstName = b.Firstname,
                            SecondName = b.Surname,
                            Description = a.Description,
                            TransactionDate = a.TransactionDate,
                            ValueDate = a.ValueDate,
                            Amount = a.DebitAmount == 0 ? a.CreditAmount : a.DebitAmount,
                            AvailableBalance = a.AvailableBalance,
                            TransactionType = a.TransactionType,
                            Beneficiary = a.Beneficiary,
                            CustomerTransactionId = a.CustomerTransactionId
                        }).AsQueryable();
            }
            if (date1 != null && date2 != null)
            {
                data = data.Where(d => d.TransactionDate.Value.Date >= date1 && d.TransactionDate.Value.Date <= date2);
            }
            var customerTrans = data.ToList();

            return customerTrans;
        }


        public byte[] GenerateExportCustomerTransaction(string acctNumber, DateTime? date1, DateTime? date2)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Account Name");
            dt.Columns.Add("Account Number");
            dt.Columns.Add("Transaction Number");
            dt.Columns.Add("Transaction Date");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Transaction Type");
            dt.Columns.Add("Available Balance");
            dt.Columns.Add("Beneficiary");
            IEnumerable<CustomerTransactionObj> data = null;
            if (!string.IsNullOrWhiteSpace(acctNumber.Trim()))
            {
                data = (from a in _dataContext.fin_customertransaction
                        join b in _dataContext.deposit_accountopening on a.AccountNumber equals b.AccountNumber
                        where a.AccountNumber.ToLower() == (acctNumber.ToLower())
                        select new CustomerTransactionObj
                        {
                            TransactionCode = a.TransactionCode,
                            CasaAccountNumber = a.AccountNumber,
                            FirstName = b.Firstname,
                            SecondName = b.Surname,
                            Description = a.Description,
                            TransactionDate = a.TransactionDate,
                            ValueDate = a.ValueDate,
                            Amount = a.DebitAmount == 0 ? a.CreditAmount : a.DebitAmount,
                            AvailableBalance = a.AvailableBalance,
                            TransactionType = a.TransactionType,
                            Beneficiary = a.Beneficiary,
                            CustomerTransactionId = a.CustomerTransactionId
                        }).AsQueryable();
            }
            if (date1 != null && date2 != null)
            {
                data = data.Where(d => d.TransactionDate >= date1 && d.TransactionDate <= date2);
            }
            var customerTrans = data.ToList();
            foreach (var kk in customerTrans)
            {
                var row = dt.NewRow();
                row["Account Name"] = kk.FirstName + " " + kk.SecondName;
                row["Account Number"] = kk.CasaAccountNumber;
                row["Transaction Number"] = kk.TransactionCode;
                row["Transaction Date"] = kk.TransactionDate;
                row["Amount"] = kk.Amount;
                row["Transaction Type"] = kk.TransactionType;
                row["Available Balance"] = kk.AvailableBalance;
                row["Beneficiary"] = kk.Beneficiary;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (customerTrans != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("customerTrans");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }


        public CustomerTransactionObj GetCustomerTransaction(int TransactionId)
        {
            try
            {
                var Transaction = (from a in _dataContext.fin_customertransaction
                                   where a.CustomerTransactionId == TransactionId
                                   select new CustomerTransactionObj
                                   {
                                       TransactionCode = a.TransactionCode,
                                       Description = a.Description,
                                       TransactionDate = a.TransactionDate,
                                       ValueDate = a.ValueDate,
                                       Amount = a.Amount,
                                       TransactionType = a.TransactionType,
                                       Beneficiary = a.Beneficiary,

                                   }).FirstOrDefault();
                return Transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
