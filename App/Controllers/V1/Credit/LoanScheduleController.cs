using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.V1;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanScheduleController : Controller
    {
        private readonly ILoanScheduleRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;

        public LoanScheduleController(ILoanScheduleRepository repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        #region LOAN_SCHEDULE
        [HttpGet(ApiRoutes.LoanSchedule.GET_ALL_FREQUENCYTYPE)]
        public async Task<ActionResult<LookupRespObj>> GetAllFrequencyTypes()
        {
            try
            {
                var response = _repo.GetAllFrequencyTypes();
                return new LookupRespObj
                {
                    LookUp = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LookupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanSchedule.GET_ALL_SCHEDULETYPE)]
        public async Task<ActionResult<LookupRespObj>> GetAllLoanScheduleType()
        {
            try
            {
                var response = _repo.GetAllLoanScheduleType();
                return new LookupRespObj
                {
                    LookUp = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LookupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanSchedule.GET_ALL_SCHEDULE_BY_LOANID)]
        public async Task<ActionResult<LoanScheduleRespObj>> GetPeriodicScheduleByLoaanId([FromQuery] LoanScheduleSearchObj search)
        {
            try
            {
                var response = _repo.GetPeriodicScheduleByLoaanId(search.LoanId);
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

        [HttpGet(ApiRoutes.LoanSchedule.GET_DELETED_LOAN_SCHEDULE)]
        public async Task<ActionResult<LoanScheduleRespObj>> GetPeriodicScheduleByLoaanIdDeleted([FromQuery] LoanScheduleSearchObj search)
        {
            try
            {
                var response = _repo.GetPeriodicScheduleByLoaanIdDeleted(search.LoanId);
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


        [HttpPost(ApiRoutes.LoanSchedule.GET_ALL_PERIODIC_SCHEDULE)]
        public async Task<ActionResult<LoanScheduleRespObj>> GeneratePeriodicLoanSchedule([FromBody] LoanPaymentScheduleInputObj model)
        {
            try
            {
                var response = _repo.GeneratePeriodicLoanSchedule(model);
                return new LoanScheduleRespObj
                 {
                        LoanPaymentSchedule = response,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful Generated" } }
                 };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanScheduleRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = ex.Message, TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanSchedule.DOWNLOAD_PERIODIC_SCHEDULE)]
        public async Task<ActionResult<LoanScheduleRespObj>> GeneratePeriodicLoanScheduleExport([FromBody] LoanPaymentScheduleInputObj model)
        {
            try
            {
                var response = _repo.GeneratePeriodicLoanScheduleExport(model);
                return new LoanScheduleRespObj
                {
                    export = response,
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

        [HttpPost(ApiRoutes.LoanSchedule.GET_ALL_TEMP_SCHEDULE)]
        public async Task<ActionResult<LoanScheduleRegRespObj>> AddTempLoanSchedule([FromQuery] LoanScheduleSearchObj search)
        {
            try
            {
                var res = await _repo.AddTempLoanSchedule(search.LoanId, search.loanInput);
                return new LoanScheduleRegRespObj
                {
                    response = res,
                    Status = new APIResponseStatus { IsSuccessful = res ? true : false, Message = new APIResponseMessage { FriendlyMessage = res ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanScheduleRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.LoanSchedule.GET_ALL_DAY_COUNT)]
        public async Task<ActionResult<LookupRespObj>> GetAllDayCount()
        {
            try
            {
                var response = _repo.GetAllDayCount();
                return new LookupRespObj
                {
                    LookUp = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LookupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}