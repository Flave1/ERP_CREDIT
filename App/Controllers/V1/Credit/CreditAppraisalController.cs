using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.FlutterWave;
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

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class CreditAppraisalController : Controller
    {
        private readonly ICreditAppraisalRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly ILoanScheduleRepository _schedule;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IFlutterWaveRequest _flutter;

        public CreditAppraisalController(ICreditAppraisalRepository repo, IIdentityServerRequest serverRequest,
            IIdentityService identityService, ILoanScheduleRepository loanScheduleRepository,
            ILoggerService logger, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IFlutterWaveRequest flutter)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            _httpContextAccessor = httpContextAccessor;
            _schedule = loanScheduleRepository;
            _flutter = flutter;
        }

        #region CREDIT_APPRAISAL
        [HttpGet(ApiRoutes.CreditAppraisal.GET_AWAITING_APPROVAL_LIST)]
        public async Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationForAppraisalAsync()
        {
            try
            {
                return await _repo.GetLoanApplicationForAppraisalAsync();
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

        [HttpPost(ApiRoutes.CreditAppraisal.GO_FOR_APPROVAL)]
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

                        var loanApplication = _dataContext.credit_loanapplication.Find(entity.TargetId);


                        var req = new IndentityServerApprovalCommand
                        {
                            ApprovalComment = entity.Comment,
                            ApprovalStatus = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            WorkflowToken = loanApplication.WorkflowToken,
                            ReferredStaffId = entity.ReferredStaffId
                        };

                        var previousDetails = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(loanApplication.WorkflowToken) && x.TargetId == entity.TargetId).ToList();
                        var lastDate = loanApplication.CreatedOn;
                        if (previousDetails.Count() > 0)
                        {
                            lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                        }
                        var details = new cor_approvaldetail
                        {
                            Comment = entity.Comment,
                            Date = DateTime.Now,
                            ArrivalDate = previousDetails.Count() > 0 ? lastDate : loanApplication.CreatedOn,
                            StatusId = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            StaffId = user.StaffId,
                            WorkflowToken = loanApplication.WorkflowToken
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
                            loanApplication.ApprovalStatusId = (int)ApprovalStatus.Processing;
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
                            loanApplication.ApprovalStatusId = (int)ApprovalStatus.Revert;
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
                            loanApplication.ApprovalStatusId = (int)ApprovalStatus.Approved;
                            var appicationResponse = _repo.LoanApplicationApproval(entity.TargetId, (short)entity.ApprovalStatusId);

                            if (appicationResponse.loanPayment != null && appicationResponse.AnyIdentifier > 0)
                            {
                                await _schedule.AddTempLoanApplicationSchedule(appicationResponse.AnyIdentifier, appicationResponse.loanPayment);
                            }
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
                            loanApplication.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
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

        [HttpGet(ApiRoutes.CreditAppraisal.GET_APPROVAL_COMMENTS)]
        public async Task<ActionResult<ApprovalDetailsRespObj>> GetApprovalTrail([FromQuery] ApprovalDetailSearchObj model)
        {
            try
            {
                return new ApprovalDetailsRespObj
                {
                    Details = _repo.GetApprovalTrail(model.TargetId, model.WorkflowToken),
                    PreviousStaff =_repo.GetApprovalTrailStaff(model.TargetId, model.WorkflowToken)
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalDetailsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region CUSTOMER_TRANSACTION
        [HttpPost(ApiRoutes.CreditAppraisal.UPDATE_CUSTOMER_TRANSACTION)]
        public async Task<ApprovalRegRespObj> CustomerTransaction([FromBody] CustomerTransactionObj entity)
        {
            try
            {
                return _repo.CustomerTransaction(entity);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                }; throw;
            }          
        }

        [HttpPost(ApiRoutes.CreditAppraisal.CUSTOMER_TRANSACTION_SEARCH)]
        public async Task<ActionResult<CustomerTransactionRespObj>> GetAllCustomerTransactionBySearch([FromBody] CustomerTransactionSearchObj model)
        {
            try
            {
                var response = _repo.GetAllCustomerTransactionBySearch(model.AccountNumber, model.Date1, model.Date2);
                return new CustomerTransactionRespObj
                {
                    customerTransaction = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerTransactionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditAppraisal.CUSTOMER_TRANSACTION_SEARCH_EXPORT)]
        public async Task<ActionResult<CustomerTransactionRespObj>> GenerateExportCustomerTransaction([FromBody] CustomerTransactionSearchObj model)
        {
            try
            {
                var response = _repo.GenerateExportCustomerTransaction(model.AccountNumber, model.Date1, model.Date2);
                return new CustomerTransactionRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerTransactionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditAppraisal.GET_ALL_FLUTTERWAVE_TRANSACTIONS)]
        public async Task<ActionResult<GetTransferRespObj>> getAllTransfer()
        {
            try
            {
                var response = _flutter.getAllTransfer().Result;
                return new GetTransferRespObj
                {
                   data  = response.data,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new GetTransferRespObj
                {
                   status = "Unsuccessful", message = ex.Message
                };
            }
        }

        [HttpGet(ApiRoutes.CreditAppraisal.VERIFY_FLUTTERWAVE_TRANSACTIONS)]
        public async Task<ActionResult<TransactionVerificationRespObj>> transactionVerification(string Id)
        {
            try
            {
                var url = "transactions/"+ Id + "/verify";
                var response = _flutter.transactionVerification(url).Result;
                return new TransactionVerificationRespObj
                {
                    status= response.status,
                    message=response.message,
                    data = response.data,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransactionVerificationRespObj
                {
                    status = "Unsuccessful",
                    message = ex.Message
                };
            }
        }
        #endregion

    }
}