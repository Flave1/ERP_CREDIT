using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.V1;
using Banking.Data;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Repository.Interface.InvestorFund;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.LoanObjs;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanOperationsController : Controller
    {
        private readonly ILoanOperationsRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly ILoanScheduleRepository _schedule;
        private readonly IIdentityServerRequest _identityServer;
        private readonly IInvestorFundService _invest;



        public LoanOperationsController(ILoanOperationsRepository repo, IIdentityService identityService, ILoggerService logger, IInvestorFundService invest, IHttpContextAccessor httpContextAccessor, DataContext context, ILoanScheduleRepository schedule, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _schedule = schedule;
            _identityServer = identityServer;
            _invest = invest;
        }

        #region LOAN_OPERATION

        [HttpPost(ApiRoutes.LoanOperation.ADD_LOAN_OPERATION_APPROVAL)]
        public async Task<ActionResult<LoanRespObj>> AddLoanBooking([FromBody] LoanReviewOperationObj model)
        {
            try
            {
                using (var _trans = _context.Database.BeginTransaction())
                {
                    var identity = await _identityServer.UserDataAsync();
                    var user = identity.UserName;

                    model.CreatedBy = user;
                    model.UpdatedBy = user;

                    var appicationResponse = _repo.AddOperationReview(model);

                    if (appicationResponse.loanPayment != null && appicationResponse.AnyIdentifier > 0)
                    {
                        await _schedule.AddLoanSchedule(appicationResponse.AnyIdentifier, appicationResponse.loanPayment);
                    }
                    await _context.SaveChangesAsync();
                    _trans.Commit();
                    return new LoanRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful =  true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    };
                }
               
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

        [HttpGet(ApiRoutes.LoanOperation.GET_APPROVED_LOAN_REVIEW)]
        public async Task<ActionResult<LoanRespObj>> GetApprovedLoanReview()
        {
            try
            {
                var response = _repo.GetApprovedLoanReview();
                return new LoanRespObj
                {
                    Loans = response,
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

        [HttpGet(ApiRoutes.LoanOperation.GET_APPROVED_LOAN_OPERATION_REVIEW)]
        public async Task<ActionResult<LoanRespObj>> GetApprovedLoanOperationReview()
        {
            try
            {
                var response = _repo.GetApprovedLoanOperationReview();
                return new LoanRespObj
                {
                    LoanReviewOperations = response,
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

        [HttpGet(ApiRoutes.LoanOperation.GET_APPROVED_LOAN_REVIEW_REMEDIAL)]
        public async Task<ActionResult<LoanRespObj>> GetApprovedLoanReviewRemedial()
        {
            try
            {
                var response = _repo.GetApprovedLoanReviewRemedial();
                return new LoanRespObj
                {
                    Loans = response,
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

        [HttpGet(ApiRoutes.LoanOperation.GET_OPERATION_TYPE_REMEDIAL)]
        public async Task<ActionResult<LoanRespObj>> GetRemedialOperationType()
        {
            try
            {
                var response = _repo.GetRemedialOperationType();
                return new LoanRespObj
                {
                    LoanOperationType = response,
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

        [HttpGet(ApiRoutes.LoanOperation.GET_OPERATION_TYPE)]
        public async Task<ActionResult<LoanRespObj>> GetOperationType()
        {
            try
            {
                var response = await _repo.GetOperationType();
                return new LoanRespObj
                {
                    LoanOperationType = response,
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


        [HttpGet(ApiRoutes.LoanOperation.GET_OPERATION_TYPE_RESTRUCTURE)]
        public async Task<ActionResult<LoanRespObj>> GetOperationTypeRestructure()
        {
            try
            {
                var response = await _repo.GetOperationTypeRestructure();
                return new LoanRespObj
                {
                    LoanOperationType = response,
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

        [HttpGet(ApiRoutes.LoanOperation.GET_SCHEDULE_BY_LOAN_ID)]
        public async Task<ActionResult<LoanScheduleRespObj>> GetLoanScheduleByLoanId([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanScheduleByLoanId(model.LoanId);
                return new LoanScheduleRespObj
                {
                    LoanPaymentSchedule = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanScheduleRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanOperation.GET_RUNNING_lOAN_BY_REFNO)]
        public async Task<ActionResult<LoanRespObj>> GetRunningLoans([FromQuery] LoanSearchObj model)
        {
            try
            {
                var response = _repo.GetRunningLoans(1, model.LoanRefNumber);
                var respList = new List<LoanObj> { response };
                return new LoanRespObj
                {
                    Loans = respList,
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

        #endregion

        #region EOD
        [AllowAnonymous]
        [HttpPost(ApiRoutes.LoanOperation.RUN_END_OF_DAY)]
        public async Task<ActionResult<LoanRespObj>> EndOfDayOperation([FromBody] EndOfDayRequestObj model)
        {
            try
            {
                    var now = DateTime.Now;
                    var res1 = _repo.ProcessDailyLoansInterestAccrual(model.RequestDate);
                    var res2 = _repo.ProcessLoanRepaymentPostingPastDue(model.RequestDate);
                    var res3 = _repo.ProcessDailyPastDueInterestAccrual(model.RequestDate);
                    var res4 = _repo.ProcessDailyPastDuePrincipalAccrual(model.RequestDate);
                    var res5 = _invest.ProcessMaturedInvestmentPosting(model.RequestDate);
                    var res6 = _invest.ProcessDailyInvestmentInterestAccrual(model.RequestDate);
                    _invest.ProcessRollOverPosting(model.RequestDate);
                    _repo.updateHistoricalLoanBalance();

                    await _context.SaveChangesAsync();
                    return Ok(new LoanRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });

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
        #endregion
    }
}
