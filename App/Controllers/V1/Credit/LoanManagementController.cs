using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanManagementController : Controller
    {
        private readonly ILoanManagementRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly ILoanScheduleRepository _schedule;

        public LoanManagementController(ILoanManagementRepository repo, IIdentityService identityService, ILoggerService logger, 
            IIdentityServerRequest serverRequest, IHttpContextAccessor httpContextAccessor, DataContext context, ILoanScheduleRepository schedule)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = context;
            _serverRequest = serverRequest;
            _schedule = schedule;
        }

        #region LOAN_MANAGEMENT
        [HttpPost(ApiRoutes.LoanManagement.GET_ALL_RUNNING_LOANS)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetAllRunningLoan([FromBody] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllRunningLoan(model.SearchString, model.CustomerTypeId, model.ProductTypeId);
                return new LoanReviewListObjRespObj
                {
                    LoanReviewList = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_ALL_RUNNING_LOANS_CUSTOMER_ID)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetAllRunningLoanByCustomerId([FromQuery] SearchQueryObj model)
        {
            try
            {
                var response = _repo.GetAllRunningLoanByCustomerId(model.CustomerId);
                return new LoanReviewListObjRespObj
                {
                    LoanReviewList = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_ALL_APPLICATION)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetAllLoanReviewApplication()
        {
            try
            {
                var response = _repo.GetAllLoanReviewApplication();
                return new LoanReviewListObjRespObj
                {
                    LoanReviewApplication = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_LOANS_REVIEW_APPLICATION_ID)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetSingleLoanReviewApplication([FromQuery] SearchQueryObj model)
        {
            try
            {
                var response = _repo.GetSingleLoanReviewApplication(model.LoanReviewApplicationId);
                var respList = new List<LoanReviewApplicationObj> { response };
                return new LoanReviewListObjRespObj
                {
                    LoanReviewApplication = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_LOANS_REVIEW_APPLICATION_LOAN_ID)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetLoanReviewApplicationbyLoanId([FromQuery] SearchQueryObj model)
        {
            try
            {
                var response = _repo.GetLoanReviewApplicationbyLoanId(model.LoanId);
                var respList = new List<LoanReviewApplicationObj> { response };
                return new LoanReviewListObjRespObj
                {
                    LoanReviewApplication = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_LOANS_REVIEW_APPLICATION)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetLoanReviewApplicationList()
        {
            try
            {
                var response = _repo.GetLoanReviewApplicationList();
                return new LoanReviewListObjRespObj
                {
                    LoanReviewApplication = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanManagement.ADD_LOAN_APPLICATION)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> AddLoanBooking([FromBody] LoanReviewApplicationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = await _repo.AddUpdateLMSApplication(model);
                return response;

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_ALL_LOAN_REVIEW_APPROVAL_LIST)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetLoanReviewAwaitingApproval()
        {
            try
            {
                var response = await _repo.GetLoanReviewAwaitingApproval();
                return response;
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanManagement.GO_FOR_LOAN_REVIEW_APPROVAL)]
        public async Task<ActionResult<ApprovalRegRespObj>> GoForApproval([FromBody] ApprovalObj entity)
        {
            try
            {

                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                        var user = await _serverRequest.UserDataAsync();

                        var loan = _dataContext.credit_loanreviewapplication.Find(entity.TargetId);


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
                            var appicationResponse = _repo.LoanReviewApplicationApproval(entity, user.StaffId, user.UserName);

                            if (appicationResponse.loanPayment != null && appicationResponse.AnyIdentifier > 0)
                            {
                                await _schedule.AddReviewedLoanSchedule(appicationResponse.AnyIdentifier, appicationResponse.loanPayment);
                            }
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Approved)
                        {
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
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpPost(ApiRoutes.LoanManagement.LOAN_REVIEW_APPROVAL_SCHEDULE)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> AddTempLoanBooking([FromBody] LoanObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = await _repo.AddTempLoanBooking(model);
                if (response)
                {
                    return new LoanReviewListObjRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record Saved Successful" } }
                    };
                }
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record Not Saved" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanManagement.LOAN_REVIEW_LOG)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> UpdateLoanReviewApplicationLog([FromBody] ApprovalRecommendationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.StaffName;

                var response = _repo.UpdateLoanReviewApplicationLog(model, user);
                return new LoanReviewListObjRespObj
                {
                    LoanApprovalRecommendationLog = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record Saved Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_LOAN_REVIEW_LOG)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetLoanReviewApplicationLog([FromQuery] SearchQueryObj model)
        {
            try
            {
                var response = _repo.GetLoanReviewApplicationLog(model.LoanReviewApplicationId);
                return new LoanReviewListObjRespObj
                {
                    LoanRecommendationLog = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanManagement.GET_LOAN_REVIEW_OFFER_LETTER)]
        public async Task<ActionResult<LoanReviewListObjRespObj>> GetAllLoanReviewApplicationOfferLetter()
        {
            try
            {
                var response = _repo.GetAllLoanReviewApplicationOfferLetter();
                return new LoanReviewListObjRespObj
                {
                    LoanReviewList = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanReviewListObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanManagement.OFFER_LETTER_UPLOAD)]
        public async Task<ActionResult<LoanApplicationRespObj>> UploadLoanReviewOfferLetter()
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

                var isDone = await _repo.UploadLoanReviewOfferLetter(model);
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
        #endregion
    }
}
