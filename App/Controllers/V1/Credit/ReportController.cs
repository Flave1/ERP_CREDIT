using Banking.Contracts.Command.Reports;
using Banking.Contracts.V1;
using Banking.Repository.Interface.Credit;
using Finance.Contracts.Response.Reports;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Controllers.V1.Credit
{
    //[Authorize]
    public class ReportController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IReportService _reportService;
        private readonly ILoggerService _logger;
        public ReportController(IMediator mediator, IReportService reportService, ILoggerService logger)
        {
            _mediator = mediator;
            _reportService = reportService;
            _logger = logger;
        }
        
        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_OFFER_LETTER)]
        public async Task<IActionResult> LOAN_OFFER_LETTER([FromQuery] GenerateOfferLetterQuery query)
        {
            var response = await _mediator.Send(query); 
            return Ok(response);
        }


        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_OFFER_LETTER_FEE)]
        public async Task<ActionResult<ReportRespObj>> GetLoanApplicationFee(string ApplicationRef)
        {
            try
            {
                var response = _reportService.GetLoanApplicationFee(ApplicationRef);
                return new ReportRespObj
                {
                    ProductFees = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_OFFER_LETTER_SCHEDULE)]
        public async Task<ActionResult<ReportRespObj>> GetOfferLeterPeriodicSchedule(string ApplicationRef)
        {
            try
            {
                var response = _reportService.GetOfferLeterPeriodicSchedule(ApplicationRef);
                return new ReportRespObj
                {
                    OfferLetterRepayments = response,
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "Successful"} }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_OFFER_LETTER_LMS)]
        public async Task<ActionResult<ReportRespObj>> GenerateOfferLetterLMS(string ApplicationRef)
        {
            try
            {
                var response1 = _reportService.GenerateOfferLetterLMS(ApplicationRef);
                var response2 = _reportService.GetOfferLeterPeriodicScheduleLMS(ApplicationRef);
                return new ReportRespObj
                {
                    OfferLetterDetails = response1,
                    OfferLetterRepayments = response2,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_OFFER_LETTER_INVESTMENT)]
        public async Task<ActionResult<ReportRespObj>> GeneratePlacementCertificate(string ApplicationRef)
        {
            try
            {
                var response1 = _reportService.GenerateInvestmentCertificate(ApplicationRef);
                var response2 = _reportService.GetPeriodicScheduleInvestmentCertificate(ApplicationRef);
                return new ReportRespObj
                {
                    OfferLetterDetails = response1,
                    OfferLetterRepayments = response2,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_CORPORATE_CUSTOMER_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetCreditCustomerCorporate(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetCreditCustomerCorporate(model.date1, model.date2, model.customerTypeId);
                return new ReportRespObj
                {
                    CorporateCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_INDIVIDUAL_CUSTOMER_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetCreditCustomerIndividual(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetCreditCustomerIndividual(model.date1, model.date2, model.customerTypeId);
                return new ReportRespObj
                {
                    IndividualCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.LOAN_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetLoan(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetLoan(model.date1, model.date2);
                return new ReportRespObj
                {
                    Loans = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.INVESTMENT_CORPORATE_CUSTOMER_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetInvestmentCustomerCorporate(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetInvestmentCustomerCorporate(model.date1, model.date2, model.customerTypeId);
                return new ReportRespObj
                {
                    CorporateInvestorCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ReportEndpoints.INVESTMENT_INDIVIDUAL_CUSTOMER_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetInvestmentCustomerIndividual(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetInvestmentCustomerIndividual(model.date1, model.date2, model.customerTypeId);
                return new ReportRespObj
                {
                    IndividualInvestorCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.ReportEndpoints.INVESTMENT_REPORT)]
        public async Task<ActionResult<ReportRespObj>> GetInvestment(ReportSearchObj model)
        {
            try
            {
                var response = _reportService.GetInvestment(model.date1, model.date2);
                return new ReportRespObj
                {
                    Investments = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ReportRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
    }
}
