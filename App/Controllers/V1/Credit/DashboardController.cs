using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.V1;
using Banking.Data;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers.V1.Credit
{ 
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;

        public DashboardController(IDashboardRepository repo,
            IMapper mapper, IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerService logger, DataContext dataContext,
            IIdentityServerRequest serverRequest)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
            _serverRequest = serverRequest;
        }

        [HttpGet(ApiRoutes.Dashboard.GET_PERFORMANCE)]
        public async Task<ActionResult<DashboardRespObj>> GetPerformanceMatrics()
        {
            try
            {
                var response =  _repo.GetPerformanceMatrics();
                return new DashboardRespObj
                {
                    PerformanceMetric = response
                };
            }   
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_CUSTOMER_TRANSACTION)]
        public async Task<ActionResult<DashboardRespObj>> GetCustomerTransactionSummary([FromQuery] DashboardSearchObj model)
        {
            try
            {
                var response = _repo.GetCustomerTransactionSummary(model.AccountNumber);
                return new DashboardRespObj
                {
                    CustomerDashboard = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_LOANAPPLICATION_DETAILS)]
        public async Task<ActionResult<DashboardRespObj>> GetLoansApplicationDetails()
        {
            try
            {
                var response = _repo.GetLoansApplicationDetails();
                return new DashboardRespObj
                {
                    LoanApplicationDetail = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_CONCENTRATION_DETAILS)]
        public async Task<ActionResult<DashboardRespObj>> GetLoanConcentrationDetails()
        {
            try
            {
                var response = _repo.GetLoanConcentrationDetails();
                return new DashboardRespObj
                {
                    LoanConcentration = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_LOAN_PAR)]
        public async Task<ActionResult<DashboardRespObj>> GetPARForDashboard()
        {
            try
            {
                var response = _repo.GetPARForDashboard();
                return new DashboardRespObj
                {
                    Par = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        //[HttpGet(ApiRoutes.Dashboard.GET_LOANOVERDUE)]
        //public async Task<ActionResult<DashboardRespObj>> GetOverDueForDashboard()
        //{
        //    try
        //    {
        //        var response = _repo.GetOverDueForDashboard();
        //        return new DashboardRespObj
        //        {
        //            LoanOverDue = response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorCode = ErrorID.Generate(5);
        //        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new DashboardRespObj
        //        {
        //            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
        //        };
        //    }
        //}

        [HttpGet(ApiRoutes.Dashboard.GET_LOANSTAGING)]
        public async Task<ActionResult<DashboardRespObj>> LoanStaging()
        {
            try
            {
                var response = _repo.LoanStaging();
                return new DashboardRespObj
                {
                    CustomerDashboard = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_INVESTMENT_APP_DETAILS)]
        public async Task<ActionResult<DashboardRespObj>> GetInvestmentApplicationDetails()
        {
            try
            {
                var response = _repo.GetInvestmentApplicationDetails();
                return new DashboardRespObj
                {
                    InvestmentApplicationDetail = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_INVESTMENT_APP_CHARTS)]
        public async Task<ActionResult<DashboardRespObj>> GetInvestmentPerformanceChart()
        {
            try
            {
                var response = _repo.GetInvestmentPerformanceChart();
                return new DashboardRespObj
                {
                    InvestmentChart = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Dashboard.GET_INVESTMENT_CONCENTRATION)]
        public async Task<ActionResult<DashboardRespObj>> GetInvestmentConcentrationDetails()
        {
            try
            {
                var response = _repo.GetInvestmentConcentrationDetails();
                return new DashboardRespObj
                {
                    InvestmentConcentration = response
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DashboardRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}