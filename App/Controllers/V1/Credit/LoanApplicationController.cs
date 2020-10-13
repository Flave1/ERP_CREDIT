using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.V1;
using Banking.Data;
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
using static Banking.Contracts.Response.Credit.ExposureObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class LoanApplicationController : Controller
    {
        private readonly ILoanApplicationRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IExposureParameter _exposure;
        private readonly DataContext _context;
        ICreditAppraisalRepository _customerTrans;
        public LoanApplicationController(ILoanApplicationRepository repo,
            IIdentityService identityService, 
            ILoggerService logger, 
            IHttpContextAccessor httpContextAccessor, 
            IExposureParameter exposure,
            ICreditAppraisalRepository customerTrans,
            DataContext context,
             IIdentityServerRequest serverRequest)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _exposure = exposure;
            _serverRequest = serverRequest;
            _customerTrans = customerTrans;
            _context = context;
        }

        #region LOAN_APPLICATION
        [HttpPost(ApiRoutes.LoanApplication.ADD_LOAN_APPLICATION)]
        public async Task<ActionResult<LoanApplicationRespObj>> UpdateLoanApplication([FromBody] LoanApplicationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                model.CompanyId = identity.CompanyId;

                return await _repo.UpdateLoanApplication(model);

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

        [HttpPost(ApiRoutes.LoanApplication.ADD_LOAN_APPLICATION_CUSTOMER)]
        public async Task<ActionResult<LoanApplicationRespObj>> UpdateLoanApplicationByCustomer([FromBody] LoanApplicationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.UpdateLoanApplicationByCustomer(model);
                //var loanList = new List<LoanApplicationObj> { isDone };
                if (isDone != null)
                {
                    return new LoanApplicationRespObj
                    {
                        ApplicationRefNumber = isDone,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved Successfully" } }
                    };
                }
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
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


        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetAllLoanApplication()
        {
            try
            {
                var response = _repo.GetAllLoanApplication();
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


        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_LIST)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationList()
        {
            try
            {
                var response = _repo.GetLoanApplicationList();
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_OFFERLETTER)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetAllLoanApplicationOfferLetter()
        {
            try
            {
                var response = _repo.GetAllLoanApplicationOfferLetter();
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_OFFERLETTER_REVIEW)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetAllLoanApplicationOfferLetterReview()
        {
            try
            {
                var response = _repo.GetAllLoanApplicationOfferLetterReview();
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_WEBSITE_LIST)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetWebsiteLoanApplicationList()
        {
            try
            {
                var response = _repo.GetWebsiteLoanApplicationList();
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_WEBSITE_ID)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetWebsiteLoanApplicationById([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetWebsiteLoanApplicationById(model.LoanApplicationId);
                var resplist = new List<LoanApplicationObj> { response };
                return new LoanApplicationRespObj
                {
                    LoanApplications = resplist,
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_PRODUCT_ID)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationIdByPID([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanApplicationIdByPID(model.CustomerTypeId, model.LoanApplicationId);
                var resplist = new List<LoanApplicationObj> { response };
                return new LoanApplicationRespObj
                {
                    LoanApplications = resplist,
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_REFERENCE)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationByRefNumber([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanApplicationByRefNumber(model.refNumber);
                //var resplist = new List<LoanApplicationObj> { response };
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

        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_LOAN_APPLICATION_CUSTOMER)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationByCustomer([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanApplicationByCustomer(model.CustomerId);
                //var resplist = new List<LoanApplicationObj> { response };
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


        [HttpGet(ApiRoutes.LoanApplication.GET_ALL_RUNNING_LOAN_APPLICATION_CUSTOMER)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetRunningLoanApplicationByCustomer([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetRunningLoanApplicationByCustomer(model.CustomerId);
                //var resplist = new List<LoanApplicationObj> { response };
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


        [HttpGet(ApiRoutes.LoanApplication.GET_LOAN_APPLICATION_BY_ID)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplication([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response =await  _repo.GetLoanApplication(model.LoanApplicationId);
                var resplist = new List<LoanApplicationObj> { response };
                return new LoanApplicationRespObj
                {
                    LoanApplications = resplist,
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

        [HttpGet(ApiRoutes.LoanApplication.SUBMIT_FOR_APPRAISAL)]
        public async Task<ActionResult<LoanApplicationRespObj>> SubmitLoanForCreditAppraisal([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = await _repo.SubmitLoanForCreditAppraisal(model.LoanApplicationId);
                if (response.Status.IsSuccessful)
                {
                    var companyList = await _serverRequest.GetAllCompanyAsync();
                    var loanObj = _context.credit_loanapplication.Find(model.LoanApplicationId);
                    var customerObj = _context.credit_loancustomer.Find(loanObj.CustomerId);
                    
                    var productFeeList = _context.credit_productfee.Where(x => x.ProductId == loanObj.ApprovedProductId && x.Deleted == false).ToList();
                    foreach(var item in productFeeList)
                    {
                        decimal productamount = 0;
                        var fee = _context.credit_fee.FirstOrDefault(x => x.FeeId == item.FeeId && x.IsIntegral == false && x.Deleted == false);
                        if (item.ProductFeeType == 2)//Percentage
                        {
                            productamount = (loanObj.ApprovedAmount * Convert.ToDecimal(item.ProductAmount)) / 100;
                        }
                        else///Fixed
                        {
                            productamount = Convert.ToDecimal(item.ProductAmount);
                        }
                        if (fee != null)
                        {
                            CustomerTransactionObj customerTrans = new CustomerTransactionObj
                            {
                                CasaAccountNumber = customerObj.CASAAccountNumber,
                                Description = "Payment of Non Integral Fee",
                                TransactionDate = DateTime.Now,
                                ValueDate = DateTime.Now,
                                TransactionType = "Debit",
                                CreditAmount = 0,
                                DebitAmount = productamount,
                                Beneficiary = companyList.companyStructures.FirstOrDefault(x => x.companyStructureId == loanObj.CompanyId)?.name,
                                ReferenceNo = loanObj.ApplicationRefNumber,
                            };
                            _customerTrans.CustomerTransaction(customerTrans);
                        }                         
                    }                  
                }
                return new LoanApplicationRespObj
                {     
                    ResponseId = response.ResponseId,              
                    Status = response.Status
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

        [HttpPost(ApiRoutes.LoanApplication.LOAN_APPLICATION_OFFERLETTER_UPLOAD)]
        public async Task<ActionResult<LoanApplicationRespObj>> UploadOfferLetter()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["supportDocument"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var loanApplicationId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["loanApplicationId"]);
                var reportStatus = Convert.ToString(_httpContextAccessor.HttpContext.Request.Form["reportStatus"]);

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = (ms.ToArray());
                        }
                    }
                }

                var model = new LoanApplicationObj
                {
                    LoanApplicationId = loanApplicationId,
                    ReportStatus = reportStatus,
                    SupportDocument = byteArray,
                    CreatedBy = createdBy
                };

                var isDone = await _repo.UploadOfferLetter(model);
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Record saved successfully" : "Record not saved" } }
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

        [HttpPost(ApiRoutes.LoanApplication.ADD_LOAN_APPLICATION_DECISION)]
        public async Task<ActionResult<LoanApplicationRespObj>> OfferLetterDecision([FromBody] LoanApplicationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = await _repo.OfferLetterDecision(model);
                //var loanList = new List<LoanApplicationObj> { isDone };
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Record saved successfully" : "Record not saved" } }
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


        [HttpGet(ApiRoutes.LoanApplication.GET_LOAN_APPLICATION_DECISION)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetOfferletterDecisionStatus([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var isDone = _repo.GetOfferletterDecisionStatus(model.LoanApplicationId);
                //var resplist = new List<LoanApplicationObj> { response };
                return new LoanApplicationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Accepted" : "Not Accepted" } }
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

        [HttpPost(ApiRoutes.LoanApplication.LOAN_APPLICATION_RECOMMENDATION)]
        public async Task<ActionResult<LoanApplicationRespObj>> OfferLetterDecision([FromBody] ApprovalRecommendationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.StaffName;

                var response = _repo.UpdateLoanApplicationRecommendation(model, user);
                var repList = new List<ApprovalRecommendationObj> { response };
                return new LoanApplicationRespObj
                {
                    ApprovalRecommendations = repList,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage =  "Record saved successfully" } }
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

        [HttpGet(ApiRoutes.LoanApplication.GET_LOAN_APPLICATION_RECOMMENDATION)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanRecommendationLog([FromQuery] LoanApplicationSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanRecommendationLog(model.LoanApplicationId);
                //var resplist = new List<LoanApplicationObj> { response };
                return new LoanApplicationRespObj
                {
                    LoanRecommendationLogs = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
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

        [HttpPost(ApiRoutes.LoanApplication.DELETE_LOAN_APPLICATION)]
        public ActionResult<LoanApplicationRespObj> DeleteLoanApplication([FromBody] DeleteLoanApplicationCommand command)
        {
            var response = false;
            var Ids = command.LoanApplicationIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanApplication(id);
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

        #region CREDIT_EXPOSURE_LIMIT
        [HttpGet(ApiRoutes.LoanApplication.GET_LOAN_APPLICATION_EXPOSURE)]
        public async Task<ActionResult<ExposureRespObj>> GetExposureParameter()
        {
            try
            {
                var response = _exposure.GetExposureParameter();
                return new ExposureRespObj
                {
                    Exposure = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ExposureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 86)]
        [HttpGet(ApiRoutes.LoanApplication.GET_LOAN_APPLICATION_EXPOSURE_ID)]
        public async Task<ActionResult<ExposureRespObj>> GetExposureParameterByID(int ExposureParamaterId)
        {
            try
            {
                var response = _exposure.GetExposureParameterByID(ExposureParamaterId);
                var resplist = new List<ExposureObj> { response };
                return new ExposureRespObj
                {
                    Exposure = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ExposureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 86)]
        [HttpPost(ApiRoutes.LoanApplication.ADD_LOAN_APPLICATION_EXPOSURE)]
        public async Task<ActionResult<ExposureRespObj>> AddUpdateCreditCategory([FromBody] ExposureObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;

                var isDone = await _exposure.AddUpdateExposureParameter(model);

                return new ExposureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ExposureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        #endregion
    }
}