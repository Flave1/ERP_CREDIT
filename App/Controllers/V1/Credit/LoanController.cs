using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Mail;
using Banking.Contracts.V1;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanController : Controller
    {
        private readonly ILoanRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly ILoanScheduleRepository _schedule;
        private readonly IMapper _mapper;
        ICreditAppraisalRepository _customerTrans;
        IIFRSRepository _ifrs;
        private readonly IFlutterWaveRequest _flutter;
        public LoanController(ILoanRepository repo, IIdentityService identityService, ILoggerService logger, IHttpContextAccessor httpContextAccessor, DataContext context, ICreditAppraisalRepository customerTrans,
        IIdentityServerRequest serverRequest, ILoanScheduleRepository schedule, IMapper mapper, IIFRSRepository ifrs, IFlutterWaveRequest flutter)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = context;
            _serverRequest = serverRequest;
            _schedule = schedule;
            _mapper = mapper;
            _customerTrans = customerTrans;
            _ifrs = ifrs;
            _flutter = flutter;
        }

        #region LOAN
        [HttpPost(ApiRoutes.Loan.ADD_LOAN_BOOKING)]
        public async Task<ActionResult<LoanRespObj>> AddLoanBooking([FromBody] LoanObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                model.CompanyId = identity.CompanyId;

                var response = await _repo.AddLoanBooking(model);
                return response;

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_REVIEWED_LOANS)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetAllLoanApplicationOfferLetterReviewed()
        {
            try
            {
                var response = _repo.GetAllLoanApplicationOfferLetterReviewed();
                return new LoanApplicationRespObj
                {
                    LoanApplications = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_REVIEWED_LOANS_ID)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetAllLoanApplicationOfferLetterReviewedById([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetAllLoanApplicationOfferLetterReviewedById(model.LoanApplicationId);
                return new LoanApplicationRespObj
                {
                    LoanApplications = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_ALL_LOANS)]
        public async Task<ActionResult<CreditLoanRespObj>> GetAllCreditLoan()
        {
            try
            {
                var response = _repo.GetAllCreditLoan();
                return Ok(new CreditLoanRespObj
                {
                    Loans = _mapper.Map<List<credit_loan_obj>>(response),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage { FriendlyMessage = response.Count() > 0 ? "Search record found" : "No record found" }
                    },
                 });
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditLoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_DETAILED_INFORMATION)]
        public async Task<ActionResult<LoanRespObj>> GetLoanDetailInformation([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = await _repo.GetLoanDetailInformation(model.LoanId);
                return new LoanRespObj
                {
                    LoanDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_SCHEDULE_INPUT)]
        public async Task<ActionResult<LoanRespObj>> GetScheduleInput([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetScheduleInput(model.LoanApplicationId);
                return new LoanRespObj
                {
                    LoanSchedule = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_PAST_DUE_INFORMATION)]
        public async Task<ActionResult<LoanRespObj>> GetPastDueLoanInformation()
        {
            try
            {
                var response = _repo.GetPastDueLoanInformation();
                return new LoanRespObj
                {
                   ManageLoans = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_PAYMENT_DUE_INFORMATION)]
        public async Task<ActionResult<LoanRespObj>> GetPaymentDueLoanInformation()
        {
            try
            {
                var response = _repo.GetPaymentDueLoanInformation();
                return new LoanRespObj
                {
                    ManageLoans = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_MANAGED_INFORMATION)]
        public async Task<ActionResult<LoanRespObj>> GetManagedLoanInformation([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetManagedLoanInformation(model.LoanId);

                return new LoanRespObj
                {
                    ManageLoans = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_BOOKING_APPROVAL_LIST)]
        public async Task<ActionResult<LoanRespObj>> GetLoanApplicationForAppraisalAsync()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                return await _repo.GetLoanBookingAwaitingApproval(user.UserName);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.UPDATE_LOAN_REPAYMENT_BY_FLUTTERWAVE)]
        public async Task<LoanRepaymentRespObj> RepaymentWithFlutterWave([FromBody]repaymentObj model)
        {
            try
            {
                return _repo.repaymentWithFlutterWave(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRepaymentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.UPDATE_LOAN_REPAYMENT_BY_FLUTTERWAVE_ZERO)]
        public async Task<LoanRepaymentRespObj> repaymentZeroWithFlutterWave([FromBody]repaymentObj model)
        {
            try
            {
                return _repo.repaymentZeroWithFlutterWave(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRepaymentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.LOAN_BOOKING_APPROVAL)]
        public async Task<ActionResult<ApprovalRegRespObj>> GoForApproval([FromBody] ApprovalObj entity)
        {
            using (var _trans = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                    var user = await _serverRequest.UserDataAsync();

                    var loan = _dataContext.credit_loan.Find(entity.TargetId);

                    var req = new IndentityServerApprovalCommand
                    {
                        ApprovalComment = entity.Comment,
                        ApprovalStatus = entity.ApprovalStatusId,
                        TargetId = entity.TargetId,
                        WorkflowToken = loan.WorkflowToken,
                        ReferredStaffId = entity.ReferredStaffId
                    };

                    var previousDetails = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(loan.WorkflowToken) && x.TargetId == entity.TargetId).ToList();
                    var lastDate = loan.CreatedOn;
                    if (previousDetails.Count() > 0)
                    {
                        lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                    }
                    if (previousDetails.Count() > 0)
                    {
                        lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                    }
                    var details = new cor_approvaldetail
                    {
                        Comment = entity.Comment,
                        Date = DateTime.Now,
                        ArrivalDate = previousDetails.Count() > 0 ? lastDate : loan.CreatedOn,
                        StatusId = entity.ApprovalStatusId,
                        TargetId = entity.TargetId,
                        StaffId = user.StaffId,
                        WorkflowToken = loan.WorkflowToken
                    };

                    var result = await _serverRequest.StaffApprovalRequestAsync(req);

                    if (!result.IsSuccessStatusCode)
                    {
                        return new ApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }

                    var stringData = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ApprovalRegRespObj>(stringData);

                    if (!response.Status.IsSuccessful)
                    {
                        return new ApprovalRegRespObj
                        {
                            Status = response.Status
                        };
                    }

                    if (response.ResponseId == (int)ApprovalStatus.Processing)
                    {
                        loan.ApprovalStatusId = (int)ApprovalStatus.Processing;
                        await _dataContext.cor_approvaldetail.AddAsync(details);
                        await _dataContext.SaveChangesAsync();
                        _trans.Commit();
                        return new ApprovalRegRespObj
                        {
                            ResponseId = (int)ApprovalStatus.Processing,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                        };
                    }


                    if (response.ResponseId == (int)ApprovalStatus.Revert)
                    {
                        loan.ApprovalStatusId = (int)ApprovalStatus.Revert;
                        await _dataContext.cor_approvaldetail.AddAsync(details);
                        await _dataContext.SaveChangesAsync();
                        _trans.Commit();
                        return new ApprovalRegRespObj
                        {
                            ResponseId = (int)ApprovalStatus.Revert,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                        };
                    }

                    if (response.ResponseId == (int)ApprovalStatus.Approved)
                    {
                        var applicationResponse = _repo.DisburseLoan(entity.TargetId, user.StaffId, user.UserName, entity.Comment);
                        var customer = _dataContext.credit_loancustomer.Find(loan.CustomerId);
                        await _serverRequest.SendMail(new MailObj
                        {
                            fromAddresses = new List<FromAddress> { },
                            toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                            subject = "Loan Successfully Approved",
                            content = $"Hi {customer.FirstName}, <br> Your loan application has been finally approved. <br/> Loan Amount : {loan.PrincipalAmount}",
                            sendIt = true,
                        });

                        //Update CustomerTransaction
                        if (applicationResponse.DisbursementEntry != null && applicationResponse.IntegralFeeEntry != null)
                        {
                            _customerTrans.CustomerTransaction(applicationResponse.DisbursementEntry);
                            _customerTrans.CustomerTransaction(applicationResponse.IntegralFeeEntry);
                        }

                        //Generate Schedule
                        if (applicationResponse.loanPayment != null && applicationResponse.AnyIdentifier > 0)
                        {
                            await _schedule.AddLoanSchedule(applicationResponse.AnyIdentifier, applicationResponse.loanPayment);
                        }

                        //Update IFRS
                        _ifrs.UpdateScoreCardHistoryByLoanDisbursement(entity.TargetId, user.UserId);

                        //Pay with Flutterwave
                        if (applicationResponse.FlutterObj != null)
                        {
                            var flutter = _serverRequest.GetFlutterWaveKeys().Result;
                            if (flutter.keys.useFlutterWave)
                            {
                                var res = _flutter.createBulkTransfer(applicationResponse.FlutterObj).Result;
                                loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
                                await _dataContext.cor_approvaldetail.AddAsync(details);
                                await _dataContext.SaveChangesAsync();
                                _trans.Commit();
                                if (res.status.ToLower().Trim() != "success")
                                {
                                    return new ApprovalRegRespObj
                                    {
                                        ResponseId = (int)ApprovalStatus.Revert,
                                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Loan disbursed successfully but transfer creation failed" } }
                                    };
                                }
                                else if (res.status.ToLower().Trim() == "success")
                                {
                                    return new ApprovalRegRespObj
                                    {
                                        ResponseId = (int)ApprovalStatus.Revert,
                                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Loan disbursed successfully and transfer creation successful" } }
                                    };
                                }
                            }
                        }
                        loan.ApprovalStatusId = (int)ApprovalStatus.Approved;
                        await _dataContext.cor_approvaldetail.AddAsync(details);
                        await _dataContext.SaveChangesAsync();
                        _trans.Commit();
                        return new ApprovalRegRespObj
                        {
                            ResponseId = (int)ApprovalStatus.Approved,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                        };
                    }

                    if (response.ResponseId == (int)ApprovalStatus.Disapproved)
                    {
                        loan.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
                        await _dataContext.cor_approvaldetail.AddAsync(details);
                        await _dataContext.SaveChangesAsync();
                        _trans.Commit();
                        return new ApprovalRegRespObj
                        {
                            ResponseId = (int)ApprovalStatus.Disapproved,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                        };
                    }

                    return new ApprovalRegRespObj
                    {
                        Status = response.Status
                    };
                }
                catch (Exception ex)
                {
                    _trans.Rollback();
                    var errorCode = ErrorID.Generate(5);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                    return new ApprovalRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                    };
                }
                finally { _trans.Dispose(); }
            }
        }

        #endregion

        #region LOAN COMMENT
        [HttpPost(ApiRoutes.Loan.ADD_LOAN_COMMENT)]
        public async Task<ActionResult<LoanRespObj>> UpdateCreditLoanComment([FromBody] CreditLoanCommentObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.UpdateCreditLoanComment(model);
                if (response)
                {
                    return new LoanRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_ALL_LOAN_COMMENT)]
        public async Task<ActionResult<LoanCommentRespObj>> GetAllCreditLoanComment([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetAllCreditLoanComment(model.LoanId, model.LoanScheduleId);
                return new LoanCommentRespObj
                {
                    LoanComments = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCommentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_COMMENT_ID)]
        public async Task<ActionResult<LoanCommentRespObj>> GetCreditLoanComment([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditLoanComment(model.LoanId, model.LoanScheduleId, model.LoanCommentId);
                var respList = new List<CreditLoanCommentObj> { response };
                return new LoanCommentRespObj
                {
                    LoanComments = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCommentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.DELETE_LOAN_COMMENT)]
        public ActionResult<LoanApplicationRespObj> DeleteCreditLoanComment([FromBody] DeleteLoanCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditLoanComment(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region LOAN DECISION
        [HttpPost(ApiRoutes.Loan.ADD_LOAN_DECISION)]
        public async Task<ActionResult<LoanRespObj>> UpdateCreditLoanDecision([FromBody] CreditLoanDecisionObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.UpdateCreditLoanDecision(model);
                if (response)
                {
                    return new LoanRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_ALL_LOAN_DECISION)]
        public async Task<ActionResult<LoanCommentRespObj>> GetAllCreditLoanDecision([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetAllCreditLoanDecision(model.LoanId, model.LoanScheduleId);
                return new LoanCommentRespObj
                {
                    LoanDecisions = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCommentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_LOAN_DECISION_ID)]
        public async Task<ActionResult<LoanCommentRespObj>> GetCreditLoanDecision([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditLoanDecision(model.LoanId, model.LoanScheduleId);
                var respList = new List<CreditLoanDecisionObj> { response };
                return new LoanCommentRespObj
                {
                    LoanDecisions = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCommentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.DELETE_LOAN_DECISION)]
        public ActionResult<LoanApplicationRespObj> DeleteCreditLoanDecision([FromBody] DeleteLoanCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditLoanDecision(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region LOAN CHEQUES

        [HttpPost(ApiRoutes.Loan.ADD_LOAN_CHEQUE)]
        public async Task<ActionResult<LoanChequeRespObj>> AddUpdateLoanCheque()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                //var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["generalUpload"].FileName;
                //var fileExtention = Path.GetExtension(fileName);
                var end = Convert.ToString(_httpContextAccessor.HttpContext.Request.Form["end"]);
                var loanChequeId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["loanChequeId"]);
                var loanId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["loanApplicationId"]);
                var start = Convert.ToString(_httpContextAccessor.HttpContext.Request.Form["start"]);

                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = ms.ToArray();
                        }
                    }
                }

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;

                var model = new loan_cheque_obj
                {
                    LoanChequeId = loanChequeId,
                    LoanId = loanId,
                    GeneralUpload = byteArray,
                    End = end,
                    Start = start,
                    CreatedBy = createdBy
                };

                var response = _repo.AddUpdateLoanCheque(model);

                    return new LoanChequeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? $"Successful" : "Unsuccessful" } }
                    };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.UPLOAD_LOAN_CHEQUE)]
        public async Task<ActionResult<LoanChequeRespObj>> UploadSingleCheque()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                var loanChequeListId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["loanChequeListId"]);
                var filename = (_httpContextAccessor.HttpContext.Request.Form.Files["singleUpload"]);

                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = ms.ToArray();
                        }
                    }
                }

                var model = new loan_cheque_obj
                {
                    LoanChequeListId = loanChequeListId,
                    SingleUpload = byteArray,
                    StatusName = filename.FileName,
                };
                return _repo.UploadSingleCheque(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.UPDATE_LOAN_CHEQUE_STATUS)]
        public async Task<ActionResult<LoanChequeRespObj>> UpdateChequeStatus([FromBody] loan_cheque_obj model)
        {
            try
            {
                return _repo.UpdateChequeStatus(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Loan.DOWNLOAD_LOAN_CHEQUE)]
        public async Task<ActionResult<LoanChequeRespObj>> DownloadCheque([FromBody] loan_cheque_obj model)
        {
            try
            {
                return _repo.DownloadCheque(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Loan.GET_ALL_LOAN_CHEQUE)]
        public async Task<ActionResult<LoanChequeRespObj>> GetAllLoanChequeList([FromQuery] LoanSearchObj model)
        {
            try
            {
                return _repo.GetAllLoanChequeList(model.LoanApplicationId);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanChequeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}



